// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-08-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-08-2018
// ***********************************************************************
// <copyright file="RfpSubJobTypesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class RFP Sub Job Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The RFP Sub Job Types namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.RfpSubJobTypes
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
    /// Class RFP SubJob Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class RfpSubJobTypesController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the RFP job types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the list of job type desciption.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetRfpSubJobTypes([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteFeeScheduleMaster))
            {
                var rfpSubJobTypes = this.rpoContext.RfpJobTypes.Where(x => x.Level == 3).Include("Parent.Parent").Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = rfpSubJobTypes.Count();
                var recordsFiltered = recordsTotal;

                var result = rfpSubJobTypes
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
        /// <returns>Get the list of job type desciption.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpsubjobtypecategories/{idRfpSubJobTypeCategory}/rfpsubjobtypes/dropdown")]
        public IHttpActionResult GetRfpSubJobTypeDropdown(int idRfpSubJobTypeCategory)
        {
            var result = this.rpoContext.RfpJobTypes.Where(x => x.IdParent == idRfpSubJobTypeCategory || (idRfpSubJobTypeCategory == 0 && x.Level == 3)).AsEnumerable().Select(c => new
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
        /// <returns>Get the detail of job type desciption.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpSubJobTypeDetail))]
        public IHttpActionResult GetRfpSubJobType(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteFeeScheduleMaster))
            {
                RfpJobType rfpSubJobType = this.rpoContext.RfpJobTypes.Include("CreatedByEmployee").Include("Parent.Parent").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

                if (rfpSubJobType == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(rfpSubJobType));
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
        private bool RfpSubJobTypeExists(int id)
        {
            return rpoContext.RfpJobTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified RFP job type.
        /// </summary>
        /// <param name="rfpSubJobType">Type of the RFP job.</param>
        /// <returns>RfpSubJobTypeDTO.</returns>
        private RfpSubJobTypeDTO Format(RfpJobType rfpSubJobType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            RfpSubJobTypeDTO rfpSubJobTypeDTO = new RfpSubJobTypeDTO();
            rfpSubJobTypeDTO.Id = rfpSubJobType.Id;
            rfpSubJobTypeDTO.Name = rfpSubJobType.Name;
            rfpSubJobTypeDTO.Level = rfpSubJobType.Level;
            if (rfpSubJobType.Parent != null && rfpSubJobType.Parent.Level == 2)
            {
                rfpSubJobTypeDTO.IdRfpSubJobTypeCategory = rfpSubJobType.IdParent;
                rfpSubJobTypeDTO.RfpSubJobTypeCategory = rfpSubJobType.Parent != null ? rfpSubJobType.Parent.Name : string.Empty;

                if (rfpSubJobType.Parent.Parent != null)
                {
                    rfpSubJobTypeDTO.IdRfpJobType = rfpSubJobType.Parent != null ? rfpSubJobType.Parent.IdParent : 0;
                    rfpSubJobTypeDTO.RfpJobType = rfpSubJobType.Parent != null && rfpSubJobType.Parent.Parent != null ? rfpSubJobType.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpSubJobType.Parent != null && rfpSubJobType.Parent.Level == 1)
            {

                rfpSubJobTypeDTO.IdRfpJobType = rfpSubJobType.IdParent;
                rfpSubJobTypeDTO.RfpJobType = rfpSubJobType.Parent != null ? rfpSubJobType.Parent.Name : string.Empty;

                rfpSubJobTypeDTO.IdRfpSubJobTypeCategory = 0;
                rfpSubJobTypeDTO.RfpSubJobTypeCategory = string.Empty;
            }

            rfpSubJobTypeDTO.CreatedBy = rfpSubJobType.CreatedBy;
            rfpSubJobTypeDTO.LastModifiedBy = rfpSubJobType.LastModifiedBy != null ? rfpSubJobType.LastModifiedBy : rfpSubJobType.CreatedBy;
            rfpSubJobTypeDTO.CreatedByEmployeeName = rfpSubJobType.CreatedByEmployee != null ? rfpSubJobType.CreatedByEmployee.FirstName + " " + rfpSubJobType.CreatedByEmployee.LastName : string.Empty;
            rfpSubJobTypeDTO.LastModifiedByEmployeeName = rfpSubJobType.LastModifiedByEmployee != null ? rfpSubJobType.LastModifiedByEmployee.FirstName + " " + rfpSubJobType.LastModifiedByEmployee.LastName : (rfpSubJobType.CreatedByEmployee != null ? rfpSubJobType.CreatedByEmployee.FirstName + " " + rfpSubJobType.CreatedByEmployee.LastName : string.Empty);
            rfpSubJobTypeDTO.CreatedDate = rfpSubJobType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpSubJobType.CreatedDate;
            rfpSubJobTypeDTO.LastModifiedDate = rfpSubJobType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpSubJobType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpSubJobType.CreatedDate);

            return rfpSubJobTypeDTO;
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="rfpSubJobType">Type of the RFP job.</param>
        /// <returns>RfpSubJobTypeDetail.</returns>
        private RfpSubJobTypeDetail FormatDetails(RfpJobType rfpSubJobType)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            RfpSubJobTypeDetail rfpSubJobTypeDetail = new RfpSubJobTypeDetail();
            rfpSubJobTypeDetail.Id = rfpSubJobType.Id;
            rfpSubJobTypeDetail.Name = rfpSubJobType.Name;
            rfpSubJobTypeDetail.Level = rfpSubJobType.Level;
            if (rfpSubJobType.Parent != null && rfpSubJobType.Parent.Level == 2)
            {
                rfpSubJobTypeDetail.IdRfpSubJobTypeCategory = rfpSubJobType.IdParent;
                rfpSubJobTypeDetail.RfpSubJobTypeCategory = rfpSubJobType.Parent != null ? rfpSubJobType.Parent.Name : string.Empty;

                if (rfpSubJobType.Parent.Parent != null)
                {
                    rfpSubJobTypeDetail.IdRfpJobType = rfpSubJobType.Parent != null ? rfpSubJobType.Parent.IdParent : 0;
                    rfpSubJobTypeDetail.RfpJobType = rfpSubJobType.Parent != null && rfpSubJobType.Parent.Parent != null ? rfpSubJobType.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpSubJobType.Parent != null && rfpSubJobType.Parent.Level == 1)
            {

                rfpSubJobTypeDetail.IdRfpJobType = rfpSubJobType.IdParent;
                rfpSubJobTypeDetail.RfpJobType = rfpSubJobType.Parent != null ? rfpSubJobType.Parent.Name : string.Empty;

                rfpSubJobTypeDetail.IdRfpSubJobTypeCategory = 0;
                rfpSubJobTypeDetail.RfpSubJobTypeCategory = string.Empty;
            }

            rfpSubJobTypeDetail.CreatedBy = rfpSubJobType.CreatedBy;
            rfpSubJobTypeDetail.LastModifiedBy = rfpSubJobType.LastModifiedBy != null ? rfpSubJobType.LastModifiedBy : rfpSubJobType.CreatedBy;
            rfpSubJobTypeDetail.CreatedByEmployeeName = rfpSubJobType.CreatedByEmployee != null ? rfpSubJobType.CreatedByEmployee.FirstName + " " + rfpSubJobType.CreatedByEmployee.LastName : string.Empty;
            rfpSubJobTypeDetail.LastModifiedByEmployeeName = rfpSubJobType.LastModifiedByEmployee != null ? rfpSubJobType.LastModifiedByEmployee.FirstName + " " + rfpSubJobType.LastModifiedByEmployee.LastName : (rfpSubJobType.CreatedByEmployee != null ? rfpSubJobType.CreatedByEmployee.FirstName + " " + rfpSubJobType.CreatedByEmployee.LastName : string.Empty);
            rfpSubJobTypeDetail.CreatedDate = rfpSubJobType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpSubJobType.CreatedDate;
            rfpSubJobTypeDetail.LastModifiedDate = rfpSubJobType.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobType.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpSubJobType.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpSubJobType.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpSubJobType.CreatedDate);

            return rfpSubJobTypeDetail;
        }

        /// <summary>
        /// RFPs the job type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpSubJobTypeNameExists(string name, int id)
        {
            return rpoContext.RfpJobTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}