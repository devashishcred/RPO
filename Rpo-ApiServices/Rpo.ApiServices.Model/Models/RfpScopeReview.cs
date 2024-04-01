

namespace Rpo.ApiServices.Model.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RfpScopeReview
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public string GeneralNotes { get; set; }

        public string ContactsCc { get; set; }
    }
}
