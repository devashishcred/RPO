// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-31-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="TaskReminderDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The TaskReminders namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.TaskReminders
{
    using System;

    /// <summary>
    /// Class TaskReminderDTO.
    /// </summary>
    public class TaskReminderDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier task.
        /// </summary>
        /// <value>The identifier task.</value>
        public int IdTask { get; set; }

        /// <summary>
        /// Gets or sets the remindme in.
        /// </summary>
        /// <value>The remindme in.</value>
        public int RemindmeIn { get; set; }

        /// <summary>
        /// Gets or sets the reminder date.
        /// </summary>
        /// <value>The reminder date.</value>
        public DateTime ReminderDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public string LastModified { get; set; }
    }
}