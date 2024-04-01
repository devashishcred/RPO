using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
  public class JobChecklistGroupDetail
    {
        [Key] 
        public int Id { get; set; }
        public int IdJobCheckListHeader { get; set; }

        [ForeignKey("IdJobCheckListHeader")]
        public virtual JobChecklistHeader JobChecklistHeaders { get; set; }

        public int IdCheckListGroup { get; set; }

        [ForeignKey("IdCheckListGroup")]
        public virtual CheckListGroup ChecklistGroups { get; set; }

        public int? Displayorder { get; set; }
        public bool IsActive { get; set; }

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
