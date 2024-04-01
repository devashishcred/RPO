// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-20-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-20-2018
// ***********************************************************************
// <copyright file="TaskHistoryDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job History DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Task Histories namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.TaskHistories
{
    using System;

    /// <summary>
    /// Class Job History DTO.
    /// </summary>
    public class TaskHistoryDTO
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
        /// Gets or sets the identifier task.
        /// </summary>
        /// <value>The identifier task.</value>
        public int IdTask { get; set; }

        /// <summary>
        /// Gets or sets the task number.
        /// </summary>
        /// <value>The task number.</value>
        public string TaskNumber { get; set; }

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
    }
}