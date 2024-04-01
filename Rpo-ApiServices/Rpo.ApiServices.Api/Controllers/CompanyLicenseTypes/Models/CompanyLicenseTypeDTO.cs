using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CompanyLicenseTypes
{
    public class CompanyLicenseTypeDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public int? CreatedBy { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public string LastModifiedByEmployeeName { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}