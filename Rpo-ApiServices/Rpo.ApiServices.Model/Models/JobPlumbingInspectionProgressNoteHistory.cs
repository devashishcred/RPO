using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class JobPlumbingInspectionProgressNoteHistory
    {
        [Key]
        public int Id { get; set; }

        public int IdJobPlumbingInspection { get; set; }

        [ForeignKey("IdJobPlumbingInspection")]
        public virtual JobPlumbingInspection JobPlumbingInspections { get; set; }
        public string Description { get; set; }

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
