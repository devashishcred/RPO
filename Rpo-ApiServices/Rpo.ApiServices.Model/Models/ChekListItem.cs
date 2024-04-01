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

    public class ChecklistItem
    {

        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }       

        public int IdCheckListGroup { get; set; }

        [ForeignKey("IdCheckListGroup")]  
        public virtual CheckListGroup CheckListGroup { get; set; }

        public virtual ICollection<JobApplicationType> JobApplicationTypes { get; set; }

       public string IdJobApplicationTypes { get; set; }

        public string IdJobWorkTypes { get; set; }
        public virtual ICollection<JobWorkType> JobWorkTypes { get; set; }
        //public virtual ICollection<JobApplicationWorkPermitType> JobApplicationWorkPermitTypes { get; set; }

   
        public bool? IsActive { get; set; }
        public bool? IsUserfillable { get; set; }
        public string ReferenceNote { get; set; }
        
        public string ExternalReferenceLink { get; set; }
        public string InternalReferenceLink { get; set; }

        public string IdReferenceDocument { get; set; }
      //  public virtual ICollection<JobDocumentType> JobDocumentTypes { get; set; }
        public virtual ICollection<ReferenceDocument> ReferenceDocuments { get; set; }
        



        public int? CreatedBy { get; set; }


        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }   
                     
        public int? IdCreateFormDocument { get; set; }
        [ForeignKey("IdCreateFormDocument")]
        public virtual DocumentMaster CreateFormDocument { get; set; }
        public int? IdUploadFormDocument { get; set; }
        [ForeignKey("IdUploadFormDocument")]
        public virtual DocumentMaster UploadFormDocument { get; set; }
        public int? DisplayOrderPlumbingInspection { get; set; }

    }
}
