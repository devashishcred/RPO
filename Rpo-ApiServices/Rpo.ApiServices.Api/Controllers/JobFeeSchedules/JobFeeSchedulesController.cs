// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-13-2018
// ***********************************************************************
// <copyright file="JobFeeSchedulesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Fee Schedules Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Job Fee Schedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobFeeSchedules
{
    using System;
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
    using System.IO;
    using System.Web;
    using Hubs;
    using SystemSettings;
    using System.Globalization;
    using JobMilestones;    /// <summary>
                            /// Class Job Fee Schedules Controller.
                            /// </summary>
                            /// 

    public class JobFeeSchedulesController : HubApiController<GroupHub>
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the job fee schedules.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the scope List.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetJobFeeSchedules([FromUri] JobFeeScheduleSearchParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJobScope)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobScope)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJobScope))
            {
                var recordsTotal = rpoContext.JobFeeSchedules.Count();
                var recordsFiltered = recordsTotal;

                string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

                List<JobMilestone> jobMilestone = rpoContext.JobMilestones.Include("JobMilestoneServices").Include("ModifiedByEmployee").Where(j => j.IdJob == dataTableParameters.IdJob).ToList();

                //var jobMilestone = rpoContext.JobMilestones.Include("JobMilestoneServices.JobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent").Include("ModifiedByEmployee").Where(j => j.IdJob == dataTableParameters.IdJob).AsQueryable();

                //if (jobMilestone == null)
                //{
                //    return this.NotFound();
                //}

                //List<JobMilestoneDTO> jobMilestoneResult = jobMilestone.AsEnumerable().Select(milestone => JobMilestoneFormat(milestone)).ToList();





                List<JobFeeScheduleDTO> result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == dataTableParameters.IdJob && x.RfpWorkType.IsShowScope == false)
                    .AsEnumerable()
                    .Select(jf => Format(jf, jobMilestone))
                    .AsQueryable()
                    .DataTableParameters(dataTableParameters, out recordsFiltered)
                    .ToList();

                //// var result = jobMilestone.AsEnumerable().Select(milestone => JobMilestoneFormat(milestone));




                ////List<JobFeeScheduleDTO> result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == dataTableParameters.IdJob)
                ////    .AsEnumerable()
                ////    .Select(jf => Format(jf, jobMilestone))
                ////    .AsQueryable()
                ////    .DataTableParameters(dataTableParameters, out recordsFiltered)
                ////    .ToList();

                //int i = 0;
                List<JobMilestone> jobMilestoneEmpty = jobMilestone.Where(x => x.JobMilestoneServices == null || (x.JobMilestoneServices != null && x.JobMilestoneServices.Count == 0)).ToList();

                if (jobMilestoneEmpty != null && jobMilestoneEmpty.Count > 0)
                {
                    foreach (JobMilestone item in jobMilestoneEmpty.OrderBy(d => d.Id))
                    {
                        if (result != null && result.Count > 0)
                        {
                            JobFeeScheduleDTO jobFeeScheduleDTO = new JobFeeScheduleDTO();

                            jobFeeScheduleDTO.JobMilestoneId = item.Id;
                            jobFeeScheduleDTO.JobMilestoneName = item.Name;
                            jobFeeScheduleDTO.JobMilestoneValue = item.Value != null ? Convert.ToDouble(item.Value).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty;
                            jobFeeScheduleDTO.JobMilestoneStatus = item.Status;
                            jobFeeScheduleDTO.JobMilestoneIsInvoiced = item.IsInvoiced;
                            jobFeeScheduleDTO.JobMilestoneInvoiceNumber = item.InvoiceNumber;
                            jobFeeScheduleDTO.JobMilestoneInvoicedDate = item.InvoicedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.InvoicedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : item.InvoicedDate;
                            jobFeeScheduleDTO.JobMilestonePONumber = item.PONumber;
                            jobFeeScheduleDTO.IdRfp = item.IdRfp;
                            if (item.IdRfp != null)
                            {
                                jobFeeScheduleDTO.RfpNumber = (from d in rpoContext.Rfps where d.Id == item.IdRfp select d).FirstOrDefault() != null ? (from d in rpoContext.Rfps where d.Id == item.IdRfp select d.RfpNumber).FirstOrDefault() : string.Empty;
                            }
                            jobFeeScheduleDTO.CreatedDate = item.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : item.CreatedDate;
                            jobFeeScheduleDTO.CreatedBy = item.CreatedByEmployee != null ? (item.CreatedByEmployee.FirstName + " " + (item.CreatedByEmployee.LastName != null ? item.CreatedByEmployee.LastName : string.Empty)) : string.Empty;
                            jobFeeScheduleDTO.LastModified = item.LastModified != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.LastModified), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : item.LastModified;
                            jobFeeScheduleDTO.LastModifiedBy = item.ModifiedByEmployee != null ? (item.ModifiedByEmployee.FirstName + " " + (item.ModifiedByEmployee.LastName != null ? item.ModifiedByEmployee.LastName : string.Empty)) : string.Empty;

                            var Taskid = (from d in rpoContext.Tasks where (d.IdJob == dataTableParameters.IdJob && d.IdMilestone == item.Id && d.IdTaskStatus == 3) select d.Id).ToArray();

                            var Transmittalid = (from d in rpoContext.JobTransmittals where (d.IdJob == dataTableParameters.IdJob && Taskid.Contains(d.IdTask.Value)) select d.Id).ToArray();

                            jobFeeScheduleDTO.MilestoneIdTask = Taskid;
                            jobFeeScheduleDTO.MilestoneIdTransamittal = Transmittalid;
                            result.Add(jobFeeScheduleDTO);
                        }
                        else
                        {
                            result = new List<JobFeeScheduleDTO>();

                            JobFeeScheduleDTO jobFeeScheduleDTO = new JobFeeScheduleDTO();

                            jobFeeScheduleDTO.JobMilestoneId = item.Id;
                            jobFeeScheduleDTO.JobMilestoneName = item.Name;
                            jobFeeScheduleDTO.JobMilestoneValue = item.Value != null ? Convert.ToDouble(item.Value).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty;
                            jobFeeScheduleDTO.JobMilestoneStatus = item.Status;
                            jobFeeScheduleDTO.JobMilestoneIsInvoiced = item.IsInvoiced;
                            jobFeeScheduleDTO.JobMilestoneInvoiceNumber = item.InvoiceNumber;
                            jobFeeScheduleDTO.JobMilestoneInvoicedDate = item.InvoicedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.InvoicedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : item.InvoicedDate;
                            jobFeeScheduleDTO.JobMilestonePONumber = item.PONumber;
                            jobFeeScheduleDTO.IdRfp = item.IdRfp;
                            if (item.IdRfp != null)
                            {
                                jobFeeScheduleDTO.RfpNumber = (from d in rpoContext.Rfps where d.Id == item.IdRfp select d).FirstOrDefault() != null ? (from d in rpoContext.Rfps where d.Id == item.IdRfp select d.RfpNumber).FirstOrDefault() : string.Empty;
                            }
                            jobFeeScheduleDTO.CreatedDate = item.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : item.CreatedDate;
                            jobFeeScheduleDTO.CreatedBy = item.CreatedByEmployee != null ? (item.CreatedByEmployee.FirstName + " " + (item.CreatedByEmployee.LastName != null ? item.CreatedByEmployee.LastName : string.Empty)) : string.Empty;
                            jobFeeScheduleDTO.LastModified = item.LastModified != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.LastModified), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : item.LastModified;
                            jobFeeScheduleDTO.LastModifiedBy = item.ModifiedByEmployee != null ? (item.ModifiedByEmployee.FirstName + " " + (item.ModifiedByEmployee.LastName != null ? item.ModifiedByEmployee.LastName : string.Empty)) : string.Empty;

                            var Taskid = (from d in rpoContext.Tasks where (d.IdJob == dataTableParameters.IdJob && d.IdMilestone == item.Id && d.IdTaskStatus == 3) select d.Id).ToArray();

                            var Transmittalid = (from d in rpoContext.JobTransmittals where (d.IdJob == dataTableParameters.IdJob && Taskid.Contains(d.IdTask.Value)) select d.Id).ToArray();

                            jobFeeScheduleDTO.MilestoneIdTask = Taskid;
                            jobFeeScheduleDTO.MilestoneIdTransamittal = Transmittalid;
                            result.Add(jobFeeScheduleDTO);
                        }
                    }
                }

                List<JobMilestone> jobMilestonelst = jobMilestone.ToList();

                foreach (JobMilestone jobMilestoneid in jobMilestonelst)
                {
                    var res = result.Where(d => d.JobMilestoneId == jobMilestoneid.Id).Select(d => d).FirstOrDefault();
                    if (res != null)
                    {
                        var Taskid = (from d in rpoContext.Tasks where (d.IdJob == dataTableParameters.IdJob && d.IdMilestone == jobMilestoneid.Id && d.IdTaskStatus == 3) select d.Id).ToArray();

                        var Transmittalid = (from d in rpoContext.JobTransmittals where (d.IdJob == dataTableParameters.IdJob && Taskid.Contains(d.IdTask.Value)) select d.Id).ToArray();
                        res.MilestoneIdTask = Taskid.Length > 0 ? Taskid : null;
                        res.MilestoneIdTransamittal = Transmittalid.Length > 0 ? Transmittalid : null;
                    }
                }

                string allidtmp = string.Empty;
                List<JobFeeScheduleDTO> rfpFeeScheduleList = new List<JobFeeScheduleDTO>();
                foreach (var item in result.OrderByDescending(d => d.Partof))
                {
                    JobFeeScheduleDTO objDetail = new JobFeeScheduleDTO();

                    if (item.Partof != null)
                    {
                        // allidtmp = string.Empty;
                        allidtmp = allidtmp + "," + item.AllIds;
                        item.TotalCost = item.TotalGroupCost.ToString();
                    }
                    //if (item.IdRfp == null && item.Partof == null)
                    //{
                    //  //  allidtmp = string.Empty;
                    //}
                    string[] str = allidtmp.Split(',');

                    //foreach (var itm in str)
                    //{
                    //    if (itm == item.AllIds)
                    //    {
                    //        goto Finish;
                    //    }
                    //}
                    rfpFeeScheduleList.Add(item);
                    //Finish:
                    string st = "tmp";
                }


                List<JobFeeScheduleDTO> milestoneListWithServices = rfpFeeScheduleList.Where(d => d.JobMilestoneId != 0).OrderBy(d => d.JobMilestoneId).ToList();

                List<JobFeeScheduleDTO> AdditionalServices = rfpFeeScheduleList.Where(d => d.JobMilestoneId == 0 && d.IsAdditionalService == true && d.IsShow == true && d.IdRfp == null).OrderBy(d => d.Id).ToList();

                List<JobFeeScheduleDTO> IndividualServices = rfpFeeScheduleList.Where(d => d.JobMilestoneId == 0 && d.IsAdditionalService == false && d.IdRfp != null).OrderBy(d => d.Id).ToList();

                List<JobFeeScheduleDTO> finalList = new List<JobFeeScheduleDTO>();
                finalList.AddRange(milestoneListWithServices);
                finalList.AddRange(AdditionalServices);
                finalList.AddRange(IndividualServices);

                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    //Data = rfpFeeSchedulenewList.OrderBy(d=>d.Orderid)
                    Data = finalList
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private JobMilestoneDTO JobMilestoneFormat(JobMilestone jobMilestone)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            JobMilestoneDTO jobMilestoneDTO = new JobMilestoneDTO();
            jobMilestoneDTO.Id = jobMilestone.Id;
            jobMilestoneDTO.IdJob = jobMilestone.IdJob;
            jobMilestoneDTO.IdRfp = jobMilestone.IdRfp;
            jobMilestoneDTO.Name = jobMilestone.Name;
            jobMilestoneDTO.Value = jobMilestone.Value;
            jobMilestoneDTO.FormattedValue = jobMilestone.Value != null ? Convert.ToDouble(jobMilestone.Value).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty;
            jobMilestoneDTO.Status = jobMilestone.Status;
            jobMilestoneDTO.IsInvoiced = jobMilestone.IsInvoiced;
            jobMilestoneDTO.InvoiceNumber = jobMilestone.InvoiceNumber;
            jobMilestoneDTO.InvoicedDate = jobMilestone.InvoicedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobMilestone.InvoicedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobMilestone.InvoicedDate;
            jobMilestoneDTO.PONumber = jobMilestone.PONumber;

            jobMilestoneDTO.JobMilestoneServices = jobMilestone.JobMilestoneServices != null ? jobMilestone.JobMilestoneServices.AsEnumerable().Select(mi => new JobMilestoneServiceDetail()
            {
                IdJobFeeSchedule = mi.IdJobFeeSchedule,
                IdMilestone = mi.IdMilestone,
                Id = mi.Id,
                ItemName = mi.JobFeeSchedule != null && mi.JobFeeSchedule.RfpWorkType != null ? FormatItemDetail(mi.JobFeeSchedule) : string.Empty
            }).ToList() : null;
            jobMilestoneDTO.LastModified = jobMilestone.LastModified != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobMilestone.LastModified), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobMilestone.LastModified;
            jobMilestoneDTO.LastModifiedBy = jobMilestone.ModifiedByEmployee != null ? (jobMilestone.ModifiedByEmployee.FirstName + " " + (jobMilestone.ModifiedByEmployee.LastName != null ? jobMilestone.ModifiedByEmployee.LastName : string.Empty)) : string.Empty;

            return jobMilestoneDTO;
        }

        /// <summary>
        /// Posts the job fee schedule.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="jobFeeScheduleCreateUpdateList">The job fee schedule create update list.</param>
        /// <returns>create a new scope.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        /// <exception cref="System.Exception">Error during roolback</exception>
        [HttpPost]
        [Route("api/JobFeeSchedules/{idJob}")]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobFeeScheduleCreateUpdate))]
        public IHttpActionResult PostJobFeeSchedule(int idJob, List<JobFeeScheduleCreateUpdate> jobFeeScheduleCreateUpdateList)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobScope))
            {
                List<string> objListMessages = new List<string>();
                using (var transaction = this.rpoContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (!ModelState.IsValid)
                        {
                            return this.BadRequest(this.ModelState);
                        }

                        var job = this.rpoContext.Jobs.FirstOrDefault(r => r.Id == idJob);

                        if (job == null)
                        {
                            return this.NotFound();
                        }

                        if (jobFeeScheduleCreateUpdateList == null)
                        {
                            throw new RpoBusinessException(StaticMessages.FeeScheduleNotExistsMessage);
                        }

                        job.LastModiefiedDate = DateTime.UtcNow;

                        if (employee != null)
                        {
                            job.LastModifiedBy = employee.Id;
                        }
                        string addScope = string.Empty;
                        var serviceName = string.Empty;
                        foreach (JobFeeScheduleCreateUpdate item in jobFeeScheduleCreateUpdateList.Where(d => d.Partof == null).OrderBy(d => d.Partof))
                        {
                            #region Main
                            JobFeeSchedule rfpFeeScheduleNew = new JobFeeSchedule();
                            rfpFeeScheduleNew.IdJob = item.IdJob;
                            rfpFeeScheduleNew.IdRfpWorkType = item.IdRfpWorkType;

                            RfpCostType cstType = (from d in rpoContext.RfpJobTypes where d.Id == item.IdRfpWorkType select d.CostType).FirstOrDefault();

                            if (cstType != null && cstType.ToString() == "HourlyCost")
                            {
                                rfpFeeScheduleNew.Quantity = item.Quantity * 60;
                            }
                            else
                            {
                                rfpFeeScheduleNew.Quantity = item.Quantity;
                            }


                            RfpJobType rfpWorkType = rpoContext.RfpJobTypes.FirstOrDefault(x => x.Id == item.IdRfpWorkType);
                            if (rfpWorkType != null)
                            {
                                switch (rfpWorkType.CostType)
                                {
                                    case RfpCostType.AdditionalCostPerUnit:
                                        rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                                        rfpFeeScheduleNew.TotalCost = rfpFeeScheduleNew.Cost + ((rfpFeeScheduleNew.Quantity - 1) * rfpWorkType.AdditionalUnitPrice);
                                        break;
                                    case RfpCostType.CostForUnitRange:

                                        var perunitcost = rfpWorkType.RfpJobTypeCostRanges != null ?
                                                           rfpWorkType.RfpJobTypeCostRanges
                                                          .Where(x => x.MaximumQuantity >= rfpFeeScheduleNew.Quantity && x.MinimumQuantity <= rfpFeeScheduleNew.Quantity)
                                                          .Select(x => x.Cost).FirstOrDefault()
                                                          : 0;
                                        rfpFeeScheduleNew.Cost = perunitcost;
                                        rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);

                                        break;
                                    case RfpCostType.CummulativeCost:
                                        if (rfpFeeScheduleNew.RfpWorkType != null && rfpFeeScheduleNew.RfpWorkType.RfpJobTypeCumulativeCosts != null)
                                        {
                                            var cummulativeCostList = rfpWorkType.RfpJobTypeCumulativeCosts.Where(x => x.Quantity <= rfpFeeScheduleNew.Quantity).ToList();

                                            if (cummulativeCostList != null)
                                            {
                                                int maxQuantity = cummulativeCostList.Max(x => x.Quantity);
                                                if (maxQuantity == rfpFeeScheduleNew.Quantity)
                                                {
                                                    rfpFeeScheduleNew.Cost = 0;
                                                    rfpFeeScheduleNew.TotalCost = cummulativeCostList.Sum(x => x.Cost);
                                                }
                                                else
                                                {
                                                    int samePriceQuantity = Convert.ToInt32(rfpFeeScheduleNew.Quantity) - maxQuantity;
                                                    double maxQuantityCost = cummulativeCostList.Where(x => x.Quantity == maxQuantity).Select(x => (double)x.Cost).FirstOrDefault();
                                                    double cummulativeCost = cummulativeCostList.Sum(x => (double)x.Cost);
                                                    rfpFeeScheduleNew.Cost = 0;
                                                    rfpFeeScheduleNew.TotalCost = cummulativeCost + (samePriceQuantity * maxQuantityCost);
                                                }
                                            }
                                            else
                                            {
                                                rfpFeeScheduleNew.Cost = 0;
                                                rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);
                                            }
                                        }
                                        else
                                        {
                                            rfpFeeScheduleNew.Cost = 0;
                                            rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);
                                        }
                                        break;
                                    case RfpCostType.FixedCost:
                                        rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                                        rfpFeeScheduleNew.TotalCost = rfpFeeScheduleNew.Cost;
                                        break;
                                    case RfpCostType.MinimumCost:
                                        rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                                        rfpFeeScheduleNew.TotalCost = rfpFeeScheduleNew.Cost;
                                        break;
                                    case RfpCostType.PerUnitPrice:
                                        rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                                        rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);
                                        break;

                                    case RfpCostType.HourlyCost:
                                        rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                                        rfpFeeScheduleNew.TotalCost = ((rfpFeeScheduleNew.Quantity / 60) * rfpFeeScheduleNew.Cost);
                                        break;
                                }
                            }

                            rfpFeeScheduleNew.Description = item.Description;
                            rfpFeeScheduleNew.QuantityAchieved = 0;
                            if (cstType != null && cstType.ToString() == "HourlyCost")
                            {
                                rfpFeeScheduleNew.QuantityPending = item.Quantity * 60;
                            }
                            else
                            {
                                rfpFeeScheduleNew.QuantityPending = item.Quantity;
                            }
                            rfpFeeScheduleNew.IsAdditionalService = item.IsAdditionalService;


                            rfpFeeScheduleNew.CreatedDate = DateTime.UtcNow;
                            if (employee != null)
                            {
                                rfpFeeScheduleNew.CreatedBy = employee.Id;
                            }
                            rfpFeeScheduleNew.IsFromScope = true;
                            this.rpoContext.JobFeeSchedules.Add(rfpFeeScheduleNew);

                            serviceName += rpoContext.RfpJobTypes.Where(x => x.Id == item.IdRfpWorkType).Select(e => e.Name).FirstOrDefault() + ", ";

                            JobFeeSchedule jobFeeSchedule = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.RfpWorkType.Id == item.IdRfpWorkType);
                            string jobFeeScheduleDetailName = string.Empty;
                            if (jobFeeSchedule != null)
                            {
                                jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeSchedule);
                            }
                            else
                            {
                                jobFeeScheduleDetailName = serviceName;
                            }

                            addScope = JobHistoryMessages.AddScope
                                .Replace("##ServiceItemName##", jobFeeScheduleDetailName);

                            objListMessages.Add(addScope);

                            this.rpoContext.SaveChanges();
                            int mainid = rfpFeeScheduleNew.Id;


                            #endregion

                            foreach (JobFeeScheduleCreateUpdate itemchild in jobFeeScheduleCreateUpdateList.Where(d => d.Partof == item.IdRfpWorkType))
                            {
                                #region Child
                                JobFeeSchedule rfpFeeScheduleNewchd = new JobFeeSchedule();
                                rfpFeeScheduleNewchd.IdJob = itemchild.IdJob;
                                rfpFeeScheduleNewchd.IdRfpWorkType = itemchild.IdRfpWorkType;
                                rfpFeeScheduleNewchd.IdParentof = mainid;

                                RfpCostType cstTypechild = (from d in rpoContext.RfpJobTypes where d.Id == itemchild.IdRfpWorkType select d.CostType).FirstOrDefault();

                                if (cstTypechild != null && cstTypechild.ToString() == "HourlyCost")
                                {
                                    rfpFeeScheduleNewchd.Quantity = itemchild.Quantity * 60;
                                }
                                else
                                {
                                    rfpFeeScheduleNewchd.Quantity = itemchild.Quantity;
                                }


                                RfpJobType rfpWorkTypechild = rpoContext.RfpJobTypes.FirstOrDefault(x => x.Id == itemchild.IdRfpWorkType);
                                if (rfpWorkTypechild != null)
                                {
                                    switch (rfpWorkTypechild.CostType)
                                    {
                                        case RfpCostType.AdditionalCostPerUnit:
                                            rfpFeeScheduleNewchd.Cost = rfpWorkTypechild.Cost;
                                            rfpFeeScheduleNewchd.TotalCost = rfpFeeScheduleNewchd.Cost + ((rfpFeeScheduleNewchd.Quantity - 1) * rfpWorkTypechild.AdditionalUnitPrice);
                                            break;
                                        case RfpCostType.CostForUnitRange:

                                            var perunitcost = rfpWorkTypechild.RfpJobTypeCostRanges != null ?
                                                               rfpWorkTypechild.RfpJobTypeCostRanges
                                                              .Where(x => x.MaximumQuantity >= rfpFeeScheduleNewchd.Quantity && x.MinimumQuantity <= rfpFeeScheduleNewchd.Quantity)
                                                              .Select(x => x.Cost).FirstOrDefault()
                                                              : 0;
                                            rfpFeeScheduleNewchd.Cost = perunitcost;
                                            rfpFeeScheduleNewchd.TotalCost = (rfpFeeScheduleNewchd.Quantity * rfpFeeScheduleNewchd.Cost);

                                            break;
                                        case RfpCostType.CummulativeCost:
                                            if (rfpFeeScheduleNewchd.RfpWorkType != null && rfpFeeScheduleNewchd.RfpWorkType.RfpJobTypeCumulativeCosts != null)
                                            {
                                                var cummulativeCostList = rfpWorkTypechild.RfpJobTypeCumulativeCosts.Where(x => x.Quantity <= rfpFeeScheduleNewchd.Quantity).ToList();

                                                if (cummulativeCostList != null)
                                                {
                                                    int maxQuantity = cummulativeCostList.Max(x => x.Quantity);
                                                    if (maxQuantity == rfpFeeScheduleNewchd.Quantity)
                                                    {
                                                        rfpFeeScheduleNewchd.Cost = 0;
                                                        rfpFeeScheduleNewchd.TotalCost = cummulativeCostList.Sum(x => x.Cost);
                                                    }
                                                    else
                                                    {
                                                        int samePriceQuantity = Convert.ToInt32(rfpFeeScheduleNewchd.Quantity) - maxQuantity;
                                                        double maxQuantityCost = cummulativeCostList.Where(x => x.Quantity == maxQuantity).Select(x => (double)x.Cost).FirstOrDefault();
                                                        double cummulativeCost = cummulativeCostList.Sum(x => (double)x.Cost);
                                                        rfpFeeScheduleNewchd.Cost = 0;
                                                        rfpFeeScheduleNewchd.TotalCost = cummulativeCost + (samePriceQuantity * maxQuantityCost);
                                                    }
                                                }
                                                else
                                                {
                                                    rfpFeeScheduleNewchd.Cost = 0;
                                                    rfpFeeScheduleNewchd.TotalCost = (rfpFeeScheduleNewchd.Quantity * rfpFeeScheduleNewchd.Cost);
                                                }
                                            }
                                            else
                                            {
                                                rfpFeeScheduleNewchd.Cost = 0;
                                                rfpFeeScheduleNewchd.TotalCost = (rfpFeeScheduleNewchd.Quantity * rfpFeeScheduleNewchd.Cost);
                                            }
                                            break;
                                        case RfpCostType.FixedCost:
                                            rfpFeeScheduleNewchd.Cost = rfpWorkTypechild.Cost;
                                            rfpFeeScheduleNewchd.TotalCost = rfpFeeScheduleNewchd.Cost;
                                            break;
                                        case RfpCostType.MinimumCost:
                                            rfpFeeScheduleNewchd.Cost = rfpWorkTypechild.Cost;
                                            rfpFeeScheduleNewchd.TotalCost = rfpFeeScheduleNewchd.Cost;
                                            break;
                                        case RfpCostType.PerUnitPrice:
                                            rfpFeeScheduleNewchd.Cost = rfpWorkTypechild.Cost;
                                            rfpFeeScheduleNewchd.TotalCost = (rfpFeeScheduleNewchd.Quantity * rfpFeeScheduleNewchd.Cost);
                                            break;

                                        case RfpCostType.HourlyCost:
                                            rfpFeeScheduleNewchd.Cost = rfpWorkTypechild.Cost;
                                            rfpFeeScheduleNewchd.TotalCost = ((rfpFeeScheduleNewchd.Quantity / 60) * rfpFeeScheduleNewchd.Cost);
                                            break;
                                    }
                                }

                                rfpFeeScheduleNewchd.Description = itemchild.Description;
                                rfpFeeScheduleNewchd.QuantityAchieved = 0;
                                if (cstTypechild != null && cstTypechild.ToString() == "HourlyCost")
                                {
                                    rfpFeeScheduleNewchd.QuantityPending = itemchild.Quantity * 60;
                                }
                                else
                                {
                                    rfpFeeScheduleNewchd.QuantityPending = itemchild.Quantity;
                                }
                                rfpFeeScheduleNewchd.IsAdditionalService = itemchild.IsAdditionalService;

                                rfpFeeScheduleNew.IsFromScope = true;

                                this.rpoContext.JobFeeSchedules.Add(rfpFeeScheduleNewchd);
                                this.rpoContext.SaveChanges();
                                #endregion
                            }
                        }

                        serviceName = serviceName.TrimEnd(',');

                        this.rpoContext.SaveChanges();

                        transaction.Commit();

                        foreach (var item in objListMessages)
                        {
                            Common.SaveJobHistory(employee.Id, job.Id, item, JobHistoryType.Scope);
                        }

                        SendJobScopeAddedNotification(job.Id, serviceName);

                        return this.Ok(jobFeeScheduleCreateUpdateList);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            throw new Exception("Error during roolback", e);
                        }

                        throw e;
                    }
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the unlinked job fee schedule.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idJobMilestone">The identifier job milestone.</param>
        /// <returns>unlinked job fee schedule.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobfeeschedules/{idJob}/unlinkedservices/{idJobMilestone}")]
        public IHttpActionResult GetUnlinkedJobFeeSchedule(int idJob, int idJobMilestone)
        {
            var milestoneServices = rpoContext.JobMilestoneServices.Where(x => x.IdMilestone != idJobMilestone && x.JobFeeSchedule.IdJob == idJob).Select(x => x.IdJobFeeSchedule).ToList();

            if (milestoneServices != null && milestoneServices.Count() > 0)
            {
                List<int> tmplist = new List<int>();

                List<JobFeeScheduleDetail> rfpFeeScheduleList = new List<JobFeeScheduleDetail>();
                foreach (var itmservice in milestoneServices)
                {
                    JobFeeSchedule objlist = rpoContext.JobFeeSchedules.FirstOrDefault(d => d.IdJob == idJob && d.Id == itmservice);

                    List<int> objlist1 = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(d => d.IdJob == idJob && d.RfpWorkType.PartOf == objlist.IdRfpWorkType).Select(d => d.Id).ToList();
                    if (objlist1.Count > 0)
                    {
                        tmplist.Add(itmservice);
                        tmplist.AddRange(objlist1);
                    }
                    else
                    {
                        tmplist.Add(itmservice);
                    }
                }

                var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == idJob && x.IsRemoved != true && !tmplist.Contains(x.Id) && x.RfpWorkType.IsShowScope == false && x.Status != "Completed")
                    .AsEnumerable()
                    .Select(jf => FormatDetail(jf))
                    .ToArray();

                string allidtmp = string.Empty;

                foreach (var item in result.OrderByDescending(d => d.Partof))
                {
                    JobFeeScheduleDetail objDetail = new JobFeeScheduleDetail();

                    if (item.Partof != null)
                    {
                        // allidtmp = string.Empty;
                        allidtmp = allidtmp + "," + item.AllIds;
                        item.TotalGroupCost = item.TotalGroupCost;
                    }
                    //if (item.IdRfp == null && item.Partof == null)
                    //{
                    //  //  allidtmp = string.Empty;
                    //}
                    string[] str = allidtmp.Split(',');

                    foreach (var itm in str)
                    {
                        if (itm == item.AllIds)
                        {
                            goto Finish;
                        }
                    }
                    rfpFeeScheduleList.Add(item);
                    Finish:
                    string st = "tmp";
                }
                return Ok(rfpFeeScheduleList);
            }
            else
            {
                var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == idJob && x.IsRemoved != true && x.Status != "Completed" && x.RfpWorkType.IsShowScope == false)
                    .AsEnumerable()
                    .Select(jf => FormatDetail(jf))
                    .ToArray();

                string allidtmp = string.Empty;
                List<JobFeeScheduleDetail> rfpFeeScheduleList = new List<JobFeeScheduleDetail>();
                foreach (var item in result.OrderByDescending(d => d.Partof))
                {
                    JobFeeScheduleDetail objDetail = new JobFeeScheduleDetail();

                    if (item.Partof != null)
                    {
                        // allidtmp = string.Empty;
                        allidtmp = item.AllIds;
                    }
                    string[] str = allidtmp.Split(',');

                    foreach (var itm in str)
                    {
                        if (itm == item.AllIds)
                        {
                            goto Finish;
                        }
                    }
                    objDetail.CostType = item.CostType;
                    objDetail.Id = item.Id;
                    objDetail.IdJobFeeSchedule = item.IdJobFeeSchedule;
                    objDetail.ItemName = item.ItemName;
                    rfpFeeScheduleList.Add(objDetail);
                    Finish:
                    string st = "tmp";

                }

                return Ok(rfpFeeScheduleList);
            }

            //    var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == idJob && x.IsRemoved != true && !milestoneServices.Contains(x.Id) && x.RfpWorkType.IsShowScope == false)
            //    .AsEnumerable()
            //    .Select(jf => FormatDetail(jf))
            //    .ToArray();



            //    //string allidtmp = string.Empty;
            //    //List<JobFeeScheduleDetail> rfpFeeScheduleList = new List<JobFeeScheduleDetail>();
            //    //foreach (var item in result.OrderByDescending(d => d.Partof))
            //    //{
            //    //    JobFeeScheduleDetail objDetail = new JobFeeScheduleDetail();

            //    //    if (item.Partof != null)
            //    //    {
            //    //        //allidtmp = string.Empty;
            //    //        allidtmp = item.AllIds;
            //    //    }
            //    //    string[] str = allidtmp.Split(',');

            //    //    foreach (var itm in str)
            //    //    {
            //    //        if (itm == item.AllIds)
            //    //        {
            //    //            goto Finish;
            //    //        }
            //    //    }
            //    //    objDetail.CostType = item.CostType;
            //    //    objDetail.Id = item.Id;
            //    //    objDetail.IdJobFeeSchedule = item.IdJobFeeSchedule;
            //    //    objDetail.ItemName = item.ItemName;
            //    //    rfpFeeScheduleList.Add(objDetail);
            //    //Finish:
            //    //    string st = "tmp";

            //    //}


            //}

        }


        /// <summary>
        /// Gets the job fee schedule.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Get the service item list aginst task.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobfeeschedules/{idJob}/dropdown/{idTask}")]
        public IHttpActionResult GetJobFeeSchedule(int idJob, int idTask)
        {
            List<int> objList = rpoContext.JobFeeSchedules.Where(d => d.IdJob == idJob && d.Status == "Completed").Select(d => d.IdRfpWorkType.Value).ToList();
            List<JobMilestone> objMilestonePending = rpoContext.JobMilestones.Include("JobMilestoneServices").Where(d => d.IdJob == idJob && d.Status == "Pending").ToList();

            Task task = rpoContext.Tasks.FirstOrDefault(x => x.Id == idTask);
            int idjobFeeSchedule = task != null && task.IdJobFeeSchedule != null ? Convert.ToInt32(task.IdJobFeeSchedule) : 0;
            var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == idJob && x.IsRemoved != true
                && x.RfpWorkType.CostType != RfpCostType.HourlyCost && (x.Status != "Completed" || x.Id == idjobFeeSchedule) && x.RfpWorkType.IsShowScope == false && !objList.Contains(x.RfpWorkType.PartOf.Value))
            .AsEnumerable()
            .Select(jf => FormatDetail(jf))
            .ToArray();

            List<JobFeeScheduleDetail> rfpFeeScheduleList = new List<JobFeeScheduleDetail>();
            foreach (var itemMilestone in objMilestonePending)
            {
                JobFeeScheduleDetail objDetail = new JobFeeScheduleDetail();

                var s = string.Join(",", itemMilestone.JobMilestoneServices.Where(p => p.IdMilestone == itemMilestone.Id)
                                 .Select(p => p.IdJobFeeSchedule.ToString()));

                objDetail.IdMilestone = itemMilestone.Id;
                objDetail.ItemName = (itemMilestone.IdRfp != null ? "(" + itemMilestone.IdRfp.Value + ")" + " - " : string.Empty) + itemMilestone.Name;
                objDetail.IsMilestone = true;
                objDetail.MilestoneServiceId = s.ToString();
                rfpFeeScheduleList.Add(objDetail);
            }

            string allidtmp = string.Empty;

            foreach (var item in result.OrderByDescending(d => d.Partof))
            {
                JobFeeScheduleDetail objDetail = new JobFeeScheduleDetail();

                if (item.Partof != null)
                {
                    allidtmp = allidtmp + "," + item.AllIds;
                    item.TotalGroupCost = item.TotalGroupCost;
                }
                string[] str = allidtmp.Split(',');

                foreach (var itm in str)
                {
                    if (itm == item.AllIds)
                    {
                        goto Finish;
                    }
                }

                rfpFeeScheduleList.Add(item);
                Finish:
                string st = "tmp";

            }

            List<JobFeeScheduleDetail> milestoneListWithServices = rfpFeeScheduleList.Where(d => d.IdMilestone != 0).OrderBy(d => d.IdMilestone).ToList();
            List<JobFeeScheduleDetail> finalList = new List<JobFeeScheduleDetail>();

            List<int> milestoneServicesid = new List<int>();

            foreach (var item in milestoneListWithServices)
            {
                finalList.AddRange(rfpFeeScheduleList.Where(d => d.IdMilestone == item.IdMilestone).ToList());

                if (!string.IsNullOrEmpty(item.MilestoneServiceId))
                {
                    int[] nums = Array.ConvertAll(item.MilestoneServiceId.Split(','), int.Parse);

                    var IDs = item.MilestoneServiceId.Split(',').Select(s => int.Parse(s)).ToList();

                    milestoneServicesid.AddRange(IDs);

                    var milestoneListWithServicesnew = rfpFeeScheduleList.Where(d => d.IdMilestone == 0 && nums.Contains(d.IdJobFeeSchedule)).OrderBy(d => d.IdMilestone).ToList();
                    finalList.AddRange(milestoneListWithServicesnew);
                }
            }


            List<JobFeeScheduleDetail> AdditionalServices = rfpFeeScheduleList.Where(d => d.IdMilestone == 0 && !milestoneServicesid.Contains(d.IdJobFeeSchedule)).OrderBy(d => d.Id).ToList();

            finalList.AddRange(AdditionalServices);

            return Ok(finalList);
        }

        /// <summary>
        /// Gets the job fee schedule hourly.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Get the service item list aginst timenotes.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobfeeschedules/{idJob}/hourlydropdown")]
        public IHttpActionResult GetJobFeeScheduleHourly(int idJob)
        {
            var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent")
                .Where(x => x.IdJob == idJob && x.IsRemoved != true && x.RfpWorkType.CostType == RfpCostType.HourlyCost && x.Status != "Completed" && x.RfpWorkType.IsShowScope == false)
                .AsEnumerable()
                .Select(jf => FormatDetail(jf))
                .ToArray();

            string allidtmp = string.Empty;
            List<JobFeeScheduleDetail> rfpFeeScheduleList = new List<JobFeeScheduleDetail>();
            foreach (var item in result.OrderByDescending(d => d.Partof))
            {
                JobFeeScheduleDetail objDetail = new JobFeeScheduleDetail();

                if (item.Partof != null)
                {
                    allidtmp = allidtmp + "," + item.AllIds;
                    item.TotalGroupCost = item.TotalGroupCost;
                }
                string[] str = allidtmp.Split(',');

                foreach (var itm in str)
                {
                    if (itm == item.AllIds)
                    {
                        goto Finish;
                    }
                }

                rfpFeeScheduleList.Add(item);
                Finish:
                string st = "tmp";

            }

            return Ok(result);
        }
        /// <summary>
        /// Get the service item list aginst timenotes.
        /// </summary>
        /// <param name="idJob"></param>
        /// <returns> Get the service item list aginst timenotes</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobfeeschedules/{idJob}/hourlydropdownEdit")]
        public IHttpActionResult GetJobFeeScheduleHourlyEdit(int idJob)
        {
            var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent")
                .Where(x => x.IdJob == idJob && x.RfpWorkType.CostType == RfpCostType.HourlyCost && x.RfpWorkType.IsShowScope == false)
                .AsEnumerable()
                .Select(jf => FormatDetail(jf))
                .ToArray();

            string allidtmp = string.Empty;
            List<JobFeeScheduleDetail> rfpFeeScheduleList = new List<JobFeeScheduleDetail>();
            foreach (var item in result.OrderByDescending(d => d.Partof))
            {
                JobFeeScheduleDetail objDetail = new JobFeeScheduleDetail();

                if (item.Partof != null)
                {
                    allidtmp = allidtmp + "," + item.AllIds;
                    item.TotalGroupCost = item.TotalGroupCost;
                }
                string[] str = allidtmp.Split(',');

                foreach (var itm in str)
                {
                    if (itm == item.AllIds)
                    {
                        goto Finish;
                    }
                }

                rfpFeeScheduleList.Add(item);
                Finish:
                string st = "tmp";

            }

            return Ok(result);
        }

        /// <summary>
        /// Puts the job fee schedule po number.
        /// </summary>
        /// <param name="idJobFeeSchedule">The identifier job fee schedule.</param>
        /// <param name="poNumber">The PO number.</param>
        /// <returns>update the ponumber on scope.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobfeeschedules/ponumber")]
        public IHttpActionResult PutJobFeeSchedulePONumber(JobFeeSchedulePONumber jobFeeSchedulePONumber)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobScope))
            {
                JobFeeSchedule jobFeeSchedule = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobFeeSchedulePONumber.IdJobFeeSchedule);
                string oldPONumberScope = jobFeeSchedule.PONumber;
                jobFeeSchedule.PONumber = jobFeeSchedulePONumber.PONumber;
                rpoContext.SaveChanges();

                string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeSchedule);
                string poNumberScope = JobHistoryMessages.PONumberScope
                                                  .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                                                  .Replace("##NewPONumber##", !string.IsNullOrEmpty(jobFeeSchedule.PONumber) ? jobFeeSchedule.PONumber : "-")
                                                  .Replace("##OldPONumber##", !string.IsNullOrEmpty(oldPONumberScope) ? oldPONumberScope : "-")
                                                  .Replace("##PONumber##", !string.IsNullOrEmpty(jobFeeSchedule.PONumber) ? jobFeeSchedule.PONumber : "-");

                Common.SaveJobHistory(employee.Id, jobFeeSchedule.IdJob, poNumberScope, JobHistoryType.Scope);

                return Ok(Format(jobFeeSchedule));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job fee schedule invoice number.
        /// </summary>
        /// <param name="idJobFeeSchedule">The identifier job fee schedule.</param>
        /// <param name="invoiceNumber">The invoice number.</param>
        /// <returns>update the invoice number in scope.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobfeeschedules/invoicenumber")]
        public IHttpActionResult PutJobFeeScheduleInvoiceNumber(JobFeeScheduleInvoiceNumber jobFeeScheduleInvoiceNumber)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobScope))
            {
                JobFeeSchedule jobFeeSchedule = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobFeeScheduleInvoiceNumber.IdJobFeeSchedule);
                string oldInvoiceNumber = jobFeeSchedule.InvoiceNumber;
                jobFeeSchedule.InvoiceNumber = jobFeeScheduleInvoiceNumber.InvoiceNumber;
                rpoContext.SaveChanges();

                var jobFeeSchedule2 = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdParentof == jobFeeScheduleInvoiceNumber.IdJobFeeSchedule).ToList();
                jobFeeSchedule2.ForEach(x => x.InvoiceNumber = jobFeeScheduleInvoiceNumber.InvoiceNumber);
                rpoContext.SaveChanges();
                string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeSchedule);

                //string invoiceNumberScope = JobHistoryMessages.InvoiceNumberScope
                //                  .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                //                  .Replace("##NewInvoiceNumber##", !string.IsNullOrEmpty(jobFeeSchedule.InvoiceNumber) ? jobFeeSchedule.InvoiceNumber : "-")
                //                  .Replace("##OldInvoiceNumber##", !string.IsNullOrEmpty(oldInvoiceNumber) ? oldInvoiceNumber : "-")
                //                  .Replace("##InvoiceNumber##", !string.IsNullOrEmpty(jobFeeSchedule.InvoiceNumber) ? jobFeeSchedule.InvoiceNumber : "-");

                //Common.SaveJobHistory(employee.Id, jobFeeSchedule.IdJob, invoiceNumberScope, JobHistoryType.Scope);

                return Ok(Format(jobFeeSchedule));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job fee schedule is invoiced.
        /// </summary>
        /// <param name="idJobFeeSchedule">The identifier job fee schedule.</param>
        /// <param name="isInvoiced">if set to <c>true</c> [is invoiced].</param>
        /// <returns>update the isinvoiced.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobfeeschedules/isinvoiced")]
        public IHttpActionResult PutJobFeeScheduleIsInvoiced(JobFeeScheduleIsInvoiced jobFeeScheduleIsInvoiced)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobScope))
            {
                JobFeeSchedule jobFeeSchedule = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobFeeScheduleIsInvoiced.IdJobFeeSchedule);

                jobFeeSchedule.IsInvoiced = jobFeeScheduleIsInvoiced.IsInvoiced;
                rpoContext.SaveChanges();

                return Ok(Format(jobFeeSchedule));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job fee schedule is invoiced.
        /// </summary>
        /// <param name="jobFeeScheduleInvoicedDate">The job fee schedule invoiced date.</param>
        /// <returns>pdate the isinvoiced date.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobfeeschedules/invoicedDate")]
        public IHttpActionResult PutJobFeeScheduleInvoicedDate(JobFeeScheduleInvoicedDate jobFeeScheduleInvoicedDate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobScope))
            {
                DateTime parsedInvoicedDate = new DateTime();
                if (!string.IsNullOrEmpty(jobFeeScheduleInvoicedDate.InvoicedDate) && !DateTime.TryParse(jobFeeScheduleInvoicedDate.InvoicedDate, out parsedInvoicedDate))
                {
                    throw new RpoBusinessException(StaticMessages.InvalidInvoiceDateMessage);
                }

                DateTime? invoicedDate = new DateTime();

                if (string.IsNullOrEmpty(jobFeeScheduleInvoicedDate.InvoicedDate))
                {
                    invoicedDate = null;
                }
                else
                {
                    invoicedDate = parsedInvoicedDate;
                }

                string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);
                JobFeeSchedule jobFeeSchedule = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobFeeScheduleInvoicedDate.IdJobFeeSchedule);
                DateTime? oldInvoiceDate = jobFeeSchedule.InvoicedDate;
                jobFeeSchedule.InvoicedDate = invoicedDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(invoicedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : invoicedDate;

                rpoContext.SaveChanges();

                string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeSchedule);

                string invoiceDateScope = JobHistoryMessages.InvoiceDateScope
                                  .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                                  .Replace("##NewInvoiceDate##", jobFeeSchedule != null && jobFeeSchedule.InvoicedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobFeeSchedule.InvoicedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToShortDateString() : "-")
                                  .Replace("##OldInvoiceDate##", oldInvoiceDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(oldInvoiceDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToShortDateString() : "-")
                                  .Replace("##InvoiceNumber##", !string.IsNullOrEmpty(jobFeeSchedule.InvoiceNumber) ? jobFeeSchedule.InvoiceNumber : "-");


                Common.SaveJobHistory(employee.Id, jobFeeSchedule.IdJob, invoiceDateScope, JobHistoryType.Scope);



                return Ok(Format(jobFeeSchedule));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job fee schedule remove.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>update the remove flag in scope.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobFeeSchedule))]
        [Route("api/jobfeeschedules/remove/{id}")]
        public IHttpActionResult PutJobFeeScheduleRemove(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJobScope))
            {
                JobFeeSchedule jobFeeSchedule = this.rpoContext.JobFeeSchedules.Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == id );                             

                if (jobFeeSchedule == null)
                {
                    return this.NotFound();
                }            

                string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeSchedule);                

                string deleteScope = JobHistoryMessages.DeleteScope
                                  .Replace("##ServiceItemName##", !string.IsNullOrEmpty(jobFeeScheduleDetailName) ? jobFeeScheduleDetailName : JobHistoryMessages.NoSetstring);


                List<JobMilestoneService> jobMilestoneServiceList = this.rpoContext.JobMilestoneServices.Include("Milestone").Where(x => x.IdJobFeeSchedule == id).ToList();
                string milestoneName = string.Empty;
                if (jobMilestoneServiceList != null && jobMilestoneServiceList.Count > 0)
                {
                    milestoneName = string.Join(",", jobMilestoneServiceList.Select(x => x.Milestone.Name));
                    this.rpoContext.JobMilestoneServices.RemoveRange(jobMilestoneServiceList);
                }

                jobFeeSchedule.IsRemoved = true;

                Common.SaveJobHistory(employee.Id, jobFeeSchedule.IdJob, deleteScope, JobHistoryType.Scope);

                this.rpoContext.SaveChanges();

                var jobFeeScheduleRemove = this.rpoContext.JobFeeSchedules.Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.Id == id || x.IdParentof == id).ToList();
                if (jobFeeScheduleRemove != null)
                {
                    jobFeeScheduleRemove.ForEach(x => x.IsRemoved = true);
                    rpoContext.SaveChanges();
                }

                return this.Ok(jobFeeSchedule);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Formats the specified RFP fee schedule.
        /// </summary>
        /// <param name="jobFeeSchedule">The RFP fee schedule.</param>
        /// <param name="jobMilestoneList">The job milestone list.</param>
        /// <returns>RfpFeeScheduleDTO.</returns>
        private JobFeeScheduleDTO Format(JobFeeSchedule jobFeeSchedule, List<JobMilestone> jobMilestoneList = null)
        {
            string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

            JobFeeScheduleDTO rfpServiceItemDTO = new JobFeeScheduleDTO();

            //if (jobFeeSchedule.RfpWorkType.CostType.ToString() == "HourlyCost")
            //{
            //    string[] strCompleted = jobFeeSchedule.Quantity.ToString().Split('.');
            //    string[] strPending = jobFeeSchedule.QuantityPending.ToString().Split('.');
            //    string[] strAchieved = jobFeeSchedule.QuantityAchieved.ToString().Split('.');

            //    int hourcomp = 0, mincomp = 0;

            //    if (strCompleted.Count()>1)
            //    {
            //        hourcomp = int.Parse(strCompleted[0].ToString());
            //        mincomp = int.Parse(strCompleted[1].ToString());                    
            //    }
            //    else if(strCompleted.Count() > 1)
            //    {
            //        hourcomp = int.Parse(strCompleted[0].ToString());
            //    }
            //    TimeSpan tscompleted = new TimeSpan(hourcomp, mincomp, 0);


            //    int hourpend = 0, minpend = 0;

            //    if (strPending.Count() > 1)
            //    {
            //        hourpend = int.Parse(strPending[0].ToString());
            //        minpend = int.Parse(strPending[1].ToString());
            //    }
            //    else if (strPending.Count() > 1)
            //    {
            //        hourpend = int.Parse(strPending[0].ToString());
            //    }

            //    TimeSpan tsPending = new TimeSpan(hourpend, minpend, 0);


            //    int hourarch = 0, minarch = 0;

            //    if (strPending.Count() > 1)
            //    {
            //        hourarch = int.Parse(strPending[0].ToString());
            //        minarch = int.Parse(strPending[1].ToString());
            //    }
            //    else if (strPending.Count() > 1)
            //    {
            //        hourarch = int.Parse(strPending[0].ToString());
            //    }

            //    TimeSpan tsAchieved = new TimeSpan(hourarch, minarch, 0);


            //    //TimeSpan tscompleted = new TimeSpan(long.Parse(jobFeeSchedule.Quantity.ToString()));
            //    //TimeSpan tsPending = new TimeSpan(long.Parse(jobFeeSchedule.QuantityPending.ToString()));
            //    //TimeSpan tsAchieved = new TimeSpan(long.Parse(jobFeeSchedule.QuantityAchieved.ToString()));
            //}
            rfpServiceItemDTO.Id = jobFeeSchedule.Id;
            rfpServiceItemDTO.IdRfpServiceItem = jobFeeSchedule.IdRfpWorkType;
            rfpServiceItemDTO.RfpServiceItem = jobFeeSchedule.RfpWorkType != null ? jobFeeSchedule.RfpWorkType.Name : string.Empty;
            rfpServiceItemDTO.IdRfp = jobFeeSchedule.IdRfp;
            rfpServiceItemDTO.RfpNumber = jobFeeSchedule.IdRfp != null && jobFeeSchedule.Rfp != null ? jobFeeSchedule.Rfp.RfpNumber : string.Empty;
            rfpServiceItemDTO.QuantityAchieved = jobFeeSchedule.QuantityAchieved;
            rfpServiceItemDTO.QuantityPending = jobFeeSchedule.QuantityPending;
            rfpServiceItemDTO.PONumber = jobFeeSchedule.PONumber;
            rfpServiceItemDTO.Status = jobFeeSchedule.Status;
            rfpServiceItemDTO.IsInvoiced = jobFeeSchedule.IsInvoiced;
            rfpServiceItemDTO.InvoiceNumber = jobFeeSchedule.InvoiceNumber;
            rfpServiceItemDTO.Quantity = jobFeeSchedule.Quantity;
            rfpServiceItemDTO.InvoicedDate = jobFeeSchedule.InvoicedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobFeeSchedule.InvoicedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobFeeSchedule.InvoicedDate;
            rfpServiceItemDTO.IdJob = jobFeeSchedule.IdJob;
            rfpServiceItemDTO.IsRemoved = jobFeeSchedule.IsRemoved;
            rfpServiceItemDTO.CostType = jobFeeSchedule.RfpWorkType != null ? jobFeeSchedule.RfpWorkType.CostType : RfpCostType.FixedCost;
            rfpServiceItemDTO.QuantityHours = (from d in rpoContext.JobTimeNotes where d.IdJobFeeSchedule == jobFeeSchedule.Id && d.IdJob == jobFeeSchedule.IdJob select d.TimeHours).FirstOrDefault() != null ? (from d in rpoContext.JobTimeNotes where d.IdJobFeeSchedule == jobFeeSchedule.Id && d.IdJob == jobFeeSchedule.IdJob select d.TimeHours).FirstOrDefault() : string.Empty;
            rfpServiceItemDTO.QuantityMinutes = (from d in rpoContext.JobTimeNotes where d.IdJobFeeSchedule == jobFeeSchedule.Id && d.IdJob == jobFeeSchedule.IdJob select d.TimeMinutes).FirstOrDefault() != null ? (from d in rpoContext.JobTimeNotes where d.IdJobFeeSchedule == jobFeeSchedule.Id && d.IdJob == jobFeeSchedule.IdJob select d.TimeMinutes).FirstOrDefault() : string.Empty;
            rfpServiceItemDTO.Cost = jobFeeSchedule.Cost != null ? Convert.ToDouble(jobFeeSchedule.Cost).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty;
            rfpServiceItemDTO.TotalCost = jobFeeSchedule.TotalCost != null ? Convert.ToDouble(jobFeeSchedule.TotalCost).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty;
            rfpServiceItemDTO.IsAdditionalService = jobFeeSchedule.IsAdditionalService;
            rfpServiceItemDTO.LastModified = jobFeeSchedule.LastModified != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobFeeSchedule.LastModified), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobFeeSchedule.LastModified;
            //    rfpServiceItemDTO.mo = jobFeeSchedule.ModifiedBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobFeeSchedule.ModifiedBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobFeeSchedule.ModifiedBy;
            rfpServiceItemDTO.LastModifiedBy = jobFeeSchedule.ModifiedByEmployee != null ? (jobFeeSchedule.ModifiedByEmployee.FirstName + " " + (jobFeeSchedule.ModifiedByEmployee.LastName != null ? jobFeeSchedule.ModifiedByEmployee.LastName : string.Empty)) : string.Empty;
            rfpServiceItemDTO.CreatedDate = jobFeeSchedule.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobFeeSchedule.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobFeeSchedule.CreatedDate;
            rfpServiceItemDTO.CreatedBy = jobFeeSchedule.CreatedByEmployee != null ? (jobFeeSchedule.CreatedByEmployee.FirstName + " " + (jobFeeSchedule.CreatedByEmployee.LastName != null ? jobFeeSchedule.CreatedByEmployee.LastName : string.Empty)) : string.Empty;
            rfpServiceItemDTO.IsShow = jobFeeSchedule.IsShow;

            var Taskid = (from d in rpoContext.Tasks where (d.IdJob == jobFeeSchedule.IdJob && d.IdJobFeeSchedule == jobFeeSchedule.Id && d.IdTaskStatus == 3) select d.Id).ToArray();

            var Transmittalid = (from d in rpoContext.JobTransmittals where (d.IdJob == jobFeeSchedule.IdJob && Taskid.Contains(d.IdTask.Value)) select d.Id).ToArray();

            rfpServiceItemDTO.ServiceItemIdTask = Taskid;
            rfpServiceItemDTO.ServiceItemIdTransamittal = Transmittalid;


            if (jobMilestoneList != null)
            {
                var jobMilestone = jobMilestoneList.FirstOrDefault(x => x.JobMilestoneServices.Select(J => J.IdJobFeeSchedule).Contains(jobFeeSchedule.Id));
                if (jobMilestone != null)
                {
                    rfpServiceItemDTO.JobMilestoneId = jobMilestone.Id;
                    rfpServiceItemDTO.RfpNumber = jobMilestone.IdRfp != null && (from d in rpoContext.Rfps where d.Id == jobMilestone.IdRfp select d).FirstOrDefault() != null ? (from d in rpoContext.Rfps where d.Id == jobMilestone.IdRfp select d.RfpNumber).FirstOrDefault() : string.Empty;
                    rfpServiceItemDTO.JobMilestoneName = jobMilestone.Name;
                    rfpServiceItemDTO.JobMilestoneValue = jobMilestone.Value != null ? Convert.ToDouble(jobMilestone.Value).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty;
                    rfpServiceItemDTO.JobMilestoneStatus = !string.IsNullOrEmpty(jobMilestone.Status) ? jobMilestone.Status : "Pending";
                    rfpServiceItemDTO.JobMilestoneIsInvoiced = jobMilestone.IsInvoiced;
                    rfpServiceItemDTO.JobMilestoneInvoiceNumber = jobMilestone.InvoiceNumber;
                    rfpServiceItemDTO.JobMilestoneInvoicedDate = jobMilestone.InvoicedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobMilestone.InvoicedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobMilestone.InvoicedDate;
                    rfpServiceItemDTO.JobMilestonePONumber = jobMilestone.PONumber;
                    rfpServiceItemDTO.IdRfp = jobMilestone.IdRfp;
                    rfpServiceItemDTO.CreatedDate = jobMilestone.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobMilestone.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobMilestone.CreatedDate;
                    rfpServiceItemDTO.CreatedBy = jobMilestone.CreatedByEmployee != null ? (jobMilestone.CreatedByEmployee.FirstName + " " + (jobMilestone.CreatedByEmployee.LastName != null ? jobMilestone.CreatedByEmployee.LastName : string.Empty)) : string.Empty;
                    rfpServiceItemDTO.LastModified = jobMilestone.LastModified != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobMilestone.LastModified), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobMilestone.LastModified;
                    rfpServiceItemDTO.LastModifiedBy = jobMilestone.ModifiedByEmployee != null ? (jobMilestone.ModifiedByEmployee.FirstName + " " + (jobMilestone.ModifiedByEmployee.LastName != null ? jobMilestone.ModifiedByEmployee.LastName : string.Empty)) : string.Empty;
                }
                else if (jobFeeSchedule.IdRfp != null)
                {
                    rfpServiceItemDTO.RfpNumber = (from d in rpoContext.Rfps where d.Id == jobFeeSchedule.IdRfp select d).FirstOrDefault() != null ? (from d in rpoContext.Rfps where d.Id == jobFeeSchedule.IdRfp select d.RfpNumber).FirstOrDefault() : string.Empty;
                }

            }

            if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 4)
            {
                rfpServiceItemDTO.IdRfpServiceGroup = jobFeeSchedule.RfpWorkType.IdParent;
                rfpServiceItemDTO.RfpServiceGroup = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 3)
                {
                    rfpServiceItemDTO.IdRfpSubJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpSubJobType = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 2)
                    {
                        rfpServiceItemDTO.IdRfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Id : 0;
                        rfpServiceItemDTO.RfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;

                        if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null)
                        {
                            rfpServiceItemDTO.IdRfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.IdParent : 0;
                            rfpServiceItemDTO.RfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 1)
                    {
                        rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
                        rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

                        rfpServiceItemDTO.IdRfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.IdParent : 0;
                        rfpServiceItemDTO.RfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                {
                    rfpServiceItemDTO.IdRfpSubJobType = 0;
                    rfpServiceItemDTO.RfpSubJobType = string.Empty;

                    rfpServiceItemDTO.IdRfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                    {
                        rfpServiceItemDTO.IdRfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.IdParent : 0;
                        rfpServiceItemDTO.RfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                {
                    rfpServiceItemDTO.IdRfpSubJobType = 0;
                    rfpServiceItemDTO.RfpSubJobType = string.Empty;

                    rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
                    rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

                    rfpServiceItemDTO.IdRfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                }
            }
            else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 3)
            {

                rfpServiceItemDTO.IdRfpServiceGroup = 0;
                rfpServiceItemDTO.RfpServiceGroup = string.Empty;

                rfpServiceItemDTO.IdRfpSubJobType = jobFeeSchedule.RfpWorkType.IdParent;
                rfpServiceItemDTO.RfpSubJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                {
                    rfpServiceItemDTO.IdRfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                    {
                        rfpServiceItemDTO.IdRfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.IdParent : 0;
                        rfpServiceItemDTO.RfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                {
                    rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
                    rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

                    rfpServiceItemDTO.IdRfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                }

            }
            else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 2)
            {
                rfpServiceItemDTO.IdRfpServiceGroup = 0;
                rfpServiceItemDTO.RfpServiceGroup = string.Empty;

                rfpServiceItemDTO.IdRfpSubJobType = 0;
                rfpServiceItemDTO.RfpSubJobType = string.Empty;

                rfpServiceItemDTO.IdRfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.IdParent;
                rfpServiceItemDTO.RfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (jobFeeSchedule.RfpWorkType.Parent.Parent != null)
                {
                    rfpServiceItemDTO.IdRfpJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.IdParent : 0;
                    rfpServiceItemDTO.RfpJobType = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                }

            }
            else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 1)
            {

                rfpServiceItemDTO.IdRfpJobType = jobFeeSchedule.RfpWorkType.IdParent;
                rfpServiceItemDTO.RfpJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                rfpServiceItemDTO.IdRfpSubJobTypeCategory = 0;
                rfpServiceItemDTO.RfpSubJobTypeCategory = string.Empty;

                rfpServiceItemDTO.IdRfpSubJobType = 0;
                rfpServiceItemDTO.RfpSubJobType = string.Empty;

                rfpServiceItemDTO.IdRfpServiceGroup = 0;
                rfpServiceItemDTO.RfpServiceGroup = string.Empty;

            }

            //rfpServiceItemDTO.RfpServiceItem =
            //   (!string.IsNullOrEmpty(rfpServiceItemDTO.RfpJobType) ? rfpServiceItemDTO.RfpJobType + " - " : string.Empty) +
            //   (!string.IsNullOrEmpty(rfpServiceItemDTO.RfpSubJobTypeCategory) ? rfpServiceItemDTO.RfpSubJobTypeCategory + " - " : string.Empty) +
            //   (!string.IsNullOrEmpty(rfpServiceItemDTO.RfpSubJobType) ? rfpServiceItemDTO.RfpSubJobType + " - " : string.Empty) +
            //   (!string.IsNullOrEmpty(rfpServiceItemDTO.RfpServiceGroup) ? rfpServiceItemDTO.RfpServiceGroup + " - " : string.Empty) +
            //   (!string.IsNullOrEmpty(rfpServiceItemDTO.RfpServiceItem) ? rfpServiceItemDTO.RfpServiceItem : string.Empty);

            string finalstr = (!string.IsNullOrEmpty(rfpServiceItemDTO.RfpJobType) ? rfpServiceItemDTO.RfpJobType + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpServiceItemDTO.RfpSubJobTypeCategory) ? rfpServiceItemDTO.RfpSubJobTypeCategory + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpServiceItemDTO.RfpSubJobType) ? rfpServiceItemDTO.RfpSubJobType + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpServiceItemDTO.RfpServiceGroup) ? rfpServiceItemDTO.RfpServiceGroup + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpServiceItemDTO.RfpServiceItem) ? rfpServiceItemDTO.RfpServiceItem : string.Empty);

            if (jobFeeSchedule.IdRfp != null && jobFeeSchedule.RfpWorkType.PartOf == null)
            {
                var result = rpoContext.JobFeeSchedules.Include("Rfp")
               .Include("RfpWorkType.Parent.Parent.Parent.Parent")
                //   .Include("RfpWorkTypeCategory")
                //.Include("ProjectDetail.RfpSubJobType")
                //.Include("ProjectDetail.RfpJobType")
                //.Include("ProjectDetail.RfpSubJobTypeCategory")
                .Where(x => x.IdRfp == jobFeeSchedule.IdRfp && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false).Select(d => d).ToList();

                string stringJoin = string.Empty;
                string allitemsid = string.Empty;
                string allid = string.Empty;
                double? totalamt = 0;
                foreach (var itemname in result)
                {
                    stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                    allid = allid + itemname.RfpWorkType.Id + ",";
                    totalamt = totalamt + itemname.TotalCost;
                }

                if (!string.IsNullOrEmpty(stringJoin))
                {
                    stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                    rfpServiceItemDTO.RfpServiceItem = finalstr + " (" + stringJoin.Trim() + ")";

                    rfpServiceItemDTO.AllIds = allid;
                    rfpServiceItemDTO.Partof = jobFeeSchedule.IdRfpWorkType;
                    rfpServiceItemDTO.TotalGroupCost = jobFeeSchedule.TotalCost + totalamt;
                }
                else
                {
                    rfpServiceItemDTO.RfpServiceItem = finalstr;
                    rfpServiceItemDTO.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
                    rfpServiceItemDTO.TotalGroupCost = jobFeeSchedule.TotalCost;
                }
            }
            else if (jobFeeSchedule.IdRfp == null && jobFeeSchedule.RfpWorkType.PartOf == null && jobFeeSchedule.IsAdditionalService == false)
            {
                var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == jobFeeSchedule.IdJob && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false && jobFeeSchedule.IsAdditionalService == false).Select(d => d).ToList();

                string stringJoin = string.Empty;
                string allitemsid = string.Empty;
                string allid = string.Empty;
                double? totalamt = 0;
                foreach (var itemname in result)
                {
                    stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                    allid = allid + itemname.RfpWorkType.Id + ",";
                    totalamt = totalamt + itemname.TotalCost;
                }

                if (!string.IsNullOrEmpty(stringJoin))
                {
                    stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                    rfpServiceItemDTO.RfpServiceItem = finalstr + " (" + stringJoin.Trim() + ")";

                    rfpServiceItemDTO.AllIds = allid;
                    rfpServiceItemDTO.Partof = jobFeeSchedule.IdRfpWorkType;
                    rfpServiceItemDTO.TotalGroupCost = jobFeeSchedule.TotalCost + totalamt;
                }
                else
                {
                    rfpServiceItemDTO.RfpServiceItem = finalstr;
                    rfpServiceItemDTO.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
                    rfpServiceItemDTO.TotalGroupCost = jobFeeSchedule.TotalCost;
                }
            }
            else if (jobFeeSchedule.IdRfp == null && jobFeeSchedule.RfpWorkType.PartOf == null && jobFeeSchedule.IsAdditionalService == true)
            {
                var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == jobFeeSchedule.IdJob && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false && x.IsAdditionalService == true && x.IdParentof == jobFeeSchedule.Id).Select(d => d).ToList();

                string stringJoin = string.Empty;
                string allitemsid = string.Empty;
                string allid = string.Empty;
                double? totalamt = 0;
                foreach (var itemname in result)
                {
                    stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                    allid = allid + itemname.RfpWorkType.Id + ",";
                    totalamt = totalamt + itemname.TotalCost;
                }

                if (!string.IsNullOrEmpty(stringJoin))
                {
                    stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                    rfpServiceItemDTO.RfpServiceItem = finalstr + " (" + stringJoin.Trim() + ")";

                    rfpServiceItemDTO.AllIds = allid;
                    rfpServiceItemDTO.Partof = jobFeeSchedule.IdRfpWorkType;
                    rfpServiceItemDTO.TotalGroupCost = jobFeeSchedule.TotalCost + totalamt;
                }
                else
                {
                    rfpServiceItemDTO.RfpServiceItem = finalstr;
                    rfpServiceItemDTO.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
                    rfpServiceItemDTO.TotalGroupCost = jobFeeSchedule.TotalCost;
                }
            }
            else
            {
                rfpServiceItemDTO.RfpServiceItem = finalstr;
                rfpServiceItemDTO.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
                rfpServiceItemDTO.TotalGroupCost = jobFeeSchedule.TotalCost;
            }

            return rfpServiceItemDTO;
        }

        /// <summary>
        /// Formats the detail.
        /// </summary>
        /// <param name="jobFeeSchedule">The job fee schedule.</param>
        /// <returns>Job Fee Schedule Detail.</returns>
        private JobFeeScheduleDetail FormatDetail(JobFeeSchedule jobFeeSchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

            JobFeeScheduleDetail rfpFeeScheduleDetail = new JobFeeScheduleDetail();
            rfpFeeScheduleDetail.IdJobFeeSchedule = jobFeeSchedule.Id;
            rfpFeeScheduleDetail.CostType = jobFeeSchedule.RfpWorkType.CostType;
            rfpFeeScheduleDetail.Id = jobFeeSchedule.Id;

            string rfpNumber = jobFeeSchedule.Rfp != null ? jobFeeSchedule.Rfp.RfpNumber : string.Empty;
            string rfpServiceItem = jobFeeSchedule.RfpWorkType != null ? jobFeeSchedule.RfpWorkType.Name : string.Empty;
            string rfpServiceGroup = string.Empty;
            string rfpSubJobType = string.Empty;
            string rfpSubJobTypeCategory = string.Empty;
            string rfpJobType = string.Empty;

            if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 4)
            {
                rfpServiceGroup = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 3)
                {
                    rfpSubJobType = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 2)
                    {
                        rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;

                        if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null)
                        {
                            rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 1)
                    {
                        rfpSubJobTypeCategory = string.Empty;
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                {
                    rfpSubJobType = string.Empty;
                    rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                    {
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                {
                    rfpSubJobType = string.Empty;
                    rfpSubJobTypeCategory = string.Empty;
                    rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                }

            }
            else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 3)
            {
                rfpServiceGroup = string.Empty;
                rfpSubJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                {
                    rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                    {
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                {
                    rfpSubJobTypeCategory = string.Empty;
                    rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                }

            }
            else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 2)
            {
                rfpServiceGroup = string.Empty;

                rfpSubJobType = string.Empty;
                rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (jobFeeSchedule.RfpWorkType.Parent.Parent != null)
                {
                    rfpJobType = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                }

            }
            else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 1)
            {
                rfpJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                rfpSubJobTypeCategory = string.Empty;

                rfpSubJobType = string.Empty;

                rfpServiceGroup = string.Empty;

            }


            //rfpFeeScheduleDetail.ItemName =
            //    (!string.IsNullOrEmpty(rfpNumber) ? rfpNumber + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);



            string finalstr = (!string.IsNullOrEmpty(rfpNumber) ? "(" + rfpNumber + ")" + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);

            if (jobFeeSchedule.IdRfp != null && jobFeeSchedule.RfpWorkType.PartOf == null)
            {
                var result = rpoContext.RfpFeeSchedules
                .Include("RfpWorkType")
                .Include("RfpWorkTypeCategory")
                .Include("ProjectDetail.RfpSubJobType")
                .Include("ProjectDetail.RfpJobType")
                .Include("ProjectDetail.RfpSubJobTypeCategory")
                .Where(x => x.IdRfp == jobFeeSchedule.IdRfp && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false).Select(d => d).ToList();

                string stringJoin = string.Empty;
                string allitemsid = string.Empty;
                string allid = string.Empty;
                double? totalamt = 0;
                foreach (var itemname in result)
                {
                    stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                    allid = allid + itemname.RfpWorkType.Id + ",";
                    totalamt = totalamt + itemname.TotalCost;
                }

                if (!string.IsNullOrEmpty(stringJoin))
                {
                    stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                    rfpFeeScheduleDetail.ItemName = finalstr + " (" + stringJoin.Trim() + ")";

                    rfpFeeScheduleDetail.AllIds = allid;
                    rfpFeeScheduleDetail.Partof = jobFeeSchedule.IdRfpWorkType;
                    rfpFeeScheduleDetail.TotalGroupCost = jobFeeSchedule.TotalCost + totalamt;
                }
                else
                {
                    rfpFeeScheduleDetail.ItemName = finalstr;
                    rfpFeeScheduleDetail.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
                    rfpFeeScheduleDetail.TotalGroupCost = jobFeeSchedule.TotalCost;
                }
            }
            else if (jobFeeSchedule.IdRfp == null && jobFeeSchedule.RfpWorkType.PartOf == null && jobFeeSchedule.IsAdditionalService == false)
            {
                var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == jobFeeSchedule.IdJob && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false && jobFeeSchedule.IsAdditionalService == false).Select(d => d).ToList();

                string stringJoin = string.Empty;
                string allitemsid = string.Empty;
                string allid = string.Empty;
                double? totalamt = 0;
                foreach (var itemname in result)
                {
                    stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                    allid = allid + itemname.RfpWorkType.Id + ",";
                    totalamt = totalamt + itemname.TotalCost;
                }

                if (!string.IsNullOrEmpty(stringJoin))
                {
                    stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                    rfpFeeScheduleDetail.ItemName = finalstr + " (" + stringJoin.Trim() + ")";

                    rfpFeeScheduleDetail.AllIds = allid;
                    rfpFeeScheduleDetail.Partof = jobFeeSchedule.IdRfpWorkType;
                    rfpFeeScheduleDetail.TotalGroupCost = jobFeeSchedule.TotalCost + totalamt;
                }
                else
                {
                    rfpFeeScheduleDetail.ItemName = finalstr;
                    rfpFeeScheduleDetail.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
                    rfpFeeScheduleDetail.TotalGroupCost = jobFeeSchedule.TotalCost;
                }
            }
            else if (jobFeeSchedule.IdRfp == null && jobFeeSchedule.RfpWorkType.PartOf == null && jobFeeSchedule.IsAdditionalService == true)
            {
                var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == jobFeeSchedule.IdJob && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false && x.IsAdditionalService == true && x.IdParentof == jobFeeSchedule.Id).Select(d => d).ToList();

                string stringJoin = string.Empty;
                string allitemsid = string.Empty;
                string allid = string.Empty;
                double? totalamt = 0;
                foreach (var itemname in result)
                {
                    stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                    allid = allid + itemname.RfpWorkType.Id + ",";
                    totalamt = totalamt + itemname.TotalCost;
                }

                if (!string.IsNullOrEmpty(stringJoin))
                {
                    stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                    rfpFeeScheduleDetail.ItemName = finalstr + " (" + stringJoin.Trim() + ")";

                    rfpFeeScheduleDetail.AllIds = allid;
                    rfpFeeScheduleDetail.Partof = jobFeeSchedule.IdRfpWorkType;
                    rfpFeeScheduleDetail.TotalGroupCost = jobFeeSchedule.TotalCost + totalamt;
                }
                else
                {
                    rfpFeeScheduleDetail.ItemName = finalstr;
                    rfpFeeScheduleDetail.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
                    rfpFeeScheduleDetail.TotalGroupCost = jobFeeSchedule.TotalCost;
                }
            }
            else
            {
                rfpFeeScheduleDetail.ItemName = finalstr;
                rfpFeeScheduleDetail.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
                rfpFeeScheduleDetail.TotalGroupCost = jobFeeSchedule.TotalCost;
            }

            //if (jobFeeSchedule.RfpWorkType.PartOf == null)
            //{
            //    var result = rpoContext.RfpFeeSchedules
            //    .Include("RfpWorkType")
            //    .Include("RfpWorkTypeCategory")
            //    .Include("ProjectDetail.RfpSubJobType")
            //    .Include("ProjectDetail.RfpJobType")
            //    .Include("ProjectDetail.RfpSubJobTypeCategory")
            //    .Where(x => x.IdRfp == jobFeeSchedule.IdRfp && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false).Select(d => d.RfpWorkType).ToList();

            //    string stringJoin = string.Empty;
            //    string allid = string.Empty;
            //    foreach (var itemname in result)
            //    {
            //        stringJoin = stringJoin + itemname.Name + ",";
            //        allid = allid + itemname.Id + ",";
            //    }
            //    if (!string.IsNullOrEmpty(stringJoin))
            //    {
            //        stringJoin = stringJoin.Remove(stringJoin.Length - 1);
            //        allid = allid.Remove(allid.Length - 1);

            //        rfpFeeScheduleDetail.ItemName = finalstr + " ( " + stringJoin + " ) ";
            //        rfpFeeScheduleDetail.AllIds = allid;
            //        rfpFeeScheduleDetail.Partof = jobFeeSchedule.IdRfpWorkType;
            //    }
            //    else
            //    {
            //        rfpFeeScheduleDetail.ItemName = finalstr;
            //        rfpFeeScheduleDetail.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
            //    }
            //}
            //else
            //{
            //    rfpFeeScheduleDetail.ItemName = finalstr;
            //    rfpFeeScheduleDetail.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
            //}
            return rfpFeeScheduleDetail;
        }

        private string FormatItemDetail(JobFeeSchedule jobFeeSchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

            JobFeeScheduleDetail rfpFeeScheduleDetail = new JobFeeScheduleDetail();
            rfpFeeScheduleDetail.IdJobFeeSchedule = jobFeeSchedule.Id;
            rfpFeeScheduleDetail.CostType = jobFeeSchedule.RfpWorkType.CostType;
            rfpFeeScheduleDetail.Id = jobFeeSchedule.Id;

            string rfpNumber = jobFeeSchedule.Rfp != null ? jobFeeSchedule.Rfp.RfpNumber : string.Empty;
            string rfpServiceItem = jobFeeSchedule.RfpWorkType != null ? jobFeeSchedule.RfpWorkType.Name : string.Empty;
            string rfpServiceGroup = string.Empty;
            string rfpSubJobType = string.Empty;
            string rfpSubJobTypeCategory = string.Empty;
            string rfpJobType = string.Empty;

            if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 4)
            {
                rfpServiceGroup = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 3)
                {
                    rfpSubJobType = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 2)
                    {
                        rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;

                        if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null)
                        {
                            rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 1)
                    {
                        rfpSubJobTypeCategory = string.Empty;
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                {
                    rfpSubJobType = string.Empty;
                    rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                    {
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                {
                    rfpSubJobType = string.Empty;
                    rfpSubJobTypeCategory = string.Empty;
                    rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                }

            }
            else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 3)
            {
                rfpServiceGroup = string.Empty;
                rfpSubJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                {
                    rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                    {
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                {
                    rfpSubJobTypeCategory = string.Empty;
                    rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                }

            }
            else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 2)
            {
                rfpServiceGroup = string.Empty;

                rfpSubJobType = string.Empty;
                rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                if (jobFeeSchedule.RfpWorkType.Parent.Parent != null)
                {
                    rfpJobType = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                }

            }
            else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 1)
            {
                rfpJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                rfpSubJobTypeCategory = string.Empty;

                rfpSubJobType = string.Empty;

                rfpServiceGroup = string.Empty;

            }


            //rfpFeeScheduleDetail.ItemName =
            //    (!string.IsNullOrEmpty(rfpNumber) ? rfpNumber + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
            //    (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);



            string finalstr = (!string.IsNullOrEmpty(rfpNumber) ? "(" + rfpNumber + ")" + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
               (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);

            string finalitem = string.Empty;
            if (jobFeeSchedule.RfpWorkType.PartOf == null)
            {
                var result = rpoContext.RfpFeeSchedules
                .Include("RfpWorkType")
                .Include("RfpWorkTypeCategory")
                .Include("ProjectDetail.RfpSubJobType")
                .Include("ProjectDetail.RfpJobType")
                .Include("ProjectDetail.RfpSubJobTypeCategory")
                .Where(x => x.IdRfp == jobFeeSchedule.IdRfp && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false).Select(d => d.RfpWorkType).ToList();

                string stringJoin = string.Empty;
                string allid = string.Empty;
                foreach (var itemname in result)
                {
                    stringJoin = stringJoin + " " + itemname.Name + ",";
                    allid = allid + itemname.Id + ",";
                }
                if (!string.IsNullOrEmpty(stringJoin))
                {
                    stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                    allid = allid.Remove(allid.Length - 1);
                    finalitem = finalstr + " (" + stringJoin.Trim() + ")";
                    rfpFeeScheduleDetail.ItemName = finalstr + " (" + stringJoin.Trim() + ")";
                    rfpFeeScheduleDetail.AllIds = allid;
                    rfpFeeScheduleDetail.Partof = jobFeeSchedule.IdRfpWorkType;
                }
                else
                {
                    finalitem = finalstr;
                    rfpFeeScheduleDetail.ItemName = finalstr;
                    rfpFeeScheduleDetail.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
                }
            }
            else
            {
                finalitem = finalstr;
                rfpFeeScheduleDetail.ItemName = finalstr;
                rfpFeeScheduleDetail.AllIds = jobFeeSchedule.IdRfpWorkType.ToString();
            }
            return finalitem;
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

        ///// <summary>
        ///// Sends the mail.
        ///// </summary>
        ///// <param name="employee">The employee.</param>
        ///// <param name="jobNumber">The job number.</param>
        ///// <param name="idJob">The identifier job.</param>
        ///// <param name="to">To.</param>
        ///// <param name="subjectMsg">The subject MSG.</param>
        ///// <param name="serviceName">Name of the service.</param>
        //private void SendJobScopeAddedMail(string serviceName, string jobNumber, int idJob)
        //{
        //    SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenJobMilestoneIsCompleted);
        //    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
        //    {
        //        foreach (var item in systemSettingDetail.Value)
        //        {
        //            string body = string.Empty;
        //            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/NewJobScopeAdded.htm")))
        //            {
        //                body = reader.ReadToEnd();
        //            }
        //            string emailBody = body;
        //            emailBody = emailBody.Replace("##EmployeeName##", item.EmployeeName);
        //            emailBody = emailBody.Replace("##serviceitem##", serviceName);
        //            emailBody = emailBody.Replace("##JobNumber##", jobNumber);
        //            emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + idJob + "/application");

        //            Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), new KeyValuePair<string, string>(item.Email, item.EmployeeName), "Job scope is added", emailBody, true);
        //            Common.SendInAppNotifications(item.Id, StaticMessages.NewJobScopeAddedNotificationMessage.Replace("##JobNumber##", jobNumber), Hub, "/job/" + idJob + "/application");
        //        }
        //    }
        //}

        private void SendJobScopeAddedNotification(int id, string serviceName)
        {
            Job jobs = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == id).FirstOrDefault();
            List<int> jobAssignList = new List<int>();

            if (jobs.IdProjectManager != null && jobs.IdProjectManager > 0)
            {
                int idProjectManager = Convert.ToInt32(jobs.IdProjectManager);
                jobAssignList.Add(idProjectManager);
            }

            if (jobs.DOBProjectTeam != null && !string.IsNullOrEmpty(jobs.DOBProjectTeam))
            {
                foreach (var item in jobs.DOBProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(employeeid);
                }
            }

            if (jobs.DOTProjectTeam != null && !string.IsNullOrEmpty(jobs.DOTProjectTeam))
            {
                foreach (var item in jobs.DOTProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(employeeid);
                }
            }

            if (jobs.ViolationProjectTeam != null && !string.IsNullOrEmpty(jobs.ViolationProjectTeam))
            {
                foreach (var item in jobs.ViolationProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(employeeid);
                }
            }

            if (jobs.DEPProjectTeam != null && !string.IsNullOrEmpty(jobs.DEPProjectTeam))
            {
                foreach (var item in jobs.DEPProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(employeeid);
                }
            }

            var employeelist = jobAssignList.Distinct();

            foreach (var item in employeelist)
            {
                int employeeid = Convert.ToInt32(item);
                Employee employee = this.rpoContext.Employees.FirstOrDefault(x => x.Id == employeeid);

                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenJobScopeIsAdded);
                if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                {
                    foreach (Employees.EmployeeDetail employeeDetail in systemSettingDetail.Value)
                    {
                        if (employeeDetail.Email != employee.Email && !employeelist.Contains(employeeDetail.Id))
                        {
                            string newJobScopeAddedSetting = InAppNotificationMessage.NewJobScopeAdded
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##ServiceItemName##", serviceName)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/scope");
                            Common.SendInAppNotifications(employeeDetail.Id, newJobScopeAddedSetting, Hub, "job/" + jobs.Id + "/scope");
                        }
                    }
                }

                string newJobScopeAdded = InAppNotificationMessage.NewJobScopeAdded
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##ServiceItemName##", serviceName)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/scope");
                Common.SendInAppNotifications(employee.Id, newJobScopeAdded, Hub, "job/" + jobs.Id + "/scope");
            }
        }

        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/jobfeeschedules/DownloadProposal/{idJob}")]
        public IHttpActionResult GetDownloadProposalList(int idJob)
        {
            List<int?> objRFPList = rpoContext.JobFeeSchedules.Where(d => d.IdJob == idJob && d.IdRfp != null).Select(d => d.IdRfp).Distinct().ToList();

            List<JobFeeRFPProposal> objlist = new List<JobFeeRFPProposal>();
            foreach (var itemRFP in objRFPList)
            {
                List<RFPEmailHistory> objEmailHistoriesList = new List<RFPEmailHistory>();
                var objRFPHistoryList = rpoContext.RFPEmailHistories.Where(d => d.IdRfp == itemRFP).OrderByDescending(d => d.Id).Select(d => d.Id).FirstOrDefault();

                if (objRFPHistoryList != null)
                {
                    objEmailHistoriesList = rpoContext.RFPEmailHistories.Where(d => d.Id == objRFPHistoryList).Select(d => d).Distinct().ToList();
                }

                if (objEmailHistoriesList != null && objEmailHistoriesList.Count > 0)
                {
                    foreach (var item in objEmailHistoriesList)
                    {
                        RFPEmailAttachmentHistory objEmailAttachment = rpoContext.RFPEmailAttachmentHistories.OrderByDescending(d => d.IdRFPEmailHistory).FirstOrDefault(d => d.IdRFPEmailHistory == item.Id);
                        if (objEmailAttachment != null)
                        {
                            string fullpath = Properties.Settings.Default.APIUrl + Properties.Settings.Default.RFPAttachmentsPath;

                            JobFeeRFPProposal objproposal = new JobFeeRFPProposal();
                            objproposal.IdRFP = item.IdRfp;
                            objproposal.Name = objEmailAttachment.Name;
                            objproposal.IdRFPEmailHistory = item.Id;
                            objproposal.DocumentPath = !string.IsNullOrEmpty(objEmailAttachment.DocumentPath) ? fullpath+"/" + objEmailAttachment.Id.ToString() + "_" + objEmailAttachment.DocumentPath : "N/A";
                            objlist.Add(objproposal);
                        }
                        else
                        {
                            JobFeeRFPProposal objproposal = new JobFeeRFPProposal();
                            objproposal.IdRFP = itemRFP;
                            //objproposal.Name = objEmailAttachment.Name;
                            objproposal.IdRFPEmailHistory = item.Id;
                            objproposal.DocumentPath = "N/A";
                            objlist.Add(objproposal);
                        }
                    }
                }
                else
                {
                    JobFeeRFPProposal objproposal = new JobFeeRFPProposal();
                    objproposal.IdRFP = itemRFP;
                    //objproposal.Name = objEmailAttachment.Name;
                    //objproposal.IdRFPEmailHistory = item.Id;
                    objproposal.DocumentPath = "N/A";
                    objlist.Add(objproposal);
                }
            }
            return Ok(objlist);
        }
    }
}