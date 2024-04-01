
namespace Rpo.ApiServices.Api.Controllers.JobViolationNotes
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Api.Tools;

    public class JobViolationNotesController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();
        
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetJobViolationNotes([FromUri] JobViolationNotesAdvancedSearchParameters dataTableParameters)
        {
            IQueryable<JobViolationNote> jobViolationNotes = rpoContext.JobViolationNotes.Include("CreatedByEmployee").Include("LastModified");
            var recordsTotal = jobViolationNotes.Count();

            if (dataTableParameters.IdJobViolation != null)
            {
                jobViolationNotes = jobViolationNotes.Where(jtn => jtn.IdJobViolation == (int)dataTableParameters.IdJobViolation);
            }

            var recordsFiltered = recordsTotal;

            var result = jobViolationNotes.AsEnumerable().Select(x => Format(x)).OrderByDescending(x => x.LastModifiedDate).AsQueryable()
            .DataTableParameters(dataTableParameters, out recordsFiltered);

            return Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result
            });
        }

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationNoteDetails))]
        public IHttpActionResult GetJobViolationNote(int id)
        {
            JobViolationNote jobViolationNote = rpoContext.JobViolationNotes.Find(id);
            if (jobViolationNote == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(jobViolationNote));
        }

        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(JobViolationNoteDetails))]
        public IHttpActionResult PostJobViolationNote(JobViolationNote jobViolationNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            jobViolationNote.LastModifiedDate = DateTime.UtcNow;
            jobViolationNote.CreatedDate = DateTime.UtcNow;
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (employee != null)
            {
                jobViolationNote.LastModifiedBy = employee.Id;
                jobViolationNote.CreatedBy = employee.Id;
            }
            rpoContext.JobViolationNotes.Add(jobViolationNote);
            rpoContext.SaveChanges();

            JobViolationNote jobViolationNoteResponse = rpoContext.JobViolationNotes.FirstOrDefault(x => x.Id == jobViolationNote.Id);
            return Ok(FormatDetails(jobViolationNoteResponse));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rpoContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RfpProgressNoteExists(int id)
        {
            return rpoContext.RfpProgressNotes.Count(e => e.Id == id) > 0;
        }

        private JobViolationNoteDetails FormatDetails(JobViolationNote jobViolationNote)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobViolationNoteDetails
            {
                Id = jobViolationNote.Id,
                IdJobViolation = jobViolationNote.IdJobViolation,
                Notes = jobViolationNote.Notes,
                CreatedBy = jobViolationNote.CreatedBy,
                LastModifiedBy = jobViolationNote.LastModifiedBy != null ? jobViolationNote.LastModifiedBy : jobViolationNote.CreatedBy,
                CreatedByEmployee = jobViolationNote.CreatedByEmployee != null ? jobViolationNote.CreatedByEmployee.FirstName + " " + jobViolationNote.CreatedByEmployee.LastName : string.Empty,
                LastModified = jobViolationNote.LastModified != null ? jobViolationNote.LastModified.FirstName + " " + jobViolationNote.LastModified.LastName : (jobViolationNote.CreatedByEmployee != null ? jobViolationNote.CreatedByEmployee.FirstName + " " + jobViolationNote.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobViolationNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolationNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolationNote.CreatedDate,
                LastModifiedDate = jobViolationNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolationNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolationNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolationNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolationNote.CreatedDate),
            };
        }

        private JobViolationNoteDTO Format(JobViolationNote jobViolationNote)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new JobViolationNoteDTO
            {
                Id = jobViolationNote.Id,
                IdJobViolation = jobViolationNote.IdJobViolation,
                Notes = jobViolationNote.Notes,
                CreatedBy = jobViolationNote.CreatedBy,
                LastModifiedBy = jobViolationNote.LastModifiedBy != null ? jobViolationNote.LastModifiedBy : jobViolationNote.CreatedBy,
                CreatedByEmployee = jobViolationNote.CreatedByEmployee != null ? jobViolationNote.CreatedByEmployee.FirstName + " " + jobViolationNote.CreatedByEmployee.LastName : string.Empty,
                LastModified = jobViolationNote.LastModified != null ? jobViolationNote.LastModified.FirstName + " " + jobViolationNote.LastModified.LastName : (jobViolationNote.CreatedByEmployee != null ? jobViolationNote.CreatedByEmployee.FirstName + " " + jobViolationNote.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobViolationNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolationNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolationNote.CreatedDate,
                LastModifiedDate = jobViolationNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolationNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolationNote.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolationNote.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolationNote.CreatedDate),
            };
        }
    }
}