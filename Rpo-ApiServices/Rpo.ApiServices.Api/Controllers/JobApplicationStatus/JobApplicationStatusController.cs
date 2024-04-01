// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-08-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="JobApplicationStatusController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Application Status Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Job Application Status namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobApplicationStatus
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
    using SODA;
    using Model.Models.Enums;    /// <summary>
                                 /// Class Job Application Status Controller.
                                 /// </summary>
                                 /// <seealso cref="System.Web.Http.ApiController" />
    public class JobApplicationStatusController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext db = new RpoContext();

        /// <summary>
        /// Gets the job application status.
        /// </summary>
        /// <returns>IQueryable&lt;ApplicationStatus&gt;.</returns>
        [Authorize]
        [RpoAuthorize]
        public IQueryable<ApplicationStatus> GetJobApplicationStatus()
        {
            return db.ApplicationStatus;
        }

        /// <summary>
        /// Gets the application status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Get the list of application status list</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ApplicationStatus))]
        public IHttpActionResult GetApplicationStatus(int id)
        {
            ApplicationStatus applicationStatus = db.ApplicationStatus.Find(id);
            if (applicationStatus == null)
            {
                return this.NotFound();
            }

            return Ok(applicationStatus);
        }

        /// <summary>
        /// Gets the application status.
        /// </summary>
        /// <returns>IHttpActionResult. Get the list of application status list for dropdown</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/JobApplicationStatus/dropdown")]
        public IHttpActionResult GetJobApplicationStatusDropdown()
        {
            var result = this.db.ApplicationStatus.AsEnumerable().Select(c => new
            {
                Id = c.Id,
                ItemName = c.Name,
                Name = c.Name,
            }).ToArray();

            return this.Ok(result);
        }

        /// <summary>
        /// Puts the application status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="applicationStatus">Update The application status.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutApplicationStatus(int id, ApplicationStatus applicationStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != applicationStatus.Id)
            {
                return BadRequest();
            }

            db.Entry(applicationStatus).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationStatusExists(id))
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
        /// Posts the application status.
        /// </summary>
        /// <param name="applicationStatus">Add new application status.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ApplicationStatus))]
        public IHttpActionResult PostApplicationStatus(ApplicationStatus applicationStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ApplicationStatus.Add(applicationStatus);
            db.SaveChanges();

            return this.CreatedAtRoute("DefaultApi", new { id = applicationStatus.Id }, applicationStatus);
        }

        /// <summary>
        /// Delete applications status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Delete the status of application</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(ApplicationStatus))]
        public IHttpActionResult DeleteapplicationStatus(int id)
        {
            ApplicationStatus applicationStatus = db.ApplicationStatus.Find(id);
            if (applicationStatus == null)
            {
                return this.NotFound();
            }

            db.ApplicationStatus.Remove(applicationStatus);
            db.SaveChanges();

            return Ok(applicationStatus);
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
        /// Applications the status exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ApplicationStatusExists(int id)
        {
            return db.ApplicationStatus.Count(e => e.Id == id) > 0;
        }

        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/JobApplicationStat")]
        public IHttpActionResult GetApplicationStat()
        {
            Tools.ClsApplicationStatus objapp = new Tools.ClsApplicationStatus();

            var client = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
            var dataset = client.GetResource<object>("rvhx-8trz");
            var rows = dataset.GetRows(limit: 1000);

            int[] jobtypearry = { 5, 6, 7, 8, 10, 11, 13, 26 };

            System.Collections.Generic.List<JobApplication> lstJobApplication = this.db.JobApplications.Where(x => x.JobApplicationType.IdParent == 1 && jobtypearry.Contains(x.JobApplicationType.Id) && x.Job.Status == JobStatus.Active).ToList();

            string qry = string.Empty;
            int i =0;
            foreach (var jobApplication in lstJobApplication)
            {
                if (!string.IsNullOrEmpty(jobApplication.ApplicationNumber))
                {
                    if (i == 0)
                    {
                        qry = qry + "(job__='" + jobApplication.ApplicationNumber + "'";
                    }
                    else
                    {
                        qry = qry + " OR " + "job__='" + jobApplication.ApplicationNumber + "'";
                    }
                    i++;
                }             
            }
            qry = qry + ")" + " AND doc__='01' ";

            var soql = new SoqlQuery().Select("job__", "doc__", "job_status", "job_status_descrp", "latest_action_date").Where(qry);

           var results = dataset.Query(soql);
            
            return Ok(results);
        }
    }
}