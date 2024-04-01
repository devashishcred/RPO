// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobViolationCreateUpdate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Violation Create Update.</summary>
// ***********************************************************************

/// <summary>
/// The Job Violations namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobViolations
    {
    using System;
    using System.Collections.Generic;
    using Model;
    using Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown;

    /// <summary>
    /// Class Job Violation Create Update.
    /// </summary>
    public class JobViolationCreateUpdate
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
        public double BalanceDue { get; set; }

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
        /// Gets or sets a value indicating whether this instance is coc.
        /// </summary>
        /// <value><c>true</c> if this instance is coc; otherwise, <c>false</c>.</value>
        public bool IsCOC { get; set; }

        /// <summary>
        /// Gets or sets the coc date.
        /// </summary>
        /// <value>The coc date.</value>
        public DateTime? COCDate { get; set; }

        /// <summary>
        /// Gets or sets the documents to delete.
        /// </summary>
        /// <value>The documents to delete.</value>
        public virtual IEnumerable<int> DocumentsToDelete { get; set; }

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
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the panelty amount.
        /// </summary>
        /// <value>The panelty amount.</value>
        public double? PaneltyAmount { get; set; }


        /// <summary>
        /// Gets or sets the violation Explanation Of Charges.
        /// </summary>
        /// <value>The violation Explanation Of Charges.</value>
        public virtual ICollection<JobViolationExplanationOfCharges> explanationOfCharges { get; set; }

        public bool IsFullyResolved { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public string ViolationType { get; set; }
        public string AggravatedLevel { get; set; }

        public DateTime? DispositionDate { get; set; }
        public string DispositionComments { get; set; }
        public string DeviceNumber { get; set; }
        public string ViolationDescription { get; set; }
        public string ECBNumber { get; set; }
        public string ViolationNumber { get; set; }
        public string ViolationCategory { get; set; }
        public string BinNumber { get; set; }
        public int? PartyResponsible { get; set; }
        public string ManualPartyResponsible { get; set; }
        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdContact { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public String HearingTime { get; set; }
        public bool isManually { get; set; }
    }
    }