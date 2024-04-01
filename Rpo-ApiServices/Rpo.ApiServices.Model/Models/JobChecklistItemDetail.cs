using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
   public class JobChecklistItemDetail
    {
        [Key]
        public int Id { get; set; }
        public int IdJobChecklistGroup { get; set; }

        // [ForeignKey("IdJobChecklistGroupDetail")]
        // public virtual JobChecklistGroupDetail JobChecklistGroupDetails { get; set; }
        [ForeignKey("IdJobChecklistGroup")]
        public virtual JobChecklistGroup JobChecklistGroups { get; set; }

        public int IdChecklistItem {get; set; }

        [ForeignKey("IdChecklistItem")]
        public virtual ChecklistItem IdChecklistItems { get; set; }
        public int Displayorder { get; set; }
        public int Status { get; set; }
       // public int ResponsibleParty { get; set; }

        public int? PartyResponsible1 { get; set; }
        public string PartyResponsible { get; set; }
        public string ManualPartyResponsible { get; set; }
        public int? IdContact { get; set; }

        [ForeignKey("IdContact")]
        public virtual Contact Contacts { get; set; }

        public int? IdDesignApplicant { get; set; }

        [ForeignKey("IdDesignApplicant")]
        public virtual Contact DesignApplicantContact { get; set; }
        public int? IdInspector { get; set; }

        [ForeignKey("IdInspector")]
        public virtual Contact InspectorContact { get; set; }
        public string Stage { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequiredForTCO { get; set; }
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
