// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="FDNYPenaltyScheduleDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class DOB Penalty Schedule Detail.</summary>
// ***********************************************************************

/// <summary>
/// The FDNYPenaltySchedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.FDNYPenaltySchedules
{
    using System;

    /// <summary>
    /// Class DOB Penalty Schedule Detail.
    /// </summary>
    public class FDNYPenaltyScheduleDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        public string Category_RCNY { get; set; }

        public string DescriptionOfViolation { get; set; }

        public string OATHViolationCode { get; set; }

        public double? FirstViolationPenalty { get; set; }
        public string FormattedFirstViolationPenalty { get; set; }

        public double? FirstViolationMitigatedPenalty { get; set; }
        public string FormattedFirstViolationMitigatedPenalty { get; set; }

        public double? FirstViolationMaximumPenalty { get; set; }
        public string FormattedFirstViolationMaximumPenalty { get; set; }

        public double? SecondSubsequentViolationPenalty { get; set; }
        public string FormattedSecondSubsequentViolationPenalty { get; set; }

        public double? SecondSubsequentViolationMitigatedPenalty { get; set; }

        public string FormattedSecondSubsequentViolationMitigatedPenalty { get; set; }

        public double? SecondSubsequentViolationMaximumPenalty { get; set; }

        public string FormattedSecondSubsequentViolationMaximumPenalty { get; set; }

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
    }
}