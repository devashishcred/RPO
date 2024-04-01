using Rpo.ApiServices.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.Reports.Models
{
    public class JobTimeNoteUnsynchDTO
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
        /// Gets or sets the job number.
        /// </summary>
        /// <value>The job number.</value>
        public string JobNumber { get; set; }

        public JobBillingType JobBillingType { get; set; }

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
        /// Gets or sets a value indicating whether this <see cref="JobTimeNoteDetail"/> is billable.
        /// </summary>
        /// <value><c>true</c> if billable; otherwise, <c>false</c>.</value>
        public bool Billable { get; set; }

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

        /// <summary>
        /// Gets or sets the quickbook synchronize error.
        /// </summary>
        /// <value>The quickbook synchronize error.</value>
        public string QuickbookSyncError { get; set; }

        /// <summary>
        /// Gets or sets the quickbook synced date.
        /// </summary>
        /// <value>The quickbook synced date.</value>
        public DateTime? QuickbookSyncedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is quickbook synced.
        /// </summary>
        /// <value><c>null</c> if [is quickbook synced] contains no value, <c>true</c> if [is quickbook synced]; otherwise, <c>false</c>.</value>
        public bool? IsQuickbookSynced { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP job.
        /// </summary>
        /// <value>The type of the identifier RFP job.</value>
        public int? IdRfpJobType { get; set; }

        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int? IdJobFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the service item.
        /// </summary>
        /// <value>The service item.</value>
        public string ServiceItem { get; set; }

        public string JobApplicationType { get; set; }
    }
}
