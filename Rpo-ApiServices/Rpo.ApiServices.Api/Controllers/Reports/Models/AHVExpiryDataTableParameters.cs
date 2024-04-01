using Rpo.ApiServices.Api.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.Reports.Models
{
    public class AHVExpiryDataTableParameters : DataTableParameters
    {
        public string IdJob { get; set; }

        public string AHVReferenceNumber { get; set; }
        public string Applicant { get; set; }

        public string IdApplicantCompany { get; set; }

        public string JobCompany { get; set; }

        public string IdJobCompany { get; set; }
        public string Type { get; set; }
        public DateTime? ExpiresFromDate { get; set; }

        public DateTime? ExpiresToDate { get; set; }
    }
}