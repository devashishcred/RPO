
namespace Rpo.ApiServices.Api.Controllers.RfpSubJobTypeCategories
{
    using System;

    public class RfpSubJobTypeCategoryDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? IdRfpJobType { get; set; }

        public string RfpJobType { get; set; }

        public int? CreatedBy { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public string LastModifiedByEmployeeName { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int Level { get; set; }
        public bool IsCurrentStatusOfFiling { get; set; }
    }
}