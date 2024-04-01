// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-06-2018
// ***********************************************************************
// <copyright file="AgentCertificatesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Agent Certificates Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.AgentCertificates
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
    /// Class Agent Certificates Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    public class AgentCertificatesController : ApiController
    {
        /// <summary>
        /// The database.
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the agent certificates.
        /// </summary>
        /// <returns>Agent Certificate List.</returns>
         [Authorize]
        [RpoAuthorize]
        public IQueryable<AgentCertificate> GetAgentCertificates()
        {
            return this.rpoContext.AgentCertificates;
        }

        /// <summary>
        /// Gets the agent certificate.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the agent certificate in detail.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(AgentCertificate))]
        public IHttpActionResult GetAgentCertificate(int id)
        {
            AgentCertificate agentCertificate = this.rpoContext.AgentCertificates.Find(id);
            if (agentCertificate == null)
            {
                return this.NotFound();
            }

            return this.Ok(agentCertificate);
        }

        /// <summary>
        /// Puts the agent certificate.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="agentCertificate">The agent certificate.</param>
        /// <returns>update the detail in database.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAgentCertificate(int id, AgentCertificate agentCertificate)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != agentCertificate.Id)
            {
                return this.BadRequest();
            }

            this.rpoContext.Entry(agentCertificate).State = EntityState.Modified;

            try
            {
                this.rpoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.AgentCertificateExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Posts the agent certificate.
        /// </summary>
        /// <param name="agentCertificate">The agent certificate.</param>
        /// <returns>create a new agent certificate.</returns>
        [ResponseType(typeof(AgentCertificate))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult PostAgentCertificate(AgentCertificate agentCertificate)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.rpoContext.AgentCertificates.Add(agentCertificate);
            this.rpoContext.SaveChanges();

            return this.CreatedAtRoute("DefaultApi", new { id = agentCertificate.Id }, agentCertificate);
        }

        /// <summary>
        /// Deletes the agent certificate.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the agent certificate in the database.</returns>
        [ResponseType(typeof(AgentCertificate))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult DeleteAgentCertificate(int id)
        {
            AgentCertificate agentCertificate = this.rpoContext.AgentCertificates.Find(id);
            if (agentCertificate == null)
            {
                return this.NotFound();
            }

            this.rpoContext.AgentCertificates.Remove(agentCertificate);
            this.rpoContext.SaveChanges();

            return this.Ok(agentCertificate);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.rpoContext.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Agents the certificate exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool AgentCertificateExists(int id)
        {
            return this.rpoContext.AgentCertificates.Count(e => e.Id == id) > 0;
        }
    }
}