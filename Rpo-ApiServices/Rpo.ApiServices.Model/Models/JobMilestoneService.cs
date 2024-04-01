namespace Rpo.ApiServices.Model.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class JobMilestoneService
    {
        [Key]
        public int Id { get; set; }

        public int IdMilestone { get; set; }

        [ForeignKey("IdMilestone")]
        public JobMilestone Milestone { get; set; }

        public int IdJobFeeSchedule { get; set; }

        [ForeignKey("IdJobFeeSchedule")]
        public JobFeeSchedule JobFeeSchedule { get; set; }
    }
}
