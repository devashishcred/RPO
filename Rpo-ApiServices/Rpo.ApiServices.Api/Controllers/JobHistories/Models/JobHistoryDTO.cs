// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-16-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="JobHistoryDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job History DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Job Histories namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobHistories
{
    using System;

    /// <summary>
    /// Class Job History DTO.
    /// </summary>
    public class JobHistoryDTO
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
        /// Gets or sets the identifier employee.
        /// </summary>
        /// <value>The identifier employee.</value>
        public int? IdEmployee { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the history date.
        /// </summary>
        /// <value>The history date.</value>
        public DateTime HistoryDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }
    }
}