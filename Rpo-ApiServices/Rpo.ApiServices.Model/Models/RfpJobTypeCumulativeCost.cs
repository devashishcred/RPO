
namespace Rpo.ApiServices.Model.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RfpJobTypeCumulativeCost
    {
        [Key]
        public int Id { get; set; }

        public int IdRfpJobType { get; set; }

        public int Quantity { get; set; }

        public double? Cost { get; set; }
    }
}
