using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.ChecklistItems
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;
    public class ChecklistItemDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int IdCheckListGroup { get; set; }
        public string IdJobApplicationTypes { get; set; }
        public string IdJobWorkTypes { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsUserfillable { get; set; }
        public string ReferenceNote { get; set; }
        public string ExternalReferenceLink { get; set; }
        public string InternalReferenceLink { get; set; }

        public string IdReferenceDocument { get; set; }
        public IEnumerable<JobApplicationType> JobApplicationTypes { get; set; }
        public IEnumerable<JobWorkType> JobWorkTypes { get; set; }
        public IEnumerable<ReferenceDocument> ReferenceDocuments { get; set; }

        public List<ChecklistAddressPropertyMappingDTO> ChecklistAddressPropertyMapings { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }


        public DateTime? LastModifiedDate { get; set; }
        public string CreatedByEmployeeName { get; set; }
        public string LastModifiedByEmployeeName { get; set; }
        public int? IdCreateFormDocument { get; set; }
        public int? IdUploadFormDocument { get; set; }


    }
}