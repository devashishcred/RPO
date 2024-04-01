// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-09-2018
// ***********************************************************************
// <copyright file="TaskAdvancedSearchParameters.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Task Advanced Search Parameters.</summary>
// ***********************************************************************

/// <summary>
/// The Tasks namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Tasks
{
    using System;
    using Rpo.ApiServices.Api.DataTable;

    /// <summary>
    /// Class Task Advanced Search Parameters.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
    public class TaskAdvancedSearchParameters : DataTableParameters
    {
        /// <summary>
        /// Gets or sets the identifier borough.
        /// </summary>
        /// <value>The identifier borough.</value>
        public int? IdBorough { get; set; }

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
        /// Gets or sets the type of the identifier task.
        /// </summary>
        /// <value>The type of the identifier task.</value>
        public int? IdTaskType { get; set; }

        /// <summary>
        /// Gets or sets the identifier task status.
        /// </summary>
        /// <value>The identifier task status.</value>
        public int? IdTaskStatus { get; set; }

        ///// <summary>
        ///// Gets or sets the identifier assigned by.
        ///// </summary>
        ///// <value>The identifier assigned by.</value>
        //public int? IdAssignedBy { get; set; }

        ///// <summary>
        ///// Gets or sets the identifier assigned to.
        ///// </summary>
        ///// <value>The identifier assigned to.</value>
        //public int? IdAssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the identifier assigned by.
        /// </summary>
        /// <value>The identifier assigned by.</value>
        public string IdAssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the identifier assigned to.
        /// </summary>
        /// <value>The identifier assigned to.</value>
        public string IdAssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the assigned from date.
        /// </summary>
        /// <value>The assigned from date.</value>
        public DateTime? AssignedFromDate { get; set; }

        /// <summary>
        /// Gets or sets the assigned to date.
        /// </summary>
        /// <value>The assigned to date.</value>
        public DateTime? AssignedToDate { get; set; }

        /// <summary>
        /// Gets or sets the completed from date.
        /// </summary>
        /// <value>The completed from date.</value>
        public DateTime? CompletedFromDate { get; set; }

        /// <summary>
        /// Gets or sets the completed to date.
        /// </summary>
        /// <value>The completed to date.</value>
        public DateTime? CompletedToDate { get; set; }

        /// <summary>
        /// Gets or sets the closed from date.
        /// </summary>
        /// <value>The closed from date.</value>
        public DateTime? ClosedFromDate { get; set; }

        /// <summary>
        /// Gets or sets the closed to date.
        /// </summary>
        /// <value>The closed to date.</value>
        public DateTime? ClosedToDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdJob { get; set; }

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
        /// Gets or sets the identifier RFP.
        /// </summary>
        /// <value>The identifier RFP.</value>
        public int? IdRfp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is job.
        /// </summary>
        /// <value><c>true</c> if this instance is job; otherwise, <c>false</c>.</value>
        public bool IsJob { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is company.
        /// </summary>
        /// <value><c>true</c> if this instance is company; otherwise, <c>false</c>.</value>
        public bool IsCompany { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is contact.
        /// </summary>
        /// <value><c>true</c> if this instance is contact; otherwise, <c>false</c>.</value>
        public bool IsContact { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is RFP.
        /// </summary>
        /// <value><c>true</c> if this instance is RFP; otherwise, <c>false</c>.</value>
        public bool IsRfp { get; set; }

        public bool IsActiveJob { get; set; }

        public bool IsHoldJob { get; set; }
        public bool IsCompletedJob { get; set; }

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

    }
}