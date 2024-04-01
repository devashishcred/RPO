
namespace Rpo.ApiServices.Model.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RfpProposalReview
    {
        [Key]
        public int Id { get; set; }

        public int IdRfp { get; set; }

        public string Content { get; set; }

        public int IdVerbiage { get; set; }

        [ForeignKey("IdVerbiage")]
        public Verbiage Verbiages { get; set; }

        public int? DisplayOrder { get; set; }
    }
}
