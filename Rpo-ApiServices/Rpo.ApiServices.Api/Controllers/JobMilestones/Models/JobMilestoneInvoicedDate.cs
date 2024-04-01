// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobMilestoneInvoicedDate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Milestone Invoiced Date.</summary>
// ***********************************************************************

/// <summary>
/// The Job Milestones namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobMilestones
{
    /// <summary>
    /// Class Job Milestone Invoiced Date.
    /// </summary>
    public class JobMilestoneInvoicedDate
    {
        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int IdJobMilestone { get; set; }

        /// <summary>
        /// Gets or sets the invoiced date.
        /// </summary>
        /// <value>The invoiced date.</value>
        public string InvoicedDate { get; set; }
    }
}