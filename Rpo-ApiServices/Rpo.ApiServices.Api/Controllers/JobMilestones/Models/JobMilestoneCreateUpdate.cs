// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-13-2018
// ***********************************************************************
// <copyright file="JobMilestoneCreateUpdate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Milestone Create Update.</summary>
// ***********************************************************************

/// <summary>
/// The Job Milestones namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobMilestones
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class Job Milestone Create Update.
    /// </summary>
    public class JobMilestoneCreateUpdate
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

        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the job milestone services.
        /// </summary>
        /// <value>The job milestone services.</value>
        public virtual ICollection<JobMilestoneServiceDetail> JobMilestoneServices { get; set; }
    }

    


    public class JobMilestonePONumber
    {
        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int IdJobMilestone { get; set; }
        
        /// <summary>
        /// Gets or sets the po number.
        /// </summary>
        /// <value>The po number.</value>
        public string PONumber { get; set; }
    }

    public class JobMilestoneInvoiceNumber
    {
        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int IdJobMilestone { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        public string InvoiceNumber { get; set; }
    }

    public class JobMilestoneIsInvoiced
    {
        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int IdJobMilestone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is invoiced.
        /// </summary>
        /// <value><c>true</c> if this instance is invoiced; otherwise, <c>false</c>.</value>
        public bool IsInvoiced { get; set; }
    }

    public class JobMilestoneStatus
    {
        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int IdJobMilestone { get; set; }

        /// <summary>
        /// Gets or sets the milestone status.
        /// </summary>
        /// <value>The milestone status.</value>
        public string MilestoneStatus { get; set; }
    }
}