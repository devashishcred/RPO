using System;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class ContactLicenceExpiryDTO
    {
        public string Company { get; set; }

        public string Contact { get; set; }

        public string ContactLicenseType { get; set; }

        public DateTime? ExpirationLicenseDate { get; set; }

        public int? IdCompany { get; set; }
        public int? IdContact { get; set; }
        public int IdContactLicense { get; set; }

        public string JobNumber { get; set; }
    }
}