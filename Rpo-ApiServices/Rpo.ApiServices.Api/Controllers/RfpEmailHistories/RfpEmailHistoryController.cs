// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-24-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="RfpEmailHistoryController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


/// <summary>
/// The RfpEmailHistories namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.RfpEmailHistories
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.Http;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Api.Tools;
    using Filters;

    /// <summary>
    /// Class RfpEmailHistoryController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class RfpEmailHistoryController : ApiController
    {
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the RFP email historys.
        /// </summary>
        /// <returns> Get the List of all RFPs Email History list.</returns>
        [Authorize]
        [RpoAuthorize]
        public IQueryable<RFPEmailHistory> GetRfpEmailHistorys()
        {
            return db.RFPEmailHistories
                .Include("RFPEmailCCHistories")
                .Include("RFPEmailAttachmentHistories")
                .Include("EmailType")
                .Include("TransmissionType")
                .Include("ContactAttention")
                .Include("ToCompany")
                .Include("FromEmployee")
                .Include("Rfp")
                .Include("SentBy")
                .OrderByDescending(x => x.SentDate);
        }

        /// <summary>
        /// Gets the RFP email history.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Get the list of Email history against the RFP.</returns>
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetRfpEmailHistory(int id)
        {
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = db.RFPEmailHistories
                .Include("RFPEmailCCHistories.Contact")
                .Include("RFPEmailAttachmentHistories")
                .Include("EmailType")
                .Include("TransmissionType")
                .Include("ContactAttention")
                .Include("ToCompany")
                .Include("FromEmployee")
                .Include("Rfp")
                .Include("SentBy")
               //.Where(x => x.IdRfp == id)
                      //added by M.B isemailsent true
                 .Where(x => x.IdRfp == id && x.IsEmailSent==true)
                .AsEnumerable()
                .Select(jc => new
                {
                    From = jc.FromEmployee.FirstName + " " + jc.FromEmployee.LastName + " ( " + jc.FromEmployee.Email + " )",
                    To = jc.ContactAttention.FirstName + " " + jc.ContactAttention.LastName + " ( " + jc.ContactAttention.Email + " )",
                    RFPEmailCC = jc.RFPEmailCCHistories,
                    Attachments = jc.RFPEmailAttachmentHistories,
                    DocumentFrontPath = APIUrl + "/" + Properties.Settings.Default.RFPAttachmentsPath + "/",
                    Transmittaltype = jc.TransmissionType,
                    SentVia = jc.EmailType,
                    SentOn = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jc.SentDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                    IdSentBy = jc.IdSentBy,
                    EmailMessage = jc.EmailMessage,
                    SentBy = jc.SentBy != null ? jc.SentBy.FirstName + " " + jc.SentBy.LastName : string.Empty,
                    Id = jc.Id,
                    IdRfp = jc.IdRfp
                }).OrderByDescending(x => x.SentOn);

            return Ok(result);
        }

        /// <summary>
        /// Formats the specified r fp email history.
        /// </summary>
        /// <param name="rFPEmailHistory">The r fp email history.</param>
        /// <returns>RfpEmailHistoryDTO.</returns>
        private RfpEmailHistoryDTO format(RFPEmailHistory rFPEmailHistory)
        {
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            return new RfpEmailHistoryDTO
            {
                From = rFPEmailHistory.FromEmployee.FirstName + " " + rFPEmailHistory.FromEmployee.LastName,
                To = rFPEmailHistory.ContactAttention.FirstName + " " + rFPEmailHistory.ContactAttention.LastName,
                CC = rFPEmailHistory.RFPEmailCCHistories,
                Attachments = rFPEmailHistory.RFPEmailAttachmentHistories,
                DocumentFrontPath = APIUrl + "/" + Properties.Settings.Default.RFPAttachmentsPath + "/",
                Transmittaltype = rFPEmailHistory.TransmissionType,
                SentVia = rFPEmailHistory.EmailType,
                SentOn = rFPEmailHistory.SentDate,
                IdSentBy = rFPEmailHistory.IdSentBy,
                EmailMessage = rFPEmailHistory.EmailMessage,
                SentBy = rFPEmailHistory.SentBy != null ? rFPEmailHistory.SentBy.FirstName + " " + rFPEmailHistory.SentBy.LastName : string.Empty,
                Id = rFPEmailHistory.Id,
                IdRfp = rFPEmailHistory.IdRfp
            };
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}