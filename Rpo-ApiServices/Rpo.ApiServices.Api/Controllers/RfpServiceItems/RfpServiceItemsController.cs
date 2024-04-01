// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-08-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-08-2018
// ***********************************************************************
// <copyright file="RfpServiceItemsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class RFP Sub Job Types Controller.</summary>
// ***********************************************************************

/// <summary>
/// The RFP Sub Job Types namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.RfpServiceItems
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using Filters;
    using Model.Models.Enums;
    using RfpJobTypes;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class RFP SubJob Types Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />

    public class RfpServiceItemsController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the RFP job types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the job Type service Item List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetRfpServiceItems([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteFeeScheduleMaster))
            {
                var rfpServiceItems = this.rpoContext.RfpJobTypes.Where(x => x.Level == 5).Include("Parent.Parent.Parent.Parent").Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

                var recordsTotal = rfpServiceItems.Count();
                var recordsFiltered = recordsTotal;

                var result = rfpServiceItems
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
        /// Gets the RFP work type dropdown.
        /// </summary>
        /// <returns>Gets the RFP job types Service item List for dropdown.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpservicegroups/{idRfpServiceItemCategory}/rfpserviceitems/dropdown")]
        public IHttpActionResult GetRfpServiceItemDropdown(int idRfpServiceItemCategory)
        {
            var result = this.rpoContext.RfpJobTypes.Where(x => (x.IdParent == idRfpServiceItemCategory || (idRfpServiceItemCategory == 0 && x.Level == 5)) && x.IsActive==true).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                Level = c.Level
            }).ToArray();

            return Ok(result);
        }
        /// <summary>
        ///  Gets the RFP work type dropdown. For Edit screen
        /// </summary>
        /// <param name="idRfpServiceItemCategory"></param>
        /// <param name="idJobtypes"></param>
        /// <returns>Gets the RFP job types Service Group and item List</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/editrfpservicegroups/{idRfpServiceItemCategory}/{idJobtypes}/rfpserviceitems/dropdown")]
        public IHttpActionResult GetEditRfpServiceItemDropdown(int idRfpServiceItemCategory,int idJobtypes)
        {
            if (idJobtypes == 0)
            {
                throw new RpoBusinessException("Value is not Null or zero!");
            }

            var result1 = this.rpoContext.RfpJobTypes.Where(x => (x.IdParent == idRfpServiceItemCategory || (idRfpServiceItemCategory == 0 && x.Level == 5)) && x.IsActive == true).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                Level = c.Level
            }).ToArray();

            var result2 = this.rpoContext.RfpJobTypes.Where(x => (x.IdParent == idRfpServiceItemCategory || (idRfpServiceItemCategory == 0 && x.Level == 5)) && x.Id == idJobtypes).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                Level = c.Level
            }).ToArray();

            var result = result1.Union(result2);
            return Ok(result);
        }

        /// <summary>
        /// Get RFP Service items list of partof items
        /// </summary>
        /// <param name="IdJobType"></param>
        /// <param name="IdJobDescription"></param>
        /// <param name="IdJobSubType"></param>
        /// <returns>Gets the RFP job types Service item of partof items List</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfppartof/{IdJobType}/{IdJobDescription}/{IdJobSubType}/dropdown")]
        public IHttpActionResult GetRfpServicePartofDropdown(int IdJobType, int IdJobDescription, int IdJobSubType)
        {
            List<int> objallids = new List<int>();
            List<int> objalllevel = new List<int>();
            if (IdJobType!=0 && IdJobDescription != 0 && IdJobSubType != 0)
            {
              //  objallids.Add(IdJobType);
              //  objallids.Add(IdJobDescription);
                objallids.Add(IdJobSubType);
                List<int> tmpList2 = this.rpoContext.RfpJobTypes.Where(d => d.IdParent == IdJobSubType && d.Level == 4).Select(d => d.Id).ToList();

                objallids.AddRange(tmpList2);
                //   objallids.AddRange(this.rpoContext.RfpJobTypes.Where(d => d.IdParent == IdJobDescription).Select(d => d.Id).ToList());
                // objallids.AddRange(this.rpoContext.RfpJobTypes.Where(d => d.IdParent == IdJobSubType).Select(d => d.Id).ToList());
            }
           else if (IdJobType != 0 && IdJobDescription != 0)
            {
                //objallids.Add(IdJobType);

                objallids.Add(IdJobDescription);

                List<int> tmpList = this.rpoContext.RfpJobTypes.Where(d => d.IdParent == IdJobDescription && d.Level==3).Select(d => d.Id).ToList();

                if(tmpList.Count() <= 0)
                {
                    List<int> tmpList2 = this.rpoContext.RfpJobTypes.Where(d => d.IdParent == IdJobDescription && d.Level == 4).Select(d => d.Id).ToList();

                   objallids.AddRange(tmpList2);
                }

                //objallids.AddRange(tmpList);

                //List<int> tmpList2 = this.rpoContext.RfpJobTypes.Where(d => tmpList.Contains(d.IdParent.Value)).Select(d => d.Id).ToList();

                //objallids.AddRange(tmpList2);

                //List<int> tmpList3 = this.rpoContext.RfpJobTypes.Where(d => tmpList2.Contains(d.IdParent.Value)).Select(d => d.Id).ToList();
                //objallids.AddRange(tmpList3);

                //objallids.Add(IdJobDescription);   
            }
            else if (IdJobType != 0 && IdJobSubType != 0)
            {
               // objallids.Add(IdJobType);
                objallids.Add(IdJobSubType);
                List<int> tmpList2 = this.rpoContext.RfpJobTypes.Where(d => d.IdParent == IdJobSubType && d.Level == 4).Select(d => d.Id).ToList();

                objallids.AddRange(tmpList2);
                // objallids.AddRange(this.rpoContext.RfpJobTypes.Where(d => d.IdParent == IdJobSubType).Select(d => d.Id).ToList());
            }
            else if (IdJobType != 0)
            {
                objallids.Add(IdJobType);
                List<int> tmpList2 = this.rpoContext.RfpJobTypes.Where(d => d.IdParent == IdJobType && d.Level == 4).Select(d => d.Id).ToList();

                objallids.AddRange(tmpList2);
            }         

            var result =  this.rpoContext.RfpJobTypes.Where(x => objallids.Contains(x.IdParent.Value) && x.Level == 5).AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
                Level = c.Level
            }).ToArray();

            return Ok(result);
        }
        /// <summary>
        /// Gets the RFP service item drop down.
        /// </summary>
        /// <returns>Gets the RFP Service Item List</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpserviceitems/dropdown")]
        [ResponseType(typeof(ServiceItemDTO))]
        public IHttpActionResult GetRfpServiceItemDropDown()
        {
            List<RfpJobType> rfpServiceItem = this.rpoContext.RfpJobTypes.Where(x => x.Level == 5 && x.IsActive==true && x.IsShowScope == false &&  x.CostType != RfpCostType.HourlyCost).Include("Parent.Parent.Parent.Parent").ToList();
            if (rfpServiceItem == null)
            {
                return this.NotFound();
            }

            return Ok(rfpServiceItem.AsEnumerable().Select(x => FormatServiceDetails(x)).ToArray());
        }
        /// <summary>
        /// Gets the RFP service item against jobtypes drop down
        /// </summary>
        /// <param name="idJobtypes"></param>
        /// <returns>Gets the RFP  Service item List for dropdown</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpserviceitems/{idJobtypes}/dropdown")]
        [ResponseType(typeof(ServiceItemDTO))]
        public IHttpActionResult GetEditRfpServiceItemDropDown(int idJobtypes)
        {
            if (idJobtypes == 0)
            {
                throw new RpoBusinessException("Value is not Null or zero!");
            }
            List<RfpJobType> rfpServiceItem = new List<RfpJobType>();
            rfpServiceItem = this.rpoContext.RfpJobTypes.Where(x => x.Level == 5 && x.IsActive == true && x.IsShowScope == false && x.CostType != RfpCostType.HourlyCost).Include("Parent.Parent.Parent.Parent").ToList();

            rfpServiceItem = this.rpoContext.RfpJobTypes.Where(x => x.Level == 5 && x.Id == idJobtypes && x.IsShowScope == false && x.CostType != RfpCostType.HourlyCost).Include("Parent.Parent.Parent.Parent").ToList();

            if (rfpServiceItem == null)
            {
                return this.NotFound();
            }

            return Ok(rfpServiceItem.AsEnumerable().Select(x => FormatServiceDetails(x)).ToArray());
        }
        /// <summary>
        /// Gets the RFP hourly service item list for drop down
        /// </summary>
        /// <returns>Gets the RFP hourly service item list for drop down</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpserviceitems/hourlydropdown")]
        [ResponseType(typeof(ServiceItemDTO))]
        public IHttpActionResult GetRfpServiceItemHourlyDropDown()
        {
            List<RfpJobType> rfpServiceItem = this.rpoContext.RfpJobTypes.Where(x => x.Level == 5 && x.IsActive == true && x.IsShowScope==false && x.CostType == RfpCostType.HourlyCost).Include("Parent.Parent.Parent.Parent").ToList();
            if (rfpServiceItem == null)
            {
                return this.NotFound();
            }

            return Ok(rfpServiceItem.AsEnumerable().Select(x => FormatServiceDetails(x)).ToArray());
        }
        /// <summary>
        /// Gets the RFP hourly service item list against jobtypes for drop down
        /// </summary>
        /// <param name="idJobtypes"></param>
        /// <returns>FP hourly service item list for drop down of show in scope </returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/rfpserviceitems/{idJobtypes}/hourlydropdown")]
        [ResponseType(typeof(ServiceItemDTO))]
        public IHttpActionResult GetRfpServiceItemHourlyDropDown(int idJobtypes)
        {
            if (idJobtypes == 0)
            {
                throw new RpoBusinessException("Value is not Null or zero!");
            }
            List<RfpJobType> rfpServiceItem = new List<RfpJobType>();

            rfpServiceItem = this.rpoContext.RfpJobTypes.Where(x => x.Level == 5 && x.IsActive == true && x.IsShowScope == false && x.CostType == RfpCostType.HourlyCost).Include("Parent.Parent.Parent.Parent").ToList();
            rfpServiceItem = this.rpoContext.RfpJobTypes.Where(x => x.Level == 5 && x.Id == idJobtypes && x.IsShowScope == false && x.CostType == RfpCostType.HourlyCost).Include("Parent.Parent.Parent.Parent").ToList();
            if (rfpServiceItem == null)
            {
                return this.NotFound();
            }

            return Ok(rfpServiceItem.AsEnumerable().Select(x => FormatServiceDetails(x)).ToArray());
        }

        /// <summary>
        /// Gets the type of the RFP job.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>RFP hourly service item list for drop down.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpServiceItemDetail))]
        public IHttpActionResult GetRfpServiceItem(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddFeeScheduleMaster)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteFeeScheduleMaster))
            {
                RfpJobType rfpServiceItem = this.rpoContext.RfpJobTypes.Where(x => x.Id == id).Include("Parent.Parent.Parent.Parent").Include("RfpJobTypeCostRanges").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault();


                if (rfpServiceItem == null)
                {
                    return this.NotFound();
                }

                return Ok(FormatDetails(rfpServiceItem));
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
        private bool RfpServiceItemExists(int id)
        {
            return rpoContext.RfpJobTypes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified RFP job type.
        /// </summary>
        /// <param name="rfpServiceItem">Type of the RFP job.</param>
        /// <returns>RfpServiceItemDTO.</returns>
        private RfpServiceItemDTO Format(RfpJobType rfpServiceItem)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            RfpServiceItemDTO rfpServiceItemDTO = new RfpServiceItemDTO();
            rfpServiceItemDTO.Id = rfpServiceItem.Id;
            rfpServiceItemDTO.Name = rfpServiceItem.Name;

            if (rfpServiceItem.Parent != null && rfpServiceItem.Parent.Level == 4)
            {
                rfpServiceItemDTO.IdRfpServiceGroup = rfpServiceItem.IdParent;
                rfpServiceItemDTO.RfpServiceGroup = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.Name : string.Empty;

                if (rfpServiceItem.Parent.Parent != null && rfpServiceItem.Parent.Parent.Level == 3)
                {
                    rfpServiceItemDTO.IdRfpSubJobType = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpSubJobType = rfpServiceItem.Parent != null && rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;

                    if (rfpServiceItem.Parent.Parent.Parent != null && rfpServiceItem.Parent.Parent.Parent.Level == 2)
                    {
                        rfpServiceItemDTO.IdRfpSubJobTypeCategory = rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.Id : 0;
                        rfpServiceItemDTO.RfpSubJobTypeCategory = rfpServiceItem.Parent.Parent.Parent != null && rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.Name : string.Empty;

                        if (rfpServiceItem.Parent.Parent.Parent.Parent != null)
                        {
                            rfpServiceItemDTO.IdRfpJobType = rfpServiceItem.Parent.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.IdParent : 0;
                            rfpServiceItemDTO.RfpJobType = rfpServiceItem.Parent.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (rfpServiceItem.Parent.Parent.Parent != null && rfpServiceItem.Parent.Parent.Parent.Level == 1)
                    {
                        rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
                        rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

                        rfpServiceItemDTO.IdRfpJobType = rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.IdParent : 0;
                        rfpServiceItemDTO.RfpJobType = rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (rfpServiceItem.Parent.Parent != null && rfpServiceItem.Parent.Parent.Level == 2)
                {
                    rfpServiceItemDTO.IdRfpSubJobType = 0;
                    rfpServiceItemDTO.RfpSubJobType = string.Empty;

                    rfpServiceItemDTO.IdRfpSubJobTypeCategory = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpSubJobTypeCategory = rfpServiceItem.Parent != null && rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;

                    if (rfpServiceItem.Parent.Parent.Parent != null)
                    {
                        rfpServiceItemDTO.IdRfpJobType = rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.IdParent : 0;
                        rfpServiceItemDTO.RfpJobType = rfpServiceItem.Parent.Parent.Parent != null && rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (rfpServiceItem.Parent.Parent != null && rfpServiceItem.Parent.Parent.Level == 1)
                {
                    rfpServiceItemDTO.IdRfpSubJobType = 0;
                    rfpServiceItemDTO.RfpSubJobType = string.Empty;

                    rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
                    rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

                    rfpServiceItemDTO.IdRfpJobType = rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpJobType = rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpServiceItem.Parent != null && rfpServiceItem.Parent.Level == 3)
            {
                rfpServiceItemDTO.IdRfpServiceGroup = 0;
                rfpServiceItemDTO.RfpServiceGroup = string.Empty;

                rfpServiceItemDTO.IdRfpSubJobType = rfpServiceItem.IdParent;
                rfpServiceItemDTO.RfpSubJobType = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.Name : string.Empty;

                if (rfpServiceItem.Parent.Parent != null && rfpServiceItem.Parent.Parent.Level == 2)
                {
                    rfpServiceItemDTO.IdRfpSubJobTypeCategory = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpSubJobTypeCategory = rfpServiceItem.Parent != null && rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;

                    if (rfpServiceItem.Parent.Parent.Parent != null)
                    {
                        rfpServiceItemDTO.IdRfpJobType = rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.IdParent : 0;
                        rfpServiceItemDTO.RfpJobType = rfpServiceItem.Parent.Parent.Parent != null && rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (rfpServiceItem.Parent.Parent != null && rfpServiceItem.Parent.Parent.Level == 1)
                {
                    rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
                    rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

                    rfpServiceItemDTO.IdRfpJobType = rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpJobType = rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpServiceItem.Parent != null && rfpServiceItem.Parent.Level == 2)
            {
                rfpServiceItemDTO.IdRfpServiceGroup = 0;
                rfpServiceItemDTO.RfpServiceGroup = string.Empty;

                rfpServiceItemDTO.IdRfpSubJobType = 0;
                rfpServiceItemDTO.RfpSubJobType = string.Empty;

                rfpServiceItemDTO.IdRfpSubJobTypeCategory = rfpServiceItem.IdParent;
                rfpServiceItemDTO.RfpSubJobTypeCategory = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.Name : string.Empty;

                if (rfpServiceItem.Parent.Parent != null)
                {
                    rfpServiceItemDTO.IdRfpJobType = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpJobType = rfpServiceItem.Parent != null && rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpServiceItem.Parent != null && rfpServiceItem.Parent.Level == 1)
            {

                rfpServiceItemDTO.IdRfpJobType = rfpServiceItem.IdParent;
                rfpServiceItemDTO.RfpJobType = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.Name : string.Empty;

                rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
                rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

                rfpServiceItemDTO.IdRfpSubJobType = 0;
                rfpServiceItemDTO.RfpSubJobType = string.Empty;

                rfpServiceItemDTO.IdRfpServiceGroup = 0;
                rfpServiceItemDTO.RfpServiceGroup = string.Empty;
            }

            rfpServiceItemDTO.CreatedBy = rfpServiceItem.CreatedBy;
            rfpServiceItemDTO.LastModifiedBy = rfpServiceItem.LastModifiedBy != null ? rfpServiceItem.LastModifiedBy : rfpServiceItem.CreatedBy;
            rfpServiceItemDTO.CreatedByEmployeeName = rfpServiceItem.CreatedByEmployee != null ? rfpServiceItem.CreatedByEmployee.FirstName + " " + rfpServiceItem.CreatedByEmployee.LastName : string.Empty;
            rfpServiceItemDTO.LastModifiedByEmployeeName = rfpServiceItem.LastModifiedByEmployee != null ? rfpServiceItem.LastModifiedByEmployee.FirstName + " " + rfpServiceItem.LastModifiedByEmployee.LastName : (rfpServiceItem.CreatedByEmployee != null ? rfpServiceItem.CreatedByEmployee.FirstName + " " + rfpServiceItem.CreatedByEmployee.LastName : string.Empty);
            rfpServiceItemDTO.CreatedDate = rfpServiceItem.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceItem.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpServiceItem.CreatedDate;
            rfpServiceItemDTO.LastModifiedDate = rfpServiceItem.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceItem.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpServiceItem.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceItem.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpServiceItem.CreatedDate);
            rfpServiceItemDTO.IsActive = rfpServiceItem.IsActive;
            rfpServiceItemDTO.Level = rfpServiceItem.Level;
            rfpServiceItemDTO.ServiceDescription = rfpServiceItem.ServiceDescription;
            rfpServiceItemDTO.AppendWorkDescription = rfpServiceItem.AppendWorkDescription;
            rfpServiceItemDTO.CustomServiceDescription = rfpServiceItem.CustomServiceDescription;
            rfpServiceItemDTO.AdditionalUnitPrice = rfpServiceItem.AdditionalUnitPrice;
            rfpServiceItemDTO.Cost = rfpServiceItem.Cost;
            rfpServiceItemDTO.FormattedCost = rfpServiceItem.Cost != null ? ((double)rfpServiceItem.Cost).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty;
            rfpServiceItemDTO.IsShowScope = rfpServiceItem.IsShowScope;
            rfpServiceItemDTO.PartOf = rfpServiceItem.PartOf;

            //rfpServiceItemDTO.IdCostType = Convert.ToInt32(rfpServiceItem.CostType);
            rfpServiceItemDTO.CostType = rfpServiceItem.CostType;

            rfpServiceItemDTO.RfpJobTypeCostRanges = rfpServiceItem.RfpJobTypeCostRanges.AsEnumerable().Select(x => new RfpJobTypeCostRangeDTO()
            {
                Id = x.Id,
                MaximumQuantity = x.MaximumQuantity,
                MinimumQuantity = x.MinimumQuantity,
                RangeCost = x.Cost,
                //FormattedRangeCost = x.Cost != null ? ((double)x.Cost).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                IdRfpJobType = x.IdRfpJobType
            }).ToList();

            rfpServiceItemDTO.RfpJobTypeCumulativeCosts = rfpServiceItem.RfpJobTypeCumulativeCosts.AsEnumerable().Select(x => new RfpJobTypeCumulativeCostDTO()
            {
                Id = x.Id,
                Qty = x.Quantity,
                CumulativeCost = x.Cost,
                //FormattedCumulativeCost = x.Cost != null ? ((double)x.Cost).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                IdRfpJobType = x.IdRfpJobType
            }).ToList();
            return rfpServiceItemDTO;
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="rfpServiceItem">Type of the RFP job.</param>
        /// <returns>RfpServiceItemDetail.</returns>
        private RfpServiceItemDetail FormatDetails(RfpJobType rfpServiceItem)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            RfpServiceItemDetail rfpServiceItemDetail = new RfpServiceItemDetail();
            rfpServiceItemDetail.Id = rfpServiceItem.Id;
            rfpServiceItemDetail.Name = rfpServiceItem.Name;

            if (rfpServiceItem.Parent != null && rfpServiceItem.Parent.Level == 4)
            {
                rfpServiceItemDetail.IdRfpServiceGroup = rfpServiceItem.IdParent;
                rfpServiceItemDetail.RfpServiceGroup = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.Name : string.Empty;

                if (rfpServiceItem.Parent.Parent != null && rfpServiceItem.Parent.Parent.Level == 3)
                {
                    rfpServiceItemDetail.IdRfpSubJobType = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDetail.RfpSubJobType = rfpServiceItem.Parent != null && rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;

                    if (rfpServiceItem.Parent.Parent.Parent != null && rfpServiceItem.Parent.Parent.Parent.Level == 2)
                    {
                        rfpServiceItemDetail.IdRfpSubJobTypeCategory = rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.IdParent : 0;
                        rfpServiceItemDetail.RfpSubJobTypeCategory = rfpServiceItem.Parent.Parent.Parent != null && rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.Name : string.Empty;

                        if (rfpServiceItem.Parent.Parent.Parent.Parent != null)
                        {
                            rfpServiceItemDetail.IdRfpJobType = rfpServiceItem.Parent.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.IdParent : 0;
                            rfpServiceItemDetail.RfpJobType = rfpServiceItem.Parent.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (rfpServiceItem.Parent.Parent.Parent != null && rfpServiceItem.Parent.Parent.Parent.Level == 1)
                    {
                        rfpServiceItemDetail.IdRfpSubJobTypeCategory = 0;
                        rfpServiceItemDetail.RfpSubJobTypeCategory = string.Empty;

                        rfpServiceItemDetail.IdRfpJobType = rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.IdParent : 0;
                        rfpServiceItemDetail.RfpJobType = rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (rfpServiceItem.Parent.Parent != null && rfpServiceItem.Parent.Parent.Level == 2)
                {
                    rfpServiceItemDetail.IdRfpSubJobType = 0;
                    rfpServiceItemDetail.RfpSubJobType = string.Empty;

                    rfpServiceItemDetail.IdRfpSubJobTypeCategory = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDetail.RfpSubJobTypeCategory = rfpServiceItem.Parent != null && rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;

                    if (rfpServiceItem.Parent.Parent.Parent != null)
                    {
                        rfpServiceItemDetail.IdRfpJobType = rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.IdParent : 0;
                        rfpServiceItemDetail.RfpJobType = rfpServiceItem.Parent.Parent.Parent != null && rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (rfpServiceItem.Parent.Parent != null && rfpServiceItem.Parent.Parent.Level == 1)
                {
                    rfpServiceItemDetail.IdRfpSubJobType = 0;
                    rfpServiceItemDetail.RfpSubJobType = string.Empty;

                    rfpServiceItemDetail.IdRfpSubJobTypeCategory = 0;
                    rfpServiceItemDetail.RfpSubJobTypeCategory = string.Empty;

                    rfpServiceItemDetail.IdRfpJobType = rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDetail.RfpJobType = rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpServiceItem.Parent != null && rfpServiceItem.Parent.Level == 3)
            {
                rfpServiceItemDetail.IdRfpServiceGroup = 0;
                rfpServiceItemDetail.RfpServiceGroup = string.Empty;

                rfpServiceItemDetail.IdRfpSubJobType = rfpServiceItem.IdParent;
                rfpServiceItemDetail.RfpSubJobType = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.Name : string.Empty;

                if (rfpServiceItem.Parent.Parent != null && rfpServiceItem.Parent.Parent.Level == 2)
                {
                    rfpServiceItemDetail.IdRfpSubJobTypeCategory = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDetail.RfpSubJobTypeCategory = rfpServiceItem.Parent != null && rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;

                    if (rfpServiceItem.Parent.Parent.Parent != null)
                    {
                        rfpServiceItemDetail.IdRfpJobType = rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.IdParent : 0;
                        rfpServiceItemDetail.RfpJobType = rfpServiceItem.Parent.Parent.Parent != null && rfpServiceItem.Parent.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (rfpServiceItem.Parent.Parent != null && rfpServiceItem.Parent.Parent.Level == 1)
                {
                    rfpServiceItemDetail.IdRfpSubJobTypeCategory = 0;
                    rfpServiceItemDetail.RfpSubJobTypeCategory = string.Empty;

                    rfpServiceItemDetail.IdRfpJobType = rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDetail.RfpJobType = rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpServiceItem.Parent != null && rfpServiceItem.Parent.Level == 2)
            {
                rfpServiceItemDetail.IdRfpServiceGroup = 0;
                rfpServiceItemDetail.RfpServiceGroup = string.Empty;

                rfpServiceItemDetail.IdRfpSubJobType = 0;
                rfpServiceItemDetail.RfpSubJobType = string.Empty;

                rfpServiceItemDetail.IdRfpSubJobTypeCategory = rfpServiceItem.IdParent;
                rfpServiceItemDetail.RfpSubJobTypeCategory = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.Name : string.Empty;

                if (rfpServiceItem.Parent.Parent != null)
                {
                    rfpServiceItemDetail.IdRfpJobType = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.IdParent : 0;
                    rfpServiceItemDetail.RfpJobType = rfpServiceItem.Parent != null && rfpServiceItem.Parent.Parent != null ? rfpServiceItem.Parent.Parent.Name : string.Empty;
                }
            }
            else if (rfpServiceItem.Parent != null && rfpServiceItem.Parent.Level == 1)
            {

                rfpServiceItemDetail.IdRfpJobType = rfpServiceItem.IdParent;
                rfpServiceItemDetail.RfpJobType = rfpServiceItem.Parent != null ? rfpServiceItem.Parent.Name : string.Empty;

                rfpServiceItemDetail.IdRfpSubJobTypeCategory = 0;
                rfpServiceItemDetail.RfpSubJobTypeCategory = string.Empty;

                rfpServiceItemDetail.IdRfpSubJobType = 0;
                rfpServiceItemDetail.RfpSubJobType = string.Empty;

                rfpServiceItemDetail.IdRfpServiceGroup = 0;
                rfpServiceItemDetail.RfpServiceGroup = string.Empty;
            }

            rfpServiceItemDetail.CreatedBy = rfpServiceItem.CreatedBy;
            rfpServiceItemDetail.LastModifiedBy = rfpServiceItem.LastModifiedBy != null ? rfpServiceItem.LastModifiedBy : rfpServiceItem.CreatedBy;
            rfpServiceItemDetail.CreatedByEmployeeName = rfpServiceItem.CreatedByEmployee != null ? rfpServiceItem.CreatedByEmployee.FirstName + " " + rfpServiceItem.CreatedByEmployee.LastName : string.Empty;
            rfpServiceItemDetail.LastModifiedByEmployeeName = rfpServiceItem.LastModifiedByEmployee != null ? rfpServiceItem.LastModifiedByEmployee.FirstName + " " + rfpServiceItem.LastModifiedByEmployee.LastName : (rfpServiceItem.CreatedByEmployee != null ? rfpServiceItem.CreatedByEmployee.FirstName + " " + rfpServiceItem.CreatedByEmployee.LastName : string.Empty);
            rfpServiceItemDetail.CreatedDate = rfpServiceItem.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceItem.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpServiceItem.CreatedDate;
            rfpServiceItemDetail.LastModifiedDate = rfpServiceItem.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceItem.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpServiceItem.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpServiceItem.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpServiceItem.CreatedDate);
            rfpServiceItemDetail.IsActive = rfpServiceItem.IsActive;
            rfpServiceItemDetail.Level = rfpServiceItem.Level;
            rfpServiceItemDetail.ServiceDescription = rfpServiceItem.ServiceDescription;
            rfpServiceItemDetail.AppendWorkDescription = rfpServiceItem.AppendWorkDescription;
            rfpServiceItemDetail.CustomServiceDescription = rfpServiceItem.CustomServiceDescription;
            rfpServiceItemDetail.AdditionalUnitPrice = rfpServiceItem.AdditionalUnitPrice;
            rfpServiceItemDetail.Cost = rfpServiceItem.Cost;
            rfpServiceItemDetail.IsShowScope = rfpServiceItem.IsShowScope;
            rfpServiceItemDetail.PartOf = rfpServiceItem.PartOf;
            //rfpServiceItemDetail.IdCostType = Convert.ToInt32(rfpServiceItem.CostType);
            rfpServiceItemDetail.CostType = rfpServiceItem.CostType;
            rfpServiceItemDetail.RfpJobTypeCostRanges = rfpServiceItem.RfpJobTypeCostRanges.AsEnumerable().Select(x => new RfpJobTypeCostRangeDTO()
            {
                Id = x.Id,
                MaximumQuantity = x.MaximumQuantity,
                MinimumQuantity = x.MinimumQuantity,
                RangeCost = x.Cost,
                IdRfpJobType = x.IdRfpJobType
            }).ToList();
            rfpServiceItemDetail.RfpJobTypeCumulativeCosts = rfpServiceItem.RfpJobTypeCumulativeCosts.AsEnumerable().Select(x => new RfpJobTypeCumulativeCostDTO()
            {
                Id = x.Id,
                Qty = x.Quantity,
                CumulativeCost = x.Cost,
                IdRfpJobType = x.IdRfpJobType
            }).ToList();

            return rfpServiceItemDetail;
        }

        private ServiceItemDTO FormatServiceDetails(RfpJobType rfpJobTypeDetails)
        {
            string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

            ServiceItemDTO serviceItemDTO = new ServiceItemDTO();
            serviceItemDTO.Id = rfpJobTypeDetails.Id;
            serviceItemDTO.CostType = rfpJobTypeDetails.CostType;
            serviceItemDTO.ItemName = Common.GetServiceItemName(rfpJobTypeDetails);
            return serviceItemDTO;

            //string rfpServiceItem = rfpJobTypeDetails != null ? rfpJobTypeDetails.Name : string.Empty;
            //string rfpServiceGroup = string.Empty;
            //string rfpSubJobType = string.Empty;
            //string rfpSubJobTypeCategory = string.Empty;
            //string rfpJobType = string.Empty;

            //if (rfpJobTypeDetails.Parent != null && rfpJobTypeDetails.Parent.Level == 4)
            //{
            //    rfpServiceGroup = rfpJobTypeDetails.Parent != null ? rfpJobTypeDetails.Parent.Name : string.Empty;

            //    if (rfpJobTypeDetails.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Level == 3)
            //    {
            //        rfpSubJobType = rfpJobTypeDetails.Parent != null && rfpJobTypeDetails.Parent.Parent != null ? rfpJobTypeDetails.Parent.Parent.Name : string.Empty;

            //        if (rfpJobTypeDetails.Parent.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Parent.Level == 2)
            //        {
            //            rfpSubJobTypeCategory = rfpJobTypeDetails.Parent.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Parent != null ? rfpJobTypeDetails.Parent.Parent.Parent.Name : string.Empty;

            //            if (rfpJobTypeDetails.Parent.Parent.Parent.Parent != null)
            //            {
            //                rfpJobType = rfpJobTypeDetails.Parent.Parent.Parent.Parent != null ? rfpJobTypeDetails.Parent.Parent.Parent.Parent.Name : string.Empty;
            //            }
            //        }
            //        else if (rfpJobTypeDetails.Parent.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Parent.Level == 1)
            //        {
            //            rfpSubJobTypeCategory = string.Empty;
            //            rfpJobType = rfpJobTypeDetails.Parent.Parent.Parent != null ? rfpJobTypeDetails.Parent.Parent.Parent.Name : string.Empty;
            //        }
            //    }
            //    else if (rfpJobTypeDetails.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Level == 2)
            //    {
            //        rfpSubJobType = string.Empty;

            //        rfpSubJobTypeCategory = rfpJobTypeDetails.Parent != null && rfpJobTypeDetails.Parent.Parent != null ? rfpJobTypeDetails.Parent.Parent.Name : string.Empty;

            //        if (rfpJobTypeDetails.Parent.Parent.Parent != null)
            //        {
            //            rfpJobType = rfpJobTypeDetails.Parent.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Parent != null ? rfpJobTypeDetails.Parent.Parent.Parent.Name : string.Empty;
            //        }
            //    }
            //    else if (rfpJobTypeDetails.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Level == 1)
            //    {
            //        rfpSubJobType = string.Empty;

            //        rfpSubJobTypeCategory = string.Empty;

            //        rfpJobType = rfpJobTypeDetails.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Parent != null ? rfpJobTypeDetails.Parent.Parent.Parent.Name : string.Empty;
            //    }
            //}
            //else if (rfpJobTypeDetails.Parent != null && rfpJobTypeDetails.Parent.Level == 3)
            //{
            //    rfpServiceGroup = string.Empty;

            //    rfpSubJobType = rfpJobTypeDetails.Parent != null ? rfpJobTypeDetails.Parent.Name : string.Empty;

            //    if (rfpJobTypeDetails.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Level == 2)
            //    {
            //        rfpSubJobTypeCategory = rfpJobTypeDetails.Parent != null && rfpJobTypeDetails.Parent.Parent != null ? rfpJobTypeDetails.Parent.Parent.Name : string.Empty;

            //        if (rfpJobTypeDetails.Parent.Parent.Parent != null)
            //        {
            //            rfpJobType = rfpJobTypeDetails.Parent.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Parent != null ? rfpJobTypeDetails.Parent.Parent.Parent.Name : string.Empty;
            //        }
            //    }
            //    else if (rfpJobTypeDetails.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Level == 1)
            //    {
            //        rfpSubJobTypeCategory = string.Empty;

            //        rfpJobType = rfpJobTypeDetails.Parent.Parent != null && rfpJobTypeDetails.Parent.Parent.Parent != null ? rfpJobTypeDetails.Parent.Parent.Parent.Name : string.Empty;
            //    }
            //}
            //else if (rfpJobTypeDetails.Parent != null && rfpJobTypeDetails.Parent.Level == 2)
            //{
            //    rfpServiceGroup = string.Empty;

            //    rfpSubJobType = string.Empty;

            //    rfpSubJobTypeCategory = rfpJobTypeDetails.Parent != null ? rfpJobTypeDetails.Parent.Name : string.Empty;

            //    if (rfpJobTypeDetails.Parent.Parent != null)
            //    {
            //        rfpJobType = rfpJobTypeDetails.Parent != null && rfpJobTypeDetails.Parent.Parent != null ? rfpJobTypeDetails.Parent.Parent.Name : string.Empty;
            //    }
            //}
            //else if (rfpJobTypeDetails.Parent != null && rfpJobTypeDetails.Parent.Level == 1)
            //{

            //    rfpJobType = rfpJobTypeDetails.Parent != null ? rfpJobTypeDetails.Parent.Name : string.Empty;

            //    rfpSubJobTypeCategory = string.Empty;

            //    rfpSubJobType = string.Empty;

            //    rfpServiceGroup = string.Empty;
            //}


            //serviceItemDTO.ItemName = (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);


        }

        /// <summary>
        /// RFPs the job type name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpServiceItemNameExists(string name, int id)
        {
            return rpoContext.RfpJobTypes.Count(e => e.Name == name && e.Id != id) > 0;
        }
    }
}