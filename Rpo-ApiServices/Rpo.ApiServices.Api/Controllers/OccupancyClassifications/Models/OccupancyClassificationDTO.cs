// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-16-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-16-2018
// ***********************************************************************
// <copyright file="OccupancyClassificationDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Occupancy Classification DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Occupancy Classifications namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.OccupancyClassifications
{
    using System;

    /// <summary>
    /// Class Occupancy Classification DTO.
    /// </summary>
    public class OccupancyClassificationDTO
    {
        

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

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

        public string Code { get; set; }

        public bool? Is_2008_2014 { get; set; }
    }
}