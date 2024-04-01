// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-19-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-24-2018
// ***********************************************************************
// <copyright file="JobTimeNoteCategoryController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Time Note Categories Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Job Time Note Categorys namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobTimeNoteCategorys
{
    using System;
    using System.Data;
    using System.Data.Entity;
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
    /// Class Job Time Note Categories Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobTimeNoteCategoriesController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the job time note categories.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetJobTimeNoteCategories([FromUri] DataTableParameters dataTableParameters)
        {
            var JobTimeNoteCategories = db.JobTimeNoteCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

            var recordsTotal = JobTimeNoteCategories.Count();
            var recordsFiltered = recordsTotal;

            var result = JobTimeNoteCategories
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

        /// <summary>
        /// Gets the job time note category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobTimeNoteCategoryDetail))]
        public IHttpActionResult GetJobTimeNoteCategory(int id)
        {
            JobTimeNoteCategory jobTimeNoteCategory = db.JobTimeNoteCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
            if (jobTimeNoteCategory == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(jobTimeNoteCategory));
        }

        /// <summary>
        /// Gets the address type dropdown.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobtimenotecategories/dropdown")]
        public IHttpActionResult GetAddressTypeDropdown()
        {
            var result = db.JobTimeNoteCategories.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Puts the job time note category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="jobTimeNoteCategory">The job time note category.</param>
        /// <returns>IHttpActionResult.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobTimeNoteCategoryDetail))]
        public IHttpActionResult PutJobTimeNoteCategory(int id, JobTimeNoteCategory jobTimeNoteCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jobTimeNoteCategory.Id)
            {
                return BadRequest();
            }
            if (JobTimeNoteCategoryNameExists(jobTimeNoteCategory.Name, jobTimeNoteCategory.Id))
            {
                throw new RpoBusinessException(StaticMessages.JobTimeNoteCategoryNameExistsMessage);
            }

            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            jobTimeNoteCategory.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                jobTimeNoteCategory.LastModifiedBy = employee.Id;
            }

            db.Entry(jobTimeNoteCategory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTimeNoteCategoryExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            JobTimeNoteCategory jobTimeNoteCategoryResponse = db.JobTimeNoteCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
            return Ok(FormatDetails(jobTimeNoteCategoryResponse));
        }

        /// <summary>
        /// Posts the type of the job work.
        /// </summary>
        /// <param name="jobTimeNoteCategory">The job time note category.</param>
        /// <returns>Posts the type of the job work.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobTimeNoteCategoryDetail))]
        public IHttpActionResult PostJobWorkType(JobTimeNoteCategory jobTimeNoteCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (JobTimeNoteCategoryNameExists(jobTimeNoteCategory.Name, jobTimeNoteCategory.Id))
            {
                throw new RpoBusinessException(StaticMessages.JobTimeNoteCategoryNameExistsMessage);
            }

            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            jobTimeNoteCategory.LastModifiedDate = DateTime.UtcNow;
            jobTimeNoteCategory.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                jobTimeNoteCategory.CreatedBy = employee.Id;
            }

            db.JobTimeNoteCategories.Add(jobTimeNoteCategory);
            db.SaveChanges();

            JobTimeNoteCategory jobTimeNoteCategoryResponse = db.JobTimeNoteCategories.Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == jobTimeNoteCategory.Id);
            return Ok(FormatDetails(jobTimeNoteCategoryResponse));
        }

        /// <summary>
        /// Deletes the job time note category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [ResponseType(typeof(JobTimeNoteCategory))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult DeleteJobTimeNoteCategory(int id)
        {
            JobTimeNoteCategory jobTimeNoteCategory = db.JobTimeNoteCategories.Find(id);
            if (jobTimeNoteCategory == null)
            {
                return this.NotFound();
            }

            db.JobTimeNoteCategories.Remove(jobTimeNoteCategory);
            db.SaveChanges();

            return Ok(jobTimeNoteCategory);
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
        /// Jobs the time note category exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobTimeNoteCategoryExists(int id)
        {
            return db.JobTimeNoteCategories.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified job time note category.
        /// </summary>
        /// <param name="jobTimeNoteCategory">The job time note category.</param>
        /// <returns>JobTimeNoteCategoryDTO.</returns>
        private JobTimeNoteCategoryDTO Format(JobTimeNoteCategory jobTimeNoteCategory)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobTimeNoteCategoryDTO
            {
                Id = jobTimeNoteCategory.Id,
                Name = jobTimeNoteCategory.Name,
                CreatedByEmployeeName = jobTimeNoteCategory.CreatedByEmployee != null ? jobTimeNoteCategory.CreatedByEmployee.FirstName + " " + jobTimeNoteCategory.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobTimeNoteCategory.LastModifiedByEmployee != null ? jobTimeNoteCategory.LastModifiedByEmployee.FirstName + " " + jobTimeNoteCategory.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = jobTimeNoteCategory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNoteCategory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNoteCategory.CreatedDate,
                LastModifiedDate = jobTimeNoteCategory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNoteCategory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNoteCategory.LastModifiedDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="jobTimeNoteCategory">The job time note category.</param>
        /// <returns>JobTimeNoteCategoryDetail.</returns>
        private JobTimeNoteCategoryDetail FormatDetails(JobTimeNoteCategory jobTimeNoteCategory)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobTimeNoteCategoryDetail
            {
                Id = jobTimeNoteCategory.Id,
                Name = jobTimeNoteCategory.Name,
                CreatedBy = jobTimeNoteCategory.CreatedBy,
                LastModifiedBy = jobTimeNoteCategory.LastModifiedBy,
                CreatedByEmployeeName = jobTimeNoteCategory.CreatedByEmployee != null ? jobTimeNoteCategory.CreatedByEmployee.FirstName + " " + jobTimeNoteCategory.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobTimeNoteCategory.LastModifiedByEmployee != null ? jobTimeNoteCategory.LastModifiedByEmployee.FirstName + " " + jobTimeNoteCategory.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = jobTimeNoteCategory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNoteCategory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNoteCategory.CreatedDate,
                LastModifiedDate = jobTimeNoteCategory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNoteCategory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNoteCategory.LastModifiedDate,
            };
        }

        /// <summary>
        /// Jobs the time note category name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobTimeNoteCategoryNameExists(string name, int id)
        {
            return db.JobTimeNoteCategories.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}