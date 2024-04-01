// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="JobApplicationDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Application DTO.</summary>
// ***********************************************************************

/// <summary>
/// The JobApplications namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobApplications
{
    using System;
    using System.Collections.Generic;
    using JobApplicationWorkPermits;    /// <summary>
                                        /// Class Job Application DTO.
                                        /// </summary>
    public class JobApplicationDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }

        /// <summary>
        /// Gets or sets the application number.
        /// </summary>
        /// <value>The application number.</value>
        public string ApplicationNumber { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier job application.
        /// </summary>
        /// <value>The type of the identifier job application.</value>
        public int? IdJobApplicationType { get; set; }

        /// <summary>
        /// Gets or sets the name of the job application type.
        /// </summary>
        /// <value>The name of the job application type.</value>
        public string JobApplicationTypeName { get; set; }

        /// <summary>
        /// Gets or sets the floor working.
        /// </summary>
        /// <value>The floor working.</value>
        public string FloorWorking { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the identifier application status.
        /// </summary>
        /// <value>The identifier application status.</value>
        public int? IdApplicationStatus { get; set; }

        /// <summary>
        /// Gets or sets the application for.
        /// </summary>
        /// <value>The application for.</value>
        public string ApplicationFor { get; set; }

        /// <summary>
        /// Gets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the street working on.
        /// </summary>
        /// <value>The street working on.</value>
        public string StreetWorkingOn { get; set; }

        /// <summary>
        /// Gets or sets the street from.
        /// </summary>
        /// <value>The street from.</value>
        public string StreetFrom { get; set; }

        /// <summary>
        /// Gets or sets the street to.
        /// </summary>
        /// <value>The street to.</value>
        public string StreetTo { get; set; }

        /// <summary>
        /// Gets or sets the application note.
        /// </summary>
        /// <value>The application note.</value>
        public string ApplicationNote { get; set; }

        /// <summary>
        /// Gets or sets the job work permit histories.
        /// </summary>
        /// <value>The job work permit histories.</value>
        public virtual IEnumerable<JobWorkPermitHistoryDTO> JobWorkPermitHistories { get; set; }

        /// <summary>
        /// Gets or sets the total cost.
        /// </summary>
        /// <value>The total cost.</value>
        public double? TotalCost { get; set; }

        /// <summary>
        /// Gets or sets the total days.
        /// </summary>
        /// <value>The total days.</value>
        public int? TotalDays { get; set; }

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
        /// Gets or sets the hydrant cost.
        /// </summary>
        /// <value>The hydrant cost.</value>
        public double? HydrantCost { get; set; }

        /// <summary>
        /// Gets or sets the water cost.
        /// </summary>
        /// <value>The water cost.</value>
        public double? WaterCost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is include holiday.
        /// </summary>
        /// <value><c>null</c> if [is include holiday] contains no value, <c>true</c> if [is include holiday]; otherwise, <c>false</c>.</value>
        public bool? IsIncludeHoliday { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is include saturday.
        /// </summary>
        /// <value><c>null</c> if [is include saturday] contains no value, <c>true</c> if [is include saturday]; otherwise, <c>false</c>.</value>
        public bool? IsIncludeSaturday { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is include sunday.
        /// </summary>
        /// <value><c>null</c> if [is include sunday] contains no value, <c>true</c> if [is include sunday]; otherwise, <c>false</c>.</value>
        public bool? IsIncludeSunday { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the purpose.
        /// </summary>
        /// <value>The purpose.</value>
        public string Purpose { get; set; }

        /// <summary>
        /// Gets or sets the model number.
        /// </summary>
        /// <value>The model number.</value>
        public string ModelNumber { get; set; }
        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        /// <value>The serial number.</value>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the job application status.
        /// </summary>
        /// <value>The job application status.</value>
        public string JobApplicationStatus { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last name of the modified by employee.
        /// </summary>
        /// <value>The last name of the modified by employee.</value>
        public string LastModifiedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the created by employee.
        /// </summary>
        /// <value>The name of the created by employee.</value>
        public string CreatedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        public bool SignOff { get; set; }

        //public int? IdJobWorkType { get; set; }
        public string JobWorkTypeName { get; set; }
        public string IdJobWorkType { get; set; }
        public bool IsExternalApplication { get; set; }
        public bool IsHighRise { get; set; }
    }
}