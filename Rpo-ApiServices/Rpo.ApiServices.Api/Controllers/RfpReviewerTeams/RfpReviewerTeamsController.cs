
namespace Rpo.ApiServices.Api.Controllers.RfpReviewerTeams
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
    /// Class RfpProgress Notes Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class RfpReviewerTeamsController : HubApiController<GroupHub>
    {
        /// <summary>
        /// Class RfpReviewerTeamsAdvancedSearchParameters.
        /// </summary>
        /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
        public class RfpReviewerTeamsAdvancedSearchParameters : DataTableParameters
        {
            /// <summary>
            /// Gets or sets the identifier RFP.
            /// </summary>
            /// <value>The identifier RFP.</value>
            public int? IdRfp { get; set; }
        }

        private RpoContext rpoContext = new RpoContext();

        //public IHttpActionResult GetRfpReviewerTeams([FromUri] RfpReviewerTeamsAdvancedSearchParameters dataTableParameters)
        //{
        //    IQueryable<RfpReviewer> rfpReviewers = rpoContext.RfpReviewers;
        //    var recordsTotal = rfpReviewers.Count();

        //    if (dataTableParameters.IdRfp != null)
        //    {
        //        rfpReviewers = rfpReviewers.Where(jtn => jtn.IdRfp == (int)dataTableParameters.IdRfp);
        //    }

        //    var recordsFiltered = recordsTotal;

        //    var result = rfpReviewers.AsEnumerable().Select(x => Format(x)).AsQueryable()
        //    .DataTableParameters(dataTableParameters, out recordsFiltered);

        //    return Ok(new DataTableResponse
        //    {
        //        Draw = dataTableParameters.Draw,
        //        RecordsFiltered = recordsFiltered,
        //        RecordsTotal = recordsTotal,
        //        Data = result
        //    });
        //}

        public IHttpActionResult GetRfpReviewerTeams(int idRfp)
        {
            return Ok(rpoContext.RfpReviewers.Include("Reviewer").Where(x => x.IdRfp == idRfp).AsEnumerable().Select(x => Format(x)).ToArray());
        }

        /// <summary>
        /// Posts the rfpProgress note.
        /// </summary>
        /// <param name="rfpReviewerTeam">The rfpProgress note.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpReviewerTeamDTO))]
        public IHttpActionResult PutRfpReviewerTeam(int idRfp, List<RfpReviewerTeamDTO> rfpReviewerTeam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rfpReviewers = rpoContext.RfpReviewers.Where(x => x.IdRfp == idRfp);
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            Rfp rfp = rpoContext.Rfps.Include("CreatedBy").Include("RfpAddress")
                .Include("RfpAddress.Borough").FirstOrDefault(x => x.Id == idRfp);

            List<int> oldReviewerList = new List<int>();
            if (rfpReviewers != null)
            {
                oldReviewerList = rfpReviewers.Where(x => x.IdReviewer != null).Select(x => (int)x.IdReviewer).ToList();
            }

            foreach (RfpReviewer item in rfpReviewers)
            {
                if (rfpReviewerTeam != null && rfpReviewerTeam.Count() > 0)
                {
                    var reviewer = rfpReviewerTeam.FirstOrDefault(x => x.Id == item.Id);
                    if (reviewer == null)
                    {
                        rpoContext.RfpReviewers.Remove(item);
                    }
                }
                else
                {
                    rpoContext.RfpReviewers.Remove(item);
                }
            }
            rpoContext.SaveChanges();

            if (rfpReviewerTeam != null && rfpReviewerTeam.Count > 0)
            {
                foreach (RfpReviewerTeamDTO item in rfpReviewerTeam)
                {
                    if (item.Id > 0)
                    {
                        var rfpReviewer = rpoContext.RfpReviewers.FirstOrDefault(x => x.Id == item.Id);
                        rfpReviewer.IdRfp = item.IdRfp;
                        rfpReviewer.IdReviewer = item.IdReviewer;
                        rpoContext.SaveChanges();

                        if (!oldReviewerList.Contains(Convert.ToInt32(item.IdReviewer)))
                        {
                            rfpReviewer = rpoContext.RfpReviewers.Include("Reviewer").FirstOrDefault(x => x.Id == rfpReviewer.Id);
                            KeyValuePair<string, string> toAddress = new KeyValuePair<string, string>(rfpReviewer.Reviewer.Email, rfpReviewer.Reviewer.FirstName + " " + rfpReviewer.Reviewer.LastName);
                          
                            //SendRequestForReviewEmail(rfp, Convert.ToInt32(rfpReviewer.IdReviewer), toAddress, rfpReviewer.Reviewer.FirstName + " " + rfpReviewer.Reviewer.LastName);


                            Tasks.TaskRequestDTO taskRequest = new Tasks.TaskRequestDTO();
                            taskRequest.IdTaskType = Api.Jobs.TaskTypeMaster.Proposal_Review.GetHashCode();
                            taskRequest.IdAssignedTo = rfpReviewer.IdReviewer;

                            Task task = rpoContext.Tasks.FirstOrDefault(x => x.IdTaskType == taskRequest.IdTaskType && x.IdAssignedTo == taskRequest.IdAssignedTo);
                            if (task == null)
                            {
                                if (employee != null)
                                {
                                    taskRequest.IdAssignedBy = employee.Id;
                                }

                                taskRequest.AssignedDate = DateTime.UtcNow;
                                taskRequest.CompleteBy = DateTime.UtcNow.AddDays(1);
                                taskRequest.IdTaskStatus = EnumTaskStatus.Pending.GetHashCode();
                                taskRequest.IdRfp = idRfp;
                                taskRequest.GeneralNotes = "Review RFP #" + rfp.RfpNumber;

                                CreateTask(taskRequest, currentTimeZone, employee);
                            }

                        }
                    }
                    else
                    {
                        RfpReviewer rfpReviewer = new RfpReviewer();
                        rfpReviewer.IdRfp = item.IdRfp;
                        rfpReviewer.IdReviewer = item.IdReviewer;
                        rpoContext.RfpReviewers.Add(rfpReviewer);
                        rpoContext.SaveChanges();

                        if (!oldReviewerList.Contains(Convert.ToInt32(item.IdReviewer)))
                        {
                            rfpReviewer = rpoContext.RfpReviewers.Include("Reviewer").FirstOrDefault(x => x.Id == rfpReviewer.Id);
                            KeyValuePair<string, string> toAddress = new KeyValuePair<string, string>(rfpReviewer.Reviewer.Email, rfpReviewer.Reviewer.FirstName + " " + rfpReviewer.Reviewer.LastName);
                            //SendRequestForReviewEmail(rfp, Convert.ToInt32(rfpReviewer.IdReviewer), toAddress, rfpReviewer.Reviewer.FirstName + " " + rfpReviewer.Reviewer.LastName);

                            Tasks.TaskRequestDTO taskRequest = new Tasks.TaskRequestDTO();
                            taskRequest.IdTaskType = Api.Jobs.TaskTypeMaster.Proposal_Review.GetHashCode();
                            taskRequest.IdAssignedTo = rfpReviewer.IdReviewer;

                            Task task = rpoContext.Tasks.FirstOrDefault(x => x.IdTaskType == taskRequest.IdTaskType && x.IdAssignedTo == taskRequest.IdAssignedTo);
                            if (task == null)
                            {
                                if (employee != null)
                                {
                                    taskRequest.IdAssignedBy = employee.Id;
                                }

                                taskRequest.AssignedDate = DateTime.UtcNow;
                                taskRequest.CompleteBy = DateTime.UtcNow.AddDays(1);
                                taskRequest.IdTaskStatus = EnumTaskStatus.Pending.GetHashCode();
                                taskRequest.IdRfp = idRfp;
                                taskRequest.GeneralNotes = "Review RFP #" + rfp.RfpNumber;

                                CreateTask(taskRequest, currentTimeZone, employee);
                            }
                        }
                    }
                }
            }

            SendEmailRFPSubmitForReviewSystemSettings(rfp);
            rpoContext.SaveChanges();

            var rfpReviewerTeamresponse = rpoContext.RfpReviewers.Where(x => x.IdRfp == idRfp).AsEnumerable();
            return Ok(rfpReviewerTeamresponse);
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
        /// RfpProgresss the note exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpReviewerTeamExists(int id)
        {
            return rpoContext.RfpReviewers.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Sends the email RFP submit for review system settings.
        /// </summary>
        /// <param name="rfp">The RFP.</param>
        private void SendEmailRFPSubmitForReviewSystemSettings(Rfp rfp)
        {
            string javascript = "click=\"redirectFromNotification(j)\"";
            SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenProposalIsReviewedAndMarkedForSendingwhenUserPressesApproveSend);
            if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
            {
                foreach (var item in systemSettingDetail.Value)
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/RFPRequestForApproval.htm")))
                    {
                        body = reader.ReadToEnd();
                    }

                    string emailBody = body;
                    emailBody = emailBody.Replace("##EmployeeFirstName##", item.EmployeeName);
                    emailBody = emailBody.Replace("##CompanyName##", rfp.Company != null ? rfp.Company.Name : "**Individual**");
                    emailBody = emailBody.Replace("##ContactName##", rfp.Contact != null ? rfp.Contact.FirstName + " " + rfp.Contact.LastName : "Not Set");
                    //emailBody = emailBody.Replace("##RFPFullAddress##", rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + (rfp.RfpAddress.Borough != null ? " " + rfp.RfpAddress.Borough.Description : string.Empty)+(rfp.City != null ? " " + rfp.City : string.Empty) + (rfp.State != null ? " " + rfp.State.Name : string.Empty) + " " + rfp.RfpAddress.ZipCode : "Not Set");
                    emailBody = emailBody.Replace("##RFPFullAddress##", rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + (rfp.RfpAddress.Borough != null ? " " + rfp.RfpAddress.Borough.Description : string.Empty) + " " + rfp.RfpAddress.ZipCode : "Not Set");
                    emailBody = emailBody.Replace("##EmployeeName##", rfp.CreatedBy != null ? rfp.CreatedBy.FirstName + " " + rfp.CreatedBy.LastName : "Not Set");
                    emailBody = emailBody.Replace("##RfpNumber##", rfp.RfpNumber);
                    emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + rfp.Id);

                    Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), new KeyValuePair<string, string>(item.Email, item.EmployeeName), "Review request for proposal", emailBody, true);

                    string rfpSubmitForReview = InAppNotificationMessage._RFPSubmitForReview;
                    rfpSubmitForReview = rfpSubmitForReview.Replace("##CompanyName##", rfp.Company != null ? rfp.Company.Name : "**Individual**");
                    rfpSubmitForReview = rfpSubmitForReview.Replace("##ContactName##", rfp.Contact != null ? rfp.Contact.FirstName + " " + rfp.Contact.LastName : "Not Set");
                    rfpSubmitForReview = rfpSubmitForReview.Replace("##RFPFullAddress##", rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + (rfp.RfpAddress.Borough != null ? " " + rfp.RfpAddress.Borough.Description : string.Empty) + " " + rfp.RfpAddress.ZipCode : "Not Set"); // + (rfp.City != null ? " " + rfp.City : string.Empty) + (rfp.State != null ? " " + rfp.State.Name : string.Empty) + " " + rfp.RfpAddress.ZipCode : "Not Set");
                    rfpSubmitForReview = rfpSubmitForReview.Replace("##EmployeeName##", rfp.CreatedBy != null ? rfp.CreatedBy.FirstName + " " + rfp.CreatedBy.LastName : "Not Set");
                    rfpSubmitForReview = rfpSubmitForReview.Replace("##RfpNumber##", rfp.RfpNumber);
                    rfpSubmitForReview = rfpSubmitForReview.Replace("##RedirectionLink##", javascript);
                    Common.SendInAppNotifications(item.IdEmployee, rfpSubmitForReview, Hub, "editSiteInformation/" + rfp.Id);
                }
            }
        }

        /// <summary>
        /// Sends the request for approval email.
        /// </summary>
        /// <param name="rfp">The RFP.</param>
        /// <param name="idReviewer">The identifier reviewer.</param>
        /// <param name="toAddress">To address.</param>
        /// <param name="employeeName">Name of the employee.</param>
        private void SendRequestForReviewEmail(Rfp rfp, int idReviewer, KeyValuePair<string, string> toAddress, string employeeName)
        {
            string javascript = "click=\"redirectFromNotification(j)\"";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/RFPRequestForApproval.htm")))
            {
                body = reader.ReadToEnd();
            }

            string emailBody = body;
            emailBody = emailBody.Replace("##EmployeeFirstName##", employeeName);
            emailBody = emailBody.Replace("##CompanyName##", rfp.Company != null ? rfp.Company.Name : "**Individual**");
            emailBody = emailBody.Replace("##ContactName##", rfp.Contact != null ? rfp.Contact.FirstName + " " + rfp.Contact.LastName : "Not Set");
            //  emailBody = emailBody.Replace("##RFPFullAddress##", rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + (rfp.RfpAddress.Borough != null ? " " + rfp.RfpAddress.Borough.Description : string.Empty) + (rfp.City != null ? " " + rfp.City : string.Empty) + (rfp.State != null ? " " + rfp.State.Name : string.Empty) + " " + rfp.RfpAddress.ZipCode : "Not Set");
            emailBody = emailBody.Replace("##RFPFullAddress##", rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + (rfp.RfpAddress.Borough != null ? " " + rfp.RfpAddress.Borough.Description : string.Empty) + " " + rfp.RfpAddress.ZipCode : "Not Set");
            emailBody = emailBody.Replace("##EmployeeName##", rfp.CreatedBy != null ? rfp.CreatedBy.FirstName + " " + rfp.CreatedBy.LastName : "Not Set");
            emailBody = emailBody.Replace("##RfpNumber##", rfp.RfpNumber);
            emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + rfp.Id);

            string subject = EmailNotificationSubject.RFPSubmitForReview;
            Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toAddress, subject, emailBody, true);

            string rfpSubmitForReview = InAppNotificationMessage._RFPSubmitForReview;
            rfpSubmitForReview = rfpSubmitForReview.Replace("##CompanyName##", rfp.Company != null ? rfp.Company.Name : "**Individual**");
            rfpSubmitForReview = rfpSubmitForReview.Replace("##ContactName##", rfp.Contact != null ? rfp.Contact.FirstName + " " + rfp.Contact.LastName : "Not Set");
            // rfpSubmitForReview = rfpSubmitForReview.Replace("##RFPFullAddress##", rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + (rfp.RfpAddress.Borough != null ? " " + rfp.RfpAddress.Borough.Description : string.Empty) + (rfp.City != null ? " " + rfp.City : string.Empty) + (rfp.State != null ? " " + rfp.State.Name : string.Empty) + " " + rfp.RfpAddress.ZipCode : "Not Set");
            rfpSubmitForReview = rfpSubmitForReview.Replace("##RFPFullAddress##", rfp.RfpAddress != null ? rfp.RfpAddress.HouseNumber + " " + rfp.RfpAddress.Street + (rfp.RfpAddress.Borough != null ? " " + rfp.RfpAddress.Borough.Description : string.Empty) + " " + rfp.RfpAddress.ZipCode : "Not Set");
            rfpSubmitForReview = rfpSubmitForReview.Replace("##EmployeeName##", rfp.CreatedBy != null ? rfp.CreatedBy.FirstName + " " + rfp.CreatedBy.LastName : "Not Set");
            rfpSubmitForReview = rfpSubmitForReview.Replace("##RfpNumber##", rfp.RfpNumber);
            rfpSubmitForReview = rfpSubmitForReview.Replace("##RedirectionLink##", javascript); 

            Common.SendInAppNotifications(idReviewer, rfpSubmitForReview, Hub, "editSiteInformation/" + rfp.Id);
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="rfpReviewer">The RFP reviewer.</param>
        /// <returns>RfpReviewerTeamDetails.</returns>
        private RfpReviewerTeamDetails FormatDetails(RfpReviewer rfpReviewer)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpReviewerTeamDetails
            {
                Id = rfpReviewer.Id,
                IdReviewer = rfpReviewer.IdReviewer,
                IdRfp = rfpReviewer.IdRfp,
                Reviewer = rfpReviewer.Reviewer != null ? rfpReviewer.Reviewer.FirstName + " " + rfpReviewer.Reviewer.LastName : string.Empty
            };
        }

        /// <summary>
        /// Formats the specified RFP reviewer.
        /// </summary>
        /// <param name="rfpReviewer">The RFP reviewer.</param>
        /// <returns>RfpReviewerTeamDTO.</returns>
        private RfpReviewerTeamDTO Format(RfpReviewer rfpReviewer)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpReviewerTeamDTO
            {
                Id = rfpReviewer.Id,
                IdReviewer = rfpReviewer.IdReviewer,
                IdRfp = rfpReviewer.IdRfp,
                Reviewer = rfpReviewer.Reviewer != null ? rfpReviewer.Reviewer.FirstName + " " + rfpReviewer.Reviewer.LastName : string.Empty,
                ItemName = rfpReviewer.Reviewer != null ? (rfpReviewer.Reviewer.FirstName + (!string.IsNullOrWhiteSpace(rfpReviewer.Reviewer.LastName) ? " " + rfpReviewer.Reviewer.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(rfpReviewer.Reviewer.Email) ? " (" + rfpReviewer.Reviewer.Email + ")" : string.Empty)) : string.Empty
            };
        }

        /// <summary>
        /// Creates the task.
        /// </summary>
        /// <param name="jobTask">The job task.</param>
        /// <param name="currentTimeZone">The current time zone.</param>
        /// <param name="employee">The employee.</param>
        private void CreateTask(Tasks.TaskRequestDTO jobTask, string currentTimeZone, Employee employee)
        {
            var task = new Task();

            task.AssignedDate = jobTask.AssignedDate;
            task.CompleteBy = jobTask.CompleteBy;
            task.GeneralNotes = jobTask.GeneralNotes;
            task.IdAssignedBy = jobTask.IdAssignedBy;
            task.IdAssignedTo = jobTask.IdAssignedTo;
            task.IdJob = jobTask.IdJob;
            task.IdJobApplication = jobTask.IdJobApplication;
            if (jobTask.IdWorkPermitType != null && jobTask.IdWorkPermitType.Count() > 0)
            {
                task.IdWorkPermitType = string.Join(",", jobTask.IdWorkPermitType.Select(x => x.ToString()));
            }
            else
            {
                task.IdWorkPermitType = string.Empty;
            }
            task.IdTaskStatus = jobTask.IdTaskStatus;
            task.IdTaskType = jobTask.IdTaskType;
            task.IdRfp = jobTask.IdRfp;
            task.IdContact = jobTask.IdContact;
            task.IdCompany = jobTask.IdCompany;
            task.IdExaminer = jobTask.IdExaminer;

            task.LastModifiedDate = DateTime.UtcNow;
            task.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                task.CreatedBy = employee.Id;
            }

            if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            {
                task.ClosedDate = DateTime.UtcNow;
            }

            rpoContext.Tasks.Add(task);
            rpoContext.SaveChanges();

            task.TaskNumber = task.Id.ToString();// .ToString("000000");
            rpoContext.SaveChanges();
        }
    }
}