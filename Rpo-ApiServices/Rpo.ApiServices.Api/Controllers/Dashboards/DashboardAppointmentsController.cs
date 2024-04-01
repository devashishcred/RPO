
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
    public class DashboardAppointmentsController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Get the appointments
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IHttpActionResult. Get the deshboard appointments current weeek </returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetDashboardAppointments([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewReport))
            //{
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                int appointmentTaskType = TaskTypeMaster.Appointments.GetHashCode();

                DateTime todaydate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                DateTime completedFromDate = TimeZoneInfo.ConvertTimeToUtc(todaydate, TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                DateTime completedToDate = completedFromDate.AddDays(7);
                int IdTaskStatus = EnumTaskStatus.Pending.GetHashCode();

                var tasks = rpoContext.Tasks
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
                                            .Include("LastModified").Where(x => x.IdTaskType == appointmentTaskType
                                                            && x.IdTaskStatus == IdTaskStatus
                                                            && (x.IdAssignedBy == employee.Id
                                                            || x.IdAssignedTo == employee.Id)
                                                            && DbFunctions.TruncateTime(x.CompleteBy) >= DbFunctions.TruncateTime(completedFromDate)
                                                            && DbFunctions.TruncateTime(x.CompleteBy) < DbFunctions.TruncateTime(completedToDate)).AsQueryable();


                var recordsTotal = tasks.Count();
                var recordsFiltered = recordsTotal;

                var result = tasks
                                           .AsEnumerable()
                                           .Select(j => FormatUpcomingAppointment(j))
                                           .AsQueryable()
                                           .DataTableParameters(dataTableParameters, out recordsFiltered)
                                           .ToArray();

                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result.OrderBy(x=>x.CompleteBy)
                });
            //}
            //else
            //{
            //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            //}
        }

        private UpcomingAppointmentDTO FormatUpcomingAppointment(Task task)
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

            UpcomingAppointmentDTO upcomingAppointmentDTO = new UpcomingAppointmentDTO();

            upcomingAppointmentDTO.Id = task.Id;
            upcomingAppointmentDTO.JobApplicationType = JobApplicationType;
            upcomingAppointmentDTO.AssignedDate = task.AssignedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.AssignedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.AssignedDate;
            upcomingAppointmentDTO.IdAssignedTo = task.IdAssignedTo;
            upcomingAppointmentDTO.AssignedTo = task.AssignedTo != null ? task.AssignedTo.FirstName + " " + task.AssignedTo.LastName : string.Empty;
            upcomingAppointmentDTO.AssignedToLastName = task.AssignedTo != null ? task.AssignedTo.LastName : string.Empty;
            upcomingAppointmentDTO.IdAssignedBy = task.IdAssignedBy;
            upcomingAppointmentDTO.AssignedBy = task.AssignedBy != null ? task.AssignedBy.FirstName + " " + task.AssignedBy.LastName : string.Empty;
            upcomingAppointmentDTO.AssignedByLastName = task.AssignedBy != null ? task.AssignedBy.LastName : string.Empty;
            upcomingAppointmentDTO.CompleteBy = task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.CompleteBy;
            upcomingAppointmentDTO.ClosedDate = task.ClosedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.ClosedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : DateTime.MinValue;
            upcomingAppointmentDTO.GeneralNotes = task.GeneralNotes;
            upcomingAppointmentDTO.SpecialPlace = task.Job != null ? task.Job.SpecialPlace : string.Empty;
            upcomingAppointmentDTO.HouseNumber = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.HouseNumber : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.HouseNumber : string.Empty);
            upcomingAppointmentDTO.Street = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.Street : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.Street : string.Empty);
            upcomingAppointmentDTO.Borough = task.Job != null && task.Job.RfpAddress != null && task.Job.RfpAddress.Borough != null ? task.Job.RfpAddress.Borough.Description : (task.Rfp != null && task.Rfp.RfpAddress != null && task.Rfp.RfpAddress.Borough != null ? task.Rfp.RfpAddress.Borough.Description : string.Empty);
            upcomingAppointmentDTO.ZipCode = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.ZipCode : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.ZipCode : string.Empty);
            upcomingAppointmentDTO.IdJob = task.IdJob;
            upcomingAppointmentDTO.IdRfp = task.IdRfp;
            upcomingAppointmentDTO.IdContact = task.IdContact;
            upcomingAppointmentDTO.IdCompany = task.IdCompany;
            upcomingAppointmentDTO.IdExaminer = task.IdExaminer;
            upcomingAppointmentDTO.TaskNumber = task.TaskNumber;
            upcomingAppointmentDTO.TaskDuration = task.TaskDuration;
            upcomingAppointmentDTO.Examiner = task.Examiner != null ? task.Examiner.FirstName + " " + task.Examiner.LastName : string.Empty;
            upcomingAppointmentDTO.TaskFor = taskFor;
            upcomingAppointmentDTO.TaskStatus = task.TaskStatus;
            upcomingAppointmentDTO.JobAddress =
                                        upcomingAppointmentDTO.HouseNumber + " " +
                                        (upcomingAppointmentDTO.Street != null ? upcomingAppointmentDTO.Street + ", " : string.Empty)
                                        + (upcomingAppointmentDTO.Borough != null ? upcomingAppointmentDTO.Borough : string.Empty);
            return upcomingAppointmentDTO;
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