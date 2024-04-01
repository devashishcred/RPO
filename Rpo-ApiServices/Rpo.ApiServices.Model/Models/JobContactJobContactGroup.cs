
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class JobContactJobContactGroup
    {
        [Key]
        public int Id { get; set; }
        
        public int IdJobContact { get; set; }
        
        [ForeignKey("IdJobContact")]
        public virtual JobContact JobContact { get; set; }

        public int IdJobContactGroup { get; set; }

        [ForeignKey("IdJobContactGroup")]
        public virtual JobContactGroup JobContactGroup { get; set; }
    }
}