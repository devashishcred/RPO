namespace Rpo.ApiServices.Model.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProjectDetail
    {
        [Key]
        public int Id { get; set; }

        public string WorkDescription { get; set; }

        public bool ArePlansNotPrepared { get; set; }

        public bool ArePlansCompleted { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDisaproved { get; set; }

        public bool IsPermitted { get; set; }

        [StringLength(9)]
        public string ApprovedJobNumber { get; set; }

        [StringLength(9)]
        public string DisApprovedJobNumber { get; set; }

        [StringLength(9)]
        public string PermittedJobNumber { get; set; }

        public int IdRfp { get; set; }

        [ForeignKey("IdProjectDetail")]
        public virtual ICollection<RfpFeeSchedule> RfpFeeSchedules { get; set; }

        public int? IdRfpJobType { get; set; }

        [ForeignKey("IdRfpJobType")]
        public RfpJobType RfpJobType { get; set; }

        public int? IdRfpSubJobTypeCategory { get; set; }

        [ForeignKey("IdRfpSubJobTypeCategory")]
        public RfpJobType RfpSubJobTypeCategory { get; set; }

        public int? IdRfpSubJobType { get; set; }

        [ForeignKey("IdRfpSubJobType")]
        public RfpJobType RfpSubJobType { get; set; }

        public int DisplayOrder { get; set; }
    }
}
