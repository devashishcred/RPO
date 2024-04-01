using System;
using System.Collections.Generic;
using Rpo.ApiServices.Model.Models;
using Rpo.ApiServices.Model.Models.Enums;

namespace Rpo.ApiServices.Api.Controllers.RfpEmailHistories
{
    public class RfpEmailHistoryDTO
    {
        public int Id { get; set; }

        public string From { get; internal set; }
        public ICollection<RFPEmailCCHistory> CC { get; internal set; }
        public string To { get; internal set; }
        public IEnumerable<RFPEmailAttachmentHistory> Attachments { get; internal set; }
        public TransmissionType Transmittaltype { get; internal set; }
        public EmailType SentVia { get; internal set; }
        public DateTime SentOn { get; internal set; }
        public string SentBy { get; internal set; }
        public int? IdRfp { get; internal set; }
        public string DocumentFrontPath { get; internal set; }
        public int? IdSentBy { get; internal set; }
        public string EmailMessage { get; internal set; }
    }
}