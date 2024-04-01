namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enums;

    /// <summary>
    /// Class Rfp JobType.
    /// </summary>
    public class RfpJobType
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier parent.
        /// </summary>
        /// <value>The identifier parent.</value>
        public int? IdParent { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        [ForeignKey("IdParent")]
        public RfpJobType Parent { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the service description.
        /// </summary>
        /// <value>The service description.</value>
        public string ServiceDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [append work description].
        /// </summary>
        /// <value><c>true</c> if [append work description]; otherwise, <c>false</c>.</value>
        public bool AppendWorkDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [custom service description].
        /// </summary>
        /// <value><c>true</c> if [custom service description]; otherwise, <c>false</c>.</value>
        public bool CustomServiceDescription { get; set; }

        /// <summary>
        /// Gets or sets the additional unit price.
        /// </summary>
        /// <value>The additional unit price.</value>
        public double? AdditionalUnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public double? Cost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is current status of filing.
        /// </summary>
        /// <value><c>true</c> if this instance is current status of filing; otherwise, <c>false</c>.</value>
        public bool IsCurrentStatusOfFiling { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the type of the cost.
        /// </summary>
        /// <value>The type of the cost.</value>
        public RfpCostType CostType { get; set; }

        /// <summary>
        /// Gets or sets the RFP job type cost ranges.
        /// </summary>
        /// <value>The RFP job type cost ranges.</value>
        [ForeignKey("IdRfpJobType")]
        public virtual ICollection<RfpJobTypeCostRange> RfpJobTypeCostRanges { get; set; }

        /// <summary>
        /// Gets or sets the RFP job type cumulative costs.
        /// </summary>
        /// <value>The RFP job type cumulative costs.</value>
        [ForeignKey("IdRfpJobType")]
        public virtual ICollection<RfpJobTypeCumulativeCost> RfpJobTypeCumulativeCosts { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified by employee.
        /// </summary>
        /// <value>The last modified by employee.</value>
        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        public bool IsShowScope { get; set; }

        public int? PartOf  { get; set; }

        public bool IsActive { get; set; }

    }
}
