
namespace Rpo.ApiServices.Api.Controllers.Reports
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DataTable;
    using Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model.Models;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using NPOI.XSSF.UserModel;
    using NPOI.SS.UserModel;
    using iTextSharp.text.pdf;
    using iTextSharp.text;

    [Authorize]
    public class ReportPermitsExpiryController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();
        /// <summary>
        /// Get the report of ReportPermitExpiry Report with filter and sorting 
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        public IHttpActionResult GetReportPermitsExpiry([FromUri] PermitsExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewPermitExpiryReport))
            {
                if (dataTableParameters != null && dataTableParameters.PermitCode == "DOB")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DOT")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOT.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DEP")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DEP.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "AHV")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "TCO")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }

                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                .Include("JobApplication.JobApplicationType")
                                                .Include("JobApplication.ApplicationStatus")
                                                .Include("JobApplication.Job.RfpAddress.Borough")
                                                .Include("JobApplication.Job.Company")
                                                .Include("JobApplication.Job.Contact")
                                                .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                .Where(x =>
                                                           x.JobApplication.JobApplicationType.IdParent == dataTableParameters.IdJobType
                                                          && ((DbFunctions.TruncateTime(x.Expires) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
                                                          && DbFunctions.TruncateTime(x.Expires) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate)))
                                                           )
                                                .AsQueryable();

                var recordsTotal = jobApplicationWorkPermitTypes.Count();
                var recordsFiltered = recordsTotal;
                string permitCode = string.Empty;
                switch (dataTableParameters.IdJobType)
                {
                    case 1:
                        permitCode = "DOB";
                        break;
                    case 2:
                        permitCode = "DOT";
                        break;
                    case 4:
                        permitCode = "DEP";
                        break;
                }

                if (dataTableParameters.IdJobType == 1)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.SignOff == false);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdBorough))
                {
                    List<int> boroughs = dataTableParameters.IdBorough != null && !string.IsNullOrEmpty(dataTableParameters.IdBorough) ? (dataTableParameters.IdBorough.Split('-') != null && dataTableParameters.IdBorough.Split('-').Any() ? dataTableParameters.IdBorough.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => boroughs.Contains(x.JobApplication.Job.RfpAddress.IdBorough ?? 0));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
                {
                    List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.JobApplication.Job.IdProjectManager ?? 0));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.JobApplication.IdJob));
                }

                if (dataTableParameters != null && dataTableParameters.PermitCode != null)
                {
                    permitCode = dataTableParameters.PermitCode;
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code == dataTableParameters.PermitCode);
                }
                else
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code != "AHV" && x.JobWorkType.Code != "TCO");
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.Job.Status == dataTableParameters.Status);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.JobApplication.Job.IdCompany ?? 0)));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.JobApplication.Job.IdContact.Value)));
                }
                if (!string.IsNullOrEmpty(dataTableParameters.IdResponsibility))
                {
                    //jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.IdResponsibility == dataTableParameters.IdResponsibility);
                    List<int> jobResponsibilityTeam = dataTableParameters.IdResponsibility != null && !string.IsNullOrEmpty(dataTableParameters.IdResponsibility) ? (dataTableParameters.IdResponsibility.Split('-') != null && dataTableParameters.IdResponsibility.Split('-').Any() ? dataTableParameters.IdResponsibility.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobResponsibilityTeam.Contains((x.IdResponsibility.Value)));
                }
                var result = jobApplicationWorkPermitTypes
                        .AsEnumerable()
                        .Select(c => this.FormatDOBPermitsExpiryReport(c, permitCode))
                        .AsQueryable()
                        .DataTableParameters(dataTableParameters, out recordsFiltered)
                        .ToArray();

                return this.Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result.OrderByDescending(x => x.IdJob)
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Get the report of ReportPermitExpiry Report with filter and sorting  and export to excel
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportPermitsExpiry/exporttoexcel")]
        public IHttpActionResult PostExportPermitsExpiryToExcel(PermitsExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                #region Filter

                if (dataTableParameters != null && dataTableParameters.PermitCode == "DOB")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DOT")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOT.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DEP")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DEP.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "AHV")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "TCO")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }

                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                    .Include("JobApplication.JobApplicationType")
                                                    .Include("JobApplication.ApplicationStatus")
                                                    .Include("JobApplication.Job.RfpAddress.Borough")
                                                    .Include("JobApplication.Job.Company")
                                                    .Include("JobApplication.Job.Contact")
                                                    .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                    .Where(x =>
                                                               x.JobApplication.JobApplicationType.IdParent == dataTableParameters.IdJobType
                                                               && ((DbFunctions.TruncateTime(x.Expires) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
                                                               && DbFunctions.TruncateTime(x.Expires) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
                                                               ))
                                                    .AsQueryable();

                var recordsTotal = jobApplicationWorkPermitTypes.Count();
                var recordsFiltered = recordsTotal;
                string permitCode = string.Empty;
                switch (dataTableParameters.IdJobType)
                {
                    case 1:
                        permitCode = "DOB";
                        break;
                    case 2:
                        permitCode = "DOT";
                        break;
                    case 4:
                        permitCode = "DEP";
                        break;
                }


                if (dataTableParameters.IdJobType == 1)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.SignOff == false);
                }
                string hearingDate = string.Empty;

                string permitExpire = string.Empty;
                if (dataTableParameters != null && dataTableParameters.ExpiresFromDate != null)
                {
                    permitExpire = permitExpire + Convert.ToDateTime(dataTableParameters.ExpiresFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.ExpiresToDate != null)
                {
                    permitExpire = permitExpire + " - " + Convert.ToDateTime(dataTableParameters.ExpiresToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(permitExpire) ? " | " : string.Empty) + (!string.IsNullOrEmpty(permitExpire) ? "Permit Expires : " + permitExpire : string.Empty);

                if (!string.IsNullOrEmpty(dataTableParameters.IdBorough))
                {
                    List<int> boroughs = dataTableParameters.IdBorough != null && !string.IsNullOrEmpty(dataTableParameters.IdBorough) ? (dataTableParameters.IdBorough.Split('-') != null && dataTableParameters.IdBorough.Split('-').Any() ? dataTableParameters.IdBorough.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => boroughs.Contains(x.JobApplication.Job.RfpAddress.IdBorough ?? 0));

                    string filteredBoroughes = string.Join(", ", rpoContext.Boroughes.Where(x => boroughs.Contains(x.Id)).Select(x => x.Description));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredBoroughes) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredBoroughes) ? "Borough : " + filteredBoroughes : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
                {
                    List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.JobApplication.Job.IdProjectManager ?? 0));

                    string filteredProjectManagers = string.Join(", ", rpoContext.Employees.Where(x => projectManagers.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredProjectManagers) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredProjectManagers) ? "Project Managers : " + filteredProjectManagers : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.JobApplication.IdJob));

                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.PermitCode != null)
                {
                    permitCode = dataTableParameters.PermitCode;
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code == dataTableParameters.PermitCode);
                }
                else
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code != "AHV" && x.JobWorkType.Code != "TCO");
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.Job.Status == dataTableParameters.Status);

                    string filteredStatus = dataTableParameters.Status.ToString();
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredStatus) ? "Status : " + filteredStatus : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.JobApplication.Job.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.JobApplication.Job.IdContact.Value)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }
                if (!string.IsNullOrEmpty(dataTableParameters.IdResponsibility))
                {
                    List<int> jobResponsibilityTeam = dataTableParameters.IdResponsibility != null && !string.IsNullOrEmpty(dataTableParameters.IdResponsibility) ? (dataTableParameters.IdResponsibility.Split('-') != null && dataTableParameters.IdResponsibility.Split('-').Any() ? dataTableParameters.IdResponsibility.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobResponsibilityTeam.Contains((x.IdResponsibility.Value)));
                }

                string direction = string.Empty;
                string orderBy = string.Empty;

                List<PermitsExpiryDTO> result = new List<PermitsExpiryDTO>();

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
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.HouseNumber);
                    if (orderBy.Trim().ToLower() == "Apartment".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Apartment);
                    if (orderBy.Trim().ToLower() == "Borough".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Borough.Description);
                    if (orderBy.Trim().ToLower() == "Client".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Contact.FirstName);
                    if (orderBy.Trim().ToLower() == "FloorNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.FloorNumber);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationTypeName".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.JobApplicationType.Description);
                    if (orderBy.Trim().ToLower() == "JobApplicationNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.ApplicationNumber);
                    if (orderBy.Trim().ToLower() == "PermitType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.PermitType);
                    if (orderBy.Trim().ToLower() == "Permittee".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Permittee);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "IsPGL".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.IsPGL);
                    if (orderBy.Trim().ToLower() == "L2".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.HasOpenWork);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.HouseNumber);
                    if (orderBy.Trim().ToLower() == "Apartment".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Apartment);
                    if (orderBy.Trim().ToLower() == "Borough".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Borough.Description);
                    if (orderBy.Trim().ToLower() == "Client".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Contact.FirstName);
                    if (orderBy.Trim().ToLower() == "FloorNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.FloorNumber);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationTypeName".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.JobApplicationType.Description);
                    if (orderBy.Trim().ToLower() == "JobApplicationNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.ApplicationNumber);
                    if (orderBy.Trim().ToLower() == "PermitType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.PermitType);
                    if (orderBy.Trim().ToLower() == "Permittee".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Permittee);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "IsPGL".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.IsPGL);
                    if (orderBy.Trim().ToLower() == "L2".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.HasOpenWork);
                }


                result = jobApplicationWorkPermitTypes
                   .AsEnumerable()
                   .Select(c => this.FormatDOBPermitsExpiryReport(c, permitCode))
                   .AsQueryable()
                   .ToList();

                #endregion


                string exportFilename = string.Empty;
                if (permitCode.ToUpper() == "TCO")
                {
                    exportFilename = "TCOPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                }
                else if (permitCode.ToUpper() == "AHV")
                {
                    exportFilename = "AHVPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                }
                else if (dataTableParameters.IdJobType == Enums.ApplicationType.DOT.GetHashCode())
                {
                    exportFilename = "DOTPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                }
                else if (dataTableParameters.IdJobType == Enums.ApplicationType.DOB.GetHashCode())
                {
                    exportFilename = "DOBPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                }
                else
                {
                    exportFilename = "DEPPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                }

                string exportFilePath = ExportToExcel(hearingDate, result, permitCode, dataTableParameters.IdJobType, exportFilename);

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

        private string ExportToExcel(string permitExpire, List<PermitsExpiryDTO> result, string permitCode, int? idJobType, string exportFilename)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
            //string exportFilename = string.Empty;
            if (permitCode.ToUpper() == "TCO")
            {
                templateFileName = "TCO_PermitExpirationTemplate.xlsx";
                //exportFilename = "TCOPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
            }
            else if (permitCode.ToUpper() == "AHV")
            {
                // templateFileName = "AHV_PermitExpirationTemplate.xlsx";
                templateFileName = "AHV_PermitExpirationTemplate - Copy.xlsx";
                //exportFilename = "AHVPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
            }
            else if (idJobType == Enums.ApplicationType.DOT.GetHashCode())
            {
                // templateFileName = "DOT_PermitExpirationTemplate.xlsx";
                templateFileName = "DOT_PermitExpirationTemplate - Copy.xlsx";

                //exportFilename = "DOTPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
            }
            else if (idJobType == Enums.ApplicationType.DOB.GetHashCode())
            {
                // templateFileName = "DOB_PermitExpirationTemplate.xlsx";
                templateFileName = "DOB_PermitExpirationTemplate - Copy.xlsx";
                //exportFilename = "DOBPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
            }
            else
            {
                templateFileName = "DEP_PermitExpirationTemplate - Copy.xlsx";

            }

            string templateFilePath = templatePath + templateFileName;
            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateFilePath);
            ISheet sheet = templateWorkbook.GetSheet("Sheet1");

            XSSFFont myFont = (XSSFFont)templateWorkbook.CreateFont();
            myFont.FontHeightInPoints = (short)12;
            myFont.FontName = "Times New Roman";
            myFont.IsBold = false;
            XSSFFont myFont_Bold = (XSSFFont)templateWorkbook.CreateFont();
            myFont_Bold.FontHeightInPoints = (short)12;
            myFont_Bold.FontName = "Times New Roman";
            myFont_Bold.IsBold = true;
            XSSFCellStyle Permitstyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            Permitstyle.SetFont(myFont);
            Permitstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            Permitstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            Permitstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            Permitstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            Permitstyle.WrapText = true;
            Permitstyle.VerticalAlignment = VerticalAlignment.Top;
            Permitstyle.Alignment = HorizontalAlignment.Left;

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
                    iDateRow.GetCell(0).SetCellValue(permitExpire);
                }
                else
                {
                    iDateRow.CreateCell(0).SetCellValue(permitExpire);
                }
            }
            else
            {
                iDateRow = sheet.CreateRow(2);
                if (iDateRow.GetCell(0) != null)
                {
                    iDateRow.GetCell(0).SetCellValue(permitExpire);
                }
                else
                {
                    iDateRow.CreateCell(0).SetCellValue(permitExpire);
                }
            }
            iDateRow.GetCell(0).CellStyle = Permitstyle;

            string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

            if (permitCode.ToUpper() == "TCO")
            {
                #region TCO

                IRow iReportDateRow = sheet.GetRow(3);
                if (iReportDateRow != null)
                {
                    if (iReportDateRow.GetCell(7) != null)
                    {
                        iReportDateRow.GetCell(7).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(7).SetCellValue(reportDate);
                    }
                }
                else
                {
                    iReportDateRow = sheet.CreateRow(3);
                    if (iReportDateRow.GetCell(7) != null)
                    {
                        iReportDateRow.GetCell(7).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(7).SetCellValue(reportDate);
                    }
                }

                int index = 5;
                foreach (PermitsExpiryDTO item in result)
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
                            iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.JobApplicationNumber);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.JobApplicationNumber);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.ConstructionSignedOff != null ? Convert.ToDateTime(item.ConstructionSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.ConstructionSignedOff != null ? Convert.ToDateTime(item.ConstructionSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.TempElevator != null ? Convert.ToDateTime(item.TempElevator).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.TempElevator != null ? Convert.ToDateTime(item.TempElevator).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.FinalElevator != null ? Convert.ToDateTime(item.FinalElevator).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.FinalElevator != null ? Convert.ToDateTime(item.FinalElevator).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.PlumbingSignedOff != null ? Convert.ToDateTime(item.PlumbingSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.PlumbingSignedOff != null ? Convert.ToDateTime(item.PlumbingSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        if (iRow.GetCell(7) != null)
                        {
                            iRow.GetCell(7).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(7).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(7).CellStyle = leftAlignCellStyle;
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
                            iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.JobApplicationNumber);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.JobApplicationNumber);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.ConstructionSignedOff != null ? Convert.ToDateTime(item.ConstructionSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.ConstructionSignedOff != null ? Convert.ToDateTime(item.ConstructionSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.TempElevator != null ? Convert.ToDateTime(item.TempElevator).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.TempElevator != null ? Convert.ToDateTime(item.TempElevator).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.FinalElevator != null ? Convert.ToDateTime(item.FinalElevator).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.FinalElevator != null ? Convert.ToDateTime(item.FinalElevator).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.PlumbingSignedOff != null ? Convert.ToDateTime(item.PlumbingSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.PlumbingSignedOff != null ? Convert.ToDateTime(item.PlumbingSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        if (iRow.GetCell(7) != null)
                        {
                            iRow.GetCell(7).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(7).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(7).CellStyle = leftAlignCellStyle;
                    }

                    index = index + 1;
                }
                #endregion
            }
            else if (permitCode.ToUpper() == "AHV")
            {
                #region AHV

                IRow iReportDateRow = sheet.GetRow(3);
                if (iReportDateRow != null)
                {
                    if (iReportDateRow.GetCell(6) != null)
                    {
                        iReportDateRow.GetCell(6).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(6).SetCellValue(reportDate);
                    }
                }
                else
                {
                    iReportDateRow = sheet.CreateRow(3);
                    if (iReportDateRow.GetCell(6) != null)
                    {
                        iReportDateRow.GetCell(6).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(6).SetCellValue(reportDate);
                    }
                }

                int index = 5;
                foreach (PermitsExpiryDTO item in result)
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
                            iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.JobApplicationTypeName);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.JobApplicationTypeName);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.JobApplicationNumber);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.JobApplicationNumber);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(string.Empty);
                        }

                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(6).CellStyle = leftAlignCellStyle;
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
                            iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.JobApplicationTypeName);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.JobApplicationTypeName);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.JobApplicationNumber);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.JobApplicationNumber);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(string.Empty);
                        }

                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(6).CellStyle = leftAlignCellStyle;

                    }

                    index = index + 1;
                }
                #endregion
            }
            else if (idJobType == Enums.ApplicationType.DOT.GetHashCode())
            {
                #region DOT

                IRow iReportDateRow = sheet.GetRow(3);
                if (iReportDateRow != null)
                {
                    if (iReportDateRow.GetCell(6) != null)
                    {
                        iReportDateRow.GetCell(6).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(6).SetCellValue(reportDate);
                    }
                }
                else
                {
                    iReportDateRow = sheet.CreateRow(3);
                    if (iReportDateRow.GetCell(5) != null)
                    {
                        iReportDateRow.GetCell(6).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(6).SetCellValue(reportDate);
                    }
                }

                int index = 5;
                foreach (PermitsExpiryDTO item in result)
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
                            iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.PermitNumber);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.PermitNumber);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.JobApplicationStreetWorkingOn + " | " + item.JobApplicationStreetFrom + " | " + item.JobApplicationStreetTo);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.JobApplicationStreetWorkingOn + " | " + item.JobApplicationStreetFrom + " | " + item.JobApplicationStreetTo);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.PermitType);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.PermitType);
                        }
                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.Company);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.Company);
                        }
                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(6).CellStyle = leftAlignCellStyle;
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
                            iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.PermitNumber);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.PermitNumber);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.JobApplicationStreetWorkingOn + " | " + item.JobApplicationStreetFrom + " | " + item.JobApplicationStreetTo);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.JobApplicationStreetWorkingOn + " | " + item.JobApplicationStreetFrom + " | " + item.JobApplicationStreetTo);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.PermitType);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.PermitType);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.Company);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.Company);
                        }
                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                    }

                    index = index + 1;
                }
                #endregion
            }
            else if (idJobType == Enums.ApplicationType.DOB.GetHashCode())
            {
                #region DOB

                IRow iReportDateRow = sheet.GetRow(3);
                if (iReportDateRow != null)
                {
                    if (iReportDateRow.GetCell(8) != null)
                    {
                        iReportDateRow.GetCell(8).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(8).SetCellValue(reportDate);
                    }
                }
                else
                {
                    iReportDateRow = sheet.CreateRow(3);
                    if (iReportDateRow.GetCell(8) != null)
                    {
                        iReportDateRow.GetCell(8).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(8).SetCellValue(reportDate);
                    }
                }

                int index = 5;
                foreach (PermitsExpiryDTO item in result)
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
                            iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.JobApplicationTypeName);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.JobApplicationTypeName);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.JobApplicationNumber);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.JobApplicationNumber);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.Permittee);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.Permittee);
                        }

                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        if (iRow.GetCell(7) != null)
                        {
                            iRow.GetCell(7).SetCellValue(item.Responsibility);
                        }
                        else
                        {
                            iRow.CreateCell(7).SetCellValue(item.Responsibility);
                        }
                        if (iRow.GetCell(8) != null)
                        {
                            string ispgl = string.Empty;
                            if (item.IsPGL != null && item.IsPGL == true)
                            {
                                ispgl = "Yes";
                            }
                            else
                            {
                                ispgl = "No";
                            }

                            iRow.GetCell(8).SetCellValue(ispgl);
                        }
                        else
                        {
                            string ispgl = string.Empty;
                            if (item.IsPGL != null && item.IsPGL == true)
                            {
                                ispgl = "Yes";
                            }
                            else
                            {
                                ispgl = "No";
                            }

                            iRow.CreateCell(8).SetCellValue(ispgl);
                        }
                        if (iRow.GetCell(9) != null)
                        {
                            string isl2 = string.Empty;
                            if (item.L2 == true)
                            {
                                isl2 = "Yes";
                            }
                            else
                            {
                                isl2 = "No";
                            }

                            iRow.GetCell(9).SetCellValue(isl2);
                        }
                        else
                        {
                            string isl2 = string.Empty;
                            if (item.L2 == true)
                            {
                                isl2 = "Yes";
                            }
                            else
                            {
                                isl2 = "No";
                            }
                            iRow.CreateCell(9).SetCellValue(isl2);
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
                            iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.JobApplicationTypeName);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.JobApplicationTypeName);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.JobApplicationNumber);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.JobApplicationNumber);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.Permittee);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.Permittee);
                        }

                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        if (iRow.GetCell(7) != null)
                        {
                            iRow.GetCell(7).SetCellValue(item.Responsibility);
                        }
                        else
                        {
                            iRow.CreateCell(7).SetCellValue(item.Responsibility);
                        }
                        if (iRow.GetCell(8) != null)
                        {
                            string ispgl = string.Empty;
                            if (item.IsPGL != null && item.IsPGL == true)
                            {
                                ispgl = "Yes";
                            }
                            else
                            {
                                ispgl = "No";
                            }

                            iRow.GetCell(8).SetCellValue(ispgl);
                        }
                        else
                        {
                            string ispgl = string.Empty;
                            if (item.IsPGL != null && item.IsPGL == true)
                            {
                                ispgl = "Yes";
                            }
                            else
                            {
                                ispgl = "No";
                            }

                            iRow.CreateCell(8).SetCellValue(ispgl);
                        }
                        if (iRow.GetCell(9) != null)
                        {
                            string isl2 = string.Empty;
                            if (item.L2 == true)
                            {
                                isl2 = "Yes";
                            }
                            else
                            {
                                isl2 = "No";
                            }

                            iRow.GetCell(9).SetCellValue(isl2);
                        }
                        else
                        {
                            string isl2 = string.Empty;
                            if (item.L2 == true)
                            {
                                isl2 = "Yes";
                            }
                            else
                            {
                                isl2 = "No";
                            }
                            iRow.CreateCell(9).SetCellValue(isl2);
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
                    }

                    index = index + 1;
                }
                #endregion
            }
            else
            {
                #region DEP

                IRow iReportDateRow = sheet.GetRow(3);
                if (iReportDateRow != null)
                {
                    if (iReportDateRow.GetCell(6) != null)
                    {
                        iReportDateRow.GetCell(6).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(6).SetCellValue(reportDate);
                    }
                }
                else
                {
                    iReportDateRow = sheet.CreateRow(3);
                    if (iReportDateRow.GetCell(6) != null)
                    {
                        iReportDateRow.GetCell(6).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(6).SetCellValue(reportDate);
                    }
                }

                int index = 5;
                foreach (PermitsExpiryDTO item in result)
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
                            iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.JobApplicationTypeName);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.JobApplicationTypeName);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.JobApplicationStreetWorkingOn + "|" + item.JobApplicationStreetFrom + "|" + item.JobApplicationStreetTo);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.JobApplicationStreetWorkingOn + "|" + item.JobApplicationStreetFrom + "|" + item.JobApplicationStreetTo);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.PermitNumber);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.PermitNumber);
                        }

                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }


                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(6).CellStyle = leftAlignCellStyle;

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
                            iRow.GetCell(1).SetCellValue(item.SpecialPlace);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SpecialPlace);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.JobApplicationTypeName);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.JobApplicationTypeName);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.JobApplicationStreetWorkingOn + "|" + item.JobApplicationStreetFrom + "|" + item.JobApplicationStreetTo);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.JobApplicationStreetWorkingOn + "|" + item.JobApplicationStreetFrom + "|" + item.JobApplicationStreetTo);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.JobWorkTypeDescription);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.PermitNumber);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.PermitNumber);
                        }

                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(6).CellStyle = leftAlignCellStyle;

                    }

                    index = index + 1;
                }
                #endregion
            }

            using (var file2 = new FileStream(exportFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                templateWorkbook.Write(file2);
                file2.Close();
            }

            return exportFilePath;
        }
        /// <summary>
        /// Get the report of ReportPermitExpiry Report with filter and sorting  and export to pdf
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportPermitsExpiry/exporttopdf")]
        public IHttpActionResult PostExportPermitsExpiryToPdf(PermitsExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                #region Filter

                if (dataTableParameters != null && dataTableParameters.PermitCode == "DOB")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DOT")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOT.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DEP")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DEP.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "AHV")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "TCO")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }

                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                    .Include("JobApplication.JobApplicationType")
                                                    .Include("JobApplication.ApplicationStatus")
                                                    .Include("JobApplication.Job.RfpAddress.Borough")
                                                    .Include("JobApplication.Job.Company")
                                                    .Include("JobApplication.Job.Contact")
                                                    .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                    .Where(x =>
                                                               x.JobApplication.JobApplicationType.IdParent == dataTableParameters.IdJobType
                                                               && ((DbFunctions.TruncateTime(x.Expires) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
                                                               && DbFunctions.TruncateTime(x.Expires) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
                                                               ))
                                                    .AsQueryable();

                var recordsTotal = jobApplicationWorkPermitTypes.Count();
                var recordsFiltered = recordsTotal;
                string permitCode = string.Empty;
                switch (dataTableParameters.IdJobType)
                {
                    case 1:
                        permitCode = "DOB";
                        break;
                    case 2:
                        permitCode = "DOT";
                        break;
                    case 4:
                        permitCode = "DEP";
                        break;
                }

                if (dataTableParameters.IdJobType == 1)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.SignOff == false);
                }

                string hearingDate = string.Empty;

                string permitExpire = string.Empty;
                if (dataTableParameters != null && dataTableParameters.ExpiresFromDate != null)
                {
                    permitExpire = permitExpire + Convert.ToDateTime(dataTableParameters.ExpiresFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.ExpiresToDate != null)
                {
                    permitExpire = permitExpire + " - " + Convert.ToDateTime(dataTableParameters.ExpiresToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(permitExpire) ? " | " : string.Empty) + (!string.IsNullOrEmpty(permitExpire) ? "Permit Expires : " + permitExpire : string.Empty);

                if (!string.IsNullOrEmpty(dataTableParameters.IdBorough))
                {
                    List<int> boroughs = dataTableParameters.IdBorough != null && !string.IsNullOrEmpty(dataTableParameters.IdBorough) ? (dataTableParameters.IdBorough.Split('-') != null && dataTableParameters.IdBorough.Split('-').Any() ? dataTableParameters.IdBorough.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => boroughs.Contains(x.JobApplication.Job.RfpAddress.IdBorough ?? 0));

                    string filteredBoroughes = string.Join(", ", rpoContext.Boroughes.Where(x => boroughs.Contains(x.Id)).Select(x => x.Description));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredBoroughes) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredBoroughes) ? "Borough : " + filteredBoroughes : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
                {
                    List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.JobApplication.Job.IdProjectManager ?? 0));

                    string filteredProjectManagers = string.Join(", ", rpoContext.Employees.Where(x => projectManagers.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredProjectManagers) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredProjectManagers) ? "Project Managers : " + filteredProjectManagers : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.JobApplication.IdJob));

                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.PermitCode != null)
                {
                    permitCode = dataTableParameters.PermitCode;
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code == dataTableParameters.PermitCode);
                }
                else
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code != "AHV" && x.JobWorkType.Code != "TCO");
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.Job.Status == dataTableParameters.Status);

                    string filteredStatus = dataTableParameters.Status.ToString();
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredStatus) ? "Status : " + filteredStatus : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.JobApplication.Job.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.JobApplication.Job.IdContact.Value)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }
                if (!string.IsNullOrEmpty(dataTableParameters.IdResponsibility))
                {
                    List<int> jobResponsibilityTeam = dataTableParameters.IdResponsibility != null && !string.IsNullOrEmpty(dataTableParameters.IdResponsibility) ? (dataTableParameters.IdResponsibility.Split('-') != null && dataTableParameters.IdResponsibility.Split('-').Any() ? dataTableParameters.IdResponsibility.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobResponsibilityTeam.Contains((x.IdResponsibility.Value)));
                }
                string direction = string.Empty;
                string orderBy = string.Empty;

                List<PermitsExpiryDTO> result = new List<PermitsExpiryDTO>();

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
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.HouseNumber);
                    if (orderBy.Trim().ToLower() == "Apartment".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Apartment);
                    if (orderBy.Trim().ToLower() == "Borough".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Borough.Description);
                    if (orderBy.Trim().ToLower() == "Client".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Contact.FirstName);
                    if (orderBy.Trim().ToLower() == "FloorNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.FloorNumber);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationTypeName".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.JobApplicationType.Description);
                    if (orderBy.Trim().ToLower() == "JobApplicationNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.ApplicationNumber);
                    if (orderBy.Trim().ToLower() == "PermitType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.PermitType);
                    if (orderBy.Trim().ToLower() == "Permittee".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Permittee);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "IsPGL".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.IsPGL);
                    if (orderBy.Trim().ToLower() == "L2".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.HasOpenWork);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.HouseNumber);
                    if (orderBy.Trim().ToLower() == "Apartment".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Apartment);
                    if (orderBy.Trim().ToLower() == "Borough".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Borough.Description);
                    if (orderBy.Trim().ToLower() == "Client".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Contact.FirstName);
                    if (orderBy.Trim().ToLower() == "FloorNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.FloorNumber);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationTypeName".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.JobApplicationType.Description);
                    if (orderBy.Trim().ToLower() == "JobApplicationNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.ApplicationNumber);
                    if (orderBy.Trim().ToLower() == "PermitType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.PermitType);
                    if (orderBy.Trim().ToLower() == "Permittee".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Permittee);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "IsPGL".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.IsPGL);
                    if (orderBy.Trim().ToLower() == "L2".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.HasOpenWork);
                }


                result = jobApplicationWorkPermitTypes
                   .AsEnumerable()
                   .Select(c => this.FormatDOBPermitsExpiryReport(c, permitCode))
                   .AsQueryable()
                   .ToList();

                #endregion

                string exportFilename = string.Empty;
                string exportFilePath = string.Empty;

                if (permitCode.ToUpper() == "TCO")
                {
                    exportFilename = "TCOPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf_TCO(hearingDate, result, permitCode, exportFilename);
                }
                else if (permitCode.ToUpper() == "AHV")
                {
                    exportFilename = "AHVPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf_AHV(hearingDate, result, permitCode, exportFilename);
                }
                else if (dataTableParameters.IdJobType == Enums.ApplicationType.DOT.GetHashCode())
                {
                    exportFilename = "DOTPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf_DOT(hearingDate, result, permitCode, exportFilename);
                }
                else if (dataTableParameters.IdJobType == Enums.ApplicationType.DOB.GetHashCode())
                {
                    exportFilename = "DOBPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf_DOB(hearingDate, result, permitCode, exportFilename);
                }
                else
                {
                    exportFilename = "DEPPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf_DEP(hearingDate, result, permitCode, exportFilename);
                }

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

        private string ExportToPdf_DOB(string permitExpire, List<PermitsExpiryDTO> result, string permitCode, string exportFilename)
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

                PdfPTable table = new PdfPTable(8);
                table.WidthPercentage = 100;
                table.SplitLate = false;

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
                cell.PaddingLeft = -35;
                cell.Colspan = 8;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -35;
                cell.Colspan = 8;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = -10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("DOB PERMIT EXPIRATION REPORT", font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 8;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(permitExpire, font_12_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 8;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 7;
                table.AddCell(cell);

                string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

                cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project # | Address", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Special Place Name", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Application Type", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Application #", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Work Type", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Permittee", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Responsibility", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                foreach (PermitsExpiryDTO item in result)
                {
                    cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.JobAddress, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.SpecialPlace, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.JobApplicationTypeName, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.JobApplicationNumber, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    string workDescription = item.JobWorkTypeDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);

                    cell = new PdfPCell(new Phrase(workDescription, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Permittee, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Responsibility, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);
                }

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }

        private string ExportToPdf_DOT(string permitExpire, List<PermitsExpiryDTO> result, string permitCode, string exportFilename)
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
                document.SetMargins(18, 18, 12, 16);
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

                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SplitLate = false;

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
                cell.PaddingLeft = -50;
                cell.Colspan = 7;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -50;
                cell.Colspan = 7;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = -10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("DOT PERMIT EXPIRATION REPORT", font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 7;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(permitExpire, font_12_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 7;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 6;
                table.AddCell(cell);

                string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

                cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project # | Address", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Special Place Name", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Permit #", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Street On | From | To", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Work Type | Permit Type", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Company Name", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);


                foreach (PermitsExpiryDTO item in result)
                {
                    cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.JobAddress, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.SpecialPlace, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.PermitNumber, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.JobApplicationStreetWorkingOn + " | " + item.JobApplicationStreetFrom + " | " + item.JobApplicationStreetTo, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    string workDescription = item.PermitType + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);

                    cell = new PdfPCell(new Phrase(workDescription, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Company, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);
                }

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }

        private string ExportToPdf_DEP(string permitExpire, List<PermitsExpiryDTO> result, string permitCode, string exportFilename)
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
                document.SetMargins(18, 18, 12, 16);
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

                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SplitLate = false;

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
                cell.PaddingLeft = -50;
                cell.Colspan = 7;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -50;
                cell.Colspan = 7;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = -10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("DEP PERMIT EXPIRATION REPORT", font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 7;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(permitExpire, font_12_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 7;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 6;
                table.AddCell(cell);

                string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

                cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project # | Address", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Special Place Name", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Application Type", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("On | From |To", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Work Type", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Permit#", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);


                foreach (PermitsExpiryDTO item in result)
                {
                    cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.JobAddress, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.SpecialPlace, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.JobApplicationTypeName, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.JobApplicationStreetWorkingOn + "|" + item.JobApplicationStreetFrom + "|" + item.JobApplicationStreetTo, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    string workDescription = item.JobWorkTypeDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);

                    cell = new PdfPCell(new Phrase(workDescription, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.PermitNumber, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);
                }

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }

        private string ExportToPdf_AHV(string permitExpire, List<PermitsExpiryDTO> result, string permitCode, string exportFilename)
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
                document.SetMargins(18, 18, 12, 16);
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

                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SplitLate = false;

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
                cell.PaddingLeft = -50;
                cell.Colspan = 7;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -50;
                cell.Colspan = 7;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = -10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("AHV PERMIT EXPIRATION REPORT", font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 7;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(permitExpire, font_12_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 7;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 6;
                table.AddCell(cell);

                string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

                cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project # | Address", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Special Place Name", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Application Type", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Application #", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Work Type", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Permittee", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);


                foreach (PermitsExpiryDTO item in result)
                {
                    cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.JobAddress, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.SpecialPlace, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.JobApplicationTypeName, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.JobApplicationNumber, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    string workDescription = item.JobWorkTypeDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);

                    cell = new PdfPCell(new Phrase(workDescription, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Permittee, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);
                }

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }

        private string ExportToPdf_TCO(string permitExpire, List<PermitsExpiryDTO> result, string permitCode, string exportFilename)
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
                document.SetMargins(18, 18, 12, 16);
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

                PdfPTable table = new PdfPTable(8);
                table.WidthPercentage = 100;
                table.SplitLate = false;

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
                cell.PaddingLeft = -35;
                cell.Colspan = 8;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -35;
                cell.Colspan = 8;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = -10;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("TCO PERMIT EXPIRATION REPORT", font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 8;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(permitExpire, font_12_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 8;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 7;
                table.AddCell(cell);

                string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

                cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project # | Address", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Special Place Name", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Application #", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Construction s/o", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Temp. Elevator", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Final Elevator", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Plumbing s/o", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("TCO expiration", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                foreach (PermitsExpiryDTO item in result)
                {
                    cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.JobAddress, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.SpecialPlace, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.JobApplicationNumber, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.ConstructionSignedOff != null ? Convert.ToDateTime(item.ConstructionSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.TempElevator != null ? Convert.ToDateTime(item.TempElevator).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.FinalElevator != null ? Convert.ToDateTime(item.FinalElevator).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.PlumbingSignedOff != null ? Convert.ToDateTime(item.PlumbingSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);
                }

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }

        private PermitsExpiryDTO FormatDOBPermitsExpiryReport(JobApplicationWorkPermitType jobApplicationWorkPermitResponse, string permitCode, int customerid)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            string JobApplicationType = string.Empty;

            if (jobApplicationWorkPermitResponse.JobApplication.Job != null && jobApplicationWorkPermitResponse.JobApplication.IdJob != null)
            {
                JobApplicationType = string.Join(",", jobApplicationWorkPermitResponse.JobApplication.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }
            var CustomerJobAccess = rpoContext.CustomerJobAccess.Where(x => x.IdJob == jobApplicationWorkPermitResponse.JobApplication.IdJob && x.IdCustomer == customerid).FirstOrDefault();
            var customerJobNames = rpoContext.CustomerJobNames.Where(x => x.IdCustomerJobAccess == CustomerJobAccess.Id).FirstOrDefault();
            string projectName = "";
            if (customerJobNames != null)
                projectName = customerJobNames.ProjectName;
            return new PermitsExpiryDTO
            {
                Id = jobApplicationWorkPermitResponse.Id,
                IdJobApplication = jobApplicationWorkPermitResponse.IdJobApplication,
                JobApplicationType = JobApplicationType,
                IdJob = jobApplicationWorkPermitResponse.JobApplication.IdJob,
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
                Withdrawn = jobApplicationWorkPermitResponse.Withdrawn != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Withdrawn), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Withdrawn,
                Filed = jobApplicationWorkPermitResponse.Filed != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Filed), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Filed,
                Issued = jobApplicationWorkPermitResponse.Issued != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Issued), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Issued,
                Expires = jobApplicationWorkPermitResponse.Expires != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Expires), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Expires,
                SignedOff = jobApplicationWorkPermitResponse.SignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.SignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.SignedOff,
                //Withdrawn = jobApplicationWorkPermitResponse.Withdrawn != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Withdrawn), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Withdrawn,
                //Filed = jobApplicationWorkPermitResponse.Filed != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Filed), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Filed,
                //Issued = jobApplicationWorkPermitResponse.Issued != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Issued), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Issued,
                //Expires = jobApplicationWorkPermitResponse.Expires != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.Expires), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.Expires,
                //SignedOff = jobApplicationWorkPermitResponse.SignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.SignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.SignedOff,
                WorkDescription = jobApplicationWorkPermitResponse.WorkDescription,
                IdResponsibility = jobApplicationWorkPermitResponse.IdResponsibility,
                Responsibility = jobApplicationWorkPermitResponse.IdResponsibility == 2 ? "Other" : jobApplicationWorkPermitResponse.IdResponsibility == 1 ? "RPO" : string.Empty,
                DocumentPath = jobApplicationWorkPermitResponse.DocumentPath != null ? APIUrl + "/" + Properties.Settings.Default.DOTWorkPermitDocument + "/" + jobApplicationWorkPermitResponse.Id + "_" + jobApplicationWorkPermitResponse.DocumentPath : string.Empty,
                LastModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                CompanyResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.Company != null ? jobApplicationWorkPermitResponse.ContactResponsible.Company.Name : string.Empty,
                PersonalResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.FirstName != null ? jobApplicationWorkPermitResponse.ContactResponsible.FirstName + (jobApplicationWorkPermitResponse.ContactResponsible.LastName != null ? " " + jobApplicationWorkPermitResponse.ContactResponsible.LastName : string.Empty) : string.Empty,
                PermitType = jobApplicationWorkPermitResponse.PermitType,
                ForPurposeOf = jobApplicationWorkPermitResponse.ForPurposeOf,
                EquipmentType = jobApplicationWorkPermitResponse.EquipmentType,
                PermitCode = permitCode,
                JobApplicationStreetWorkingOn = jobApplicationWorkPermitResponse.JobApplication.StreetWorkingOn,
                JobApplicationStreetFrom = jobApplicationWorkPermitResponse.JobApplication.StreetFrom,
                JobApplicationStreetTo = jobApplicationWorkPermitResponse.JobApplication.StreetTo,
                PlumbingSignedOff = jobApplicationWorkPermitResponse.PlumbingSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.PlumbingSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.PlumbingSignedOff,
                FinalElevator = jobApplicationWorkPermitResponse.FinalElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.FinalElevator), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.FinalElevator,
                TempElevator = jobApplicationWorkPermitResponse.TempElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.TempElevator), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.TempElevator,
                ConstructionSignedOff = jobApplicationWorkPermitResponse.ConstructionSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.ConstructionSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.ConstructionSignedOff,
                Permittee = jobApplicationWorkPermitResponse.Permittee,
                IsPGL = jobApplicationWorkPermitResponse.IsPGL,
                L2 = jobApplicationWorkPermitResponse.JobApplication.Job.HasOpenWork,
                ProjectName = projectName

            };
        }
        private PermitsExpiryDTO FormatDOBPermitsExpiryReport(JobApplicationWorkPermitType jobApplicationWorkPermitResponse, string permitCode)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

            string JobApplicationType = string.Empty;

            if (jobApplicationWorkPermitResponse.JobApplication.Job != null && jobApplicationWorkPermitResponse.JobApplication.IdJob != null)
            {
                JobApplicationType = string.Join(",", jobApplicationWorkPermitResponse.JobApplication.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }
            //rpoContext.CustomerJobAccess.Where(x=>x.IdJob== jobApplicationWorkPermitResponse.JobApplication.IdJob && x.IdCustomer == customerid).FirstOrDefault()
            // string projectName = rpoContext.CustomerJobNames.Where(x => x.IdJob == jobApplicationWorkPermitResponse.JobApplication.IdJob && x.IdCustomer == customerid).FirstOrDefault().ProjectName;
            return new PermitsExpiryDTO
            {
                Id = jobApplicationWorkPermitResponse.Id,
                IdJobApplication = jobApplicationWorkPermitResponse.IdJobApplication,
                JobApplicationType = JobApplicationType,
                IdJob = jobApplicationWorkPermitResponse.JobApplication.IdJob,
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
                Withdrawn = jobApplicationWorkPermitResponse.Withdrawn != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Withdrawn), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Withdrawn,
                Filed = jobApplicationWorkPermitResponse.Filed != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Filed), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Filed,
                Issued = jobApplicationWorkPermitResponse.Issued != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Issued), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Issued,
                Expires = jobApplicationWorkPermitResponse.Expires != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.Expires), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.Expires,
                SignedOff = jobApplicationWorkPermitResponse.SignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.SignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.SignedOff,
                WorkDescription = jobApplicationWorkPermitResponse.WorkDescription,
                IdResponsibility = jobApplicationWorkPermitResponse.IdResponsibility,
                Responsibility = jobApplicationWorkPermitResponse.IdResponsibility == 2 ? "Other" : jobApplicationWorkPermitResponse.IdResponsibility == 1 ? "RPO" : string.Empty,
                DocumentPath = jobApplicationWorkPermitResponse.DocumentPath != null ? APIUrl + "/" + Properties.Settings.Default.DOTWorkPermitDocument + "/" + jobApplicationWorkPermitResponse.Id + "_" + jobApplicationWorkPermitResponse.DocumentPath : string.Empty,
                LastModifiedDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                CompanyResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.Company != null ? jobApplicationWorkPermitResponse.ContactResponsible.Company.Name : string.Empty,
                PersonalResponsible = jobApplicationWorkPermitResponse.ContactResponsible != null && jobApplicationWorkPermitResponse.ContactResponsible.FirstName != null ? jobApplicationWorkPermitResponse.ContactResponsible.FirstName + (jobApplicationWorkPermitResponse.ContactResponsible.LastName != null ? " " + jobApplicationWorkPermitResponse.ContactResponsible.LastName : string.Empty) : string.Empty,
                PermitType = jobApplicationWorkPermitResponse.PermitType,
                ForPurposeOf = jobApplicationWorkPermitResponse.ForPurposeOf,
                EquipmentType = jobApplicationWorkPermitResponse.EquipmentType,
                PermitCode = permitCode,
                JobApplicationStreetWorkingOn = jobApplicationWorkPermitResponse.JobApplication.StreetWorkingOn,
                JobApplicationStreetFrom = jobApplicationWorkPermitResponse.JobApplication.StreetFrom,
                JobApplicationStreetTo = jobApplicationWorkPermitResponse.JobApplication.StreetTo,
                PlumbingSignedOff = jobApplicationWorkPermitResponse.PlumbingSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.PlumbingSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.PlumbingSignedOff,
                FinalElevator = jobApplicationWorkPermitResponse.FinalElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.FinalElevator), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.FinalElevator,
                TempElevator = jobApplicationWorkPermitResponse.TempElevator != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.TempElevator), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.TempElevator,
                ConstructionSignedOff = jobApplicationWorkPermitResponse.ConstructionSignedOff != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobApplicationWorkPermitResponse.ConstructionSignedOff), DateTimeKind.Utc) : jobApplicationWorkPermitResponse.ConstructionSignedOff,

                Permittee = jobApplicationWorkPermitResponse.Permittee,
                IsPGL = jobApplicationWorkPermitResponse.IsPGL,
                L2 = jobApplicationWorkPermitResponse.JobApplication.Job.HasOpenWork,


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

        /// <summary>
        /// Get the report of ReportPermitExpiry Report with filter and sorting and export and send email
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportPermitsExpiry/exporttoexcelemail")]
        public IHttpActionResult PostExportPermitsExpiryToExcelEmail(PermitsExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                #region Filter

                if (dataTableParameters != null && dataTableParameters.PermitCode == "DOB")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DOT")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOT.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DEP")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DEP.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "AHV")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "TCO")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }

                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                    .Include("JobApplication.JobApplicationType")
                                                    .Include("JobApplication.ApplicationStatus")
                                                    .Include("JobApplication.Job.RfpAddress.Borough")
                                                    .Include("JobApplication.Job.Company")
                                                    .Include("JobApplication.Job.Contact")
                                                    .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                    .Where(x =>
                                                               x.JobApplication.JobApplicationType.IdParent == dataTableParameters.IdJobType
                                                               && ((DbFunctions.TruncateTime(x.Expires) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
                                                               && DbFunctions.TruncateTime(x.Expires) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
                                                               ))
                                                    .AsQueryable();

                var recordsTotal = jobApplicationWorkPermitTypes.Count();
                var recordsFiltered = recordsTotal;
                string permitCode = string.Empty;
                switch (dataTableParameters.IdJobType)
                {
                    case 1:
                        permitCode = "DOB";
                        break;
                    case 2:
                        permitCode = "DOT";
                        break;
                    case 4:
                        permitCode = "DEP";
                        break;
                }


                if (dataTableParameters.IdJobType == 1)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.SignOff == false);
                }
                string hearingDate = string.Empty;

                string permitExpire = string.Empty;
                if (dataTableParameters != null && dataTableParameters.ExpiresFromDate != null)
                {
                    permitExpire = permitExpire + Convert.ToDateTime(dataTableParameters.ExpiresFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.ExpiresToDate != null)
                {
                    permitExpire = permitExpire + " - " + Convert.ToDateTime(dataTableParameters.ExpiresToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(permitExpire) ? " | " : string.Empty) + (!string.IsNullOrEmpty(permitExpire) ? "Permit Expires : " + permitExpire : string.Empty);

                if (!string.IsNullOrEmpty(dataTableParameters.IdBorough))
                {
                    List<int> boroughs = dataTableParameters.IdBorough != null && !string.IsNullOrEmpty(dataTableParameters.IdBorough) ? (dataTableParameters.IdBorough.Split('-') != null && dataTableParameters.IdBorough.Split('-').Any() ? dataTableParameters.IdBorough.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => boroughs.Contains(x.JobApplication.Job.RfpAddress.IdBorough ?? 0));

                    string filteredBoroughes = string.Join(", ", rpoContext.Boroughes.Where(x => boroughs.Contains(x.Id)).Select(x => x.Description));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredBoroughes) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredBoroughes) ? "Borough : " + filteredBoroughes : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
                {
                    List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.JobApplication.Job.IdProjectManager ?? 0));

                    string filteredProjectManagers = string.Join(", ", rpoContext.Employees.Where(x => projectManagers.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredProjectManagers) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredProjectManagers) ? "Project Managers : " + filteredProjectManagers : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.JobApplication.IdJob));

                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.PermitCode != null)
                {
                    permitCode = dataTableParameters.PermitCode;
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code == dataTableParameters.PermitCode);
                }
                else
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code != "AHV" && x.JobWorkType.Code != "TCO");
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.Job.Status == dataTableParameters.Status);

                    string filteredStatus = dataTableParameters.Status.ToString();
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredStatus) ? "Status : " + filteredStatus : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.JobApplication.Job.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.JobApplication.Job.IdContact.Value)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }
                if (!string.IsNullOrEmpty(dataTableParameters.IdResponsibility))
                {
                    List<int> jobResponsibilityTeam = dataTableParameters.IdResponsibility != null && !string.IsNullOrEmpty(dataTableParameters.IdResponsibility) ? (dataTableParameters.IdResponsibility.Split('-') != null && dataTableParameters.IdResponsibility.Split('-').Any() ? dataTableParameters.IdResponsibility.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobResponsibilityTeam.Contains((x.IdResponsibility.Value)));
                }

                string direction = string.Empty;
                string orderBy = string.Empty;

                List<PermitsExpiryDTO> result = new List<PermitsExpiryDTO>();

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
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.HouseNumber);
                    if (orderBy.Trim().ToLower() == "Apartment".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Apartment);
                    if (orderBy.Trim().ToLower() == "Borough".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Borough.Description);
                    if (orderBy.Trim().ToLower() == "Client".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Contact.FirstName);
                    if (orderBy.Trim().ToLower() == "FloorNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.FloorNumber);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationTypeName".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.JobApplicationType.Description);
                    if (orderBy.Trim().ToLower() == "JobApplicationNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.ApplicationNumber);
                    if (orderBy.Trim().ToLower() == "PermitType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.PermitType);
                    if (orderBy.Trim().ToLower() == "Permittee".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Permittee);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "IsPGL".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.IsPGL);
                    if (orderBy.Trim().ToLower() == "L2".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.HasOpenWork);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.HouseNumber);
                    if (orderBy.Trim().ToLower() == "Apartment".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Apartment);
                    if (orderBy.Trim().ToLower() == "Borough".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Borough.Description);
                    if (orderBy.Trim().ToLower() == "Client".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Contact.FirstName);
                    if (orderBy.Trim().ToLower() == "FloorNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.FloorNumber);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationTypeName".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.JobApplicationType.Description);
                    if (orderBy.Trim().ToLower() == "JobApplicationNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.ApplicationNumber);
                    if (orderBy.Trim().ToLower() == "PermitType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.PermitType);
                    if (orderBy.Trim().ToLower() == "Permittee".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Permittee);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "IsPGL".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.IsPGL);
                    if (orderBy.Trim().ToLower() == "L2".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.HasOpenWork);
                }


                result = jobApplicationWorkPermitTypes
                   .AsEnumerable()
                   .Select(c => this.FormatDOBPermitsExpiryReport(c, permitCode))
                   .AsQueryable()
                   .ToList();

                #endregion


                string exportFilename = string.Empty;
                if (permitCode.ToUpper() == "TCO")
                {
                    exportFilename = "TCOPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                }
                else if (permitCode.ToUpper() == "AHV")
                {
                    exportFilename = "AHVPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                }
                else if (dataTableParameters.IdJobType == Enums.ApplicationType.DOT.GetHashCode())
                {
                    exportFilename = "DOTPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                }
                else if (dataTableParameters.IdJobType == Enums.ApplicationType.DOB.GetHashCode())
                {
                    exportFilename = "DOBPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                }
                else
                {
                    exportFilename = "DEPPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                }

                string exportFilePath = ExportToExcel(hearingDate, result, permitCode, dataTableParameters.IdJobType, exportFilename);

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
        /// /Get the report of ReportPermitExpiry Report with filter and sorting and export it and send email
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportPermitsExpiry/exporttopdfemail")]
        public IHttpActionResult PostExportPermitsExpiryToPdfEmail(PermitsExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                #region Filter

                if (dataTableParameters != null && dataTableParameters.PermitCode == "DOB")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DOT")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOT.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DEP")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DEP.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "AHV")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "TCO")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }

                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                    .Include("JobApplication.JobApplicationType")
                                                    .Include("JobApplication.ApplicationStatus")
                                                    .Include("JobApplication.Job.RfpAddress.Borough")
                                                    .Include("JobApplication.Job.Company")
                                                    .Include("JobApplication.Job.Contact")
                                                    .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                    .Where(x =>
                                                               x.JobApplication.JobApplicationType.IdParent == dataTableParameters.IdJobType
                                                               && ((DbFunctions.TruncateTime(x.Expires) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
                                                               && DbFunctions.TruncateTime(x.Expires) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
                                                               ))
                                                    .AsQueryable();

                var recordsTotal = jobApplicationWorkPermitTypes.Count();
                var recordsFiltered = recordsTotal;
                string permitCode = string.Empty;
                switch (dataTableParameters.IdJobType)
                {
                    case 1:
                        permitCode = "DOB";
                        break;
                    case 2:
                        permitCode = "DOT";
                        break;
                    case 4:
                        permitCode = "DEP";
                        break;
                }

                if (dataTableParameters.IdJobType == 1)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.SignOff == false);
                }

                string hearingDate = string.Empty;

                string permitExpire = string.Empty;
                if (dataTableParameters != null && dataTableParameters.ExpiresFromDate != null)
                {
                    permitExpire = permitExpire + Convert.ToDateTime(dataTableParameters.ExpiresFromDate).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.ExpiresToDate != null)
                {
                    permitExpire = permitExpire + " - " + Convert.ToDateTime(dataTableParameters.ExpiresToDate).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(permitExpire) ? " | " : string.Empty) + (!string.IsNullOrEmpty(permitExpire) ? "Permit Expires : " + permitExpire : string.Empty);

                if (!string.IsNullOrEmpty(dataTableParameters.IdBorough))
                {
                    List<int> boroughs = dataTableParameters.IdBorough != null && !string.IsNullOrEmpty(dataTableParameters.IdBorough) ? (dataTableParameters.IdBorough.Split('-') != null && dataTableParameters.IdBorough.Split('-').Any() ? dataTableParameters.IdBorough.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => boroughs.Contains(x.JobApplication.Job.RfpAddress.IdBorough ?? 0));

                    string filteredBoroughes = string.Join(", ", rpoContext.Boroughes.Where(x => boroughs.Contains(x.Id)).Select(x => x.Description));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredBoroughes) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredBoroughes) ? "Borough : " + filteredBoroughes : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
                {
                    List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.JobApplication.Job.IdProjectManager ?? 0));

                    string filteredProjectManagers = string.Join(", ", rpoContext.Employees.Where(x => projectManagers.Contains(x.Id)).Select(x => x.FirstName + " " + x.LastName));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredProjectManagers) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredProjectManagers) ? "Project Managers : " + filteredProjectManagers : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.JobApplication.IdJob));

                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.PermitCode != null)
                {
                    permitCode = dataTableParameters.PermitCode;
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code == dataTableParameters.PermitCode);
                }
                else
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code != "AHV" && x.JobWorkType.Code != "TCO");
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.Job.Status == dataTableParameters.Status);

                    string filteredStatus = dataTableParameters.Status.ToString();
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredStatus) ? "Status : " + filteredStatus : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.JobApplication.Job.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.JobApplication.Job.IdContact.Value)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }
                if (!string.IsNullOrEmpty(dataTableParameters.IdResponsibility))
                {
                    List<int> jobResponsibilityTeam = dataTableParameters.IdResponsibility != null && !string.IsNullOrEmpty(dataTableParameters.IdResponsibility) ? (dataTableParameters.IdResponsibility.Split('-') != null && dataTableParameters.IdResponsibility.Split('-').Any() ? dataTableParameters.IdResponsibility.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobResponsibilityTeam.Contains((x.IdResponsibility.Value)));
                }

                string direction = string.Empty;
                string orderBy = string.Empty;

                List<PermitsExpiryDTO> result = new List<PermitsExpiryDTO>();

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
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.HouseNumber);
                    if (orderBy.Trim().ToLower() == "Apartment".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Apartment);
                    if (orderBy.Trim().ToLower() == "Borough".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Borough.Description);
                    if (orderBy.Trim().ToLower() == "Client".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.Contact.FirstName);
                    if (orderBy.Trim().ToLower() == "FloorNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.FloorNumber);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationTypeName".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.JobApplicationType.Description);
                    if (orderBy.Trim().ToLower() == "JobApplicationNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.ApplicationNumber);
                    if (orderBy.Trim().ToLower() == "PermitType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.PermitType);
                    if (orderBy.Trim().ToLower() == "Permittee".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Permittee);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "IsPGL".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.IsPGL);
                    if (orderBy.Trim().ToLower() == "L2".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderByDescending(o => o.JobApplication.Job.HasOpenWork);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.IdJob);
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.HouseNumber);
                    if (orderBy.Trim().ToLower() == "Apartment".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Apartment);
                    if (orderBy.Trim().ToLower() == "Borough".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Borough.Description);
                    if (orderBy.Trim().ToLower() == "Client".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.Contact.FirstName);
                    if (orderBy.Trim().ToLower() == "FloorNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.FloorNumber);
                    if (orderBy.Trim().ToLower() == "SpecialPlace".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.SpecialPlace);
                    if (orderBy.Trim().ToLower() == "JobApplicationTypeName".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.JobApplicationType.Description);
                    if (orderBy.Trim().ToLower() == "JobApplicationNumber".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.ApplicationNumber);
                    if (orderBy.Trim().ToLower() == "PermitType".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.PermitType);
                    if (orderBy.Trim().ToLower() == "Permittee".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Permittee);
                    if (orderBy.Trim().ToLower() == "Expires".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Expires);
                    if (orderBy.Trim().ToLower() == "Issued".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.Issued);
                    if (orderBy.Trim().ToLower() == "IsPGL".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.IsPGL);
                    if (orderBy.Trim().ToLower() == "L2".Trim().ToLower())
                        jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.OrderBy(o => o.JobApplication.Job.HasOpenWork);
                }


                result = jobApplicationWorkPermitTypes
                   .AsEnumerable()
                   .Select(c => this.FormatDOBPermitsExpiryReport(c, permitCode))
                   .AsQueryable()
                   .ToList();

                #endregion

                string exportFilename = string.Empty;
                string exportFilePath = string.Empty;

                if (permitCode.ToUpper() == "TCO")
                {
                    exportFilename = "TCOPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf_TCO(hearingDate, result, permitCode, exportFilename);
                }
                else if (permitCode.ToUpper() == "AHV")
                {
                    exportFilename = "AHVPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf_AHV(hearingDate, result, permitCode, exportFilename);
                }
                else if (dataTableParameters.IdJobType == Enums.ApplicationType.DOT.GetHashCode())
                {
                    exportFilename = "DOTPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf_DOT(hearingDate, result, permitCode, exportFilename);
                }
                else if (dataTableParameters.IdJobType == Enums.ApplicationType.DOB.GetHashCode())
                {
                    exportFilename = "DOBPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf_DOB(hearingDate, result, permitCode, exportFilename);
                }
                else
                {
                    exportFilename = "DEPPermitExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf_DEP(hearingDate, result, permitCode, exportFilename);
                }

                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

                string path = APIUrl + "/UploadedDocuments/ReportExportToExcel/" + exportFilename;

                return Ok(new { Filepath = exportFilePath, NewPath = path, ReportName = exportFilename });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        [Route("api/GetCustomerReportPermitsExpiry")]
        public IHttpActionResult GetCustomerReportPermitsExpiry([FromUri] PermitsExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewPermitExpiryReport))
            {
                if (dataTableParameters != null && dataTableParameters.PermitCode == "DOB")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DOT")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOT.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "DEP")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DEP.GetHashCode();
                    dataTableParameters.PermitCode = null;
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "AHV")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }
                else if (dataTableParameters != null && dataTableParameters.PermitCode == "TCO")
                {
                    dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
                }
                var CustomerJobAccess = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == employee.Id).Select(y => y.IdJob).ToList();
                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                .Include("JobApplication.JobApplicationType")
                                                .Include("JobApplication.ApplicationStatus")
                                                .Include("JobApplication.Job.RfpAddress.Borough")
                                                .Include("JobApplication.Job.Company")
                                                .Include("JobApplication.Job.Contact")
                                                .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                .Where(x =>
                                                           x.JobApplication.JobApplicationType.IdParent == dataTableParameters.IdJobType
                                                          && CustomerJobAccess.Contains(x.JobApplication.IdJob)
                                                          && ((DbFunctions.TruncateTime(x.Expires) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
                                                          && DbFunctions.TruncateTime(x.Expires) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate)))
                                                           )
                                                .AsQueryable();

                var recordsTotal = jobApplicationWorkPermitTypes.Count();
                var recordsFiltered = recordsTotal;
                string permitCode = string.Empty;
                switch (dataTableParameters.IdJobType)
                {
                    case 1:
                        permitCode = "DOB";
                        break;
                    case 2:
                        permitCode = "DOT";
                        break;
                    case 4:
                        permitCode = "DEP";
                        break;
                }

                if (dataTableParameters.IdJobType == 1)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.SignOff == false);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdBorough))
                {
                    List<int> boroughs = dataTableParameters.IdBorough != null && !string.IsNullOrEmpty(dataTableParameters.IdBorough) ? (dataTableParameters.IdBorough.Split('-') != null && dataTableParameters.IdBorough.Split('-').Any() ? dataTableParameters.IdBorough.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => boroughs.Contains(x.JobApplication.Job.RfpAddress.IdBorough ?? 0));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
                {
                    List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.JobApplication.Job.IdProjectManager ?? 0));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.JobApplication.IdJob));
                }

                if (dataTableParameters != null && dataTableParameters.PermitCode != null)
                {
                    permitCode = dataTableParameters.PermitCode;
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code == dataTableParameters.PermitCode);
                }
                else
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code != "AHV" && x.JobWorkType.Code != "TCO");
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.Job.Status == dataTableParameters.Status);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.JobApplication.Job.IdCompany ?? 0)));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.JobApplication.Job.IdContact.Value)));
                }
                if (!string.IsNullOrEmpty(dataTableParameters.IdResponsibility))
                {
                    //jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.IdResponsibility == dataTableParameters.IdResponsibility);
                    List<int> jobResponsibilityTeam = dataTableParameters.IdResponsibility != null && !string.IsNullOrEmpty(dataTableParameters.IdResponsibility) ? (dataTableParameters.IdResponsibility.Split('-') != null && dataTableParameters.IdResponsibility.Split('-').Any() ? dataTableParameters.IdResponsibility.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobResponsibilityTeam.Contains((x.IdResponsibility.Value)));
                }
                //rpoContext.CustomerJobNames.Where(x => x.IdJob == jobApplicationWorkPermitTypes.Select(x=>x.JobApplication.IdJob) && x.IdCustomer == employee.Id)
                var result = jobApplicationWorkPermitTypes
                        .AsEnumerable()
                        .Select(c => this.FormatDOBPermitsExpiryReport(c, permitCode, employee.Id))
                        .AsQueryable()
                        .DataTableParameters(dataTableParameters, out recordsFiltered)
                        .ToArray();

                return this.Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result.OrderByDescending(x => x.IdJob)
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
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
                //base.OnEndPage(writer, doc);

                //int pageNumber = writer.PageNumber;

                //PdfPTable table = new PdfPTable(1);
                ////table.WidthPercentage = 100;
                ////table.TotalWidth = 592F;

                //table.TotalWidth = doc.PageSize.Width - 80f;
                //table.WidthPercentage = 70;

                //PdfPCell cell = new PdfPCell(new Phrase("Page " + pageNumber, new Font(Font.FontFamily.TIMES_ROMAN, 12)));
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.Border = PdfPCell.NO_BORDER;
                ////cell.PaddingTop = 10f;
                //table.AddCell(cell);

                //table.WriteSelectedRows(0, -1, 40, doc.Bottom, writer.DirectContent);
                ////doc.Add(table);
                ////table.WriteSelectedRows(0, -1, 0, doc.Bottom, writer.DirectContent);
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