// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="JobApplicationWorkPermitDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Application Work Permit DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.JobApplicationWorkPermits
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Job Application Work Permit DTO.
    /// </summary>
    public class JobApplicationWorkPermitDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job application.
        /// </summary>
        /// <value>The identifier job application.</value>
        public int IdJobApplication { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }

        /// <summary>
        /// Gets or sets the job application number.
        /// </summary>
        /// <value>The job application number.</value>
        public string JobApplicationNumber { get; set; }

        /// <summary>
        /// Gets or sets the job application for.
        /// </summary>
        /// <value>The job application for.</value>
        public string JobApplicationFor { get; set; }

        /// <summary>
        /// Gets or sets the job application status identifier.
        /// </summary>
        /// <value>The job application status identifier.</value>
        public int? JobApplicationStatusId { get; set; }

        /// <summary>
        /// Gets or sets the job application status.
        /// </summary>
        /// <value>The job application status.</value>
        public string JobApplicationStatus { get; set; }

        /// <summary>
        /// Gets or sets the job application floor.
        /// </summary>
        /// <value>The job application floor.</value>
        public string JobApplicationFloor { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier job work.
        /// </summary>
        /// <value>The type of the identifier job work.</value>
        public int? IdJobWorkType { get; set; }

        /// <summary>
        /// Gets or sets the job work type description.
        /// </summary>
        /// <value>The job work type description.</value>
        public string JobWorkTypeDescription { get; set; }

        /// <summary>
        /// Gets or sets the content of the job work type.
        /// </summary>
        /// <value>The content of the job work type.</value>
        public string JobWorkTypeContent { get; set; }

        /// <summary>
        /// Gets or sets the job work type number.
        /// </summary>
        /// <value>The job work type number.</value>
        public string JobWorkTypeNumber { get; set; }

        /// <summary>
        /// Gets or sets the job work type code.
        /// </summary>
        /// <value>The job work type code.</value>
        public string JobWorkTypeCode { get; set; }

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
        /// Gets or sets the company responsible.
        /// </summary>
        /// <value>The company responsible.</value>
        public string CompanyResponsible { get; set; }

        /// <summary>
        /// Gets or sets the personal responsible.
        /// </summary>
        /// <value>The personal responsible.</value>
        public string PersonalResponsible { get; set; }

        /// <summary>
        /// Gets or sets the work description.
        /// </summary>
        /// <value>The work description.</value>
        public string WorkDescription { get; set; }

        /// <summary>
        /// Gets the name of the job application type.
        /// </summary>
        /// <value>The name of the job application type.</value>
        public string JobApplicationTypeName { get; set; }

        /// <summary>
        /// Gets the type of the identifier job application.
        /// </summary>
        /// <value>The type of the identifier job application.</value>
        public int? IdJobApplicationType { get; set; }

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
        /// Gets or sets the identifier responsibility.
        /// </summary>
        /// <value>The identifier responsibility.</value>
        public int? IdResponsibility { get; set; }

        /// <summary>
        /// Gets the contact responsible.
        /// </summary>
        /// <value>The contact responsible.</value>
        public Contact ContactResponsible { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }
        /// <summary>
        /// Gets the renewal fee.
        /// </summary>
        /// <value>The renewal fee.</value>
        public double? RenewalFee { get; set; }

        /// <summary>
        /// Gets the previous permit number.
        /// </summary>
        /// <value>The previous permit number.</value>
        public string PreviousPermitNumber { get; set; }

        /// <summary>
        /// Gets the permit number.
        /// </summary>
        /// <value>The permit number.</value>
        public string PermitNumber { get; set; }

        /// <summary>
        /// Gets or sets the document path.
        /// </summary>
        /// <value>The document path.</value>
        public string DocumentPath { get; set; }
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

        public IEnumerable<JobWorkPermitHistoryDTO> JobWorkPermitHistories { get; set; }

        public string Permittee { get; set; }

        public DateTime? PlumbingSignedOff { get; set; }

        public DateTime? FinalElevator { get; set; }

        public DateTime? TempElevator { get; set; }

        public DateTime? ConstructionSignedOff { get; set; }
        public bool? IsPGL { get; set; }

        public bool HasSuperintendentofconstruction { get; set; }      
        public bool HasSiteSafetyCoordinator { get; set; }
        public bool HasSiteSafetyManager { get; set; }


        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Created By Employee Name
        /// </summary>
        public string CreatedByEmployeeName { get; set; }

        /// <summary>
        /// Created By Employee Name
        /// </summary>
        public string LastModifiedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? LastModifiedBy { get; set; }
        public string DetailURL { get; set; }
    }

    public class JobWorkPermitHistoryDTO
    {
        public int Id { get; set; }

        public int? IdWorkPermit { get; set; }

        public int? IdJobApplication { get; set; }

        public string NewNumber { get; set; }

        public int? CreatedBy { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string OldNumber { get; internal set; }
    }
}