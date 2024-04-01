// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-10-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="JobHistoryController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job History Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Job Histories namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobHistories
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Job History Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobHistoryController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the job history.
        /// </summary>
        /// <returns> Gets the job history List.</returns>
        [Authorize]
        [RpoAuthorize]
        public IQueryable<JobHistory> GetJobHistorys()
        {
            return rpoContext.JobHistories.Include("Employee").Include("Job").OrderByDescending(x => x.HistoryDate);
        }

        /// <summary>
        /// Gets the job history.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="jobHistoryAdvanceSearch">The job history advance search.</param>
        /// <returns>IQueryable&lt;JobHistoryDTO&gt;.</returns>
        [Authorize]
        [RpoAuthorize]
        public IQueryable<JobHistoryDTO> GetJobHistory(int id, [FromUri] JobHistoryAdvanceSearch jobHistoryAdvanceSearch)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            //if (jobHistoryAdvanceSearch.FromDate != null)
            //{
            //    jobHistoryAdvanceSearch.FromDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobHistoryAdvanceSearch.FromDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
            //}

            //if (jobHistoryAdvanceSearch.ToDate != null)
            //{
            //    jobHistoryAdvanceSearch.ToDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobHistoryAdvanceSearch.ToDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
            //}

            IQueryable<JobHistoryDTO> jobHistorie = rpoContext.JobHistories
                .Include("Employee")
                .Include("Job")
                .Where(x => x.IdJob == id
                && (
                    (x.JobHistoryType == JobHistoryType.Job && jobHistoryAdvanceSearch.IsJob) ||
                    (x.JobHistoryType == JobHistoryType.Applications && jobHistoryAdvanceSearch.IsApplications) ||
                    (x.JobHistoryType == JobHistoryType.WorkPermits && jobHistoryAdvanceSearch.IsWorkPermits) ||
                    (x.JobHistoryType == JobHistoryType.Contacts && jobHistoryAdvanceSearch.IsContacts) ||
                    (x.JobHistoryType == JobHistoryType.Documents && jobHistoryAdvanceSearch.IsDocuments) ||
                    (x.JobHistoryType == JobHistoryType.Transmittals_Memo && jobHistoryAdvanceSearch.IsTransmittals_Memo) ||
                    (x.JobHistoryType == JobHistoryType.Scope && jobHistoryAdvanceSearch.IsScope) ||
                    (x.JobHistoryType == JobHistoryType.Milestone && jobHistoryAdvanceSearch.IsMilestone)
                    || (!jobHistoryAdvanceSearch.IsJob && !jobHistoryAdvanceSearch.IsApplications
                    && !jobHistoryAdvanceSearch.IsWorkPermits && !jobHistoryAdvanceSearch.IsContacts
                    && !jobHistoryAdvanceSearch.IsDocuments && !jobHistoryAdvanceSearch.IsTransmittals_Memo
                    && !jobHistoryAdvanceSearch.IsScope && !jobHistoryAdvanceSearch.IsMilestone)
                )
                //&& (DbFunctions.TruncateTime(x.HistoryDate) >= DbFunctions.TruncateTime(jobHistoryAdvanceSearch.FromDate) || jobHistoryAdvanceSearch.FromDate == null)
                //&& (DbFunctions.TruncateTime(x.HistoryDate) <= DbFunctions.TruncateTime(jobHistoryAdvanceSearch.ToDate) || jobHistoryAdvanceSearch.ToDate == null)
                && (x.IdEmployee == jobHistoryAdvanceSearch.IdEmployee || jobHistoryAdvanceSearch.IdEmployee == null))
                .AsEnumerable()
                .Select(c => new JobHistoryDTO
                {
                    Id = c.Id,
                    Description = c.Description,
                    IdEmployee = c.IdEmployee,
                    FirstName = c.Employee != null ? c.Employee.FirstName : string.Empty,
                    LastName = c.Employee != null ? c.Employee.LastName : string.Empty,
                    HistoryDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(c.HistoryDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                    IdJob = c.IdJob
                }).AsQueryable();

            if (jobHistoryAdvanceSearch.FromDate != null)
            {
                jobHistorie = jobHistorie.AsEnumerable().Where(x => x.HistoryDate.Date >= Convert.ToDateTime(jobHistoryAdvanceSearch.FromDate).Date).AsQueryable();
            }

            if (jobHistoryAdvanceSearch.ToDate != null)
            {
                jobHistorie = jobHistorie.AsEnumerable().Where(x => x.HistoryDate.Date <= Convert.ToDateTime(jobHistoryAdvanceSearch.ToDate).Date).AsQueryable();
            }

            return jobHistorie.AsQueryable().OrderByDescending(x => x.HistoryDate);
        }

        /// <summary>
        /// Posts the job time note.
        /// </summary>
        /// <param name="jobHistoryCreateOrUpdate">The job history create or update.</param>
        /// <returns>create job history.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobHistoryDTO))]
        public IHttpActionResult PostJobTimeNote(JobHistoryCreateOrUpdate jobHistoryCreateOrUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            JobHistory jobHistory = new JobHistory();

            if (Request.GetRequestContext().Principal.Identity.IsAuthenticated)
            {
                var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

                if (employee != null)
                {
                    jobHistory.IdEmployee = employee.Id;
                }
            }

            jobHistory.Description = jobHistoryCreateOrUpdate.Description;
            jobHistory.HistoryDate = DateTime.UtcNow;
            jobHistory.JobHistoryType = jobHistoryCreateOrUpdate.JobHistoryType;
            jobHistory.IdJob = jobHistoryCreateOrUpdate.IdJob;

            rpoContext.JobHistories.Add(jobHistory);
            rpoContext.SaveChanges();

            return Ok(jobHistoryCreateOrUpdate);
        }
        
        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rpoContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}