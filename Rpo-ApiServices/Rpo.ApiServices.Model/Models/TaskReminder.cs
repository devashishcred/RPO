using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
    public class TaskReminder
    {
        [Key]
        public int Id { get; set; }

        public int IdTask { get; set; }

        [ForeignKey("IdTask")]
        public Task Task { get; set; }

        public int RemindmeIn { get; set; }

        public DateTime ReminderDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModified { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
