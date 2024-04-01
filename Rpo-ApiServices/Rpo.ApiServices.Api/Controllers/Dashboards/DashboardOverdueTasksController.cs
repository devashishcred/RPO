
namespace Rpo.ApiServices.Api.Controllers.Dashboards
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Api.Jobs;
    using DataTable;
    using Enums;
    using Filters;
    using Model.Models;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;

    [Authorize]
    public class DashboardOverdueTasksController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();


        /// <summary>
        /// Get the OverdueTask
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Get the deshboard OverdueTaskList current datetime </returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        //[Route("api/Dashboards/OverdueTaskList")]
        public IHttpActionResult GetDashboardOverdueTasks([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewReport))
            //{
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                var recordsTotal = rpoContext.Tasks.Count();
                var recordsFiltered = recordsTotal;
                int IdTaskStatus = EnumTaskStatus.Pending.GetHashCode();

                DateTime todaydate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                DateTime completedToDate = TimeZoneInfo.ConvertTimeToUtc(todaydate, TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                IQueryable<Task> tasks = rpoContext.Tasks
                                            .Include("Rfp.RfpAddress.Borough")
                                            .Include("Job.RfpAddress.Borough")
                                            .Include("AssignedTo")
                                            .Include("AssignedBy")
                                            .Include("TaskType")
                                            .Include("TaskStatus")
                                            .Include("JobFeeSchedule")
                                            .Include("Examiner")
                                            .Include("JobApplication.JobApplicationType")
                                            .Include("JobViolation")
                                            .Include("LastModified").Where(x => (x.IdAssignedBy == employee.Id || x.IdAssignedTo == employee.Id)
                                            && DbFunctions.TruncateTime(x.CompleteBy) < DbFunctions.TruncateTime(completedToDate)
                                            && x.IdTaskStatus == IdTaskStatus).AsQueryable();

                IQueryable<DashBoardTaskDTO> tasklists = tasks.AsEnumerable().Select(j => FormatDashBoardTaskDTO(j)).AsQueryable();

                var result = tasklists
                           .AsEnumerable()
                           .Select(j => j)
                           .AsQueryable()
                           .DataTableParameters(dataTableParameters, out recordsFiltered)
                           .ToList();

                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result.OrderBy(x => x.CompleteBy)
                });
            //}
            //else
            //{
            //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            //}
        }

        private DashBoardTaskDTO FormatDashBoardTaskDTO(Task task)
        {
            string taskFor = string.Empty;
            string JobApplicationType = string.Empty;
            if (task.IdJob != null && task.Job != null)
            {
                taskFor = "Job# " + task.Job.JobNumber;
                JobApplicationType = string.Join(",", task.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }
            else if (task.IdRfp != null && task.Rfp != null)
            {
                taskFor = "RFP# " + task.Rfp.RfpNumber;
            }
            else if (task.IdContact != null && task.Contact != null)
            {
                taskFor = "Contact: " + task.Contact.FirstName + " " + task.Contact.LastName;
            }
            else if (task.IdCompany != null && task.Company != null)
            {
                taskFor = "Company: " + task.Company.Name;
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            string badgeClass = string.Empty;

            DateTime? completeBy = task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.CompleteBy;
            DateTime utcDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
            if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() || task.IdTaskStatus == EnumTaskStatus.Unattainable.GetHashCode())
            {
                badgeClass = "grey";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date == utcDate.Date)
            {
                badgeClass = "orange";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date == (utcDate.AddDays(1)).Date)
            {
                badgeClass = "blue";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date >= (utcDate.AddDays(1)).Date && Convert.ToDateTime(completeBy).Date <= (utcDate.AddDays(5)).Date)
            {
                badgeClass = "yellow";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date > (utcDate.AddDays(5)).Date)
            {
                badgeClass = "green";
            }
            else
            {
                badgeClass = "red";
            }

            int dobApplicationType = ApplicationType.DOB.GetHashCode();
            int dotApplicationType = ApplicationType.DOT.GetHashCode();
            int violationApplicationType = ApplicationType.Violation.GetHashCode();

            List<int> workPermitType = task.IdWorkPermitType != null && !string.IsNullOrEmpty(task.IdWorkPermitType) ? (task.IdWorkPermitType.Split(',') != null && task.IdWorkPermitType.Split(',').Any() ? task.IdWorkPermitType.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            DashBoardTaskDTO dashBoardTaskDTO = new DashBoardTaskDTO();

            dashBoardTaskDTO.Id = task.Id;
            dashBoardTaskDTO.JobApplicationType = JobApplicationType;
            dashBoardTaskDTO.AssignedDate = task.AssignedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.AssignedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.AssignedDate;
            dashBoardTaskDTO.IdAssignedTo = task.IdAssignedTo;
            dashBoardTaskDTO.AssignedTo = task.AssignedTo != null ? task.AssignedTo.FirstName + " " + task.AssignedTo.LastName : string.Empty;
            dashBoardTaskDTO.AssignedToLastName = task.AssignedTo != null ? task.AssignedTo.LastName : string.Empty;
            dashBoardTaskDTO.IdAssignedBy = task.IdAssignedBy;
            dashBoardTaskDTO.AssignedBy = task.AssignedBy != null ? task.AssignedBy.FirstName + " " + task.AssignedBy.LastName : string.Empty;
            dashBoardTaskDTO.AssignedByLastName = task.AssignedBy != null ? task.AssignedBy.LastName : string.Empty;
            dashBoardTaskDTO.IdTaskType = task.IdTaskType;
            dashBoardTaskDTO.TaskType = task.TaskType != null ?
            task.TaskType.Name +
            (task.TaskType.IsDisplayContact != null && task.TaskType.IsDisplayContact == true && task.Examiner != null ? " with " + task.Examiner.FirstName + " " + task.Examiner.LastName : string.Empty) +
            (task.TaskType.IsDisplayTime != null && task.TaskType.IsDisplayTime == true ? " at " + (task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToShortTimeString() : string.Empty) : string.Empty)
             : string.Empty;
            dashBoardTaskDTO.CompleteBy = task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.CompleteBy;
            dashBoardTaskDTO.ClosedDate = task.ClosedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.ClosedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : DateTime.MinValue;
            dashBoardTaskDTO.IdTaskStatus = task.IdTaskStatus;
            dashBoardTaskDTO.TaskStatus = task.TaskStatus != null ? task.TaskStatus.Name : string.Empty;
            dashBoardTaskDTO.GeneralNotes = task.GeneralNotes;
            dashBoardTaskDTO.IdJobApplication = task.IdJobApplication;
            dashBoardTaskDTO.SpecialPlace = task.Job != null ? task.Job.SpecialPlace : string.Empty;
            dashBoardTaskDTO.HouseNumber = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.HouseNumber : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.HouseNumber : string.Empty);
            dashBoardTaskDTO.Street = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.Street : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.Street : string.Empty);
            dashBoardTaskDTO.Borough = task.Job != null && task.Job.RfpAddress != null && task.Job.RfpAddress.Borough != null ? task.Job.RfpAddress.Borough.Description : (task.Rfp != null && task.Rfp.RfpAddress != null && task.Rfp.RfpAddress.Borough != null ? task.Rfp.RfpAddress.Borough.Description : string.Empty);
            dashBoardTaskDTO.ZipCode = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.ZipCode : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.ZipCode : string.Empty);
            dashBoardTaskDTO.JobApplication = task.IdJob != null && task.IdJob > 0 ?
            ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType && task.JobApplication.ApplicationNumber != null && task.JobApplication.ApplicationNumber != string.Empty)
            ? "A# : " + task.JobApplication.ApplicationNumber :
            ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
            ? "LOC : " + task.JobApplication.JobApplicationType.Description + "|" + task.JobApplication.StreetWorkingOn + "|" + task.JobApplication.StreetFrom + "|" + task.JobApplication.StreetTo :
            ((task.JobViolation != null && task.JobViolation.SummonsNumber != null && task.JobViolation.SummonsNumber != string.Empty) ? ("V# : " + task.JobViolation.SummonsNumber) : string.Empty))) : string.Empty;
            dashBoardTaskDTO.WorkPermitType =
            task.IdJob != null && task.IdJob > 0 ?
            ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType)
            ? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.JobWorkType.Description))
            : ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
            ? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Where(x => workPermitType.Contains(x.Id)).Select(x => x.PermitNumber)) : string.Empty)) : string.Empty;
            dashBoardTaskDTO.IdJobViolation = task.IdJobViolation;
            dashBoardTaskDTO.SummonsNumber = task.JobViolation != null ? task.JobViolation.SummonsNumber : string.Empty;
            dashBoardTaskDTO.IdJob = task.IdJob;
            dashBoardTaskDTO.IdRfp = task.IdRfp;
            dashBoardTaskDTO.IdContact = task.IdContact;
            dashBoardTaskDTO.IdCompany = task.IdCompany;
            dashBoardTaskDTO.IdExaminer = task.IdExaminer;
            dashBoardTaskDTO.TaskNumber = task.TaskNumber;
            dashBoardTaskDTO.TaskDuration = task.TaskDuration;
            dashBoardTaskDTO.IdJobType = task.IdJobType;
            dashBoardTaskDTO.Examiner = task.Examiner != null ? task.Examiner.FirstName + " " + task.Examiner.LastName : string.Empty;
            dashBoardTaskDTO.LastModifiedDate = task.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.LastModifiedDate;
            dashBoardTaskDTO.LastModifiedBy = task.LastModifiedBy != null ? task.LastModifiedBy : task.CreatedBy;
            dashBoardTaskDTO.LastModified = task.LastModified != null ? task.LastModified.FirstName + " " + task.LastModified.LastName : (task.CreatedByEmployee != null ? task.CreatedByEmployee.FirstName + " " + task.CreatedByEmployee.LastName : string.Empty);
            dashBoardTaskDTO.BadgeClass = badgeClass;
            dashBoardTaskDTO.TaskFor = taskFor;
            dashBoardTaskDTO.ProgressNote = task.Notes != null ? task.Notes.OrderByDescending(x => x.LastModifiedDate).Select(x => x.Notes).FirstOrDefault() : string.Empty;
            dashBoardTaskDTO.IdJobStatus = task.Job != null ? (int)task.Job.Status : 0;
            dashBoardTaskDTO.IsScopeRemoved = task.JobFeeSchedule != null ? task.JobFeeSchedule.IsRemoved : false;

            dashBoardTaskDTO.JobAddress = dashBoardTaskDTO.HouseNumber + " " + (dashBoardTaskDTO.Street != null ? dashBoardTaskDTO.Street + ", " : string.Empty)
                                         + (dashBoardTaskDTO.Borough != null ? dashBoardTaskDTO.Borough + (dashBoardTaskDTO.ZipCode != null ? ", " : string.Empty) : string.Empty)
                                         + (dashBoardTaskDTO.ZipCode != null ? dashBoardTaskDTO.ZipCode : string.Empty) + (dashBoardTaskDTO.SpecialPlace != null ? " | " + dashBoardTaskDTO.SpecialPlace : string.Empty);
            return dashBoardTaskDTO;
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
    }


}