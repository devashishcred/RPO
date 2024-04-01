// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-27-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-27-2018
// ***********************************************************************
// <copyright file="JobDetailsDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Details DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Job Details DTO.
    /// </summary>
    public class JobDetailsDTO
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
        public virtual RfpAddress RfpAddress { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP.
        /// </summary>
        /// <value>The identifier RFP.</value>
        public int? IdRfp { get; set; }

        /// <summary>
        /// Gets or sets the RFP.
        /// </summary>
        /// <value>The RFP.</value>
        public virtual Rfp Rfp { get; set; }

        /// <summary>
        /// Gets or sets the identifier borough.
        /// </summary>
        /// <value>The identifier borough.</value>
        public int? IdBorough { get; set; }

        /// <summary>
        /// Gets or sets the borough.
        /// </summary>
        /// <value>The borough.</value>
        public virtual Borough Borough { get; set; }

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
        /// Gets or sets the job application types.
        /// </summary>
        /// <value>The job application types.</value>
        public virtual ICollection<JobApplicationType> JobApplicationTypes { get; set; }

        /// <summary>
        /// Gets or sets the identifier company.
        /// </summary>
        /// <value>The identifier company.</value>
        public int? IdCompany { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public virtual Company Company { get; set; }

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
        /// Gets or sets the contact.
        /// </summary>
        /// <value>The contact.</value>
        public virtual Contact Contact { get; set; }

        /// <summary>
        /// Gets or sets the identifier project manager.
        /// </summary>
        /// <value>The identifier project manager.</value>
        public int? IdProjectManager { get; set; }

        /// <summary>
        /// Gets or sets the project manager.
        /// </summary>
        /// <value>The project manager.</value>
        public virtual Employee ProjectManager { get; set; }

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
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModiefiedDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public JobStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the scope general notes.
        /// </summary>
        /// <value>The scope general notes.</value>
        public string ScopeGeneralNotes { get; set; }

        /// <summary>
        /// Gets or sets the applications.
        /// </summary>
        /// <value>The applications.</value>
        public virtual ICollection<JobApplication> Applications { get; set; }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        /// <value>The contacts.</value>
        public virtual ICollection<JobContact> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>The documents.</value>
        public virtual ICollection<JobDocument> Documents { get; set; }

        /// <summary>
        /// Gets or sets the transmittals.
        /// </summary>
        /// <value>The transmittals.</value>
        public virtual ICollection<JobTransmittal> Transmittals { get; set; }

        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        public virtual ICollection<Task> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        /// <value>The scopes.</value>
        public virtual ICollection<JobScope> Scopes { get; set; }

        /// <summary>
        /// Gets or sets the milestones.
        /// </summary>
        /// <value>The milestones.</value>
        public virtual ICollection<JobMilestone> Milestones { get; set; }

        /// <summary>
        /// Gets or sets the jobs.
        /// </summary>
        /// <value>The jobs.</value>
        public virtual ICollection<Job> Jobs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has holiday embargo.
        /// </summary>
        /// <value><c>true</c> if this instance has holiday embargo; otherwise, <c>false</c>.</value>
        public bool HasHolidayEmbargo { get; set; }

        /// <summary>
        /// Gets or sets the time notes.
        /// </summary>
        /// <value>The time notes.</value>
        public virtual ICollection<JobTimeNote> TimeNotes { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        public string CreatedByEmployeeName { get; set; }

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
        public string LastModifiedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the dot project team list.
        /// </summary>
        /// <value>The dot project team list.</value>
        public IEnumerable<JobProjectTeam> DOTProjectTeam { get; set; }

        /// <summary>
        /// Gets or sets the violation project team list.
        /// </summary>
        /// <value>The violation project team list.</value>
        public IEnumerable<JobProjectTeam> ViolationProjectTeam { get; set; }

        /// <summary>
        /// Gets or sets the dep project team list.
        /// </summary>
        /// <value>The dep project team list.</value>
        public IEnumerable<JobProjectTeam> DEPProjectTeam { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the job application.
        /// </summary>
        /// <value>The type of the job application.</value>
        public string JobApplicationType { get; set; }

        /// <summary>
        /// Gets or sets the ocmc number.
        /// </summary>
        /// <value>The ocmc number.</value>
        public string OCMCNumber { get;  set; }

        /// <summary>
        /// Gets or sets the street working from.
        /// </summary>
        /// <value>The street working from.</value>
        public string StreetWorkingFrom { get;  set; }

        /// <summary>
        /// Gets or sets the street working on.
        /// </summary>
        /// <value>The street working on.</value>
        public string StreetWorkingOn { get;  set; }

        /// <summary>
        /// Gets or sets the street working to.
        /// </summary>
        /// <value>The street working to.</value>
        public string StreetWorkingTo { get;  set; }

        /// <summary>
        /// Gets or sets the dob project team.
        /// </summary>
        /// <value>The dob project team.</value>
        public IQueryable<JobProjectTeam> DOBProjectTeam { get; set; }

        /// <summary>
        /// Gets or sets the bin number.
        /// </summary>
        /// <value>The bin number.</value>
        public string BinNumber { get; set; }

        public string ProjectDescription { get; set; }
        public string QBJobName { get; set; }

        //ML
        public int? IdReferredByCompany { get; set; }
        
        public int? IdReferredByContact { get; set; }
        public bool IsBSADecision { get; set; }
        public string JobStatusNotes { get; set; }
    }
}