using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class JobPlumbingInspectionDueDate
    {
        [Key]
        public int IdJobPlumbingInspectionDueDate { get; set; }

        public int IdJobPlumbingInspection { get; set; }

        [ForeignKey("IdJobPlumbingInspection")]
        public virtual JobPlumbingInspection JobPlumbingInspections { get; set; }
        public DateTime DueDate { get; set; }
        
        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
