// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="EmployeeDocumentsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Employee Documents Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.EmployeeDocuments
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
    /// Class Employee Documents Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class EmployeeDocumentsController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the employee documents.
        /// </summary>
        /// <returns>IQueryable&lt;EmployeeDocument&gt;.</returns>
        public IQueryable<EmployeeDocument> GetEmployeeDocuments()
        {
            return db.EmployeeDocuments;
        }

        /// <summary>
        /// Gets the employee document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(EmployeeDocument))]
        public IHttpActionResult GetEmployeeDocument(int id)
        {
            EmployeeDocument employeeDocument = db.EmployeeDocuments.Find(id);
            if (employeeDocument == null)
            {
                return this.NotFound();
            }

            return Ok(employeeDocument);
        }

        /// <summary>
        /// Puts the employee document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="employeeDocument">The employee document.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeDocument(int id, EmployeeDocument employeeDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeDocument.Id)
            {
                return BadRequest();
            }

            db.Entry(employeeDocument).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeDocumentExists(id))
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
        /// Posts the employee document.
        /// </summary>
        /// <param name="employeeDocument">The employee document.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(EmployeeDocument))]
        public IHttpActionResult PostEmployeeDocument(EmployeeDocument employeeDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeDocuments.Add(employeeDocument);
            db.SaveChanges();

            return this.CreatedAtRoute("DefaultApi", new { id = employeeDocument.Id }, employeeDocument);
        }

        /// <summary>
        /// Deletes the employee document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(EmployeeDocument))]
        public IHttpActionResult DeleteEmployeeDocument(int id)
        {
            EmployeeDocument employeeDocument = db.EmployeeDocuments.Find(id);
            if (employeeDocument == null)
            {
                return this.NotFound();
            }

            db.EmployeeDocuments.Remove(employeeDocument);
            db.SaveChanges();

            return Ok(employeeDocument);
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
        /// Employees the document exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool EmployeeDocumentExists(int id)
        {
            return db.EmployeeDocuments.Count(e => e.Id == id) > 0;
        }
    }
}