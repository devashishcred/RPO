// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobTimeNoteCreateUpdate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Time Note Create Update.</summary>
// ***********************************************************************


/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobTimeNotes.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Job Time Note Create Update.
    /// </summary>
    public class JobTimeNoteCreateUpdate
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
        public int? IdJob { get; set; }

        /// <summary>
        /// Gets or sets the type of the job billing.
        /// </summary>
        /// <value>The type of the job billing.</value>
        public JobBillingType JobBillingType { get; set; }

        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int? IdJobFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP job.
        /// </summary>
        /// <value>The type of the identifier RFP job.</value>
        public int? IdRfpJobType { get; set; }

        /// <summary>
        /// Gets or sets the progress notes.
        /// </summary>
        /// <value>The progress notes.</value>
        public string ProgressNotes { get; set; }

        /// <summary>
        /// Gets or sets the time note date.
        /// </summary>
        /// <value>The time note date.</value>
        public DateTime TimeNoteDate { get; set; }

        /// <summary>
        /// Gets or sets the time hours.
        /// </summary>
        /// <value>The time hours.</value>
        public string TimeHours { get; set; }
        /// <summary>
        /// Gets or sets the time minutes.
        /// </summary>
        /// <value>The time minutes.</value>
        public string TimeMinutes { get; set; }
        public bool FromProgressionNote { get; set; }

    }
}