// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-15-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-15-2018
// ***********************************************************************
// <copyright file="RfpProgressNotesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Progress Notes Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.RfpProgressNotes
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
    /// Class Rfp Progress Notes Controller.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.Controllers.HubApiController{Rpo.ApiServices.Api.Hubs.GroupHub}" />
    public class RfpProgressNotesController : HubApiController<GroupHub>
    {
        /// <summary>
        /// Class Rfp Progress Notes Advanced Search Parameters.
        /// </summary>
        /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
        public class RfpProgressNotesAdvancedSearchParameters : DataTableParameters
        {
            /// <summary>
            /// Gets or sets the identifier RFP.
            /// </summary>
            /// <value>The identifier RFP.</value>
            public int? IdRfp { get; set; }
        }

        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the RFP progress notes.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Get the list of progress notes list against the RFP Id.</returns>
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetRfpProgressNotes([FromUri] RfpProgressNotesAdvancedSearchParameters dataTableParameters)
        {
            IQueryable<RfpProgressNote> rfpProgressNotes = rpoContext.RfpProgressNotes.Include("CreatedByEmployee").Include("LastModified");
            var recordsTotal = rfpProgressNotes.Count();

            if (dataTableParameters.IdRfp != null)
            {
                rfpProgressNotes = rfpProgressNotes.Where(jtn => jtn.IdRfp == (int)dataTableParameters.IdRfp);
            }

            var recordsFiltered = recordsTotal;

            var result = rfpProgressNotes.AsEnumerable().Select(x => Format(x)).OrderByDescending(x => x.LastModifiedDate).AsQueryable()
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
        /// Gets the RFP progress note.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Get the detail of progress note against the id</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpProgressNoteDetails))]
        public IHttpActionResult GetRfpProgressNote(int id)
        {
            RfpProgressNote rfpProgressNote = rpoContext.RfpProgressNotes.Find(id);
            if (rfpProgressNote == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(rfpProgressNote));
        }

        /// <summary>
        /// Posts the RFP progress note.
        /// </summary>
        /// <remarks>create a new progess note of RFPs and send the project team to send notification.</remarks>
        /// <param name="rfpProgressNote">The RFP progress note.</param>
        /// <returns>create a new progess note of RFPs.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpProgressNote))]
        public IHttpActionResult PostRfpProgressNote(RfpProgressNote rfpProgressNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            rfpProgressNote.LastModifiedDate = DateTime.UtcNow;
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (employee != null)
            {
                rfpProgressNote.LastModifiedBy = employee.Id;
            }
            rpoContext.RfpProgressNotes.Add(rfpProgressNote);
            rpoContext.SaveChanges();

            RfpProgressNote rfpProgressNoteResponse = rpoContext.RfpProgressNotes.Include("LastModified").Include("Rfp").Include("Rfp.CreatedBy").FirstOrDefault(x => x.Id == rfpProgressNote.Id);
            this.SendProgressNoteNotification(rfpProgressNoteResponse);
            return Ok(FormatDetails(rfpProgressNoteResponse));
        }

        private void SendProgressNoteNotification(RfpProgressNote rfpProgressNoteDetails)
        {
            if (rfpProgressNoteDetails.Rfp != null)
            {
                if (rfpProgressNoteDetails.Rfp.CreatedBy != null)
                {
                    string javascript = "click=\"redirectFromNotification(j)\"";
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/RfpProgressNoteTemplate.htm")))
                    {
                        body = reader.ReadToEnd();
                    }

                    KeyValuePair<string, string> toAddress = new KeyValuePair<string, string>(rfpProgressNoteDetails.Rfp.CreatedBy.Email, rfpProgressNoteDetails.Rfp.CreatedBy.FirstName + " " + rfpProgressNoteDetails.Rfp.CreatedBy.LastName);

                    string emailBody = body;
                    emailBody = emailBody.Replace("##EmployeeName##", rfpProgressNoteDetails.Rfp.CreatedBy.FirstName + " " + rfpProgressNoteDetails.Rfp.CreatedBy.LastName);
                    emailBody = emailBody.Replace("##ProgressNoteAddedBy##", rfpProgressNoteDetails.LastModified != null ? rfpProgressNoteDetails.LastModified.FirstName + " " + rfpProgressNoteDetails.LastModified.LastName : "Not Set");
                    emailBody = emailBody.Replace("##RFPNumber##", rfpProgressNoteDetails.Rfp.RfpNumber);
                    emailBody = emailBody.Replace("##RfpProgressNoteDetail##", rfpProgressNoteDetails.Notes);
                    //emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + rfpProgressNoteDetails.Rfp.Id);
                    emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/rfpSubmit/" + rfpProgressNoteDetails.Rfp.Id);

                    string subject = "Progress note added to RFP";
                    if (rfpProgressNoteDetails.Rfp.IdCreatedBy != rfpProgressNoteDetails.LastModifiedBy)
                    {
                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toAddress, subject, emailBody, true);

                        string progressNoteAddedToRFPSetting = InAppNotificationMessage.ProgressNoteAddedToRFP
                                    .Replace("##ProgressNote##", rfpProgressNoteDetails.Notes)
                                    .Replace("##RfpNumber##", rfpProgressNoteDetails.Rfp.RfpNumber)
                                    .Replace("##RFPFullAddress##", rfpProgressNoteDetails.Rfp.RfpAddress != null ? rfpProgressNoteDetails.Rfp.RfpAddress.HouseNumber + " " + rfpProgressNoteDetails.Rfp.RfpAddress.Street + (rfpProgressNoteDetails.Rfp.RfpAddress.Borough != null ? " " + rfpProgressNoteDetails.Rfp.RfpAddress.Borough.Description : string.Empty) + (rfpProgressNoteDetails.Rfp.City != null ? " " + rfpProgressNoteDetails.Rfp.City : string.Empty) + (rfpProgressNoteDetails.Rfp.State != null ? " " + rfpProgressNoteDetails.Rfp.State.Name : string.Empty) + (rfpProgressNoteDetails.Rfp.ZipCode != null ? " " + rfpProgressNoteDetails.Rfp.ZipCode : string.Empty) : string.Empty)
                        //            .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + rfpProgressNoteDetails.Rfp.Id);
                        //Common.SendInAppNotifications(rfpProgressNoteDetails.Rfp.IdCreatedBy.Value, progressNoteAddedToRFPSetting, Hub, "editSiteInformation/" + rfpProgressNoteDetails.Rfp.Id);
                        .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/rfpSubmit/" + rfpProgressNoteDetails.Rfp.Id);
                        Common.SendInAppNotifications(rfpProgressNoteDetails.Rfp.IdCreatedBy.Value, progressNoteAddedToRFPSetting, Hub, "rfpSubmit/" + rfpProgressNoteDetails.Rfp.Id);

                    }
                }


                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenAProgressNoteIsAddedToRFP);
                if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                {
                    foreach (var item in systemSettingDetail.Value)
                    {
                        string progressNoteAddedToRFPSetting = InAppNotificationMessage.ProgressNoteAddedToRFP
                                    .Replace("##ProgressNote##", rfpProgressNoteDetails.Notes)
                                    .Replace("##RfpNumber##", rfpProgressNoteDetails.Rfp.RfpNumber)
                                    .Replace("##RFPFullAddress##", rfpProgressNoteDetails.Rfp.RfpAddress != null ? rfpProgressNoteDetails.Rfp.RfpAddress.HouseNumber + " " + rfpProgressNoteDetails.Rfp.RfpAddress.Street + (rfpProgressNoteDetails.Rfp.RfpAddress.Borough != null ? " " + rfpProgressNoteDetails.Rfp.RfpAddress.Borough.Description : string.Empty) + (rfpProgressNoteDetails.Rfp.City != null ? " " + rfpProgressNoteDetails.Rfp.City : string.Empty) + (rfpProgressNoteDetails.Rfp.State != null ? " " + rfpProgressNoteDetails.Rfp.State.Name : string.Empty) + (rfpProgressNoteDetails.Rfp.ZipCode != null ? " " + rfpProgressNoteDetails.Rfp.ZipCode : string.Empty) : string.Empty)
                       //                .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + rfpProgressNoteDetails.Rfp.Id);
                       //    Common.SendInAppNotifications(item.Id, progressNoteAddedToRFPSetting, Hub, "editSiteInformation/" + rfpProgressNoteDetails.Rfp.Id);
                       //
                       .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/rfpSubmit/" + rfpProgressNoteDetails.Rfp.Id);
                        Common.SendInAppNotifications(item.Id, progressNoteAddedToRFPSetting, Hub, "rfpSubmit/" + rfpProgressNoteDetails.Rfp.Id);

                    }
                }
            }
        }

        /// <summary>
        /// Sends the progress note email.
        /// </summary>
        /// <param name="rfpProgressNoteDetails">The RFP progress note details.</param>
        //private void SendProgressNoteEmail(RfpProgressNote rfpProgressNoteDetails)
        //{
        //    if (rfpProgressNoteDetails.Rfp != null && rfpProgressNoteDetails.Rfp.CreatedBy != null)
        //    {
        //        string body = string.Empty;
        //        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/RfpProgressNoteTemplate.htm")))
        //        {
        //            body = reader.ReadToEnd();
        //        }

        //        if (rfpProgressNoteDetails.Rfp != null && rfpProgressNoteDetails.Rfp.CreatedBy != null && !string.IsNullOrWhiteSpace(rfpProgressNoteDetails.Rfp.CreatedBy.Email))
        //        {
        //            string emailBody = body;
        //            emailBody = emailBody.Replace("##EmployeeName##", rfpProgressNoteDetails.Rfp != null && rfpProgressNoteDetails.Rfp.CreatedBy != null ? rfpProgressNoteDetails.Rfp.CreatedBy.FirstName + " " + rfpProgressNoteDetails.Rfp.CreatedBy.LastName : string.Empty);
        //            emailBody = emailBody.Replace("##RFPNumber##", rfpProgressNoteDetails.Rfp.RfpNumber);
        //            emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + rfpProgressNoteDetails.Rfp.Id);
        //            emailBody = emailBody.Replace("##RfpProgressNoteDetail##", rfpProgressNoteDetails.Notes);
        //            emailBody = emailBody.Replace("##ProgressNoteAddedBy##", rfpProgressNoteDetails.LastModified != null ? rfpProgressNoteDetails.LastModified.FirstName + " " + rfpProgressNoteDetails.LastModified.LastName : string.Empty);
        //            KeyValuePair<string, string> toEmpleyee = new KeyValuePair<string, string>(rfpProgressNoteDetails.Rfp.CreatedBy.Email, rfpProgressNoteDetails.Rfp.CreatedBy.FirstName + " " + rfpProgressNoteDetails.Rfp.CreatedBy.LastName);

        //            Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toEmpleyee, "Progress note added to RFP", emailBody, true);
        //            Common.SendInAppNotifications(rfpProgressNoteDetails.Rfp.CreatedBy.Id, StaticMessages.NewRfpProgressNoteAddedNotificationMessage.Replace("##RfpNumber##", rfpProgressNoteDetails.Rfp.RfpNumber), Hub, "/editSiteInformation/" + rfpProgressNoteDetails.Rfp.Id);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Sends the progress note email system setting.
        ///// </summary>
        ///// <param name="rfpProgressNoteDetails">The RFP progress note details.</param>
        //private void SendProgressNoteEmailSystemSetting(RfpProgressNote rfpProgressNoteDetails)
        //{
        //    SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenAProgressNoteIsAddedToRFP);
        //    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
        //    {
        //        foreach (var item in systemSettingDetail.Value)
        //        {
        //            string body = string.Empty;
        //            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/RfpProgressNoteTemplate.htm")))
        //            {
        //                body = reader.ReadToEnd();
        //            }

        //            if (!string.IsNullOrWhiteSpace(item.Email))
        //            {
        //                string emailBody = body;
        //                emailBody = emailBody.Replace("##EmployeeName##", item.EmployeeName);
        //                emailBody = emailBody.Replace("##RFPNumber##", rfpProgressNoteDetails.Rfp.RfpNumber);
        //                emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + rfpProgressNoteDetails.Rfp.Id);
        //                emailBody = emailBody.Replace("##RfpProgressNoteDetail##", rfpProgressNoteDetails.Notes);
        //                emailBody = emailBody.Replace("##ProgressNoteAddedBy##", rfpProgressNoteDetails.LastModified != null ? rfpProgressNoteDetails.LastModified.FirstName + " " + rfpProgressNoteDetails.LastModified.LastName : string.Empty);
        //                KeyValuePair<string, string> toEmpleyee = new KeyValuePair<string, string>(item.Email, item.EmployeeName);

        //                Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toEmpleyee, "Progress note added to RFP", emailBody, true);
        //                Common.SendInAppNotifications(item.Id, StaticMessages.NewRfpProgressNoteAddedNotificationMessage.Replace("##RfpNumber##", rfpProgressNoteDetails.Rfp.RfpNumber), Hub, "/editSiteInformation/" + rfpProgressNoteDetails.Rfp.Id);
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
        /// RFPs the progress note exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpProgressNoteExists(int id)
        {
            return rpoContext.RfpProgressNotes.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="rfpProgressNote">The RFP progress note.</param>
        /// <returns>RfpProgressNoteDetails.</returns>
        private RfpProgressNoteDetails FormatDetails(RfpProgressNote rfpProgressNote)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpProgressNoteDetails
            {
                Id = rfpProgressNote.Id,
                IdRfp = rfpProgressNote.IdRfp,
                Notes = rfpProgressNote.Notes,
                CreatedBy = rfpProgressNote.CreatedBy,
                LastModifiedBy = rfpProgressNote.LastModifiedBy != null ? rfpProgressNote.LastModifiedBy : rfpProgressNote.CreatedBy,
                CreatedByEmployee = rfpProgressNote.CreatedByEmployee != null ? rfpProgressNote.CreatedByEmployee.FirstName + " " + rfpProgressNote.CreatedByEmployee.LastName : string.Empty,
                LastModified = rfpProgressNote.LastModified != null ? rfpProgressNote.LastModified.FirstName + " " + rfpProgressNote.LastModified.LastName : (rfpProgressNote.CreatedByEmployee != null ? rfpProgressNote.CreatedByEmployee.FirstName + " " + rfpProgressNote.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = rfpProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpProgressNote.CreatedDate,
                LastModifiedDate = rfpProgressNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpProgressNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpProgressNote.CreatedDate),
            };
        }

        /// <summary>
        /// Formats the specified RFP progress note.
        /// </summary>
        /// <param name="rfpProgressNote">The RFP progress note.</param>
        /// <returns>RfpProgressNoteDTO.</returns>
        private RfpProgressNoteDTO Format(RfpProgressNote rfpProgressNote)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpProgressNoteDTO
            {
                Id = rfpProgressNote.Id,
                IdRfp = rfpProgressNote.IdRfp,
                Notes = rfpProgressNote.Notes,
                CreatedBy = rfpProgressNote.CreatedBy,
                LastModifiedBy = rfpProgressNote.LastModifiedBy != null ? rfpProgressNote.LastModifiedBy : rfpProgressNote.CreatedBy,
                CreatedByEmployee = rfpProgressNote.CreatedByEmployee != null ? rfpProgressNote.CreatedByEmployee.FirstName + " " + rfpProgressNote.CreatedByEmployee.LastName : string.Empty,
                LastModified = rfpProgressNote.LastModified != null ? rfpProgressNote.LastModified.FirstName + " " + rfpProgressNote.LastModified.LastName : (rfpProgressNote.CreatedByEmployee != null ? rfpProgressNote.CreatedByEmployee.FirstName + " " + rfpProgressNote.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = rfpProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpProgressNote.CreatedDate,
                LastModifiedDate = rfpProgressNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpProgressNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (rfpProgressNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfpProgressNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfpProgressNote.CreatedDate),
            };
        }
    }
}