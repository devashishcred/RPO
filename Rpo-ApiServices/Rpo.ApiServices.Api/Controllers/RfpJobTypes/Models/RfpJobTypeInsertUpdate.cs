

namespace Rpo.ApiServices.Api.Controllers.RfpJobTypes
{
    using System.Collections.Generic;
    using Model.Models;
    using Model.Models.Enums;

    public class RfpJobTypeInsertUpdate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? IdParent { get; set; }

        public int Level { get; set; }

        public string ServiceDescription { get; set; }

        public bool AppendWorkDescription { get; set; }

        public bool CustomServiceDescription { get; set; }

        public double? AdditionalUnitPrice { get; set; }

        public int? DisplayOrder { get; set; }

        public double? Cost { get; set; }
        
        public RfpCostType CostType { get; set; }

        public virtual ICollection<RfpJobTypeCostRangeDTO> RfpJobTypeCostRanges { get; set; }

        public virtual ICollection<RfpJobTypeCumulativeCostDTO> RfpJobTypeCumulativeCosts { get; set; }

        public bool IsCurrentStatusOfFiling { get; set; }

        public bool IsShowScope { get; set; }

        public int? PartOf { get; set; }

        public bool IsActive { get; set; }

    }

    public class RfpJobTypeCostRangeDTO
    {
        public int Id { get; set; }

        public int IdRfpJobType { get; set; }

        public int? MinimumQuantity { get; set; }

        public int? MaximumQuantity { get; set; }

        public double? RangeCost { get; set; }

        //public string FormattedRangeCost { get; set; }
    }

    public class RfpJobTypeCumulativeCostDTO
    {
        public int Id { get; set; }

        public int IdRfpJobType { get; set; }

        public int Qty { get; set; }

        public double? CumulativeCost { get; set; }

        //public string FormattedCumulativeCost { get; set; }
    }
}