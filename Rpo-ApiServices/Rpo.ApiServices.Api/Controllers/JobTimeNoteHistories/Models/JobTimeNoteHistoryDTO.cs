// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-18-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobTimeNoteHistoryDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Time Note History DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Job Time Note Histories namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobTimeNoteHistories
{
    using System;

    /// <summary>
    /// Class Job Time Note History DTO.
    /// </summary>
    public class JobTimeNoteHistoryDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the identifier employee.
        /// </summary>
        /// <value>The identifier employee.</value>
        public int? IdEmployee { get; set; }

        /// <summary>
        /// Gets or sets the history date.
        /// </summary>
        /// <value>The history date.</value>
        public DateTime HistoryDate { get; set; }

        /// <summary>
        /// Gets or sets the progress notes.
        /// </summary>
        /// <value>The progress notes.</value>
        public string ProgressNotes { get; set; }

        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int? IdJobFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the identifier job time note.
        /// </summary>
        /// <value>The identifier job time note.</value>
        public int IdJobTimeNote { get;  set; }

      
        /// <summary>
        /// Gets or sets the time hours.
        /// </summary>
        /// <value>The time hours.</value>
        public Int16? TimeHours { get; set; }
        /// <summary>
        /// Gets or sets the time minutes.
        /// </summary>
        /// <value>The time minutes.</value>
        public Int16? TimeMinutes { get; set; }
        /// <summary>
        /// Gets or sets the time note date.
        /// </summary>
        /// <value>The time note date.</value>
        public DateTime TimeNoteDate { get; set; }
    }
}