
using Rpo.ApiServices.Model.Models;
using System;

namespace Rpo.ApiServices.Api.Controllers.Companies
{
    public class CompanyLicenseDTODetail
    {
        public int Id { get; set; }

        public virtual int IdCompanyLicenseType { get; set; }

        public CompanyLicenseType CompanyLicenseType { get; set; }

        public string Number { get; set; }

        public DateTime? ExpirationLicenseDate { get; set; }
    }
}
