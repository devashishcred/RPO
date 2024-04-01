// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="DEPNoiseCodePenaltyScheduleDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class DOB Penalty Schedule DTO.</summary>
// ***********************************************************************

/// <summary>
/// The DEPNoiseCodePenaltySchedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.DEPNoiseCodePenaltySchedules
{
    using System;

    /// <summary>
    /// Class DOB Penalty Schedule  DTO.
    /// </summary>
    public class DEPNoiseCodePenaltyScheduleDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        public string SectionOfLaw { get; set; }

        public string ViolationDescription { get; set; }

        public string Compliance { get; set; }

        public string Offense_1 { get; set; }

        public double? Penalty_1 { get; set; }

        public double? DefaultPenalty_1 { get; set; }

        public string FormattedPenalty_1 { get; set; }

        public string FormattedDefaultPenalty_1 { get; set; }

        public bool Stipulation_1 { get; set; }

        public string Offense_2 { get; set; }

        public double? Penalty_2 { get; set; }

        public double? DefaultPenalty_2 { get; set; }

        public string FormattedPenalty_2 { get; set; }

        public string FormattedDefaultPenalty_2 { get; set; }

        public bool Stipulation_2 { get; set; }

        public string Offense_3 { get; set; }

        public double? Penalty_3 { get; set; }

        public double? DefaultPenalty_3 { get; set; }

        public string FormattedPenalty_3 { get; set; }

        public string FormattedDefaultPenalty_3 { get; set; }

        public bool Stipulation_3 { get; set; }

        public string Offense_4 { get; set; }

        public double? Penalty_4 { get; set; }

        public double? DefaultPenalty_4 { get; set; }

        public string FormattedPenalty_4 { get; set; }

        public string FormattedDefaultPenalty_4 { get; set; }

        public bool Stipulation_4 { get; set; }

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