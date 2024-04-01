using Rpo.ApiServices.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        public DateTime? AssignedDate { get; set; }

        public int? IdAssignedTo { get; set; }
        [ForeignKey("IdAssignedTo")]
        public virtual Employee AssignedTo { get; set; }

        public int? IdAssignedBy { get; set; }
        [ForeignKey("IdAssignedBy")]
        public virtual Employee AssignedBy { get; set; }

        public int IdTaskType { get; set; }
        [ForeignKey("IdTaskType")]
        public virtual TaskType TaskType { get; set; }

        public DateTime? CompleteBy { get; set; }

        public int? IdTaskStatus { get; set; }
        [ForeignKey("IdTaskStatus")]
        public virtual TaskStatus TaskStatus { get; set; }

        public string GeneralNotes { get; set; }

        public int? IdJobApplication { get; set; }
        [ForeignKey("IdJobApplication")]
        public virtual JobApplication JobApplication { get; set; }

        public string IdWorkPermitType { get; set; }
        
        [ForeignKey("IdTask")]
        public virtual ICollection<TaskNote> Notes { get; set; }

        [ForeignKey("IdTask")]
        public virtual ICollection<TaskReminder> TaskReminders { get; set; }

        [ForeignKey("IdTask")]
        public virtual ICollection<TaskHistory> TaskHistories { get; set; }

        public int? IdJob { get; set; }
        [ForeignKey("IdJob")]
        public virtual Job Job { get; set; }

        public int? IdRfp { get; set; }
        [ForeignKey("IdRfp")]
        public virtual Rfp Rfp { get; set; }

        public JobBillingType? JobBillingType { get; set; }

        public int? IdJobFeeSchedule { get; set; }

        [ForeignKey("IdJobFeeSchedule")]
        public virtual JobFeeSchedule JobFeeSchedule { get; set; }

        public double? ServiceQuantity { get; set; }

        public int? IdRfpJobType { get; set; }

        [ForeignKey("IdRfpJobType")]
        public virtual RfpJobType RfpJobType { get; set; }

        public int? IdContact { get; set; }
        [ForeignKey("IdContact")]
        public virtual Contact Contact { get; set; }

        public int? IdCompany { get; set; }
        [ForeignKey("IdCompany")]
        public virtual Company Company { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModified { get; set; }

        public int? IdExaminer { get; set; }

        [ForeignKey("IdExaminer")]
        public Contact Examiner { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string TaskNumber { get; set; }

        public string TaskDuration { get; set; }
        public int? IdJobViolation { get; set; }

        [ForeignKey("IdJobViolation")]
        public JobViolation JobViolation { get; set; }

        public int? IdJobType { get; set; }

        [ForeignKey("IdTask")]
        public virtual ICollection<TaskDocument> TaskDocuments { get; set; }

        [ForeignKey("IdTask")]
        public virtual ICollection<TaskJobDocument> TaskJobDocuments { get; set; }

        [ForeignKey("IdTask")]
        public virtual ICollection<JobTransmittal> JobTransmittals { get; set; }

        public int? IdMilestone { get; set; }

        [ForeignKey("IdMilestone")]
        public virtual JobMilestone JobMilestone { get; set; }
        public bool IsGeneric { get; set; }
    }
}