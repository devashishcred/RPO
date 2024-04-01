using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CompanyTypes
{
    public class CompanyTypeDTO
    {
        public int Id { get; set; }

        public string ItemName { get; set; }

        public int? CreatedBy { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public string LastModifiedByEmployeeName { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? IdParent { get; set; }
    }
}