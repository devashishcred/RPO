// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-18-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobTimeNoteHistoryController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Time Note History Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Job Time Note Histories namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobTimeNoteHistories
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.Http;
    using Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;

    /// <summary>
    /// Class Job Time Note History Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class JobTimeNoteHistoryController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the job time note history.
        /// </summary>
        /// <param name="idJobFeeSchedule">The identifier job fee schedule.</param>
        /// <returns>Gets the job time note history.</returns>
        [Authorize]
        [RpoAuthorize]
        public IQueryable<JobTimeNoteHistoryDTO> GetJobTimeNoteHistory(int idJobFeeSchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            IQueryable<JobTimeNoteHistoryDTO> taskHistories = rpoContext.JobTimeNoteHistories.Include("Employee").Include("JobTimeNote")
                .Where(x => x.IdJobFeeSchedule == idJobFeeSchedule)
                .AsEnumerable()
                .Select(c => new JobTimeNoteHistoryDTO
                {
                    Id = c.Id,
                    Description = c.Description,
                    IdEmployee = c.IdEmployee,
                    FirstName = c.Employee != null ? c.Employee.FirstName : string.Empty,
                    LastName = c.Employee != null ? c.Employee.LastName : string.Empty,
                    HistoryDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(c.HistoryDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                    IdJobTimeNote = c.IdJobTimeNote,
                    ProgressNotes = c.JobTimeNote != null ? c.JobTimeNote.ProgressNotes : string.Empty,
                    TimeHours = c.TimeHours != null ? c.TimeHours:0,
                    TimeMinutes = c.TimeHours != null ? c.TimeMinutes : 0,
                    TimeNoteDate = c.JobTimeNote.TimeNoteDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(c.JobTimeNote.TimeNoteDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : c.JobTimeNote.TimeNoteDate,
                    IdJobFeeSchedule =c.IdJobFeeSchedule
                }).OrderByDescending(x => x.HistoryDate).AsQueryable();

            return taskHistories;
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