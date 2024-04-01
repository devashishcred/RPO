// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-21-2018
// ***********************************************************************
// <copyright file="AdvancedSearchParameters.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Advanced Search Parameters.</summary>
// ***********************************************************************


namespace Rpo.ApiServices.Api.Controllers.Jobs
{
    using System;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.DataTable;

    /// <summary>
    /// Class Advanced Search Parameters.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
    public class AdvancedSearchParameters : DataTableParameters
    {
        /// <summary>
        /// Gets or sets a value indicating whether [only my jobs].
        /// </summary>
        /// <value><c>null</c> if [only my jobs] contains no value, <c>true</c> if [only my jobs]; otherwise, <c>false</c>.</value>
        public bool? OnlyMyJobs { get; set; }

        /// <summary>
        /// Gets or sets the job number.
        /// </summary>
        /// <value>The job number.</value>
        public string JobNumber { get; set; }

        /// <summary>
        /// Gets or sets the borough.
        /// </summary>
        /// <value>The borough.</value>
        public int? Borough { get; set; }

        /// <summary>
        /// Gets or sets the house number.
        /// </summary>
        /// <value>The house number.</value>
        public string HouseNumber { get; set; }

        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        /// <value>The street.</value>
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the name of the special place.
        /// </summary>
        /// <value>The name of the special place.</value>
        public string SpecialPlaceName { get; set; }

        /// <summary>
        /// Gets or sets the floor.
        /// </summary>
        /// <value>The floor.</value>
        public string Floor { get; set; }

        /// <summary>
        /// Gets or sets the apt.
        /// </summary>
        /// <value>The apt.</value>
        public string Apt { get; set; }

        /// <summary>
        /// Gets or sets the job start.
        /// </summary>
        /// <value>The job start.</value>
        public string JobStart { get; set; }

        /// <summary>
        /// Gets or sets the project manager.
        /// </summary>
        /// <value>The project manager.</value>
        public int? ProjectManager { get; set; }

        /// <summary>
        /// Gets or sets the other team member.
        /// </summary>
        /// <value>The other team member.</value>
        public string OtherTeamMember { get; set; }

        /// <summary>
        /// Gets or sets the job status.
        /// </summary>
        /// <value>The job status.</value>
        public JobStatus? JobStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is landmark.
        /// </summary>
        /// <value><c>null</c> if [is landmark] contains no value, <c>true</c> if [is landmark]; otherwise, <c>false</c>.</value>
        public bool? IsLandmark { get; set; }

        /// <summary>
        /// Gets or sets the job start date.
        /// </summary>
        /// <value>The job start date.</value>
        public DateTime? JobStartDate { get; set; }

        /// <summary>
        /// Gets or sets the job end date.
        /// </summary>
        /// <value>The job end date.</value>
        public DateTime? JobEndDate { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public int? Client { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [little e].
        /// </summary>
        /// <value><c>null</c> if [little e] contains no value, <c>true</c> if [little e]; otherwise, <c>false</c>.</value>
        public bool? LittleE { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [holiday embargo].
        /// </summary>
        /// <value><c>null</c> if [holiday embargo] contains no value, <c>true</c> if [holiday embargo]; otherwise, <c>false</c>.</value>
        public bool? HolidayEmbargo { get; set; }

        /// <summary>
        /// Gets or sets the identifier company.
        /// </summary>
        /// <value>The identifier company.</value>
        public int? IdCompany { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int? IdContact { get; set; }

        /// <summary>
        /// Gets or sets the type of the global search.
        /// </summary>
        /// <value>The type of the global search.</value>
        public int? GlobalSearchType { get; set; }

        /// <summary>
        /// Gets or sets the global search text.
        /// </summary>
        /// <value>The global search text.</value>
        public string GlobalSearchText { get; set; }

        /// <summary>
        /// Gets the identifier RFP address.
        /// </summary>
        /// <value>The identifier RFP address.</value>
        public int? IdRfpAddress { get; set; }

        /// <summary>
        /// Gets or sets the identifier job types.
        /// </summary>
        /// <value>The identifier job types.</value>
        public string IdJobTypes { get; set; }


        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

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
    }
}