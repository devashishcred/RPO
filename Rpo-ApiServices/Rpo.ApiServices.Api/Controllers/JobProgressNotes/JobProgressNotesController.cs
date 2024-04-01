// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 05-08-2023
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-15-2018
// ***********************************************************************
// <copyright file="JobProgressNotesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Progress Notes Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.JobProgressNotes
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Hubs;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using SystemSettings;
    /// <summary>
    /// Class Job Progress Notes Controller.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.Controllers.HubApiController{Rpo.ApiServices.Api.Hubs.GroupHub}" />
    public class JobProgressNotesController : HubApiController<GroupHub>
    {
        /// <summary>
        /// Class Job Progress Notes Advanced Search Parameters.
        /// </summary>
        /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
        public class JobProgressNotesAdvancedSearchParameters : DataTableParameters
        {
            /// <summary>
            /// Gets or sets the identifier Job.
            /// </summary>
            /// <value>The identifier Job.</value>
            public int? IdJob { get; set; }
        }

        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        ///// <summary>
        ///// Gets the Job progress notes.
        ///// </summary>
        ///// <param name="dataTableParameters">The data table parameters.</param>
        ///// <returns>Get the list of progress notes list against the Job Id.</returns>
        //[Authorize]
        //[RpoAuthorize]
        //public IHttpActionResult GetJobProgressNotes([FromUri] JobProgressNotesAdvancedSearchParameters dataTableParameters)
        //{
        //   IQueryable<JobProgressNote> JobProgressNotes = rpoContext.JobProgressNotes.Include("CreatedByEmployee").Include("LastModified");
        //    var recordsTotal = JobProgressNotes.Count();

        //    if (dataTableParameters.IdJob != null)
        //    {
        //        JobProgressNotes = JobProgressNotes.Where(jtn => jtn.IdJob == (int)dataTableParameters.IdJob);
        //    }

        //    var recordsFiltered = recordsTotal;

        //    var result = JobProgressNotes.AsEnumerable().Select(x => Format(x)).OrderByDescending(x => x.LastModifiedDate).AsQueryable()
        //    .DataTableParameters(dataTableParameters, out recordsFiltered);

        //    return Ok(new DataTableResponse
        //    {
        //        Draw = dataTableParameters.Draw,
        //        RecordsFiltered = recordsFiltered,
        //        RecordsTotal = recordsTotal,
        //        Data = result
        //    });
        //}

        /// <summary>
        /// Gets the Job progress note.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Get the detail of progress note against the id</returns>
        //[Authorize]
        //[RpoAuthorize]
        //[ResponseType(typeof(JobProgressNoteDetails))]
        //public IHttpActionResult GetJobProgressNote(int id)
        //{
        //    JobProgressNote JobProgressNote = rpoContext.JobProgressNotes.Find(id);
        //    if (JobProgressNote == null)
        //    {
        //        return this.NotFound();
        //    }

        //    return Ok(FormatDetails(JobProgressNote));
        //}

        /// <summary>
        /// Posts the Job progress note.
        /// </summary>
        /// <remarks>create a new progess note of Jobs and send the project team to send notification.</remarks>
        /// <param name="JobProgressNote">The Job progress note.</param>
        /// <returns>create a new progess note of Jobs.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobProgressNote))]
        public IHttpActionResult PostJobProgressNote(JobProgressNote JobProgressNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            JobProgressNote.LastModifiedDate = DateTime.UtcNow;
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (employee != null)
            {
                JobProgressNote.LastModifiedBy = employee.Id;
            }
          rpoContext.JobProgressNotes.Add(JobProgressNote);
            rpoContext.SaveChanges();

            JobProgressNote JobProgressNoteResponse = rpoContext.JobProgressNotes.Include("LastModified").Include("Job").FirstOrDefault(x => x.Id == JobProgressNote.Id);
           // Include("Job.CreatedBy").FirstOrDefault(x => x.Id == JobProgressNote.Id);
           // this.SendProgressNoteNotification(JobProgressNoteResponse);
            return Ok(FormatDetails(JobProgressNoteResponse));
        }
        
        //private void SendProgressNoteNotification(JobProgressNote JobProgressNoteDetails)
        //{
        //    if (JobProgressNoteDetails.job != null)
        //    {
        //        if (JobProgressNoteDetails.job.CreatedBy != null)
        //        {
        //            string javascript = "click=\"redirectFromNotification(j)\"";
        //            string body = string.Empty;
        //            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/JobProgressNoteTemplate.htm")))
        //            {
        //                body = reader.ReadToEnd();
        //            }

        //            KeyValuePair<string, string> toAddress = new KeyValuePair<string, string>(JobProgressNoteDetails.Job.CreatedBy.Email, JobProgressNoteDetails.Job.CreatedBy.FirstName + " " + JobProgressNoteDetails.Job.CreatedBy.LastName);

        //            string emailBody = body;
        //            emailBody = emailBody.Replace("##EmployeeName##", JobProgressNoteDetails.job.CreatedBy + " " + JobProgressNoteDetails.job.CreatedBy);
        //            emailBody = emailBody.Replace("##ProgressNoteAddedBy##", JobProgressNoteDetails.LastModified != null ? JobProgressNoteDetails.LastModified.FirstName + " " + JobProgressNoteDetails.LastModified.LastName : "Not Set");
        //            emailBody = emailBody.Replace("##JobNumber##", JobProgressNoteDetails.job.JobNumber);
        //            emailBody = emailBody.Replace("##JobProgressNoteDetail##", JobProgressNoteDetails.Notes);
        //            //emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + JobProgressNoteDetails.Job.Id);
        //            emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/JobSubmit/" + JobProgressNoteDetails.job.Id);

        //            string subject = "Progress note added to Job";
        //            if (JobProgressNoteDetails.job.CreatedBy != JobProgressNoteDetails.LastModifiedBy)
        //            {
        //                Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toAddress, subject, emailBody, true);

        //                string progressNoteAddedToJobSetting = InAppNotificationMessage.ProgressNoteAddedToJob
        //                            .Replace("##ProgressNote##", JobProgressNoteDetails.Notes)
        //                            .Replace("##JobNumber##", JobProgressNoteDetails.job.JobNumber)
        //                            .Replace("##JobFullAddress##", JobProgressNoteDetails.job.JobAddress != null ? JobProgressNoteDetails.job.JobAddress.HouseNumber + " " + JobProgressNoteDetails.job.JobAddress.Street + (JobProgressNoteDetails.job.JobAddress.Borough != null ? " " + JobProgressNoteDetails.job.JobAddress.Borough.Description : string.Empty) + (JobProgressNoteDetails.job.City != null ? " " + JobProgressNoteDetails.job.JobAddressCity : string.Empty) + (JobProgressNoteDetails.job.State != null ? " " + JobProgressNoteDetails.job.State.Name : string.Empty) + (JobProgressNoteDetails.job.ZipCode != null ? " " + JobProgressNoteDetails.job.ZipCode : string.Empty) : string.Empty)
        //                //            .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + JobProgressNoteDetails.Job.Id);
        //                //Common.SendInAppNotifications(JobProgressNoteDetails.Job.IdCreatedBy.Value, progressNoteAddedToJobSetting, Hub, "editSiteInformation/" + JobProgressNoteDetails.Job.Id);
        //                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/JobSubmit/" + JobProgressNoteDetails.job.Id);
        //                Common.SendInAppNotifications(JobProgressNoteDetails.job.IdCreatedBy.Value, progressNoteAddedToJobSetting, Hub, "JobSubmit/" + JobProgressNoteDetails.job.Id);

        //            }
        //        }


        //        SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenAProgressNoteIsAddedToJob);
        //        if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
        //        {
        //            foreach (var item in systemSettingDetail.Value)
        //            {
        //                string progressNoteAddedToJobSetting = InAppNotificationMessage.ProgressNoteAddedToJob
        //                            .Replace("##ProgressNote##", JobProgressNoteDetails.Notes)
        //                            .Replace("##JobNumber##", JobProgressNoteDetails.Job.JobNumber)
        //                            .Replace("##JobFullAddress##", JobProgressNoteDetails.Job.JobAddress != null ? JobProgressNoteDetails.Job.JobAddress.HouseNumber + " " + JobProgressNoteDetails.Job.JobAddress.Street + (JobProgressNoteDetails.Job.JobAddress.Borough != null ? " " + JobProgressNoteDetails.Job.JobAddress.Borough.Description : string.Empty) + (JobProgressNoteDetails.Job.City != null ? " " + JobProgressNoteDetails.Job.City : string.Empty) + (JobProgressNoteDetails.Job.State != null ? " " + JobProgressNoteDetails.Job.State.Name : string.Empty) + (JobProgressNoteDetails.Job.ZipCode != null ? " " + JobProgressNoteDetails.Job.ZipCode : string.Empty) : string.Empty)
        //               //                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + JobProgressNoteDetails.Job.Id);
        //               //    Common.SendInAppNotifications(item.Id, progressNoteAddedToJobSetting, Hub, "editSiteInformation/" + JobProgressNoteDetails.Job.Id);
        //               //
        //               .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/JobSubmit/" + JobProgressNoteDetails.Job.Id);
        //                Common.SendInAppNotifications(item.Id, progressNoteAddedToJobSetting, Hub, "JobSubmit/" + JobProgressNoteDetails.Job.Id);

        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Sends the progress note email.
        /// </summary>
        /// <param name="JobProgressNoteDetails">The Job progress note details.</param>
        //private void SendProgressNoteEmail(JobProgressNote JobProgressNoteDetails)
        //{
        //    if (JobProgressNoteDetails.Job != null && JobProgressNoteDetails.Job.CreatedBy != null)
        //    {
        //        string body = string.Empty;
        //        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/JobProgressNoteTemplate.htm")))
        //        {
        //            body = reader.ReadToEnd();
        //        }

        //        if (JobProgressNoteDetails.Job != null && JobProgressNoteDetails.Job.CreatedBy != null && !string.IsNullOrWhiteSpace(JobProgressNoteDetails.Job.CreatedBy.Email))
        //        {
        //            string emailBody = body;
        //            emailBody = emailBody.Replace("##EmployeeName##", JobProgressNoteDetails.Job != null && JobProgressNoteDetails.Job.CreatedBy != null ? JobProgressNoteDetails.Job.CreatedBy.FirstName + " " + JobProgressNoteDetails.Job.CreatedBy.LastName : string.Empty);
        //            emailBody = emailBody.Replace("##JobNumber##", JobProgressNoteDetails.Job.JobNumber);
        //            emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + JobProgressNoteDetails.Job.Id);
        //            emailBody = emailBody.Replace("##JobProgressNoteDetail##", JobProgressNoteDetails.Notes);
        //            emailBody = emailBody.Replace("##ProgressNoteAddedBy##", JobProgressNoteDetails.LastModified != null ? JobProgressNoteDetails.LastModified.FirstName + " " + JobProgressNoteDetails.LastModified.LastName : string.Empty);
        //            KeyValuePair<string, string> toEmpleyee = new KeyValuePair<string, string>(JobProgressNoteDetails.Job.CreatedBy.Email, JobProgressNoteDetails.Job.CreatedBy.FirstName + " " + JobProgressNoteDetails.Job.CreatedBy.LastName);

        //            Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toEmpleyee, "Progress note added to Job", emailBody, true);
        //            Common.SendInAppNotifications(JobProgressNoteDetails.Job.CreatedBy.Id, StaticMessages.NewJobProgressNoteAddedNotificationMessage.Replace("##JobNumber##", JobProgressNoteDetails.Job.JobNumber), Hub, "/editSiteInformation/" + JobProgressNoteDetails.Job.Id);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Sends the progress note email system setting.
        ///// </summary>
        ///// <param name="JobProgressNoteDetails">The Job progress note details.</param>
        //private void SendProgressNoteEmailSystemSetting(JobProgressNote JobProgressNoteDetails)
        //{
        //    SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenAProgressNoteIsAddedToJob);
        //    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
        //    {
        //        foreach (var item in systemSettingDetail.Value)
        //        {
        //            string body = string.Empty;
        //            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/JobProgressNoteTemplate.htm")))
        //            {
        //                body = reader.ReadToEnd();
        //            }

        //            if (!string.IsNullOrWhiteSpace(item.Email))
        //            {
        //                string emailBody = body;
        //                emailBody = emailBody.Replace("##EmployeeName##", item.EmployeeName);
        //                emailBody = emailBody.Replace("##JobNumber##", JobProgressNoteDetails.Job.JobNumber);
        //                emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + JobProgressNoteDetails.Job.Id);
        //                emailBody = emailBody.Replace("##JobProgressNoteDetail##", JobProgressNoteDetails.Notes);
        //                emailBody = emailBody.Replace("##ProgressNoteAddedBy##", JobProgressNoteDetails.LastModified != null ? JobProgressNoteDetails.LastModified.FirstName + " " + JobProgressNoteDetails.LastModified.LastName : string.Empty);
        //                KeyValuePair<string, string> toEmpleyee = new KeyValuePair<string, string>(item.Email, item.EmployeeName);

        //                Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toEmpleyee, "Progress note added to Job", emailBody, true);
        //                Common.SendInAppNotifications(item.Id, StaticMessages.NewJobProgressNoteAddedNotificationMessage.Replace("##JobNumber##", JobProgressNoteDetails.Job.JobNumber), Hub, "/editSiteInformation/" + JobProgressNoteDetails.Job.Id);
        //            }

        //        }
        //    }
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

        /// <summary>
        /// Jobs the progress note exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool JobProgressNoteExists(int id)
        {
          return rpoContext.JobProgressNotes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="JobProgressNote">The Job progress note.</param>
        /// <returns>JobProgressNoteDetails.</returns>
        private JobProgressNoteDetails FormatDetails(JobProgressNote JobProgressNote)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobProgressNoteDetails
            {
                Id = JobProgressNote.Id,
                IdJob = JobProgressNote.IdJob,
                Notes = JobProgressNote.Notes,
                CreatedBy = JobProgressNote.CreatedBy,
                LastModifiedBy = JobProgressNote.LastModifiedBy != null ? JobProgressNote.LastModifiedBy : JobProgressNote.CreatedBy,
                CreatedByEmployee = JobProgressNote.CreatedByEmployee != null ? JobProgressNote.CreatedByEmployee.FirstName + " " + JobProgressNote.CreatedByEmployee.LastName : string.Empty,
                LastModified = JobProgressNote.LastModified != null ? JobProgressNote.LastModified.FirstName + " " + JobProgressNote.LastModified.LastName : (JobProgressNote.CreatedByEmployee != null ? JobProgressNote.CreatedByEmployee.FirstName + " " + JobProgressNote.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = JobProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobProgressNote.CreatedDate,
                LastModifiedDate = JobProgressNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobProgressNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (JobProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobProgressNote.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the specified Job progress note.
        /// </summary>
        /// <param name="JobProgressNote">The Job progress note.</param>
        /// <returns>JobProgressNoteDTO.</returns>
        private JobProgressNoteDTO Format(JobProgressNote JobProgressNote)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobProgressNoteDTO
            {
                Id = JobProgressNote.Id,
                IdJob = JobProgressNote.IdJob,
                Notes = JobProgressNote.Notes,
                CreatedBy = JobProgressNote.CreatedBy,
                LastModifiedBy = JobProgressNote.LastModifiedBy != null ? JobProgressNote.LastModifiedBy : JobProgressNote.CreatedBy,
                CreatedByEmployee = JobProgressNote.CreatedByEmployee != null ? JobProgressNote.CreatedByEmployee.FirstName + " " + JobProgressNote.CreatedByEmployee.LastName : string.Empty,
                LastModified = JobProgressNote.LastModified != null ? JobProgressNote.LastModified.FirstName + " " + JobProgressNote.LastModified.LastName : (JobProgressNote.CreatedByEmployee != null ? JobProgressNote.CreatedByEmployee.FirstName + " " + JobProgressNote.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = JobProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobProgressNote.CreatedDate,
                LastModifiedDate = JobProgressNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobProgressNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (JobProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobProgressNote.CreatedDate),
            };
        }

        [Route("api/Jobs/{idJob}/Jobprogressnotes")]
        [ResponseType(typeof(JobProgressNote))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobProgressNote(int idJob)
        {
            Job Job = rpoContext.Jobs.Find(idJob);
            if (Job == null)
            {
                return this.NotFound();
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = rpoContext.JobProgressNotes
                .Include("LastModified")
                .Include("CreatedByEmployee")
                .Where(x => x.IdJob == idJob)
                .AsEnumerable()
                .Select(JobProgressNote => new
                {
                    Id = JobProgressNote.Id,
                    IdJob = JobProgressNote.IdJob,
                    Notes = JobProgressNote.Notes,
                    CreatedBy = JobProgressNote.CreatedBy,
                    LastModifiedBy = JobProgressNote.LastModifiedBy != null ? JobProgressNote.LastModifiedBy : JobProgressNote.CreatedBy,
                    CreatedByEmployee = JobProgressNote.CreatedByEmployee != null ? JobProgressNote.CreatedByEmployee.FirstName + " " + JobProgressNote.CreatedByEmployee.LastName : string.Empty,
                    LastModified = JobProgressNote.LastModified != null ? JobProgressNote.LastModified.FirstName + " " + JobProgressNote.LastModified.LastName : (JobProgressNote.CreatedByEmployee != null ? JobProgressNote.CreatedByEmployee.FirstName + " " + JobProgressNote.CreatedByEmployee.LastName : string.Empty),
                    CreatedDate = JobProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobProgressNote.CreatedDate,
                    LastModifiedDate = JobProgressNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobProgressNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (JobProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(JobProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : JobProgressNote.CreatedDate)
                }).OrderByDescending(x => x.LastModifiedDate);

            return Ok(result);
        }
    }
}