// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-18-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-01-2018
// ***********************************************************************
// <copyright file="JobMilestoneDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Milestone DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Jobs
{
    using System;

    /// <summary>
    /// Class Job Milestone DTO.
    /// </summary>
    public class JobMilestoneDTO
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
        public int IdJob { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double? Value { get; set; }

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>The units.</value>
        public int? Units { get; set; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public string LastModifiedBy { get; set; }
    }
}
