// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-21-2018
// ***********************************************************************
// <copyright file="RfpFeeSchedulesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Fee Schedules Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.RfpFeeSchedules
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using System.Collections.Generic;
    /// <summary>
    /// Class Rfp Fee Schedules Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />

    public class RfpFeeSchedulesController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        public class RfpFeeScheduleSearchParameters : DataTableParameters
        {
            public int IdRfp { get; set; }

        }

        /// <summary>
        /// Gets the RFP fee schedules List.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the RFP fee schedules List.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetRfpFeeSchedules([FromUri] RfpFeeScheduleSearchParameters dataTableParameters)
        {
            var rfpFeeSchedules = rpoContext.RfpFeeSchedules
                .Include("RfpWorkType")
                .Include("RfpWorkTypeCategory")
                .Include("ProjectDetail.RfpSubJobType")
                .Include("ProjectDetail.RfpJobType")
                .Include("ProjectDetail.RfpSubJobTypeCategory")
                .Where(x => x.IdRfp == dataTableParameters.IdRfp);
            var recordsTotal = rfpFeeSchedules.Count();
            var recordsFiltered = recordsTotal;

            var result = rfpFeeSchedules
                .AsEnumerable()
                .Select(rf => Format(rf))
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
        /// Gets the RFP fee schedule in list for dropdown.
        /// </summary>
        /// <param name="idRfp">The identifier RFP.</param>
        /// <returns> Gets the RFP fee schedule in against RFPs.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/rfpfeeschedules/{idRfp}/dropdown")]
        public IHttpActionResult GetRfpFeeSchedule(int idRfp)
        {
            var recordsTotal = rpoContext.RfpFeeSchedules.Count();
            var recordsFiltered = recordsTotal;

            var result = rpoContext.RfpFeeSchedules
                .Include("RfpWorkType")
                .Include("RfpWorkTypeCategory")
                .Include("ProjectDetail.RfpSubJobType")
                .Include("ProjectDetail.RfpJobType")
                .Include("ProjectDetail.RfpSubJobTypeCategory")
                .Where(x => x.IdRfp == idRfp && x.RfpWorkType.IsShowScope==false)
                .AsEnumerable()
                .Select(rf => FormatDetail(rf))
                .ToArray();

            List<RfpFeeScheduleDetail> rfpFeeScheduleList = new List<RfpFeeScheduleDetail>();
            foreach (var item in result)
            {
                RfpFeeScheduleDetail objDetail = new RfpFeeScheduleDetail();
                if (item.IdPartof == null)
                {
                    objDetail.IdRfpFeeSchedule = item.IdRfpFeeSchedule;
                    string stringJoin = string.Empty;
                    var tmpResult= (from d in result where d.IdPartof == item.IdRFPWorkType select d.RFPName).ToList(); 

                    foreach (var itemname in tmpResult)
                    {
                        stringJoin = stringJoin +" "+ itemname + ",";
                    }
                    if (!string.IsNullOrEmpty(stringJoin))
                    {
                        stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                        objDetail.ItemName = item.ItemName + " (" + stringJoin.Trim() + ")";
                    }
                    else
                    {
                        objDetail.ItemName = item.ItemName;
                    }
                    objDetail.IdRFPWorkType = item.IdRFPWorkType;
                    rfpFeeScheduleList.Add(objDetail);
                }
            }


            return Ok(rfpFeeScheduleList);
        }

        /// <summary>
        /// Formats the specified RFP fee schedule.
        /// </summary>
        /// <param name="rfpFeeSchedule">The RFP fee schedule.</param>
        /// <returns>RfpFeeScheduleDTO.</returns>
        private RfpFeeScheduleDTO Format(RfpFeeSchedule rfpFeeSchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

            RfpFeeScheduleDTO rfpServiceItemDTO = new RfpFeeScheduleDTO();
            rfpServiceItemDTO.Id = rfpFeeSchedule.Id;
            rfpServiceItemDTO.IdProjectDetail = rfpFeeSchedule.IdProjectDetail;

            rfpServiceItemDTO.Cost = rfpFeeSchedule.Cost != null ? Convert.ToDouble(rfpFeeSchedule.Cost).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty;
            rfpServiceItemDTO.Quantity = rfpFeeSchedule.Quantity;
            rfpServiceItemDTO.TotalCost = rfpFeeSchedule.TotalCost;
            rfpServiceItemDTO.FormatedTotalCost = rfpFeeSchedule.TotalCost != null ? Convert.ToDouble(rfpFeeSchedule.TotalCost).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty;

            rfpServiceItemDTO.IdRfpServiceItem = rfpFeeSchedule.IdRfpWorkType;
            rfpServiceItemDTO.RfpServiceItem = rfpFeeSchedule.RfpWorkType != null ? rfpFeeSchedule.RfpWorkType.Name : string.Empty;

            rfpServiceItemDTO.IdRfpServiceGroup = rfpFeeSchedule.IdRfpWorkTypeCategory;
            rfpServiceItemDTO.RfpServiceGroup = rfpFeeSchedule.RfpWorkTypeCategory != null ? rfpFeeSchedule.RfpWorkTypeCategory.Name : string.Empty;

            rfpServiceItemDTO.IdRfpSubJobTypeCategory = rfpFeeSchedule.ProjectDetail != null ? rfpFeeSchedule.ProjectDetail.IdRfpSubJobTypeCategory : 0;
            rfpServiceItemDTO.RfpSubJobTypeCategory = rfpFeeSchedule.ProjectDetail != null && rfpFeeSchedule.ProjectDetail.RfpSubJobTypeCategory != null ? rfpFeeSchedule.ProjectDetail.RfpSubJobTypeCategory.Name : string.Empty;

            rfpServiceItemDTO.IdRfpSubJobType = rfpFeeSchedule.ProjectDetail != null ? rfpFeeSchedule.ProjectDetail.IdRfpSubJobType : 0;
            rfpServiceItemDTO.RfpSubJobType = rfpFeeSchedule.ProjectDetail != null && rfpFeeSchedule.ProjectDetail.RfpSubJobType != null ? rfpFeeSchedule.ProjectDetail.RfpSubJobType.Name : string.Empty;

            rfpServiceItemDTO.IdRfpJobType = rfpFeeSchedule.ProjectDetail != null ? rfpFeeSchedule.ProjectDetail.IdRfpJobType : 0;
            rfpServiceItemDTO.RfpJobType = rfpFeeSchedule.ProjectDetail != null && rfpFeeSchedule.ProjectDetail.RfpJobType != null ? rfpFeeSchedule.ProjectDetail.RfpJobType.Name : string.Empty;

            return rfpServiceItemDTO;
        }

        private RfpFeeScheduleDetail FormatDetail(RfpFeeSchedule rfpFeeSchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

            RfpFeeScheduleDetail rfpFeeScheduleDetail = new RfpFeeScheduleDetail();
            rfpFeeScheduleDetail.IdRfpFeeSchedule = rfpFeeSchedule.Id;
            rfpFeeScheduleDetail.IdPartof = rfpFeeSchedule.RfpWorkType.PartOf;
            rfpFeeScheduleDetail.IdRFPWorkType = rfpFeeSchedule.RfpWorkType.Id;
            rfpFeeScheduleDetail.RFPName = rfpFeeSchedule.RfpWorkType.Name;

            string rfpServiceItem = rfpFeeSchedule.RfpWorkType != null ? rfpFeeSchedule.RfpWorkType.Name : string.Empty;
            string rfpServiceGroup = string.Empty;
            string rfpSubJobType = string.Empty;
            string rfpSubJobTypeCategory = string.Empty;
            string rfpJobType = string.Empty;

            rfpServiceItem = rfpFeeSchedule.RfpWorkType != null ? rfpFeeSchedule.RfpWorkType.Name : string.Empty;
            rfpServiceGroup = rfpFeeSchedule.RfpWorkTypeCategory != null ? rfpFeeSchedule.RfpWorkTypeCategory.Name : string.Empty;
            rfpSubJobTypeCategory = rfpFeeSchedule.ProjectDetail != null && rfpFeeSchedule.ProjectDetail.RfpSubJobTypeCategory != null ? rfpFeeSchedule.ProjectDetail.RfpSubJobTypeCategory.Name : string.Empty;
            rfpSubJobType = rfpFeeSchedule.ProjectDetail != null && rfpFeeSchedule.ProjectDetail.RfpSubJobType != null ? rfpFeeSchedule.ProjectDetail.RfpSubJobType.Name : string.Empty;
            rfpJobType = rfpFeeSchedule.ProjectDetail != null && rfpFeeSchedule.ProjectDetail.RfpJobType != null ? rfpFeeSchedule.ProjectDetail.RfpJobType.Name : string.Empty;

            rfpFeeScheduleDetail.ItemName = (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);

            return rfpFeeScheduleDetail;
        }

        //private RfpFeeScheduleDTO Format(RfpFeeSchedule rfpFeeSchedule)
        //{
        //    string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

        //    RfpFeeScheduleDTO rfpServiceItemDTO = new RfpFeeScheduleDTO();
        //    rfpServiceItemDTO.Id = rfpFeeSchedule.Id;
        //    rfpServiceItemDTO.IdProjectDetail = rfpFeeSchedule.IdProjectDetail;
        //    rfpServiceItemDTO.IdRfpServiceItem = rfpFeeSchedule.IdRfpWorkType;
        //    rfpServiceItemDTO.RfpServiceItem = rfpFeeSchedule.RfpWorkType != null ? rfpFeeSchedule.RfpWorkType.Name : string.Empty;
        //    rfpServiceItemDTO.Cost = rfpFeeSchedule.Cost;
        //    rfpServiceItemDTO.Quantity = rfpFeeSchedule.Quantity;
        //    rfpServiceItemDTO.TotalCost = rfpFeeSchedule.TotalCost;

        //    if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 4)
        //    {
        //        rfpServiceItemDTO.IdRfpServiceGroup = rfpFeeSchedule.RfpWorkType.IdParent;
        //        rfpServiceItemDTO.RfpServiceGroup = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

        //        if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 3)
        //        {
        //            rfpServiceItemDTO.IdRfpSubJobType = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.IdParent : 0;
        //            rfpServiceItemDTO.RfpSubJobType = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

        //            if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 2)
        //            {
        //                rfpServiceItemDTO.IdRfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.IdParent : 0;
        //                rfpServiceItemDTO.RfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;

        //                if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null)
        //                {
        //                    rfpServiceItemDTO.IdRfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.IdParent : 0;
        //                    rfpServiceItemDTO.RfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent.Name : string.Empty;
        //                }
        //            }
        //            else if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 1)
        //            {
        //                rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
        //                rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

        //                rfpServiceItemDTO.IdRfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.IdParent : 0;
        //                rfpServiceItemDTO.RfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
        //            }
        //        }
        //        else if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
        //        {
        //            rfpServiceItemDTO.IdRfpSubJobType = 0;
        //            rfpServiceItemDTO.RfpSubJobType = string.Empty;

        //            rfpServiceItemDTO.IdRfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.IdParent : 0;
        //            rfpServiceItemDTO.RfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

        //            if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
        //            {
        //                rfpServiceItemDTO.IdRfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.IdParent : 0;
        //                rfpServiceItemDTO.RfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
        //            }
        //        }
        //        else if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
        //        {
        //            rfpServiceItemDTO.IdRfpSubJobType = 0;
        //            rfpServiceItemDTO.RfpSubJobType = string.Empty;

        //            rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
        //            rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

        //            rfpServiceItemDTO.IdRfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.IdParent : 0;
        //            rfpServiceItemDTO.RfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
        //        }
        //    }
        //    else if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 3)
        //    {
        //        rfpServiceItemDTO.IdRfpServiceGroup = 0;
        //        rfpServiceItemDTO.RfpServiceGroup = string.Empty;

        //        rfpServiceItemDTO.IdRfpSubJobType = rfpFeeSchedule.RfpWorkType.IdParent;
        //        rfpServiceItemDTO.RfpSubJobType = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

        //        if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
        //        {
        //            rfpServiceItemDTO.IdRfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.IdParent : 0;
        //            rfpServiceItemDTO.RfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

        //            if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
        //            {
        //                rfpServiceItemDTO.IdRfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.IdParent : 0;
        //                rfpServiceItemDTO.RfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
        //            }
        //        }
        //        else if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
        //        {
        //            rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
        //            rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

        //            rfpServiceItemDTO.IdRfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.IdParent : 0;
        //            rfpServiceItemDTO.RfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
        //        }
        //    }
        //    else if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 2)
        //    {
        //        rfpServiceItemDTO.IdRfpServiceGroup = 0;
        //        rfpServiceItemDTO.RfpServiceGroup = string.Empty;

        //        rfpServiceItemDTO.IdRfpSubJobType = 0;
        //        rfpServiceItemDTO.RfpSubJobType = string.Empty;

        //        rfpServiceItemDTO.IdRfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.IdParent;
        //        rfpServiceItemDTO.RfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

        //        if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null)
        //        {
        //            rfpServiceItemDTO.IdRfpJobType = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.IdParent : 0;
        //            rfpServiceItemDTO.RfpJobType = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
        //        }
        //    }
        //    else if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 1)
        //    {

        //        rfpServiceItemDTO.IdRfpJobType = rfpFeeSchedule.RfpWorkType.IdParent;
        //        rfpServiceItemDTO.RfpJobType = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

        //        rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
        //        rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

        //        rfpServiceItemDTO.IdRfpSubJobType = 0;
        //        rfpServiceItemDTO.RfpSubJobType = string.Empty;

        //        rfpServiceItemDTO.IdRfpServiceGroup = 0;
        //        rfpServiceItemDTO.RfpServiceGroup = string.Empty;
        //    }


        //    return rfpServiceItemDTO;
        //}


        //private RfpFeeScheduleDetail FormatDetail(RfpFeeSchedule rfpFeeSchedule)
        //{
        //    string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

        //    RfpFeeScheduleDetail rfpFeeScheduleDetail = new RfpFeeScheduleDetail();
        //    rfpFeeScheduleDetail.IdRfpFeeSchedule = rfpFeeSchedule.Id;

        //    string rfpServiceItem = rfpFeeSchedule.RfpWorkType != null ? rfpFeeSchedule.RfpWorkType.Name : string.Empty;
        //    string rfpServiceGroup = string.Empty;
        //    string rfpSubJobType = string.Empty;
        //    string rfpSubJobTypeCategory = string.Empty;
        //    string rfpJobType = string.Empty;

        //    if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 4)
        //    {
        //        rfpServiceGroup = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

        //        if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 3)
        //        {
        //            rfpSubJobType = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

        //            if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 2)
        //            {
        //                rfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;

        //                if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null)
        //                {
        //                    rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent.Name : string.Empty;
        //                }
        //            }
        //            else if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 1)
        //            {
        //                rfpSubJobTypeCategory = string.Empty;
        //                rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
        //            }
        //        }
        //        else if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
        //        {
        //            rfpSubJobType = string.Empty;

        //            rfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

        //            if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
        //            {
        //                rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
        //            }
        //        }
        //        else if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
        //        {
        //            rfpSubJobType = string.Empty;

        //            rfpSubJobTypeCategory = string.Empty;

        //            rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
        //        }
        //    }
        //    else if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 3)
        //    {
        //        rfpServiceGroup = string.Empty;

        //        rfpSubJobType = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

        //        if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
        //        {
        //            rfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

        //            if (rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
        //            {
        //                rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
        //            }
        //        }
        //        else if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
        //        {
        //            rfpSubJobTypeCategory = string.Empty;

        //            rfpJobType = rfpFeeSchedule.RfpWorkType.Parent.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
        //        }
        //    }
        //    else if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 2)
        //    {
        //        rfpServiceGroup = string.Empty;

        //        rfpSubJobType = string.Empty;

        //        rfpSubJobTypeCategory = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

        //        if (rfpFeeSchedule.RfpWorkType.Parent.Parent != null)
        //        {
        //            rfpJobType = rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
        //        }
        //    }
        //    else if (rfpFeeSchedule.RfpWorkType.Parent != null && rfpFeeSchedule.RfpWorkType.Parent.Level == 1)
        //    {

        //        rfpJobType = rfpFeeSchedule.RfpWorkType.Parent != null ? rfpFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

        //        rfpSubJobTypeCategory = string.Empty;

        //        rfpSubJobType = string.Empty;

        //        rfpServiceGroup = string.Empty;
        //    }


        //    rfpFeeScheduleDetail.ItemName = (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
        //        (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
        //        (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
        //        (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
        //        (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);

        //    return rfpFeeScheduleDetail;
        //}

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
    }
}