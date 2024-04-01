using System;

namespace Rpo.ApiServices.Api.Controllers.Dashboards
{
    public class UpcomingHearingDateDTO
    {
        public object Address { get; internal set; }
        public double? BalanceDue { get; set; }
        public string CertificationStatus { get; set; }
        public DateTime? COCDate { get; set; }
        public DateTime? ComplianceOn { get; set; }
        public DateTime? CureDate { get; set; }
        public DateTime? DateIssued { get; set; }
        public DateTime? HearingDate { get; set; }
        public string HearingLocation { get; set; }
        public string HearingResult { get; set; }
        public int Id { get; set; }
       // public int? IdJob { get; set; }
        public string IdJob { get; set; }
        public string InspectionLocation { get; set; }
        public bool IsCOC { get; set; }
        public bool IsFullyResolved { get; set; }
        public string IssuingAgency { get; set; }
        public string JobNumber { get; internal set; }
        public string Notes { get; set; }
        public double? PaneltyAmount { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string RespondentAddress { get; set; }
        public string RespondentName { get; set; }
        public string StatusOfSummonsNotice { get; set; }
        public string SummonsNumber { get; set; }
        public string JobApplicationType { get; set; }
    }
}