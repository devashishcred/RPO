// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-24-2018
// ***********************************************************************
// <copyright file="JobWorkTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job WorkTypes Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.JobWorkTypes
{
    using System;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Job WorkTypes Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobWorkTypesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();


        /// <summary>
        /// Gets the job Wrok Types.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the job Wrok Types List.</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobWorkTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var jobWorkTypes = this.rpoContext.JobWorkTypes.Include("JobApplicationType.Parent").Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = jobWorkTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = jobWorkTypes
                    .AsEnumerable()
                    .Select(c => this.Format(c))
                    .AsQueryable()
                    .DataTableParameters(dataTableParameters, out recordsFiltered)
                    .ToArray();

                return this.Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result.OrderByDescending(x => x.LastModifiedDate)
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the type of the job work.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the type of the job work in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobWorkTypeDetail))]
        public IHttpActionResult GetJobWorkType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                JobWorkType jobWorkType = this.rpoContext.JobWorkTypes.Include("JobApplicationType.Parent").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (jobWorkType == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(jobWorkType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the type of the job work.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the type of the job work for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobworktypes/dropdown")]
        public IHttpActionResult GetJobWorkTypeDropdown()
        {
            var result = this.rpoContext.JobWorkTypes.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Description,
                Description = c.Description,
                Code = c.Code
            }).ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Puts the type of the job work.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the type of the job work for dropdown.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobworktypes/dropdown/{id}")]
        public IHttpActionResult GetWorkTypeDropdown(int id)
        {
            var result = this.rpoContext.JobWorkTypes.Where(x => x.IdJobApplicationType == id).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Description,
                Description = c.Description,
                Code = c.Code
            }).ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Puts the type of the job work.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="jobWorkType">Type of the job work.</param>
        /// <returns>update the detail of job work type.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJobWorkType(int id, JobWorkTypeCreateUpdate jobWorkTypeCreateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != jobWorkTypeCreateUpdate.Id)
                {
                    return BadRequest();
                }

                if (this.JobWorkTypeNameExists(jobWorkTypeCreateUpdate.Description, jobWorkTypeCreateUpdate.Id, jobWorkTypeCreateUpdate.IdJobApplicationType))
                {
                    throw new RpoBusinessException(StaticMessages.JobWorkTypeNameExistsMessage);
                }

                if (this.JobWorkTypeCodeExists(jobWorkTypeCreateUpdate.Code, jobWorkTypeCreateUpdate.Id, jobWorkTypeCreateUpdate.IdJobApplicationType))
                {
                    throw new RpoBusinessException(StaticMessages.JobWorkTypeCodeExistsMessage);
                }

                JobWorkType jobWorkType = rpoContext.JobWorkTypes.FirstOrDefault(x => x.Id == id);
                jobWorkType.Description = jobWorkTypeCreateUpdate.Description;
                jobWorkType.Content = jobWorkTypeCreateUpdate.Content;
                jobWorkType.Code = jobWorkTypeCreateUpdate.Code;
                jobWorkType.IdJobApplicationType = jobWorkTypeCreateUpdate.IdJobApplicationType;
                jobWorkType.Cost = jobWorkTypeCreateUpdate.Cost;

                jobWorkType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobWorkType.LastModifiedBy = employee.Id;
                }

                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobWorkTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                JobWorkType jobWorkTypeResponse = this.rpoContext.JobWorkTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(jobWorkTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the job work.
        /// </summary>
        /// <param name="jobWorkType">Type of the job work.</param>
        /// <returns>create a new job work type.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobWorkType))]
        public IHttpActionResult PostJobWorkType(JobWorkTypeCreateUpdate jobWorkTypeCreateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (this.JobWorkTypeNameExists(jobWorkTypeCreateUpdate.Description, jobWorkTypeCreateUpdate.Id, jobWorkTypeCreateUpdate.IdJobApplicationType))
                {
                    throw new RpoBusinessException(StaticMessages.JobWorkTypeNameExistsMessage);
                }

                if (this.JobWorkTypeCodeExists(jobWorkTypeCreateUpdate.Code, jobWorkTypeCreateUpdate.Id, jobWorkTypeCreateUpdate.IdJobApplicationType))
                {
                    throw new RpoBusinessException(StaticMessages.JobWorkTypeCodeExistsMessage);
                }

                JobWorkType jobWorkType = new JobWorkType();
                jobWorkType.Description = jobWorkTypeCreateUpdate.Description;
                jobWorkType.Content = jobWorkTypeCreateUpdate.Content;
                jobWorkType.Code = jobWorkTypeCreateUpdate.Code;
                jobWorkType.IdJobApplicationType = jobWorkTypeCreateUpdate.IdJobApplicationType;
                jobWorkType.Cost = jobWorkTypeCreateUpdate.Cost;

                jobWorkType.LastModifiedDate = DateTime.UtcNow;
                jobWorkType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobWorkType.CreatedBy = employee.Id;
                    jobWorkType.LastModifiedBy = employee.Id;
                }

                this.rpoContext.JobWorkTypes.Add(jobWorkType);
                this.rpoContext.SaveChanges();

                JobWorkType jobWorkTypeResponse = this.rpoContext.JobWorkTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == jobWorkType.Id);
                return this.Ok(this.FormatDetails(jobWorkTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the job work.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the job work type.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobWorkType))]
        public IHttpActionResult DeleteJobWorkType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                JobWorkType jobWorkType = this.rpoContext.JobWorkTypes.Find(id);
                if (jobWorkType == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.JobWorkTypes.Remove(jobWorkType);
                this.rpoContext.SaveChanges();

                return this.Ok(jobWorkType);
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
                this.rpoContext.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Jobs the work type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobWorkTypeExists(int id)
        {
            return rpoContext.JobWorkTypes.Count(e => e.Id == id) > 0;
        }

        private bool JobWorkTypeNameExists(string name, int id, int idJobApplicationType)
        {
            return this.rpoContext.JobWorkTypes.Count(e => e.Description == name && e.Id != id && e.IdJobApplicationType == idJobApplicationType) > 0;
        }

        private bool JobWorkTypeCodeExists(string code, int id, int idJobApplicationType)
        {
            return this.rpoContext.JobWorkTypes.Count(e => e.Code == code && e.Id != id && e.IdJobApplicationType == idJobApplicationType) > 0;
        }

        private JobWorkTypeDTO Format(JobWorkType jobWorkType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobWorkTypeDTO
            {
                Id = jobWorkType.Id,
                Description = jobWorkType.Description,
                Code = jobWorkType.Code,
                Cost = jobWorkType.Cost,
                IdJobApplicationType = jobWorkType.IdJobApplicationType,
                JobApplicationType = jobWorkType.JobApplicationType != null ? jobWorkType.JobApplicationType.Description : string.Empty,
                IdJobType = jobWorkType.JobApplicationType != null ? jobWorkType.JobApplicationType.IdParent : null,
                JobType = jobWorkType.JobApplicationType != null && jobWorkType.JobApplicationType.Parent != null ? jobWorkType.JobApplicationType.Parent.Description : string.Empty,
                Content = jobWorkType.Content,
                CreatedBy = jobWorkType.CreatedBy,
                LastModifiedBy = jobWorkType.LastModifiedBy != null ? jobWorkType.LastModifiedBy : jobWorkType.CreatedBy,
                CreatedByEmployeeName = jobWorkType.CreatedByEmployee != null ? jobWorkType.CreatedByEmployee.FirstName + " " + jobWorkType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobWorkType.LastModifiedByEmployee != null ? jobWorkType.LastModifiedByEmployee.FirstName + " " + jobWorkType.LastModifiedByEmployee.LastName : (jobWorkType.CreatedByEmployee != null ? jobWorkType.CreatedByEmployee.FirstName + " " + jobWorkType.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobWorkType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobWorkType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobWorkType.CreatedDate,
                LastModifiedDate = jobWorkType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobWorkType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobWorkType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobWorkType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobWorkType.CreatedDate),
            };
        }

        private JobWorkTypeDetail FormatDetails(JobWorkType jobWorkType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobWorkTypeDetail
            {
                Id = jobWorkType.Id,
                Description = jobWorkType.Description,
                Code = jobWorkType.Code,
                Cost = jobWorkType.Cost,
                IdJobApplicationType = jobWorkType.IdJobApplicationType,
                JobApplicationType = jobWorkType.JobApplicationType != null ? jobWorkType.JobApplicationType.Description : string.Empty,
                IdJobType = jobWorkType.JobApplicationType != null ? jobWorkType.JobApplicationType.IdParent : null,
                JobType = jobWorkType.JobApplicationType != null && jobWorkType.JobApplicationType.Parent != null ? jobWorkType.JobApplicationType.Parent.Description : string.Empty,
                Content = jobWorkType.Content,
                CreatedBy = jobWorkType.CreatedBy,
                LastModifiedBy = jobWorkType.LastModifiedBy != null ? jobWorkType.LastModifiedBy : jobWorkType.CreatedBy,
                CreatedByEmployeeName = jobWorkType.CreatedByEmployee != null ? jobWorkType.CreatedByEmployee.FirstName + " " + jobWorkType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobWorkType.LastModifiedByEmployee != null ? jobWorkType.LastModifiedByEmployee.FirstName + " " + jobWorkType.LastModifiedByEmployee.LastName : (jobWorkType.CreatedByEmployee != null ? jobWorkType.CreatedByEmployee.FirstName + " " + jobWorkType.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobWorkType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobWorkType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobWorkType.CreatedDate,
                LastModifiedDate = jobWorkType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobWorkType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobWorkType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobWorkType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobWorkType.CreatedDate)
            };
        }
    }
}