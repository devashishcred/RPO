// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-31-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="JobHistoryAdvanceSearch.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job History Advance Search.</summary>
// ***********************************************************************

/// <summary>
/// The Job Histories namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobHistories
{
    using System;

    /// <summary>
    /// Class Job History Advance Search.
    /// </summary>
    public class JobHistoryAdvanceSearch
    {
        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>From date.</value>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Gets or sets to date.
        /// </summary>
        /// <value>To date.</value>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier employee.
        /// </summary>
        /// <value>The identifier employee.</value>
        public int? IdEmployee { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is job.
        /// </summary>
        /// <value><c>true</c> if this instance is job; otherwise, <c>false</c>.</value>
        public bool IsJob { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is applications.
        /// </summary>
        /// <value><c>true</c> if this instance is applications; otherwise, <c>false</c>.</value>
        public bool IsApplications { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is work permits.
        /// </summary>
        /// <value><c>true</c> if this instance is work permits; otherwise, <c>false</c>.</value>
        public bool IsWorkPermits { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is contacts.
        /// </summary>
        /// <value><c>true</c> if this instance is contacts; otherwise, <c>false</c>.</value>
        public bool IsContacts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is documents.
        /// </summary>
        /// <value><c>true</c> if this instance is documents; otherwise, <c>false</c>.</value>
        public bool IsDocuments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is transmittals memo.
        /// </summary>
        /// <value><c>true</c> if this instance is transmittals memo; otherwise, <c>false</c>.</value>
        public bool IsTransmittals_Memo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is scope.
        /// </summary>
        /// <value><c>true</c> if this instance is scope; otherwise, <c>false</c>.</value>
        public bool IsScope { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is milestone.
        /// </summary>
        /// <value><c>true</c> if this instance is milestone; otherwise, <c>false</c>.</value>
        public bool IsMilestone { get; set; }
    }
}