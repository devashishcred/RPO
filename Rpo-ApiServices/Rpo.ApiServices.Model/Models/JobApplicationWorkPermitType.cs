// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="JobApplicationWorkPermitType.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Application Work Permit Type.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Job Application Work Permit Type.
    /// </summary>
    public class JobApplicationWorkPermitType
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job application.
        /// </summary>
        /// <value>The identifier job application.</value>
        public int IdJobApplication { get; set; }
        /// <summary>
        /// Gets or sets the job application.
        /// </summary>
        /// <value>The job application.</value>
        [ForeignKey("IdJobApplication")]
        public virtual JobApplication JobApplication { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier job work.
        /// </summary>
        /// <value>The type of the identifier job work.</value>
        public int? IdJobWorkType { get; set; }
        /// <summary>
        /// Gets or sets the type of the job work.
        /// </summary>
        /// <value>The type of the job work.</value>
        [ForeignKey("IdJobWorkType")]
        public virtual JobWorkType JobWorkType { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the estimated cost.
        /// </summary>
        /// <value>The estimated cost.</value>
        public double? EstimatedCost { get; set; }

        /// <summary>
        /// Gets or sets the withdrawn.
        /// </summary>
        /// <value>The withdrawn.</value>
        public DateTime? Withdrawn { get; set; }

        /// <summary>
        /// Gets or sets the filed.
        /// </summary>
        /// <value>The filed.</value>
        public DateTime? Filed { get; set; }

        /// <summary>
        /// Gets or sets the issued.
        /// </summary>
        /// <value>The issued.</value>
        public DateTime? Issued { get; set; }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Gets or sets the signed off.
        /// </summary>
        /// <value>The signed off.</value>
        public DateTime? SignedOff { get; set; }

        /// <summary>
        /// Gets or sets the work description.
        /// </summary>
        /// <value>The work description.</value>
        public string WorkDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is person responsible.
        /// </summary>
        /// <value><c>true</c> if this instance is person responsible; otherwise, <c>false</c>.</value>
        public bool IsPersonResponsible { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact responsible.
        /// </summary>
        /// <value>The identifier contact responsible.</value>
        public int? IdContactResponsible { get; set; }

        /// <summary>
        /// Gets or sets the contact responsible.
        /// </summary>
        /// <value>The contact responsible.</value>
        [ForeignKey("IdContactResponsible")]
        public virtual Contact ContactResponsible { get; set; }

        /// <summary>
        /// Gets or sets the identifier responsibility.
        /// </summary>
        /// <value>The identifier responsibility.</value>
        public int? IdResponsibility { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

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
        /// Gets or sets the last modified by employee.
        /// </summary>
        /// <value>The last modified by employee.</value>
        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the document path.
        /// </summary>
        /// <value>The document path.</value>
        public string DocumentPath { get; set; }

        /// <summary>
        /// Gets or sets the permit number.
        /// </summary>
        /// <value>The permit number.</value>
        public string PermitNumber { get; set; }

        /// <summary>
        /// Gets or sets the previous permit number.
        /// </summary>
        /// <value>The previous permit number.</value>
        public string PreviousPermitNumber { get; set; }

        /// <summary>
        /// Gets or sets the renewal fee.
        /// </summary>
        /// <value>The renewal fee.</value>
        public double? RenewalFee { get; set; }

        /// <summary>
        /// Gets or sets the type of the permit.
        /// </summary>
        /// <value>The type of the permit.</value>
        public string PermitType { get; set; }

        /// <summary>
        /// Gets or sets for purpose of.
        /// </summary>
        /// <value>For purpose of.</value>
        public string ForPurposeOf { get; set; }

        /// <summary>
        /// Gets or sets the type of the equipment.
        /// </summary>
        /// <value>The type of the equipment.</value>
        public string EquipmentType { get; set; }

        [ForeignKey("IdWorkPermit")]
        public virtual ICollection<JobWorkPermitHistory> JobWorkPermitHistories { get; set; }

        public DateTime? PlumbingSignedOff { get; set; }
        public DateTime? FinalElevator { get; set; }
        public DateTime? TempElevator { get; set; }
        public DateTime? ConstructionSignedOff { get; set; }

        public string Permittee { get; set; }

        public bool? IsPGL { get; set; }

        public bool? IsCompleted { get; set; }

        /// <summary>
        /// Gets or sets the permit number.
        /// </summary>
        /// <value>The permit number.</value>
        public string DefaultUrl { get; set; }
        public bool HasSuperintendentofconstruction { get; set; }
        public bool HasSiteSafetyCoordinator { get; set; }
        public bool HasSiteSafetyManager { get; set; }
    }
}