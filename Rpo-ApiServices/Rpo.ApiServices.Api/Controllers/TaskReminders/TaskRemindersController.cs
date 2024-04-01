// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-27-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="TaskRemindersController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Task Reminders Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Task Reminders namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.TaskReminders
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model.Models;
    using Filters;

    /// <summary>
    /// Class Task Reminders Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class TaskRemindersController : ApiController
    {
        /// <summary>
        /// Class Task Reminders Advanced Search Parameters.
        /// </summary>
        /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
        public class TaskRemindersAdvancedSearchParameters : DataTableParameters
        {
            /// <summary>
            /// Gets or sets the identifier task.
            /// </summary>
            /// <value>The identifier task.</value>
            public int? IdTask { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether this instance is my reminder.
            /// </summary>
            /// <value><c>true</c> if this instance is my reminder; otherwise, <c>false</c>.</value>
            public bool IsMyReminder { get; set; }
        }

        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the task reminders.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns> Gets the task reminders List.</returns>
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetTaskReminders([FromUri] TaskRemindersAdvancedSearchParameters dataTableParameters/*,int? IdTask,bool IsMyReminder*/)
        {
            IQueryable<TaskReminder> taskReminders = db.TaskReminders;
            var recordsTotal = taskReminders.Count();

            if (dataTableParameters.IdTask != null)
            {
                taskReminders = taskReminders.Where(jtn => jtn.IdTask == (int)dataTableParameters.IdTask);
            }

            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (employee != null)
            {
                if (dataTableParameters.IsMyReminder)
                {
                    taskReminders = taskReminders.Where(jtn => jtn.LastModifiedBy == employee.Id);
                }
            }

            var recordsFiltered = recordsTotal;

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = taskReminders
                .AsEnumerable()
                .Select(x => new TaskReminderDTO
                {
                    Id = x.Id,
                    IdTask = x.IdTask,
                    RemindmeIn = x.RemindmeIn,
                    ReminderDate = TimeZoneInfo.ConvertTimeFromUtc(x.ReminderDate, TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                    LastModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                    LastModifiedBy = x.LastModifiedBy,
                    LastModified = x.LastModified != null ? x.LastModified.FirstName + " " + x.LastModified.LastName : string.Empty
                })
                .AsQueryable()
                .DataTableParameters(dataTableParameters, out recordsFiltered);

            return Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result
            });
        }

        /// <summary>
        /// Gets the task reminder.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the task reminder.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskReminder))]
        public IHttpActionResult GetTaskReminder(int id)
        {
            TaskReminder taskReminder = db.TaskReminders.Find(id);
            if (taskReminder == null)
            {
                return this.NotFound();
            }

            return Ok(taskReminder);
        }

        /// <summary>
        /// Posts the task reminder.
        /// </summary>
        /// <param name="taskReminder">The task reminder.</param>
        /// <returns>create new task reminder.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskReminder))]
        public IHttpActionResult PostTaskReminder(TaskReminder taskReminder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            taskReminder.LastModifiedDate = DateTime.UtcNow;
            var employee = db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (employee != null)
            {
                taskReminder.LastModifiedBy = employee.Id;
            }
            taskReminder.ReminderDate = Convert.ToDateTime(taskReminder.LastModifiedDate).AddDays(taskReminder.RemindmeIn);
            db.TaskReminders.Add(taskReminder);
            db.SaveChanges();

            return this.CreatedAtRoute("DefaultApi", new { id = taskReminder.Id }, taskReminder);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Tasks the reminder exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TaskReminderExists(int id)
        {
            return db.TaskReminders.Count(e => e.Id == id) > 0;
        }
    }
}