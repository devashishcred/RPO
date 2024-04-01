
namespace Rpo.ApiServices.Model.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RfpFeeSchedule
    {
        [Key]
        public int Id { get; set; }

        public int IdProjectDetail { get; set; }

        [ForeignKey("IdProjectDetail")]
        public ProjectDetail ProjectDetail { get; set; }

        public int IdRfp { get; set; }

        public int? IdRfpWorkTypeCategory { get; set; }

        [ForeignKey("IdRfpWorkTypeCategory")]
        public RfpJobType RfpWorkTypeCategory { get; set; }

        public int? IdRfpWorkType { get; set; }

        [ForeignKey("IdRfpWorkType")]
        public RfpJobType RfpWorkType { get; set; }

        public double? Cost { get; set; }

        public double? Quantity { get; set; }

        public double? TotalCost { get; set; }

        public string Description { get; set; }

        public int? IdOldRfpFeeSchedule { get; set; }

        public int? IdPartof { get; set; }
    }
}
