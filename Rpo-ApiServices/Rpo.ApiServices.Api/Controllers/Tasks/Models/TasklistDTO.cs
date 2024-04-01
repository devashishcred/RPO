using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.Tasks
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Api.Controllers.TaskNotes;
    using Model.Models.Enums;
    public class TasklistDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the assigned date.
        /// </summary>
        /// <value>The assigned date.</value>
        public DateTime? AssignedDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier assigned to.
        /// </summary>
        /// <value>The identifier assigned to.</value>
        public int? IdAssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the assigned to.
        /// </summary>
        /// <value>The assigned to.</value>
        public string AssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the identifier assigned by.
        /// </summary>
        /// <value>The identifier assigned by.</value>
        //public int? IdAssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the assigned by.
        /// </summary>
        /// <value>The assigned by.</value>
        public string AssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier task.
        /// </summary>
        /// <value>The type of the identifier task.</value>
        public int IdTaskType { get; set; }

        /// <summary>
        /// Gets or sets the type of the task.
        /// </summary>
        /// <value>The type of the task.</value>
        public string TaskType { get; set; }

        /// <summary>
        /// Gets or sets the complete by.
        /// </summary>
        /// <value>The complete by.</value>
        public DateTime? CompleteBy { get; set; }

        /// <summary>
        /// Gets or sets the identifier task status.
        /// </summary>
        /// <value>The identifier task status.</value>
        public int? IdTaskStatus { get; set; }

        /// <summary>
        /// Gets or sets the task status.
        /// </summary>
        /// <value>The task status.</value>
        public string TaskStatus { get; set; }

        /// <summary>
        /// Gets or sets the general notes.
        /// </summary>
        /// <value>The general notes.</value>
        public string GeneralNotes { get; set; }

        public string NotesDateStamp { get; set; }

        public string NotesTimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the identifier job application.
        /// </summary>
        /// <value>The identifier job application.</value>
       // public int? IdJobApplication { get; set; }

        /// <summary>
        /// Gets or sets the job application.
        /// </summary>
        /// <value>The job application.</value>
        public string JobApplication { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier work permit.
        /// </summary>
        /// <value>The type of the identifier work permit.</value>
        //public IEnumerable<WorkPermitTypeDTO> IdWorkPermitType { get; set; }

        /// <summary>
        /// Gets or sets the type of the work permit.
        /// </summary>
        /// <value>The type of the work permit.</value>
        public string WorkPermitType { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdJob { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP.
        /// </summary>
        /// <value>The identifier RFP.</value>
        public int? IdRfp { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int? IdContact { get; set; }

        /// <summary>
        /// Gets or sets the identifier company.
        /// </summary>
        /// <value>The identifier company.</value>
        public int? IdCompany { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        //public DateTime? LastModifiedDate { get; set; }

        //public DateTime? CreatedDate { get; set; }

        //public string CreatedByEmployee { get; set; }
        /// <summary>
        /// Gets or sets the closed date.
        /// </summary>
        /// <value>The closed date.</value>
        public DateTime ClosedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        //public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        //public string LastModified { get; set; }

        /// <summary>
        /// Gets or sets the badge class.
        /// </summary>
        /// <value>The badge class.</value>
        public string BadgeClass { get; set; }

        /// <summary>
        /// Gets or sets the task for.
        /// </summary>
        /// <value>The task for.</value>
        public object TaskFor { get; set; }

        /// <summary>
        /// Gets or sets the examiner.
        /// </summary>
        /// <value>The examiner.</value>
        //public string Examiner { get; set; }

        /// <summary>
        /// Gets or sets the identifier examiner.
        /// </summary>
        /// <value>The identifier examiner.</value>
        //public int? IdExaminer { get; set; }

        /// <summary>
        /// Gets or sets the identifier job status.
        /// </summary>
        /// <value>The identifier job status.</value>
        public int? IdJobStatus { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        //public IEnumerable<TaskNoteDetails> Notes { get; set; }

        /// <summary>
        /// Gets or sets the job number.
        /// </summary>
        /// <value>The job number.</value>
        public string JobNumber { get; set; }

        /// <summary>
        /// Gets or sets the RFP number.
        /// </summary>
        /// <value>The RFP number.</value>
        public string RfpNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        /// <value>The name of the contact.</value>
        //public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>The name of the company.</value>
        //public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the task number.
        /// </summary>
        /// <value>The task number.</value>
        public string TaskNumber { get; set; }

        /// <summary>
        /// Gets or sets the type of the job billing.
        /// </summary>
        /// <value>The type of the job billing.</value>
        //public JobBillingType? JobBillingType { get; set; }

        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        //public int? IdJobFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP job.
        /// </summary>
        /// <value>The type of the identifier RFP job.</value>
        //public int? IdRfpJobType { get; set; }

        /// <summary>
        /// Gets or sets the service quantity.
        /// </summary>
        /// <value>The service quantity.</value>
        //public double? ServiceQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is scope removed.
        /// </summary>
        /// <value><c>true</c> if this instance is scope removed; otherwise, <c>false</c>.</value>
        //public bool IsScopeRemoved { get; set; }

        //public string TaskDuration { get; set; }
        //public int? IdJobViolation { get; set; }
        //public string SummonsNumber { get; set; }
        //public string JobFeeSchedule { get; set; }

        //public RfpCostType CostType { get; set; }
        //public string RfpJobType { get; set; }
        //public int? IdJobType { get; set; }
        public string SpecialPlace { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Borough { get; set; }
        public string ZipCode { get; set; }
        public string ProgressNote { get; set; }
        public string AssignedToLastName { get; set; }
        public string AssignedByLastName { get; set; }
        //public string CostTypeName { get; set; }

        //public virtual ICollection<TaskDocumentDetail> TaskDocuments { get; set; }

        //public virtual ICollection<TaskJobDocumentDetail> TaskJobDocuments { get; set; }

        //public virtual ICollection<TaskJobTransmittalDetail> TaskJobTransmittals { get; set; }
        //public string JobContact { get; set; }
        //public string JobCompany { get; set; }
        //public string RFPContact { get; set; }
        //public string RFPCompany { get; set; }

        //public int? IdMilestone { get; set; }

        public string JobApplicationType { get; set; }
        public bool IsGeneric { get; set; }

    }
}