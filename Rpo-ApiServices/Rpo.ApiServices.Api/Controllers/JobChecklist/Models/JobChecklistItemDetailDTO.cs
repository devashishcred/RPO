using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.JobChecklist
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;
    public class JobChecklistItemDetailDTO
    {        
        public int Id { get; set; }
        public int IdJobChecklistGroup { get; set; }
        public IEnumerable<JobChecklistGroup> JobChecklistGroup { get; set; }
        public int IdChecklistItem { get; set; }
        public IEnumerable<ChecklistItem> ChecklistItems { get; set; }     
        public int Displayorder { get; set; }
        public string Stage { get; set; }
        public int? IdDesignApplicant { get; set; }
        public int? IdInspector { get; set; }
        public int Status { get; set; }
        public int PartyResponsible1 { get; set; }
        
        public string PartyResponsible { get; set; }
        
        public int? IdContact { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public string ManualPartyResponsible { get; set; }     
        public bool IsActive { get; set; }
        public bool IsRequiredForTCO { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }
      
        public DateTime? LastModifiedDate { get; set; }
        public string Others { get; set; }
    }
    public class JobChecklistItemStage
    {
        public int Id { get; set; }
        public string Stage { get; set; }
    }
}