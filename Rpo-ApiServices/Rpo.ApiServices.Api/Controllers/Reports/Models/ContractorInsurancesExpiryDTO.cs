using System;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    internal class ContractorInsurancesExpiryDTO
    {
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Company { get; set; }
        public string IBMNumber { get; set; }
        public int? Id { get; set; }
        public DateTime? InsuranceDisability { get; set; }
        public DateTime? InsuranceGeneralLiability { get; set; }
        public DateTime? DOTInsuranceObstructionBond { get; set; }
        public DateTime? InsuranceWorkCompensation { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public DateTime? TrackingExpiry { get; set; }
        public string TrackingNumber { get; set; }
        public string ZipCode { get; set; }

        public DateTime? DOTInsuranceWorkCompensation { get; set; }
        public DateTime? DOTInsuranceGeneralLiability { get; set; }
        //mb resp
        public string ResponsibilityName { get; set; }
    }
}