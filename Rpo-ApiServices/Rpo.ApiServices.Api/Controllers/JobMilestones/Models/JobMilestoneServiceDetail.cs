// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobMilestoneServiceDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Milestone Service Detail.</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobMilestones
{
    /// <summary>
    /// Class Job Milestone Service Detail.
    /// </summary>
    public class JobMilestoneServiceDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier milestone.
        /// </summary>
        /// <value>The identifier milestone.</value>
        public int IdMilestone { get; set; }

        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int IdJobFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public string ItemName { get; set; }
    }
}