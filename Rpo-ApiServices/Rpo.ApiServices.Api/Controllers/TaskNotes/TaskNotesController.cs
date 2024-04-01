// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-24-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="TaskNotesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Task Notes Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Task Notes namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.TaskNotes
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Api.Jobs;
    using Filters;
    using Hubs;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using SystemSettings;

    /// <summary>
    /// Class Task Notes Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class TaskNotesController : HubApiController<GroupHub>
    {
        /// <summary>
        /// Class TasksNotesAdvancedSearchParameters.
        /// </summary>
        /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
        public class TasksNotesAdvancedSearchParameters : DataTableParameters
        {
            /// <summary>
            /// Gets or sets the identifier task.
            /// </summary>
            /// <value>The identifier task.</value>
            public int? IdTask { get; set; }
        }

        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the task notes.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the task notes List.</returns>
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetTaskNotes([FromUri] TasksNotesAdvancedSearchParameters dataTableParameters)
        {
            IQueryable<TaskNote> taskNotes = rpoContext.TaskNotes;
            var recordsTotal = taskNotes.Count();

            if (dataTableParameters.IdTask != null)
            {
                taskNotes = taskNotes.Where(jtn => jtn.IdTask == (int)dataTableParameters.IdTask);
            }

            var recordsFiltered = recordsTotal;

            var result = taskNotes.OrderByDescending(x => x.LastModifiedDate)
            .DataTableParameters(dataTableParameters, out recordsFiltered);

            return Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result
            });
        }

        /// <summary>
        /// Gets the task note.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the task note.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskNote))]
        public IHttpActionResult GetTaskNote(int id)
        {
            TaskNote taskNote = rpoContext.TaskNotes.Find(id);
            if (taskNote == null)
            {
                return this.NotFound();
            }

            return Ok(taskNote);
        }

        /// <summary>
        /// Posts the task note.
        /// </summary>
        /// <param name="taskNote">The task note.</param>
        /// <returns>create new task notes.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskNote))]
        public IHttpActionResult PostTaskNote(TaskNote taskNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            taskNote.LastModifiedDate = DateTime.UtcNow;
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (employee != null)
            {
                taskNote.LastModifiedBy = employee.Id;
            }
            rpoContext.TaskNotes.Add(taskNote);
            rpoContext.SaveChanges();

            TaskNote tasknoteDetails = rpoContext.TaskNotes.Include("LastModified").Include("Task.TaskType").Include("Task.AssignedBy").Include("Task.AssignedTo").Where(x => x.Id == taskNote.Id).FirstOrDefault();

            Job job = rpoContext.Jobs.Include("RfpAddress").Include("RfpAddress.Borough").FirstOrDefault(x => x.Id == tasknoteDetails.Task.IdJob);
            string JobAddress = string.Empty;
            string subjectmessage1 = EmailNotificationSubject.ProgressNoteAddedToTask;
            string subjectmessage = string.Empty;
            if (job != null)
            {
                JobAddress = job != null && job.RfpAddress != null ? (!string.IsNullOrEmpty(job.RfpAddress.HouseNumber) ? job.RfpAddress.HouseNumber : string.Empty) + " " + (!string.IsNullOrEmpty(job.RfpAddress.Street) ? job.RfpAddress.Street : string.Empty) + " " + (job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : JobHistoryMessages.NoSetstring) + " " + (!string.IsNullOrEmpty(job.RfpAddress.ZipCode) ? job.RfpAddress.ZipCode : string.Empty) + " " + (!string.IsNullOrEmpty(job.SpecialPlace) ? "-" + job.SpecialPlace : string.Empty) : string.Empty;
                subjectmessage = subjectmessage1 + " " + "for Job# "+job.JobNumber+" at "+ JobAddress + "";
            }
            else if (tasknoteDetails.Task.IdRfp != null)
            {
                Rfp rfp = rpoContext.Rfps.FirstOrDefault(d => d.Id == tasknoteDetails.Task.IdRfp);

                string address_Street = rfp.RfpAddress != null && rfp.RfpAddress.Street != null ? rfp.RfpAddress.Street : string.Empty;
                string address_HouseNumber = rfp.RfpAddress != null && rfp.RfpAddress.HouseNumber != null ? rfp.RfpAddress.HouseNumber : string.Empty;
                string address_Borough = rfp.RfpAddress != null && rfp.RfpAddress.Borough != null ? rfp.RfpAddress.Borough.Description : string.Empty;
                string address_Zipcode = rfp.RfpAddress != null && rfp.RfpAddress.ZipCode != null ? rfp.RfpAddress.ZipCode : string.Empty;
                string specialPlaceName = rfp.SpecialPlace;
                string rfpNumber = rfp.RfpNumber;

                subjectmessage = subjectmessage1 + " " + "for Rfp# " + rfp.RfpNumber + " at " + address_HouseNumber + " " + address_Street + " " + address_Borough + " " + address_Zipcode + "";
            }
            else if (tasknoteDetails.Task.IdContact != null)
            {
                Contact objcontact = rpoContext.Contacts.FirstOrDefault(d => d.Id == tasknoteDetails.Task.IdContact);
                if (objcontact != null)
                {
                    subjectmessage = subjectmessage1 + " " + "for Contact - " + objcontact.FirstName + " " + objcontact.LastName + "";
                }
                else
                {
                    subjectmessage = subjectmessage1;
                }
            }
            else if (tasknoteDetails.Task.IdCompany != null)
            {
                Company objcompany = rpoContext.Companies.FirstOrDefault(d => d.Id == tasknoteDetails.Task.IdCompany);
                if (objcompany != null)
                {
                    subjectmessage = subjectmessage1 + " " + "for Company - " + objcompany.Name + "";
                }
                else
                {
                    subjectmessage = subjectmessage1;
                }
            }
            else
            {
                subjectmessage = subjectmessage1;
            }

            if (tasknoteDetails.Task != null && tasknoteDetails.Task.AssignedTo != null && tasknoteDetails.Task.IdAssignedBy == tasknoteDetails.Task.IdAssignedTo)
            {
                string body = string.Empty;

                string setRedirecturl = string.Empty;
                if (tasknoteDetails.Task.IdJob != null)
                {
                    setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + tasknoteDetails.Task.IdJob + "" + "/jobtask";
                }
                else
                {
                    setRedirecturl = Properties.Settings.Default.FrontEndUrl + "tasks";
                }

                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/TaskProgressNoteTemplate.htm")))
                {
                    body = reader.ReadToEnd();
                }

                if (tasknoteDetails.Task != null && tasknoteDetails.Task.AssignedTo != null && !string.IsNullOrWhiteSpace(tasknoteDetails.Task.AssignedTo.Email))
                {

                    string emailBody = body;
                    emailBody = emailBody.Replace("##EmployeeName##", tasknoteDetails.Task != null && tasknoteDetails.Task.AssignedTo != null ? tasknoteDetails.Task.AssignedTo.FirstName : string.Empty);
                    emailBody = emailBody.Replace("##ProgressNote##", tasknoteDetails != null ? tasknoteDetails.Notes : string.Empty);
                    emailBody = emailBody.Replace("##TaskNumber##", tasknoteDetails.Task != null ? tasknoteDetails.Task.TaskNumber : string.Empty);
                    emailBody = emailBody.Replace("##TaskType##", tasknoteDetails.Task != null && tasknoteDetails.Task.TaskType != null ? tasknoteDetails.Task.TaskType.Name : string.Empty);
                    emailBody = emailBody.Replace("##TaskDetails##", tasknoteDetails.Task != null ? tasknoteDetails.Task.GeneralNotes : string.Empty);
                    emailBody = emailBody.Replace("##RedirectionLink##", setRedirecturl);
                    //emailBody = emailBody.Replace("##RedirectionLink##", "/tasks");

                    List<KeyValuePair<string, string>> toAssignedTo = new List<KeyValuePair<string, string>>();
                    toAssignedTo.Add(new KeyValuePair<string, string>(tasknoteDetails.Task.AssignedTo.Email, tasknoteDetails.Task.AssignedTo.FirstName + " " + tasknoteDetails.Task.AssignedTo.LastName));

                    string progressNoteAddedToTask = InAppNotificationMessage.ProgressNoteAddedToTask
                   .Replace("##ProgressNote##", tasknoteDetails != null ? tasknoteDetails.Notes : string.Empty)
                   .Replace("##TaskNumber##", tasknoteDetails.Task != null ? tasknoteDetails.Task.TaskNumber : string.Empty)
                   .Replace("##TaskType##", tasknoteDetails.Task != null && tasknoteDetails.Task.TaskType != null ? tasknoteDetails.Task.TaskType.Name : string.Empty)
                   .Replace("##TaskDetails##", tasknoteDetails.Task != null ? tasknoteDetails.Task.GeneralNotes : string.Empty)
                   .Replace("##JobAddress##", JobAddress)
                   .Replace("##JobNumber##", job != null ? job.JobNumber : string.Empty)
                   .Replace("##RedirectionLink##", setRedirecturl);

                    Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toAssignedTo, subjectmessage, emailBody, true);


                    //.Replace("##RedirectionLink##", "/tasks");

                    Common.SendInAppNotifications(tasknoteDetails.Task.AssignedTo.Id, progressNoteAddedToTask, Hub, "tasks");
                }
            }
            else
            {
                if (tasknoteDetails.Task != null && tasknoteDetails.Task.AssignedBy != null)
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/TaskProgressNoteTemplate.htm")))
                    {
                        body = reader.ReadToEnd();
                    }

                    if (tasknoteDetails.Task != null && tasknoteDetails.Task.AssignedBy != null && tasknoteDetails.Task.IdAssignedBy != tasknoteDetails.Task.IdAssignedTo && !string.IsNullOrWhiteSpace(tasknoteDetails.Task.AssignedBy.Email))
                    {
                        string setRedirecturl = string.Empty;
                        if (tasknoteDetails.Task.IdJob != null)
                        {
                            setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + tasknoteDetails.Task.IdJob + "" + "/jobtask";
                        }
                        else
                        {
                            setRedirecturl = Properties.Settings.Default.FrontEndUrl + "tasks";
                        }

                        string emailBody = body;
                        emailBody = emailBody.Replace("##EmployeeName##", tasknoteDetails.Task != null && tasknoteDetails.Task.AssignedBy != null ? tasknoteDetails.Task.AssignedBy.FirstName : string.Empty);
                        emailBody = emailBody.Replace("##ProgressNote##", tasknoteDetails != null ? tasknoteDetails.Notes : string.Empty);
                        emailBody = emailBody.Replace("##TaskNumber##", tasknoteDetails.Task != null ? tasknoteDetails.Task.TaskNumber : string.Empty);
                        emailBody = emailBody.Replace("##TaskType##", tasknoteDetails.Task != null && tasknoteDetails.Task.TaskType != null ? tasknoteDetails.Task.TaskType.Name : string.Empty);
                        emailBody = emailBody.Replace("##TaskDetails##", tasknoteDetails.Task != null ? tasknoteDetails.Task.GeneralNotes : string.Empty);
                        emailBody = emailBody.Replace("##RedirectionLink##", setRedirecturl);



                        string progressNoteAddedToTask = InAppNotificationMessage.ProgressNoteAddedToTask
                                            .Replace("##ProgressNote##", tasknoteDetails != null ? tasknoteDetails.Notes : string.Empty)
                                            .Replace("##TaskNumber##", tasknoteDetails.Task != null ? tasknoteDetails.Task.TaskNumber : string.Empty)
                                            .Replace("##TaskType##", tasknoteDetails.Task != null && tasknoteDetails.Task.TaskType != null ? tasknoteDetails.Task.TaskType.Name : string.Empty)
                                            .Replace("##TaskDetails##", tasknoteDetails.Task != null ? tasknoteDetails.Task.GeneralNotes : string.Empty)
                                            .Replace("##JobAddress##", JobAddress)
                                            .Replace("##JobNumber##", job != null ? job.JobNumber : string.Empty)
                                            .Replace("##RedirectionLink##", setRedirecturl);

                        //emailBody = emailBody.Replace("##RedirectionLink##", "/tasks");

                        List<KeyValuePair<string, string>> toAssignedBy = new List<KeyValuePair<string, string>>();
                        toAssignedBy.Add(new KeyValuePair<string, string>(tasknoteDetails.Task.AssignedBy.Email, tasknoteDetails.Task.AssignedBy.FirstName + " " + tasknoteDetails.Task.AssignedBy.LastName));

                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toAssignedBy, subjectmessage, emailBody, true);

                        //.Replace("##RedirectionLink##", "/tasks");
                        Common.SendInAppNotifications(tasknoteDetails.Task.AssignedBy.Id, progressNoteAddedToTask, Hub, "tasks");
                    }
                }
                if (tasknoteDetails.Task != null && tasknoteDetails.Task.AssignedTo != null)
                {
                    string body = string.Empty;

                    string setRedirecturl = string.Empty;
                    if (tasknoteDetails.Task.IdJob != null)
                    {
                        setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + tasknoteDetails.Task.IdJob + "" + "/jobtask";
                    }
                    else
                    {
                        setRedirecturl = Properties.Settings.Default.FrontEndUrl + "tasks";
                    }

                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/TaskProgressNoteTemplate.htm")))
                    {
                        body = reader.ReadToEnd();
                    }

                    if (tasknoteDetails.Task != null && tasknoteDetails.Task.AssignedTo != null && !string.IsNullOrWhiteSpace(tasknoteDetails.Task.AssignedTo.Email))
                    {

                        string emailBody = body;
                        emailBody = emailBody.Replace("##EmployeeName##", tasknoteDetails.Task != null && tasknoteDetails.Task.AssignedTo != null ? tasknoteDetails.Task.AssignedTo.FirstName : string.Empty);
                        emailBody = emailBody.Replace("##ProgressNote##", tasknoteDetails != null ? tasknoteDetails.Notes : string.Empty);
                        emailBody = emailBody.Replace("##TaskNumber##", tasknoteDetails.Task != null ? tasknoteDetails.Task.TaskNumber : string.Empty);
                        emailBody = emailBody.Replace("##TaskType##", tasknoteDetails.Task != null && tasknoteDetails.Task.TaskType != null ? tasknoteDetails.Task.TaskType.Name : string.Empty);
                        emailBody = emailBody.Replace("##TaskDetails##", tasknoteDetails.Task != null ? tasknoteDetails.Task.GeneralNotes : string.Empty);
                        emailBody = emailBody.Replace("##RedirectionLink##", setRedirecturl);
                        //emailBody = emailBody.Replace("##RedirectionLink##", "/tasks");

                        List<KeyValuePair<string, string>> toAssignedTo = new List<KeyValuePair<string, string>>();
                        toAssignedTo.Add(new KeyValuePair<string, string>(tasknoteDetails.Task.AssignedTo.Email, tasknoteDetails.Task.AssignedTo.FirstName + " " + tasknoteDetails.Task.AssignedTo.LastName));

                        string progressNoteAddedToTask = InAppNotificationMessage.ProgressNoteAddedToTask
                       .Replace("##ProgressNote##", tasknoteDetails != null ? tasknoteDetails.Notes : string.Empty)
                       .Replace("##TaskNumber##", tasknoteDetails.Task != null ? tasknoteDetails.Task.TaskNumber : string.Empty)
                       .Replace("##TaskType##", tasknoteDetails.Task != null && tasknoteDetails.Task.TaskType != null ? tasknoteDetails.Task.TaskType.Name : string.Empty)
                       .Replace("##TaskDetails##", tasknoteDetails.Task != null ? tasknoteDetails.Task.GeneralNotes : string.Empty)
                       .Replace("##JobAddress##", JobAddress)
                       .Replace("##JobNumber##", job != null ? job.JobNumber : string.Empty)
                       .Replace("##RedirectionLink##", setRedirecturl);

                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toAssignedTo, subjectmessage, emailBody, true);


                        //.Replace("##RedirectionLink##", "/tasks");

                        Common.SendInAppNotifications(tasknoteDetails.Task.AssignedTo.Id, progressNoteAddedToTask, Hub, "tasks");
                    }
                }
            }

            SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenAProgressNoteIsAddedToaTask);
            if (tasknoteDetails.Task != null && systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
            {

                foreach (var item in systemSettingDetail.Value)
                {
                    string setRedirecturl = string.Empty;
                    if (tasknoteDetails.Task.IdJob != null)
                    {
                        setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + tasknoteDetails.Task.IdJob + "" + "/jobtask";
                    }
                    else
                    {
                        setRedirecturl = Properties.Settings.Default.FrontEndUrl + "tasks";
                    }

                    if (item.Email != tasknoteDetails.Task.AssignedTo.Email && item.Email != tasknoteDetails.Task.AssignedBy.Email && item.Email != tasknoteDetails.Task.AssignedTo.Email)
                    {
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/TaskProgressNoteTemplate.htm")))
                        {
                            body = reader.ReadToEnd();
                        }

                        string emailBody = body;
                        emailBody = emailBody.Replace("##EmployeeName##", item.FirstName);
                        emailBody = emailBody.Replace("##ProgressNote##", tasknoteDetails != null ? tasknoteDetails.Notes : string.Empty);
                        emailBody = emailBody.Replace("##TaskNumber##", tasknoteDetails.Task != null ? tasknoteDetails.Task.TaskNumber : string.Empty);
                        emailBody = emailBody.Replace("##TaskType##", tasknoteDetails.Task != null && tasknoteDetails.Task.TaskType != null ? tasknoteDetails.Task.TaskType.Name : string.Empty);
                        emailBody = emailBody.Replace("##TaskDetails##", tasknoteDetails.Task != null ? tasknoteDetails.Task.GeneralNotes : string.Empty);
                        emailBody = emailBody.Replace("##RedirectionLink##", setRedirecturl);
                        //emailBody = emailBody.Replace("##RedirectionLink##", "/tasks");

                        string progressNoteAddedToTask = InAppNotificationMessage.ProgressNoteAddedToTask
                                              .Replace("##ProgressNote##", tasknoteDetails != null ? tasknoteDetails.Notes : string.Empty)
                                              .Replace("##TaskNumber##", tasknoteDetails.Task != null ? tasknoteDetails.Task.TaskNumber : string.Empty)
                                              .Replace("##TaskType##", tasknoteDetails.Task != null && tasknoteDetails.Task.TaskType != null ? tasknoteDetails.Task.TaskType.Name : string.Empty)
                                              .Replace("##TaskDetails##", tasknoteDetails.Task != null ? tasknoteDetails.Task.GeneralNotes : string.Empty)
                                               .Replace("##JobAddress##", JobAddress)
                                              .Replace("##JobNumber##", job != null ? job.JobNumber : string.Empty)
                                              .Replace("##RedirectionLink##", setRedirecturl);

                        List<KeyValuePair<string, string>> toEmployees = new List<KeyValuePair<string, string>>();
                        toEmployees.Add(new KeyValuePair<string, string>(item.Email, item.EmployeeName));
                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toEmployees, subjectmessage, emailBody, true);



                        Common.SendInAppNotifications(item.Id, progressNoteAddedToTask, Hub, "tasks");
                    }
                }
            }

            TaskNote taskNoteresponse = rpoContext.TaskNotes.FirstOrDefault(x => x.Id == taskNote.Id);
            return Ok(FormatDetails(taskNoteresponse));
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
        /// Tasks the note exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TaskNoteExists(int id)
        {
            return rpoContext.TaskNotes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="taskNote">The task note.</param>
        /// <returns>TaskNoteDetails.</returns>
        private TaskNoteDetails FormatDetails(TaskNote taskNote)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new TaskNoteDetails
            {
                Id = taskNote.Id,
                Notes = taskNote.Notes,
                CreatedBy = taskNote.CreatedBy,
                LastModifiedBy = taskNote.LastModifiedBy != null ? taskNote.LastModifiedBy : taskNote.CreatedBy,
                CreatedByEmployee = taskNote.CreatedByEmployee != null ? taskNote.CreatedByEmployee.FirstName + " " + taskNote.CreatedByEmployee.LastName : string.Empty,
                LastModified = taskNote.LastModified != null ? taskNote.LastModified.FirstName + " " + taskNote.LastModified.LastName : (taskNote.CreatedByEmployee != null ? taskNote.CreatedByEmployee.FirstName + " " + taskNote.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = taskNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taskNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : taskNote.CreatedDate,
                LastModifiedDate = taskNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taskNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (taskNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taskNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : taskNote.CreatedDate),
            };
        }
    }
}