
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class TaskHistory
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(400)]
        public string Description { get; set; }

        public int? IdEmployee { get; set; }
        
        [ForeignKey("IdEmployee")]
        public Employee Employee { get; set; }
        
        public DateTime HistoryDate { get; set; }

        public int IdTask { get; set; }
        
        [ForeignKey("IdTask")]
        public Task Task { get; set; }

        public int? IdJobFeeSchedule { get; set; }

        [ForeignKey("IdJobFeeSchedule")]
        public JobFeeSchedule JobFeeSchedule { get; set; }

        public int? IdMilestone { get; set; }

        [ForeignKey("IdMilestone")]
        public virtual JobMilestone JobMilestone { get; set; }
    }
}
