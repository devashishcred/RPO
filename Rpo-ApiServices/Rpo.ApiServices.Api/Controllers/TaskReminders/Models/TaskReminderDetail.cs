// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-02-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-02-2018
// ***********************************************************************
// <copyright file="TaskReminderDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Task Reminder Detail.</summary>
// ***********************************************************************

/// <summary>
/// The Task Reminders namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.TaskReminders
{
    using System;
    using Model.Models;

    /// <summary>
    /// Class Task Reminder Detail.
    /// </summary>
    public class TaskReminderDetail
    {
        /// <summary>
        /// Gets or sets the task reminder.
        /// </summary>
        /// <value>The task reminder.</value>
        public TaskReminder TaskReminder { get; set; }

        /// <summary>
        /// Gets or sets the task reminder date.
        /// </summary>
        /// <value>The task reminder date.</value>
        public DateTime TaskReminderDate { get; set; }
        
    }

    /// <summary>
    /// Class Task Detail.
    /// </summary>
    public class TaskDetail
    {
        /// <summary>
        /// Gets or sets the task.
        /// </summary>
        /// <value>The task.</value>
        public Task Task { get; set; }

        /// <summary>
        /// Gets or sets the task due date.
        /// </summary>
        /// <value>The task due date.</value>
        public DateTime TaskDueDate { get; set; }
    }
}