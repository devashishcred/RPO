
namespace Rpo.ApiServices.Api.Controllers.Reports
{
    using DataTable;
    using Filters;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;

    [Authorize]
    public class ReportApplicationStatusController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();
        /// <summary>
        /// Get the report of Application Status Report with filter and sorting
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        public IHttpActionResult GetReportApplicationStatus([FromUri] ApplicationStatusDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewApplicationStatusReport))
            {

                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                               .Include("JobApplication.JobApplicationType")
                                                               .Include("JobApplication.ApplicationStatus")
                                                               .Include("JobApplication.Job.RfpAddress.Borough")
                                                               .Include("JobApplication.Job.Company")
                                                               .Include("JobApplication.Job.Contact")
                                                               .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                               .AsEnumerable()
                                                               .Select(c => this.FormatApplicationStatusReport(c))
                                                               .AsQueryable();

                var recordsTotal = jobApplicationWorkPermitTypes.Count();
                var recordsFiltered = recordsTotal;
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
                {
                    List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.IdProjectManager ?? 0));
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.IdJob));
                }

                if (dataTableParameters != null && dataTableParameters.FiledOnFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Filed != null && Convert.ToDateTime(t.Filed).Date >= Convert.ToDateTime(dataTableParameters.FiledOnFromDate).Date);
                }

                if (dataTableParameters != null && dataTableParameters.FiledOnToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Filed != null && Convert.ToDateTime(t.Filed).Date <= Convert.ToDateTime(dataTableParameters.FiledOnToDate).Date);
                }

                if (dataTableParameters != null && dataTableParameters.IssuedFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Issued != null && Convert.ToDateTime(t.Issued).Date >= Convert.ToDateTime(dataTableParameters.IssuedFromDate).Date);
                }

                if (dataTableParameters != null && dataTableParameters.IssuedToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Issued != null && Convert.ToDateTime(t.Issued).Date <= Convert.ToDateTime(dataTableParameters.IssuedToDate).Date);
                }

                if (dataTableParameters.SignedOffFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.SignedOff != null && Convert.ToDateTime(t.SignedOff).Date >= Convert.ToDateTime(dataTableParameters.SignedOffFromDate).Date);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.Description))
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.WorkDescription.Contains(dataTableParameters.Description));
                }

                if (dataTableParameters != null && dataTableParameters.SignedOffToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.SignedOff != null && Convert.ToDateTime(t.SignedOff).Date <= Convert.ToDateTime(dataTableParameters.SignedOffToDate).Date);
                }

                if (dataTableParameters != null && dataTableParameters.HasLandMarkStatus != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.HasLandMarkStatus == (dataTableParameters.HasLandMarkStatus ?? false));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.IdCompany ?? 0)));
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.IdContact)));
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationType))
                {
                    List<int> jobContactTeam = dataTableParameters.IdApplicationType != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationType) ? (dataTableParameters.IdApplicationType.Split('-') != null && dataTableParameters.IdApplicationType.Split('-').Any() ? dataTableParameters.IdApplicationType.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.IdJobApplicationType ?? 0)));
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationStatus))
                {
                    List<int> jobContactTeam = dataTableParameters.IdApplicationStatus != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationStatus) ? (dataTableParameters.IdApplicationStatus.Split('-') != null && dataTableParameters.IdApplicationStatus.Split('-').Any() ? dataTableParameters.IdApplicationStatus.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.JobApplicationStatusId ?? 0)));
                }

                if (dataTableParameters.JobStatus != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(j => j.JobStatus == dataTableParameters.JobStatus);
                }

                string direction = string.Empty;
                string orderBy = string.Empty;

                List<ApplicationStatusReportDTO> result = new List<ApplicationStatusReportDTO>();

                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Dir))
                {
                    direction = dataTableParameters.OrderedColumn.Dir;
                }
                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Column))
                {
                    orderBy = dataTableParameters.OrderedColumn.Column;
                }


                if (!string.IsNullOrEmpty(direction) && direction == "desc")
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobAddress);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplicationType);
                    if (orderBy.Trim().ToLower() == "JobWorkTypeCode".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobWorkTypeCode);
                    if (orderBy.Trim().ToLower() == "JobApplicationStatus".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplicationStatus);
                    if (orderBy.Trim().ToLower() == "Filed".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Filed);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Withdrawn".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Withdrawn);
                    if (orderBy.Trim().ToLower() == "SignedOff".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.SignedOff);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobAddress);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplicationType);
                    if (orderBy.Trim().ToLower() == "JobWorkTypeCode".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobWorkTypeCode);
                    if (orderBy.Trim().ToLower() == "JobApplicationStatus".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplicationStatus);
                    if (orderBy.Trim().ToLower() == "Filed".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Filed);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Withdrawn".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Withdrawn);
                    if (orderBy.Trim().ToLower() == "SignedOff".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.SignedOff);
                }


                result = jobApplicationWorkPermitTypes
                    .AsEnumerable()
                    .Select(c => c)
                    .AsQueryable()
                    .ToList();

                return this.Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Get the report of Application Status Report with filter and sorting and export to excel
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>

        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportApplicationStatus/exporttoexcel")]
        public IHttpActionResult PostExportReportApplicationStatusToExcel(ApplicationStatusDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                               .Include("JobApplication.JobApplicationType")
                                                               .Include("JobApplication.ApplicationStatus")
                                                               .Include("JobApplication.Job.RfpAddress.Borough")
                                                               .Include("JobApplication.Job.Company")
                                                               .Include("JobApplication.Job.Contact")
                                                               .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                               .AsEnumerable()
                                                               .Select(c => this.FormatApplicationStatusReport(c))
                                                               .AsQueryable();

                var recordsTotal = jobApplicationWorkPermitTypes.Count();
                var recordsFiltered = recordsTotal;
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                string hearingDate = string.Empty;
                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
                {
                    List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.IdProjectManager ?? 0));

                    string filteredProjectManagers = string.Join(", ", rpoContext.Employees.Where(x => projectManagers.Contains(x.Id)).Select(x => x.Id));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredProjectManagers) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredProjectManagers) ? "Project Manager : " + filteredProjectManagers : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.IdJob));

                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                string filteredFiledOn = string.Empty;
                if (dataTableParameters != null && dataTableParameters.FiledOnFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Filed != null && Convert.ToDateTime(t.Filed).Date >= Convert.ToDateTime(dataTableParameters.FiledOnFromDate).Date);
                    filteredFiledOn = filteredFiledOn + Convert.ToDateTime(dataTableParameters.FiledOnFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.FiledOnToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Filed != null && Convert.ToDateTime(t.Filed).Date <= Convert.ToDateTime(dataTableParameters.FiledOnToDate).Date);
                    filteredFiledOn = filteredFiledOn + " - " + Convert.ToDateTime(dataTableParameters.FiledOnToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredFiledOn) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredFiledOn) ? "Filed Date : " + filteredFiledOn : string.Empty);

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.Description))
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.WorkDescription.Contains(dataTableParameters.Description));

                    string workDescription = dataTableParameters.Description;
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(workDescription) ? " | " : string.Empty) + (!string.IsNullOrEmpty(workDescription) ? "Work Description : " + workDescription : string.Empty);
                }

                string filteredIssued = string.Empty;
                if (dataTableParameters != null && dataTableParameters.IssuedFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Issued != null && Convert.ToDateTime(t.Issued).Date >= Convert.ToDateTime(dataTableParameters.IssuedFromDate).Date);
                    filteredIssued = filteredIssued + Convert.ToDateTime(dataTableParameters.IssuedFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.IssuedToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Issued != null && Convert.ToDateTime(t.Issued).Date <= Convert.ToDateTime(dataTableParameters.IssuedToDate).Date);
                    filteredIssued = filteredIssued + " - " + Convert.ToDateTime(dataTableParameters.IssuedToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredIssued) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredIssued) ? "Issued Date : " + filteredIssued : string.Empty);

                string filteredCompletion = string.Empty;
                if (dataTableParameters.SignedOffFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.SignedOff != null && Convert.ToDateTime(t.SignedOff).Date >= Convert.ToDateTime(dataTableParameters.SignedOffFromDate).Date);
                    filteredCompletion = filteredCompletion + Convert.ToDateTime(dataTableParameters.SignedOffFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.SignedOffToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.SignedOff != null && Convert.ToDateTime(t.SignedOff).Date <= Convert.ToDateTime(dataTableParameters.SignedOffToDate).Date);
                    filteredCompletion = filteredCompletion + " - " + Convert.ToDateTime(dataTableParameters.SignedOffToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompletion) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompletion) ? "Completion Date : " + filteredCompletion : string.Empty);

                if (dataTableParameters != null && dataTableParameters.HasLandMarkStatus != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.HasLandMarkStatus == (dataTableParameters.HasLandMarkStatus ?? false));

                    string hasLandMarkStatus = dataTableParameters.HasLandMarkStatus != null && Convert.ToBoolean(dataTableParameters.HasLandMarkStatus) == true ? "Yes" : "No";
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(hasLandMarkStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(hasLandMarkStatus) ? "LandMark Status : " + hasLandMarkStatus : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.IdContact)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationType))
                {
                    List<int> jobApplicationType = dataTableParameters.IdApplicationType != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationType) ? (dataTableParameters.IdApplicationType.Split('-') != null && dataTableParameters.IdApplicationType.Split('-').Any() ? dataTableParameters.IdApplicationType.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobApplicationType.Contains((x.IdJobApplicationType ?? 0)));

                    string filteredApplicationType = string.Join(", ", rpoContext.JobApplicationTypes.Where(x => jobApplicationType.Contains(x.Id)).Select(x => (x.Description ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredApplicationType) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredApplicationType) ? "Application Type : " + filteredApplicationType : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationStatus))
                {
                    List<int> jobApplicationStatus = dataTableParameters.IdApplicationStatus != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationStatus) ? (dataTableParameters.IdApplicationStatus.Split('-') != null && dataTableParameters.IdApplicationStatus.Split('-').Any() ? dataTableParameters.IdApplicationStatus.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobApplicationStatus.Contains((x.JobApplicationStatusId ?? 0)));

                    string filteredApplicationStatus = string.Join(", ", rpoContext.ApplicationStatus.Where(x => jobApplicationStatus.Contains(x.Id)).Select(x => (x.Name ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredApplicationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredApplicationStatus) ? "Application Status : " + filteredApplicationStatus : string.Empty);
                }
                if (dataTableParameters.JobStatus != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(j => j.JobStatus == dataTableParameters.JobStatus);
                }

                string direction = string.Empty;
                string orderBy = string.Empty;

                List<ApplicationStatusReportDTO> result = new List<ApplicationStatusReportDTO>();

                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Dir))
                {
                    direction = dataTableParameters.OrderedColumn.Dir;
                }
                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Column))
                {
                    orderBy = dataTableParameters.OrderedColumn.Column;
                }


                if (!string.IsNullOrEmpty(direction) && direction == "desc")
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobAddress);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplicationType);
                    if (orderBy.Trim().ToLower() == "JobWorkTypeCode".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobWorkTypeCode);
                    if (orderBy.Trim().ToLower() == "JobApplicationStatus".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplicationStatus);
                    if (orderBy.Trim().ToLower() == "Filed".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Filed);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Withdrawn".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Withdrawn);
                    if (orderBy.Trim().ToLower() == "SignedOff".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.SignedOff);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobAddress);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplicationType);
                    if (orderBy.Trim().ToLower() == "JobWorkTypeCode".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobWorkTypeCode);
                    if (orderBy.Trim().ToLower() == "JobApplicationStatus".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplicationStatus);
                    if (orderBy.Trim().ToLower() == "Filed".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Filed);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Withdrawn".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Withdrawn);
                    if (orderBy.Trim().ToLower() == "SignedOff".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.SignedOff);
                }


                result = jobApplicationWorkPermitTypes
                    .AsEnumerable()
                    .Select(c => c)
                    .AsQueryable()
                    .ToList();

                string exportFilename = "ApplicationStatusReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                string exportFilePath = ExportToExcel(hearingDate, result, exportFilename);
                FileInfo fileinfo = new FileInfo(exportFilePath);
                long fileinfoSize = fileinfo.Length;

                var downloadFilePath = Properties.Settings.Default.APIUrl + "/" + Properties.Settings.Default.ReportExcelExportPath + "/" + exportFilename;
                List<KeyValuePair<string, string>> fileResult = new List<KeyValuePair<string, string>>();
                fileResult.Add(new KeyValuePair<string, string>("exportFilePath", downloadFilePath));
                fileResult.Add(new KeyValuePair<string, string>("exportFilesize", Convert.ToString(fileinfoSize)));

                return Ok(fileResult);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        ///create file and Export to Excel
        /// </summary>
        /// <param name="hearingDate"></param>
        /// <param name="result"></param>
        /// <param name="exportFilename"></param>
        /// <returns>To create excel file and return the path of file</returns>
        private string ExportToExcel(string hearingDate, List<ApplicationStatusReportDTO> result, string exportFilename)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
            // templateFileName = "ApplicationStatusTemplate.xlsx";
            templateFileName = "ApplicationStatusTemplate - Copy.xlsx";
            string templateFilePath = templatePath + templateFileName;
            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateFilePath);
            ISheet sheet = templateWorkbook.GetSheet("Sheet1");

            XSSFFont myFont = (XSSFFont)templateWorkbook.CreateFont();
            myFont.FontHeightInPoints = (short)12;
            myFont.FontName = "Times New Roman";
            myFont.IsBold = false;

            XSSFCellStyle leftAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            leftAlignCellStyle.SetFont(myFont);
            leftAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            leftAlignCellStyle.WrapText = true;
            leftAlignCellStyle.VerticalAlignment = VerticalAlignment.Top;
            leftAlignCellStyle.Alignment = HorizontalAlignment.Left;

            XSSFCellStyle rightAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            rightAlignCellStyle.SetFont(myFont);
            rightAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.WrapText = true;
            rightAlignCellStyle.VerticalAlignment = VerticalAlignment.Top;
            rightAlignCellStyle.Alignment = HorizontalAlignment.Right;

            IRow iDateRow = sheet.GetRow(2);
            if (iDateRow != null)
            {
                if (iDateRow.GetCell(0) != null)
                {
                    iDateRow.GetCell(0).SetCellValue(hearingDate);
                }
                else
                {
                    iDateRow.CreateCell(0).SetCellValue(hearingDate);
                }
            }
            else
            {
                iDateRow = sheet.CreateRow(2);
                if (iDateRow.GetCell(0) != null)
                {
                    iDateRow.GetCell(0).SetCellValue(hearingDate);
                }
                else
                {
                    iDateRow.CreateCell(0).SetCellValue(hearingDate);
                }
            }

            string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);
            IRow iReportDateRow = sheet.GetRow(3);
            if (iReportDateRow != null)
            {
                if (iReportDateRow.GetCell(11) != null)
                {
                    iReportDateRow.GetCell(11).SetCellValue(reportDate);
                }
                else
                {
                    iReportDateRow.CreateCell(11).SetCellValue(reportDate);
                }
            }
            else
            {
                iReportDateRow = sheet.CreateRow(3);
                if (iReportDateRow.GetCell(11) != null)
                {
                    iReportDateRow.GetCell(11).SetCellValue(reportDate);
                }
                else
                {
                    iReportDateRow.CreateCell(11).SetCellValue(reportDate);
                }
            }

            int index = 5;
            foreach (ApplicationStatusReportDTO item in result)
            {
                IRow iRow = sheet.GetRow(index);
                if (iRow != null)
                {
                    if (iRow.GetCell(0) != null)
                    {
                        iRow.GetCell(0).SetCellValue(item.JobNumber + " | " + item.JobAddress);
                    }
                    else
                    {
                        iRow.CreateCell(0).SetCellValue(item.JobNumber + " | " + item.JobAddress);
                    }

                    if (iRow.GetCell(1) != null)
                    {
                        iRow.GetCell(1).SetCellValue(item.Apartment);
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(item.Apartment);
                    }

                    if (iRow.GetCell(2) != null)
                    {
                        iRow.GetCell(2).SetCellValue(item.FloorNumber);
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(item.FloorNumber);
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(item.SpecialPlace);
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(item.SpecialPlace);
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.JobApplicationTypeName);
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.JobApplicationTypeName);
                    }

                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(!string.IsNullOrEmpty(item.PermitType) ? item.PermitType : item.JobWorkTypeCode);
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(!string.IsNullOrEmpty(item.PermitType) ? item.PermitType : item.JobWorkTypeCode);
                    }

                    if (iRow.GetCell(6) != null)
                    {
                        iRow.GetCell(6).SetCellValue(item.JobApplicationStatus);
                    }
                    else
                    {
                        iRow.CreateCell(6).SetCellValue(item.JobApplicationStatus);
                    }

                    if (iRow.GetCell(7) != null)
                    {
                        iRow.GetCell(7).SetCellValue(item.Filed != null ? Convert.ToDateTime(item.Filed).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(7).SetCellValue(item.Filed != null ? Convert.ToDateTime(item.Filed).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(8) != null)
                    {
                        iRow.GetCell(8).SetCellValue(item.Issued != null ? Convert.ToDateTime(item.Issued).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(8).SetCellValue(item.Issued != null ? Convert.ToDateTime(item.Issued).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(9) != null)
                    {
                        iRow.GetCell(9).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(9).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(10) != null)
                    {
                        iRow.GetCell(10).SetCellValue(item.Withdrawn != null ? Convert.ToDateTime(item.Withdrawn).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(10).SetCellValue(item.Withdrawn != null ? Convert.ToDateTime(item.Withdrawn).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(11) != null)
                    {
                        iRow.GetCell(11).SetCellValue(item.SignedOff != null ? Convert.ToDateTime(item.SignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(11).SetCellValue(item.SignedOff != null ? Convert.ToDateTime(item.SignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(7).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(9).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(10).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(11).CellStyle = leftAlignCellStyle;
                }
                else
                {
                    iRow = sheet.CreateRow(index);

                    if (iRow.GetCell(0) != null)
                    {
                        iRow.GetCell(0).SetCellValue(item.JobNumber + " | " + item.JobAddress);
                    }
                    else
                    {
                        iRow.CreateCell(0).SetCellValue(item.JobNumber + " | " + item.JobAddress);
                    }

                    if (iRow.GetCell(1) != null)
                    {
                        iRow.GetCell(1).SetCellValue(item.Apartment);
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(item.Apartment);
                    }

                    if (iRow.GetCell(2) != null)
                    {
                        iRow.GetCell(2).SetCellValue(item.FloorNumber);
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(item.FloorNumber);
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(item.SpecialPlace);
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(item.SpecialPlace);
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.JobApplicationTypeName);
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.JobApplicationTypeName);
                    }

                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(!string.IsNullOrEmpty(item.PermitType) ? item.PermitType : item.JobWorkTypeCode);
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(!string.IsNullOrEmpty(item.PermitType) ? item.PermitType : item.JobWorkTypeCode);
                    }

                    if (iRow.GetCell(6) != null)
                    {
                        iRow.GetCell(6).SetCellValue(item.JobApplicationStatus);
                    }
                    else
                    {
                        iRow.CreateCell(6).SetCellValue(item.JobApplicationStatus);
                    }

                    if (iRow.GetCell(7) != null)
                    {
                        iRow.GetCell(7).SetCellValue(item.Filed != null ? Convert.ToDateTime(item.Filed).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(7).SetCellValue(item.Filed != null ? Convert.ToDateTime(item.Filed).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(8) != null)
                    {
                        iRow.GetCell(8).SetCellValue(item.Issued != null ? Convert.ToDateTime(item.Issued).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(8).SetCellValue(item.Issued != null ? Convert.ToDateTime(item.Issued).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(9) != null)
                    {
                        iRow.GetCell(9).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(9).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(10) != null)
                    {
                        iRow.GetCell(10).SetCellValue(item.Withdrawn != null ? Convert.ToDateTime(item.Withdrawn).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(10).SetCellValue(item.Withdrawn != null ? Convert.ToDateTime(item.Withdrawn).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(11) != null)
                    {
                        iRow.GetCell(11).SetCellValue(item.SignedOff != null ? Convert.ToDateTime(item.SignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(11).SetCellValue(item.SignedOff != null ? Convert.ToDateTime(item.SignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(7).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(9).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(10).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(11).CellStyle = leftAlignCellStyle;
                }

                index = index + 1;
            }

            using (var file2 = new FileStream(exportFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                templateWorkbook.Write(file2);
                file2.Close();
            }

            return exportFilePath;
        }
        /// <summary>
        /// Get the report of Application Status Report with filter and sorting and export to pdf
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportApplicationStatus/exporttopdf")]
        public IHttpActionResult PostExportReportApplicationStatusToPdf(ApplicationStatusDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                               .Include("JobApplication.JobApplicationType")
                                                               .Include("JobApplication.ApplicationStatus")
                                                               .Include("JobApplication.Job.RfpAddress.Borough")
                                                               .Include("JobApplication.Job.Company")
                                                               .Include("JobApplication.Job.Contact")
                                                               .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                               .AsEnumerable()
                                                               .Select(c => this.FormatApplicationStatusReport(c))
                                                               .AsQueryable();

                var recordsTotal = jobApplicationWorkPermitTypes.Count();
                var recordsFiltered = recordsTotal;
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                string hearingDate = string.Empty;
                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
                {
                    List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.IdProjectManager ?? 0));

                    string filteredProjectManagers = string.Join(", ", rpoContext.Employees.Where(x => projectManagers.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredProjectManagers) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredProjectManagers) ? "Project Manager : " + filteredProjectManagers : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.IdJob));

                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                string filteredFiledOn = string.Empty;
                if (dataTableParameters != null && dataTableParameters.FiledOnFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Filed != null && Convert.ToDateTime(t.Filed).Date >= Convert.ToDateTime(dataTableParameters.FiledOnFromDate).Date);
                    filteredFiledOn = filteredFiledOn + Convert.ToDateTime(dataTableParameters.FiledOnFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.FiledOnToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Filed != null && Convert.ToDateTime(t.Filed).Date <= Convert.ToDateTime(dataTableParameters.FiledOnToDate).Date);
                    filteredFiledOn = filteredFiledOn + " - " + Convert.ToDateTime(dataTableParameters.FiledOnToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredFiledOn) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredFiledOn) ? "Filed Date : " + filteredFiledOn : string.Empty);

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.Description))
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.WorkDescription.Contains(dataTableParameters.Description));

                    string workDescription = dataTableParameters.Description;
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(workDescription) ? " | " : string.Empty) + (!string.IsNullOrEmpty(workDescription) ? "Work Description : " + workDescription : string.Empty);
                }

                string filteredIssued = string.Empty;
                if (dataTableParameters != null && dataTableParameters.IssuedFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Issued != null && Convert.ToDateTime(t.Issued).Date >= Convert.ToDateTime(dataTableParameters.IssuedFromDate).Date);
                    filteredIssued = filteredIssued + Convert.ToDateTime(dataTableParameters.IssuedFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.IssuedToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Issued != null && Convert.ToDateTime(t.Issued).Date <= Convert.ToDateTime(dataTableParameters.IssuedToDate).Date);
                    filteredIssued = filteredIssued + " - " + Convert.ToDateTime(dataTableParameters.IssuedToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredIssued) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredIssued) ? "Issued Date : " + filteredIssued : string.Empty);

                string filteredCompletion = string.Empty;
                if (dataTableParameters.SignedOffFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.SignedOff != null && Convert.ToDateTime(t.SignedOff).Date >= Convert.ToDateTime(dataTableParameters.SignedOffFromDate).Date);
                    filteredCompletion = filteredCompletion + Convert.ToDateTime(dataTableParameters.SignedOffFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.SignedOffToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.SignedOff != null && Convert.ToDateTime(t.SignedOff).Date <= Convert.ToDateTime(dataTableParameters.SignedOffToDate).Date);
                    filteredCompletion = filteredCompletion + " - " + Convert.ToDateTime(dataTableParameters.SignedOffToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompletion) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompletion) ? "Completion Date : " + filteredCompletion : string.Empty);

                if (dataTableParameters != null && dataTableParameters.HasLandMarkStatus != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.HasLandMarkStatus == (dataTableParameters.HasLandMarkStatus ?? false));

                    string hasLandMarkStatus = dataTableParameters.HasLandMarkStatus != null && Convert.ToBoolean(dataTableParameters.HasLandMarkStatus) == true ? "Yes" : "No";
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(hasLandMarkStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(hasLandMarkStatus) ? "LandMark Status : " + hasLandMarkStatus : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.IdContact)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationType))
                {
                    List<int> jobApplicationType = dataTableParameters.IdApplicationType != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationType) ? (dataTableParameters.IdApplicationType.Split('-') != null && dataTableParameters.IdApplicationType.Split('-').Any() ? dataTableParameters.IdApplicationType.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobApplicationType.Contains((x.IdJobApplicationType ?? 0)));

                    string filteredApplicationType = string.Join(", ", rpoContext.JobApplicationTypes.Where(x => jobApplicationType.Contains(x.Id)).Select(x => (x.Description ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredApplicationType) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredApplicationType) ? "Application Type : " + filteredApplicationType : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationStatus))
                {
                    List<int> jobApplicationStatus = dataTableParameters.IdApplicationStatus != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationStatus) ? (dataTableParameters.IdApplicationStatus.Split('-') != null && dataTableParameters.IdApplicationStatus.Split('-').Any() ? dataTableParameters.IdApplicationStatus.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobApplicationStatus.Contains((x.JobApplicationStatusId ?? 0)));

                    string filteredApplicationStatus = string.Join(", ", rpoContext.ApplicationStatus.Where(x => jobApplicationStatus.Contains(x.Id)).Select(x => (x.Name ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredApplicationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredApplicationStatus) ? "Application Status : " + filteredApplicationStatus : string.Empty);
                }
                if (dataTableParameters.JobStatus != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(j => j.JobStatus == dataTableParameters.JobStatus);
                }
                string direction = string.Empty;
                string orderBy = string.Empty;

                List<ApplicationStatusReportDTO> result = new List<ApplicationStatusReportDTO>();

                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Dir))
                {
                    direction = dataTableParameters.OrderedColumn.Dir;
                }
                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Column))
                {
                    orderBy = dataTableParameters.OrderedColumn.Column;
                }


                if (!string.IsNullOrEmpty(direction) && direction == "desc")
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobAddress);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplicationType);
                    if (orderBy.Trim().ToLower() == "JobWorkTypeCode".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobWorkTypeCode);
                    if (orderBy.Trim().ToLower() == "JobApplicationStatus".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplicationStatus);
                    if (orderBy.Trim().ToLower() == "Filed".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Filed);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Withdrawn".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Withdrawn);
                    if (orderBy.Trim().ToLower() == "SignedOff".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.SignedOff);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobAddress);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplicationType);
                    if (orderBy.Trim().ToLower() == "JobWorkTypeCode".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobWorkTypeCode);
                    if (orderBy.Trim().ToLower() == "JobApplicationStatus".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplicationStatus);
                    if (orderBy.Trim().ToLower() == "Filed".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Filed);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Withdrawn".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Withdrawn);
                    if (orderBy.Trim().ToLower() == "SignedOff".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.SignedOff);
                }


                result = jobApplicationWorkPermitTypes
                    .AsEnumerable()
                    .Select(c => c)
                    .AsQueryable()
                    .ToList();

                string exportFilename = "ApplicationStatusReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                string exportFilePath = ExportToPdf(hearingDate, result, exportFilename);
                FileInfo fileinfo = new FileInfo(exportFilePath);
                long fileinfoSize = fileinfo.Length;

                var downloadFilePath = Properties.Settings.Default.APIUrl + "/" + Properties.Settings.Default.ReportExcelExportPath + "/" + exportFilename;
                List<KeyValuePair<string, string>> fileResult = new List<KeyValuePair<string, string>>();
                fileResult.Add(new KeyValuePair<string, string>("exportFilePath", downloadFilePath));
                fileResult.Add(new KeyValuePair<string, string>("exportFilesize", Convert.ToString(fileinfoSize)));

                return Ok(fileResult);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Get the report of Application Status Report with filter and sorting and export to excel and send email
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns>To send email with attachment of excel</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportApplicationStatus/exporttoexcelemail")]
        public IHttpActionResult PostExportReportApplicationStatusToExcelEmail(ApplicationStatusDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                           .Include("JobApplication.JobApplicationType")
                                                           .Include("JobApplication.ApplicationStatus")
                                                           .Include("JobApplication.Job.RfpAddress.Borough")
                                                           .Include("JobApplication.Job.Company")
                                                           .Include("JobApplication.Job.Contact")
                                                           .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                           .AsEnumerable()
                                                           .Select(c => this.FormatApplicationStatusReport(c))
                                                           .AsQueryable();

                var recordsTotal = jobApplicationWorkPermitTypes.Count();
                var recordsFiltered = recordsTotal;
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                string hearingDate = string.Empty;
                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
                {
                    List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.IdProjectManager ?? 0));

                    string filteredProjectManagers = string.Join(", ", rpoContext.Employees.Where(x => projectManagers.Contains(x.Id)).Select(x => x.Id));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredProjectManagers) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredProjectManagers) ? "Project Manager : " + filteredProjectManagers : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.IdJob));

                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                string filteredFiledOn = string.Empty;
                if (dataTableParameters != null && dataTableParameters.FiledOnFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Filed != null && Convert.ToDateTime(t.Filed).Date >= Convert.ToDateTime(dataTableParameters.FiledOnFromDate).Date);
                    filteredFiledOn = filteredFiledOn + Convert.ToDateTime(dataTableParameters.FiledOnFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.FiledOnToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Filed != null && Convert.ToDateTime(t.Filed).Date <= Convert.ToDateTime(dataTableParameters.FiledOnToDate).Date);
                    filteredFiledOn = filteredFiledOn + " - " + Convert.ToDateTime(dataTableParameters.FiledOnToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredFiledOn) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredFiledOn) ? "Filed Date : " + filteredFiledOn : string.Empty);

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.Description))
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.WorkDescription.Contains(dataTableParameters.Description));

                    string workDescription = dataTableParameters.Description;
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(workDescription) ? " | " : string.Empty) + (!string.IsNullOrEmpty(workDescription) ? "Work Description : " + workDescription : string.Empty);
                }

                string filteredIssued = string.Empty;
                if (dataTableParameters != null && dataTableParameters.IssuedFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Issued != null && Convert.ToDateTime(t.Issued).Date >= Convert.ToDateTime(dataTableParameters.IssuedFromDate).Date);
                    filteredIssued = filteredIssued + Convert.ToDateTime(dataTableParameters.IssuedFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.IssuedToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Issued != null && Convert.ToDateTime(t.Issued).Date <= Convert.ToDateTime(dataTableParameters.IssuedToDate).Date);
                    filteredIssued = filteredIssued + " - " + Convert.ToDateTime(dataTableParameters.IssuedToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredIssued) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredIssued) ? "Issued Date : " + filteredIssued : string.Empty);

                string filteredCompletion = string.Empty;
                if (dataTableParameters.SignedOffFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.SignedOff != null && Convert.ToDateTime(t.SignedOff).Date >= Convert.ToDateTime(dataTableParameters.SignedOffFromDate).Date);
                    filteredCompletion = filteredCompletion + Convert.ToDateTime(dataTableParameters.SignedOffFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.SignedOffToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.SignedOff != null && Convert.ToDateTime(t.SignedOff).Date <= Convert.ToDateTime(dataTableParameters.SignedOffToDate).Date);
                    filteredCompletion = filteredCompletion + " - " + Convert.ToDateTime(dataTableParameters.SignedOffToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompletion) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompletion) ? "Completion Date : " + filteredCompletion : string.Empty);

                if (dataTableParameters != null && dataTableParameters.HasLandMarkStatus != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.HasLandMarkStatus == (dataTableParameters.HasLandMarkStatus ?? false));

                    string hasLandMarkStatus = dataTableParameters.HasLandMarkStatus != null && Convert.ToBoolean(dataTableParameters.HasLandMarkStatus) == true ? "Yes" : "No";
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(hasLandMarkStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(hasLandMarkStatus) ? "LandMark Status : " + hasLandMarkStatus : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.IdContact)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationType))
                {
                    List<int> jobApplicationType = dataTableParameters.IdApplicationType != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationType) ? (dataTableParameters.IdApplicationType.Split('-') != null && dataTableParameters.IdApplicationType.Split('-').Any() ? dataTableParameters.IdApplicationType.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobApplicationType.Contains((x.IdJobApplicationType ?? 0)));

                    string filteredApplicationType = string.Join(", ", rpoContext.JobApplicationTypes.Where(x => jobApplicationType.Contains(x.Id)).Select(x => (x.Description ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredApplicationType) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredApplicationType) ? "Application Type : " + filteredApplicationType : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationStatus))
                {
                    List<int> jobApplicationStatus = dataTableParameters.IdApplicationStatus != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationStatus) ? (dataTableParameters.IdApplicationStatus.Split('-') != null && dataTableParameters.IdApplicationStatus.Split('-').Any() ? dataTableParameters.IdApplicationStatus.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobApplicationStatus.Contains((x.JobApplicationStatusId ?? 0)));

                    string filteredApplicationStatus = string.Join(", ", rpoContext.ApplicationStatus.Where(x => jobApplicationStatus.Contains(x.Id)).Select(x => (x.Name ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredApplicationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredApplicationStatus) ? "Application Status : " + filteredApplicationStatus : string.Empty);
                }

                if (dataTableParameters.JobStatus != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(j => j.JobStatus == dataTableParameters.JobStatus);
                }

                string direction = string.Empty;
                string orderBy = string.Empty;

                List<ApplicationStatusReportDTO> result = new List<ApplicationStatusReportDTO>();

                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Dir))
                {
                    direction = dataTableParameters.OrderedColumn.Dir;
                }
                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Column))
                {
                    orderBy = dataTableParameters.OrderedColumn.Column;
                }


                if (!string.IsNullOrEmpty(direction) && direction == "desc")
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobAddress);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplicationType);
                    if (orderBy.Trim().ToLower() == "JobWorkTypeCode".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobWorkTypeCode);
                    if (orderBy.Trim().ToLower() == "JobApplicationStatus".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplicationStatus);
                    if (orderBy.Trim().ToLower() == "Filed".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Filed);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Withdrawn".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Withdrawn);
                    if (orderBy.Trim().ToLower() == "SignedOff".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.SignedOff);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobAddress);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplicationType);
                    if (orderBy.Trim().ToLower() == "JobWorkTypeCode".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobWorkTypeCode);
                    if (orderBy.Trim().ToLower() == "JobApplicationStatus".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplicationStatus);
                    if (orderBy.Trim().ToLower() == "Filed".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Filed);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Withdrawn".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Withdrawn);
                    if (orderBy.Trim().ToLower() == "SignedOff".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.SignedOff);
                }


                result = jobApplicationWorkPermitTypes
                    .AsEnumerable()
                    .Select(c => c)
                    .AsQueryable()
                    .ToList();

                string exportFilename = "ApplicationStatusReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                string exportFilePath = ExportToExcel(hearingDate, result, exportFilename);
                //FileInfo fileinfo = new FileInfo(exportFilePath);
                //long fileinfoSize = fileinfo.Length;

                //var downloadFilePath = Properties.Settings.Default.APIUrl + "/" + Properties.Settings.Default.ReportExcelExportPath + "/" + exportFilename;
                //List<KeyValuePair<string, string>> fileResult = new List<KeyValuePair<string, string>>();
                //fileResult.Add(new KeyValuePair<string, string>("exportFilePath", downloadFilePath));
                //fileResult.Add(new KeyValuePair<string, string>("exportFilesize", Convert.ToString(fileinfoSize)));

                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
                string path = APIUrl + "/UploadedDocuments/ReportExportToExcel/" + exportFilename;



                return Ok(new { Filepath = exportFilePath, NewPath = path, ReportName = exportFilename });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Get the report of Application Status Report with filter and sorting and export to pdf and send email
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns>To send email with attachment of pdf</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportApplicationStatus/exporttopdfemail")]
        public IHttpActionResult PostExportReportApplicationStatusToPdfEmail(ApplicationStatusDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                           .Include("JobApplication.JobApplicationType")
                                                           .Include("JobApplication.ApplicationStatus")
                                                           .Include("JobApplication.Job.RfpAddress.Borough")
                                                           .Include("JobApplication.Job.Company")
                                                           .Include("JobApplication.Job.Contact")
                                                           .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                           .AsEnumerable()
                                                           .Select(c => this.FormatApplicationStatusReport(c))
                                                           .AsQueryable();

                var recordsTotal = jobApplicationWorkPermitTypes.Count();
                var recordsFiltered = recordsTotal;
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                string hearingDate = string.Empty;
                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
                {
                    List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.IdProjectManager ?? 0));

                    string filteredProjectManagers = string.Join(", ", rpoContext.Employees.Where(x => projectManagers.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredProjectManagers) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredProjectManagers) ? "Project Manager : " + filteredProjectManagers : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.IdJob));

                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                string filteredFiledOn = string.Empty;
                if (dataTableParameters != null && dataTableParameters.FiledOnFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Filed != null && Convert.ToDateTime(t.Filed).Date >= Convert.ToDateTime(dataTableParameters.FiledOnFromDate).Date);
                    filteredFiledOn = filteredFiledOn + Convert.ToDateTime(dataTableParameters.FiledOnFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.FiledOnToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Filed != null && Convert.ToDateTime(t.Filed).Date <= Convert.ToDateTime(dataTableParameters.FiledOnToDate).Date);
                    filteredFiledOn = filteredFiledOn + " - " + Convert.ToDateTime(dataTableParameters.FiledOnToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredFiledOn) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredFiledOn) ? "Filed Date : " + filteredFiledOn : string.Empty);

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.Description))
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.WorkDescription.Contains(dataTableParameters.Description));

                    string workDescription = dataTableParameters.Description;
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(workDescription) ? " | " : string.Empty) + (!string.IsNullOrEmpty(workDescription) ? "Work Description : " + workDescription : string.Empty);
                }

                string filteredIssued = string.Empty;
                if (dataTableParameters != null && dataTableParameters.IssuedFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Issued != null && Convert.ToDateTime(t.Issued).Date >= Convert.ToDateTime(dataTableParameters.IssuedFromDate).Date);
                    filteredIssued = filteredIssued + Convert.ToDateTime(dataTableParameters.IssuedFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.IssuedToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.Issued != null && Convert.ToDateTime(t.Issued).Date <= Convert.ToDateTime(dataTableParameters.IssuedToDate).Date);
                    filteredIssued = filteredIssued + " - " + Convert.ToDateTime(dataTableParameters.IssuedToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredIssued) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredIssued) ? "Issued Date : " + filteredIssued : string.Empty);

                string filteredCompletion = string.Empty;
                if (dataTableParameters.SignedOffFromDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.SignedOff != null && Convert.ToDateTime(t.SignedOff).Date >= Convert.ToDateTime(dataTableParameters.SignedOffFromDate).Date);
                    filteredCompletion = filteredCompletion + Convert.ToDateTime(dataTableParameters.SignedOffFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.SignedOffToDate != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.SignedOff != null && Convert.ToDateTime(t.SignedOff).Date <= Convert.ToDateTime(dataTableParameters.SignedOffToDate).Date);
                    filteredCompletion = filteredCompletion + " - " + Convert.ToDateTime(dataTableParameters.SignedOffToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompletion) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompletion) ? "Completion Date : " + filteredCompletion : string.Empty);

                if (dataTableParameters != null && dataTableParameters.HasLandMarkStatus != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(t => t.HasLandMarkStatus == (dataTableParameters.HasLandMarkStatus ?? false));

                    string hasLandMarkStatus = dataTableParameters.HasLandMarkStatus != null && Convert.ToBoolean(dataTableParameters.HasLandMarkStatus) == true ? "Yes" : "No";
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(hasLandMarkStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(hasLandMarkStatus) ? "LandMark Status : " + hasLandMarkStatus : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.IdContact)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationType))
                {
                    List<int> jobApplicationType = dataTableParameters.IdApplicationType != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationType) ? (dataTableParameters.IdApplicationType.Split('-') != null && dataTableParameters.IdApplicationType.Split('-').Any() ? dataTableParameters.IdApplicationType.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobApplicationType.Contains((x.IdJobApplicationType ?? 0)));

                    string filteredApplicationType = string.Join(", ", rpoContext.JobApplicationTypes.Where(x => jobApplicationType.Contains(x.Id)).Select(x => (x.Description ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredApplicationType) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredApplicationType) ? "Application Type : " + filteredApplicationType : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationStatus))
                {
                    List<int> jobApplicationStatus = dataTableParameters.IdApplicationStatus != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicationStatus) ? (dataTableParameters.IdApplicationStatus.Split('-') != null && dataTableParameters.IdApplicationStatus.Split('-').Any() ? dataTableParameters.IdApplicationStatus.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobApplicationStatus.Contains((x.JobApplicationStatusId ?? 0)));

                    string filteredApplicationStatus = string.Join(", ", rpoContext.ApplicationStatus.Where(x => jobApplicationStatus.Contains(x.Id)).Select(x => (x.Name ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredApplicationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredApplicationStatus) ? "Application Status : " + filteredApplicationStatus : string.Empty);
                }
                if (dataTableParameters.JobStatus != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(j => j.JobStatus == dataTableParameters.JobStatus);
                }
                string direction = string.Empty;
                string orderBy = string.Empty;

                List<ApplicationStatusReportDTO> result = new List<ApplicationStatusReportDTO>();

                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Dir))
                {
                    direction = dataTableParameters.OrderedColumn.Dir;
                }
                if (dataTableParameters.OrderedColumn != null && !string.IsNullOrEmpty(dataTableParameters.OrderedColumn.Column))
                {
                    orderBy = dataTableParameters.OrderedColumn.Column;
                }


                if (!string.IsNullOrEmpty(direction) && direction == "desc")
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobAddress);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplicationType);
                    if (orderBy.Trim().ToLower() == "JobWorkTypeCode".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobWorkTypeCode);
                    if (orderBy.Trim().ToLower() == "JobApplicationStatus".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplicationStatus);
                    if (orderBy.Trim().ToLower() == "Filed".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Filed);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Withdrawn".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Withdrawn);
                    if (orderBy.Trim().ToLower() == "SignedOff".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.SignedOff);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobAddress);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplicationType);
                    if (orderBy.Trim().ToLower() == "JobWorkTypeCode".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobWorkTypeCode);
                    if (orderBy.Trim().ToLower() == "JobApplicationStatus".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplicationStatus);
                    if (orderBy.Trim().ToLower() == "Filed".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Filed);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Withdrawn".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Withdrawn);
                    if (orderBy.Trim().ToLower() == "SignedOff".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.SignedOff);
                }


                result = jobApplicationWorkPermitTypes
                    .AsEnumerable()
                    .Select(c => c)
                    .AsQueryable()
                    .ToList();


                string exportFilename = "ApplicationStatusReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                string exportFilePath = ExportToPdf(hearingDate, result, exportFilename);
                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

                string path = APIUrl + "/UploadedDocuments/ReportExportToExcel/" + exportFilename;

                return Ok(new { Filepath = exportFilePath, NewPath = path, ReportName = exportFilename });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }



        /// <summary>
        /// create file and Export to pdf
        /// </summary>
        /// <param name="hearingDate"></param>
        /// <param name="result"></param>
        /// <param name="exportFilename"></param>
        /// <returns></returns>
        private string ExportToPdf(string hearingDate, List<ApplicationStatusReportDTO> result, string exportFilename)
        {

            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            using (MemoryStream stream = new MemoryStream())
            {
                Document document = new Document(PageSize.LETTER.Rotate());
                document.SetMargins(18, 18, 12, 36);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(exportFilePath, FileMode.Create));
                writer.PageEvent = new Header();
                document.Open();

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\logo-new.jpg"));
                logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                logo.ScaleToFit(120, 60);
                logo.SetAbsolutePosition(260, 760);

                Font font_10_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 0);
                Font font_12_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 0);

                Font font_10_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1);
                Font font_12_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);
                Font font_16_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 16, 1);

                Font font_Table_Header = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1);
                Font font_Table_Data = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 0);

                PdfPTable table = new PdfPTable(12);
                table.WidthPercentage = 100;
                table.SplitLate = false;

                //PdfPCell cell = new PdfPCell(logo);
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Rowspan = 2;
                //table.AddCell(cell);

                //string reportHeader = "NEW YORK CITY AGENCY FILINGS, APPROVALS & PERMITS"
                //    + Environment.NewLine + "146 WEST 29TH STREET, SUITE 2E"
                //    + Environment.NewLine + "NEW YORK, NY 10001"
                //    + Environment.NewLine + "TEL: (212) 566-5110"
                //    + Environment.NewLine + "FAX: (212) 566-5112";

                //cell = new PdfPCell(new Phrase("RPO INCORPORATED", font_11_Bold));
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 11;
                //cell.PaddingLeft = -60;
                //table.AddCell(cell);

                //cell = new PdfPCell(new Phrase(reportHeader, font_11_Normal));
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 11;
                //cell.PaddingLeft = -60;
                //cell.VerticalAlignment = Element.ALIGN_TOP;
                //table.AddCell(cell);


                //cell = new PdfPCell(SnapCorLogo);
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Rowspan = 2;
                ////cell.PaddingLeft = -80;
                //cell.PaddingTop = -60;
                //cell.Colspan = 12;
                //table.AddCell(cell);

                string reportHeader = //"RPO INCORPORATED" + Environment.NewLine +
                   "146 West 29th Street, Suite 2E"
                   + Environment.NewLine + "New York, NY 10001"
                   + Environment.NewLine + "(212) 566-5110"
                   + Environment.NewLine + "www.rpoinc.com";

                PdfPCell cell = new PdfPCell(logo);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Rowspan = 2;
                cell.PaddingBottom = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Construction Consultants", font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -5;
                cell.Colspan = 11;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -5;
                cell.Colspan = 11;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = -10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("APPLICATION STATUS REPORT", font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 12;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(hearingDate, font_12_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 12;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 12;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 10;
                table.AddCell(cell);

                string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

                cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project # | Address", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Apt", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Floor", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Special Place Name", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Application Type", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Work Type / Permit Type", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Application Status", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Filed", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Issued", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Expired", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Withdrawn", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Completion", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                foreach (ApplicationStatusReportDTO item in result)
                {
                    cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.JobAddress, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Apartment, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.FloorNumber, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.SpecialPlace, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.JobApplicationTypeName, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(!string.IsNullOrEmpty(item.PermitType) ? item.PermitType : item.JobWorkTypeCode, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.JobApplicationStatus, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Filed != null ? Convert.ToDateTime(item.Filed).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Issued != null ? Convert.ToDateTime(item.Issued).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Withdrawn != null ? Convert.ToDateTime(item.Withdrawn).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.SignedOff != null ? Convert.ToDateTime(item.SignedOff).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    table.AddCell(cell);
                }

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }
        /// <summary>
        /// Bind the values of each object for Application Status Report Required
        /// </summary>
        /// <param name="jobApplicationWorkPermitResponse"></param>
        /// <returns> Bind the values of  Application Status Report</returns>
        private ApplicationStatusReportDTO FormatApplicationStatusReport(JobApplicationWorkPermitType jobApplicationWorkPermitResponse)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string JobApplicationType = string.Empty;

            if (jobApplicationWorkPermitResponse.JobApplication.Job != null && jobApplicationWorkPermitResponse.JobApplication.IdJob != null)
            {
                JobApplicationType = string.Join(",", jobApplicationWorkPermitResponse.JobApplication.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }

            return new ApplicationStatusReportDTO
            {
                Id = jobApplicationWorkPermitResponse.Id,
                IdJobApplication = jobApplicationWorkPermitResponse.IdJobApplication,
                JobApplicationType = JobApplicationType,
                IdJob = jobApplicationWorkPermitResponse.JobApplication.IdJob,
                IdParent = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.JobApplicationType != null ? jobApplicationWorkPermitResponse.JobApplication.JobApplicationType.IdParent : null,
                IdProjectManager = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null ? jobApplicationWorkPermitResponse.JobApplication.Job.IdProjectManager : null,
                IdContact = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null && jobApplicationWorkPermitResponse.JobApplication.Job.IdContact != null ? jobApplicationWorkPermitResponse.JobApplication.Job.IdContact.Value : 0,
                HasLandMarkStatus = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null ? jobApplicationWorkPermitResponse.JobApplication.Job.HasLandMarkStatus : false,
                IdCompany = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null ? jobApplicationWorkPermitResponse.JobApplication.Job.IdCompany : null,
                JobNumber = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null ? jobApplicationWorkPermitResponse.JobApplication.Job.JobNumber : string.Empty,
                JobAddress = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null
                     && jobApplicationWorkPermitResponse.JobApplication.Job.RfpAddress != null
                     ? jobApplicationWorkPermitResponse.JobApplication.Job.RfpAddress.HouseNumber + " " + (jobApplicationWorkPermitResponse.JobApplication.Job.RfpAddress.Street != null ? jobApplicationWorkPermitResponse.JobApplication.Job.RfpAddress.Street + ", " : string.Empty)
                       + (jobApplicationWorkPermitResponse.JobApplication.Job.RfpAddress.Borough != null ? jobApplicationWorkPermitResponse.JobApplication.Job.RfpAddress.Borough.Description : string.Empty)
                    : string.Empty,
                Borough = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null && jobApplicationWorkPermitResponse.JobApplication.Job.RfpAddress != null && jobApplicationWorkPermitResponse.JobApplication.Job.RfpAddress.Borough != null ? jobApplicationWorkPermitResponse.JobApplication.Job.RfpAddress.Borough.Description : string.Empty,
                Apartment = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null ? jobApplicationWorkPermitResponse.JobApplication.Job.Apartment : string.Empty,
                FloorNumber = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null ? jobApplicationWorkPermitResponse.JobApplication.Job.FloorNumber : string.Empty,
                SpecialPlace = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null ? jobApplicationWorkPermitResponse.JobApplication.Job.SpecialPlace : string.Empty,
                Client = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null && jobApplicationWorkPermitResponse.JobApplication.Job.Contact != null ? jobApplicationWorkPermitResponse.JobApplication.Job.Contact.FirstName + " " + jobApplicationWorkPermitResponse.JobApplication.Job.Contact.LastName : string.Empty,
                Company = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.Job != null && jobApplicationWorkPermitResponse.JobApplication.Job.Company != null ? jobApplicationWorkPermitResponse.JobApplication.Job.Company.Name : string.Empty,
                JobApplicationNumber = jobApplicationWorkPermitResponse.JobApplication.ApplicationNumber,
                JobApplicationFor = jobApplicationWorkPermitResponse.JobApplication.ApplicationFor,
                JobApplicationStatusId = jobApplicationWorkPermitResponse.JobApplication.IdApplicationStatus,
                JobApplicationStatus = jobApplicationWorkPermitResponse.JobApplication.ApplicationStatus != null ? jobApplicationWorkPermitResponse.JobApplication.ApplicationStatus.Name : string.Empty,
                JobApplicationFloor = jobApplicationWorkPermitResponse.JobApplication.FloorWorking,
                JobApplicationTypeName = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.JobApplicationType != null ? jobApplicationWorkPermitResponse.JobApplication.JobApplicationType.Description : string.Empty,
                IdJobApplicationType = jobApplicationWorkPermitResponse.JobApplication.IdJobApplicationType,
                IdJobWorkType = jobApplicationWorkPermitResponse.IdJobWorkType,
                JobWorkTypeDescription = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Description : string.Empty,
                JobWorkTypeContent = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Content : string.Empty,
                JobWorkTypeCode = jobApplicationWorkPermitResponse.JobWorkType != null ? jobApplicationWorkPermitResponse.JobWorkType.Code : string.Empty,
                Code = jobApplicationWorkPermitResponse.Code,
                EstimatedCost = jobApplicationWorkPermitResponse.EstimatedCost,
                RenewalFee = jobApplicationWorkPermitResponse.RenewalFee,
                PreviousPermitNumber = jobApplicationWorkPermitResponse.PreviousPermitNumber,
                PermitNumber = jobApplicationWorkPermitResponse.PermitNumber,
                Withdrawn = jobApplicationWorkPermitResponse.Withdrawn != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Withdrawn), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Withdrawn,
                Filed = jobApplicationWorkPermitResponse.Filed != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Filed), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Filed,
                Issued = jobApplicationWorkPermitResponse.Issued != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Issued), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Issued,
                Expires = jobApplicationWorkPermitResponse.Expires != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Expires), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Expires,
                SignedOff = jobApplicationWorkPermitResponse.SignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.SignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.SignedOff,
                WorkDescription = jobApplicationWorkPermitResponse.WorkDescription,
                IdResponsibility = jobApplicationWorkPermitResponse.IdResponsibility,
                DocumentPath = jobApplicationWorkPermitResponse.DocumentPath != null ? APIUrl + "/" + Properties.Settings.Default.DOTWorkPermitDocument + "/" + jobApplicationWorkPermitResponse.Id + "_" + jobApplicationWorkPermitResponse.DocumentPath : string.Empty,
                LastModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                CompanyResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.Company != null ? jobApplicationWorkPermitResponse.ContactResponsible.Company.Name : string.Empty,
                PersonalResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.FirstName != null ? jobApplicationWorkPermitResponse.ContactResponsible.FirstName + (jobApplicationWorkPermitResponse.ContactResponsible.LastName != null ? jobApplicationWorkPermitResponse.ContactResponsible.LastName : string.Empty) : string.Empty,
                PermitType = jobApplicationWorkPermitResponse.PermitType,
                ForPurposeOf = jobApplicationWorkPermitResponse.ForPurposeOf,
                EquipmentType = jobApplicationWorkPermitResponse.EquipmentType,
                PlumbingSignedOff = jobApplicationWorkPermitResponse.PlumbingSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.PlumbingSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.PlumbingSignedOff,
                FinalElevator = jobApplicationWorkPermitResponse.FinalElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.FinalElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.FinalElevator,
                TempElevator = jobApplicationWorkPermitResponse.TempElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.TempElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.TempElevator,
                ConstructionSignedOff = jobApplicationWorkPermitResponse.ConstructionSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.ConstructionSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.ConstructionSignedOff,
                Permittee = jobApplicationWorkPermitResponse.Permittee,
                JobStatus = jobApplicationWorkPermitResponse.JobApplication.Job.Status,
            };
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

        public partial class Header : PdfPageEventHelper
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Header"/> class.
            /// </summary>
            /// <param name="FirstLastName">First name of the last.</param>
            /// <param name="proposalName">Name of the proposal.</param>
            public Header()
            {
            }

            /// <summary>
            /// Called when [end page].
            /// </summary>
            /// <param name="writer">The writer.</param>
            /// <param name="doc">The document.</param>
            public override void OnEndPage(PdfWriter writer, Document doc)
            {
                base.OnEndPage(writer, doc);

                int pageNumber = writer.PageNumber;

                iTextSharp.text.Image SnapCorLogo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\logo-footer.jpg"));
                SnapCorLogo.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                SnapCorLogo.ScaleToFit(112, 32);
                SnapCorLogo.SetAbsolutePosition(260, 760);

                PdfPTable table = new PdfPTable(1);

                table.TotalWidth = doc.PageSize.Width - 80f;
                table.WidthPercentage = 70;

                PdfPCell cell = new PdfPCell(new Phrase("" + pageNumber, new Font(Font.FontFamily.TIMES_ROMAN, 12)));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                //cell.PaddingTop = 10f;
                table.AddCell(cell);


                cell = new PdfPCell(SnapCorLogo);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingTop = -15;
                table.AddCell(cell);

                table.WriteSelectedRows(0, -1, 40, doc.Bottom, writer.DirectContent);
            }
        }
    }
}