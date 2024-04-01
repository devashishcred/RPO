// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="DOTPenaltyScheduleDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class DOB Penalty Schedule DTO.</summary>
// ***********************************************************************

/// <summary>
/// The DOTPenaltySchedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.DOTPenaltySchedules
{
    using System;

    /// <summary>
    /// Class DOB Penalty Schedule  DTO.
    /// </summary>
    public class DOTPenaltyScheduleDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        public string Section { get; set; }

        public string Description { get; set; }

        public double? Penalty { get; set; }

        public string FormattedPenalty { get; set; }

        public double? DefaultPenalty { get; set; }

        public string FormattedDefaultPenalty { get; set; }

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