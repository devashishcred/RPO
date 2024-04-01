using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class SeismicDesignCategory
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Description { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        [StringLength(10)]
        public string Code { get; set; }
    }
}
