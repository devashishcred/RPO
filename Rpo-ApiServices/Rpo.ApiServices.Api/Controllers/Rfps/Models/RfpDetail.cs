// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-14-2018
// ***********************************************************************
// <copyright file="RfpDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Detail.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    using System;
    using System.Collections.Generic;
    using RfpAddresses;

    /// <summary>
    /// Class Rfp Detail.
    /// </summary>
    public class RfpDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the RFP number.
        /// </summary>
        /// <value>The RFP number.</value>
        public string RfpNumber { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP status.
        /// </summary>
        /// <value>The identifier RFP status.</value>
        public int? IdRfpStatus { get; set; }

        /// <summary>
        /// Gets or sets the RFP status.
        /// </summary>
        /// <value>The RFP status.</value>
        public string RfpStatus { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP address.
        /// </summary>
        /// <value>The identifier RFP address.</value>
        public int? IdRfpAddress { get; set; }

        /// <summary>
        /// Gets or sets the RFP address.
        /// </summary>
        /// <value>The RFP address.</value>
        public RfpAddressDTO RfpAddress { get; set; }

        /// <summary>
        /// Gets or sets the identifier borough.
        /// </summary>
        /// <value>The identifier borough.</value>
        public int IdBorough { get; set; }

        /// <summary>
        /// Gets or sets the borough.
        /// </summary>
        /// <value>The borough.</value>
        public string Borough { get; set; }

        /// <summary>
        /// Gets or sets the house number.
        /// </summary>
        /// <value>The house number.</value>
        public string HouseNumber { get; set; }

        /// <summary>
        /// Gets or sets the street number.
        /// </summary>
        /// <value>The street number.</value>
        public string StreetNumber { get; set; }

        /// <summary>
        /// Gets or sets the floor number.
        /// </summary>
        /// <value>The floor number.</value>
        public string FloorNumber { get; set; }

        /// <summary>
        /// Gets or sets the apartment.
        /// </summary>
        /// <value>The apartment.</value>
        public string Apartment { get; set; }

        /// <summary>
        /// Gets or sets the special place.
        /// </summary>
        /// <value>The special place.</value>
        public string SpecialPlace { get; set; }

        /// <summary>
        /// Gets or sets the block.
        /// </summary>
        /// <value>The block.</value>
        public string Block { get; set; }

        /// <summary>
        /// Gets or sets the lot.
        /// </summary>
        /// <value>The lot.</value>
        public string Lot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has land mark status.
        /// </summary>
        /// <value><c>true</c> if this instance has land mark status; otherwise, <c>false</c>.</value>
        public bool HasLandMarkStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has environmental restriction.
        /// </summary>
        /// <value><c>true</c> if this instance has environmental restriction; otherwise, <c>false</c>.</value>
        public bool HasEnvironmentalRestriction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has open work.
        /// </summary>
        /// <value><c>true</c> if this instance has open work; otherwise, <c>false</c>.</value>
        public bool HasOpenWork { get; set; }

        /// <summary>
        /// Gets or sets the identifier company.
        /// </summary>
        /// <value>The identifier company.</value>
        public int? IdCompany { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int? IdContact { get; set; }

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>The contact.</value>
        public string Contact { get; set; }

        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>The address1.</value>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the address2.
        /// </summary>
        /// <value>The address2.</value>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the identifier referred by company.
        /// </summary>
        /// <value>The identifier referred by company.</value>
        public int? IdReferredByCompany { get; set; }

        /// <summary>
        /// Gets or sets the referred by company.
        /// </summary>
        /// <value>The referred by company.</value>
        public string ReferredByCompany { get; set; }

        /// <summary>
        /// Gets or sets the identifier referred by contact.
        /// </summary>
        /// <value>The identifier referred by contact.</value>
        public int? IdReferredByContact { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier last modified by.
        /// </summary>
        /// <value>The identifier last modified by.</value>
        public int? IdLastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the status changed date.
        /// </summary>
        /// <value>The status changed date.</value>
        public DateTime? StatusChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier created by.
        /// </summary>
        /// <value>The identifier created by.</value>
        public int? IdCreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the go next step.
        /// </summary>
        /// <value>The go next step.</value>
        public int? GoNextStep { get; set; }

        /// <summary>
        /// Gets or sets the last updated step.
        /// </summary>
        /// <value>The last updated step.</value>
        public int LastUpdatedStep { get; set; }

        /// <summary>
        /// Gets or sets the completed step.
        /// </summary>
        /// <value>The completed step.</value>
        public int CompletedStep { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP scope review.
        /// </summary>
        /// <value>The identifier RFP scope review.</value>
        public int? IdRfpScopeReview { get; set; }

        /// <summary>
        /// Gets or sets the scope review.
        /// </summary>
        /// <value>The scope review.</value>
        public RfpScopeReviewDTO ScopeReview { get; set; }

        /// <summary>
        /// Gets or sets the referred by contact.
        /// </summary>
        /// <value>The referred by contact.</value>
        public string ReferredByContact { get; set; }

        /// <summary>
        /// Gets or sets the RFP reviewers.
        /// </summary>
        /// <value>The RFP reviewers.</value>
        public ICollection<RfpReviewerDetail> RfpReviewers { get; set; }

        /// <summary>
        /// Gets or sets the proposal review.
        /// </summary>
        /// <value>The proposal review.</value>
        public ICollection<RfpProposalReviewDetail> ProposalReview { get; set; }

        /// <summary>
        /// Gets or sets the milestones.
        /// </summary>
        /// <value>The milestones.</value>
        public ICollection<MilestoneDetail> Milestones { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state of the identifier.
        /// </summary>
        /// <value>The state of the identifier.</value>
        public int? IdState { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public double Cost { get; set; }

        /// <summary>
        /// Gets or sets the display name of the RFP status.
        /// </summary>
        /// <value>The display name of the RFP status.</value>
        public string RfpStatusDisplayName { get; set; }

        public int? IdClientAddress { get; set; }

        public string ProjectDescription { get; set; }
        public string IdSignature { get; set; }
    }
}