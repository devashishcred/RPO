
namespace Rpo.ApiServices.Api.Controllers.RfpServiceGroups
{
    using System;

    public class RfpServiceGroupDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? IdRfpJobType { get; set; }

        public string RfpJobType { get; set; }

        public int? IdRfpSubJobType { get; set; }

        public string RfpSubJobType { get; set; }

        public int? IdRfpSubJobTypeCategory { get; set; }

        public string RfpSubJobTypeCategory { get; set; }
        
        public int? CreatedBy { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public string LastModifiedByEmployeeName { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public int Level { get; internal set; }
        public int? DisplayOrder { get; internal set; }
    }
}