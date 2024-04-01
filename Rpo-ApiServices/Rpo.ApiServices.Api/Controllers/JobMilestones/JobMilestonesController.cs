// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobMilestonesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Milestones Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Job Milestones namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobMilestones
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Hubs;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using SystemSettings;

    /// <summary>
    /// Class Job Milestones Controller.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.Controllers.HubApiController{Rpo.ApiServices.Api.Hubs.GroupHub}" />
    public class JobMilestonesController : HubApiController<GroupHub>
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the milestones.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <returns>Gets the milestones List.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobMilestones/{idJob}")]
        [ResponseType(typeof(IEnumerable<JobMilestoneDTO>))]
        public IHttpActionResult GetMilestones(int idJob)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJobMilestone)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobMilestone)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJobMilestone))
            {
                var jobMilestone = rpoContext.JobMilestones.Include("JobMilestoneServices.JobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent").Include("ModifiedByEmployee").Where(j => j.IdJob == idJob).AsQueryable();

                if (jobMilestone == null)
                {
                    return this.NotFound();
                }

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                var result = jobMilestone.AsEnumerable().Select(milestone => Format(milestone));

                return Ok(result);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the milestones.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="idJobMilestone">The identifier job milestone.</param>
        /// <returns>Gets the milestones List.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobMilestones/{idJob}/{idJobMilestone}")]
        [ResponseType(typeof(IEnumerable<JobMilestoneDTO>))]
        public IHttpActionResult GetMilestones(int idJob, int idJobMilestone)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobMilestone))
            {
                var jobMilestone = rpoContext.JobMilestones.Include("JobMilestoneServices.JobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent").Include("ModifiedByEmployee").FirstOrDefault(j => j.Id == idJobMilestone);

                if (jobMilestone == null)
                {
                    return this.NotFound();
                }

                return Ok(Format(jobMilestone));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the milestone.
        /// </summary>
        /// <param name="idJobMilestone">The identifier job milestone.</param>
        /// <param name="jobMilestone">The job milestone.</param>
        /// <returns>update the detail of milestone.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobMilestones/{idJobMilestone}")]
        [ResponseType(typeof(JobMilestoneDTO))]
        public IHttpActionResult PutMilestone(int idJobMilestone, JobMilestoneCreateUpdate jobMilestone)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobMilestone))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var milestone = rpoContext.JobMilestones.FirstOrDefault(M => M.Id == idJobMilestone);

                if (milestone == null)
                {
                    return this.NotFound();
                }

                double? oldValue = milestone.Value;

                milestone.IdJob = jobMilestone.IdJob;
                milestone.Name = jobMilestone.Name;
                milestone.Value = jobMilestone.Value;
                milestone.Status = jobMilestone.Status;
                milestone.LastModified = DateTime.UtcNow;
                milestone.ModifiedBy = employee.Id;

                rpoContext.SaveChanges();

                ICollection<JobMilestoneService> jobMilestoneServicesList = milestone.JobMilestoneServices.Select(x => x).ToList();

                foreach (var item in jobMilestoneServicesList)
                {
                    var jobMil = jobMilestone.JobMilestoneServices.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (jobMil == null)
                    {
                        rpoContext.JobMilestoneServices.Remove(item);
                    }
                }

                if (jobMilestone.JobMilestoneServices != null)
                {
                    foreach (var item in jobMilestone.JobMilestoneServices)
                    {
                        if (item.Id > 0)
                        {
                            JobMilestoneService jobMilestoneService = rpoContext.JobMilestoneServices.FirstOrDefault(x => x.Id == item.Id);

                            if (jobMilestoneService == null)
                            {
                                return this.NotFound();
                            }

                            jobMilestoneService.IdJobFeeSchedule = item.IdJobFeeSchedule;
                            jobMilestoneService.IdMilestone = item.IdMilestone;
                        }
                        else
                        {
                            JobMilestoneService jobMilestoneService = new JobMilestoneService();
                            jobMilestoneService.IdJobFeeSchedule = item.IdJobFeeSchedule;
                            jobMilestoneService.IdMilestone = item.IdMilestone;
                            rpoContext.JobMilestoneServices.Add(jobMilestoneService);
                        }
                    }
                }

                rpoContext.SaveChanges();

                string editMilestone = JobHistoryMessages.EditMilestone
                    .Replace("##BillingPointName##", !string.IsNullOrEmpty(milestone.Name) ? milestone.Name : JobHistoryMessages.NoSetstring);
                Common.SaveJobHistory(employee.Id, milestone.IdJob, editMilestone, JobHistoryType.Milestone);

                if (jobMilestone.Status == "Completed")
                {
                    JobMilestone completedMilestone = rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").FirstOrDefault(x => x.Id == milestone.Id);
                    string houseStreetNameBorrough = completedMilestone != null && completedMilestone.Job.RfpAddress != null ? completedMilestone.Job.RfpAddress.HouseNumber + " " + completedMilestone.Job.RfpAddress.Street + (completedMilestone.Job.RfpAddress.Borough != null ? " " + completedMilestone.Job.RfpAddress.Borough.Description : string.Empty) : string.Empty;
                    string specialPlaceName = completedMilestone != null && completedMilestone.Job.SpecialPlace != null ? " - " + completedMilestone.Job.SpecialPlace : string.Empty;
                    NotificationMails.SendMilestoneCompletedMail(completedMilestone.Name, completedMilestone.Job.JobNumber, houseStreetNameBorrough, specialPlaceName, completedMilestone.IdJob, null, Hub);
                }

                UpdateMilestoneStatus(milestone.Id);
                return Ok(Format(milestone));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Update the scope milestone
        /// </summary>
        /// <param name="idJobMilestone"></param>
        /// <param name="jobMilestone"></param>
        /// <returns>Update the scope milestone</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/ScopeMilestones/{idJobMilestone}")]
        [ResponseType(typeof(JobMilestoneDTO))]
        public IHttpActionResult PutScopeMilestone(int idJobMilestone, JobMilestoneCreateUpdate jobMilestone)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobMilestone))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var milestone = rpoContext.JobMilestones.FirstOrDefault(M => M.Id == idJobMilestone);

                if (milestone == null)
                {
                    return this.NotFound();
                }

                double? oldValue = milestone.Value;

                milestone.IdJob = jobMilestone.IdJob;
                milestone.Name = jobMilestone.Name;
                milestone.Value = jobMilestone.Value;
                milestone.Status = jobMilestone.Status;
                milestone.LastModified = DateTime.UtcNow;
                milestone.ModifiedBy = employee.Id;

                rpoContext.SaveChanges();

                ICollection<JobMilestoneService> jobMilestoneServicesList = milestone.JobMilestoneServices.Select(x => x).ToList();

                foreach (var item in jobMilestoneServicesList)
                {
                    rpoContext.JobMilestoneServices.Remove(item);
                }

                if (jobMilestone.JobMilestoneServices != null)
                {
                    foreach (var item in jobMilestone.JobMilestoneServices)
                    {
                        //if (item.Id > 0)
                        //{
                        //    JobMilestoneService jobMilestoneService = rpoContext.JobMilestoneServices.FirstOrDefault(x => x.Id == item.Id);

                        //    if (jobMilestoneService == null)
                        //    {
                        //        return this.NotFound();
                        //    }

                        //    jobMilestoneService.IdJobFeeSchedule = item.IdJobFeeSchedule;
                        //    jobMilestoneService.IdMilestone = item.IdMilestone;
                        //}
                        //else
                        //{
                        JobMilestoneService jobMilestoneService = new JobMilestoneService();
                        jobMilestoneService.IdJobFeeSchedule = item.IdJobFeeSchedule;
                        jobMilestoneService.IdMilestone = idJobMilestone;
                        rpoContext.JobMilestoneServices.Add(jobMilestoneService);

                        //JobFeeSchedule objjobfeeschedule = rpoContext.JobFeeSchedules.FirstOrDefault(d => d.Id == item.IdJobFeeSchedule);

                        //if(objjobfeeschedule !=null)
                        //{
                        //    objjobfeeschedule.IsAdditionalService = false;
                        //    rpoContext.SaveChanges();
                        //}



                        // }
                    }
                }

                rpoContext.SaveChanges();

                string editMilestone = JobHistoryMessages.EditMilestone
                    .Replace("##BillingPointName##", !string.IsNullOrEmpty(milestone.Name) ? milestone.Name : JobHistoryMessages.NoSetstring);

                Common.SaveJobHistory(employee.Id, milestone.IdJob, editMilestone, JobHistoryType.Milestone);

                if (jobMilestone.Status == "Completed")
                {
                    JobMilestone completedMilestone = rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").FirstOrDefault(x => x.Id == milestone.Id);
                    string houseStreetNameBorrough = completedMilestone != null && completedMilestone.Job.RfpAddress != null ? completedMilestone.Job.RfpAddress.HouseNumber + " " + completedMilestone.Job.RfpAddress.Street + (completedMilestone.Job.RfpAddress.Borough != null ? " " + completedMilestone.Job.RfpAddress.Borough.Description : string.Empty) : string.Empty;
                    string specialPlaceName = completedMilestone != null && completedMilestone.Job.SpecialPlace != null ? " - " + completedMilestone.Job.SpecialPlace : string.Empty;
                    NotificationMails.SendMilestoneCompletedMail(completedMilestone.Name, completedMilestone.Job.JobNumber, houseStreetNameBorrough, specialPlaceName, completedMilestone.IdJob, null, Hub);
                }

                UpdateMilestoneStatus(milestone.Id);
                return Ok(Format(milestone));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }


        /// <summary>
        /// Posts the milestone.
        /// </summary>
        /// <param name="idjob">The idjob.</param>
        /// <param name="jobMilestone">The job milestone.</param>
        /// <returns>create a new milestone in scope.</returns>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobMilestones/{idjob}")]
        [ResponseType(typeof(JobMilestone))]
        public IHttpActionResult PostMilestone(int idjob, JobMilestoneCreateUpdate jobMilestone)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobMilestone))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                JobMilestone milestone = new JobMilestone();
                milestone.IdJob = jobMilestone.IdJob;
                milestone.Name = jobMilestone.Name;
                milestone.Value = jobMilestone.Value;
                milestone.Status = jobMilestone.Status;
                milestone.LastModified = DateTime.UtcNow;
                milestone.CreatedDate = DateTime.UtcNow;
                milestone.ModifiedBy = employee.Id;
                milestone.CreatedBy = employee.Id;

                rpoContext.JobMilestones.Add(milestone);

                rpoContext.SaveChanges();

                if (jobMilestone.JobMilestoneServices != null)
                {
                    foreach (var item in jobMilestone.JobMilestoneServices)
                    {

                        JobMilestoneService jobMilestoneService = new JobMilestoneService();
                        jobMilestoneService.IdJobFeeSchedule = item.IdJobFeeSchedule;
                        jobMilestoneService.IdMilestone = milestone.Id;
                        rpoContext.JobMilestoneServices.Add(jobMilestoneService);
                    }
                }

                rpoContext.SaveChanges();

                string addMilestone = JobHistoryMessages.AddMilestone
                                                        .Replace("##BillingPointName##", milestone != null ? milestone.Name : JobHistoryMessages.NoSetstring);

                Common.SaveJobHistory(employee.Id, milestone.IdJob, addMilestone, JobHistoryType.Milestone);

                UpdateMilestoneStatus(milestone.Id);

                JobMilestone completedMilestone = rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").FirstOrDefault(x => x.Id == milestone.Id);

                SendJobMilestoneAddedNotification(completedMilestone.IdJob, completedMilestone.Name);

                if (jobMilestone.Status == "Completed")
                {
                    string houseStreetNameBorrough = completedMilestone != null && completedMilestone.Job.RfpAddress != null ? completedMilestone.Job.RfpAddress.HouseNumber + " " + completedMilestone.Job.RfpAddress.Street + (completedMilestone.Job.RfpAddress.Borough != null ? " " + completedMilestone.Job.RfpAddress.Borough.Description : string.Empty) : string.Empty;
                    string specialPlaceName = completedMilestone != null && completedMilestone.Job.SpecialPlace != null ? " - " + completedMilestone.Job.SpecialPlace : string.Empty;
                    NotificationMails.SendMilestoneCompletedMail(completedMilestone.Name, completedMilestone.Job.JobNumber, houseStreetNameBorrough, specialPlaceName, completedMilestone.IdJob, null, Hub);
                }

                return Ok(Format(milestone));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the milestone.
        /// </summary>
        /// <param name="idJobMilestone">The identifier job milestone.</param>
        /// <returns>delete the milestone in table.</returns>
        [HttpDelete]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobMilestones/{id}")]
        [ResponseType(typeof(string))]
        public IHttpActionResult DeleteMilestone(int id)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJobMilestone))
            {
                JobMilestone milestone = rpoContext.JobMilestones.FirstOrDefault(m => m.Id == id);

                if (milestone == null)
                {
                    return this.NotFound();
                }
                string deleteMilestone = JobHistoryMessages.DeleteMilestone
                 .Replace("##BillingPointName##", !string.IsNullOrEmpty(milestone.Name) ? milestone.Name : JobHistoryMessages.NoSetstring);

                if (milestone.JobMilestoneServices != null && milestone.JobMilestoneServices.Count > 0)
                {
                    foreach (var item in milestone.JobMilestoneServices)
                    {
                        var jobFeeScheduleRemove = this.rpoContext.JobFeeSchedules.Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.Id == item.IdJobFeeSchedule).ToList();
                        if (jobFeeScheduleRemove != null)
                        {
                            jobFeeScheduleRemove.ForEach(x => x.IsRemoved = true);
                            rpoContext.SaveChanges();
                        }
                    }
                    rpoContext.JobMilestoneServices.RemoveRange(milestone.JobMilestoneServices);
                }

                Common.SaveJobHistory(employee.Id, milestone.IdJob, deleteMilestone, JobHistoryType.Milestone);

                rpoContext.JobMilestones.Remove(milestone);

                rpoContext.SaveChanges();


                return Ok("Deleted");
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job milestones status.
        /// </summary>
        /// <param name="idJobMilestone">The identifier job milestone.</param>
        /// <param name="milestoneStatus">The milestone status.</param>
        /// <returns>update the status of milestone.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobMilestones/status")]
        public IHttpActionResult PutJobMilestonesStatus(JobMilestoneStatus jobMilestoneStatus)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobMilestone))
            {
                JobMilestone jobMilestone = rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").FirstOrDefault(x => x.Id == jobMilestoneStatus.IdJobMilestone);

                string oldStatus = jobMilestone.Status;

                jobMilestone.Status = jobMilestoneStatus.MilestoneStatus;
                jobMilestone.ModifiedBy = employee.Id;
                jobMilestone.LastModified = DateTime.UtcNow;
                rpoContext.SaveChanges();


                string changeStatusMilestone = JobHistoryMessages.ChangeStatusMilestone
                   .Replace("##BillingPointName##", !string.IsNullOrEmpty(jobMilestone.Name) ? jobMilestone.Name : JobHistoryMessages.NoSetstring)
                   .Replace("##OldStatus##", !string.IsNullOrEmpty(oldStatus) ? oldStatus : JobHistoryMessages.NoSetstring)
                   .Replace("##NewStatus##", !string.IsNullOrEmpty(jobMilestone.Status) ? jobMilestone.Status : JobHistoryMessages.NoSetstring);

                Common.SaveJobHistory(employee.Id, jobMilestone.IdJob, changeStatusMilestone, JobHistoryType.Milestone);

                if (jobMilestoneStatus.MilestoneStatus == "Completed")
                {
                    string houseStreetNameBorrough = jobMilestone != null && jobMilestone.Job.RfpAddress != null ? jobMilestone.Job.RfpAddress.HouseNumber + " " + jobMilestone.Job.RfpAddress.Street + (jobMilestone.Job.RfpAddress.Borough != null ? " " + jobMilestone.Job.RfpAddress.Borough.Description : string.Empty) : string.Empty;
                    string specialPlaceName = jobMilestone != null && jobMilestone.Job.SpecialPlace != null ? " - " + jobMilestone.Job.SpecialPlace : string.Empty;
                    NotificationMails.SendMilestoneCompletedMail(jobMilestone.Name, jobMilestone.Job.JobNumber, houseStreetNameBorrough, specialPlaceName, jobMilestone.IdJob, null, Hub);
                }


                JobMilestone jobMilestoneafter = rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").FirstOrDefault(x => x.Id == jobMilestoneStatus.IdJobMilestone);

                return Ok(Format(jobMilestoneafter));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job milestones po number.
        /// </summary>
        /// <param name="idJobMilestone">The identifier job milestone.</param>
        /// <param name="poNumber">The po number.</param>
        /// <returns>update job milestone for ponumber.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobMilestones/ponumber")]
        public IHttpActionResult PutJobMilestonesPONumber(JobMilestonePONumber jobMilestonePONumber)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobMilestone))
            {
                JobMilestone jobMilestone = rpoContext.JobMilestones.Include("JobMilestoneServices.JobFeeSchedule").FirstOrDefault(x => x.Id == jobMilestonePONumber.IdJobMilestone);

                if (jobMilestone == null)
                {
                    return NotFound();
                }

                string oldPONumber = jobMilestone.PONumber;
                jobMilestone.PONumber = jobMilestonePONumber.PONumber;
                jobMilestone.ModifiedBy = employee.Id;
                jobMilestone.LastModified = DateTime.UtcNow;
                if (jobMilestone.JobMilestoneServices != null && jobMilestone.JobMilestoneServices.Count > 0)
                {
                    foreach (var item in jobMilestone.JobMilestoneServices)
                    {
                        if (item.JobFeeSchedule != null)
                        {
                            item.JobFeeSchedule.PONumber = jobMilestonePONumber.PONumber;
                        }
                    }
                }

                rpoContext.SaveChanges();

                string poNumberMilestone = JobHistoryMessages.PONumberMilestone
                   .Replace("##BillingPointName##", jobMilestone.Name)
                   .Replace("##OldPONumber##", oldPONumber)
                   .Replace("##PONumber##", jobMilestone.PONumber)
                   .Replace("##NewPONumber##", jobMilestone.PONumber);

                Common.SaveJobHistory(employee.Id, jobMilestone.IdJob, poNumberMilestone, JobHistoryType.Milestone);

                return Ok(Format(jobMilestone));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job job milestones invoice number.
        /// </summary>
        /// <param name="idJobMilestone">The identifier job milestone.</param>
        /// <param name="invoiceNumber">The invoice number.</param>
        /// <returns>update job milestones invoice number.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobMilestones/invoicenumber")]
        public IHttpActionResult PutJobJobMilestonesInvoiceNumber(JobMilestoneInvoiceNumber jobMilestoneInvoiceNumber)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobMilestone))
            {
                JobMilestone jobMilestone = rpoContext.JobMilestones.Include("JobMilestoneServices.JobFeeSchedule").FirstOrDefault(x => x.Id == jobMilestoneInvoiceNumber.IdJobMilestone);

                if (jobMilestone == null)
                {
                    return NotFound();
                }

                string oldInvoiceNumber = jobMilestone.InvoiceNumber;

                jobMilestone.InvoiceNumber = jobMilestoneInvoiceNumber.InvoiceNumber;
                jobMilestone.ModifiedBy = employee.Id;
                jobMilestone.LastModified = DateTime.UtcNow;
                if (jobMilestone.JobMilestoneServices != null && jobMilestone.JobMilestoneServices.Count > 0)
                {
                    foreach (var item in jobMilestone.JobMilestoneServices)
                    {
                        if (item.JobFeeSchedule != null)
                        {
                            item.JobFeeSchedule.InvoiceNumber = jobMilestoneInvoiceNumber.InvoiceNumber;
                            var jobFeeSchedule2 = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdParentof == item.IdJobFeeSchedule).ToList();
                            jobFeeSchedule2.ForEach(x => x.InvoiceNumber = jobMilestoneInvoiceNumber.InvoiceNumber);
                            rpoContext.SaveChanges();

                        }
                    }
                }

                rpoContext.SaveChanges();

                string invoiceNumberMilestone = JobHistoryMessages.InvoiceNumberMilestone
                .Replace("##InvoiceNumber##", !string.IsNullOrEmpty(jobMilestone.InvoiceNumber) ? jobMilestone.InvoiceNumber : "-")
                .Replace("##NewInvoiceNumber##", !string.IsNullOrEmpty(jobMilestone.InvoiceNumber) ? jobMilestone.InvoiceNumber : "-")
                .Replace("##OldInvoiceNumber##", !string.IsNullOrEmpty(oldInvoiceNumber) ? oldInvoiceNumber : "-")
                .Replace("##BillingPointName##", jobMilestone != null ? jobMilestone.Name : string.Empty);

                Common.SaveJobHistory(employee.Id, jobMilestone.IdJob, invoiceNumberMilestone, JobHistoryType.Milestone);

                return Ok(Format(jobMilestone));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job milestones is invoiced.
        /// </summary>
        /// <param name="idJobMilestone">The identifier job milestone.</param>
        /// <param name="isInvoiced">if set to <c>true</c> [is invoiced].</param>
        /// <returns>update the detail of the job milestones is invoiced.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobMilestones/isinvoiced")]
        public IHttpActionResult PutJobMilestonesIsInvoiced(JobMilestoneIsInvoiced jobMilestoneIsInvoiced)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobMilestone))
            {
                JobMilestone jobMilestone = rpoContext.JobMilestones.Include("JobMilestoneServices.JobFeeSchedule").FirstOrDefault(x => x.Id == jobMilestoneIsInvoiced.IdJobMilestone);

                if (jobMilestone == null)
                {
                    return NotFound();
                }

                jobMilestone.IsInvoiced = jobMilestoneIsInvoiced.IsInvoiced;
                jobMilestone.ModifiedBy = employee.Id;
                jobMilestone.LastModified = DateTime.UtcNow;
                if (jobMilestone.JobMilestoneServices != null && jobMilestone.JobMilestoneServices.Count > 0)
                {
                    foreach (var item in jobMilestone.JobMilestoneServices)
                    {
                        if (item.JobFeeSchedule != null)
                        {
                            item.JobFeeSchedule.IsInvoiced = jobMilestoneIsInvoiced.IsInvoiced;
                        }
                    }
                }

                rpoContext.SaveChanges();

                return Ok(Format(jobMilestone));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Puts the job milestones is invoiced.
        /// </summary>
        /// <param name="jobMilestoneInvoicedDate">The job milestone invoiced date.</param>
        /// <returns>update the invoice date.</returns>
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/JobMilestones/invoicedDate")]
        public IHttpActionResult PutJobMilestonesInvoicedDate(JobMilestoneInvoicedDate jobMilestoneInvoicedDate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJobMilestone))
            {
                DateTime parsedInvoicedDate = new DateTime();
                if (!string.IsNullOrEmpty(jobMilestoneInvoicedDate.InvoicedDate) && !DateTime.TryParse(jobMilestoneInvoicedDate.InvoicedDate, out parsedInvoicedDate))
                {
                    throw new RpoBusinessException(StaticMessages.InvalidInvoiceDateMessage);
                }

                string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

                JobMilestone jobMilestone = rpoContext.JobMilestones.Include("JobMilestoneServices.JobFeeSchedule").FirstOrDefault(x => x.Id == jobMilestoneInvoicedDate.IdJobMilestone);

                if (jobMilestone == null)
                {
                    return NotFound();
                }

                DateTime? oldInvoiceDate = jobMilestone.InvoicedDate;
                DateTime? invoicedDate = new DateTime();

                if (string.IsNullOrEmpty(jobMilestoneInvoicedDate.InvoicedDate))
                {
                    invoicedDate = null;
                }
                else
                {
                    invoicedDate = parsedInvoicedDate;
                }

                jobMilestone.InvoicedDate = invoicedDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(invoicedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : invoicedDate;
                jobMilestone.ModifiedBy = employee.Id;
                jobMilestone.LastModified = DateTime.UtcNow;

                if (jobMilestone.JobMilestoneServices != null && jobMilestone.JobMilestoneServices.Count > 0)
                {
                    foreach (var item in jobMilestone.JobMilestoneServices)
                    {
                        if (item.JobFeeSchedule != null)
                        {
                            item.JobFeeSchedule.InvoicedDate = invoicedDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(invoicedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : invoicedDate;
                        }
                    }
                }

                string invoiceDateMilestone = JobHistoryMessages.InvoiceDateMilestone
                .Replace("##InvoiceNumber##", !string.IsNullOrEmpty(jobMilestone.InvoiceNumber) ? jobMilestone.InvoiceNumber : "-")
                .Replace("##NewInvoiceDate##", jobMilestone != null && jobMilestone.InvoicedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobMilestone.InvoicedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToShortDateString() : "-")
                .Replace("##OldInvoiceDate##", oldInvoiceDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(oldInvoiceDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToShortDateString() : "-")
                .Replace("##BillingPointName##", jobMilestone != null ? jobMilestone.Name : string.Empty);

                rpoContext.SaveChanges();

                Common.SaveJobHistory(employee.Id, jobMilestone.IdJob, invoiceDateMilestone, JobHistoryType.Milestone);

                return Ok(Format(jobMilestone));
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
        /// Formats the specified job milestone.
        /// </summary>
        /// <param name="jobMilestone">The job milestone.</param>
        /// <returns>Job Milestone DTO.</returns>
        private JobMilestoneDTO Format(JobMilestone jobMilestone)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            JobMilestoneDTO jobMilestoneDTO = new JobMilestoneDTO();
            jobMilestoneDTO.Id = jobMilestone.Id;
            jobMilestoneDTO.IdJob = jobMilestone.IdJob;
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
                ItemName = mi.JobFeeSchedule != null && mi.JobFeeSchedule.RfpWorkType != null ? FormatDetail(mi.JobFeeSchedule) : string.Empty
            }).ToList() : null;
            jobMilestoneDTO.LastModified = jobMilestone.LastModified != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobMilestone.LastModified), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobMilestone.LastModified;
            jobMilestoneDTO.LastModifiedBy = jobMilestone.ModifiedByEmployee != null ? (jobMilestone.ModifiedByEmployee.FirstName + " " + (jobMilestone.ModifiedByEmployee.LastName != null ? jobMilestone.ModifiedByEmployee.LastName : string.Empty)) : string.Empty;

            return jobMilestoneDTO;
        }

        /// <summary>
        /// Formats the detail.
        /// </summary>
        /// <param name="rfpFeeSchedule">The RFP fee schedule.</param>
        /// <returns>System String.</returns>
        private string FormatDetail(JobFeeSchedule jobFeeSchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(Request, Common.CurrentTimezoneHeaderKey);

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


            rfpServiceItem = (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);

            return rfpServiceItem;
        }

        /// <summary>
        /// Sends the milestone added mail.
        /// </summary>
        /// <param name="milestoneName">Name of the milestone.</param>
        /// <param name="jobNumber">The job number.</param>
        /// <param name="idJobMilestone">The identifier job milestone.</param>
        /// <param name="idJob">The identifier job.</param>
        //private void SendMilestoneAddedMail(string milestoneName, string jobNumber, int idJob)
        //{
        //    SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenNewJobMilestoneIsAdded);
        //    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
        //    {
        //        foreach (var item in systemSettingDetail.Value)
        //        {
        //            string body = string.Empty;
        //            string htmTemplate = "NewJobMilestoneAddedTemplate.htm";
        //            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/" + htmTemplate)))
        //            {
        //                body = reader.ReadToEnd();
        //            }
        //            string emailBody = body;
        //            emailBody = emailBody.Replace("##EmployeeName##", item.EmployeeName);
        //            emailBody = emailBody.Replace("##MilestoneName##", milestoneName);
        //            emailBody = emailBody.Replace("##JobNumber##", jobNumber);
        //            emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + idJob + "/application");

        //            Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), new KeyValuePair<string, string>(item.Email, item.EmployeeName), "New job milestone is added", emailBody, true);
        //            Common.SendInAppNotifications(item.Id, StaticMessages.NewJobMileStoneNotificationMessage.Replace("##JobNumber##", jobNumber), Hub, "/job/" + idJob + "/application");
        //        }
        //    }
        //}

        private void SendJobMilestoneAddedNotification(int id, string milestoneName)
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

                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenNewJobMilestoneIsAdded);
                if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                {
                    foreach (Employees.EmployeeDetail employeeDetail in systemSettingDetail.Value)
                    {
                        if (employeeDetail.Email != employee.Email && !employeelist.Contains(employeeDetail.Id))
                        {
                            string newJobMileStoneAddedSetting = InAppNotificationMessage.NewJobMileStoneAdded
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##BillingPointName##", milestoneName)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/scope");
                            Common.SendInAppNotifications(employeeDetail.Id, newJobMileStoneAddedSetting, Hub, "job/" + jobs.Id + "/scope");
                        }
                    }
                }

                string newJobMileStoneAdded = InAppNotificationMessage.NewJobMileStoneAdded
                                .Replace("##JobNumber##", jobs.JobNumber)
                                .Replace("##BillingPointName##", milestoneName)
                                .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/scope");
                Common.SendInAppNotifications(employee.Id, newJobMileStoneAdded, Hub, "job/" + jobs.Id + "/scope");
            }
        }

        /// <summary>
        /// Updates the milestone status.
        /// </summary>
        /// <param name="idJobMilestone">The identifier job milestone.</param>
        private void UpdateMilestoneStatus(int idJobMilestone)
        {
            JobMilestone jobMilestone = rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").FirstOrDefault(x => x.Id == idJobMilestone);
            if (jobMilestone != null)
            {
                if (jobMilestone.Status != "Completed")
                {
                    int totalServiceCount = rpoContext.JobMilestoneServices.Include("JobFeeSchedule").Where(x => x.IdMilestone == jobMilestone.Id && x.JobFeeSchedule.IsRemoved != true).Count();
                    int completedServiceCount = rpoContext.JobMilestoneServices.Include("JobFeeSchedule").Where(x => x.IdMilestone == jobMilestone.Id && x.JobFeeSchedule.IsRemoved != true && x.JobFeeSchedule.Status == "Completed").Count();
                    if (totalServiceCount > 0 && completedServiceCount > 0 && totalServiceCount == completedServiceCount)
                    {
                        jobMilestone.Status = "Completed";
                        jobMilestone.LastModified = DateTime.UtcNow;
                        rpoContext.SaveChanges();

                        string houseStreetNameBorrough = jobMilestone != null && jobMilestone.Job.RfpAddress != null ? jobMilestone.Job.RfpAddress.HouseNumber + " " + jobMilestone.Job.RfpAddress.Street + (jobMilestone.Job.RfpAddress.Borough != null ? " " + jobMilestone.Job.RfpAddress.Borough.Description : string.Empty) : string.Empty;
                        string specialPlaceName = jobMilestone != null && jobMilestone.Job.SpecialPlace != null ? " - " + jobMilestone.Job.SpecialPlace : string.Empty;
                        NotificationMails.SendMilestoneCompletedMail(jobMilestone.Name, jobMilestone.Job.JobNumber, houseStreetNameBorrough, specialPlaceName, jobMilestone.IdJob, null, Hub);
                    }
                }
            }
        }
    }
}
