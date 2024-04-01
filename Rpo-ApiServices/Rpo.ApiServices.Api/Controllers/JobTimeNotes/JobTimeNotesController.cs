// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-16-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-24-2018
// ***********************************************************************
// <copyright file="JobTimeNotesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Time Notes Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.JobTimeNotes
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Hubs;
    using Model.Models.Enums;
    using Models;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Collections.Generic;
    using SystemSettings;
    using System.IO;
    using System.Web;
    using System.Net;
    using System.Net.Http;/// <summary>
                          /// Class Job Time Notes Controller.
                          /// </summary>
                          /// <seealso cref="System.Web.Http.ApiController" />
    public class JobTimeNotesController : HubApiController<GroupHub>
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the job time notes.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the job time notes List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetJobTimeNotes([FromUri] JobTimeNoteDataTableParameters dataTableParameters)
        {
            var jobTimeNotes = rpoContext.JobTimeNotes
                .Include("JobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent")
                .Include("RfpJobType.Parent.Parent.Parent.Parent")
                .Include("Job")
                .Include("CreatedByEmployee")
                .Include("LastModifiedByEmployee")
                .AsQueryable();

            if (dataTableParameters.IdJob != null && dataTableParameters.IdJob > 0)
            {
                jobTimeNotes = jobTimeNotes.Where(jc => jc.IdJob == dataTableParameters.IdJob).AsQueryable();
            }

            var recordsTotal = jobTimeNotes.Count();
            var recordsFiltered = recordsTotal;

            var result = jobTimeNotes
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
        /// Gets the job time note.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the job time notes in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobTimeNoteDetail))]
        public IHttpActionResult GetJobTimeNote(int id)
        {
            JobTimeNote jobTimeNote = rpoContext.JobTimeNotes.Include("JobFeeSchedule")
                .Include("RfpJobType")
                .Include("Job").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);

            if (jobTimeNote == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(jobTimeNote));
        }

        /// <summary>
        ///put the job timenotes.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>update the detail of job timenotes.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPut]
        [ResponseType(typeof(JobTimeNoteDetail))]
        //  [Route("api/JobTimeNotes/Test")]
        public IHttpActionResult PutJobTimeNote(int id, JobTimeNoteCreateUpdate jobTimeNoteCreateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditJobTimenotes))
            //{
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jobTimeNoteCreateUpdate.Id)
            {
                return BadRequest();
            }

            var oldTimenotes = rpoContext.JobTimeNotes.Find(id);

            double OldTotalTimeEntry2ormore = 0;
            List<JobTimeNote> jobTimeNoteList = rpoContext.JobTimeNotes.Where(d => d.IdJobFeeSchedule == oldTimenotes.IdJobFeeSchedule && d.Id != id).ToList();

            foreach (var item in jobTimeNoteList)
            {
                double TotalTimeEntryold = (double.Parse((!string.IsNullOrEmpty(item.TimeHours)) ? item.TimeHours : "0") * 60) + double.Parse((!string.IsNullOrEmpty(item.TimeMinutes)) ? item.TimeMinutes : "0");
                OldTotalTimeEntry2ormore = OldTotalTimeEntry2ormore + TotalTimeEntryold;
            }

            //string timeQuantity = (jobTimeNoteCreateUpdate.TimeHours != null ? Convert.ToString(jobTimeNoteCreateUpdate.TimeHours) : "00") + "." + (jobTimeNoteCreateUpdate.TimeMinutes != null ? Convert.ToString(jobTimeNoteCreateUpdate.TimeMinutes) : "00");

            //  string oldtimeQuantity = (oldTimenotes.TimeHours != null ? Convert.ToString(oldTimenotes.TimeHours) : "00") + "." + (oldTimenotes.TimeMinutes != null ? Convert.ToString(oldTimenotes.TimeMinutes) : "00");
            double OldTotalTimeEntry = 0;
            if (oldTimenotes.JobBillingType == JobBillingType.ScopeBilling)
            {
                OldTotalTimeEntry = (double.Parse((!string.IsNullOrEmpty(oldTimenotes.TimeHours)) ? oldTimenotes.TimeHours : "0") * 60) + double.Parse((!string.IsNullOrEmpty(oldTimenotes.TimeMinutes)) ? oldTimenotes.TimeMinutes : "0");
            }

            if (jobTimeNoteCreateUpdate.JobBillingType == JobBillingType.ScopeBilling && jobTimeNoteCreateUpdate.IdJobFeeSchedule != null && jobTimeNoteCreateUpdate.IdJobFeeSchedule > 0)
            {
                var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == jobTimeNoteCreateUpdate.IdJobFeeSchedule);

                double TotalTimeEntry = (double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeHours)) ? jobTimeNoteCreateUpdate.TimeHours : "0") * 60) + double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeMinutes)) ? jobTimeNoteCreateUpdate.TimeMinutes : "0");

                if (jobFeeSchedules.QuantityPending < 0 || jobFeeSchedules.QuantityPending + Math.Abs(OldTotalTimeEntry) < TotalTimeEntry)
                {
                    throw new RpoBusinessException(StaticMessages.FeeScheduleJobimeNotesExceedsLimitMessage);
                }
            }

            if (oldTimenotes.IdJobFeeSchedule != null && oldTimenotes.IdJobFeeSchedule > 0)
            {
                JobFeeSchedule jobFeeSchedules = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == oldTimenotes.IdJobFeeSchedule);

                if (oldTimenotes.JobBillingType == JobBillingType.ScopeBilling)
                {
                    if (jobFeeSchedules.RfpWorkType.CostType.ToString() == "HourlyCost")
                    {
                        double? totalPending = jobFeeSchedules.QuantityPending;
                        double? totalAchieved = jobFeeSchedules.QuantityAchieved;

                        //if (totalPending <= TotalTimeEntry)
                        //{
                        totalPending = (totalPending + OldTotalTimeEntry);
                        totalAchieved = (totalAchieved - OldTotalTimeEntry);

                        jobFeeSchedules.QuantityPending = totalPending;
                        jobFeeSchedules.QuantityAchieved = totalAchieved > 0 ? totalAchieved : 0;
                        //  }
                    }
                    else
                    {
                        jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending + OldTotalTimeEntry;
                        jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved - OldTotalTimeEntry;
                    }

                    if ((jobFeeSchedules.QuantityAchieved - OldTotalTimeEntry) == jobFeeSchedules.Quantity)
                    {
                        jobFeeSchedules.Status = "Completed";
                        jobFeeSchedules.LastModified = DateTime.UtcNow;
                        if (employee != null)
                        {
                            jobFeeSchedules.ModifiedBy = employee.Id;
                        }
                    }
                    else
                    {
                        jobFeeSchedules.Status = "";
                        jobFeeSchedules.LastModified = DateTime.UtcNow;
                        if (employee != null)
                        {
                            jobFeeSchedules.ModifiedBy = employee.Id;
                        }
                    }
                }
                rpoContext.SaveChanges();
            }



            //rpoContext.Entry(jobTimeNote).State = EntityState.Modified;
            JobTimeNote jobTimeNote = rpoContext.JobTimeNotes.FirstOrDefault(e => e.Id == jobTimeNoteCreateUpdate.Id);
            jobTimeNote.Id = jobTimeNoteCreateUpdate.Id;
            jobTimeNote.IdJob = jobTimeNoteCreateUpdate.IdJob;
            if (jobTimeNoteCreateUpdate.JobBillingType == JobBillingType.NonBillableItems)
            {
                jobTimeNote.IdJobFeeSchedule = null;
                jobTimeNote.IdRfpJobType = null;
            }
            else
            {
                jobTimeNote.IdJobFeeSchedule = jobTimeNoteCreateUpdate.IdJobFeeSchedule;
                jobTimeNote.IdRfpJobType = jobTimeNoteCreateUpdate.IdRfpJobType;
            }

            jobTimeNote.JobBillingType = jobTimeNoteCreateUpdate.JobBillingType;
            jobTimeNote.ProgressNotes = jobTimeNoteCreateUpdate.ProgressNotes;
            jobTimeNote.TimeNoteDate = DateTime.SpecifyKind(Convert.ToDateTime(jobTimeNoteCreateUpdate.TimeNoteDate), DateTimeKind.Utc);
            jobTimeNote.TimeHours = jobTimeNoteCreateUpdate.TimeHours;
            jobTimeNote.TimeMinutes = jobTimeNoteCreateUpdate.TimeMinutes;
            //mb flag
            jobTimeNote.FromProgressionNote = jobTimeNoteCreateUpdate.FromProgressionNote;
            jobTimeNote.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                jobTimeNote.LastModifiedBy = employee.Id;
            }

            jobTimeNote.IsQuickbookSynced = false;

            try
            {
                rpoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTimeNoteExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }


            string hours = string.Empty;

            int additiontype = JobBillingType.AdditionalBilling.GetHashCode();
            string changeStatusScope = string.Empty;
            if (jobTimeNote.JobBillingType == JobBillingType.AdditionalBilling)
            {
                // OtherBillable(additiontype, jobTimeNote.Id, jobTimeNote.IdJob);
            }
            if (jobTimeNote.IdJobFeeSchedule != null && jobTimeNote.IdJobFeeSchedule > 0)
            {
                JobFeeSchedule jobFeeSchedules = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobTimeNote.IdJobFeeSchedule);

                double TotalTimeEntry = (double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeHours)) ? jobTimeNoteCreateUpdate.TimeHours : "0") * 60) + double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeMinutes)) ? jobTimeNoteCreateUpdate.TimeMinutes : "0");
                if (jobTimeNote.JobBillingType == JobBillingType.ScopeBilling)
                {
                    if (jobFeeSchedules.RfpWorkType.CostType.ToString() == "HourlyCost")
                    {
                        double? totalPending = jobFeeSchedules.QuantityPending;
                        double? totalAchieved = jobFeeSchedules.QuantityAchieved;

                        //if (totalPending <= TotalTimeEntry)
                        //{
                        totalPending = (totalPending - TotalTimeEntry);
                        totalAchieved = (totalAchieved + TotalTimeEntry);

                        jobFeeSchedules.QuantityPending = totalPending;
                        jobFeeSchedules.QuantityAchieved = totalAchieved;
                        //  }
                    }
                    else
                    {
                        jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending - TotalTimeEntry;
                        jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved + TotalTimeEntry;
                    }
                }
                rpoContext.SaveChanges();

                hours = (!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeHours) ? jobTimeNoteCreateUpdate.TimeHours : "0") + "h" + ":" + (!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeMinutes) ? jobTimeNoteCreateUpdate.TimeMinutes : "0") + "m";
                string oldServiceItemStatus = jobFeeSchedules.Status;
                if (jobFeeSchedules.QuantityAchieved == jobFeeSchedules.Quantity)
                {
                    jobFeeSchedules.Status = "Completed";
                    jobFeeSchedules.CompletedDate = DateTime.UtcNow;
                    jobFeeSchedules.LastModified = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobFeeSchedules.ModifiedBy = employee.Id;
                    }
                    JobFeeSchedule jobFeeScheduletmp = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobTimeNote.IdJobFeeSchedule);

                    string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeScheduletmp);

                    // double TotalTimeEntry = (double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeHours)) ? jobTimeNoteCreateUpdate.TimeHours : "0") * 60) + double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeMinutes)) ? jobTimeNoteCreateUpdate.TimeMinutes : "0");


                    changeStatusScope = JobHistoryMessages.ChangeStatusScope
                        .Replace("##Hours##", hours)
                        .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                        .Replace("##NewServiceItemStatus##", jobFeeSchedules.Status)
                        .Replace("##OldServiceItemStatus##", oldServiceItemStatus)
                        .Replace("##TaskNumber##", "Not Set")
                        .Replace("##TaskType##", "Not Set")
                        .Replace("##JobNumber##", jobFeeScheduletmp.IdJob.ToString())
                       .Replace("##RedirectionLinkJob##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobFeeScheduletmp.IdJob + "/application");

                    string changeStatusScopeHistory = JobHistoryMessages.ChangeStatusScopeMessage
                        .Replace("##ServiceItemName##", !string.IsNullOrEmpty(jobFeeScheduleDetailName) ? jobFeeScheduleDetailName : JobHistoryMessages.NoSetstring);



                    Common.SaveJobHistory(employee.Id, Convert.ToInt32(jobTimeNote.IdJob), changeStatusScopeHistory, JobHistoryType.Scope);

                    Common.SendInAppNotifications(employee.Id, changeStatusScope, Hub, jobTimeNote.IdJob > 0 || jobTimeNote.IdJob != null ? "job/" + jobTimeNote.IdJob + "/timenotes" : "timenotes");

                }

                rpoContext.SaveChanges();

                Common.SaveTimeNoteHistory(employee.Id, StaticMessages.JobTimeNotesAddedHistoryMessage.Replace("##Hours##", hours).Replace("##JobTimeNoteDescription##", jobTimeNote.ProgressNotes), jobTimeNote.Id, jobFeeSchedules.Id);
            }

            updateMilestoneStatus(jobTimeNote.Id, jobTimeNote.IdJobFeeSchedule, jobTimeNote.IdJob, hours, employee.Id);

            JobTimeNote jobTimeNoteResponse = rpoContext.JobTimeNotes.Include("JobFeeSchedule").Include("RfpJobType").Include("Job").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == id);
            return Ok(FormatDetails(jobTimeNoteResponse));
            //}
            //else
            //{
            //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            //}
        }

        /// <summary>
        /// Posts the job time note.
        /// </summary>
        /// <param name="jobTimeNoteCreateUpdate">The job time note create update.</param>
        /// <returns>create a new job time notes.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobTimeNoteDetail))]
        public IHttpActionResult PostJobTimeNote(JobTimeNoteCreateUpdate jobTimeNoteCreateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string timeQuantity = (jobTimeNoteCreateUpdate.TimeHours != null ? Convert.ToString(jobTimeNoteCreateUpdate.TimeHours) : "00") + "." + (jobTimeNoteCreateUpdate.TimeMinutes != null ? Convert.ToString(jobTimeNoteCreateUpdate.TimeMinutes) : "00");

            if (jobTimeNoteCreateUpdate.IdJobFeeSchedule != null && jobTimeNoteCreateUpdate.IdJobFeeSchedule > 0)
            {
                var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == jobTimeNoteCreateUpdate.IdJobFeeSchedule);
                //  string TimeQuantity = Convert.ToString(jobTimeNoteCreateUpdate.TimeHours != null ? jobTimeNoteCreateUpdate.TimeHours : 0) + "." + Convert.ToString(jobTimeNoteCreateUpdate.TimeHours != null ? jobTimeNoteCreateUpdate.TimeMinutes : 0);

                double TotalTimeEntry = (double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeHours)) ? jobTimeNoteCreateUpdate.TimeHours : "0") * 60) + double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeMinutes)) ? jobTimeNoteCreateUpdate.TimeMinutes : "0");

                if (jobFeeSchedules.QuantityPending < 0 || jobFeeSchedules.QuantityPending < TotalTimeEntry)
                {
                    //  throw new RpoBusinessException(StaticMessages.FeeScheduleQuantityTimeNotesExceedsLimitMessage);
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { Error = StaticMessages.FeeScheduleJobimeNotesExceedsLimitMessage }));
                }
            }

            JobTimeNote jobTimeNote = new JobTimeNote();
            jobTimeNote.Id = jobTimeNoteCreateUpdate.Id;
            jobTimeNote.IdJob = jobTimeNoteCreateUpdate.IdJob;
            jobTimeNote.IdJobFeeSchedule = jobTimeNoteCreateUpdate.IdJobFeeSchedule;
            jobTimeNote.IdRfpJobType = jobTimeNoteCreateUpdate.IdRfpJobType;
            jobTimeNote.JobBillingType = jobTimeNoteCreateUpdate.JobBillingType;
            jobTimeNote.ProgressNotes = jobTimeNoteCreateUpdate.ProgressNotes;
            jobTimeNote.TimeNoteDate = DateTime.SpecifyKind(Convert.ToDateTime(jobTimeNoteCreateUpdate.TimeNoteDate), DateTimeKind.Utc);
            jobTimeNote.TimeHours = jobTimeNoteCreateUpdate.TimeHours;
            jobTimeNote.TimeMinutes = jobTimeNoteCreateUpdate.TimeMinutes;
            //mb flag
            jobTimeNote.FromProgressionNote = jobTimeNoteCreateUpdate.FromProgressionNote;
            jobTimeNote.LastModifiedDate = DateTime.UtcNow;
            jobTimeNote.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                jobTimeNote.CreatedBy = employee.Id;
                jobTimeNote.LastModifiedBy = employee.Id;
            }

            jobTimeNote.IsQuickbookSynced = false;
            jobTimeNote.QuickbookSyncedDate = null;
            jobTimeNote.QuickbookSyncError = null;

            rpoContext.JobTimeNotes.Add(jobTimeNote);
            rpoContext.SaveChanges();

            string hours = string.Empty;

            int additiontype = JobBillingType.AdditionalBilling.GetHashCode();
            string changeStatusScope = string.Empty;
            if (jobTimeNote.JobBillingType == JobBillingType.AdditionalBilling)
            {
                // OtherBillable(additiontype, jobTimeNote.Id, jobTimeNote.IdJob);
            }
            if (jobTimeNote.IdJobFeeSchedule != null && jobTimeNote.IdJobFeeSchedule > 0)
            {
                JobFeeSchedule jobFeeSchedules = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobTimeNote.IdJobFeeSchedule);

                double TotalTimeEntry = (double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeHours)) ? jobTimeNoteCreateUpdate.TimeHours : "0") * 60) + double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeMinutes)) ? jobTimeNoteCreateUpdate.TimeMinutes : "0");
                if (jobTimeNote.JobBillingType == JobBillingType.ScopeBilling)
                {
                    if (jobFeeSchedules.RfpWorkType.CostType.ToString() == "HourlyCost")
                    {
                        double? totalPending = jobFeeSchedules.QuantityPending;
                        double? totalAchieved = jobFeeSchedules.QuantityAchieved;

                        //if (totalPending <= TotalTimeEntry)
                        //{
                        totalPending = (totalPending - TotalTimeEntry);
                        totalAchieved = (totalAchieved + TotalTimeEntry);

                        jobFeeSchedules.QuantityPending = totalPending;
                        jobFeeSchedules.QuantityAchieved = totalAchieved;
                        //  }
                    }
                    else
                    {
                        jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending - TotalTimeEntry;
                        jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved + TotalTimeEntry;
                    }
                }
                rpoContext.SaveChanges();

                hours = (!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeHours) ? jobTimeNoteCreateUpdate.TimeHours : "0") + "h" + ":" + (!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeMinutes) ? jobTimeNoteCreateUpdate.TimeMinutes : "0") + "m";
                string oldServiceItemStatus = jobFeeSchedules.Status;
                if (jobFeeSchedules.QuantityAchieved == jobFeeSchedules.Quantity)
                {
                    jobFeeSchedules.Status = "Completed";
                    jobFeeSchedules.CompletedDate = DateTime.UtcNow;
                    jobFeeSchedules.LastModified = DateTime.UtcNow;
                    if (employee != null)
                    {
                        jobFeeSchedules.ModifiedBy = employee.Id;
                    }
                    JobFeeSchedule jobFeeScheduletmp = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobTimeNote.IdJobFeeSchedule);

                    string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeScheduletmp);

                    // double TotalTimeEntry = (double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeHours)) ? jobTimeNoteCreateUpdate.TimeHours : "0") * 60) + double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeMinutes)) ? jobTimeNoteCreateUpdate.TimeMinutes : "0");


                    changeStatusScope = JobHistoryMessages.ChangeStatusScope
                        .Replace("##Hours##", hours)
                        .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                        .Replace("##NewServiceItemStatus##", jobFeeSchedules.Status)
                        .Replace("##OldServiceItemStatus##", oldServiceItemStatus)
                        .Replace("##TaskNumber##", "Not Set")
                        .Replace("##TaskType##", "Not Set")
                        .Replace("##JobNumber##", jobFeeScheduletmp.IdJob.ToString())
                       .Replace("##RedirectionLinkJob##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobFeeScheduletmp.IdJob + "/application");

                    string changeStatusScopeHistory = JobHistoryMessages.ChangeStatusScopeMessage
                        .Replace("##ServiceItemName##", !string.IsNullOrEmpty(jobFeeScheduleDetailName) ? jobFeeScheduleDetailName : JobHistoryMessages.NoSetstring);



                    Common.SaveJobHistory(employee.Id, Convert.ToInt32(jobTimeNote.IdJob), changeStatusScopeHistory, JobHistoryType.Scope);

                    Common.SendInAppNotifications(employee.Id, changeStatusScope, Hub, jobTimeNote.IdJob > 0 || jobTimeNote.IdJob != null ? "job/" + jobTimeNote.IdJob + "/timenotes" : "timenotes");

                }

                rpoContext.SaveChanges();

                Common.SaveTimeNoteHistory(employee.Id, StaticMessages.JobTimeNotesAddedHistoryMessage.Replace("##Hours##", hours).Replace("##JobTimeNoteDescription##", jobTimeNote.ProgressNotes), jobTimeNote.Id, jobFeeSchedules.Id);
                // Common.UpdateMilestoneStatus(jobFeeSchedules.Id, Hub);
            }


            //if (jobTimeNote.IdRfpJobType != null && jobTimeNote.IdRfpJobType > 0)
            //{
            //    RfpJobType rfpJobType = rpoContext.RfpJobTypes.FirstOrDefault(x => x.Id == jobTimeNote.IdRfpJobType);

            //    double TotalTimeEntry = (double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeHours)) ? jobTimeNoteCreateUpdate.TimeHours : "0") * 60) + double.Parse((!string.IsNullOrEmpty(jobTimeNoteCreateUpdate.TimeMinutes)) ? jobTimeNoteCreateUpdate.TimeMinutes : "0");

            //    JobFeeSchedule jobFeeSchedule = new JobFeeSchedule();
            //    jobFeeSchedule.IdJob = Convert.ToInt32(jobTimeNote.IdJob);
            //    jobFeeSchedule.IdRfpWorkType = jobTimeNote.IdRfpJobType;
            //    jobFeeSchedule.IsInvoiced = false;
            //    jobFeeSchedule.Quantity = TotalTimeEntry;
            //    jobFeeSchedule.QuantityAchieved = TotalTimeEntry;
            //    jobFeeSchedule.QuantityPending = 0;
            //    jobFeeSchedule.Cost = rfpJobType.Cost;
            //    jobFeeSchedule.TotalCost = (rfpJobType.Cost * (jobFeeSchedule.Quantity / 60));
            //    jobFeeSchedule.Status = "Completed";
            //    jobFeeSchedule.CompletedDate = DateTime.UtcNow;
            //    jobFeeSchedule.LastModified = DateTime.UtcNow;
            //    if (employee != null)
            //    {
            //        jobFeeSchedule.ModifiedBy = employee.Id;
            //    }
            //    rpoContext.JobFeeSchedules.Add(jobFeeSchedule);

            //    rpoContext.SaveChanges();

            //    JobFeeSchedule jobFeeSchedulestmp = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobFeeSchedule.Id);

            //    string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeSchedulestmp);

            //    string jobTimeMessage = JobHistoryMessages.ChangeStatusScopeMessage
            //       .Replace("##hours##", hours)
            //       .Replace("##ServiceItemName##", jobFeeScheduleDetailName);
            //    Common.SaveJobHistory(employee.Id, Convert.ToInt32(jobTimeNote.IdJob), jobTimeMessage, JobHistoryType.Scope);

            //    Common.SaveTimeNoteHistory(employee.Id, StaticMessages.JobTimeNotesAddedHistoryMessage.Replace("##Hours##", hours).Replace("##JobTimeNoteDescription##", jobTimeNote.ProgressNotes), jobTimeNote.Id, jobFeeSchedule.Id);

            //    // Common.UpdateMilestoneStatus(jobFeeSchedule.Id, Hub);
            //}

            updateMilestoneStatus(jobTimeNote.Id, jobTimeNote.IdJobFeeSchedule, jobTimeNote.IdJob, hours, employee.Id);
            //additionalCompleted(jobTimeNote.IdJobFeeSchedule, jobTimeNote.Id, jobTimeNote.IdJob, hours, changeStatusScope,employee.Id);

            JobTimeNote jobTimeNoteResponse = rpoContext.JobTimeNotes.Include("JobFeeSchedule").Include("RfpJobType").Include("Job").Include("CreatedByEmployee").Include("LastModifiedByEmployee").FirstOrDefault(x => x.Id == jobTimeNote.Id);
            return Ok(FormatDetails(jobTimeNoteResponse));
        }

        private void OtherBillable(int billableType, int? timenoteid, int? idJob)
        {
            if (timenoteid > 0)
            {
                JobTimeNote objtimenote = rpoContext.JobTimeNotes.Where(x => x.Id == timenoteid && x.IdJob == idJob).FirstOrDefault();

                RfpJobType rfpJobType = rpoContext.RfpJobTypes.FirstOrDefault(x => x.Id == objtimenote.IdRfpJobType);

                JobFeeSchedule rfpFeeScheduleNew = new JobFeeSchedule();
                rfpFeeScheduleNew.IdJob = objtimenote.IdJob.Value;
                rfpFeeScheduleNew.IdRfpWorkType = objtimenote.IdRfpJobType;

                RfpCostType cstType = (from d in rpoContext.RfpJobTypes where d.Id == objtimenote.IdRfpJobType select d.CostType).FirstOrDefault();

                double TotalTimeEntry = (double.Parse((!string.IsNullOrEmpty(objtimenote.TimeHours)) ? objtimenote.TimeHours : "0") * 60) + double.Parse((!string.IsNullOrEmpty(objtimenote.TimeMinutes)) ? objtimenote.TimeMinutes : "0");


                rfpFeeScheduleNew.IsInvoiced = false;
                rfpFeeScheduleNew.Quantity = TotalTimeEntry;
                rfpFeeScheduleNew.QuantityPending = 0;
                rfpFeeScheduleNew.QuantityAchieved = TotalTimeEntry;
                rfpFeeScheduleNew.Cost = rfpJobType.Cost;
                rfpFeeScheduleNew.TotalCost = (rfpJobType.Cost * (rfpFeeScheduleNew.Quantity / 60));
                rfpFeeScheduleNew.Status = "Completed";
                rfpFeeScheduleNew.CompletedDate = DateTime.UtcNow;
                rfpFeeScheduleNew.LastModified = DateTime.UtcNow;

                var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

                if (employee != null)
                {
                    rfpFeeScheduleNew.ModifiedBy = employee.Id;
                }
                rfpFeeScheduleNew.Description = objtimenote.ProgressNotes;

                rfpFeeScheduleNew.IsAdditionalService = true;

                this.rpoContext.JobFeeSchedules.Add(rfpFeeScheduleNew);
                this.rpoContext.SaveChanges();

                objtimenote.IdJobFeeSchedule = rfpFeeScheduleNew.Id;
                rpoContext.SaveChanges();
            }

        }

        private void updateMilestoneStatus(int? taskid, int? jobFeeScheduleid, int? idJob, string hours, int employeeid)
        {
            List<int> objmilestonemain = (from d in rpoContext.JobMilestones where d.IdJob == idJob select d.Id).ToList();

            JobMilestoneService objservie = (from d in rpoContext.JobMilestoneServices where objmilestonemain.Contains(d.IdMilestone) && d.IdJobFeeSchedule == jobFeeScheduleid select d).FirstOrDefault();

            if (objservie != null && objservie.IdMilestone != 0)
            {
                JobMilestone objmilestone = (from d in rpoContext.JobMilestones where d.Id == objservie.IdMilestone && d.IdJob == idJob select d).FirstOrDefault();

                List<int> objservieresList = (from d in rpoContext.JobMilestoneServices where d.IdMilestone == objmilestone.Id select d.IdJobFeeSchedule).ToList();

                string tmpstatus = "Completed";

                List<JobFeeSchedule> objschdule = (from d in rpoContext.JobFeeSchedules where d.IdJob == idJob && objservieresList.Contains(d.Id) && d.QuantityPending == 0 && d.Status.Trim().Contains(tmpstatus) select d).ToList();

                if (objservieresList.Count == objschdule.Count && objmilestone.Status != "Completed")
                {
                    string changeStatusMilestone = JobHistoryMessages.ChangeStatusMilestone
               .Replace("##BillingPointName##", !string.IsNullOrEmpty(objmilestone.Name) ? objmilestone.Name : JobHistoryMessages.NoSetstring)
               .Replace("##OldStatus##", "Pending")
               .Replace("##NewStatus##", !string.IsNullOrEmpty(objmilestone.Status) ? objmilestone.Status : JobHistoryMessages.NoSetstring);

                    Common.SaveJobHistory(employeeid, objmilestone.IdJob, changeStatusMilestone, JobHistoryType.Milestone);

                    objmilestone.Status = "Completed";
                    objmilestone.LastModified = DateTime.UtcNow;
                    rpoContext.SaveChanges();

                    JobMilestoneCompleted(objmilestone.Id, taskid, objmilestone.IdJob, hours);
                }
            }
        }
        private void additionalCompleted(int? jobFeeScheduleid, int? taskid, int? idJob, string hours, string changeStatusScope, int employeeid)
        {
            if (taskid > 0)
            {
                string javascript = "click=\"redirectFromNotification(j)\"";

                JobTimeNote timenote = rpoContext.JobTimeNotes.Where(x => x.Id == taskid && x.IdJob == idJob).FirstOrDefault();

                List<int> objmilestonemain = (from d in rpoContext.JobMilestones where d.IdJob == idJob select d.Id).ToList();

                List<int> objservie = (from d in rpoContext.JobMilestoneServices where d.IdJobFeeSchedule == jobFeeScheduleid && objmilestonemain.Contains(d.IdMilestone) select d.IdJobFeeSchedule).ToList();


                JobFeeSchedule additionalservice = rpoContext.JobFeeSchedules.Where(x => x.IdJob == idJob && x.Id == jobFeeScheduleid && x.IsAdditionalService == true && x.Status == "Completed" && !objservie.Contains(x.Id)).FirstOrDefault();

                if (additionalservice != null && timenote != null)
                {

                    JobFeeSchedule jobFeeScheduletmp = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobFeeScheduleid && x.IdJob == idJob);
                    string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeScheduletmp);

                    string jobTimeMessage = JobHistoryMessages.ChangeStatusScopeMessage
                                                                    .Replace("##hours##", hours)
                                                                     .Replace("##ServiceItemName##", jobFeeScheduleDetailName);

                    string changeStatusScopeHistory = JobHistoryMessages.ChangeStatusScopeMessage
                        .Replace("##ServiceItemName##", !string.IsNullOrEmpty(jobFeeScheduleDetailName) ? jobFeeScheduleDetailName : JobHistoryMessages.NoSetstring);

                    Common.SaveJobHistory(employeeid, idJob.Value, changeStatusScopeHistory, JobHistoryType.Scope);


                    SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.AdditionalBilling);
                    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                    {
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/TaskCompleted.htm")))
                        {
                            body = reader.ReadToEnd();
                        }

                        foreach (var item in systemSettingDetail.Value)
                        {
                            string setRedirecturl = string.Empty;
                            if (timenote.IdJob != null)
                            {
                                setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + timenote.IdJob + "" + "/jobtask";
                            }
                            else
                            {
                                setRedirecturl = Properties.Settings.Default.FrontEndUrl + "tasks";
                            }

                            string emailBody = body;
                            emailBody = emailBody.Replace("##EmployeeName##", item.FirstName != null ? item.FirstName : string.Empty);

                            emailBody = emailBody.Replace("##Message##", changeStatusScope);

                            // Common.SendInAppNotifications(item.Id, changeStatusScope, Hub, idJob > 0 || idJob != null ? "job/" + idJob + "/timenotes" : "timenotes");

                            List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                            to.Add(new KeyValuePair<string, string>(item.Email, item.FirstName + " " + item.LastName));

                            List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                            Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, EmailNotificationSubject.AdditionalCompleted.Replace("##Job##", idJob != null && idJob != 0 ? idJob.ToString() + ": " : string.Empty), emailBody, true);
                        }
                    }

                    if (jobTimeMessage != "")
                    {
                        Job job = rpoContext.Jobs.FirstOrDefault(x => x.Id == idJob);

                        List<int> dobProjectTeam = job != null && job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                        List<int> dotProjectTeam = job != null && job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                        List<int> violationProjectTeam = job != null && job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                        List<int> depProjectTeam = job != null && job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                        var resultproject = rpoContext.Employees.Where(x => dotProjectTeam.Contains(x.Id)
                                                                    || violationProjectTeam.Contains(x.Id)
                                                                    || depProjectTeam.Contains(x.Id)
                                                                    || dobProjectTeam.Contains(x.Id)
                                                                    || x.Id == job.IdProjectManager
                                                                    ).Select(x => new
                                                                    {
                                                                        Id = x.Id,
                                                                        ItemName = x.Email

                                                                    }).ToArray();
                        foreach (var itemcomp in resultproject)
                        {
                            Common.SendInAppNotifications(itemcomp.Id, jobTimeMessage, Hub, "job/" + idJob + "/scope");
                        }
                    }
                }
            }
        }
        private void JobMilestoneCompleted(int? milestoneid, int? taskid, int? idJob, string hours)
        {
            if (taskid > 0)
            {
                string javascript = "click=\"redirectFromNotification(j)\"";
                JobTimeNote timenote = rpoContext.JobTimeNotes.Where(x => x.Id == taskid && x.IdJob == idJob).FirstOrDefault();

                if (milestoneid != null && timenote != null)
                {
                    SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.ScopeBilling);
                    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                    {
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/TaskCompleted.htm")))
                        {
                            body = reader.ReadToEnd();
                        }


                        JobMilestone jobMilestone = rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").FirstOrDefault(x => x.Id == milestoneid && x.IdJob == idJob);

                        string houseStreetNameBorrough = jobMilestone != null && jobMilestone.Job.RfpAddress != null ? jobMilestone.Job.RfpAddress.HouseNumber + " " + jobMilestone.Job.RfpAddress.Street + (jobMilestone.Job.RfpAddress.Borough != null ? " " + jobMilestone.Job.RfpAddress.Borough.Description : string.Empty) : string.Empty;
                        string specialPlaceName = jobMilestone != null && jobMilestone.Job.SpecialPlace != null ? " - " + jobMilestone.Job.SpecialPlace : string.Empty;
                        NotificationMails.SendMilestoneCompletedMail(jobMilestone.Name, jobMilestone.Job.JobNumber, houseStreetNameBorrough, specialPlaceName, jobMilestone.IdJob, null, Hub);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the job time note.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the job time note.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobTimeNote))]
        public IHttpActionResult DeleteJobTimeNote(int id)
        {
            JobTimeNote jobTimeNote = rpoContext.JobTimeNotes.Find(id);
            if (jobTimeNote == null)
            {
                return this.NotFound();
            }

            rpoContext.JobTimeNotes.Remove(jobTimeNote);
            rpoContext.SaveChanges();

            return Ok(jobTimeNote);
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
        /// Jobs the time note exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobTimeNoteExists(int id)
        {
            return rpoContext.JobTimeNotes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="jobTimeNote">The job time note.</param>
        /// <returns>JobTimeNoteDetail.</returns>
        private JobTimeNoteDetail FormatDetails(JobTimeNote jobTimeNote)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobTimeNoteDetail
            {
                Id = jobTimeNote.Id,
                JobNumber = jobTimeNote.Job != null ? jobTimeNote.Job.JobNumber : string.Empty,
                JobBillingType = jobTimeNote.JobBillingType,
                ProgressNotes = jobTimeNote.ProgressNotes,
                IdJob = jobTimeNote.IdJob,
                TimeHours = jobTimeNote.TimeHours,
                TimeMinutes = jobTimeNote.TimeMinutes,
                QuickbookSyncError = jobTimeNote.QuickbookSyncError,
                QuickbookSyncedDate = jobTimeNote.QuickbookSyncedDate,
                IsQuickbookSynced = jobTimeNote.IsQuickbookSynced,
                IdRfpJobType = jobTimeNote.IdRfpJobType,
                IdJobFeeSchedule = jobTimeNote.IdJobFeeSchedule,
                //  TimeNoteDate = jobTimeNote.TimeNoteDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNote.TimeNoteDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNote.TimeNoteDate,
                TimeNoteDate = jobTimeNote.TimeNoteDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobTimeNote.TimeNoteDate), DateTimeKind.Utc) : jobTimeNote.TimeNoteDate,
                CreatedBy = jobTimeNote.CreatedBy,
                LastModifiedBy = jobTimeNote.LastModifiedBy,
                CreatedByEmployeeName = jobTimeNote.CreatedByEmployee != null ? jobTimeNote.CreatedByEmployee.FirstName + " " + jobTimeNote.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobTimeNote.LastModifiedByEmployee != null ? jobTimeNote.LastModifiedByEmployee.FirstName + " " + jobTimeNote.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = jobTimeNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNote.CreatedDate,
                LastModifiedDate = jobTimeNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNote.LastModifiedDate,
                //mb flag
                FromProgressionNote = jobTimeNote.FromProgressionNote
            };
        }

        /// <summary>
        /// Formats the specified job time note.
        /// </summary>
        /// <param name="jobTimeNote">The job time note.</param>
        /// <returns>JobTimeNoteDTO.</returns>
        private JobTimeNoteDTO Format(JobTimeNote jobTimeNote)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobTimeNoteDTO
            {
                Id = jobTimeNote.Id,
                JobNumber = jobTimeNote.Job != null ? jobTimeNote.Job.JobNumber : string.Empty,
                JobBillingType = jobTimeNote.JobBillingType,
                ProgressNotes = jobTimeNote.ProgressNotes,
                IdJob = jobTimeNote.IdJob,
                TimeHours = jobTimeNote.TimeHours,
                TimeMinutes = jobTimeNote.TimeMinutes,
                QuickbookSyncError = jobTimeNote.QuickbookSyncError,
                QuickbookSyncedDate = jobTimeNote.QuickbookSyncedDate,
                IsQuickbookSynced = jobTimeNote.IsQuickbookSynced,
                IdRfpJobType = jobTimeNote.IdRfpJobType,
                IdJobFeeSchedule = jobTimeNote.IdJobFeeSchedule,
                ServiceItem = jobTimeNote.JobFeeSchedule != null ? Common.GetServiceItemName(jobTimeNote.JobFeeSchedule) : (jobTimeNote.RfpJobType != null ? Common.GetServiceItemName(jobTimeNote.RfpJobType) : string.Empty),
                //  TimeNoteDate = jobTimeNote.TimeNoteDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNote.TimeNoteDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNote.TimeNoteDate,
                TimeNoteDate = jobTimeNote.TimeNoteDate,// != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobTimeNote.TimeNoteDate), DateTimeKind.Utc) : jobTimeNote.TimeNoteDate,
                CreatedBy = jobTimeNote.CreatedBy,
                LastModifiedBy = jobTimeNote.LastModifiedBy,
                CreatedByEmployeeName = jobTimeNote.CreatedByEmployee != null ? jobTimeNote.CreatedByEmployee.FirstName + " " + jobTimeNote.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobTimeNote.LastModifiedByEmployee != null ? jobTimeNote.LastModifiedByEmployee.FirstName + " " + jobTimeNote.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = jobTimeNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNote.CreatedDate,
                LastModifiedDate = jobTimeNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNote.LastModifiedDate,
                //mb flag
                FromProgressionNote = jobTimeNote.FromProgressionNote
            };
        }
    }
}