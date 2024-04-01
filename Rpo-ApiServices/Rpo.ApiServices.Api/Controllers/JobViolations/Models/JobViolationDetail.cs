// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-08-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobViolationDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Violation Detail.</summary>
// ***********************************************************************

/// <summary>
/// The Job Violations namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobViolations
{
    using Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown;
    using Rpo.ApiServices.Api.Controllers.JobViolations.Models;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class Job Violation Detail.
    /// </summary>
    public class JobViolationDetail
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
        /// Gets the job violation documents.
        /// </summary>
        /// <value>The job violation documents.</value>
        public ICollection<JobViolationDocumentDetail> JobViolationDocuments { get; internal set; }

        /// <summary>
        /// Gets or sets the coc date.
        /// </summary>
        /// <value>The coc date.</value>
        public DateTime? COCDate { get;  set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is coc.
        /// </summary>
        /// <value><c>true</c> if this instance is coc; otherwise, <c>false</c>.</value>
        public bool IsCOC { get; set; }

       

        /// <summary>
        /// Gets or sets the violation Explanation Of Charges.
        /// </summary>
        /// <value>The violation Explanation Of Charges.</value>
        public virtual ICollection<JobViolationExplanationOfChargesDTO> explanationOfCharges { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public bool IsFullyResolved { get; set; }
        public DateTime? NotesLastModifiedDate { get; set; }
        public string NotesLastModifiedByEmployeeName { get; set; }
        public DateTime? CureDate { get; set; }
        /// <summary>
        /// Gets or sets the Aggravated Level.
        /// </summary>
        /// <value>The AggravatedLevel.</value>
        public string AggravatedLevel { get; set; }
        /// <summary>
        /// Gets or sets the Violation Type.
        /// </summary>
        /// <value>The ViolationType.</value>
        public string ViolationType { get; set; }
        public int? PartyResponsible { get; set; }        
        public DateTime? DispositionDate { get; set; }
        public string DispositionComments { get; set; }
        public string DeviceNumber { get; set; }
        public string ViolationDescription { get; set; }
        public string ECBNumber { get; set; }
        public string ViolationNumber { get; set; }
        public string ViolationCategory { get; set; }
        public string BinNumber { get; set; }
        public List<ViolationPartOfJobs> PartOfJobs { get; set; }
       
        public int? IdContact { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public string ManualPartyResponsible { get; set; }
        
       

       
        public DateTime? HearingTime { get; set; }

        public int? NotesLastModifiedBy { get; set; }

        public string violation_type_code { get; set; }

        public string violation_number { get; set; }

        public string device_number { get; set; }

        public string description_date { get; set; }

        public string ecb_number { get; set; }

        public string violation_category { get; set; }
        public string DOB_Description { get; set; }
        public DateTime? Disposition_Date { get; set; }
        public string Disposition_Comments { get; set; }
      
        public bool? TCOToggle { get; set; }

        public bool? IsChecklistView { get; set; }
        public string Type_ECB_DOB { get; set; }
        public string Infraction_Code1 { get; set; }
        public string Section_Law_Description1 { get; set; }
        public string Infraction_Code2 { get; set; }
        public string Section_Law_Description2 { get; set; }
        public string Infraction_Code3 { get; set; }
        public string Section_Law_Description3 { get; set; }
        public string Infraction_Code4 { get; set; }
        public string Section_Law_Description4 { get; set; }
        public string Infraction_Code5 { get; set; }
        public string Section_Law_Description5 { get; set; }
        public string Infraction_Code6 { get; set; }
        public string Section_Law_Description6 { get; set; }
        public string Infraction_Code7 { get; set; }
        public string Section_Law_Description7 { get; set; }
        public string Infraction_Code8 { get; set; }
        public string Section_Law_Description8 { get; set; }
        public string Infraction_Code9 { get; set; }
        public string Section_Law_Description9 { get; set; }
        public string Infraction_Code10 { get; set; }
        public string Section_Law_Description10 { get; set; }
        public bool IsManually { get; set; }
        public int Status { get; set; }

    }
}