// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-21-2018
// ***********************************************************************
// <copyright file="Job.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Job.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the job number.
        /// </summary>
        /// <value>The job number.</value>
        public string JobNumber { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP address.
        /// </summary>
        /// <value>The identifier RFP address.</value>
        public int IdRfpAddress { get; set; }

        /// <summary>
        /// Gets or sets the RFP address.
        /// </summary>
        /// <value>The RFP address.</value>
        [ForeignKey("IdRfpAddress")]
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
        [ForeignKey("IdRfp")]
        public virtual Rfp Rfp { get; set; }

        /// <summary>
        /// Gets or sets the identifier borough.
        /// </summary>
        /// <value>The identifier borough.</value>
        public int IdBorough { get; set; }

        /// <summary>
        /// Gets or sets the borough.
        /// </summary>
        /// <value>The borough.</value>
        [ForeignKey("IdBorough")]
        public virtual Borough Borough { get; set; }

        /// <summary>
        /// Gets or sets the house number.
        /// </summary>
        /// <value>The house number.</value>
        [StringLength(50)]
        public string HouseNumber { get; set; }

        /// <summary>
        /// Gets or sets the street number.
        /// </summary>
        /// <value>The street number.</value>
        [StringLength(50)]
        public string StreetNumber { get; set; }

        /// <summary>
        /// Gets or sets the floor number.
        /// </summary>
        /// <value>The floor number.</value>
        [StringLength(50)]
        public string FloorNumber { get; set; }

        /// <summary>
        /// Gets or sets the apartment.
        /// </summary>
        /// <value>The apartment.</value>
        [StringLength(50)]
        public string Apartment { get; set; }

        /// <summary>
        /// Gets or sets the special place.
        /// </summary>
        /// <value>The special place.</value>
        [StringLength(50)]
        public string SpecialPlace { get; set; }

        /// <summary>
        /// Gets or sets the block.
        /// </summary>
        /// <value>The block.</value>
        [StringLength(50)]
        public string Block { get; set; }

        /// <summary>
        /// Gets or sets the lot.
        /// </summary>
        /// <value>The lot.</value>
        [StringLength(50)]
        public string Lot { get; set; }

        /// <summary>
        /// Gets or sets the dot project team.
        /// </summary>
        /// <value>The dot project team.</value>
        public string DOTProjectTeam { get; set; }

        public string DOBProjectTeam { get; set; }

        /// <summary>
        /// Gets or sets the violation project team.
        /// </summary>
        /// <value>The violation project team.</value>
        public string ViolationProjectTeam { get; set; }

        /// <summary>
        /// Gets or sets the dep project team.
        /// </summary>
        /// <value>The dep project team.</value>
        public string DEPProjectTeam { get; set; }

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
        [ForeignKey("IdCompany")]
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
        [ForeignKey("IdJobContactType")]
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
        [ForeignKey("IdContact")]
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
        [ForeignKey("IdProjectManager")]
        public virtual Employee ProjectManager { get; set; }

        ///// <summary>
        ///// Gets or sets the identifier project coordinator.
        ///// </summary>
        ///// <value>The identifier project coordinator.</value>
        //public int? IdProjectCoordinator { get; set; }
        ///// <summary>
        ///// Gets or sets the project coordinator.
        ///// </summary>
        ///// <value>The project coordinator.</value>
        //[ForeignKey("IdProjectCoordinator")]
        //public virtual Employee ProjectCoordinator { get; set; }

        ///// <summary>
        ///// Gets or sets the identifier signoff coordinator.
        ///// </summary>
        ///// <value>The identifier signoff coordinator.</value>
        //public int? IdSignoffCoordinator { get; set; }
        ///// <summary>
        ///// Gets or sets the signoff coordinator.
        ///// </summary>
        ///// <value>The signoff coordinator.</value>
        //[ForeignKey("IdSignoffCoordinator")]
        //public virtual Employee SignoffCoordinator { get; set; }

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
        /// Gets or sets the scope general notes.
        /// </summary>
        /// <value>The scope general notes.</value>
        public string ScopeGeneralNotes { get; set; }

        /// <summary>
        /// Gets or sets the applications.
        /// </summary>
        /// <value>The applications.</value>
        [ForeignKey("IdJob")]
        public virtual ICollection<JobApplication> Applications { get; set; }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        /// <value>The contacts.</value>
        [ForeignKey("IdJob")]
        public virtual ICollection<JobContact> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>The documents.</value>
        [ForeignKey("IdJob")]
        public virtual ICollection<JobDocument> Documents { get; set; }

        /// <summary>
        /// Gets or sets the transmittals.
        /// </summary>
        /// <value>The transmittals.</value>
        [ForeignKey("IdJob")]
        public virtual ICollection<JobTransmittal> Transmittals { get; set; }

        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        [ForeignKey("IdJob")]
        public virtual ICollection<Task> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        /// <value>The scopes.</value>
        [ForeignKey("IdJob")]
        public virtual ICollection<JobScope> Scopes { get; set; }

        /// <summary>
        /// Gets or sets the milestones.
        /// </summary>
        /// <value>The milestones.</value>
        [ForeignKey("IdJob")]
        public virtual ICollection<JobMilestone> Milestones { get; set; }

        /// <summary>
        /// Gets or sets the job fee schedules.
        /// </summary>
        /// <value>The job fee schedules.</value>
        [ForeignKey("IdJob")]
        public virtual ICollection<JobFeeSchedule> JobFeeSchedules { get; set; }

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
        [ForeignKey("IdJob")]
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
        /// Gets or sets the po number.
        /// </summary>
        /// <value>The po number.</value>
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

        public int? IdReferredByCompany { get; set; }

        [ForeignKey("IdReferredByCompany")]
        public virtual Company ReferredByCompany { get; set; }

        public int? IdReferredByContact { get; set; }

        public string JobStatusNotes { get; set; }
        /// <summary>
        /// Gets or sets the onholdcompleteddate date.
        /// </summary>
        /// <value>The onholdcompleteddate.</value>
        public DateTime? OnHoldCompletedDate { get; set; }

        ///// <summary>
        ///// Gets or sets the start date.
        ///// </summary>
        ///// <value>The start date.</value>
        //public  DateTime? OnHoldCompletedDate { get; set; }
       

}
}
