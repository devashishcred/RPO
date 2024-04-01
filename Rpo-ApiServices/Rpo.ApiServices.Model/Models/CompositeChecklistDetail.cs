using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Rpo.ApiServices.Model.Models
{
    public class CompositeChecklistDetail
    {
        [Key]
        public int Id { get; set; }

        public int IdCompositeChecklist { get; set; }

        [ForeignKey("IdCompositeChecklist")]
        public virtual CompositeChecklist CompositeChecklists { get; set; }

        public int IdJobChecklistHeader { get; set; }
        [ForeignKey("IdJobChecklistHeader")]
        public virtual JobChecklistHeader JobChecklistHeaders { get; set; }
        //public int IdJobChecklistItemDetail { get; set; }
        //[ForeignKey("IdJobChecklistItemDetail")]
        //public virtual JobChecklistItemDetail JobChecklistItemDetails { get; set; }
        public int IdJobChecklistGroup { get; set; }
        [ForeignKey("IdJobChecklistGroup")]
        public virtual JobChecklistGroup JobChecklistGroups { get; set; }
        public bool IsParentCheckList { get; set; }
        
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public int? CompositeOrder { get; set; }

    }
}
