using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpo.ApiServices.Model.Models;

namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    public class RfpCreateUpdate
    {
        public int Id { get; set; }

        public string RfpNumber { get; set; }

        public int? IdRfpStatus { get; set; }
        
        public Rpo.ApiServices.Model.Models.RfpStatus RfpStatus { get; set; }

        public int? IdRfpAddress { get; set; }
        
        public virtual RfpAddress RfpAddress { get; set; }

        public int IdBorough { get; set; }
        
        public virtual Borough Borough { get; set; }
        
        public string HouseNumber { get; set; }
        
        public string StreetNumber { get; set; }
        
        public string FloorNumber { get; set; }
        
        public string Apartment { get; set; }
        
        public string SpecialPlace { get; set; }
        
        public string Block { get; set; }
        
        public string Lot { get; set; }

        public bool HasLandMarkStatus { get; set; }

        public bool HasEnvironmentalRestriction { get; set; }

        public bool HasOpenWork { get; set; }

        public int? IdCompany { get; set; }
        
        public virtual Company Company { get; set; }

        public int? IdContact { get; set; }
        
        public virtual Contact Contact { get; set; }

        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        
        public string Phone { get; set; }
        
        public string Email { get; set; }

        public int? IdReferredByCompany { get; set; }
        
        public virtual Company ReferredByCompany { get; set; }

        public int? IdReferredByContact { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public int? IdLastModifiedBy { get; set; }
        
        public Employee LastModifiedBy { get; set; }

        public DateTime? StatusChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? IdCreatedBy { get; set; }

        public Employee CreatedBy { get; set; }

        public int? GoNextStep { get; set; }

        public int LastUpdatedStep { get; set; }

        public int CompletedStep { get; set; }

        public int? IdRfpScopeReview { get; set; }
        
        public virtual RfpScopeReview ScopeReview { get; set; }
        
        public virtual Contact ReferredByContact { get; set; }
        
        public virtual ICollection<RfpFeeSchedule> RfpFeeSchedules { get; set; }
        
        public virtual ICollection<ProjectDetail> ProjectDetails { get; set; }
        
        public virtual ICollection<RfpReviewer> RfpReviewers { get; set; }

        public virtual ICollection<RfpProposalReview> ProposalReview { get; set; }

        public virtual ICollection<Milestone> Milestones { get; set; }

        public virtual ICollection<RfpDocument> RfpDocuments { get; set; }

        public string City { get; set; }

        public int? IdState { get; set; }
        
        public virtual State State { get; set; }

        public string ZipCode { get; set; }

        public double Cost { get; set; }

        public virtual IEnumerable<int> DocumentsToDelete { get; set; }

        public int? IdClientAddress { get; set; }

        public string ProjectDescription { get; set; }
    }
}