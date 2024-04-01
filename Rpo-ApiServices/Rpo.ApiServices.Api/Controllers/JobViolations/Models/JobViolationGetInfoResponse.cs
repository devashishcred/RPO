// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-07-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-11-2018
// ***********************************************************************
// <copyright file="JobViolationGetInfoResponse.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Violation Get Info Response.</summary>
// ***********************************************************************

using System.Collections.Generic;
/// <summary>
/// The Job Violations namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobViolations
{
    /// <summary>
    /// Class Job Violation Get Info Response.
    /// </summary>
    public class JobViolationGetInfoResponse
    {
        /// <summary>
        /// Gets or sets the summons number.
        /// </summary>
        /// <value>The summons number.</value>
        public string SummonsNumber { get; set; }

        /// <summary>
        /// Gets or sets the date issued.
        /// </summary>
        /// <value>The date issued.</value>
        public string DateIssued { get; set; }

        /// <summary>
        /// Gets or sets the hearing date.
        /// </summary>
        /// <value>The hearing date.</value>
        public string HearingDate { get; set; }

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
        public string BalanceDue { get; set; }

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

        public bool IsFullyResolved { get; set; }

        /// <summary>
        /// Gets or sets the compliance on.
        /// </summary>
        /// <value>The compliance on.</value>
        public string ComplianceOn { get; set; }

        /// <summary>
        /// Gets or sets the certification status.
        /// </summary>
        /// <value>The certification status.</value>
        public string CertificationStatus { get; set; }
        
        /// <summary>
        /// Gets or sets the explanation of charges.
        /// </summary>
        /// <value>The explanation of charges.</value>
        public List<ExplanationOfCharge> ExplanationOfCharges { get; set; }
    }

    /// <summary>
    /// Class ExplanationOfCharge.
    /// </summary>
    public class ExplanationOfCharge
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the panelty amount.
        /// </summary>
        /// <value>The panelty amount.</value>
        public string PaneltyAmount { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the code section.
        /// </summary>
        /// <value>The code section.</value>
        public string CodeSection { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        public bool IsFromAuth { get; set; }
    }
}