using System;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class AllViolationDTO
    {
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the balance due.
        /// </summary>
        /// <value>The balance due.</value>
        public double? BalanceDue { get; set; }

        /// <summary>
        /// Gets or sets the certification status.
        /// </summary>
        /// <value>The certification status.</value>
        public string CertificationStatus { get; set; }

        /// <summary>
        /// Gets or sets the code section.
        /// </summary>
        /// <value>The code section.</value>
        public string CodeSection { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the created by employee.
        /// </summary>
        /// <value>The name of the created by employee.</value>
        public string CreatedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        public DateTime? CureDate { get; set; }

        /// <summary>
        /// Gets or sets the date issued.
        /// </summary>
        /// <value>The date issued.</value>
        public DateTime? DateIssued { get; set; }
        public string Description { get; set; }
        public string FormattedBalanceDue { get; set; }

        /// <summary>
        /// Gets or sets the hearing date.
        /// </summary>
        /// <value>The hearing date.</value>
        public DateTime? HearingDate { get; set; }

        /// <summary>
        /// Gets or sets the hearing location.
        /// </summary>
        /// <value>The hearing location.</value>
        public string HearingLocation { get; set; }

        /// <summary>
        /// Gets or sets the hearing result.
        /// </summary>
        /// <value>The hearing result.</value>
        public string HearingResult { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdJob { get; set; }

        /// <summary>
        /// Gets or sets the infraction code.
        /// </summary>
        /// <value>The infraction code.</value>
        public string InfractionCode { get; set; }

        public string InspectionLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fully resolved.
        /// </summary>
        /// <value><c>true</c> if this instance is fully resolved; otherwise, <c>false</c>.</value>
        public bool IsFullyResolved { get; set; }

        /// <summary>
        /// Gets or sets the issuing agency.
        /// </summary>
        /// <value>The issuing agency.</value>
        public string IssuingAgency { get; set; }

        public string JobNumber { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last name of the modified by employee.
        /// </summary>
        /// <value>The last name of the modified by employee.</value>
        public string LastModifiedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the resolved date.
        /// </summary>
        /// <value>The resolved date.</value>
        public DateTime? ResolvedDate { get; set; }

        /// <summary>
        /// Gets or sets the status of summons notice.
        /// </summary>
        /// <value>The status of summons notice.</value>
        public string StatusOfSummonsNotice { get; set; }

        /// <summary>
        /// Gets or sets the summons number.
        /// </summary>
        /// <value>The summons number.</value>
        public string SummonsNumber { get; set; }
        public string JobNumbers { get; set; }
        public string JobNames { get; set; }
        public string ECBNumber { get; set; }
        public int PartyResponsible { get; set; }
        public string ManualPartyResponsible { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
        public string RelatedECB { get; set; }
        public string ViolationCategory { get; set; }
        public string ViolationDescription { get; set; }
    }
}