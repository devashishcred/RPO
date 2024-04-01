
namespace Rpo.ApiServices.Model.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RfpJobTypeCostRange
    {
        [Key]
        public int Id { get; set; }

        public int IdRfpJobType { get; set; }

        public int? MinimumQuantity { get; set; }

        public int? MaximumQuantity { get; set; }

        public double? Cost { get; set; }
    }
}
