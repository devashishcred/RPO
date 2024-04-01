namespace Rpo.ApiServices.Model.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MilestoneService
    {
        [Key]
        public int Id { get; set; }

        public int IdMilestone { get; set; }

        [ForeignKey("IdMilestone")]
        public Milestone Milestone { get; set; }

        public int IdRfpFeeSchedule { get; set; }

        [ForeignKey("IdRfpFeeSchedule")]
        public RfpFeeSchedule RfpFeeSchedule { get; set; }
    }
}
