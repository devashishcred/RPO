using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist
{
    public class CompositeChecklistDTO
    {
  
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] ChecklistheaderIds { get; set; }
        public int ParentChecklistheaderId { get; set; }
        public int ParentJobId { get; set; }
        public bool IsCOProject { get; set; }
        public int? CreatedBy { get; set; }      
        public string CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }
        
        public string LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}