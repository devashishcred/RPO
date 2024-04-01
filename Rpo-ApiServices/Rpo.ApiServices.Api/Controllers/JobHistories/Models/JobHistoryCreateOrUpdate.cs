// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-17-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-17-2018
// ***********************************************************************
// <copyright file="JobHistoryCreateOrUpdate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job History Create Or Update.</summary>
// ***********************************************************************

/// <summary>
/// The Job Histories namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobHistories
{
    using System.ComponentModel.DataAnnotations;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Job History Create Or Update.
    /// </summary>
    public class JobHistoryCreateOrUpdate
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the job history.
        /// </summary>
        /// <value>The type of the job history.</value>
        public JobHistoryType JobHistoryType { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }
    }
}