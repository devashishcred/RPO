using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.Reports.Models
{
    public class AHVExpiryReport
    {
        public string IdJob { get; set; }

        public string JobNumber { get; set; }

        public DateTime? CreatedModifiedDate { get; set; }

        public DateTime? SubmittedDate { get; set; }
        public string AHVReferenceNumber { get; set; }

        public DateTime? IssuedDate { get; set; }

        public string Type { get; set; }
        public string JobAddress { get; set; }

        public string ApplicationNo { get; set; }

        public string WorkPermitType { get; set; }

        public string ApplicantCompany { get; set; }

        public DateTime? TrackingExpiryDate { get; set; } 

        public string JobCompany { get; set; }

        public string DocumentDescription { get; set; }

        public int? IdJobCompany { get; set; }

        public int? IdApplicantCompany { get; set; }

        public string JobApplicationType { get; set; }

        public DateTime? NextDate { get; set; }

    }
}