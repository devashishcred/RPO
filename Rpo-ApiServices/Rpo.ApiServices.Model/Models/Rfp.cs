

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Rfp
    {
        [Key]
        public int Id { get; set; }

        public string RfpNumber { get; set; }

        public int? IdRfpStatus { get; set; }

        [ForeignKey("IdRfpStatus")]
        public RfpStatus RfpStatus { get; set; }

        public int? IdRfpAddress { get; set; }

        [ForeignKey("IdRfpAddress")]
        public virtual RfpAddress RfpAddress { get; set; }

        public int IdBorough { get; set; }

        [ForeignKey("IdBorough")]
        public virtual Borough Borough { get; set; }

        [StringLength(50)]
        public string HouseNumber { get; set; }

        [StringLength(50)]
        public string StreetNumber { get; set; }

        [StringLength(50)]
        public string FloorNumber { get; set; }

        [StringLength(50)]
        public string Apartment { get; set; }

        [StringLength(100)]
        public string SpecialPlace { get; set; }

        [StringLength(50)]
        public string Block { get; set; }

        [StringLength(50)]
        public string Lot { get; set; }

        public bool HasLandMarkStatus { get; set; }

        public bool HasEnvironmentalRestriction { get; set; }

        public bool HasOpenWork { get; set; }

        public int? IdCompany { get; set; }

        [ForeignKey("IdCompany")]
        public virtual Company Company { get; set; }

        public int? IdContact { get; set; }

        [ForeignKey("IdContact")]
        public virtual Contact Contact { get; set; }

        [StringLength(50)]
        public string Address1 { get; set; }

        [StringLength(50)]
        public string Address2 { get; set; }

        [StringLength(14)]
        public string Phone { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        public int? IdReferredByCompany { get; set; }

        [ForeignKey("IdReferredByCompany")]
        public virtual Company ReferredByCompany { get; set; }

        public int? IdReferredByContact { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public int? IdLastModifiedBy { get; set; }

        [ForeignKey("IdLastModifiedBy")]
        public Employee LastModifiedBy { get; set; }

        public DateTime? StatusChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? IdCreatedBy { get; set; }

        [ForeignKey("IdCreatedBy")]
        public Employee CreatedBy { get; set; }

        public int? GoNextStep { get; set; }

        public int LastUpdatedStep { get; set; }

        public int CompletedStep { get; set; }

        public int? IdRfpScopeReview { get; set; }

        [ForeignKey("IdRfpScopeReview")]
        public virtual RfpScopeReview ScopeReview { get; set; }

        [ForeignKey("IdReferredByContact")]
        public virtual Contact ReferredByContact { get; set; }

        [ForeignKey("IdRfp")]
        public virtual ICollection<RfpFeeSchedule> RfpFeeSchedules { get; set; }

        [ForeignKey("IdRfp")]
        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }

        [ForeignKey("IdRfp")]
        public virtual ICollection<RfpReviewer> RfpReviewers { get; set; }

        [ForeignKey("IdRfp")]
        public virtual ICollection<RfpProposalReview> ProposalReview { get; set; }

        [ForeignKey("IdRfp")]
        public virtual ICollection<Milestone> Milestones { get; set; }

        [ForeignKey("IdRfp")]
        public virtual ICollection<RfpDocument> RfpDocuments { get; set; }

        public string City { get; set; }

        public int? IdState { get; set; }

        [ForeignKey("IdState")]
        public virtual State State { get; set; }

        [MaxLength(10)]
        public string ZipCode { get; set; }

        public double Cost { get; set; }

        public bool? IsSignatureNewPage { get; set; }

        public int? IdClientAddress { get; set; }

        public string ProjectDescription { get; set; }

        public string IdSignature { get; set; }
    }
}
