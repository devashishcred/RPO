using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist.Models
{
    public class JobCompositeViolationDTO
    {
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
        /// Gets or sets the summons number.
        /// </summary>
        /// <value>The summons number.</value>
        public string SummonsNumber { get; set; }

        /// <summary>
        /// Gets or sets the date issued.
        /// </summary>
        /// <value>The date issued.</value>
        public DateTime? DateIssued { get; set; }

        public DateTime? CureDate { get; set; }

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
        /// Gets or sets the status of summons notice.
        /// </summary>
        /// <value>The status of summons notice.</value>
        public string StatusOfSummonsNotice { get; set; }

        /// <summary>
        /// Gets or sets the respondent address.
        /// </summary>
        /// <value>The respondent address.</value>
        public string RespondentAddress { get; set; }

        /// <summary>
        /// Gets or sets the inspection location.
        /// </summary>
        /// <value>The inspection location.</value>
        public string InspectionLocation { get; set; }

        /// <summary>
        /// Gets or sets the balance due.
        /// </summary>
        /// <value>The balance due.</value>
        public double? BalanceDue { get; set; }

        /// <summary>
        /// Gets or sets the name of the respondent.
        /// </summary>
        /// <value>The name of the respondent.</value>
        public string RespondentName { get; set; }

        /// <summary>
        /// Gets or sets the issuing agency.
        /// </summary>
        /// <value>The issuing agency.</value>
        public string IssuingAgency { get; set; }

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
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }


        /// <summary>
        /// Gets or sets the panelty amount.
        /// </summary>
        /// <value>The panelty amount.</value>
        public double? PaneltyAmount { get; set; }


        public bool IsCOC { get; set; }

        /// <summary>
        /// Gets or sets the coc date.
        /// </summary>
        /// <value>The coc date.</value>
        public DateTime? COCDate { get; set; }

        /// <summary>
        /// Gets or sets the Compliance On.
        /// </summary>
        /// <value>The ComplianceOn.</value>
        public DateTime? ComplianceOn { get; set; }

        /// <summary>
        /// Gets or sets the Certification Status.
        /// </summary>
        /// <value>The CertificationStatus.</value>
        public string CertificationStatus { get; set; }

        /// <summary>
        /// Gets or sets the code string.
        /// </summary>
        /// <value>The code string.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the code section string.
        /// </summary>
        /// <value>The code section string.</value>
        public string CodeSection { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public bool IsFullyResolved { get; set; }

        public DateTime? NotesLastModifiedDate { get; set; }

        public string NotesLastModifiedByEmployeeName { get; set; }
        public DateTime? DispositionDate { get; set; }
        public string DispositionComments { get; set; }
        public string DeviceNumber { get; set; }
        public string ViolationDescription { get; set; }
        public string ECBNumber { get; set; }
        public string ViolationNumber { get; set; }
        public string ViolationCategory { get; set; }
        public string BinNumber { get; set; }
        public bool TCOToggle { get; set; }
        public int? PartyResponsible { get; set; }
        public string ManualPartyResponsible { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
        public string isnViolation { get; set; }
    }
}