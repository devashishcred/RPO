
namespace Rpo.ApiServices.Api.Controllers.Reports
{
    using Rpo.ApiServices.Api.Controllers.Reports.Models;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.Tools;
    using Model.Models;

    [Authorize]
    public class TimenoteUnsyncReportController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();


        /// <summary>
        /// Get the report of Unsynch Timenotes Report with filter and sorting 

        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        public IHttpActionResult GetTimenoteUnsyncReport([FromUri] TimenoteUnsyncDataTableParameters dataTableParameters)
        {
            var jobTimeNotes = rpoContext.JobTimeNotes
              .Include("JobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent")
              .Include("RfpJobType.Parent.Parent.Parent.Parent")
              .Include("Job")              
              .Include("CreatedByEmployee")
              .Include("LastModifiedByEmployee")
              .AsQueryable();

            dataTableParameters.IsQuickbookSynced = false;

            jobTimeNotes = jobTimeNotes.Where(jc => jc.JobBillingType != Model.Models.Enums.JobBillingType.NonBillableItems && jc.IsQuickbookSynced == dataTableParameters.IsQuickbookSynced).AsQueryable();

            var recordsTotal = jobTimeNotes.Count();
            var recordsFiltered = recordsTotal;

            var result = jobTimeNotes
                .AsEnumerable()
                .Select(c => Format(c))
                .AsQueryable()
                .DataTableParameters(dataTableParameters, out recordsFiltered)
                .ToArray();

            return Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result
            });
        }

        private JobTimeNoteUnsynchDTO Format(JobTimeNote jobTimeNote)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            string JobApplicationType = string.Empty;

            if (jobTimeNote.Job != null && jobTimeNote.IdJob != null)
            {
                JobApplicationType = string.Join(",", jobTimeNote.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }

            return new JobTimeNoteUnsynchDTO
            {
                Id = jobTimeNote.Id,
                JobNumber = jobTimeNote.Job != null ? jobTimeNote.Job.JobNumber : string.Empty,
                JobApplicationType = JobApplicationType,
                JobBillingType = jobTimeNote.JobBillingType,
                ProgressNotes = jobTimeNote.ProgressNotes,
                IdJob = jobTimeNote.IdJob,
                TimeHours = jobTimeNote.TimeHours,
                TimeMinutes = jobTimeNote.TimeMinutes,
                QuickbookSyncError = jobTimeNote.QuickbookSyncError,
                QuickbookSyncedDate = jobTimeNote.QuickbookSyncedDate,
                IsQuickbookSynced = jobTimeNote.IsQuickbookSynced,
                IdRfpJobType = jobTimeNote.IdRfpJobType,
                IdJobFeeSchedule = jobTimeNote.IdJobFeeSchedule,
                ServiceItem = jobTimeNote.JobFeeSchedule != null ? Common.GetServiceItemName(jobTimeNote.JobFeeSchedule) : (jobTimeNote.RfpJobType != null ? Common.GetServiceItemName(jobTimeNote.RfpJobType) : string.Empty),
                //  TimeNoteDate = jobTimeNote.TimeNoteDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNote.TimeNoteDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNote.TimeNoteDate,
                TimeNoteDate = jobTimeNote.TimeNoteDate,// != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobTimeNote.TimeNoteDate), DateTimeKind.Utc) : jobTimeNote.TimeNoteDate,
                CreatedBy = jobTimeNote.CreatedBy,
                LastModifiedBy = jobTimeNote.LastModifiedBy,
                CreatedByEmployeeName = jobTimeNote.CreatedByEmployee != null ? jobTimeNote.CreatedByEmployee.FirstName + " " + jobTimeNote.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobTimeNote.LastModifiedByEmployee != null ? jobTimeNote.LastModifiedByEmployee.FirstName + " " + jobTimeNote.LastModifiedByEmployee.LastName : string.Empty,
                CreatedDate = jobTimeNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNote.CreatedDate,
                LastModifiedDate = jobTimeNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobTimeNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTimeNote.LastModifiedDate
            };
        }
    }
}
