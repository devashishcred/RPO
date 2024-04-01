// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="DOBPenaltyScheduleDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class DOB Penalty Schedule DTO.</summary>
// ***********************************************************************

/// <summary>
/// The DOBPenaltySchedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.DOBPenaltySchedules
{
    using System;

    /// <summary>
    /// Class DOB Penalty Schedule  DTO.
    /// </summary>
    public class DOBPenaltyScheduleDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        public string SectionOfLaw { get; set; }

        public string Classification { get; set; }

        public string InfractionCode { get; set; }

        public string ViolationDescription { get; set; }

        public bool Cure { get; set; }

        public bool Stipulation { get; set; }

        public double? StandardPenalty { get; set; }

        public string FormattedStandardPenalty { get; set; }

        public bool MitigatedPenalty { get; set; }

        public double? DefaultPenalty { get; set; }

        public string FormattedDefaultPenalty { get; set; }

        public double? AggravatedPenalty_I { get; set; }

        public string FormattedAggravatedPenalty_I { get; set; }

        public double? AggravatedDefaultPenalty_I { get; set; }

        public string FormattedAggravatedDefaultPenalty_I { get; set; }

        public double? AggravatedPenalty_II { get; set; }

        public string FormattedAggravatedPenalty_II { get; set; }

        public double? AggravatedDefaultMaxPenalty_II { get; set; }

        public string FormattedAggravatedDefaultMaxPenalty_II { get; set; }

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