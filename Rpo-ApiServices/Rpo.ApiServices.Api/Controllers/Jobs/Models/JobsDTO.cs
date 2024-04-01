// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="JobsDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Jobs DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Jobs
{
    using Rpo.ApiServices.Model.Models.Enums;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class Jobs DTO.
    /// </summary>
    public class JobsDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the job number.
        /// </summary>
        /// <value>The job number.</value>
        public string JobNumber { get; set; }
        public string PoNumber { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP address.
        /// </summary>
        /// <value>The identifier RFP address.</value>
        public int IdRfpAddress { get; set; }

        /// <summary>
        /// Gets or sets the RFP address.
        /// </summary>
        /// <value>The RFP address.</value>
        public string RfpAddress { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP.
        /// </summary>
        /// <value>The identifier RFP.</value>
        public int? IdRfp { get; set; }

        /// <summary>
        /// Gets or sets the identifier borough.
        /// </summary>
        /// <value>The identifier borough.</value>
        public int? IdBorough { get; set; }

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
        /// Gets or sets the type of the identifier job contact.
        /// </summary>
        /// <value>The type of the identifier job contact.</value>
        public int? IdJobContactType { get; set; }

        //public JobContactType JobContactType { get; set; }

        /// <summary>
        /// Gets or sets the job contact type description.
        /// </summary>
        /// <value>The job contact type description.</value>
        public string JobContactTypeDescription { get; set; }

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
        /// Gets or sets the identifier project manager.
        /// </summary>
        /// <value>The identifier project manager.</value>
        public int? IdProjectManager { get; set; }

        /// <summary>
        /// Gets or sets the project manager.
        /// </summary>
        /// <value>The project manager.</value>
        public string ProjectManager { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public virtual DateTime? StartDate { get; set; }
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the last modiefied date.
        /// </summary>
        /// <value>The last modiefied date.</value>
        public DateTime? LastModiefiedDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public JobStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the status description.
        /// </summary>
        /// <value>The status description.</value>
        public string StatusDescription { get; set; }

        /// <summary>
        /// Gets or sets the applications.
        /// </summary>
        /// <value>The applications.</value>
        public IEnumerable<string> Applications { get; set; }

        /// <summary>
        /// Gets or sets the parent status identifier.
        /// </summary>
        /// <value>The parent status identifier.</value>
        public int ParentStatusId { get; set; }

        /// <summary>
        /// Gets or sets the type of the job application.
        /// </summary>
        /// <value>The type of the job application.</value>
        public string JobApplicationType { get; set; }

        /// <summary>
        /// Gets or sets the ocmc number.
        /// </summary>
        /// <value>The ocmc number.</value>
        public string OCMCNumber { get; set; }
        /// <summary>
        /// Gets or sets the street working from.
        /// </summary>
        /// <value>The street working from.</value>
        public string StreetWorkingFrom { get; set; }
        /// <summary>
        /// Gets or sets the street working on.
        /// </summary>
        /// <value>The street working on.</value>
        public string StreetWorkingOn { get; set; }
        /// <summary>
        /// Gets or sets the street working to.
        /// </summary>
        /// <value>The street working to.</value>
        public string StreetWorkingTo { get; set; }

        public string BinNumber { get; set; }

        public string QBJobName { get; set; }

        /// <summary>
        /// Gets or sets the hearing date.
        /// </summary>
        /// <value>The hearing date.</value>
        public DateTime? HearingDate { get; set; }

        /// <summary>
        /// Gets or sets the hearing location.
        /// </summary>
        /// <value>The hearing location.</value>
        public string HearingLocation { get; set; }

        /// <summary>
        /// Gets or sets the hearing result.
        /// </summary>
        /// <value>The hearing result.</value>
        public string HearingResult { get; set; }

        public string Address { get; set; }
        //ML
        /// <summary>
        /// Gets or sets the identifier referred by contact.
        /// </summary>
        /// <value>The identifier referred by contact.</value>
        public int? IdReferredByCompany { get; set; }

        /// <summary>
        /// Gets or sets the referred by company.
        /// </summary>
        /// <value>The referred by company.</value>
        public int? IdReferredByContact { get; set; }

        public string JobStatusNotes { get; set; }
        public string IsAuthorized { get; set; }
        
        public string ProjectHighlights { get; set; }
        public DateTime? HighlightLastModiefiedDate { get; set; }
    }
}