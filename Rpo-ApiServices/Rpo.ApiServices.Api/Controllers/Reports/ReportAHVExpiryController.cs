using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Rpo.ApiServices.Model;
using Rpo.ApiServices.Model.Models;
using Rpo.ApiServices.Api.Tools;
using Rpo.ApiServices.Model.Models;
using System.Collections.Generic;
using System.IO;
using System.Web;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Rpo.ApiServices.Api.DataTable;
using Rpo.ApiServices.Api.Filters;
using Rpo.ApiServices.Api.Controllers.Reports.Models;
using System.Globalization;
using NPOI.SS.Util;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class ReportAHVExpiryController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the Report for AHVExpiry Report apply the filser and sorting.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        [HttpGet]
        public IHttpActionResult GetReportAHVExpiry([FromUri] AHVExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Rpo.ApiServices.Api.Tools.Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewAHVPermitExpiryReport))
            {
                int idPW517Document = Enums.Document.AfterHoursPermitApplicationPW517.GetHashCode();

            var Jobslist = (from d in rpoContext.Jobs select d).ToList();

            List<AHVExpiryReport> objAHVExpiryReportList = new List<AHVExpiryReport>();

            string AHVReferenceNumber = string.Empty;
            if (dataTableParameters != null && dataTableParameters.AHVReferenceNumber != null && !string.IsNullOrEmpty(dataTableParameters.AHVReferenceNumber))
            {
                AHVReferenceNumber = dataTableParameters.AHVReferenceNumber;
            }
            List<Rpo.ApiServices.Model.Models.JobDocument> jobDocumentResponse = jobDocumentResponse = rpoContext.JobDocuments.Where(d => d.IdDocument == idPW517Document).Include("JobDocumentFields.DocumentField.Field").ToList();


            if (dataTableParameters != null && dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
            {
                List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                jobDocumentResponse = jobDocumentResponse.Where(d => joNumbers.Contains(d.IdJob)).ToList();
            }


            if (dataTableParameters != null && dataTableParameters.IdJobCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdJobCompany))
            {
                List<int> jobCompanyTeam = dataTableParameters.IdJobCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdJobCompany) ? (dataTableParameters.IdJobCompany.Split('-') != null && dataTableParameters.IdJobCompany.Split('-').Any() ? dataTableParameters.IdJobCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                jobDocumentResponse = jobDocumentResponse.Where(d => jobCompanyTeam.Contains((d.Job.IdCompany != null ? d.Job.IdCompany.Value : 0))).ToList();
            }


            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            CultureInfo provider = CultureInfo.CurrentCulture;

            if (dataTableParameters != null && dataTableParameters.ExpiresFromDate != null)
            {
                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresFromDate.ToString(), "MM/dd/yyyy", provider);
                //DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresFromDate.ToString(), "dd/MM/yyyy", provider);
                fromDate = dataTableParameters.ExpiresFromDate.Value;
            }
            else
            {
                //string fmdate = DateTime.Now.AddDays(-30).ToShortDateString();
                fromDate = DateTime.Now.AddDays(-30);

                //fromDate = DateTime.ParseExact(fmdate, "dd/MM/yyyy", provider);
                //fromDate = DateTime.ParseExact(fmdate, "MM/dd/yyyy", provider);
            }


            if (dataTableParameters != null && dataTableParameters.ExpiresToDate != null)
            {
                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresToDate.ToString(), "MM/dd/yyyy", provider);

                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresToDate.ToString(), "dd/MM/yyyy", provider);

                toDate = dataTableParameters.ExpiresToDate.Value;
            }
            else
            {
                //string todatenw = DateTime.Now.AddDays(+30).ToShortDateString();
                toDate = DateTime.Now.AddDays(+30);
                //toDate = DateTime.ParseExact(todatenw, "MM/dd/yyyy", provider);
                // toDate = DateTime.ParseExact(todatenw, "dd/MM/yyyy", provider);
            }

            if (dataTableParameters != null && dataTableParameters.AHVReferenceNumber != null && AHVReferenceNumber != "")
            {
                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "AHVReferenceNumber" && b.Value == AHVReferenceNumber)).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }
            if (dataTableParameters != null && dataTableParameters.Type != null && !string.IsNullOrEmpty(dataTableParameters.Type))
            {
                List<string> jobType = dataTableParameters.Type != null && !string.IsNullOrEmpty(dataTableParameters.Type) ? (dataTableParameters.Type.Split('-') != null && dataTableParameters.Type.Split('-').Any() ? dataTableParameters.Type.ToLower().Trim().Split('-').Select(x => x).ToList() : new List<string>()) : new List<string>();

                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "Type" && jobType.Contains(b.ActualValue.ToLower().Trim() ?? ""))).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }
            if (dataTableParameters != null && dataTableParameters.IdApplicantCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicantCompany))
            {
                List<string> objApplicantCompany = dataTableParameters.IdApplicantCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicantCompany) ? (dataTableParameters.IdApplicantCompany.Split('-') != null && dataTableParameters.IdApplicantCompany.ToLower().Trim().Split('-').Any() ? dataTableParameters.IdApplicantCompany.ToLower().Trim().Split('-').Select(x => x).ToList() : new List<string>()) : new List<string>();

                var strconteacts = (from d in rpoContext.JobContacts where objApplicantCompany.Contains(d.IdCompany.ToString() ?? "") select d.Id.ToString()).ToList();

                var aatmp = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "txtLast")).ToList();

                var bb = aatmp.Where(d => d.JobDocumentFields.Any(b => objApplicantCompany.Contains(b.Value ?? ""))).ToList();

                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "txtLast" && strconteacts.Contains(b.Value.ToString() ?? ""))).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }

            if (fromDate != null && toDate != null)
            {
                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "Start Date"

                && DateTime.ParseExact(b.Value, "MM/dd/yy", provider).Date >= fromDate.Date && DateTime.ParseExact(b.Value, "MM/dd/yy", provider).Date <= toDate.Date)).ToList();
                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }

            foreach (var item in jobDocumentResponse)
            {
                List<JobDocumentField> jobDocumentFields = item.JobDocumentFields.ToList();


                Job objjobs = (from d in this.rpoContext.Jobs.Include("Borough").Include("Applications")
            .Include("JobApplicationTypes")
                               where d.Id == item.IdJob
                               select d).FirstOrDefault();

                JobDocumentField jobDocumentField = new JobDocumentField();

                List<JobDocumentField> objDocFields = new List<JobDocumentField>();

                AHVExpiryReport objAHVExpiryReport = new AHVExpiryReport();

                objAHVExpiryReport.IdJob = item.IdJob.ToString();
                objAHVExpiryReport.JobNumber = item.Job.JobNumber.ToString();
                objAHVExpiryReport.CreatedModifiedDate = item.LastModifiedDate != null ? item.LastModifiedDate : item.CreatedDate;

                var objAHvReferenceNumber = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "AHVReferenceNumber") select d.Value).FirstOrDefault();

                objAHVExpiryReport.AHVReferenceNumber = objAHvReferenceNumber != null ? objAHvReferenceNumber : "";

                var objType = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Type") select d.ActualValue).FirstOrDefault();

                objAHVExpiryReport.Type = objType != null ? objType : "";


                objAHVExpiryReport.JobAddress = objjobs.HouseNumber + " " + objjobs.StreetNumber + ", " + objjobs.Borough.Description;

                var objApplicationNo = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Application") select d).FirstOrDefault();

                objAHVExpiryReport.ApplicationNo = objApplicationNo != null && objApplicationNo.JobDocument != null && objApplicationNo.JobDocument.JobApplication != null && objApplicationNo.JobDocument.JobApplication.ApplicationNumber != null ? objApplicationNo.JobDocument.JobApplication.ApplicationNumber : "";

                var objWorkPermit = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Work Permits") select d.ActualValue).FirstOrDefault();
                objAHVExpiryReport.WorkPermitType = objWorkPermit != null ? objWorkPermit : "";

                var objApplicant = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "txtLast") select d.Value).FirstOrDefault();

                string ApplicantCompany = string.Empty;
                DateTime? ApplicantExpiryDate = null;
                int? ApplicantId = null;
                if (objApplicant != null)
                {
                    int applicantid = int.Parse(objApplicant);
                    var objapplicantCompany = (from d in rpoContext.JobContacts where d.Id == applicantid select d).FirstOrDefault();
                    if (objapplicantCompany != null && objapplicantCompany.Company != null)
                    {
                        ApplicantCompany = objapplicantCompany.Company.Name;
                        ApplicantId = objapplicantCompany.Company.Id;
                        ApplicantExpiryDate = objapplicantCompany.Company.TrackingExpiry;
                    }
                }

                objAHVExpiryReport.ApplicantCompany = ApplicantCompany;


                objAHVExpiryReport.TrackingExpiryDate = ApplicantExpiryDate != null ? ApplicantExpiryDate : null;
                objAHVExpiryReport.JobCompany = item.Job != null && item.Job.Company != null ? item.Job.Company.Name : string.Empty;
                objAHVExpiryReport.JobApplicationType = string.Join(",", item.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
                string[] objstrarray = item.DocumentDescription.Split(new char[] { '|' });

                string Description = string.Empty;

                foreach (var items in objstrarray)
                {
                    if (items.Contains("Dates"))
                    {
                        Description = items.ToString();
                    }
                }

                var objIssueDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Issued Date") select d.Value).FirstOrDefault();

                if (objIssueDate != null && !string.IsNullOrEmpty(objIssueDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objIssueDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.IssuedDate = dtvalue;
                }

                var objSubmittedDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Submitted Date") select d.Value).FirstOrDefault();

                if (objSubmittedDate != null && !string.IsNullOrEmpty(objSubmittedDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objSubmittedDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.SubmittedDate = dtvalue;
                }

                var objNextDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Next Date") select d.Value).FirstOrDefault();
                if (objNextDate != null && !string.IsNullOrEmpty(objNextDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objNextDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.NextDate = dtvalue;
                }
                objAHVExpiryReport.IdJobCompany = item.Job != null ? item.Job.IdCompany : null;
                objAHVExpiryReport.IdApplicantCompany = ApplicantId;
                objAHVExpiryReport.DocumentDescription = Description != "" ? Description : "";

                objAHVExpiryReportList.Add(objAHVExpiryReport);

                endloop:
                Console.WriteLine("");

            }
            string direction = string.Empty;
            string orderBy = string.Empty;

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
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobNumber).ToList();
                if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobAddress).ToList();
                if (orderBy.Trim().ToLower() == "JobCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobCompany).ToList();
                if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobApplicationType).ToList();
                if (orderBy.Trim().ToLower() == "TrackingExpiryDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.TrackingExpiryDate).ToList();
                if (orderBy.Trim().ToLower() == "ApplicationNo".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.ApplicationNo).ToList();
                if (orderBy.Trim().ToLower() == "ApplicantCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.ApplicantCompany).ToList();
                if (orderBy.Trim().ToLower() == "WorkPermitType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.WorkPermitType).ToList();
                if (orderBy.Trim().ToLower() == "AHVReferenceNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.AHVReferenceNumber).ToList();
                if (orderBy.Trim().ToLower() == "Type".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.Type).ToList();
                if (orderBy.Trim().ToLower() == "DocumentDescription".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.DocumentDescription).ToList();
                if (orderBy.Trim().ToLower() == "SubmittedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.SubmittedDate).ToList();
                if (orderBy.Trim().ToLower() == "IssuedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.IssuedDate).ToList();
                if (orderBy.Trim().ToLower() == "NextDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.NextDate).ToList();
                if (orderBy.Trim().ToLower() == "CreatedModifiedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.CreatedModifiedDate).ToList();
            }
            else
            {
                if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobNumber).ToList();
                if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobAddress).ToList();
                if (orderBy.Trim().ToLower() == "JobCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobCompany).ToList();
                if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobApplicationType).ToList();
                if (orderBy.Trim().ToLower() == "TrackingExpiryDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.TrackingExpiryDate).ToList();
                if (orderBy.Trim().ToLower() == "ApplicationNo".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.ApplicationNo).ToList();
                if (orderBy.Trim().ToLower() == "ApplicantCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.ApplicantCompany).ToList();
                if (orderBy.Trim().ToLower() == "WorkPermitType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.WorkPermitType).ToList();
                if (orderBy.Trim().ToLower() == "AHVReferenceNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.AHVReferenceNumber).ToList();
                if (orderBy.Trim().ToLower() == "Type".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.Type).ToList();
                if (orderBy.Trim().ToLower() == "DocumentDescription".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.DocumentDescription).ToList();
                if (orderBy.Trim().ToLower() == "SubmittedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.SubmittedDate).ToList();
                if (orderBy.Trim().ToLower() == "IssuedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.IssuedDate).ToList();
                if (orderBy.Trim().ToLower() == "NextDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.NextDate).ToList();
                if (orderBy.Trim().ToLower() == "CreatedModifiedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.CreatedModifiedDate).ToList();
            }
            

            return this.Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = objAHVExpiryReportList.Count(),
                RecordsTotal = objAHVExpiryReportList.Count(),
                Data = objAHVExpiryReportList
            });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }



        /// <summary>
        /// Gets the Report for AHVExpiry Report apply the filser and sorting. and Export to Excel
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportAHVExpiry/exporttoexcel")]
        public IHttpActionResult PostExportAHVExpiryToExcel(AHVExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Rpo.ApiServices.Api.Tools.Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                string currentTimeZone = Rpo.ApiServices.Api.Tools.Common.FetchHeaderValues(this.Request, Rpo.ApiServices.Api.Tools.Common.CurrentTimezoneHeaderKey);

            #region Filter

            int idPW517Document = Enums.Document.AfterHoursPermitApplicationPW517.GetHashCode();

            var Jobslist = (from d in rpoContext.Jobs select d).ToList();

            List<AHVExpiryReport> objAHVExpiryReportList = new List<AHVExpiryReport>();
            string hearingDate = string.Empty;

            string AHVReferenceNumber = string.Empty;
            if (dataTableParameters != null && dataTableParameters.AHVReferenceNumber != null && !string.IsNullOrEmpty(dataTableParameters.AHVReferenceNumber))
            {
                AHVReferenceNumber = dataTableParameters.AHVReferenceNumber;

                hearingDate = hearingDate + (!string.IsNullOrEmpty(AHVReferenceNumber) && !string.IsNullOrEmpty(AHVReferenceNumber) ? " | " : string.Empty) + (!string.IsNullOrEmpty(AHVReferenceNumber) ? "AHVReferenceNumber : " + AHVReferenceNumber : string.Empty);
            }
            List<Rpo.ApiServices.Model.Models.JobDocument> jobDocumentResponse = jobDocumentResponse = rpoContext.JobDocuments.Where(d => d.IdDocument == idPW517Document).Include("JobDocumentFields.DocumentField.Field").ToList();


            if (dataTableParameters != null && dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
            {
                List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                jobDocumentResponse = jobDocumentResponse.Where(d => joNumbers.Contains(d.IdJob)).ToList();

                string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Project #: " + filteredJob : string.Empty);
            }


            if (dataTableParameters != null && dataTableParameters.IdJobCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdJobCompany))
            {
                List<int> jobCompanyTeam = dataTableParameters.IdJobCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdJobCompany) ? (dataTableParameters.IdJobCompany.Split('-') != null && dataTableParameters.IdJobCompany.Split('-').Any() ? dataTableParameters.IdJobCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                jobDocumentResponse = jobDocumentResponse.Where(d => jobCompanyTeam.Contains((d.Job.IdCompany != null ? d.Job.IdCompany.Value : 0))).ToList();

                string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
            }


            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            CultureInfo provider = CultureInfo.CurrentCulture;

            if (dataTableParameters != null && dataTableParameters.ExpiresFromDate != null)
            {
                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresFromDate.ToString(), "MM/dd/yyyy", provider);
                //DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresFromDate.ToString(), "dd/MM/yyyy", provider);
                fromDate = dataTableParameters.ExpiresFromDate.Value;
            }
            else
            {
                //string fmdate = DateTime.Now.AddDays(-30).ToShortDateString();
                fromDate = DateTime.Now.AddDays(-30);

                //fromDate = DateTime.ParseExact(fmdate, "dd/MM/yyyy", provider);
                //fromDate = DateTime.ParseExact(fmdate, "MM/dd/yyyy", provider);
            }


            if (dataTableParameters != null && dataTableParameters.ExpiresToDate != null)
            {
                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresToDate.ToString(), "MM/dd/yyyy", provider);

                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresToDate.ToString(), "dd/MM/yyyy", provider);

                toDate = dataTableParameters.ExpiresToDate.Value;
            }
            else
            {
                //string todatenw = DateTime.Now.AddDays(+30).ToShortDateString();
                toDate = DateTime.Now.AddDays(+30);
                //toDate = DateTime.ParseExact(todatenw, "MM/dd/yyyy", provider);
                // toDate = DateTime.ParseExact(todatenw, "dd/MM/yyyy", provider);
            }


            hearingDate = hearingDate + (!string.IsNullOrEmpty(fromDate.ToString()) && !string.IsNullOrEmpty(fromDate.ToString()) ? " | " : string.Empty) + (!string.IsNullOrEmpty(fromDate.ToString()) ? "FromDate : " + fromDate.ToString() : string.Empty);



            hearingDate = hearingDate + (!string.IsNullOrEmpty(toDate.ToString()) && !string.IsNullOrEmpty(toDate.ToString()) ? " | " : string.Empty) + (!string.IsNullOrEmpty(toDate.ToString()) ? "ToDate : " + toDate.ToString() : string.Empty);

            if (dataTableParameters != null && dataTableParameters.AHVReferenceNumber != null && AHVReferenceNumber != "")
            {
                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "AHVReferenceNumber" && b.Value == AHVReferenceNumber)).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }
            if (dataTableParameters != null && dataTableParameters.Type != null && !string.IsNullOrEmpty(dataTableParameters.Type))
            {
                hearingDate = hearingDate + (!string.IsNullOrEmpty(dataTableParameters.Type) && !string.IsNullOrEmpty(dataTableParameters.Type) ? " | " : string.Empty) + (!string.IsNullOrEmpty(dataTableParameters.Type) ? "Type : " + dataTableParameters.Type : string.Empty);

                List<string> jobType = dataTableParameters.Type != null && !string.IsNullOrEmpty(dataTableParameters.Type) ? (dataTableParameters.Type.Split('-') != null && dataTableParameters.Type.Split('-').Any() ? dataTableParameters.Type.ToLower().Trim().Split('-').Select(x => x).ToList() : new List<string>()) : new List<string>();

                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "Type" && jobType.Contains(b.ActualValue.ToLower().Trim() ?? ""))).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }
            if (dataTableParameters != null && dataTableParameters.IdApplicantCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicantCompany))
            {
                List<string> objApplicantCompany = dataTableParameters.IdApplicantCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicantCompany) ? (dataTableParameters.IdApplicantCompany.Split('-') != null && dataTableParameters.IdApplicantCompany.ToLower().Trim().Split('-').Any() ? dataTableParameters.IdApplicantCompany.ToLower().Trim().Split('-').Select(x => x).ToList() : new List<string>()) : new List<string>();

                string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => objApplicantCompany.Contains(x.Id.ToString() ?? "")).Select(x => x.Name));
                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "ApplicantCompany : " + filteredCompany : string.Empty);


                var strconteacts = (from d in rpoContext.JobContacts where objApplicantCompany.Contains(d.IdCompany.ToString() ?? "") select d.Id.ToString()).ToList();

                var aatmp = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "txtLast")).ToList();

                var bb = aatmp.Where(d => d.JobDocumentFields.Any(b => objApplicantCompany.Contains(b.Value ?? ""))).ToList();

                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "txtLast" && strconteacts.Contains(b.Value.ToString() ?? ""))).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }

            if (fromDate != null && toDate != null)
            {
                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "Start Date"

                && DateTime.ParseExact(b.Value, "MM/dd/yy", provider).Date >= fromDate.Date && DateTime.ParseExact(b.Value, "MM/dd/yy", provider).Date <= toDate.Date)).ToList();
                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }

            foreach (var item in jobDocumentResponse)
            {
                List<JobDocumentField> jobDocumentFields = item.JobDocumentFields.ToList();


                Job objjobs = (from d in this.rpoContext.Jobs.Include("Borough") where d.Id == item.IdJob select d).FirstOrDefault();

                JobDocumentField jobDocumentField = new JobDocumentField();

                List<JobDocumentField> objDocFields = new List<JobDocumentField>();

                AHVExpiryReport objAHVExpiryReport = new AHVExpiryReport();

                objAHVExpiryReport.IdJob = item.IdJob.ToString();
                objAHVExpiryReport.JobNumber = item.Job.JobNumber.ToString();
                objAHVExpiryReport.CreatedModifiedDate = item.LastModifiedDate != null ? item.LastModifiedDate : item.CreatedDate;

                var objAHvReferenceNumber = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "AHVReferenceNumber") select d.Value).FirstOrDefault();

                objAHVExpiryReport.AHVReferenceNumber = objAHvReferenceNumber != null ? objAHvReferenceNumber : "";

                var objType = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Type") select d.ActualValue).FirstOrDefault();

                objAHVExpiryReport.Type = objType != null ? objType : "";


                objAHVExpiryReport.JobAddress = objjobs.HouseNumber + " " + objjobs.StreetNumber + ", " + objjobs.Borough.Description;

                var objApplicationNo = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Application") select d).FirstOrDefault();

                objAHVExpiryReport.ApplicationNo = objApplicationNo != null && objApplicationNo.JobDocument != null && objApplicationNo.JobDocument.JobApplication != null && objApplicationNo.JobDocument.JobApplication.ApplicationNumber != null ? objApplicationNo.JobDocument.JobApplication.ApplicationNumber : "";

                var objWorkPermit = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Work Permits") select d.ActualValue).FirstOrDefault();
                objAHVExpiryReport.WorkPermitType = objWorkPermit != null ? objWorkPermit : "";

                var objApplicant = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "txtLast") select d.Value).FirstOrDefault();

                string ApplicantCompany = string.Empty;
                DateTime? ApplicantExpiryDate = null;
                int? ApplicantId = null;
                if (objApplicant != null)
                {
                    int applicantid = int.Parse(objApplicant);
                    var objapplicantCompany = (from d in rpoContext.JobContacts where d.Id == applicantid select d).FirstOrDefault();
                    if (objapplicantCompany != null && objapplicantCompany.Company != null)
                    {
                        ApplicantCompany = objapplicantCompany.Company.Name;
                        ApplicantId = objapplicantCompany.Company.Id;
                        ApplicantExpiryDate = objapplicantCompany.Company.TrackingExpiry;
                    }
                }

                objAHVExpiryReport.ApplicantCompany = ApplicantCompany;


                objAHVExpiryReport.TrackingExpiryDate = ApplicantExpiryDate != null ? ApplicantExpiryDate : null;
                objAHVExpiryReport.JobCompany = item.Job != null && item.Job.Company != null ? item.Job.Company.Name : string.Empty;

                string[] objstrarray = item.DocumentDescription.Split(new char[] { '|' });

                string Description = string.Empty;

                foreach (var items in objstrarray)
                {
                    if (items.Contains("Dates"))
                    {
                        Description = items.ToString();
                    }
                }

                var objIssueDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Issued Date") select d.Value).FirstOrDefault();

                if (objIssueDate != null && !string.IsNullOrEmpty(objIssueDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objIssueDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.IssuedDate = dtvalue;
                }

                var objSubmittedDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Submitted Date") select d.Value).FirstOrDefault();

                if (objSubmittedDate != null && !string.IsNullOrEmpty(objSubmittedDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objSubmittedDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.SubmittedDate = dtvalue;
                }
                var objNextDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Next Date") select d.Value).FirstOrDefault();
                if (objNextDate != null && !string.IsNullOrEmpty(objNextDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objNextDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.NextDate = dtvalue;
                }
                objAHVExpiryReport.IdJobCompany = item.Job != null ? item.Job.IdCompany : null;
                objAHVExpiryReport.IdApplicantCompany = ApplicantId;
                objAHVExpiryReport.DocumentDescription = Description != "" ? Description : "";

                objAHVExpiryReportList.Add(objAHVExpiryReport);

                endloop:
                Console.WriteLine("");


            }
            string direction = string.Empty;
            string orderBy = string.Empty;

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
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobNumber).ToList();
                if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobAddress).ToList();
                if (orderBy.Trim().ToLower() == "JobCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobCompany).ToList();
                if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobApplicationType).ToList();
                if (orderBy.Trim().ToLower() == "TrackingExpiryDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.TrackingExpiryDate).ToList();
                if (orderBy.Trim().ToLower() == "ApplicationNo".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.ApplicationNo).ToList();
                if (orderBy.Trim().ToLower() == "ApplicantCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.ApplicantCompany).ToList();
                if (orderBy.Trim().ToLower() == "WorkPermitType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.WorkPermitType).ToList();
                if (orderBy.Trim().ToLower() == "AHVReferenceNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.AHVReferenceNumber).ToList();
                if (orderBy.Trim().ToLower() == "Type".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.Type).ToList();
                if (orderBy.Trim().ToLower() == "DocumentDescription".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.DocumentDescription).ToList();
                if (orderBy.Trim().ToLower() == "SubmittedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.SubmittedDate).ToList();
                if (orderBy.Trim().ToLower() == "IssuedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.IssuedDate).ToList();
                if (orderBy.Trim().ToLower() == "NextDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.NextDate).ToList();
                if (orderBy.Trim().ToLower() == "CreatedModifiedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.CreatedModifiedDate).ToList();
            }
            else
            {
                if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobNumber).ToList();
                if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobAddress).ToList();
                if (orderBy.Trim().ToLower() == "JobCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobCompany).ToList();
                if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobApplicationType).ToList();
                if (orderBy.Trim().ToLower() == "TrackingExpiryDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.TrackingExpiryDate).ToList();
                if (orderBy.Trim().ToLower() == "ApplicationNo".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.ApplicationNo).ToList();
                if (orderBy.Trim().ToLower() == "ApplicantCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.ApplicantCompany).ToList();
                if (orderBy.Trim().ToLower() == "WorkPermitType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.WorkPermitType).ToList();
                if (orderBy.Trim().ToLower() == "AHVReferenceNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.AHVReferenceNumber).ToList();
                if (orderBy.Trim().ToLower() == "Type".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.Type).ToList();
                if (orderBy.Trim().ToLower() == "DocumentDescription".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.DocumentDescription).ToList();
                if (orderBy.Trim().ToLower() == "SubmittedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.SubmittedDate).ToList();
                if (orderBy.Trim().ToLower() == "IssuedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.IssuedDate).ToList();
                if (orderBy.Trim().ToLower() == "NextDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.NextDate).ToList();
                if (orderBy.Trim().ToLower() == "CreatedModifiedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.CreatedModifiedDate).ToList();
            }

            #endregion

            if (objAHVExpiryReportList != null && objAHVExpiryReportList.Count > 0)
            {
                string exportFilename = string.Empty;
                exportFilename = "AHVExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                string exportFilePath = ExportToExcel(hearingDate, objAHVExpiryReportList, exportFilename);

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
                throw new RpoBusinessException(StaticMessages.NoRecordFoundMessage);
            }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private string ExportToExcel(string hearingDate, List<AHVExpiryReport> result, string exportFilename)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
            templateFileName = "AHV_PermitExpirationTemplate - Copy.xlsx";
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

            string reportDate = "Date:" + DateTime.Today.ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat);
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
            // iReportDateRow.GetCell(6).CellStyle = rightAlignCellStyle;
            int index = 5;
            foreach (AHVExpiryReport item in result)
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
                        iRow.GetCell(1).SetCellValue(item.JobCompany);
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(item.JobCompany);
                    }

                    if (iRow.GetCell(2) != null)
                    {
                        iRow.GetCell(2).SetCellValue(item.TrackingExpiryDate != null ? Convert.ToDateTime(item.TrackingExpiryDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(item.TrackingExpiryDate != null ? Convert.ToDateTime(item.TrackingExpiryDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(item.WorkPermitType);
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(item.WorkPermitType);
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.AHVReferenceNumber);
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.AHVReferenceNumber);
                    }


                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(item.Type);
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(item.Type);
                    }
                    if (iRow.GetCell(6) != null)
                    {
                        iRow.GetCell(6).SetCellValue(!string.IsNullOrEmpty(item.ApplicantCompany) ? item.ApplicantCompany : item.ApplicantCompany);
                    }
                    else
                    {
                        iRow.CreateCell(6).SetCellValue(!string.IsNullOrEmpty(item.ApplicantCompany) ? item.ApplicantCompany : item.ApplicantCompany);
                    }


                    if (iRow.GetCell(7) != null)
                    {
                        iRow.GetCell(7).SetCellValue(!string.IsNullOrEmpty(item.DocumentDescription) ? item.DocumentDescription : item.DocumentDescription);
                    }
                    else
                    {
                        iRow.CreateCell(7).SetCellValue(!string.IsNullOrEmpty(item.DocumentDescription) ? item.DocumentDescription : item.DocumentDescription);
                    }

                    if (iRow.GetCell(8) != null)
                    {
                        iRow.GetCell(8).SetCellValue(item.SubmittedDate != null ? Convert.ToDateTime(item.SubmittedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(8).SetCellValue(item.SubmittedDate != null ? Convert.ToDateTime(item.SubmittedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(9) != null)
                    {
                        iRow.GetCell(9).SetCellValue(item.IssuedDate != null ? Convert.ToDateTime(item.IssuedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(9).SetCellValue(item.IssuedDate != null ? Convert.ToDateTime(item.IssuedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    if (iRow.GetCell(10) != null)
                    {
                        iRow.GetCell(10).SetCellValue(item.NextDate != null ? Convert.ToDateTime(item.NextDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(10).SetCellValue(item.NextDate != null ? Convert.ToDateTime(item.NextDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    if (iRow.GetCell(11) != null)
                    {
                        iRow.GetCell(11).SetCellValue(item.CreatedModifiedDate != null ? Convert.ToDateTime(item.CreatedModifiedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(11).SetCellValue(item.CreatedModifiedDate != null ? Convert.ToDateTime(item.CreatedModifiedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
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
                        iRow.GetCell(1).SetCellValue(item.JobCompany);
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(item.JobCompany);
                    }

                    if (iRow.GetCell(2) != null)
                    {
                        iRow.GetCell(2).SetCellValue(item.TrackingExpiryDate != null ? Convert.ToDateTime(item.TrackingExpiryDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(item.TrackingExpiryDate != null ? Convert.ToDateTime(item.TrackingExpiryDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(item.WorkPermitType);
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(item.WorkPermitType);
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.AHVReferenceNumber);
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.AHVReferenceNumber);
                    }


                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(item.Type);
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(item.Type);
                    }
                    if (iRow.GetCell(6) != null)
                    {
                        iRow.GetCell(6).SetCellValue(!string.IsNullOrEmpty(item.ApplicantCompany) ? item.ApplicantCompany : item.ApplicantCompany);
                    }
                    else
                    {
                        iRow.CreateCell(6).SetCellValue(!string.IsNullOrEmpty(item.ApplicantCompany) ? item.ApplicantCompany : item.ApplicantCompany);
                    }


                    if (iRow.GetCell(7) != null)
                    {
                        iRow.GetCell(7).SetCellValue(!string.IsNullOrEmpty(item.DocumentDescription) ? item.DocumentDescription : item.DocumentDescription);
                    }
                    else
                    {
                        iRow.CreateCell(7).SetCellValue(!string.IsNullOrEmpty(item.DocumentDescription) ? item.DocumentDescription : item.DocumentDescription);
                    }

                    if (iRow.GetCell(8) != null)
                    {
                        iRow.GetCell(8).SetCellValue(item.SubmittedDate != null ? Convert.ToDateTime(item.SubmittedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(8).SetCellValue(item.SubmittedDate != null ? Convert.ToDateTime(item.SubmittedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(9) != null)
                    {
                        iRow.GetCell(9).SetCellValue(item.IssuedDate != null ? Convert.ToDateTime(item.IssuedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(9).SetCellValue(item.IssuedDate != null ? Convert.ToDateTime(item.IssuedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    if (iRow.GetCell(10) != null)
                    {
                        iRow.GetCell(10).SetCellValue(item.NextDate != null ? Convert.ToDateTime(item.NextDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(10).SetCellValue(item.NextDate != null ? Convert.ToDateTime(item.NextDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    if (iRow.GetCell(11) != null)
                    {
                        iRow.GetCell(11).SetCellValue(item.CreatedModifiedDate != null ? Convert.ToDateTime(item.CreatedModifiedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(11).SetCellValue(item.CreatedModifiedDate != null ? Convert.ToDateTime(item.CreatedModifiedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty);
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
        /// Gets the Report for AHVExpiry Report apply the filser and sorting. and export to pdf 
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportAHVExpiry/exporttopdf")]
        public IHttpActionResult PostExportReportApplicationStatusToPdf(AHVExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Rpo.ApiServices.Api.Tools.Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                #region Filter

                int idPW517Document = Enums.Document.AfterHoursPermitApplicationPW517.GetHashCode();

            var Jobslist = (from d in rpoContext.Jobs select d).ToList();

            List<AHVExpiryReport> objAHVExpiryReportList = new List<AHVExpiryReport>();
            string hearingDate = string.Empty;

            string AHVReferenceNumber = string.Empty;
            if (dataTableParameters != null && dataTableParameters.AHVReferenceNumber != null && !string.IsNullOrEmpty(dataTableParameters.AHVReferenceNumber))
            {
                AHVReferenceNumber = dataTableParameters.AHVReferenceNumber;

                hearingDate = hearingDate + (!string.IsNullOrEmpty(AHVReferenceNumber) && !string.IsNullOrEmpty(AHVReferenceNumber) ? " | " : string.Empty) + (!string.IsNullOrEmpty(AHVReferenceNumber) ? "AHVReferenceNumber : " + AHVReferenceNumber : string.Empty);
            }
            List<Rpo.ApiServices.Model.Models.JobDocument> jobDocumentResponse = jobDocumentResponse = rpoContext.JobDocuments.Where(d => d.IdDocument == idPW517Document).Include("JobDocumentFields.DocumentField.Field").ToList();


            if (dataTableParameters != null && dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
            {
                List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                jobDocumentResponse = jobDocumentResponse.Where(d => joNumbers.Contains(d.IdJob)).ToList();

                string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
            }


            if (dataTableParameters != null && dataTableParameters.IdJobCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdJobCompany))
            {
                List<int> jobCompanyTeam = dataTableParameters.IdJobCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdJobCompany) ? (dataTableParameters.IdJobCompany.Split('-') != null && dataTableParameters.IdJobCompany.Split('-').Any() ? dataTableParameters.IdJobCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                jobDocumentResponse = jobDocumentResponse.Where(d => jobCompanyTeam.Contains((d.Job.IdCompany != null ? d.Job.IdCompany.Value : 0))).ToList();

                string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
            }


            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            CultureInfo provider = CultureInfo.CurrentCulture;

            if (dataTableParameters != null && dataTableParameters.ExpiresFromDate != null)
            {
                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresFromDate.ToString(), "MM/dd/yyyy", provider);
                //DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresFromDate.ToString(), "dd/MM/yyyy", provider);
                fromDate = dataTableParameters.ExpiresFromDate.Value;
            }
            else
            {
                //string fmdate = DateTime.Now.AddDays(-30).ToShortDateString();
                fromDate = DateTime.Now.AddDays(-30);

                //fromDate = DateTime.ParseExact(fmdate, "dd/MM/yyyy", provider);
                //fromDate = DateTime.ParseExact(fmdate, "MM/dd/yyyy", provider);
            }


            if (dataTableParameters != null && dataTableParameters.ExpiresToDate != null)
            {
                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresToDate.ToString(), "MM/dd/yyyy", provider);

                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresToDate.ToString(), "dd/MM/yyyy", provider);

                toDate = dataTableParameters.ExpiresToDate.Value;
            }
            else
            {
                //string todatenw = DateTime.Now.AddDays(+30).ToShortDateString();
                toDate = DateTime.Now.AddDays(+30);
                //toDate = DateTime.ParseExact(todatenw, "MM/dd/yyyy", provider);
                // toDate = DateTime.ParseExact(todatenw, "dd/MM/yyyy", provider);
            }


            hearingDate = hearingDate + (!string.IsNullOrEmpty(fromDate.ToString()) && !string.IsNullOrEmpty(fromDate.ToString()) ? " | " : string.Empty) + (!string.IsNullOrEmpty(fromDate.ToString()) ? "FromDate : " + fromDate.ToString() : string.Empty);



            hearingDate = hearingDate + (!string.IsNullOrEmpty(toDate.ToString()) && !string.IsNullOrEmpty(toDate.ToString()) ? " | " : string.Empty) + (!string.IsNullOrEmpty(toDate.ToString()) ? "ToDate : " + toDate.ToString() : string.Empty);

            if (dataTableParameters != null && dataTableParameters.AHVReferenceNumber != null && AHVReferenceNumber != "")
            {
                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "AHVReferenceNumber" && b.Value == AHVReferenceNumber)).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }
            if (dataTableParameters != null && dataTableParameters.Type != null && !string.IsNullOrEmpty(dataTableParameters.Type))
            {
                hearingDate = hearingDate + (!string.IsNullOrEmpty(dataTableParameters.Type) && !string.IsNullOrEmpty(dataTableParameters.Type) ? " | " : string.Empty) + (!string.IsNullOrEmpty(dataTableParameters.Type) ? "Type : " + dataTableParameters.Type : string.Empty);

                List<string> jobType = dataTableParameters.Type != null && !string.IsNullOrEmpty(dataTableParameters.Type) ? (dataTableParameters.Type.Split('-') != null && dataTableParameters.Type.Split('-').Any() ? dataTableParameters.Type.ToLower().Trim().Split('-').Select(x => x).ToList() : new List<string>()) : new List<string>();

                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "Type" && jobType.Contains(b.ActualValue.ToLower().Trim() ?? ""))).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }
            if (dataTableParameters != null && dataTableParameters.IdApplicantCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicantCompany))
            {
                List<string> objApplicantCompany = dataTableParameters.IdApplicantCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicantCompany) ? (dataTableParameters.IdApplicantCompany.Split('-') != null && dataTableParameters.IdApplicantCompany.ToLower().Trim().Split('-').Any() ? dataTableParameters.IdApplicantCompany.ToLower().Trim().Split('-').Select(x => x).ToList() : new List<string>()) : new List<string>();

                string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => objApplicantCompany.Contains(x.Id.ToString() ?? "")).Select(x => x.Name));
                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "ApplicantCompany : " + filteredCompany : string.Empty);


                var strconteacts = (from d in rpoContext.JobContacts where objApplicantCompany.Contains(d.IdCompany.ToString() ?? "") select d.Id.ToString()).ToList();

                var aatmp = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "txtLast")).ToList();

                var bb = aatmp.Where(d => d.JobDocumentFields.Any(b => objApplicantCompany.Contains(b.Value ?? ""))).ToList();

                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "txtLast" && strconteacts.Contains(b.Value.ToString() ?? ""))).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }

            if (fromDate != null && toDate != null)
            {
                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "Start Date"

                && DateTime.ParseExact(b.Value, "MM/dd/yy", provider).Date >= fromDate.Date && DateTime.ParseExact(b.Value, "MM/dd/yy", provider).Date <= toDate.Date)).ToList();
                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }

            foreach (var item in jobDocumentResponse)
            {
                List<JobDocumentField> jobDocumentFields = item.JobDocumentFields.ToList();


                Job objjobs = (from d in this.rpoContext.Jobs.Include("Borough") where d.Id == item.IdJob select d).FirstOrDefault();

                JobDocumentField jobDocumentField = new JobDocumentField();

                List<JobDocumentField> objDocFields = new List<JobDocumentField>();

                AHVExpiryReport objAHVExpiryReport = new AHVExpiryReport();

                objAHVExpiryReport.IdJob = item.IdJob.ToString();
                objAHVExpiryReport.JobNumber = item.Job.JobNumber.ToString();
                objAHVExpiryReport.CreatedModifiedDate = item.LastModifiedDate != null ? item.LastModifiedDate : item.CreatedDate;

                var objAHvReferenceNumber = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "AHVReferenceNumber") select d.Value).FirstOrDefault();

                objAHVExpiryReport.AHVReferenceNumber = objAHvReferenceNumber != null ? objAHvReferenceNumber : "";

                var objType = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Type") select d.ActualValue).FirstOrDefault();

                objAHVExpiryReport.Type = objType != null ? objType : "";


                objAHVExpiryReport.JobAddress = objjobs.HouseNumber + " " + objjobs.StreetNumber + ", " + objjobs.Borough.Description;

                var objApplicationNo = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Application") select d).FirstOrDefault();

                objAHVExpiryReport.ApplicationNo = objApplicationNo != null && objApplicationNo.JobDocument != null && objApplicationNo.JobDocument.JobApplication != null && objApplicationNo.JobDocument.JobApplication.ApplicationNumber != null ? objApplicationNo.JobDocument.JobApplication.ApplicationNumber : "";

                var objWorkPermit = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Work Permits") select d.ActualValue).FirstOrDefault();
                objAHVExpiryReport.WorkPermitType = objWorkPermit != null ? objWorkPermit : "";

                var objApplicant = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "txtLast") select d.Value).FirstOrDefault();

                string ApplicantCompany = string.Empty;
                DateTime? ApplicantExpiryDate = null;
                int? ApplicantId = null;
                if (objApplicant != null)
                {
                    int applicantid = int.Parse(objApplicant);
                    var objapplicantCompany = (from d in rpoContext.JobContacts where d.Id == applicantid select d).FirstOrDefault();
                    if (objapplicantCompany != null && objapplicantCompany.Company != null)
                    {
                        ApplicantCompany = objapplicantCompany.Company.Name;
                        ApplicantId = objapplicantCompany.Company.Id;
                        ApplicantExpiryDate = objapplicantCompany.Company.TrackingExpiry;
                    }
                }

                objAHVExpiryReport.ApplicantCompany = ApplicantCompany;


                objAHVExpiryReport.TrackingExpiryDate = ApplicantExpiryDate != null ? ApplicantExpiryDate : null;
                objAHVExpiryReport.JobCompany = item.Job != null && item.Job.Company != null ? item.Job.Company.Name : string.Empty;

                string[] objstrarray = item.DocumentDescription.Split(new char[] { '|' });

                string Description = string.Empty;

                foreach (var items in objstrarray)
                {
                    if (items.Contains("Dates"))
                    {
                        Description = items.ToString();
                    }
                }

                var objIssueDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Issued Date") select d.Value).FirstOrDefault();

                if (objIssueDate != null && !string.IsNullOrEmpty(objIssueDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objIssueDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.IssuedDate = dtvalue;
                }

                var objSubmittedDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Submitted Date") select d.Value).FirstOrDefault();

                if (objSubmittedDate != null && !string.IsNullOrEmpty(objSubmittedDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objSubmittedDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.SubmittedDate = dtvalue;
                }
                var objNextDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Next Date") select d.Value).FirstOrDefault();
                if (objNextDate != null && !string.IsNullOrEmpty(objNextDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objNextDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.NextDate = dtvalue;
                }
                objAHVExpiryReport.IdJobCompany = item.Job != null ? item.Job.IdCompany : null;
                objAHVExpiryReport.IdApplicantCompany = ApplicantId;
                objAHVExpiryReport.DocumentDescription = Description != "" ? Description : "";

                objAHVExpiryReportList.Add(objAHVExpiryReport);

                endloop:
                Console.WriteLine("");


            }

            string direction = string.Empty;
            string orderBy = string.Empty;

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
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobNumber).ToList();
                if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobAddress).ToList();
                if (orderBy.Trim().ToLower() == "JobCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobCompany).ToList();
                if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobApplicationType).ToList();
                if (orderBy.Trim().ToLower() == "TrackingExpiryDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.TrackingExpiryDate).ToList();
                if (orderBy.Trim().ToLower() == "ApplicationNo".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.ApplicationNo).ToList();
                if (orderBy.Trim().ToLower() == "ApplicantCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.ApplicantCompany).ToList();
                if (orderBy.Trim().ToLower() == "WorkPermitType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.WorkPermitType).ToList();
                if (orderBy.Trim().ToLower() == "AHVReferenceNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.AHVReferenceNumber).ToList();
                if (orderBy.Trim().ToLower() == "Type".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.Type).ToList();
                if (orderBy.Trim().ToLower() == "DocumentDescription".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.DocumentDescription).ToList();
                if (orderBy.Trim().ToLower() == "SubmittedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.SubmittedDate).ToList();
                if (orderBy.Trim().ToLower() == "IssuedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.IssuedDate).ToList();
                if (orderBy.Trim().ToLower() == "NextDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.NextDate).ToList();
                if (orderBy.Trim().ToLower() == "CreatedModifiedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.CreatedModifiedDate).ToList();
            }
            else
            {
                if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobNumber).ToList();
                if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobAddress).ToList();
                if (orderBy.Trim().ToLower() == "JobCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobCompany).ToList();
                if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobApplicationType).ToList();
                if (orderBy.Trim().ToLower() == "TrackingExpiryDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.TrackingExpiryDate).ToList();
                if (orderBy.Trim().ToLower() == "ApplicationNo".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.ApplicationNo).ToList();
                if (orderBy.Trim().ToLower() == "ApplicantCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.ApplicantCompany).ToList();
                if (orderBy.Trim().ToLower() == "WorkPermitType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.WorkPermitType).ToList();
                if (orderBy.Trim().ToLower() == "AHVReferenceNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.AHVReferenceNumber).ToList();
                if (orderBy.Trim().ToLower() == "Type".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.Type).ToList();
                if (orderBy.Trim().ToLower() == "DocumentDescription".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.DocumentDescription).ToList();
                if (orderBy.Trim().ToLower() == "SubmittedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.SubmittedDate).ToList();
                if (orderBy.Trim().ToLower() == "IssuedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.IssuedDate).ToList();
                if (orderBy.Trim().ToLower() == "NextDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.NextDate).ToList();
                if (orderBy.Trim().ToLower() == "CreatedModifiedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.CreatedModifiedDate).ToList();
            }
            #endregion



            if (objAHVExpiryReportList != null && objAHVExpiryReportList.Count > 0)
            {
                string exportFilename = "AHVExpirationReport" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                string exportFilePath = ExportToPdf(hearingDate, objAHVExpiryReportList, exportFilename);
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
                throw new RpoBusinessException(StaticMessages.NoRecordFoundMessage);
            }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportAHVExpiry/exporttoexcelemail")]
        public IHttpActionResult PostExportAHVExpiryToExcelEmail(AHVExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Rpo.ApiServices.Api.Tools.Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                string currentTimeZone = Rpo.ApiServices.Api.Tools.Common.FetchHeaderValues(this.Request, Rpo.ApiServices.Api.Tools.Common.CurrentTimezoneHeaderKey);

            #region Filter

            int idPW517Document = Enums.Document.AfterHoursPermitApplicationPW517.GetHashCode();

            var Jobslist = (from d in rpoContext.Jobs select d).ToList();

            List<AHVExpiryReport> objAHVExpiryReportList = new List<AHVExpiryReport>();
            string hearingDate = string.Empty;

            string AHVReferenceNumber = string.Empty;
            if (dataTableParameters != null && dataTableParameters.AHVReferenceNumber != null && !string.IsNullOrEmpty(dataTableParameters.AHVReferenceNumber))
            {
                AHVReferenceNumber = dataTableParameters.AHVReferenceNumber;

                hearingDate = hearingDate + (!string.IsNullOrEmpty(AHVReferenceNumber) && !string.IsNullOrEmpty(AHVReferenceNumber) ? " | " : string.Empty) + (!string.IsNullOrEmpty(AHVReferenceNumber) ? "AHVReferenceNumber : " + AHVReferenceNumber : string.Empty);
            }
            List<Rpo.ApiServices.Model.Models.JobDocument> jobDocumentResponse = jobDocumentResponse = rpoContext.JobDocuments.Where(d => d.IdDocument == idPW517Document).Include("JobDocumentFields.DocumentField.Field").ToList();


            if (dataTableParameters != null && dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
            {
                List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                jobDocumentResponse = jobDocumentResponse.Where(d => joNumbers.Contains(d.IdJob)).ToList();

                string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));

                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
            }


            if (dataTableParameters != null && dataTableParameters.IdJobCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdJobCompany))
            {
                List<int> jobCompanyTeam = dataTableParameters.IdJobCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdJobCompany) ? (dataTableParameters.IdJobCompany.Split('-') != null && dataTableParameters.IdJobCompany.Split('-').Any() ? dataTableParameters.IdJobCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                jobDocumentResponse = jobDocumentResponse.Where(d => jobCompanyTeam.Contains((d.Job.IdCompany != null ? d.Job.IdCompany.Value : 0))).ToList();

                string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
            }


            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            CultureInfo provider = CultureInfo.CurrentCulture;

            if (dataTableParameters != null && dataTableParameters.ExpiresFromDate != null)
            {
                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresFromDate.ToString(), "MM/dd/yyyy", provider);
                //DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresFromDate.ToString(), "dd/MM/yyyy", provider);
                fromDate = dataTableParameters.ExpiresFromDate.Value;
            }
            else
            {
                //string fmdate = DateTime.Now.AddDays(-30).ToShortDateString();
                fromDate = DateTime.Now.AddDays(-30);

                //fromDate = DateTime.ParseExact(fmdate, "dd/MM/yyyy", provider);
                //fromDate = DateTime.ParseExact(fmdate, "MM/dd/yyyy", provider);
            }


            if (dataTableParameters != null && dataTableParameters.ExpiresToDate != null)
            {
                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresToDate.ToString(), "MM/dd/yyyy", provider);

                // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresToDate.ToString(), "dd/MM/yyyy", provider);

                toDate = dataTableParameters.ExpiresToDate.Value;
            }
            else
            {
                //string todatenw = DateTime.Now.AddDays(+30).ToShortDateString();
                toDate = DateTime.Now.AddDays(+30);
                //toDate = DateTime.ParseExact(todatenw, "MM/dd/yyyy", provider);
                // toDate = DateTime.ParseExact(todatenw, "dd/MM/yyyy", provider);
            }


            hearingDate = hearingDate + (!string.IsNullOrEmpty(fromDate.ToString()) && !string.IsNullOrEmpty(fromDate.ToString()) ? " | " : string.Empty) + (!string.IsNullOrEmpty(fromDate.ToString()) ? "FromDate : " + fromDate.ToString() : string.Empty);



            hearingDate = hearingDate + (!string.IsNullOrEmpty(toDate.ToString()) && !string.IsNullOrEmpty(toDate.ToString()) ? " | " : string.Empty) + (!string.IsNullOrEmpty(toDate.ToString()) ? "ToDate : " + toDate.ToString() : string.Empty);

            if (dataTableParameters != null && dataTableParameters.AHVReferenceNumber != null && AHVReferenceNumber != "")
            {
                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "AHVReferenceNumber" && b.Value == AHVReferenceNumber)).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }
            if (dataTableParameters != null && dataTableParameters.Type != null && !string.IsNullOrEmpty(dataTableParameters.Type))
            {
                hearingDate = hearingDate + (!string.IsNullOrEmpty(dataTableParameters.Type) && !string.IsNullOrEmpty(dataTableParameters.Type) ? " | " : string.Empty) + (!string.IsNullOrEmpty(dataTableParameters.Type) ? "Type : " + dataTableParameters.Type : string.Empty);

                List<string> jobType = dataTableParameters.Type != null && !string.IsNullOrEmpty(dataTableParameters.Type) ? (dataTableParameters.Type.Split('-') != null && dataTableParameters.Type.Split('-').Any() ? dataTableParameters.Type.ToLower().Trim().Split('-').Select(x => x).ToList() : new List<string>()) : new List<string>();

                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "Type" && jobType.Contains(b.ActualValue.ToLower().Trim() ?? ""))).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }
            if (dataTableParameters != null && dataTableParameters.IdApplicantCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicantCompany))
            {
                List<string> objApplicantCompany = dataTableParameters.IdApplicantCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicantCompany) ? (dataTableParameters.IdApplicantCompany.Split('-') != null && dataTableParameters.IdApplicantCompany.ToLower().Trim().Split('-').Any() ? dataTableParameters.IdApplicantCompany.ToLower().Trim().Split('-').Select(x => x).ToList() : new List<string>()) : new List<string>();

                string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => objApplicantCompany.Contains(x.Id.ToString() ?? "")).Select(x => x.Name));
                hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "ApplicantCompany : " + filteredCompany : string.Empty);


                var strconteacts = (from d in rpoContext.JobContacts where objApplicantCompany.Contains(d.IdCompany.ToString() ?? "") select d.Id.ToString()).ToList();

                var aatmp = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "txtLast")).ToList();

                var bb = aatmp.Where(d => d.JobDocumentFields.Any(b => objApplicantCompany.Contains(b.Value ?? ""))).ToList();

                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "txtLast" && strconteacts.Contains(b.Value.ToString() ?? ""))).ToList();

                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }

            if (fromDate != null && toDate != null)
            {
                var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "Start Date"

                && DateTime.ParseExact(b.Value, "MM/dd/yy", provider).Date >= fromDate.Date && DateTime.ParseExact(b.Value, "MM/dd/yy", provider).Date <= toDate.Date)).ToList();
                if (aa != null)
                {
                    jobDocumentResponse = aa;
                }
            }

            foreach (var item in jobDocumentResponse)
            {
                List<JobDocumentField> jobDocumentFields = item.JobDocumentFields.ToList();


                Job objjobs = (from d in this.rpoContext.Jobs.Include("Borough") where d.Id == item.IdJob select d).FirstOrDefault();

                JobDocumentField jobDocumentField = new JobDocumentField();

                List<JobDocumentField> objDocFields = new List<JobDocumentField>();

                AHVExpiryReport objAHVExpiryReport = new AHVExpiryReport();

                objAHVExpiryReport.IdJob = item.IdJob.ToString();
                objAHVExpiryReport.JobNumber = item.Job.JobNumber.ToString();
                objAHVExpiryReport.CreatedModifiedDate = item.LastModifiedDate != null ? item.LastModifiedDate : item.CreatedDate;

                var objAHvReferenceNumber = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "AHVReferenceNumber") select d.Value).FirstOrDefault();

                objAHVExpiryReport.AHVReferenceNumber = objAHvReferenceNumber != null ? objAHvReferenceNumber : "";

                var objType = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Type") select d.ActualValue).FirstOrDefault();

                objAHVExpiryReport.Type = objType != null ? objType : "";


                objAHVExpiryReport.JobAddress = objjobs.HouseNumber + " " + objjobs.StreetNumber + ", " + objjobs.Borough.Description;

                var objApplicationNo = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Application") select d).FirstOrDefault();

                objAHVExpiryReport.ApplicationNo = objApplicationNo != null && objApplicationNo.JobDocument != null && objApplicationNo.JobDocument.JobApplication != null && objApplicationNo.JobDocument.JobApplication.ApplicationNumber != null ? objApplicationNo.JobDocument.JobApplication.ApplicationNumber : "";

                var objWorkPermit = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Work Permits") select d.ActualValue).FirstOrDefault();
                objAHVExpiryReport.WorkPermitType = objWorkPermit != null ? objWorkPermit : "";

                var objApplicant = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "txtLast") select d.Value).FirstOrDefault();

                string ApplicantCompany = string.Empty;
                DateTime? ApplicantExpiryDate = null;
                int? ApplicantId = null;
                if (objApplicant != null)
                {
                    int applicantid = int.Parse(objApplicant);
                    var objapplicantCompany = (from d in rpoContext.JobContacts where d.Id == applicantid select d).FirstOrDefault();
                    if (objapplicantCompany != null && objapplicantCompany.Company != null)
                    {
                        ApplicantCompany = objapplicantCompany.Company.Name;
                        ApplicantId = objapplicantCompany.Company.Id;
                        ApplicantExpiryDate = objapplicantCompany.Company.TrackingExpiry;
                    }
                }

                objAHVExpiryReport.ApplicantCompany = ApplicantCompany;


                objAHVExpiryReport.TrackingExpiryDate = ApplicantExpiryDate != null ? ApplicantExpiryDate : null;
                objAHVExpiryReport.JobCompany = item.Job != null && item.Job.Company != null ? item.Job.Company.Name : string.Empty;

                string[] objstrarray = item.DocumentDescription.Split(new char[] { '|' });

                string Description = string.Empty;

                foreach (var items in objstrarray)
                {
                    if (items.Contains("Dates"))
                    {
                        Description = items.ToString();
                    }
                }

                var objIssueDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Issued Date") select d.Value).FirstOrDefault();

                if (objIssueDate != null && !string.IsNullOrEmpty(objIssueDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objIssueDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.IssuedDate = dtvalue;
                }

                var objSubmittedDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Submitted Date") select d.Value).FirstOrDefault();

                if (objSubmittedDate != null && !string.IsNullOrEmpty(objSubmittedDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objSubmittedDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.SubmittedDate = dtvalue;
                }
                var objNextDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Next Date") select d.Value).FirstOrDefault();
                if (objNextDate != null && !string.IsNullOrEmpty(objNextDate))
                {
                    DateTime dtvalue = DateTime.ParseExact(objNextDate, "MM/dd/yy", provider);

                    objAHVExpiryReport.NextDate = dtvalue;
                }
                objAHVExpiryReport.IdJobCompany = item.Job != null ? item.Job.IdCompany : null;
                objAHVExpiryReport.IdApplicantCompany = ApplicantId;
                objAHVExpiryReport.DocumentDescription = Description != "" ? Description : "";

                objAHVExpiryReportList.Add(objAHVExpiryReport);

                endloop:
                Console.WriteLine("");


            }


            string direction = string.Empty;
            string orderBy = string.Empty;

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
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobNumber).ToList();
                if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobAddress).ToList();
                if (orderBy.Trim().ToLower() == "JobCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobCompany).ToList();
                if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobApplicationType).ToList();
                if (orderBy.Trim().ToLower() == "TrackingExpiryDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.TrackingExpiryDate).ToList();
                if (orderBy.Trim().ToLower() == "ApplicationNo".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.ApplicationNo).ToList();
                if (orderBy.Trim().ToLower() == "ApplicantCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.ApplicantCompany).ToList();
                if (orderBy.Trim().ToLower() == "WorkPermitType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.WorkPermitType).ToList();
                if (orderBy.Trim().ToLower() == "AHVReferenceNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.AHVReferenceNumber).ToList();
                if (orderBy.Trim().ToLower() == "Type".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.Type).ToList();
                if (orderBy.Trim().ToLower() == "DocumentDescription".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.DocumentDescription).ToList();
                if (orderBy.Trim().ToLower() == "SubmittedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.SubmittedDate).ToList();
                if (orderBy.Trim().ToLower() == "IssuedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.IssuedDate).ToList();
                if (orderBy.Trim().ToLower() == "NextDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.NextDate).ToList();
                if (orderBy.Trim().ToLower() == "CreatedModifiedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.CreatedModifiedDate).ToList();
            }
            else
            {
                if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobNumber).ToList();
                if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobAddress).ToList();
                if (orderBy.Trim().ToLower() == "JobCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobCompany).ToList();
                if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobApplicationType).ToList();
                if (orderBy.Trim().ToLower() == "TrackingExpiryDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.TrackingExpiryDate).ToList();
                if (orderBy.Trim().ToLower() == "ApplicationNo".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.ApplicationNo).ToList();
                if (orderBy.Trim().ToLower() == "ApplicantCompany".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.ApplicantCompany).ToList();
                if (orderBy.Trim().ToLower() == "WorkPermitType".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.WorkPermitType).ToList();
                if (orderBy.Trim().ToLower() == "AHVReferenceNumber".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.AHVReferenceNumber).ToList();
                if (orderBy.Trim().ToLower() == "Type".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.Type).ToList();
                if (orderBy.Trim().ToLower() == "DocumentDescription".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.DocumentDescription).ToList();
                if (orderBy.Trim().ToLower() == "SubmittedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.SubmittedDate).ToList();
                if (orderBy.Trim().ToLower() == "IssuedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.IssuedDate).ToList();
                if (orderBy.Trim().ToLower() == "NextDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.NextDate).ToList();
                if (orderBy.Trim().ToLower() == "CreatedModifiedDate".Trim().ToLower())
                    objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.CreatedModifiedDate).ToList();
            }

            #endregion

            if (objAHVExpiryReportList != null && objAHVExpiryReportList.Count > 0)
            {
                string exportFilename = string.Empty;
                exportFilename = "AHVExpirationReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                string exportFilePath = ExportToExcel(hearingDate, objAHVExpiryReportList, exportFilename);
                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
                string path = APIUrl + "/UploadedDocuments/ReportExportToExcel/" + exportFilename;

                return Ok(new { Filepath = exportFilePath, NewPath = path, ReportName = exportFilename });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoRecordFoundMessage);
            }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportAHVExpiry/exporttopdfemail")]
        public IHttpActionResult PostExportReportApplicationStatusToPdfEmail(AHVExpiryDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (ApiServices.Api.Tools.Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                #region Filter

                int idPW517Document = Enums.Document.AfterHoursPermitApplicationPW517.GetHashCode();

                var Jobslist = (from d in rpoContext.Jobs select d).ToList();

                List<AHVExpiryReport> objAHVExpiryReportList = new List<AHVExpiryReport>();
                string hearingDate = string.Empty;

                string AHVReferenceNumber = string.Empty;
                if (dataTableParameters != null && dataTableParameters.AHVReferenceNumber != null && !string.IsNullOrEmpty(dataTableParameters.AHVReferenceNumber))
                {
                    AHVReferenceNumber = dataTableParameters.AHVReferenceNumber;

                    hearingDate = hearingDate + (!string.IsNullOrEmpty(AHVReferenceNumber) && !string.IsNullOrEmpty(AHVReferenceNumber) ? " | " : string.Empty) + (!string.IsNullOrEmpty(AHVReferenceNumber) ? "AHVReferenceNumber : " + AHVReferenceNumber : string.Empty);
                }
                List<Rpo.ApiServices.Model.Models.JobDocument> jobDocumentResponse = jobDocumentResponse = rpoContext.JobDocuments.Where(d => d.IdDocument == idPW517Document).Include("JobDocumentFields.DocumentField.Field").ToList();


                if (dataTableParameters != null && dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobDocumentResponse = jobDocumentResponse.Where(d => joNumbers.Contains(d.IdJob)).ToList();

                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));

                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Job #: " + filteredJob : string.Empty);
                }


                if (dataTableParameters != null && dataTableParameters.IdJobCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdJobCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdJobCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdJobCompany) ? (dataTableParameters.IdJobCompany.Split('-') != null && dataTableParameters.IdJobCompany.Split('-').Any() ? dataTableParameters.IdJobCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                    jobDocumentResponse = jobDocumentResponse.Where(d => jobCompanyTeam.Contains((d.Job.IdCompany != null ? d.Job.IdCompany.Value : 0))).ToList();

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }


                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();
                CultureInfo provider = CultureInfo.CurrentCulture;

                if (dataTableParameters != null && dataTableParameters.ExpiresFromDate != null)
                {
                    // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresFromDate.ToString(), "MM/dd/yyyy", provider);
                    //DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresFromDate.ToString(), "dd/MM/yyyy", provider);
                    fromDate = dataTableParameters.ExpiresFromDate.Value;
                }
                else
                {
                    //string fmdate = DateTime.Now.AddDays(-30).ToShortDateString();
                    fromDate = DateTime.Now.AddDays(-30);

                    //fromDate = DateTime.ParseExact(fmdate, "dd/MM/yyyy", provider);
                    //fromDate = DateTime.ParseExact(fmdate, "MM/dd/yyyy", provider);
                }


                if (dataTableParameters != null && dataTableParameters.ExpiresToDate != null)
                {
                    // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresToDate.ToString(), "MM/dd/yyyy", provider);

                    // DateTime dtvalue = DateTime.ParseExact(dataTableParameters.ExpiresToDate.ToString(), "dd/MM/yyyy", provider);

                    toDate = dataTableParameters.ExpiresToDate.Value;
                }
                else
                {
                    //string todatenw = DateTime.Now.AddDays(+30).ToShortDateString();
                    toDate = DateTime.Now.AddDays(+30);
                    //toDate = DateTime.ParseExact(todatenw, "MM/dd/yyyy", provider);
                    // toDate = DateTime.ParseExact(todatenw, "dd/MM/yyyy", provider);
                }


                hearingDate = hearingDate + (!string.IsNullOrEmpty(fromDate.ToString()) && !string.IsNullOrEmpty(fromDate.ToString()) ? " | " : string.Empty) + (!string.IsNullOrEmpty(fromDate.ToString()) ? "FromDate : " + fromDate.ToString() : string.Empty);



                hearingDate = hearingDate + (!string.IsNullOrEmpty(toDate.ToString()) && !string.IsNullOrEmpty(toDate.ToString()) ? " | " : string.Empty) + (!string.IsNullOrEmpty(toDate.ToString()) ? "ToDate : " + toDate.ToString() : string.Empty);

                if (dataTableParameters != null && dataTableParameters.AHVReferenceNumber != null && AHVReferenceNumber != "")
                {
                    var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "AHVReferenceNumber" && b.Value == AHVReferenceNumber)).ToList();

                    if (aa != null)
                    {
                        jobDocumentResponse = aa;
                    }
                }
                if (dataTableParameters != null && dataTableParameters.Type != null && !string.IsNullOrEmpty(dataTableParameters.Type))
                {
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(dataTableParameters.Type) && !string.IsNullOrEmpty(dataTableParameters.Type) ? " | " : string.Empty) + (!string.IsNullOrEmpty(dataTableParameters.Type) ? "Type : " + dataTableParameters.Type : string.Empty);

                    List<string> jobType = dataTableParameters.Type != null && !string.IsNullOrEmpty(dataTableParameters.Type) ? (dataTableParameters.Type.Split('-') != null && dataTableParameters.Type.Split('-').Any() ? dataTableParameters.Type.ToLower().Trim().Split('-').Select(x => x).ToList() : new List<string>()) : new List<string>();

                    var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "Type" && jobType.Contains(b.ActualValue.ToLower().Trim() ?? ""))).ToList();

                    if (aa != null)
                    {
                        jobDocumentResponse = aa;
                    }
                }
                if (dataTableParameters != null && dataTableParameters.IdApplicantCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicantCompany))
                {
                    List<string> objApplicantCompany = dataTableParameters.IdApplicantCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdApplicantCompany) ? (dataTableParameters.IdApplicantCompany.Split('-') != null && dataTableParameters.IdApplicantCompany.ToLower().Trim().Split('-').Any() ? dataTableParameters.IdApplicantCompany.ToLower().Trim().Split('-').Select(x => x).ToList() : new List<string>()) : new List<string>();

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => objApplicantCompany.Contains(x.Id.ToString() ?? "")).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "ApplicantCompany : " + filteredCompany : string.Empty);


                    var strconteacts = (from d in rpoContext.JobContacts where objApplicantCompany.Contains(d.IdCompany.ToString() ?? "") select d.Id.ToString()).ToList();

                    var aatmp = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "txtLast")).ToList();

                    var bb = aatmp.Where(d => d.JobDocumentFields.Any(b => objApplicantCompany.Contains(b.Value ?? ""))).ToList();

                    var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "txtLast" && strconteacts.Contains(b.Value.ToString() ?? ""))).ToList();

                    if (aa != null)
                    {
                        jobDocumentResponse = aa;
                    }
                }

                if (fromDate != null && toDate != null)
                {
                    var aa = jobDocumentResponse.Where(a => a.JobDocumentFields.Any(b => b.DocumentField.Field.FieldName == "Start Date"

                    && DateTime.ParseExact(b.Value, "MM/dd/yy", provider).Date >= fromDate.Date && DateTime.ParseExact(b.Value, "MM/dd/yy", provider).Date <= toDate.Date)).ToList();
                    if (aa != null)
                    {
                        jobDocumentResponse = aa;
                    }
                }

                foreach (var item in jobDocumentResponse)
                {
                    List<JobDocumentField> jobDocumentFields = item.JobDocumentFields.ToList();


                    Job objjobs = (from d in this.rpoContext.Jobs.Include("Borough") where d.Id == item.IdJob select d).FirstOrDefault();

                    JobDocumentField jobDocumentField = new JobDocumentField();

                    List<JobDocumentField> objDocFields = new List<JobDocumentField>();

                    AHVExpiryReport objAHVExpiryReport = new AHVExpiryReport();

                    objAHVExpiryReport.IdJob = item.IdJob.ToString();
                    objAHVExpiryReport.JobNumber = item.Job.JobNumber.ToString();
                    objAHVExpiryReport.CreatedModifiedDate = item.LastModifiedDate != null ? item.LastModifiedDate : item.CreatedDate;

                    var objAHvReferenceNumber = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "AHVReferenceNumber") select d.Value).FirstOrDefault();

                    objAHVExpiryReport.AHVReferenceNumber = objAHvReferenceNumber != null ? objAHvReferenceNumber : "";

                    var objType = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Type") select d.ActualValue).FirstOrDefault();

                    objAHVExpiryReport.Type = objType != null ? objType : "";


                    objAHVExpiryReport.JobAddress = objjobs.HouseNumber + " " + objjobs.StreetNumber + ", " + objjobs.Borough.Description;

                    var objApplicationNo = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Application") select d).FirstOrDefault();

                    objAHVExpiryReport.ApplicationNo = objApplicationNo != null && objApplicationNo.JobDocument != null && objApplicationNo.JobDocument.JobApplication != null && objApplicationNo.JobDocument.JobApplication.ApplicationNumber != null ? objApplicationNo.JobDocument.JobApplication.ApplicationNumber : "";

                    var objWorkPermit = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Work Permits") select d.ActualValue).FirstOrDefault();
                    objAHVExpiryReport.WorkPermitType = objWorkPermit != null ? objWorkPermit : "";

                    var objApplicant = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "txtLast") select d.Value).FirstOrDefault();

                    string ApplicantCompany = string.Empty;
                    DateTime? ApplicantExpiryDate = null;
                    int? ApplicantId = null;
                    if (objApplicant != null)
                    {
                        int applicantid = int.Parse(objApplicant);
                        var objapplicantCompany = (from d in rpoContext.JobContacts where d.Id == applicantid select d).FirstOrDefault();
                        if (objapplicantCompany != null && objapplicantCompany.Company != null)
                        {
                            ApplicantCompany = objapplicantCompany.Company.Name;
                            ApplicantId = objapplicantCompany.Company.Id;
                            ApplicantExpiryDate = objapplicantCompany.Company.TrackingExpiry;
                        }
                    }

                    objAHVExpiryReport.ApplicantCompany = ApplicantCompany;


                    objAHVExpiryReport.TrackingExpiryDate = ApplicantExpiryDate != null ? ApplicantExpiryDate : null;
                    objAHVExpiryReport.JobCompany = item.Job != null && item.Job.Company != null ? item.Job.Company.Name : string.Empty;

                    string[] objstrarray = item.DocumentDescription.Split(new char[] { '|' });

                    string Description = string.Empty;

                    foreach (var items in objstrarray)
                    {
                        if (items.Contains("Dates"))
                        {
                            Description = items.ToString();
                        }
                    }

                    var objIssueDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Issued Date") select d.Value).FirstOrDefault();

                    if (objIssueDate != null && !string.IsNullOrEmpty(objIssueDate))
                    {
                        DateTime dtvalue = DateTime.ParseExact(objIssueDate, "MM/dd/yy", provider);

                        objAHVExpiryReport.IssuedDate = dtvalue;
                    }

                    var objSubmittedDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Submitted Date") select d.Value).FirstOrDefault();

                    if (objSubmittedDate != null && !string.IsNullOrEmpty(objSubmittedDate))
                    {
                        DateTime dtvalue = DateTime.ParseExact(objSubmittedDate, "MM/dd/yy", provider);

                        objAHVExpiryReport.SubmittedDate = dtvalue;
                    }
                    var objNextDate = (from d in item.JobDocumentFields.Where(d => d.DocumentField.Field.FieldName == "Next Date") select d.Value).FirstOrDefault();
                    if (objNextDate != null && !string.IsNullOrEmpty(objNextDate))
                    {
                        DateTime dtvalue = DateTime.ParseExact(objNextDate, "MM/dd/yy", provider);

                        objAHVExpiryReport.NextDate = dtvalue;
                    }


                    objAHVExpiryReport.IdJobCompany = item.Job != null ? item.Job.IdCompany : null;
                    objAHVExpiryReport.IdApplicantCompany = ApplicantId;
                    objAHVExpiryReport.DocumentDescription = Description != "" ? Description : "";

                    objAHVExpiryReportList.Add(objAHVExpiryReport);

                    endloop:
                    Console.WriteLine("");


                }

                string direction = string.Empty;
                string orderBy = string.Empty;

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
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobNumber).ToList();
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobAddress).ToList();
                    if (orderBy.Trim().ToLower() == "JobCompany".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobCompany).ToList();
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.JobApplicationType).ToList();
                    if (orderBy.Trim().ToLower() == "TrackingExpiryDate".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.TrackingExpiryDate).ToList();
                    if (orderBy.Trim().ToLower() == "ApplicationNo".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.ApplicationNo).ToList();
                    if (orderBy.Trim().ToLower() == "ApplicantCompany".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.ApplicantCompany).ToList();
                    if (orderBy.Trim().ToLower() == "WorkPermitType".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.WorkPermitType).ToList();
                    if (orderBy.Trim().ToLower() == "AHVReferenceNumber".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.AHVReferenceNumber).ToList();
                    if (orderBy.Trim().ToLower() == "Type".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.Type).ToList();
                    if (orderBy.Trim().ToLower() == "DocumentDescription".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.DocumentDescription).ToList();
                    if (orderBy.Trim().ToLower() == "SubmittedDate".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.SubmittedDate).ToList();
                    if (orderBy.Trim().ToLower() == "IssuedDate".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.IssuedDate).ToList();
                    if (orderBy.Trim().ToLower() == "NextDate".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.NextDate).ToList();
                    if (orderBy.Trim().ToLower() == "CreatedModifiedDate".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderByDescending(o => o.CreatedModifiedDate).ToList();
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobNumber).ToList();
                    if (orderBy.Trim().ToLower() == "JobAddress".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobAddress).ToList();
                    if (orderBy.Trim().ToLower() == "JobCompany".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobCompany).ToList();
                    if (orderBy.Trim().ToLower() == "JobApplicationType".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.JobApplicationType).ToList();
                    if (orderBy.Trim().ToLower() == "TrackingExpiryDate".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.TrackingExpiryDate).ToList();
                    if (orderBy.Trim().ToLower() == "ApplicationNo".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.ApplicationNo).ToList();
                    if (orderBy.Trim().ToLower() == "ApplicantCompany".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.ApplicantCompany).ToList();
                    if (orderBy.Trim().ToLower() == "WorkPermitType".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.WorkPermitType).ToList();
                    if (orderBy.Trim().ToLower() == "AHVReferenceNumber".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.AHVReferenceNumber).ToList();
                    if (orderBy.Trim().ToLower() == "Type".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.Type).ToList();
                    if (orderBy.Trim().ToLower() == "DocumentDescription".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.DocumentDescription).ToList();
                    if (orderBy.Trim().ToLower() == "SubmittedDate".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.SubmittedDate).ToList();
                    if (orderBy.Trim().ToLower() == "IssuedDate".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.IssuedDate).ToList();
                    if (orderBy.Trim().ToLower() == "NextDate".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.NextDate).ToList();
                    if (orderBy.Trim().ToLower() == "CreatedModifiedDate".Trim().ToLower())
                        objAHVExpiryReportList = objAHVExpiryReportList.OrderBy(o => o.CreatedModifiedDate).ToList();
                }
                #endregion



                if (objAHVExpiryReportList != null && objAHVExpiryReportList.Count > 0)
                {
                    string exportFilename = "AHVExpirationReport" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    string exportFilePath = ExportToPdf(hearingDate, objAHVExpiryReportList, exportFilename);
                    string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);

                    string path = APIUrl + "/UploadedDocuments/ReportExportToExcel/" + exportFilename;

                    return Ok(new { Filepath = exportFilePath, NewPath = path, ReportName = exportFilename });
                }
                else
                {
                    throw new RpoBusinessException(StaticMessages.NoRecordFoundMessage);
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }



        private string ExportToPdf(string hearingDate, List<AHVExpiryReport> result, string exportFilename)
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
                //cell.Colspan = 12;
                //cell.PaddingLeft = -60;
                //table.AddCell(cell);

                //cell = new PdfPCell(new Phrase(reportHeader, font_11_Normal));
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 12;
                //cell.PaddingLeft = -60;
                //cell.VerticalAlignment = Element.ALIGN_TOP;
                //table.AddCell(cell);


                //cell = new PdfPCell(SnapCorLogo);
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Rowspan = 2;
                ////cell.PaddingLeft = -80;
                //cell.PaddingTop = -60;
                //cell.Colspan = 13;
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
                cell.PaddingLeft = -10;
                cell.Colspan = 12;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -10;
                cell.Colspan = 12;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = -10;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("AHV PERMIT EXPIRY REPORT", font_16_Bold));
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

                string reportDate = "Date:" + DateTime.Today.ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat);

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

                cell = new PdfPCell(new Phrase("Company", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Tracking Expiry Date", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Work Permit Type", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("AHV Reference", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Type", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Applicant Company", font_Table_Header));
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

                cell = new PdfPCell(new Phrase("Submitted Date", font_Table_Header));
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

                cell = new PdfPCell(new Phrase("Next Date Needed", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Created/Modified Date", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                //cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //table.AddCell(cell);

                foreach (AHVExpiryReport item in result)
                {
                    cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.JobAddress, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.JobCompany, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.TrackingExpiryDate != null ? Convert.ToDateTime(item.TrackingExpiryDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.WorkPermitType, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.AHVReferenceNumber, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(!string.IsNullOrEmpty(item.Type) ? item.Type : item.Type, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.ApplicantCompany, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.DocumentDescription, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.SubmittedDate != null ? Convert.ToDateTime(item.SubmittedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.IssuedDate != null ? Convert.ToDateTime(item.IssuedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.NextDate != null ? Convert.ToDateTime(item.NextDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.CreatedModifiedDate != null ? Convert.ToDateTime(item.CreatedModifiedDate).ToString(Rpo.ApiServices.Api.Tools.Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //cell.Border = PdfPCell.NO_BORDER;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //table.AddCell(cell);
                }
                //cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.Colspan = 11;
                //table.AddCell(cell);

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
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

