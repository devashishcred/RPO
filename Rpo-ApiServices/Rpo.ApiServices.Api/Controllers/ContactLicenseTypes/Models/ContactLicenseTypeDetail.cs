

namespace Rpo.ApiServices.Api.Controllers.ContactLicenseTypes
{
    using System;
    using Rpo.ApiServices.Model.Models;

    public class ContactLicenseTypeDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? CreatedBy { get; set; }

        //public Employee CreatedByEmployee { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        //public Employee LastModifiedByEmployee { get; set; }

        public string LastModifiedByEmployeeName { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}