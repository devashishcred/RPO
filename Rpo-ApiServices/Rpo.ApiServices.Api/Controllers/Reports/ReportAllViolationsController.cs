
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
    //using System.Drawing;
    using System.Globalization;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System.Linq.Expressions;
    using Model.Models.Enums;

    [Authorize]
    public class ReportAllViolationsController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Get the result of all violation.
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetReportAllViolations([FromUri] AllViolationDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewAllViolationsReport))
            {

                //var jobViolations = this.rpoContext.JobViolations.Include("Job.RfpAddress.Borough").Where(x=>x.Job.Status == JobStatus.Active).AsQueryable();
                var jobViolations = (from vio in rpoContext.JobViolations
                                     join rfps in rpoContext.RfpAddresses on vio.BinNumber equals rfps.BinNumber
                                     join job in rpoContext.Jobs on rfps.Id equals job.IdRfpAddress
                                     where vio.Type_ECB_DOB == dataTableParameters.Type_ECB_DOB
                                     select new { vio, job }).AsQueryable();

                var recordsTotal = jobViolations.Count();
                var recordsFiltered = recordsTotal;

                if (dataTableParameters != null && dataTableParameters.SummonsNumber != null)
                {
                    jobViolations = jobViolations.Where(x => x.vio.SummonsNumber.Contains(dataTableParameters.SummonsNumber));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    //jobViolations = jobViolations.Where(x => joNumbers.Contains((x.IdJob ?? 0)));
                    jobViolations = jobViolations.Where(x => joNumbers.Contains(x.job.Id));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobCompanyTeam.Contains((x.job.IdCompany ?? 0)));
                }

                if (dataTableParameters != null && dataTableParameters.IsFullyResolved != null)
                {
                    jobViolations = jobViolations.Where(x => x.vio.IsFullyResolved == dataTableParameters.IsFullyResolved);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobContactTeam.Contains((x.vio.IdContact.Value)));
                }

                if (dataTableParameters != null && dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus))
                {
                    List<string> listViolationStatus = dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus) ? (dataTableParameters.ViolationStatus.Split('-') != null && dataTableParameters.ViolationStatus.Split('-').Any() ? dataTableParameters.ViolationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    if (listViolationStatus != null && listViolationStatus.Count > 0)
                    {
                        for (int i = 0; i < listViolationStatus.Count; i++)
                        {
                            if (listViolationStatus[i] == "No Status")
                            {
                                listViolationStatus[i] = "";
                            }
                        }
                    }
                    jobViolations = jobViolations.Where(t => listViolationStatus.Contains((t.vio.StatusOfSummonsNotice ?? "")));
                }

                if (dataTableParameters != null && dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus))
                {
                    List<string> listcertificationStatus = dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus) ? (dataTableParameters.CertificationStatus.Split('-') != null && dataTableParameters.CertificationStatus.Split('-').Any() ? dataTableParameters.CertificationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    if (listcertificationStatus != null && listcertificationStatus.Count > 0)
                    {
                        for (int i = 0; i < listcertificationStatus.Count; i++)
                        {
                            if (listcertificationStatus[i] == "No Status")
                            {
                                listcertificationStatus[i] = "";
                            }
                        }
                    }

                    jobViolations = jobViolations.Where(t => listcertificationStatus.Contains((t.vio.CertificationStatus ?? "")));
                }

                if (dataTableParameters != null && dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult))
                {
                    List<string> listHearingResult = dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult) ? (dataTableParameters.HearingResult.Split('-') != null && dataTableParameters.HearingResult.Split('-').Any() ? dataTableParameters.HearingResult.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    if (listHearingResult != null && listHearingResult.Count > 0)
                    {
                        for (int i = 0; i < listHearingResult.Count; i++)
                        {
                            if (listHearingResult[i] == "No Result")
                            {
                                listHearingResult[i] = "";
                            }
                        }
                    }

                    jobViolations = jobViolations.Where(t => listHearingResult.Contains((t.vio.HearingResult ?? "")));
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CreatedDate) >= DbFunctions.TruncateTime(dataTableParameters.CreatedDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CreatedDate) <= DbFunctions.TruncateTime(dataTableParameters.CreatedDateTo));
                }

                if (dataTableParameters != null && dataTableParameters.HearingDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.HearingDate) >= DbFunctions.TruncateTime(dataTableParameters.HearingDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.HearingDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.HearingDate) <= DbFunctions.TruncateTime(dataTableParameters.HearingDateTo));
                }

                if (dataTableParameters != null && dataTableParameters.CureDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CureDate) >= DbFunctions.TruncateTime(dataTableParameters.CureDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CureDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CureDate) <= DbFunctions.TruncateTime(dataTableParameters.CureDateTo));
                }
                //if (dataTableParameters != null && dataTableParameters.Type_ECB_DOB != null) //ECB or DOB type
                //{
                //    jobViolations = jobViolations.Where(x => x.vio.Type_ECB_DOB == dataTableParameters.Type_ECB_DOB);
                //}
                var result = jobViolations.Distinct()
               .AsEnumerable()
               .Select(c => this.FormatAllViolationReport(c.vio))
               .AsQueryable()
               .DataTableParameters(dataTableParameters, out recordsFiltered)
               .ToArray();

                //var result = upcomingHearingDatelists
                //          .AsEnumerable()
                //          .Select(j => j)
                //          .AsQueryable()
                //          .DataTableParameters(dataTableParameters, out recordsFiltered)
                //          .ToList();

                return this.Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result.OrderByDescending(x => x.HearingDate)
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Export to excel of the all violation report
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>

        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportAllViolations/exporttoexcel")]
        public IHttpActionResult PostExportAllViolationToExcel(AllViolationDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                //var jobViolations = this.rpoContext.JobViolations.Include("Job.RfpAddress.Borough").AsQueryable();
                var jobViolations = (from vio in rpoContext.JobViolations
                                     join rfps in rpoContext.RfpAddresses on vio.BinNumber equals rfps.BinNumber
                                     join job in rpoContext.Jobs on rfps.Id equals job.IdRfpAddress
                                     where vio.Type_ECB_DOB == dataTableParameters.Type_ECB_DOB
                                     select new { vio, job }).AsQueryable();

                string hearingDate = string.Empty;
                if (dataTableParameters != null && dataTableParameters.SummonsNumber != null)
                {
                    jobViolations = jobViolations.Where(x => x.vio.SummonsNumber.Contains(dataTableParameters.SummonsNumber));
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => joNumbers.Contains(x.job.Id));
                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobCompanyTeam.Contains((x.job.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.IsFullyResolved != null)
                {
                    jobViolations = jobViolations.Where(x => x.vio.IsFullyResolved == dataTableParameters.IsFullyResolved);

                    string iIsFullyResolved = dataTableParameters.IsFullyResolved != null && Convert.ToBoolean(dataTableParameters.IsFullyResolved) == true ? "Yes" : "No";
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(iIsFullyResolved) ? " | " : string.Empty) + (!string.IsNullOrEmpty(iIsFullyResolved) ? "Fully Resolved : " + iIsFullyResolved : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobContactTeam.Contains((x.job.IdContact.Value)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus))
                {
                    List<string> listViolationStatus = dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus) ? (dataTableParameters.ViolationStatus.Split('-') != null && dataTableParameters.ViolationStatus.Split('-').Any() ? dataTableParameters.ViolationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredViolationStatus = string.Join(", ", listViolationStatus.Select(x => x));
                    if (listViolationStatus != null && listViolationStatus.Count > 0)
                    {
                        for (int i = 0; i < listViolationStatus.Count; i++)
                        {
                            if (listViolationStatus[i] == "No Status")
                            {
                                listViolationStatus[i] = "";
                            }
                        }
                    }
                    jobViolations = jobViolations.Where(t => listViolationStatus.Contains((t.vio.StatusOfSummonsNotice ?? "")));

                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredViolationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredViolationStatus) ? "Status : " + filteredViolationStatus : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus))
                {
                    List<string> listcertificationStatus = dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus) ? (dataTableParameters.CertificationStatus.Split('-') != null && dataTableParameters.CertificationStatus.Split('-').Any() ? dataTableParameters.CertificationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredcertificationStatus = string.Join(", ", listcertificationStatus.Select(x => x));

                    if (listcertificationStatus != null && listcertificationStatus.Count > 0)
                    {
                        for (int i = 0; i < listcertificationStatus.Count; i++)
                        {
                            if (listcertificationStatus[i] == "No Status")
                            {
                                listcertificationStatus[i] = "";
                            }
                        }
                    }

                    jobViolations = jobViolations.Where(t => listcertificationStatus.Contains((t.vio.CertificationStatus ?? "")));

                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredcertificationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredcertificationStatus) ? "Certification Status : " + filteredcertificationStatus : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult))
                {
                    List<string> listHearingResult = dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult) ? (dataTableParameters.HearingResult.Split('-') != null && dataTableParameters.HearingResult.Split('-').Any() ? dataTableParameters.HearingResult.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredHearingResult = string.Join(", ", listHearingResult.Select(x => x));

                    if (listHearingResult != null && listHearingResult.Count > 0)
                    {
                        for (int i = 0; i < listHearingResult.Count; i++)
                        {
                            if (listHearingResult[i] == "No Result")
                            {
                                listHearingResult[i] = "";
                            }
                        }
                    }

                    jobViolations = jobViolations.Where(t => listHearingResult.Contains((t.vio.HearingResult ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredHearingResult) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredHearingResult) ? "Hearing Result : " + filteredHearingResult : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CreatedDate) >= DbFunctions.TruncateTime(dataTableParameters.CreatedDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CreatedDate) <= DbFunctions.TruncateTime(dataTableParameters.CreatedDateTo));
                }

                string filteredhearingDate = string.Empty;
                if (dataTableParameters != null && dataTableParameters.HearingDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.HearingDate) >= DbFunctions.TruncateTime(dataTableParameters.HearingDateFrom));
                    filteredhearingDate = filteredhearingDate + Convert.ToDateTime(dataTableParameters.HearingDateFrom).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.HearingDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.HearingDate) <= DbFunctions.TruncateTime(dataTableParameters.HearingDateTo));
                    filteredhearingDate = filteredhearingDate + " - " + Convert.ToDateTime(dataTableParameters.HearingDateTo).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredhearingDate) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredhearingDate) ? "Hearing Date : " + filteredhearingDate : string.Empty);

                if (dataTableParameters != null && dataTableParameters.CureDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CureDate) >= DbFunctions.TruncateTime(dataTableParameters.CureDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CureDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CureDate) <= DbFunctions.TruncateTime(dataTableParameters.CureDateTo));
                }
                //if (dataTableParameters != null && dataTableParameters.Type_ECB_DOB != null) //ECB or DOB type
                //{
                //    jobViolations = jobViolations.Where(x => x.vio.Type_ECB_DOB == dataTableParameters.Type_ECB_DOB);
                //}


                string direction = string.Empty;
                string orderBy = string.Empty;

                List<AllViolationDTO> result = new List<AllViolationDTO>();

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
                    if (orderBy.Trim().ToLower() == "Id".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.Id);
                    if (orderBy.Trim().ToLower() == "SummonsNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.SummonsNumber);
                    if (orderBy.Trim().ToLower() == "DateIssued".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.DateIssued);
                    if (orderBy.Trim().ToLower() == "CureDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CureDate);
                    if (orderBy.Trim().ToLower() == "InspectionLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.InspectionLocation);
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.job.Id);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.job.RfpAddress.HouseNumber);
                    if (orderBy.Trim().ToLower() == "IssuingAgency".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.IssuingAgency);
                    if (orderBy.Trim().ToLower() == "HearingDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingDate);
                    if (orderBy.Trim().ToLower() == "HearingLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingLocation);
                    if (orderBy.Trim().ToLower() == "HearingResult".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingResult);
                    if (orderBy.Trim().ToLower() == "BalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "FormattedBalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "IsFullyResolved".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.IsFullyResolved);

                    if (orderBy.Trim().ToLower() == "ResolvedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.ResolvedDate);
                    if (orderBy.Trim().ToLower() == "StatusOfSummonsNotice".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.StatusOfSummonsNotice);
                    if (orderBy.Trim().ToLower() == "CertificationStatus".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CertificationStatus);
                    if (orderBy.Trim().ToLower() == "CreatedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CreatedBy);
                    if (orderBy.Trim().ToLower() == "LastModifiedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.LastModifiedBy);
                    if (orderBy.Trim().ToLower() == "CreatedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CreatedDate);
                    if (orderBy.Trim().ToLower() == "LastModifiedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.LastModifiedDate);
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "Id".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.Id);
                    if (orderBy.Trim().ToLower() == "SummonsNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.SummonsNumber);
                    if (orderBy.Trim().ToLower() == "DateIssued".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.DateIssued);
                    if (orderBy.Trim().ToLower() == "CureDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CureDate);
                    if (orderBy.Trim().ToLower() == "InspectionLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.InspectionLocation);
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.job.Id);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.job.RfpAddress.HouseNumber);
                    if (orderBy.Trim().ToLower() == "IssuingAgency".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.IssuingAgency);
                    if (orderBy.Trim().ToLower() == "HearingDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingDate);
                    if (orderBy.Trim().ToLower() == "HearingLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingLocation);
                    if (orderBy.Trim().ToLower() == "HearingResult".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingResult);
                    if (orderBy.Trim().ToLower() == "BalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "FormattedBalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "IsFullyResolved".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.IsFullyResolved);

                    if (orderBy.Trim().ToLower() == "ResolvedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.ResolvedDate);
                    if (orderBy.Trim().ToLower() == "StatusOfSummonsNotice".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.StatusOfSummonsNotice);
                    if (orderBy.Trim().ToLower() == "CertificationStatus".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CertificationStatus);
                    if (orderBy.Trim().ToLower() == "CreatedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CreatedBy);
                    if (orderBy.Trim().ToLower() == "LastModifiedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.LastModifiedBy);
                    if (orderBy.Trim().ToLower() == "CreatedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CreatedDate);
                    if (orderBy.Trim().ToLower() == "LastModifiedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.LastModifiedDate);
                }
                var jobViolation = jobViolations.Distinct();
                result = jobViolation
                 .AsEnumerable().OrderByDescending(x => x.vio.HearingDate)
                 .Select(c => this.FormatAllViolationReport(c.vio))
                 .AsQueryable()
                 .ToList();

                string exportFilename = "AllViolationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                string exportFilePath = ExportToExcel(hearingDate, result, exportFilename, dataTableParameters.Type_ECB_DOB);

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

        private string ExportToExcel(string hearingDate, List<AllViolationDTO> result, string exportFilename, string Type)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string templateFileName = string.Empty;
            if (Type == "ECB")
            { templateFileName = "AllViolationReportExportTemplateECB.xlsx"; }
            else
            { templateFileName = "AllViolationReportExportTemplateDOB.xlsx"; }
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
            XSSFFont ColumnHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
            ColumnHeaderFont.FontHeightInPoints = (short)12;
            ColumnHeaderFont.FontName = "Times New Roman";
            ColumnHeaderFont.IsBold = true;
            #region Column Header cell style
            XSSFCellStyle ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            ColumnHeaderCellStyle.SetFont(ColumnHeaderFont);
            ColumnHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
            ColumnHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
            ColumnHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
            ColumnHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
            ColumnHeaderCellStyle.WrapText = true;
            ColumnHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
            ColumnHeaderCellStyle.Alignment = HorizontalAlignment.Left;
            ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
            ColumnHeaderCellStyle.FillPattern = FillPattern.SolidForeground;
            #endregion

            if (Type == "ECB")
            {
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
                string reportDate = "Date: " + DateTime.Today.ToString(Common.ExportReportDateFormat);
                IRow iReportDateRow = sheet.GetRow(3);
                if (iReportDateRow != null)
                {
                    if (iReportDateRow.GetCell(10) != null)
                    {
                        iReportDateRow.GetCell(10).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(10).SetCellValue(reportDate);
                    }
                }
                else
                {
                    iReportDateRow = sheet.CreateRow(3);
                    if (iReportDateRow.GetCell(10) != null)
                    {
                        iReportDateRow.GetCell(10).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(10).SetCellValue(reportDate);
                    }
                }
                int index = 4;
                #region column header
                IRow iRow = sheet.GetRow(index);
                iRow = sheet.CreateRow(index);

                if (iRow.GetCell(0) != null)
                {
                    iRow.GetCell(0).SetCellValue("Project # | Address");
                }
                else
                {
                    iRow.CreateCell(0).SetCellValue("Project # | Address");
                }

                if (iRow.GetCell(1) != null)
                {
                    iRow.GetCell(1).SetCellValue("Violation #");
                }
                else
                {
                    iRow.CreateCell(1).SetCellValue("Violation #");
                }

                if (iRow.GetCell(2) != null)
                {
                    iRow.GetCell(2).SetCellValue("Date Issued");
                }
                else
                {
                    iRow.CreateCell(2).SetCellValue("Date Issued");
                }

                if (iRow.GetCell(3) != null)
                {
                    iRow.GetCell(3).SetCellValue("Issuing Agency");
                }
                else
                {
                    iRow.CreateCell(3).SetCellValue("Issuing Agency");
                }

                if (iRow.GetCell(4) != null)
                {
                    iRow.GetCell(4).SetCellValue("Description");
                }
                else
                {
                    iRow.CreateCell(4).SetCellValue("Description");
                }

                if (iRow.GetCell(5) != null)
                {
                    iRow.GetCell(5).SetCellValue("Hearing Date");
                }
                else
                {
                    iRow.CreateCell(5).SetCellValue("Hearing Date");
                }

                if (iRow.GetCell(6) != null)
                {
                    iRow.GetCell(6).SetCellValue("Hearing Result");
                }
                else
                {
                    iRow.CreateCell(6).SetCellValue("Hearing Result");
                }

                if (iRow.GetCell(7) != null)
                {
                    iRow.GetCell(7).SetCellValue("Balance Due");
                }
                else
                {
                    iRow.CreateCell(7).SetCellValue("Balance Due");
                }

                if (iRow.GetCell(8) != null)
                {
                    iRow.GetCell(8).SetCellValue("Status");
                }
                else
                {
                    iRow.CreateCell(8).SetCellValue("Status");
                }

                if (iRow.GetCell(9) != null)
                {
                    iRow.GetCell(9).SetCellValue("Certification Status");
                }
                else
                {
                    iRow.CreateCell(9).SetCellValue("Certification Status");
                }

                if (iRow.GetCell(10) != null)
                {
                    iRow.GetCell(10).SetCellValue("Fully Resolved?");
                }
                else
                {
                    iRow.CreateCell(10).SetCellValue("Fully Resolved?");
                }
                iRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(1).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(6).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(7).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(9).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(10).CellStyle = ColumnHeaderCellStyle;
                #endregion
                index = 5;
                foreach (AllViolationDTO item in result)
                {
                    iRow = sheet.GetRow(index);
                    if (iRow != null)
                    {
                        if (iRow.GetCell(0) != null)
                        {
                            iRow.GetCell(0).SetCellValue(item.JobNumber + " | " + item.Address);
                        }
                        else
                        {
                            iRow.CreateCell(0).SetCellValue(item.JobNumber + " | " + item.Address);
                        }

                        if (iRow.GetCell(1) != null)
                        {
                            iRow.GetCell(1).SetCellValue(item.SummonsNumber);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SummonsNumber);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.IssuingAgency);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.IssuingAgency);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.Description);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.Description);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.HearingDate != null ? Convert.ToDateTime(item.HearingDate).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.HearingDate != null ? Convert.ToDateTime(item.HearingDate).ToString("MM/dd/yyyy") : string.Empty);
                        }

                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.HearingResult);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.HearingResult);
                        }

                        if (iRow.GetCell(7) != null)
                        {
                            iRow.GetCell(7).SetCellValue(item.FormattedBalanceDue);
                        }
                        else
                        {
                            iRow.CreateCell(7).SetCellValue(item.FormattedBalanceDue);
                        }

                        if (iRow.GetCell(8) != null)
                        {
                            iRow.GetCell(8).SetCellValue(item.StatusOfSummonsNotice);
                        }
                        else
                        {
                            iRow.CreateCell(8).SetCellValue(item.StatusOfSummonsNotice);
                        }

                        if (iRow.GetCell(9) != null)
                        {
                            iRow.GetCell(9).SetCellValue(item.CertificationStatus);
                        }
                        else
                        {
                            iRow.CreateCell(9).SetCellValue(item.CertificationStatus);
                        }

                        if (iRow.GetCell(10) != null)
                        {
                            iRow.GetCell(10).SetCellValue(item.IsFullyResolved ? "Yes" : "No");
                        }
                        else
                        {
                            iRow.CreateCell(10).SetCellValue(item.IsFullyResolved ? "Yes" : "No");
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(7).CellStyle = rightAlignCellStyle;
                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(9).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(10).CellStyle = leftAlignCellStyle;
                    }
                    else
                    {
                        iRow = sheet.CreateRow(index);

                        if (iRow.GetCell(0) != null)
                        {
                            iRow.GetCell(0).SetCellValue(item.JobNumber + " | " + item.Address);
                        }
                        else
                        {
                            iRow.CreateCell(0).SetCellValue(item.JobNumber + " | " + item.Address);
                        }

                        if (iRow.GetCell(1) != null)
                        {
                            iRow.GetCell(1).SetCellValue(item.SummonsNumber);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.SummonsNumber);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.IssuingAgency);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.IssuingAgency);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.Description);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.Description);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.HearingDate != null ? Convert.ToDateTime(item.HearingDate).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.HearingDate != null ? Convert.ToDateTime(item.HearingDate).ToString("MM/dd/yyyy") : string.Empty);
                        }

                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.HearingResult);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.HearingResult);
                        }

                        if (iRow.GetCell(7) != null)
                        {
                            iRow.GetCell(7).SetCellValue(item.FormattedBalanceDue);
                        }
                        else
                        {
                            iRow.CreateCell(7).SetCellValue(item.FormattedBalanceDue);
                        }

                        if (iRow.GetCell(8) != null)
                        {
                            iRow.GetCell(8).SetCellValue(item.StatusOfSummonsNotice);
                        }
                        else
                        {
                            iRow.CreateCell(8).SetCellValue(item.StatusOfSummonsNotice);
                        }

                        if (iRow.GetCell(9) != null)
                        {
                            iRow.GetCell(9).SetCellValue(item.CertificationStatus);
                        }
                        else
                        {
                            iRow.CreateCell(9).SetCellValue(item.CertificationStatus);
                        }

                        if (iRow.GetCell(10) != null)
                        {
                            iRow.GetCell(10).SetCellValue(item.IsFullyResolved ? "Yes" : "No");
                        }
                        else
                        {
                            iRow.CreateCell(10).SetCellValue(item.IsFullyResolved ? "Yes" : "No");
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(7).CellStyle = rightAlignCellStyle;
                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(9).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(10).CellStyle = leftAlignCellStyle;
                    }

                    index = index + 1;
                }
            }
            else
            {
                string reportDate = "Date: " + DateTime.Today.ToString(Common.ExportReportDateFormat);
                IRow iReportDateRow = sheet.GetRow(2);
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
                    iReportDateRow = sheet.CreateRow(2);
                    if (iReportDateRow.GetCell(6) != null)
                    {
                        iReportDateRow.GetCell(6).SetCellValue(reportDate);
                    }
                    else
                    {
                        iReportDateRow.CreateCell(6).SetCellValue(reportDate);
                    }
                }
                int index = 3;
                #region column header
                IRow iRow = sheet.GetRow(index);
                iRow = sheet.CreateRow(index);

                if (iRow.GetCell(0) != null)
                {
                    iRow.GetCell(0).SetCellValue("Project #");
                }
                else
                {
                    iRow.CreateCell(0).SetCellValue("Project #");
                }
                if (iRow.GetCell(1) != null)
                {
                    iRow.GetCell(1).SetCellValue("Project Address");
                }
                else
                {
                    iRow.CreateCell(1).SetCellValue("Project Address");
                }
                if (iRow.GetCell(2) != null)
                {
                    iRow.GetCell(2).SetCellValue("Issued Date");
                }
                else
                {
                    iRow.CreateCell(2).SetCellValue("Issued Date");
                }
                if (iRow.GetCell(3) != null)
                {
                    iRow.GetCell(3).SetCellValue("DOB Violation #");
                }
                else
                {
                    iRow.CreateCell(3).SetCellValue("DOB Violation #");
                }

                if (iRow.GetCell(4) != null)
                {
                    iRow.GetCell(4).SetCellValue("ECB Violation #");
                }
                else
                {
                    iRow.CreateCell(4).SetCellValue("ECB Violation #");
                }

                if (iRow.GetCell(5) != null)
                {
                    iRow.GetCell(5).SetCellValue("Violation Description");
                }
                else
                {
                    iRow.CreateCell(5).SetCellValue("Violation Description");
                }

                if (iRow.GetCell(6) != null)
                {
                    iRow.GetCell(6).SetCellValue("Violation Category");
                }
                else
                {
                    iRow.CreateCell(6).SetCellValue("Violation Category");
                }


                iRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(1).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(6).CellStyle = ColumnHeaderCellStyle;

                #endregion
                index = 5;
                foreach (AllViolationDTO item in result)
                {
                    iRow = sheet.GetRow(index);
                    if (iRow != null)
                    {
                        if (iRow.GetCell(0) != null)
                        {
                            iRow.GetCell(0).SetCellValue(item.JobNumber);
                        }
                        else
                        {
                            iRow.CreateCell(0).SetCellValue(item.JobNumber);
                        }
                        if (iRow.GetCell(1) != null)
                        {
                            iRow.GetCell(1).SetCellValue(item.Address);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.Address);
                        }
                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.SummonsNumber);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.SummonsNumber);
                        }
                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.RelatedECB);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.RelatedECB);
                        }

                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.ViolationDescription);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.ViolationDescription);
                        }
                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.ViolationCategory);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.ViolationCategory);
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
                            iRow.GetCell(0).SetCellValue(item.JobNumber);
                        }
                        else
                        {
                            iRow.CreateCell(0).SetCellValue(item.JobNumber);
                        }
                        if (iRow.GetCell(1) != null)
                        {
                            iRow.GetCell(1).SetCellValue(item.Address);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.Address);
                        }
                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.SummonsNumber);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.SummonsNumber);
                        }
                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.RelatedECB);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.RelatedECB);
                        }
                        if (iRow.GetCell(5) != null)
                        {
                            iRow.GetCell(5).SetCellValue(item.ViolationDescription);
                        }
                        else
                        {
                            iRow.CreateCell(5).SetCellValue(item.ViolationDescription);
                        }

                        if (iRow.GetCell(6) != null)
                        {
                            iRow.GetCell(6).SetCellValue(item.ViolationCategory);
                        }
                        else
                        {
                            iRow.CreateCell(6).SetCellValue(item.ViolationCategory);
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
            }

            using (var file2 = new FileStream(exportFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                templateWorkbook.Write(file2);
                file2.Close();
            }

            return exportFilePath;
        }


        /// <summary>
        /// 
        /// Export to pdf of the all violation report
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportAllViolations/exporttopdf")]
        public IHttpActionResult PostExportAllViolationToPdf(AllViolationDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                //var jobViolations = this.rpoContext.JobViolations.Include("Job.RfpAddress.Borough").AsQueryable();
                var jobViolations = (from vio in rpoContext.JobViolations
                                     join rfps in rpoContext.RfpAddresses on vio.BinNumber equals rfps.BinNumber
                                     join job in rpoContext.Jobs on rfps.Id equals job.IdRfpAddress
                                     where vio.Type_ECB_DOB == dataTableParameters.Type_ECB_DOB
                                     select new { vio, job }).AsQueryable();
                string hearingDate = string.Empty;
                if (dataTableParameters != null && dataTableParameters.SummonsNumber != null)
                {
                    jobViolations = jobViolations.Where(x => x.vio.SummonsNumber.Contains(dataTableParameters.SummonsNumber));
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => joNumbers.Contains(x.job.Id));
                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobCompanyTeam.Contains((x.job.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.IsFullyResolved != null)
                {
                    jobViolations = jobViolations.Where(x => x.vio.IsFullyResolved == dataTableParameters.IsFullyResolved);

                    string iIsFullyResolved = dataTableParameters.IsFullyResolved != null && Convert.ToBoolean(dataTableParameters.IsFullyResolved) == true ? "Yes" : "No";
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(iIsFullyResolved) ? " | " : string.Empty) + (!string.IsNullOrEmpty(iIsFullyResolved) ? "Fully Resolved : " + iIsFullyResolved : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobContactTeam.Contains((x.vio.Job.IdContact.Value)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus))
                {
                    List<string> listViolationStatus = dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus) ? (dataTableParameters.ViolationStatus.Split('-') != null && dataTableParameters.ViolationStatus.Split('-').Any() ? dataTableParameters.ViolationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredViolationStatus = string.Join(", ", listViolationStatus.Select(x => x));
                    if (listViolationStatus != null && listViolationStatus.Count > 0)
                    {
                        for (int i = 0; i < listViolationStatus.Count; i++)
                        {
                            if (listViolationStatus[i] == "No Status")
                            {
                                listViolationStatus[i] = "";
                            }
                        }
                    }
                    jobViolations = jobViolations.Where(t => listViolationStatus.Contains((t.vio.StatusOfSummonsNotice ?? "")));

                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredViolationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredViolationStatus) ? "Status : " + filteredViolationStatus : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus))
                {
                    List<string> listcertificationStatus = dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus) ? (dataTableParameters.CertificationStatus.Split('-') != null && dataTableParameters.CertificationStatus.Split('-').Any() ? dataTableParameters.CertificationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredcertificationStatus = string.Join(", ", listcertificationStatus.Select(x => x));

                    if (listcertificationStatus != null && listcertificationStatus.Count > 0)
                    {
                        for (int i = 0; i < listcertificationStatus.Count; i++)
                        {
                            if (listcertificationStatus[i] == "No Status")
                            {
                                listcertificationStatus[i] = "";
                            }
                        }
                    }

                    jobViolations = jobViolations.Where(t => listcertificationStatus.Contains((t.vio.CertificationStatus ?? "")));

                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredcertificationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredcertificationStatus) ? "Certification Status : " + filteredcertificationStatus : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult))
                {
                    List<string> listHearingResult = dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult) ? (dataTableParameters.HearingResult.Split('-') != null && dataTableParameters.HearingResult.Split('-').Any() ? dataTableParameters.HearingResult.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredHearingResult = string.Join(", ", listHearingResult.Select(x => x));

                    if (listHearingResult != null && listHearingResult.Count > 0)
                    {
                        for (int i = 0; i < listHearingResult.Count; i++)
                        {
                            if (listHearingResult[i] == "No Result")
                            {
                                listHearingResult[i] = "";
                            }
                        }
                    }

                    jobViolations = jobViolations.Where(t => listHearingResult.Contains((t.vio.HearingResult ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredHearingResult) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredHearingResult) ? "Hearing Result : " + filteredHearingResult : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CreatedDate) >= DbFunctions.TruncateTime(dataTableParameters.CreatedDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CreatedDate) <= DbFunctions.TruncateTime(dataTableParameters.CreatedDateTo));
                }

                string filteredhearingDate = string.Empty;
                if (dataTableParameters != null && dataTableParameters.HearingDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.HearingDate) >= DbFunctions.TruncateTime(dataTableParameters.HearingDateFrom));
                    filteredhearingDate = filteredhearingDate + Convert.ToDateTime(dataTableParameters.HearingDateFrom).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.HearingDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.HearingDate) <= DbFunctions.TruncateTime(dataTableParameters.HearingDateTo));
                    filteredhearingDate = filteredhearingDate + " - " + Convert.ToDateTime(dataTableParameters.HearingDateTo).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredhearingDate) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredhearingDate) ? "Hearing Date : " + filteredhearingDate : string.Empty);

                if (dataTableParameters != null && dataTableParameters.CureDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CureDate) >= DbFunctions.TruncateTime(dataTableParameters.CureDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CureDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CureDate) <= DbFunctions.TruncateTime(dataTableParameters.CureDateTo));
                }
                //if (dataTableParameters != null && dataTableParameters.Type_ECB_DOB != null) //ECB or DOB type
                //{
                //    jobViolations = jobViolations.Where(x => x.vio.Type_ECB_DOB == dataTableParameters.Type_ECB_DOB);
                //}

                string direction = string.Empty;
                string orderBy = string.Empty;

                List<AllViolationDTO> result = new List<AllViolationDTO>();

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
                    if (orderBy.Trim().ToLower() == "Id".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.Id);
                    if (orderBy.Trim().ToLower() == "SummonsNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.SummonsNumber);
                    if (orderBy.Trim().ToLower() == "DateIssued".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.DateIssued);
                    if (orderBy.Trim().ToLower() == "CureDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CureDate);
                    if (orderBy.Trim().ToLower() == "InspectionLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.InspectionLocation);
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.job.Id);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.job.RfpAddress.HouseNumber);
                    if (orderBy.Trim().ToLower() == "IssuingAgency".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.IssuingAgency);
                    if (orderBy.Trim().ToLower() == "HearingDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingDate);
                    if (orderBy.Trim().ToLower() == "HearingLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingLocation);
                    if (orderBy.Trim().ToLower() == "HearingResult".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingResult);
                    if (orderBy.Trim().ToLower() == "BalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "FormattedBalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "IsFullyResolved".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.IsFullyResolved);

                    if (orderBy.Trim().ToLower() == "ResolvedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.ResolvedDate);
                    if (orderBy.Trim().ToLower() == "StatusOfSummonsNotice".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.StatusOfSummonsNotice);
                    if (orderBy.Trim().ToLower() == "CertificationStatus".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CertificationStatus);
                    if (orderBy.Trim().ToLower() == "CreatedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CreatedBy);
                    if (orderBy.Trim().ToLower() == "LastModifiedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.LastModifiedBy);
                    if (orderBy.Trim().ToLower() == "CreatedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CreatedDate);
                    if (orderBy.Trim().ToLower() == "LastModifiedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.LastModifiedDate);
                }
                else
                {

                    if (orderBy.Trim().ToLower() == "Id".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.Id);
                    if (orderBy.Trim().ToLower() == "SummonsNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.SummonsNumber);
                    if (orderBy.Trim().ToLower() == "DateIssued".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.DateIssued);
                    if (orderBy.Trim().ToLower() == "CureDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CureDate);
                    if (orderBy.Trim().ToLower() == "InspectionLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.InspectionLocation);
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.job.Id);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.job.RfpAddress.HouseNumber);
                    if (orderBy.Trim().ToLower() == "IssuingAgency".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.IssuingAgency);
                    if (orderBy.Trim().ToLower() == "HearingDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingDate);
                    if (orderBy.Trim().ToLower() == "HearingLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingLocation);
                    if (orderBy.Trim().ToLower() == "HearingResult".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingResult);
                    if (orderBy.Trim().ToLower() == "BalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "FormattedBalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "IsFullyResolved".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.IsFullyResolved);
                    if (orderBy.Trim().ToLower() == "ResolvedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.ResolvedDate);
                    if (orderBy.Trim().ToLower() == "StatusOfSummonsNotice".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.StatusOfSummonsNotice);
                    if (orderBy.Trim().ToLower() == "CertificationStatus".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CertificationStatus);
                    if (orderBy.Trim().ToLower() == "CreatedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CreatedBy);
                    if (orderBy.Trim().ToLower() == "LastModifiedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.LastModifiedBy);
                    if (orderBy.Trim().ToLower() == "CreatedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CreatedDate);
                    if (orderBy.Trim().ToLower() == "LastModifiedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.LastModifiedDate);
                }
                var jobViolation = jobViolations.Distinct();
                result = jobViolation
                 .AsEnumerable().OrderByDescending(x => x.vio.HearingDate)
                 .Select(c => this.FormatAllViolationReport(c.vio))
                 .AsQueryable()
                 .ToList();


                string exportFilename = "AllViolationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                string exportFilePath = ExportToPdf(hearingDate, result, exportFilename, dataTableParameters.Type_ECB_DOB);
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

        private string ExportToPdf(string hearingDate, List<AllViolationDTO> result, string exportFilename, string Type)
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

                int tableType = 0;
                if (Type == "ECB")
                { tableType = 11; }
                else
                { tableType = 7; }
                PdfPTable table = new PdfPTable(tableType);
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
                if (Type == "ECB")
                {
                    cell.PaddingLeft = -10;
                    cell.Colspan = 11;
                }
                else
                {
                    cell.PaddingLeft = -50;
                    cell.Colspan = 7;
                }
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                if (Type == "ECB")
                {
                    cell.PaddingLeft = -10;
                    cell.Colspan = tableType;
                }
                else
                {
                    cell.PaddingLeft = -50;
                    cell.Colspan = 7;
                }
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = -10;
                table.AddCell(cell);

                string reportType = string.Empty;
                if (Type == "ECB")
                { reportType = "ECB VIOLATION REPORT"; }
                else
                { reportType = "DOB VIOLATION REPORT"; }
                cell = new PdfPCell(new Phrase(reportType, font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = tableType;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);



                if (Type == "ECB")
                {
                    cell = new PdfPCell(new Phrase(hearingDate, font_12_Bold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = PdfPCell.TOP_BORDER;
                    cell.Colspan = tableType;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 9;
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

                    cell = new PdfPCell(new Phrase("Violation #", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Date Issued", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Issuing Agency", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Description", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Hearing Date", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Hearing Result", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Balance Due", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Status", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Certification Status", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Fully Resolved?", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    foreach (AllViolationDTO item in result)
                    {
                        cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.Address, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.SummonsNumber, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString("MM/dd/yyyy") : string.Empty, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.IssuingAgency, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.Description, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.HearingDate != null ? Convert.ToDateTime(item.HearingDate).ToString("MM/dd/yyyy") : string.Empty, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.HearingResult, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.FormattedBalanceDue, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.StatusOfSummonsNotice, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.CertificationStatus, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.IsFullyResolved ? "Yes" : "No", font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);
                    }
                }
                else
                {
                    cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 5;
                    table.AddCell(cell);

                    string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

                    cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 2;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Project #", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Project Address", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Issued Date", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("DOB Violation #", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("ECB Violation #", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Violation Description", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Violation Category", font_Table_Header));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.Colspan = 1;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                    table.AddCell(cell);

                    foreach (AllViolationDTO item in result)
                    {
                        cell = new PdfPCell(new Phrase(item.JobNumber, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.Address, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString("MM/dd/yyyy") : string.Empty, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.SummonsNumber, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.RelatedECB, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.ViolationDescription, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(item.ViolationCategory, font_Table_Data));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        table.AddCell(cell);
                    }
                }
                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }

        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        [Route("api/GetReportAllViolationsForCustomer")]
        public IHttpActionResult GetReportAllViolationsForCustomer([FromUri] AllViolationDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewAllViolationsReport))
            {
                //var jobViolations = this.rpoContext.JobViolations.Include("Job.RfpAddress.Borough").AsQueryable();
                var jobViolations = (from v in rpoContext.JobViolations
                                     join rfps in rpoContext.RfpAddresses on v.BinNumber equals rfps.BinNumber
                                     join job in rpoContext.Jobs on rfps.Id equals job.IdRfpAddress
                                     select v).AsQueryable();
                var recordsTotal = jobViolations.Count();
                var recordsFiltered = recordsTotal;

                //var b = jobViolations.Count();
                if (dataTableParameters != null && dataTableParameters.SummonsNumber != null)
                {
                    jobViolations = jobViolations.Where(x => x.SummonsNumber.Contains(dataTableParameters.SummonsNumber));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => joNumbers.Contains((x.IdJob ?? 0)));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobCompanyTeam.Contains((x.Job.IdCompany ?? 0)));
                }

                if (dataTableParameters != null && dataTableParameters.IsFullyResolved != null)
                {
                    jobViolations = jobViolations.Where(x => x.IsFullyResolved == dataTableParameters.IsFullyResolved);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobContactTeam.Contains((x.Job.IdContact.Value)));
                }

                if (dataTableParameters != null && dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus))
                {
                    List<string> listViolationStatus = dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus) ? (dataTableParameters.ViolationStatus.Split('-') != null && dataTableParameters.ViolationStatus.Split('-').Any() ? dataTableParameters.ViolationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    if (listViolationStatus != null && listViolationStatus.Count > 0)
                    {
                        for (int i = 0; i < listViolationStatus.Count; i++)
                        {
                            if (listViolationStatus[i] == "No Status")
                            {
                                listViolationStatus[i] = "";
                            }
                        }
                    }
                    jobViolations = jobViolations.Where(t => listViolationStatus.Contains((t.StatusOfSummonsNotice ?? "")));
                }

                if (dataTableParameters != null && dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus))
                {
                    List<string> listcertificationStatus = dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus) ? (dataTableParameters.CertificationStatus.Split('-') != null && dataTableParameters.CertificationStatus.Split('-').Any() ? dataTableParameters.CertificationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    if (listcertificationStatus != null && listcertificationStatus.Count > 0)
                    {
                        for (int i = 0; i < listcertificationStatus.Count; i++)
                        {
                            if (listcertificationStatus[i] == "No Status")
                            {
                                listcertificationStatus[i] = "";
                            }
                        }
                    }
                    jobViolations = jobViolations.Where(t => listcertificationStatus.Contains((t.CertificationStatus ?? "")));
                }

                if (dataTableParameters != null && dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult))
                {
                    List<string> listHearingResult = dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult) ? (dataTableParameters.HearingResult.Split('-') != null && dataTableParameters.HearingResult.Split('-').Any() ? dataTableParameters.HearingResult.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    if (listHearingResult != null && listHearingResult.Count > 0)
                    {
                        for (int i = 0; i < listHearingResult.Count; i++)
                        {
                            if (listHearingResult[i] == "No Result")
                            {
                                listHearingResult[i] = "";
                            }
                        }
                    }

                    jobViolations = jobViolations.Where(t => listHearingResult.Contains((t.HearingResult ?? "")));
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.CreatedDate) >= DbFunctions.TruncateTime(dataTableParameters.CreatedDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.CreatedDate) <= DbFunctions.TruncateTime(dataTableParameters.CreatedDateTo));
                }

                if (dataTableParameters != null && dataTableParameters.HearingDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.HearingDate) >= DbFunctions.TruncateTime(dataTableParameters.HearingDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.HearingDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.HearingDate) <= DbFunctions.TruncateTime(dataTableParameters.HearingDateTo));
                }

                if (dataTableParameters != null && dataTableParameters.CureDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.CureDate) >= DbFunctions.TruncateTime(dataTableParameters.CureDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CureDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.CureDate) <= DbFunctions.TruncateTime(dataTableParameters.CureDateTo));
                }

                if (dataTableParameters != null && dataTableParameters.Type_ECB_DOB != null) //ECB or DOB type
                {
                    jobViolations = jobViolations.Where(x => x.Type_ECB_DOB == dataTableParameters.Type_ECB_DOB);
                }
                //var countofviolation = jobViolations.Count();
                //List<string> binnumbers = jobViolations.Select(x => x.BinNumber).ToList();

                var customerjobaceess = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == employee.Id).Select(y => y.IdJob).ToList();
                var rfpaddressids = customerjobaceess != null ? rpoContext.Jobs.Where(x => customerjobaceess.Contains(x.Id)).Select(x => x.IdRfpAddress).ToList() : null;
                var Binumbers = rfpaddressids != null ? rpoContext.RfpAddresses.Where(x => rfpaddressids.Contains(x.Id)).Select(x => x.BinNumber).ToList() : null;

                jobViolations = jobViolations.Where(x => Binumbers.Contains(x.BinNumber) || customerjobaceess.Contains((int)x.IdJob));

                var jobViolation = jobViolations.Distinct();
                var result = jobViolation
                    .AsEnumerable().OrderByDescending(d => d.HearingDate)
                    .Select(c => this.FormatAllViolationReport(c, employee.Id))
                    .AsQueryable()
                    .DataTableParameters(dataTableParameters, out recordsFiltered)
                    .ToArray().Distinct();

                return this.Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result.OrderByDescending(x => x.HearingDate)
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private AllViolationDTO FormatAllViolationReport(JobViolation jobViolation)
        {
            string strjobnumbers = "";
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string binNumber = rpoContext.JobViolations.Where(i => i.SummonsNumber == jobViolation.SummonsNumber).Select(i => i.BinNumber).FirstOrDefault();
            var addresses = rpoContext.RfpAddresses.Where(b => b.BinNumber == binNumber.TrimEnd()).ToList();
            List<int> lstjobids = new List<int>();
            string Projectnumbers = string.Empty;
            String eachaddress = "";
            foreach (var address in addresses)
            {
                List<int> jobids = rpoContext.Jobs.Where(j => j.IdRfpAddress == address.Id).Select(i => i.Id).ToList();
                if (jobids.Count > 0)
                {
                    lstjobids.AddRange(jobids);

                    // List<string> alladdresses = new List<string>();
                    //var RfpAddress = rpoContext.RfpAddresses.Where(j => j.Id == address.Id).FirstOrDefault();
                    var boroughDescription = rpoContext.Boroughes.Where(x => x.Id == address.IdBorough).Select(y => y.Description).FirstOrDefault();
                    eachaddress += address != null ? address.HouseNumber + " " + (address.Street != null ? address.Street : string.Empty)
                   + " " + boroughDescription + ", " : string.Empty;
                }
            }
            if (eachaddress != null && eachaddress != "")
            { eachaddress = eachaddress.Remove(eachaddress.Length - 2, 2); }

            if (jobViolation.IdJob != null)
            {
                Projectnumbers += Convert.ToString(jobViolation.IdJob) + ", ";
                if (jobViolation.Type_ECB_DOB == "DOB")
                    strjobnumbers = "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobViolation.IdJob + "/violation?highlighted=" + jobViolation.SummonsNumber + "&isDob=true" + "\">" + jobViolation.IdJob + "</a>, ";
                else
                    strjobnumbers += "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobViolation.IdJob + "/violation?highlighted=" + jobViolation.SummonsNumber + "\">" + jobViolation.IdJob + "</a>, ";

            }
            else
            {
                if (!string.IsNullOrEmpty(jobViolation.BinNumber))
                {
                    //List<int> rfpaddesses = rpoContext.RfpAddresses.Where(x => x.BinNumber == jobViolation.BinNumber).Select(y => y.Id).ToList();
                    //List<int> jobnumbers = rpoContext.Jobs.Where(x => rfpaddesses.Contains(x.IdRfpAddress)).Select(x => x.Id).ToList();
                    foreach (var jobId in lstjobids)
                    {
                        Projectnumbers += Convert.ToString(jobId) + ", ";
                        if (jobViolation.Type_ECB_DOB == "DOB")
                            strjobnumbers += "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobId + "/violation?highlighted=" + jobViolation.SummonsNumber + "&isDob=true" + "\">" + jobId + "</a>, ";
                        else
                            strjobnumbers += "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobId + "/violation?highlighted=" + jobViolation.SummonsNumber + "\">" + jobId + "</a>, ";
                    }
                }
            }
            if (!string.IsNullOrEmpty(strjobnumbers))
                strjobnumbers = strjobnumbers.Remove(strjobnumbers.Length - 2, 2);
            if (!string.IsNullOrEmpty(Projectnumbers))
                Projectnumbers = Projectnumbers.Remove(Projectnumbers.Length - 2, 2);
            return new AllViolationDTO
            {
                Id = jobViolation.Id,
                IdJob = jobViolation.IdJob,
                SummonsNumber = jobViolation.SummonsNumber,
                DateIssued = jobViolation.DateIssued,
                CureDate = jobViolation.CureDate,
                InspectionLocation = jobViolation.InspectionLocation,
                //JobNumber = jobViolation.Job != null ? jobViolation.Job.JobNumber : string.Empty,
                JobNumber = Projectnumbers,
                JobNumbers = strjobnumbers,
                //Address = jobViolation.Job != null && jobViolation.Job.RfpAddress != null ?
                //                               (jobViolation.Job.RfpAddress.HouseNumber + " " + (jobViolation.Job.RfpAddress.Street != null ? jobViolation.Job.RfpAddress.Street + ", " : string.Empty)
                //                             + (jobViolation.Job.RfpAddress.Borough != null ? jobViolation.Job.RfpAddress.Borough.Description : string.Empty)) : string.Empty,
                Address = eachaddress,
                IssuingAgency = jobViolation.IssuingAgency,
                InfractionCode = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Code)) : string.Empty,
                CodeSection = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.CodeSection)) : string.Empty,
                HearingDate = jobViolation.HearingDate,
                HearingLocation = jobViolation.HearingLocation,
                HearingResult = jobViolation.HearingResult,
                BalanceDue = jobViolation.BalanceDue,
                FormattedBalanceDue = jobViolation.BalanceDue != null ? Convert.ToDouble(jobViolation.BalanceDue).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) : string.Empty,
                IsFullyResolved = jobViolation.IsFullyResolved,
                ResolvedDate = jobViolation.ResolvedDate,
                StatusOfSummonsNotice = jobViolation.StatusOfSummonsNotice,
                CertificationStatus = jobViolation.CertificationStatus,
                CreatedBy = jobViolation.CreatedBy,
                LastModifiedBy = jobViolation.LastModifiedBy != null ? jobViolation.LastModifiedBy : jobViolation.CreatedBy,
                CreatedByEmployeeName = jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobViolation.LastModifiedByEmployee != null ? jobViolation.LastModifiedByEmployee.FirstName + " " + jobViolation.LastModifiedByEmployee.LastName : (jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate,
                LastModifiedDate = jobViolation.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate),
                Description = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Description)) : string.Empty,
                ViolationDescription = jobViolation.ViolationDescription,
                ViolationCategory = jobViolation.violation_category,
                RelatedECB = jobViolation.ECBnumber
            };

        }
        private AllViolationDTO FormatAllViolationReport(JobViolation jobViolation, int customerid)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            var rfpaddresses = rpoContext.RfpAddresses.Where(x => x.BinNumber == jobViolation.BinNumber).Select(x => x.Id).ToList();
            var jobs = rpoContext.Jobs.Where(x => rfpaddresses.Contains(x.IdRfpAddress)).Select(x => x.Id).ToList();
            string projectnumbers = "";
            string projectNames = "";
            String eachaddress = "";
            if (jobs != null)
            {
                var customerjobaceess = rpoContext.CustomerJobAccess.Where(x => jobs.Contains(x.IdJob) && x.IdCustomer == customerid).Select(y => y.IdJob).ToList();

                if (customerjobaceess != null)
                {
                    var lstCustomerJobAccess = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == customerid && jobs.Contains(x.IdJob)).Select(x => x.Id).ToList();
                    var projects = rpoContext.CustomerJobNames.Where(x => lstCustomerJobAccess.Contains(x.IdCustomerJobAccess)).ToList();
                    if (projects.Count > 0)
                    {
                        foreach (var p in projects)
                        {
                            projectNames += p.ProjectName.ToString() + ", ";
                        }
                        if (projectNames != null && projectNames != "")
                            projectNames = projectNames.Remove(projectNames.Length - 2, 2);
                    }
                    string binNumber = rpoContext.JobViolations.Where(i => i.SummonsNumber == jobViolation.SummonsNumber).Select(i => i.BinNumber).FirstOrDefault();
                    var address = rpoContext.RfpAddresses.Where(b => b.BinNumber == binNumber).ToList();
                    List<int> lstjobids = new List<int>();
                    foreach (var a in address)
                    {
                        var RfpAddress = rpoContext.RfpAddresses.Where(j => j.Id == a.Id).FirstOrDefault();
                        var boroughDescription = rpoContext.Boroughes.Where(x => x.Id == RfpAddress.IdBorough).Select(y => y.Description).FirstOrDefault();
                        eachaddress += RfpAddress != null ? RfpAddress.HouseNumber + " " + (RfpAddress.Street != null ? RfpAddress.Street : string.Empty)
                       + " " + boroughDescription + ", " : string.Empty;

                    }
                }

                if (customerjobaceess.Count > 0)
                {

                    foreach (var p in customerjobaceess)
                    {
                        if (jobViolation.Type_ECB_DOB == "DOB")
                            projectnumbers += "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "/job/" + p + "/violation?highlighted=" + jobViolation.SummonsNumber + "&isDob=true" + "\">" + p + "</a>, ";
                        else
                            projectnumbers += "<a href=\"" + Properties.Settings.Default.FrontEndUrl + "/job/" + p + "/violation?highlighted=" + jobViolation.SummonsNumber + "\">" + p + "</a>, ";
                    }
                    if (projectnumbers != null && projectnumbers != "")
                        projectnumbers = projectnumbers.Remove(projectnumbers.Length - 2, 2);
                }
            }
            if (eachaddress != null && eachaddress != "")
            { eachaddress = eachaddress.Remove(eachaddress.Length - 2, 2); }

            string comment = (from cmt in rpoContext.ChecklistJobViolationComments
                              where cmt.IdJobViolation == jobViolation.Id
                              orderby cmt.LastModifiedDate descending
                              select cmt.Description).FirstOrDefault();
            int partyresponsible = jobViolation.PartyResponsible != null ? (int)jobViolation.PartyResponsible : 0;
            string manualpartyresponsible = null;
            if (jobViolation.PartyResponsible != null)
            {
                manualpartyresponsible = jobViolation.PartyResponsible == 3 ? jobViolation.ManualPartyResponsible : null;
            }

            return new AllViolationDTO
            {
                Id = jobViolation.Id,
                IdJob = jobViolation.IdJob,
                SummonsNumber = jobViolation.SummonsNumber,
                DateIssued = jobViolation.DateIssued,
                CureDate = jobViolation.CureDate,
                InspectionLocation = jobViolation.InspectionLocation,
                Address = eachaddress,
                //Address = jobViolation.Job != null && jobViolation.Job.RfpAddress != null ?
                //(jobViolation.Job.RfpAddress.HouseNumber + " " + (jobViolation.Job.RfpAddress.Street != null ? jobViolation.Job.RfpAddress.Street + ", " : string.Empty)
                //+ (jobViolation.Job.RfpAddress.Borough != null ? jobViolation.Job.RfpAddress.Borough.Description : string.Empty)) : string.Empty,
                IssuingAgency = jobViolation.IssuingAgency,
                InfractionCode = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Code)) : string.Empty,
                CodeSection = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.CodeSection)) : string.Empty,
                HearingDate = jobViolation.HearingDate,
                HearingLocation = jobViolation.HearingLocation,
                HearingResult = jobViolation.HearingResult,
                BalanceDue = jobViolation.BalanceDue,
                FormattedBalanceDue = jobViolation.BalanceDue != null ? Convert.ToDouble(jobViolation.BalanceDue).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")) : string.Empty,
                IsFullyResolved = jobViolation.IsFullyResolved,
                ResolvedDate = jobViolation.ResolvedDate,
                StatusOfSummonsNotice = jobViolation.StatusOfSummonsNotice,
                CertificationStatus = jobViolation.CertificationStatus,
                CreatedBy = jobViolation.CreatedBy,
                LastModifiedBy = jobViolation.LastModifiedBy != null ? jobViolation.LastModifiedBy : jobViolation.CreatedBy,
                CreatedByEmployeeName = jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = jobViolation.LastModifiedByEmployee != null ? jobViolation.LastModifiedByEmployee.FirstName + " " + jobViolation.LastModifiedByEmployee.LastName : (jobViolation.CreatedByEmployee != null ? jobViolation.CreatedByEmployee.FirstName + " " + jobViolation.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate,
                LastModifiedDate = jobViolation.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (jobViolation.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobViolation.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobViolation.CreatedDate),
                Description = jobViolation.explanationOfCharges != null ? String.Join(", ", jobViolation.explanationOfCharges.Select(c => c.Description)) : string.Empty,
                JobNames = projectNames,
                JobNumbers = projectnumbers,
                ECBNumber = jobViolation.ECBnumber,
                PartyResponsible = partyresponsible,
                ManualPartyResponsible = manualpartyresponsible,
                Comment = comment,
                Status = jobViolation.Status,
                RelatedECB = jobViolation.ECBnumber,
                ViolationCategory = jobViolation.violation_category,
                ViolationDescription = jobViolation.ViolationDescription
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
        /// Get The status of allviolation list in dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]

        [Route("api/ReportAllViolations/statusdropdown")]
        public IHttpActionResult GetStatusDropdown()
        {
            var result = this.rpoContext.JobViolations.AsEnumerable().Where(x => x.StatusOfSummonsNotice != null).Select(c => new
            {
                Id = c.StatusOfSummonsNotice,
                ItemName = c.StatusOfSummonsNotice

            }).Distinct().ToList();
            result.Insert(0, new { Id = "No Status", ItemName = "No Status" });

            return Ok(result);
        }
        /// <summary>
        ///  Get The status of allviolation list in dropdown of hearing date
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/ReportAllViolations/hearingResultdropdown")]
        public IHttpActionResult GetHearingResultDropdown()
        {
            var result = this.rpoContext.JobViolations.AsEnumerable().Where(x => (x.HearingResult ?? "") != "").Select(c => new
            {
                Id = c.HearingResult,
                ItemName = c.HearingResult

            }).Distinct().ToList();
            result.Insert(0, new { Id = "No Result", ItemName = "No Result" });

            return Ok(result);
        }
        /// <summary>
        ///  Get The status of allviolation list in dropdown for certification status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]

        [Route("api/ReportAllViolations/certificationstatusdropdown")]
        public IHttpActionResult GetCertificationStatusDropdown()
        {
            var result = this.rpoContext.JobViolations.AsEnumerable().Where(x => x.CertificationStatus != null).Select(c => new
            {
                Id = c.CertificationStatus,
                ItemName = c.CertificationStatus

            }).Distinct().ToList();
            result.Insert(0, new { Id = "No Status", ItemName = "No Certification Status" });
            return Ok(result);
        }

        /// <summary>
        ///  Get The status of allviolation list and export excel and send email
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportAllViolations/exporttoexcelemail")]
        public IHttpActionResult PostExportAllViolationToExcelEmail(AllViolationDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                //var jobViolations = this.rpoContext.JobViolations.Include("Job.RfpAddress.Borough").AsQueryable();
                var jobViolations = (from vio in rpoContext.JobViolations
                                     join rfps in rpoContext.RfpAddresses on vio.BinNumber equals rfps.BinNumber
                                     join job in rpoContext.Jobs on rfps.Id equals job.IdRfpAddress
                                     where vio.Type_ECB_DOB == dataTableParameters.Type_ECB_DOB
                                     select new { vio, job }).AsQueryable();

                string hearingDate = string.Empty;
                if (dataTableParameters != null && dataTableParameters.SummonsNumber != null)
                {
                    jobViolations = jobViolations.Where(x => x.vio.SummonsNumber.Contains(dataTableParameters.SummonsNumber));
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => joNumbers.Contains(x.job.Id));
                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobCompanyTeam.Contains((x.job.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.IsFullyResolved != null)
                {
                    jobViolations = jobViolations.Where(x => x.vio.IsFullyResolved == dataTableParameters.IsFullyResolved);

                    string iIsFullyResolved = dataTableParameters.IsFullyResolved != null && Convert.ToBoolean(dataTableParameters.IsFullyResolved) == true ? "Yes" : "No";
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(iIsFullyResolved) ? " | " : string.Empty) + (!string.IsNullOrEmpty(iIsFullyResolved) ? "Fully Resolved : " + iIsFullyResolved : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobContactTeam.Contains((x.job.IdContact.Value)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus))
                {
                    List<string> listViolationStatus = dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus) ? (dataTableParameters.ViolationStatus.Split('-') != null && dataTableParameters.ViolationStatus.Split('-').Any() ? dataTableParameters.ViolationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredViolationStatus = string.Join(", ", listViolationStatus.Select(x => x));
                    if (listViolationStatus != null && listViolationStatus.Count > 0)
                    {
                        for (int i = 0; i < listViolationStatus.Count; i++)
                        {
                            if (listViolationStatus[i] == "No Status")
                            {
                                listViolationStatus[i] = "";
                            }
                        }
                    }
                    jobViolations = jobViolations.Where(t => listViolationStatus.Contains((t.vio.StatusOfSummonsNotice ?? "")));

                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredViolationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredViolationStatus) ? "Status : " + filteredViolationStatus : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus))
                {
                    List<string> listcertificationStatus = dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus) ? (dataTableParameters.CertificationStatus.Split('-') != null && dataTableParameters.CertificationStatus.Split('-').Any() ? dataTableParameters.CertificationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredcertificationStatus = string.Join(", ", listcertificationStatus.Select(x => x));

                    if (listcertificationStatus != null && listcertificationStatus.Count > 0)
                    {
                        for (int i = 0; i < listcertificationStatus.Count; i++)
                        {
                            if (listcertificationStatus[i] == "No Status")
                            {
                                listcertificationStatus[i] = "";
                            }
                        }
                    }

                    jobViolations = jobViolations.Where(t => listcertificationStatus.Contains((t.vio.CertificationStatus ?? "")));

                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredcertificationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredcertificationStatus) ? "Certification Status : " + filteredcertificationStatus : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult))
                {
                    List<string> listHearingResult = dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult) ? (dataTableParameters.HearingResult.Split('-') != null && dataTableParameters.HearingResult.Split('-').Any() ? dataTableParameters.HearingResult.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredHearingResult = string.Join(", ", listHearingResult.Select(x => x));

                    if (listHearingResult != null && listHearingResult.Count > 0)
                    {
                        for (int i = 0; i < listHearingResult.Count; i++)
                        {
                            if (listHearingResult[i] == "No Result")
                            {
                                listHearingResult[i] = "";
                            }
                        }
                    }

                    jobViolations = jobViolations.Where(t => listHearingResult.Contains((t.vio.HearingResult ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredHearingResult) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredHearingResult) ? "Hearing Result : " + filteredHearingResult : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CreatedDate) >= DbFunctions.TruncateTime(dataTableParameters.CreatedDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CreatedDate) <= DbFunctions.TruncateTime(dataTableParameters.CreatedDateTo));
                }

                string filteredhearingDate = string.Empty;
                if (dataTableParameters != null && dataTableParameters.HearingDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.HearingDate) >= DbFunctions.TruncateTime(dataTableParameters.HearingDateFrom));
                    filteredhearingDate = filteredhearingDate + Convert.ToDateTime(dataTableParameters.HearingDateFrom).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.HearingDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.HearingDate) <= DbFunctions.TruncateTime(dataTableParameters.HearingDateTo));
                    filteredhearingDate = filteredhearingDate + " - " + Convert.ToDateTime(dataTableParameters.HearingDateTo).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredhearingDate) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredhearingDate) ? "Hearing Date : " + filteredhearingDate : string.Empty);

                if (dataTableParameters != null && dataTableParameters.CureDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CureDate) >= DbFunctions.TruncateTime(dataTableParameters.CureDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CureDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CureDate) <= DbFunctions.TruncateTime(dataTableParameters.CureDateTo));
                }
                //if (dataTableParameters != null && dataTableParameters.Type_ECB_DOB != null) //ECB or DOB type
                //{
                //    jobViolations = jobViolations.Where(x => x.vio.Type_ECB_DOB == dataTableParameters.Type_ECB_DOB);
                //}

                string direction = string.Empty;
                string orderBy = string.Empty;

                List<AllViolationDTO> result = new List<AllViolationDTO>();

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
                    if (orderBy.Trim().ToLower() == "Id".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.Id);
                    if (orderBy.Trim().ToLower() == "SummonsNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.SummonsNumber);
                    if (orderBy.Trim().ToLower() == "DateIssued".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.DateIssued);
                    if (orderBy.Trim().ToLower() == "CureDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CureDate);
                    if (orderBy.Trim().ToLower() == "InspectionLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.InspectionLocation);
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.job.Id);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.job.RfpAddress.HouseNumber);
                    if (orderBy.Trim().ToLower() == "IssuingAgency".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.IssuingAgency);
                    if (orderBy.Trim().ToLower() == "HearingDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingDate);
                    if (orderBy.Trim().ToLower() == "HearingLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingLocation);
                    if (orderBy.Trim().ToLower() == "HearingResult".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingResult);
                    if (orderBy.Trim().ToLower() == "BalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "FormattedBalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "IsFullyResolved".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.IsFullyResolved);

                    if (orderBy.Trim().ToLower() == "ResolvedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.ResolvedDate);
                    if (orderBy.Trim().ToLower() == "StatusOfSummonsNotice".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.StatusOfSummonsNotice);
                    if (orderBy.Trim().ToLower() == "CertificationStatus".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CertificationStatus);
                    if (orderBy.Trim().ToLower() == "CreatedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CreatedBy);
                    if (orderBy.Trim().ToLower() == "LastModifiedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.LastModifiedBy);
                    if (orderBy.Trim().ToLower() == "CreatedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CreatedDate);
                    if (orderBy.Trim().ToLower() == "LastModifiedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.LastModifiedDate);
                }
                else
                {

                    if (orderBy.Trim().ToLower() == "Id".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.Id);
                    if (orderBy.Trim().ToLower() == "SummonsNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.SummonsNumber);
                    if (orderBy.Trim().ToLower() == "DateIssued".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.DateIssued);
                    if (orderBy.Trim().ToLower() == "CureDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CureDate);
                    if (orderBy.Trim().ToLower() == "InspectionLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.InspectionLocation);
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.Id);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.job.RfpAddress.HouseNumber);
                    if (orderBy.Trim().ToLower() == "IssuingAgency".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.IssuingAgency);
                    if (orderBy.Trim().ToLower() == "HearingDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingDate);
                    if (orderBy.Trim().ToLower() == "HearingLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingLocation);
                    if (orderBy.Trim().ToLower() == "HearingResult".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingResult);
                    if (orderBy.Trim().ToLower() == "BalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "FormattedBalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "IsFullyResolved".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.IsFullyResolved);

                    if (orderBy.Trim().ToLower() == "ResolvedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.ResolvedDate);
                    if (orderBy.Trim().ToLower() == "StatusOfSummonsNotice".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.StatusOfSummonsNotice);
                    if (orderBy.Trim().ToLower() == "CertificationStatus".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CertificationStatus);
                    if (orderBy.Trim().ToLower() == "CreatedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CreatedBy);
                    if (orderBy.Trim().ToLower() == "LastModifiedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.LastModifiedBy);
                    if (orderBy.Trim().ToLower() == "CreatedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CreatedDate);
                    if (orderBy.Trim().ToLower() == "LastModifiedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.LastModifiedDate);
                }
                var jobViolation = jobViolations.Distinct();
                result = jobViolation
                 .AsEnumerable().OrderByDescending(x => x.vio.HearingDate)
                 .Select(c => this.FormatAllViolationReport(c.vio))
                 .AsQueryable()
                 .ToList();

                string exportFilename = "AllViolationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                string exportFilePath = ExportToExcel(hearingDate, result, exportFilename, dataTableParameters.Type_ECB_DOB);

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
        ///  Get The status of allviolation list and export pdf and send email
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportAllViolations/exporttopdfemail")]
        public IHttpActionResult PostExportAllViolationToPdfEmail(AllViolationDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                //var jobViolations = this.rpoContext.JobViolations.Include("Job.RfpAddress.Borough").AsQueryable();
                var jobViolations = (from vio in rpoContext.JobViolations
                                     join rfps in rpoContext.RfpAddresses on vio.BinNumber equals rfps.BinNumber
                                     join job in rpoContext.Jobs on rfps.Id equals job.IdRfpAddress
                                     where vio.Type_ECB_DOB == dataTableParameters.Type_ECB_DOB
                                     select new { vio, job }).AsQueryable();

                string hearingDate = string.Empty;
                if (dataTableParameters != null && dataTableParameters.SummonsNumber != null)
                {
                    jobViolations = jobViolations.Where(x => x.vio.SummonsNumber.Contains(dataTableParameters.SummonsNumber));
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => joNumbers.Contains(x.job.Id));
                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobCompanyTeam.Contains((x.job.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.IsFullyResolved != null)
                {
                    jobViolations = jobViolations.Where(x => x.vio.IsFullyResolved == dataTableParameters.IsFullyResolved);

                    string iIsFullyResolved = dataTableParameters.IsFullyResolved != null && Convert.ToBoolean(dataTableParameters.IsFullyResolved) == true ? "Yes" : "No";
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(iIsFullyResolved) ? " | " : string.Empty) + (!string.IsNullOrEmpty(iIsFullyResolved) ? "Fully Resolved : " + iIsFullyResolved : string.Empty);
                }

                if (dataTableParameters != null && !string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobViolations = jobViolations.Where(x => jobContactTeam.Contains((x.job.IdContact.Value)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus))
                {
                    List<string> listViolationStatus = dataTableParameters.ViolationStatus != null && !string.IsNullOrEmpty(dataTableParameters.ViolationStatus) ? (dataTableParameters.ViolationStatus.Split('-') != null && dataTableParameters.ViolationStatus.Split('-').Any() ? dataTableParameters.ViolationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredViolationStatus = string.Join(", ", listViolationStatus.Select(x => x));
                    if (listViolationStatus != null && listViolationStatus.Count > 0)
                    {
                        for (int i = 0; i < listViolationStatus.Count; i++)
                        {
                            if (listViolationStatus[i] == "No Status")
                            {
                                listViolationStatus[i] = "";
                            }
                        }
                    }
                    jobViolations = jobViolations.Where(t => listViolationStatus.Contains((t.vio.StatusOfSummonsNotice ?? "")));

                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredViolationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredViolationStatus) ? "Status : " + filteredViolationStatus : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus))
                {
                    List<string> listcertificationStatus = dataTableParameters.CertificationStatus != null && !string.IsNullOrEmpty(dataTableParameters.CertificationStatus) ? (dataTableParameters.CertificationStatus.Split('-') != null && dataTableParameters.CertificationStatus.Split('-').Any() ? dataTableParameters.CertificationStatus.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredcertificationStatus = string.Join(", ", listcertificationStatus.Select(x => x));

                    if (listcertificationStatus != null && listcertificationStatus.Count > 0)
                    {
                        for (int i = 0; i < listcertificationStatus.Count; i++)
                        {
                            if (listcertificationStatus[i] == "No Status")
                            {
                                listcertificationStatus[i] = "";
                            }
                        }
                    }

                    jobViolations = jobViolations.Where(t => listcertificationStatus.Contains((t.vio.CertificationStatus ?? "")));

                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredcertificationStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredcertificationStatus) ? "Certification Status : " + filteredcertificationStatus : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult))
                {
                    List<string> listHearingResult = dataTableParameters.HearingResult != null && !string.IsNullOrEmpty(dataTableParameters.HearingResult) ? (dataTableParameters.HearingResult.Split('-') != null && dataTableParameters.HearingResult.Split('-').Any() ? dataTableParameters.HearingResult.Split('-').Select(x => Convert.ToString(x)).ToList() : new List<String>()) : new List<String>();
                    string filteredHearingResult = string.Join(", ", listHearingResult.Select(x => x));

                    if (listHearingResult != null && listHearingResult.Count > 0)
                    {
                        for (int i = 0; i < listHearingResult.Count; i++)
                        {
                            if (listHearingResult[i] == "No Result")
                            {
                                listHearingResult[i] = "";
                            }
                        }
                    }

                    jobViolations = jobViolations.Where(t => listHearingResult.Contains((t.vio.HearingResult ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredHearingResult) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredHearingResult) ? "Hearing Result : " + filteredHearingResult : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CreatedDate) >= DbFunctions.TruncateTime(dataTableParameters.CreatedDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CreatedDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CreatedDate) <= DbFunctions.TruncateTime(dataTableParameters.CreatedDateTo));
                }

                string filteredhearingDate = string.Empty;
                if (dataTableParameters != null && dataTableParameters.HearingDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.HearingDate) >= DbFunctions.TruncateTime(dataTableParameters.HearingDateFrom));
                    filteredhearingDate = filteredhearingDate + Convert.ToDateTime(dataTableParameters.HearingDateFrom).ToString(Common.ExportReportDateFormat);
                }

                if (dataTableParameters != null && dataTableParameters.HearingDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.HearingDate) <= DbFunctions.TruncateTime(dataTableParameters.HearingDateTo));
                    filteredhearingDate = filteredhearingDate + " - " + Convert.ToDateTime(dataTableParameters.HearingDateTo).ToString(Common.ExportReportDateFormat);
                }

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredhearingDate) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredhearingDate) ? "Hearing Date : " + filteredhearingDate : string.Empty);

                if (dataTableParameters != null && dataTableParameters.CureDateFrom != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CureDate) >= DbFunctions.TruncateTime(dataTableParameters.CureDateFrom));
                }

                if (dataTableParameters != null && dataTableParameters.CureDateTo != null)
                {
                    jobViolations = jobViolations.Where(x => DbFunctions.TruncateTime(x.vio.CureDate) <= DbFunctions.TruncateTime(dataTableParameters.CureDateTo));
                }
                //if (dataTableParameters != null && dataTableParameters.Type_ECB_DOB != null) //ECB or DOB type
                //{
                //    jobViolations = jobViolations.Where(x => x.vio.Type_ECB_DOB == dataTableParameters.Type_ECB_DOB);
                //}

                string direction = string.Empty;
                string orderBy = string.Empty;

                List<AllViolationDTO> result = new List<AllViolationDTO>();

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
                    if (orderBy.Trim().ToLower() == "Id".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.Id);
                    if (orderBy.Trim().ToLower() == "SummonsNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.SummonsNumber);
                    if (orderBy.Trim().ToLower() == "DateIssued".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.DateIssued);
                    if (orderBy.Trim().ToLower() == "CureDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CureDate);
                    if (orderBy.Trim().ToLower() == "InspectionLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.InspectionLocation);
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.job.Id);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.job.RfpAddress.HouseNumber);
                    if (orderBy.Trim().ToLower() == "IssuingAgency".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.IssuingAgency);
                    if (orderBy.Trim().ToLower() == "HearingDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingDate);
                    if (orderBy.Trim().ToLower() == "HearingLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingLocation);
                    if (orderBy.Trim().ToLower() == "HearingResult".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.HearingResult);
                    if (orderBy.Trim().ToLower() == "BalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "FormattedBalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "IsFullyResolved".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.IsFullyResolved);
                    if (orderBy.Trim().ToLower() == "ResolvedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.ResolvedDate);
                    if (orderBy.Trim().ToLower() == "StatusOfSummonsNotice".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.StatusOfSummonsNotice);
                    if (orderBy.Trim().ToLower() == "CertificationStatus".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CertificationStatus);
                    if (orderBy.Trim().ToLower() == "CreatedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CreatedBy);
                    if (orderBy.Trim().ToLower() == "LastModifiedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.LastModifiedBy);
                    if (orderBy.Trim().ToLower() == "CreatedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.CreatedDate);
                    if (orderBy.Trim().ToLower() == "LastModifiedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderByDescending(o => o.vio.LastModifiedDate);
                }
                else
                {

                    if (orderBy.Trim().ToLower() == "Id".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.Id);
                    if (orderBy.Trim().ToLower() == "SummonsNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.SummonsNumber);
                    if (orderBy.Trim().ToLower() == "DateIssued".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.DateIssued);
                    if (orderBy.Trim().ToLower() == "CureDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CureDate);
                    if (orderBy.Trim().ToLower() == "InspectionLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.InspectionLocation);
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.job.Id);
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.job.RfpAddress.HouseNumber);
                    if (orderBy.Trim().ToLower() == "IssuingAgency".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.IssuingAgency);
                    if (orderBy.Trim().ToLower() == "HearingDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingDate);
                    if (orderBy.Trim().ToLower() == "HearingLocation".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingLocation);
                    if (orderBy.Trim().ToLower() == "HearingResult".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.HearingResult);
                    if (orderBy.Trim().ToLower() == "BalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "FormattedBalanceDue".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.BalanceDue);
                    if (orderBy.Trim().ToLower() == "IsFullyResolved".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.IsFullyResolved);

                    if (orderBy.Trim().ToLower() == "ResolvedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.ResolvedDate);
                    if (orderBy.Trim().ToLower() == "StatusOfSummonsNotice".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.StatusOfSummonsNotice);
                    if (orderBy.Trim().ToLower() == "CertificationStatus".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CertificationStatus);
                    if (orderBy.Trim().ToLower() == "CreatedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CreatedBy);
                    if (orderBy.Trim().ToLower() == "LastModifiedBy".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.LastModifiedBy);
                    if (orderBy.Trim().ToLower() == "CreatedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.CreatedDate);
                    if (orderBy.Trim().ToLower() == "LastModifiedDate".Trim().ToLower())
                        jobViolations = jobViolations.OrderBy(o => o.vio.LastModifiedDate);
                }

                var jobViolation = jobViolations.Distinct();
                result = jobViolation
                 .AsEnumerable().OrderByDescending(x => x.vio.HearingDate)
                 .Select(c => this.FormatAllViolationReport(c.vio))
                 .AsQueryable()
                 .ToList();

                string exportFilename = "AllViolationReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";

                string exportFilePath = ExportToPdf(hearingDate, result, exportFilename, dataTableParameters.Type_ECB_DOB);

                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

                string path = APIUrl + "/UploadedDocuments/ReportExportToExcel/" + exportFilename;

                return Ok(new { Filepath = exportFilePath, NewPath = path, ReportName = exportFilename });
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