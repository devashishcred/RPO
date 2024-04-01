// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-21-2018
// ***********************************************************************
// <copyright file="JobCreateOrUpdateDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Create Or UpdateDTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Jobs
{
    using Rpo.ApiServices.Model.Models.Enums;
    using System;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Job Create Or UpdateDTO.
    /// </summary>
    public class JobCreateOrUpdateDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP address.
        /// </summary>
        /// <value>The identifier RFP address.</value>
        public int IdRfpAddress { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP.
        /// </summary>
        /// <value>The identifier RFP.</value>
        public int? IdRfp { get; set; }

        /// <summary>
        /// Gets or sets the identifier borough.
        /// </summary>
        /// <value>The identifier borough.</value>
        public int IdBorough { get; set; }

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
        /// Gets or sets the type of the identifier job contact.
        /// </summary>
        /// <value>The type of the identifier job contact.</value>
        public int? IdJobContactType { get; set; }

        /// <summary>
        /// Gets or sets the type of the job contact.
        /// </summary>
        /// <value>The type of the job contact.</value>
        public JobContactType JobContactType { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int? IdContact { get; set; }

        /// <summary>
        /// Gets or sets the identifier project manager.
        /// </summary>
        /// <value>The identifier project manager.</value>
        public int? IdProjectManager { get; set; }
        
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public JobStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the job application types.
        /// </summary>
        /// <value>The job application types.</value>
        public int[] JobApplicationTypes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has holiday embargo.
        /// </summary>
        /// <value><c>true</c> if this instance has holiday embargo; otherwise, <c>false</c>.</value>
        public bool HasHolidayEmbargo { get; set; }

        /// <summary>
        /// Gets or sets the dot project team.
        /// </summary>
        /// <value>The dot project team.</value>
        public int[] DOTProjectTeam { get; set; }

        public int[] DOBProjectTeam { get; set; }

        /// <summary>
        /// Gets or sets the violation project team.
        /// </summary>
        /// <value>The violation project team.</value>
        public int[] ViolationProjectTeam { get; set; }

        /// <summary>
        /// Gets or sets the dep project team.
        /// </summary>
        /// <value>The dep project team.</value>
        public int[] DEPProjectTeam { get; set; }

        /// <summary>
        /// Gets or sets the PO number.
        /// </summary>
        /// <value>The PO number.</value>
        public string PONumber { get; set; }

        /// <summary>
        /// Gets or sets the ocmc number.
        /// </summary>
        /// <value>The ocmc number.</value>
        public string OCMCNumber { get; set; }

        /// <summary>
        /// Gets or sets the street working on.
        /// </summary>
        /// <value>The street working on.</value>
        public string StreetWorkingOn { get; set; }

        /// <summary>
        /// Gets or sets the street working from.
        /// </summary>
        /// <value>The street working from.</value>
        public string StreetWorkingFrom { get; set; }

        /// <summary>
        /// Gets or sets the street working to.
        /// </summary>
        /// <value>The street working to.</value>
        public string StreetWorkingTo { get; set; }

        public string ProjectDescription { get; set; }
        public string QBJobName { get; set; }

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
        public string JobStatusNotes { get; set; }
    }
}