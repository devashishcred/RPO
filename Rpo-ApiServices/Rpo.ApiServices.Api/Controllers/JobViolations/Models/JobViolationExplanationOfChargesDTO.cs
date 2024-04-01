// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Richa Patel
// Created          : 04-06-2018

// ***********************************************************************
// <copyright file="JobViolationExplanationOfChargesDTO.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary>Class Job Violation Explanation Of Charges DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobViolations
{
    using Model.Models;
    using System;

    /// <summary>
    /// Class Violation penalty Code DTO.
    /// </summary>
    public class JobViolationExplanationOfChargesDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }


        /// <summary>
        /// Gets or sets the Id of Violation.
        /// </summary>
        /// <value>The Violation ID.</value>
        public int? IdViolation { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }



        /// <summary>
        /// Gets or sets the code section.
        /// </summary>
        /// <value>The code section.</value>
        public string CodeSection { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets the FaceAmount.
        /// </summary>
        /// <value>The FaceAmount.</value>
        public double? PaneltyAmount { get; set; }

        /// <summary>
        /// Gets the IsFromAuth.
        /// </summary>
        /// <value>The IsFromAuth.</value>
        public bool IsFromAuth { get; set; }


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

        public DOBPenaltySchedule DOBPenaltySchedule { get; set; }

        public FDNYPenaltySchedule FDNYPenaltySchedule { get; set; }
    }
}