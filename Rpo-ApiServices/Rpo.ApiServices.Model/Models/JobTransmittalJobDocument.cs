using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class JobTransmittalJobDocument
    {
        [Key]
        public int Id { get; set; }
        
        public int? IdJobTransmittal { get; set; }

        [ForeignKey("IdJobTransmittal")]
        public JobTransmittal JobTransmittal { get; set; }

        public int? IdJobDocument { get; set; }

        [ForeignKey("IdJobDocument")]
        public JobDocument JobDocument { get; set; }

        [StringLength(500)]
        public string DocumentPath { get; set; }
        
        public int Copies { get; set; } = 1;
    }
}
