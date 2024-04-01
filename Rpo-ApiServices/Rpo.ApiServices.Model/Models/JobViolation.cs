// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="JobViolation.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Violation.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class JobViolation.
    /// </summary>
    public class JobViolation
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        public int? IdJob { get; set; }

        [ForeignKey("IdJob")]
        public virtual Job Job { get; set; }
        public string BinNumber { get; set; }
        [Required]
        public string SummonsNumber { get; set; }

        public DateTime? DateIssued { get; set; }

        public DateTime? HearingDate { get; set; }
        public DateTime? HearingTime { get; set; }

        public string HearingLocation { get; set; }

        public string HearingResult { get; set; }

        public string StatusOfSummonsNotice { get; set; }

        public string RespondentAddress { get; set; }

        public string InspectionLocation { get; set; }

        public double? BalanceDue { get; set; }

        public string RespondentName { get; set; }

        public string IssuingAgency { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsCOC { get; set; }

        public DateTime? COCDate { get; set; }
        public double? PaneltyAmount { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }
        public DateTime? ComplianceOn { get; set; }
        public string CertificationStatus { get; set; }
        public int? NotesLastModifiedBy { get; set; }

        [ForeignKey("NotesLastModifiedBy")]
        public Employee NotesLastModifiedByEmployee { get; set; }

        public DateTime? NotesLastModifiedDate { get; set; }
        public string violation_type_code { get; set; }

        public string violation_number { get; set; }

        public string device_number { get; set; }

        public string description_date { get; set; }

        public string ECBnumber { get; set; }

        public string violation_category { get; set; }

        public string violation_type { get; set; }
        public string DOB_Description { get; set; }
        public DateTime? Disposition_Date { get; set; }
        public string Disposition_Comments { get; set; }
        public bool IsFullyResolved { get; set; }

        public DateTime? ResolvedDate { get; set; }
        public DateTime? CureDate { get; set; }
        public int? PartyResponsible { get; set; }

        public string ManualPartyResponsible { get; set; }
        public int? IdContact { get; set; }
        public string ViolationDescription { get; set; }
        public bool? TCOToggle { get; set; }

        public bool? IsChecklistView { get; set; }
        public string Type_ECB_DOB { get; set; }
        public string aggravated_level { get; set; }
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
        public string ISNViolation { get; set; }
        
        /// <summary>
        /// Gets or sets the job violation documents.
        /// </summary>
        /// <value>The job violation documents.</value>
        [ForeignKey("IdJobViolation")]
        public virtual ICollection<JobViolationDocument> JobViolationDocuments { get; set; }

        /// <summary>
        /// Gets or sets the job violation Explanation Of Charges.
        /// </summary>
        /// <value>The job violation Explanation Of Charges.</value>
        [ForeignKey("IdViolation")]
        public virtual ICollection<JobViolationExplanationOfCharges> explanationOfCharges { get; set; }

        [ForeignKey("IdContact")]
        public virtual Contact Contacts { get; set; }

        public int Status { get; set; }
        public bool IsNewMailsent { get; set; }
        public bool IsUpdateMailsent { get; set; }

    }
    }
