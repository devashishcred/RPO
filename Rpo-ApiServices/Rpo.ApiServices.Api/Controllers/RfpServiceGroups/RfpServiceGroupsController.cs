namespace Rpo.ApiServices.Api.Controllers.RfpServiceGroups
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


    public class RfpServiceGroupsController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the RFP job types Service Group List.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the RFP job types Service Group List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetRfpServiceGroups([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteFeeScheduleMaster))
            {
                var rfpServiceGroups = this.rpoContext.RfpJobTypes.Where(x => x.Level == 4).Include("Parent.Parent.Parent").Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = rfpServiceGroups.Count();
                var recordsFiltered = recordsTotal;

                var result = rfpServiceGroups
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
        /// <returns>Gets the RFP job types Service Group List for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpsubjobtypes/{idRfpSubJobType}/rfpservicegroups/dropdown")]
        public IHttpActionResult GetRfpServiceGroupDropdown(int idRfpSubJobType)
        {
            var result = this.rpoContext.RfpJobTypes.Where(x => x.IdParent == idRfpSubJobType || (idRfpSubJobType == 0 && x.Level == 4)).AsEnumerable().Select(c => new
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
        /// <returns>Gets the RFP job types Service Group detail.</returns>
        [ResponseType(typeof(RfpServiceGroupDetail))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetRfpServiceGroup(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteFeeScheduleMaster))
            {
                RfpJobType rfpServiceGroup = this.rpoContext.RfpJobTypes.Where(x => x.Level == 4).Include("Parent.Parent.Parent").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (rfpServiceGroup == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(rfpServiceGroup));
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
        private bool RfpServiceGroupsExists(int id)
        {
            return rpoContext.RfpJobTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified RFP job type.
        /// </summary>
        /// <param name="rfpServiceGroup">Type of the RFP job.</param>
        /// <returns>RfpWorkTypeCategorieDTO.</returns>
        private RfpServiceGroupDTO Format(RfpJobType rfpServiceGroup)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            RfpServiceGroupDTO rfpServiceGroupDTO = new RfpServiceGroupDTO();
            rfpServiceGroupDTO.Id = rfpServiceGroup.Id;
            rfpServiceGroupDTO.Name = rfpServiceGroup.Name;
            rfpServiceGroupDTO.Level = rfpServiceGroup.Level;
            if (rfpServiceGroup.Parent != null && rfpServiceGroup.Parent.Level == 3)
            {
                rfpServiceGroupDTO.IdRfpSubJobType = rfpServiceGroup.IdParent;
                rfpServiceGroupDTO.RfpSubJobType = rfpServiceGroup.Parent != null ? rfpServiceGroup.Parent.Name : string.Empty;

                if (rfpServiceGroup.Parent.Parent != null && rfpServiceGroup.Parent.Parent.Level == 2)
                {
                    rfpServiceGroupDTO.IdRfpSubJobTypeCategory = rfpServiceGroup.Parent != null ? rfpServiceGroup.Parent.IdParent : 0;
                    rfpServiceGroupDTO.RfpSubJobTypeCategory = rfpServiceGroup.Parent != null && rfpServiceGroup.Parent.Parent != null ? rfpServiceGroup.Parent.Parent.Name : string.Empty;

                    if (rfpServiceGroup.Parent.Parent.Parent != null)
                    {
                        rfpServiceGroupDTO.IdRfpJobType = rfpServiceGroup.Parent.Parent.Parent != null ? rfpServiceGroup.Parent.Parent.IdParent : 0;
                        rfpServiceGroupDTO.RfpJobType = rfpServiceGroup.Parent.Parent.Parent != null && rfpServiceGroup.Parent.Parent.Parent != null ? rfpServiceGroup.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (rfpServiceGroup.Parent.Parent != null && rfpServiceGroup.Parent.Parent.Level == 1)
                {
                    rfpServiceGroupDTO.IdRfpSubJobTypeCategory = 0;
                    rfpServiceGroupDTO.RfpSubJobTypeCategory = string.Empty;

                    rfpServiceGroupDTO.IdRfpJobType = rfpServiceGroup.Parent.Parent != null ? rfpServiceGroup.Parent.IdParent : 0;
                    rfpServiceGroupDTO.RfpJobType = rfpServiceGroup.Parent.Parent != null && rfpServiceGroup.Parent.Parent != null ? rfpServiceGroup.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpServiceGroup.Parent != null && rfpServiceGroup.Parent.Level == 2)
            {

                rfpServiceGroupDTO.IdRfpSubJobType = 0;
                rfpServiceGroupDTO.RfpSubJobType = string.Empty;

                rfpServiceGroupDTO.IdRfpSubJobTypeCategory = rfpServiceGroup.IdParent;
                rfpServiceGroupDTO.RfpSubJobTypeCategory = rfpServiceGroup.Parent != null ? rfpServiceGroup.Parent.Name : string.Empty;

                if (rfpServiceGroup.Parent.Parent != null)
                {
                    rfpServiceGroupDTO.IdRfpJobType = rfpServiceGroup.Parent != null ? rfpServiceGroup.Parent.IdParent : 0;
                    rfpServiceGroupDTO.RfpJobType = rfpServiceGroup.Parent != null && rfpServiceGroup.Parent.Parent != null ? rfpServiceGroup.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpServiceGroup.Parent != null && rfpServiceGroup.Parent.Level == 1)
            {

                rfpServiceGroupDTO.IdRfpJobType = rfpServiceGroup.IdParent;
                rfpServiceGroupDTO.RfpJobType = rfpServiceGroup.Parent != null ? rfpServiceGroup.Parent.Name : string.Empty;

                rfpServiceGroupDTO.IdRfpSubJobTypeCategory = 0;
                rfpServiceGroupDTO.RfpSubJobTypeCategory = string.Empty;

                rfpServiceGroupDTO.IdRfpSubJobType = 0;
                rfpServiceGroupDTO.RfpSubJobType = string.Empty;
            }

            rfpServiceGroupDTO.DisplayOrder = rfpServiceGroup.DisplayOrder;
            rfpServiceGroupDTO.CreatedBy = rfpServiceGroup.CreatedBy;
            rfpServiceGroupDTO.LastModifiedBy = rfpServiceGroup.LastModifiedBy != null ? rfpServiceGroup.LastModifiedBy : rfpServiceGroup.CreatedBy;
            rfpServiceGroupDTO.CreatedByEmployeeName = rfpServiceGroup.CreatedByEmployee != null ? rfpServiceGroup.CreatedByEmployee.FirstName + " " + rfpServiceGroup.CreatedByEmployee.LastName : string.Empty;
            rfpServiceGroupDTO.LastModifiedByEmployeeName = rfpServiceGroup.LastModifiedByEmployee != null ? rfpServiceGroup.LastModifiedByEmployee.FirstName + " " + rfpServiceGroup.LastModifiedByEmployee.LastName : (rfpServiceGroup.CreatedByEmployee != null ? rfpServiceGroup.CreatedByEmployee.FirstName + " " + rfpServiceGroup.CreatedByEmployee.LastName : string.Empty);
            rfpServiceGroupDTO.CreatedDate = rfpServiceGroup.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceGroup.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpServiceGroup.CreatedDate;
            rfpServiceGroupDTO.LastModifiedDate = rfpServiceGroup.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceGroup.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpServiceGroup.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceGroup.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpServiceGroup.CreatedDate);

            return rfpServiceGroupDTO;
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="rfpServiceGroup">Type of the RFP job.</param>
        /// <returns>RfpWorkTypeCategorieDetail.</returns>
        private RfpServiceGroupDetail FormatDetails(RfpJobType rfpServiceGroup)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            RfpServiceGroupDetail rfpServiceGroupDetail = new RfpServiceGroupDetail();
            rfpServiceGroupDetail.Id = rfpServiceGroup.Id;
            rfpServiceGroupDetail.Name = rfpServiceGroup.Name;
            rfpServiceGroupDetail.Level = rfpServiceGroup.Level;

            if (rfpServiceGroup.Parent != null && rfpServiceGroup.Parent.Level == 3)
            {
                rfpServiceGroupDetail.IdRfpSubJobType = rfpServiceGroup.IdParent;
                rfpServiceGroupDetail.RfpSubJobType = rfpServiceGroup.Parent != null ? rfpServiceGroup.Parent.Name : string.Empty;

                if (rfpServiceGroup.Parent.Parent != null && rfpServiceGroup.Parent.Parent.Level == 2)
                {
                    rfpServiceGroupDetail.IdRfpSubJobTypeCategory = rfpServiceGroup.Parent != null ? rfpServiceGroup.Parent.IdParent : 0;
                    rfpServiceGroupDetail.RfpSubJobTypeCategory = rfpServiceGroup.Parent != null && rfpServiceGroup.Parent.Parent != null ? rfpServiceGroup.Parent.Parent.Name : string.Empty;

                    if (rfpServiceGroup.Parent.Parent.Parent != null)
                    {
                        rfpServiceGroupDetail.IdRfpJobType = rfpServiceGroup.Parent.Parent.Parent != null ? rfpServiceGroup.Parent.Parent.IdParent : 0;
                        rfpServiceGroupDetail.RfpJobType = rfpServiceGroup.Parent.Parent.Parent != null && rfpServiceGroup.Parent.Parent.Parent != null ? rfpServiceGroup.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (rfpServiceGroup.Parent.Parent != null && rfpServiceGroup.Parent.Parent.Level == 1)
                {
                    rfpServiceGroupDetail.IdRfpSubJobTypeCategory = 0;
                    rfpServiceGroupDetail.RfpSubJobTypeCategory = string.Empty;

                    rfpServiceGroupDetail.IdRfpJobType = rfpServiceGroup.Parent.Parent != null ? rfpServiceGroup.Parent.IdParent : 0;
                    rfpServiceGroupDetail.RfpJobType = rfpServiceGroup.Parent.Parent != null && rfpServiceGroup.Parent.Parent != null ? rfpServiceGroup.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpServiceGroup.Parent != null && rfpServiceGroup.Parent.Level == 2)
            {

                rfpServiceGroupDetail.IdRfpSubJobType = 0;
                rfpServiceGroupDetail.RfpSubJobType = string.Empty;

                rfpServiceGroupDetail.IdRfpSubJobTypeCategory = rfpServiceGroup.IdParent;
                rfpServiceGroupDetail.RfpSubJobTypeCategory = rfpServiceGroup.Parent != null ? rfpServiceGroup.Parent.Name : string.Empty;

                if (rfpServiceGroup.Parent.Parent != null)
                {
                    rfpServiceGroupDetail.IdRfpJobType = rfpServiceGroup.Parent != null ? rfpServiceGroup.Parent.IdParent : 0;
                    rfpServiceGroupDetail.RfpJobType = rfpServiceGroup.Parent != null && rfpServiceGroup.Parent.Parent != null ? rfpServiceGroup.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpServiceGroup.Parent != null && rfpServiceGroup.Parent.Level == 1)
            {

                rfpServiceGroupDetail.IdRfpJobType = rfpServiceGroup.IdParent;
                rfpServiceGroupDetail.RfpJobType = rfpServiceGroup.Parent != null ? rfpServiceGroup.Parent.Name : string.Empty;

                rfpServiceGroupDetail.IdRfpSubJobTypeCategory = 0;
                rfpServiceGroupDetail.RfpSubJobTypeCategory = string.Empty;

                rfpServiceGroupDetail.IdRfpSubJobType = 0;
                rfpServiceGroupDetail.RfpSubJobType = string.Empty;
            }

            rfpServiceGroupDetail.DisplayOrder = rfpServiceGroup.DisplayOrder;
            rfpServiceGroupDetail.CreatedBy = rfpServiceGroup.CreatedBy;
            rfpServiceGroupDetail.LastModifiedBy = rfpServiceGroup.LastModifiedBy != null ? rfpServiceGroup.LastModifiedBy : rfpServiceGroup.CreatedBy;
            rfpServiceGroupDetail.CreatedByEmployeeName = rfpServiceGroup.CreatedByEmployee != null ? rfpServiceGroup.CreatedByEmployee.FirstName + " " + rfpServiceGroup.CreatedByEmployee.LastName : string.Empty;
            rfpServiceGroupDetail.LastModifiedByEmployeeName = rfpServiceGroup.LastModifiedByEmployee != null ? rfpServiceGroup.LastModifiedByEmployee.FirstName + " " + rfpServiceGroup.LastModifiedByEmployee.LastName : (rfpServiceGroup.CreatedByEmployee != null ? rfpServiceGroup.CreatedByEmployee.FirstName + " " + rfpServiceGroup.CreatedByEmployee.LastName : string.Empty);
            rfpServiceGroupDetail.CreatedDate = rfpServiceGroup.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceGroup.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpServiceGroup.CreatedDate;
            rfpServiceGroupDetail.LastModifiedDate = rfpServiceGroup.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceGroup.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpServiceGroup.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceGroup.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpServiceGroup.CreatedDate);

            return rfpServiceGroupDetail;
        }

        /// <summary>
        /// RFPs the job type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpServiceGroupsNameExists(string name, int id)
        {
            return rpoContext.RfpJobTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}