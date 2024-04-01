
namespace Rpo.ApiServices.Api.Controllers.ChecklistItems
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Api.Controllers.TaskNotes;
    using Model.Models.Enums;
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;
    public class ChecklistItemDTO
    {


        public int Id { get; set; }

        public string Name { get; set; }

        public int IdCheckListGroup { get; set; }
        public string CheckListGroupName { get; set; }
        public string IdJobApplicationTypes { get; set; }    
        public string IdJobWorkTypes { get; set; }
        public string JobWorkTypewithapplication { get; set; }
        public string IdReferenceDocuments { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsUserfillable { get; set; }
        public string ReferenceNote { get; set; }
        public string ExternalReferenceLink { get; set; }     
        public CheckListGroup CheckListGroups { get; set; }
        public IEnumerable<JobApplicationType> JobApplicationTypes { get; set; }
        public IEnumerable<JobWorkType> JobWorkTypes { get; set; }
        public IEnumerable<ReferenceDocument> ReferenceDocuments { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }


        public DateTime? LastModifiedDate { get; set; }
        public int? IdCreateFormDocument { get; set; }
        public int? IdUploadFormDocument { get; set; }
        public string[] ChecklistAddressPropertyMapings { get; set; }
        public int? DisplayOrderPlumbingInspection { get; set; }
        
    }
}
