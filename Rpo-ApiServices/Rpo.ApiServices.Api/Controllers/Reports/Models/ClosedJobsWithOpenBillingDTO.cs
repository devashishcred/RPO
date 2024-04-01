using System;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class ClosedJobsWithOpenBillingDTO
    {
        public string Address { get; set; }

        public string Apartment { get; set; }

        //public double? BillingCost { get; set; }

        //public string BillingPointName { get; set; }

        public string Company { get; set; }

        public string Contact { get; set; }

        public string FloorNumber { get; set; }

        //public string FormattedBillingCost { get; set; }

        public int? IdCompany { get; set; }

        public int? IdContact { get; set; }
        public int IdJob { get; internal set; }
        //public string InvoiceNumber { get; set; }

        //public bool IsInvoiced { get; set; }

        public string JobNumber { get; set; }

        //public string PONumber { get; set; }

        public string SpecialPlaceName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string JobApplicationType { get; set; }

        // public string Status { get; set; }
    }
}