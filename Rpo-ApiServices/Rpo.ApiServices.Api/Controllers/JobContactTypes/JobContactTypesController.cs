// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-22-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-21-2018
// ***********************************************************************
// <copyright file="JobContactTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Contact Types Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.JobContactTypes
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Job Contact Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobContactTypesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the job contact types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns> Gets the job contact types List.</returns>
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetJobContactTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
            //      || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))

                if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob)
                && Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob))
                {
                var jobContactType = db.JobContactTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = jobContactType.Count();
                var recordsFiltered = recordsTotal;

                var result = jobContactType
                    .AsEnumerable()
                    .Select(c => Format(c))
                    .AsQueryable()
                    .DataTableParameters(dataTableParameters, out recordsFiltered)
                    .ToArray();

                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the job contact type dropdown.
        /// </summary>
        /// <returns> Gets the job contact type for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobcontacttypes/dropdown")]
        public IHttpActionResult GetJobContactTypeDropdown()
        {
            var result = db.JobContactTypes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the type of the job contact.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Gets the detail of job contact types.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobContactTypeDetail))]
        public IHttpActionResult GetJobContactType(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                JobContactType jobContactType = db.JobContactTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (jobContactType == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(jobContactType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the type of the job contact.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="jobContactType">Type of the job contact.</param>
        /// <returns>update the detail of job contact Type.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobContactTypeDetail))]
        public IHttpActionResult PutJobContactType(int id, JobContactType jobContactType)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != jobContactType.Id)
                {
                    return BadRequest();
                }

                if (JobContactTypeNameExists(jobContactType.Name, jobContactType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.JobContactTypeNameExistsMessage);
                }
                jobContactType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobContactType.LastModifiedBy = employee.Id;
                }

                db.Entry(jobContactType).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobContactTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                JobContactType jobContactTypeResponse = db.JobContactTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return Ok(FormatDetails(jobContactTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the job contact.
        /// </summary>
        /// <param name="jobContactType">Type of the job contact.</param>
        /// <returns>create a new job contact type..</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobContactTypeDetail))]
        public IHttpActionResult PostJobContactType(JobContactType jobContactType)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (JobContactTypeNameExists(jobContactType.Name, jobContactType.Id))
                {
                    throw new RpoBusinessException(StaticMessages.JobContactTypeNameExistsMessage);
                }

                jobContactType.LastModifiedDate = DateTime.UtcNow;
                jobContactType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobContactType.CreatedBy = employee.Id;
                }

                db.JobContactTypes.Add(jobContactType);
                db.SaveChanges();

                JobContactType jobContactTypeResponse = db.JobContactTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == jobContactType.Id);
                return Ok(FormatDetails(jobContactTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the job contact.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the job contact types.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobContactType))]
        public IHttpActionResult DeleteJobContactType(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                JobContactType jobContactType = db.JobContactTypes.Find(id);
                if (jobContactType == null)
                {
                    return this.NotFound();
                }

                db.JobContactTypes.Remove(jobContactType);
                db.SaveChanges();

                return Ok(jobContactType);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Jobs the contact type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobContactTypeExists(int id)
        {
            return db.JobContactTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified job contact type.
        /// </summary>
        /// <param name="jobContactType">Type of the job contact.</param>
        /// <returns>JobContactTypeDTO.</returns>
        private JobContactTypeDTO Format(JobContactType jobContactType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobContactTypeDTO
            {
                Id = jobContactType.Id,
                Name = jobContactType.Name,
                CreatedBy = jobContactType.CreatedBy,
                LastModifiedBy = jobContactType.LastModifiedBy,
                CreatedByEmployeeName = jobContactType.CreatedByEmployee != null ? jobContactType.CreatedByEmployee.FirstName + " " + jobContactType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobContactType.LastModifiedByEmployee != null ? jobContactType.LastModifiedByEmployee.FirstName + " " + jobContactType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = jobContactType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContactType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContactType.CreatedDate,
                LastModifiedDate = jobContactType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContactType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContactType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="jobContactType">Type of the job contact.</param>
        /// <returns>JobContactTypeDetail.</returns>
        private JobContactTypeDetail FormatDetails(JobContactType jobContactType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobContactTypeDetail
            {
                Id = jobContactType.Id,
                Name = jobContactType.Name,
                CreatedBy = jobContactType.CreatedBy,
                LastModifiedBy = jobContactType.LastModifiedBy,
                CreatedByEmployeeName = jobContactType.CreatedByEmployee != null ? jobContactType.CreatedByEmployee.FirstName + " " + jobContactType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobContactType.LastModifiedByEmployee != null ? jobContactType.LastModifiedByEmployee.FirstName + " " + jobContactType.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = jobContactType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContactType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContactType.CreatedDate,
                LastModifiedDate = jobContactType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobContactType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobContactType.LastModifiedDate,
            };
        }

        /// <summary>
        /// Jobs the contact type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobContactTypeNameExists(string name, int id)
        {
            return db.JobContactTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}