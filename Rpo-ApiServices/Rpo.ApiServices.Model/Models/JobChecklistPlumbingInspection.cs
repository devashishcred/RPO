using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class JobChecklistPlumbingInspection
    {
        [Key]
        public int Id { get; set; }
        public int IdJobPlumbingInspection { get; set; }
        public int IdJobChecklistGroup { get; set; }

        [ForeignKey("IdJobChecklistGroup")]
        public virtual JobChecklistGroup JobChecklistGroups { get; set; }

        public int IdChecklistItem { get; set; }

        [ForeignKey("IdChecklistItem")]
        public virtual ChecklistItem IdChecklistItems { get; set; }
        public int DisplayOrder { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; }

        public int IdJobPlumbingCheckListFloors { get; set; }

        [ForeignKey("IdJobPlumbingCheckListFloors")]
        public virtual JobPlumbingChecklistFloors JobPlumbingCheckListFloor { get; set; }

        public string WorkOrderNo { get; set; }
        

        public DateTime? NextInspection { get; set; }

        public string Result { get; set; }

        public bool IsRequiredTCO_CO { get; set; }
        
        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }
        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }
    }
}
