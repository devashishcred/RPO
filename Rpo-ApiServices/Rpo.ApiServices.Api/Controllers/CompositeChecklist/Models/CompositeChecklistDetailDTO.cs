using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpo.ApiServices.Model.Models;
namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist.Models
{
    public class CompositeChecklistDetailDTO
    {
        public int Id { get; set; }
        public int IdJob { get; set; }

        public int IdCompositeChecklist { get; set; }
        public string CompositeChecklistName { get; set; }        
        public int IdJobChecklistHeader { get; set; }
        public string ChecklistName { get; set; }

        public virtual JobChecklistHeader JobChecklistHeaders { get; set; }
        public int IdJobChecklistItemDetail { get; set; }
      
        public virtual JobChecklistItemDetail JobChecklistItemDetails { get; set; }
        public bool IsParentCheckList { get; set; }
        public bool IsCOProject { get; set; }
        public int? CreatedBy { get; set; }
      
        public string CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

       
        public string LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}