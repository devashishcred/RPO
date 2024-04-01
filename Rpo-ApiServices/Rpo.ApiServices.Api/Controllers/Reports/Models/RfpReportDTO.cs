using System;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class RfpReportDTO
    {
        public string Address { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Apartment { get; set; }

        public string Block { get; set; }

        public string Company { get; set; }

        public double Cost { get; set; }

        public string CreatedByEmployee { get; set; }

        public DateTime CreatedDate { get; set; }
        public string FormattedCost { get; set; }
        public int Id { get; set; }

        public int? IdCompany { get; set; }

        public int? IdContact { get; set; }

        public int? IdRfpStatus { get; set; }

        public string LastModifiedByEmployee { get; set; }

        public DateTime LastModifiedDate { get; set; }
        public int LastUpdatedStep { get; set; }
        public string RfpNumber { get; set; }

        public string RfpStatus { get; set; }

        public string SpecialPlaceName { get; set; }
    }
}