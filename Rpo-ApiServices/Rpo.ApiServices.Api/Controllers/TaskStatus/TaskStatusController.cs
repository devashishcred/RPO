// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-24-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="TaskStatusController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Task Status Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.TaskStatuses
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Task Status Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class TaskStatusController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the task status.
        /// </summary>
        /// <returns>Gets the task status list.</returns>
        public IQueryable<TaskStatus> GetTaskStatus()
        {
            return db.TaskStatuses;
        }

        /// <summary>
        /// Gets the task status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the task status.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskStatus))]
        public IHttpActionResult GetTaskStatus(int id)
        {
            TaskStatus TaskStatus = db.TaskStatuses.Find(id);
            if (TaskStatus == null)
            {
                return this.NotFound();
            }

            return Ok(TaskStatus);
        }

        /// <summary>
        /// Puts the task status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="TaskStatus">The task status.</param>
        /// <returns>update the task status.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTaskStatus(int id, TaskStatus TaskStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != TaskStatus.Id)
            {
                return BadRequest();
            }

            db.Entry(TaskStatus).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskStatusExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Posts the task status.
        /// </summary>
        /// <param name="TaskStatus">The task status.</param>
        /// <returns>crate new task status.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskStatus))]
        public IHttpActionResult PostTaskStatus(TaskStatus TaskStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaskStatuses.Add(TaskStatus);
            db.SaveChanges();

            return this.CreatedAtRoute("DefaultApi", new { id = TaskStatus.Id }, TaskStatus);
        }

        /// <summary>
        /// Deletes the task status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the task status.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskStatus))]
        public IHttpActionResult DeleteTaskStatus(int id)
        {
            TaskStatus TaskStatus = db.TaskStatuses.Find(id);
            if (TaskStatus == null)
            {
                return this.NotFound();
            }

            db.TaskStatuses.Remove(TaskStatus);
            db.SaveChanges();

            return Ok(TaskStatus);
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
        /// Tasks the status exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TaskStatusExists(int id)
        {
            return db.TaskStatuses.Count(e => e.Id == id) > 0;
        }
    }
}