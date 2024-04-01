using System;
using Rpo.ApiServices.Api.DataTable;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class AllViolationDataTableParameters : DataTableParameters
    {

        public string IdCompany { get; set; }

        public string IdContact { get; set; }

        public bool? IsFullyResolved { get; set; }

        public string IdJob { get; set; }

        public string SummonsNumber { get; set; }

        public DateTime? CreatedDateFrom { get; set; }

        public DateTime? CreatedDateTo { get; set; }

        public DateTime? HearingDateFrom { get; set; }

        public DateTime? HearingDateTo { get; set; }

        public DateTime? CureDateFrom { get; set; }

        public DateTime? CureDateTo { get; set; }

        public string ViolationStatus { get; set; }

        public string CertificationStatus { get; set; }

        public string HearingResult { get; set; }

        public bool IsOpenCOC { get; set; }
        public string Type_ECB_DOB { get; set; }


    }
}