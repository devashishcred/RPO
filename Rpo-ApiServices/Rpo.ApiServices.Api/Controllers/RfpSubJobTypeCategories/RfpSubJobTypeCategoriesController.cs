/// <summary>
/// The RfpSubJobTypeCategories namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.RfpSubJobTypeCategories
{
    using System;
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
    /// Class RfpSubJobTypeCategoriesController.
    /// </summary>
    public class RfpSubJobTypeCategoriesController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the RFP job types desciption.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the list of job type desciption.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetRfpSubJobTypeCategories([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteFeeScheduleMaster))
            {
                var rfpSubJobTypeCategories = this.rpoContext.RfpJobTypes.Where(x => x.Level == 2).Include("Parent").Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = rfpSubJobTypeCategories.Count();
                var recordsFiltered = recordsTotal;

                var result = rfpSubJobTypeCategories
                    .AsEnumerable()
                    .Select(c => Format(c)).OrderByDescending(x => x.LastModifiedDate)
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
        /// Gets the RFP job type dropdown.
        /// </summary>
        /// <returns>Get the list of job type desciption</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpjobtypes/{idRfpJobType}/rfpsubjobtypecategories/dropdown")]
        public IHttpActionResult GetRfpSubJobTypeCategoryDropdown(int idRfpJobType)
        {
            var result = this.rpoContext.RfpJobTypes.Where(x => x.IdParent == idRfpJobType || (idRfpJobType == 0 && x.Level == 2)).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                Level = c.Level
            }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the type of the RFP job.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Get the list of job type desciption.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpSubJobTypeCategoryDetail))]
        public IHttpActionResult GetRfpSubJobTypeCategory(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteFeeScheduleMaster))
            {
                RfpJobType rfpSubJobTypeCategory = this.rpoContext.RfpJobTypes.Include("Parent").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (rfpSubJobTypeCategory == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(rfpSubJobTypeCategory));
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
        /// RFPs the job type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpSubJobTypeCategoriesExists(int id)
        {
            return rpoContext.RfpJobTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified RFP job type.
        /// </summary>
        /// <param name="rfpSubJobTypeCategory">Type of the RFP job.</param>
        /// <returns>RfpSubJobTypeCategorieDTO.</returns>
        private RfpSubJobTypeCategoryDTO Format(RfpJobType rfpSubJobTypeCategory)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpSubJobTypeCategoryDTO
            {
                Id = rfpSubJobTypeCategory.Id,
                Name = rfpSubJobTypeCategory.Name,
                Level = rfpSubJobTypeCategory.Level,
                IsCurrentStatusOfFiling = rfpSubJobTypeCategory.IsCurrentStatusOfFiling,
                IdRfpJobType = rfpSubJobTypeCategory.IdParent,
                RfpJobType = rfpSubJobTypeCategory.Parent != null ? rfpSubJobTypeCategory.Parent.Name : string.Empty,
                CreatedBy = rfpSubJobTypeCategory.CreatedBy,
                LastModifiedBy = rfpSubJobTypeCategory.LastModifiedBy != null ? rfpSubJobTypeCategory.LastModifiedBy : rfpSubJobTypeCategory.CreatedBy,
                CreatedByEmployeeName = rfpSubJobTypeCategory.CreatedByEmployee != null ? rfpSubJobTypeCategory.CreatedByEmployee.FirstName + " " + rfpSubJobTypeCategory.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = rfpSubJobTypeCategory.LastModifiedByEmployee != null ? rfpSubJobTypeCategory.LastModifiedByEmployee.FirstName + " " + rfpSubJobTypeCategory.LastModifiedByEmployee.LastName : (rfpSubJobTypeCategory.CreatedByEmployee != null ? rfpSubJobTypeCategory.CreatedByEmployee.FirstName + " " + rfpSubJobTypeCategory.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = rfpSubJobTypeCategory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobTypeCategory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpSubJobTypeCategory.CreatedDate,
                LastModifiedDate = rfpSubJobTypeCategory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobTypeCategory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpSubJobTypeCategory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobTypeCategory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpSubJobTypeCategory.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="rfpSubJobTypeCategory">Type of the RFP job.</param>
        /// <returns>Get the list of job type desciption.</returns>
        private RfpSubJobTypeCategoryDetail FormatDetails(RfpJobType rfpSubJobTypeCategory)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpSubJobTypeCategoryDetail
            {
                Id = rfpSubJobTypeCategory.Id,
                Name = rfpSubJobTypeCategory.Name,
                Level = rfpSubJobTypeCategory.Level,
                IsCurrentStatusOfFiling = rfpSubJobTypeCategory.IsCurrentStatusOfFiling,
                IdRfpJobType = rfpSubJobTypeCategory.IdParent,
                RfpJobType = rfpSubJobTypeCategory.Parent != null ? rfpSubJobTypeCategory.Parent.Name : string.Empty,
                CreatedBy = rfpSubJobTypeCategory.CreatedBy,
                LastModifiedBy = rfpSubJobTypeCategory.LastModifiedBy != null ? rfpSubJobTypeCategory.LastModifiedBy : rfpSubJobTypeCategory.CreatedBy,
                CreatedByEmployeeName = rfpSubJobTypeCategory.CreatedByEmployee != null ? rfpSubJobTypeCategory.CreatedByEmployee.FirstName + " " + rfpSubJobTypeCategory.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = rfpSubJobTypeCategory.LastModifiedByEmployee != null ? rfpSubJobTypeCategory.LastModifiedByEmployee.FirstName + " " + rfpSubJobTypeCategory.LastModifiedByEmployee.LastName : (rfpSubJobTypeCategory.CreatedByEmployee != null ? rfpSubJobTypeCategory.CreatedByEmployee.FirstName + " " + rfpSubJobTypeCategory.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = rfpSubJobTypeCategory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobTypeCategory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpSubJobTypeCategory.CreatedDate,
                LastModifiedDate = rfpSubJobTypeCategory.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobTypeCategory.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpSubJobTypeCategory.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobTypeCategory.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpSubJobTypeCategory.CreatedDate),
            };
        }

        /// <summary>
        /// RFPs the job type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpSubJobTypeCategoriesNameExists(string name, int id)
        {
            return rpoContext.RfpJobTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}