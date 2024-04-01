// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-05-2018
// ***********************************************************************
// <copyright file="RfpStatusController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


/// <summary>
/// The RfpStatus namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.RfpStatus
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
    /// Class RfpStatusController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class RfpStatusController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the RFP status.
        /// </summary>
        /// <returns>IQueryable&lt;RfpStatus&gt;.</returns>
        [Authorize]
        [RpoAuthorize]
        public IQueryable<RfpStatus> GetRfpStatus()
        {
            return db.RfpStatuses;
        }

        /// <summary>
        /// Gets the RFP status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpStatus))]
        public IHttpActionResult GetRfpStatus(int id)
        {
            RfpStatus rfpStatus = db.RfpStatuses.Find(id);
            if (rfpStatus == null)
            {
                return this.NotFound();
            }

            return Ok(rfpStatus);
        }

        /// <summary>
        /// Puts the RFP status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rfpStatus">The RFP status.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRfpStatus(int id, RfpStatus rfpStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rfpStatus.Id)
            {
                return BadRequest();
            }

            db.Entry(rfpStatus).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RfpStatusExists(id))
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
        /// Posts the RFP status.
        /// </summary>
        /// <param name="rfpStatus">The RFP status.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpStatus))]
        public IHttpActionResult PostRfpStatus(RfpStatus rfpStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RfpStatuses.Add(rfpStatus);
            db.SaveChanges();

            return this.CreatedAtRoute("DefaultApi", new { id = rfpStatus.Id }, rfpStatus);
        }

        /// <summary>
        /// Deletes the RFP status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(RfpStatus))]
        public IHttpActionResult DeleteRfpStatus(int id)
        {
            RfpStatus rfpStatus = db.RfpStatuses.Find(id);
            if (rfpStatus == null)
            {
                return this.NotFound();
            }

            db.RfpStatuses.Remove(rfpStatus);
            db.SaveChanges();

            return Ok(rfpStatus);
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
        /// RFPs the status exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool RfpStatusExists(int id)
        {
            return db.RfpStatuses.Count(e => e.Id == id) > 0;
        }
    }
}