using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
    public class ReferenceDocument
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(500)]
        [Required]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Keywords { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [MaxLength(500)]
        [Required]
        public string FileName { get; set; }

        [Required]
        public string ContentPath { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
