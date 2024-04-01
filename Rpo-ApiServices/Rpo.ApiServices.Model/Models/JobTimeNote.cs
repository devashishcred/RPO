// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 01-16-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="JobTimeNote.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Time Note.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enums;
    /// <summary>
    /// Class Job Time Note.
    /// </summary>
    public class JobTimeNote
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdJob { get; set; }

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        /// <value>The job.</value>
        [ForeignKey("IdJob")]
        public Job Job { get; set; }

        public JobBillingType JobBillingType { get; set; }

        public int? IdJobFeeSchedule { get; set; }

        [ForeignKey("IdJobFeeSchedule")]
        public virtual JobFeeSchedule JobFeeSchedule { get; set; }
        
        public int? IdRfpJobType { get; set; }

        [ForeignKey("IdRfpJobType")]
        public virtual RfpJobType RfpJobType { get; set; }

        public bool? IsQuickbookSynced { get; set; }

        public DateTime? QuickbookSyncedDate { get; set; }

        public string QuickbookSyncError { get; set; }

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
        [StringLength(4)]
        public string TimeHours { get; set; }

        [StringLength(4)]
        /// <summary>
        /// Gets or sets the time minutes.
        /// </summary>
        /// <value>The time minutes.</value>
        public string TimeMinutes { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

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
        /// Gets or sets the last modified by employee.
        /// </summary>
        /// <value>The last modified by employee.</value>
        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        public bool FromProgressionNote { get; set; }
    }
}
