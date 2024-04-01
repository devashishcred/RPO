// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="JobApplicationTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Application Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Job Application Types namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobApplicationTypes
{
    using System;
    using System.Collections.Generic;
    using System.Data;
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
    /// Class Job Application Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobApplicationTypesController : ApiController
    {

        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the job application types.
        /// </summary>
        /// <returns>IEnumerable&lt;JobApplicationType&gt;.</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobApplicationTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                var jobApplicationTypes = this.rpoContext.JobApplicationTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = jobApplicationTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = jobApplicationTypes
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
        /// Gets the type of the job application.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplicationType))]
        public IHttpActionResult GetJobApplicationType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                JobApplicationType jobApplicationType = this.rpoContext.JobApplicationTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (jobApplicationType == null)
                {
                    return this.NotFound();
                }

                return this.Ok(this.FormatDetails(jobApplicationType));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the type of the job application work.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Route("api/JobApplicationTypes/{id}/workTypes")]
        [ResponseType(typeof(JobWorkType))]
        public IHttpActionResult GetJobApplicationWorkType(int id)
        {
            ICollection<JobWorkType> jobWorkType = rpoContext.JobWorkTypes.Where(x => x.IdJobApplicationType == id).ToList();
            if (jobWorkType == null)
            {
                return this.NotFound();
            }
            return Ok(jobWorkType);
        }

        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobapplicationtypes/dropdown/{idParent}")]
        public IHttpActionResult GetJobApplicationTypeDropdown(int? idParent)
        {
            var result = this.rpoContext.JobApplicationTypes.Where(x => x.IdParent == idParent).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Description,
                Description = c.Description,
                IdParent = c.IdParent,
            }).ToArray();

            return this.Ok(result);
        }

        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/jobapplicationtypes/Alldropdown")]
        public IHttpActionResult GetJobApplicationTypeallDropdown()
        {
            var result = this.rpoContext.JobApplicationTypes.Where(x => x.IdParent != null).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Description,
                Description = c.Description,
                IdParent = c.IdParent,
            }).ToArray();

            return this.Ok(result);
        }


        /// <summary>
        /// Puts the type of the job application.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="jobApplicationTypeCreateUpdate">Type of the job application.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJobApplicationType(int id, JobApplicationTypeCreateUpdate jobApplicationTypeCreateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != jobApplicationTypeCreateUpdate.Id)
                {
                    return BadRequest();
                }

                JobApplicationType jobApplicationType = rpoContext.JobApplicationTypes.Find(id);

                jobApplicationType.Content = jobApplicationTypeCreateUpdate.Content;
                jobApplicationType.Description = jobApplicationTypeCreateUpdate.Description;
                jobApplicationType.IdParent = jobApplicationTypeCreateUpdate.IdParent;

                jobApplicationType.LastModifiedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobApplicationType.LastModifiedBy = employee.Id;
                }

                try
                {
                    rpoContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobApplicationTypeExists(id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                JobApplicationType jobApplicationTypeResponse = this.rpoContext.JobApplicationTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
                return this.Ok(this.FormatDetails(jobApplicationTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the type of the job application.
        /// </summary>
        /// <param name="jobApplicationType">Type of the job application.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplicationType))]
        public IHttpActionResult PostJobApplicationType(JobApplicationTypeCreateUpdate jobApplicationTypeCreateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                JobApplicationType jobApplicationType = new JobApplicationType();

                jobApplicationType.Content = jobApplicationTypeCreateUpdate.Content;
                jobApplicationType.Description = jobApplicationTypeCreateUpdate.Description;
                jobApplicationType.IdParent = jobApplicationTypeCreateUpdate.IdParent;

                jobApplicationType.LastModifiedDate = DateTime.UtcNow;
                jobApplicationType.CreatedDate = DateTime.UtcNow;
                if (employee != null)
                {
                    jobApplicationType.LastModifiedBy = employee.Id;
                    jobApplicationType.CreatedBy = employee.Id;
                }

                rpoContext.JobApplicationTypes.Add(jobApplicationType);
                rpoContext.SaveChanges();

                JobApplicationType jobApplicationTypeResponse = this.rpoContext.JobApplicationTypes.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == jobApplicationType.Id);
                return this.Ok(this.FormatDetails(jobApplicationTypeResponse));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the type of the job application.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobApplicationType))]
        public IHttpActionResult DeleteJobApplicationType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData))
            {
                JobApplicationType jobApplicationType = this.rpoContext.JobApplicationTypes.Find(id);
                if (jobApplicationType == null)
                {
                    return this.NotFound();
                }

                this.rpoContext.JobApplicationTypes.Remove(jobApplicationType);
                this.rpoContext.SaveChanges();

                return this.Ok(jobApplicationType);
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
                rpoContext.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Jobs the application type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobApplicationTypeExists(int id)
        {
            return rpoContext.JobApplicationTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Jobs the application type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="idParent">The identifier parent.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobApplicationTypeNameExists(string name, int id, int idParent)
        {
            return this.rpoContext.JobApplicationTypes.Count(e => e.Description == name && e.Id != id && e.IdParent == idParent) > 0;
        }

        /// <summary>
        /// Formats the specified job application type.
        /// </summary>
        /// <param name="jobApplicationType">Type of the job application.</param>
        /// <returns>JobApplicationTypeDTO.</returns>
        private JobApplicationTypeDTO Format(JobApplicationType jobApplicationType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobApplicationTypeDTO
            {
                Id = jobApplicationType.Id,
                Description = jobApplicationType.Description,
                IdParent = jobApplicationType.IdParent,
                Parent = jobApplicationType.Parent != null ? jobApplicationType.Parent.Description : string.Empty,
                Content = jobApplicationType.Content,
                CreatedBy = jobApplicationType.CreatedBy,
                LastModifiedBy = jobApplicationType.LastModifiedBy != null ? jobApplicationType.LastModifiedBy : jobApplicationType.CreatedBy,
                CreatedByEmployeeName = jobApplicationType.CreatedByEmployee != null ? jobApplicationType.CreatedByEmployee.FirstName + " " + jobApplicationType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobApplicationType.LastModifiedByEmployee != null ? jobApplicationType.LastModifiedByEmployee.FirstName + " " + jobApplicationType.LastModifiedByEmployee.LastName : (jobApplicationType.CreatedByEmployee != null ? jobApplicationType.CreatedByEmployee.FirstName + " " + jobApplicationType.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobApplicationType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationType.CreatedDate,
                LastModifiedDate = jobApplicationType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobApplicationType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationType.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="jobApplicationType">Type of the job application.</param>
        /// <returns>JobApplicationTypeDetail.</returns>
        private JobApplicationTypeDetail FormatDetails(JobApplicationType jobApplicationType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobApplicationTypeDetail
            {
                Id = jobApplicationType.Id,
                Description = jobApplicationType.Description,
                IdParent = jobApplicationType.IdParent,
                Parent = jobApplicationType.Parent != null ? jobApplicationType.Parent.Description : string.Empty,
                Content = jobApplicationType.Content,
                CreatedBy = jobApplicationType.CreatedBy,
                LastModifiedBy = jobApplicationType.LastModifiedBy != null ? jobApplicationType.LastModifiedBy : jobApplicationType.CreatedBy,
                CreatedByEmployeeName = jobApplicationType.CreatedByEmployee != null ? jobApplicationType.CreatedByEmployee.FirstName + " " + jobApplicationType.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobApplicationType.LastModifiedByEmployee != null ? jobApplicationType.LastModifiedByEmployee.FirstName + " " + jobApplicationType.LastModifiedByEmployee.LastName : (jobApplicationType.CreatedByEmployee != null ? jobApplicationType.CreatedByEmployee.FirstName + " " + jobApplicationType.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobApplicationType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationType.CreatedDate,
                LastModifiedDate = jobApplicationType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobApplicationType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationType.CreatedDate)
            };
        }
    }
}