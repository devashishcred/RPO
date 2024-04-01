
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class JobFeeSchedule
    {
        [Key]
        public int Id { get; set; }

        public int IdJob { get; set; }

        [ForeignKey("IdJob")]
        public Job Job { get; set; }

        public int? IdRfpWorkType { get; set; }

        [ForeignKey("IdRfpWorkType")]
        public RfpJobType RfpWorkType { get; set; }

        public double? Quantity { get; set; }

        public string Description { get; set; }

        public int? IdRfp { get; set; }

        [ForeignKey("IdRfp")]
        public Rfp Rfp { get; set; }

        public double? QuantityAchieved { get; set; }

        public double? QuantityPending { get; set; }

        public string PONumber { get; set; }

        public string Status { get; set; }

        public bool IsInvoiced { get; set; }

        public double? Cost { get; set; }

        public double? TotalCost { get; set; }

        public DateTime? InvoicedDate { get; set; }

        public string InvoiceNumber { get; set; }

        public int? IdRfpFeeSchedule { get; set; }

        public bool IsRemoved { get; set; } = false;

        public bool IsAdditionalService { get; set; } = false;

        public bool IsMilestoneService { get; set; } = false;

        public DateTime? CompletedDate { get; set; }

        public int? IdParentof { get; set; }

        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified by employee.
        /// </summary>
        /// <value>The modified by employee.</value>
        [ForeignKey("ModifiedBy")]
        public Employee ModifiedByEmployee { get; set; }

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

        public DateTime? LastModified { get; set; }

        public bool IsShow { get; set; } = true;

        public bool FromTask { get; set; } = false;


        public bool IsFromScope { get; set; }

    }
}
