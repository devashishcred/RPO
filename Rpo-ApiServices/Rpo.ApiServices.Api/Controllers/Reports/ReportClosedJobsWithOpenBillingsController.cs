
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
    using Api.Jobs;
    using System.Globalization;
    using System.Web;
    using System.IO;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    [Authorize]
    public class ReportClosedJobsWithOpenBillingsController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();
        /// <summary>
        /// Get the report ofReportClosed JobsWith OpenBillings Report with filter and sorting 
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetReportClosedJobsWithOpenBillings([FromUri] ClosedJobsWithOpenBillingDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewClosedScopeReport))
            {
                List<int> idJobFeescheduleList = this.rpoContext.JobMilestoneServices.Include("Milestone.Job.RfpAddress")
                .Where(x => (x.Milestone.InvoiceNumber ?? string.Empty) == string.Empty && x.Milestone.Status == "Pending")
                .Select(x => x.IdJobFeeSchedule).ToList();

                var milestones = this.rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").Where(x =>
                   (x.InvoiceNumber ?? string.Empty) == string.Empty
                   // && x.Status == "Pending" 
                   && x.Job.Status == Model.Models.Enums.JobStatus.Close).AsQueryable();

                var scopes = this.rpoContext.JobFeeSchedules.Include("Job.RfpAddress.Borough").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x =>
                   (x.InvoiceNumber ?? string.Empty) == string.Empty && x.RfpWorkType.IsShowScope == false && x.IsRemoved == false
                   // && x.Status == "Pending"
                   && !idJobFeescheduleList.Contains(x.Id) && x.Job.Status == Model.Models.Enums.JobStatus.Close && x.IsAdditionalService == true).AsQueryable();


                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    milestones = milestones.Where(x => joNumbers.Contains(x.IdJob));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    scopes = scopes.Where(x => joNumbers.Contains(x.IdJob));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    milestones = milestones.Where(x => jobCompanyTeam.Contains((x.Job.IdCompany ?? 0)));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    scopes = scopes.Where(x => jobCompanyTeam.Contains((x.Job.IdCompany ?? 0)));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    milestones = milestones.Where(x => jobContactTeam.Contains((x.Job.IdContact.Value)));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    scopes = scopes.Where(x => jobContactTeam.Contains((x.Job.IdContact.Value)));
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    milestones = milestones.Where(x => x.Job.Status == dataTableParameters.Status);
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    scopes = scopes.Where(x => x.Job.Status == dataTableParameters.Status);
                }

                List<ClosedJobsWithOpenBillingDTO> completedScopeBillingPointDTOList = new List<ClosedJobsWithOpenBillingDTO>();

                completedScopeBillingPointDTOList.AddRange(milestones.AsEnumerable().Select(x => FormatJobMilestoneReport(x)));

                completedScopeBillingPointDTOList.AddRange(scopes.AsEnumerable().Select(x => FormatJobFeeScheduleReport(x)));

                var recordsTotal = completedScopeBillingPointDTOList.Count();
                var recordsFiltered = recordsTotal;

                var result = completedScopeBillingPointDTOList
                    .AsEnumerable()
                    .Select(c => new
                    {
                        IdJob = c.IdJob,
                        JobApplicationType = c.JobApplicationType,
                        JobNumber = c.JobNumber,
                        Address = c.Address,
                        Company = c.Company,
                        Apartment = c.Apartment,
                        FloorNumber = c.FloorNumber,
                        SpecialPlaceName = c.SpecialPlaceName,
                        StartDate = c.StartDate,
                        IdCompany = c.IdCompany,
                        IdContact = c.IdContact,
                        Contact = c.Contact,
                        EndDate = c.EndDate
                    }).Distinct()
                    .AsQueryable()
                    .DataTableParameters(dataTableParameters, out recordsFiltered)
                    .ToArray();

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
        ///  /// <summary>
        /// Get the report ofReportClosed JobsWith OpenBillings Report with filter and sorting  and export to excel
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns> 
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportClosedJobsWithOpenBillings/exporttoexcel")]
        public IHttpActionResult PostExportReportClosedJobsWithOpenBillingsToExcel(ClosedJobsWithOpenBillingDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                List<int> idJobFeescheduleList = this.rpoContext.JobMilestoneServices.Include("Milestone.Job.RfpAddress")
                .Where(x => (x.Milestone.InvoiceNumber ?? string.Empty) == string.Empty && x.Milestone.Status == "Completed")
                .Select(x => x.IdJobFeeSchedule).ToList();

                var milestones = this.rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").Where(x =>
                   (x.InvoiceNumber ?? string.Empty) == string.Empty
                   //&& x.Status == "Completed" 
                   && x.Job.Status == Model.Models.Enums.JobStatus.Close).AsQueryable();

                var scopes = this.rpoContext.JobFeeSchedules.Include("Job.RfpAddress.Borough").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x =>
                   (x.InvoiceNumber ?? string.Empty) == string.Empty && x.RfpWorkType.IsShowScope == false && x.IsRemoved == false
                   // && x.Status == "Completed" 
                   && x.Job.Status == Model.Models.Enums.JobStatus.Close
                   && !idJobFeescheduleList.Contains(x.Id) && x.IsAdditionalService == true).AsQueryable();

                string hearingDate = string.Empty;
                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    milestones = milestones.Where(x => joNumbers.Contains(x.IdJob));

                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Project #: " + filteredJob : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    scopes = scopes.Where(x => joNumbers.Contains(x.IdJob));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    milestones = milestones.Where(x => jobCompanyTeam.Contains((x.Job.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    scopes = scopes.Where(x => jobCompanyTeam.Contains((x.Job.IdCompany ?? 0)));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    milestones = milestones.Where(x => jobContactTeam.Contains((x.Job.IdContact.Value)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    scopes = scopes.Where(x => jobContactTeam.Contains((x.Job.IdContact.Value)));
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    milestones = milestones.Where(x => x.Job.Status == dataTableParameters.Status);

                    string filteredStatus = dataTableParameters.Status.ToString();
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredStatus) ? "Status : " + filteredStatus : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    scopes = scopes.Where(x => x.Job.Status == dataTableParameters.Status);
                }

                List<ClosedJobsWithOpenBillingDTO> completedScopeBillingPointDTOList = new List<ClosedJobsWithOpenBillingDTO>();

                completedScopeBillingPointDTOList.AddRange(milestones.AsEnumerable().Select(x => FormatJobMilestoneReport(x)));

                completedScopeBillingPointDTOList.AddRange(scopes.AsEnumerable().Select(x => FormatJobFeeScheduleReport(x)));

                //List<ClosedJobsWithOpenBillingDTO> result = completedScopeBillingPointDTOList
                //    .AsEnumerable()
                //    .Select(c => new ClosedJobsWithOpenBillingDTO
                //    {
                //        IdJob = c.IdJob,
                //        JobNumber = c.JobNumber,
                //        Address = c.Address,
                //        Company = c.Company,
                //        Apartment = c.Apartment,
                //        FloorNumber = c.FloorNumber,
                //        SpecialPlaceName = c.SpecialPlaceName,
                //        StartDate = c.StartDate,
                //        IdCompany = c.IdCompany,
                //        IdContact = c.IdContact,
                //        Contact = c.Contact,
                //    }).Distinct()
                //    .AsQueryable().OrderBy(x => x.JobNumber).ThenBy(x => x.Address)
                //    .ToList();

                List<ClosedJobsWithOpenBillingDTO> result1 = new List<ClosedJobsWithOpenBillingDTO>();

                var result = completedScopeBillingPointDTOList
                 .AsEnumerable()
                 .Select(c => new
                 {
                     IdJob = c.IdJob,
                     JobApplicationType = c.JobApplicationType,
                     JobNumber = c.JobNumber,
                     Address = c.Address,
                     Company = c.Company,
                     Apartment = c.Apartment,
                     FloorNumber = c.FloorNumber,
                     SpecialPlaceName = c.SpecialPlaceName,
                     StartDate = c.StartDate,
                     IdCompany = c.IdCompany,
                     IdContact = c.IdContact,
                     Contact = c.Contact,
                     EndDate = c.EndDate
                 })
                 .AsQueryable().Distinct()
                 .ToList();

                result1 = result.Select(c => new ClosedJobsWithOpenBillingDTO
                {
                    IdJob = c.IdJob,
                    JobApplicationType = c.JobApplicationType,
                    JobNumber = c.JobNumber,
                    Address = c.Address,
                    Company = c.Company,
                    Apartment = c.Apartment,
                    FloorNumber = c.FloorNumber,
                    SpecialPlaceName = c.SpecialPlaceName,
                    StartDate = c.StartDate,
                    IdCompany = c.IdCompany,
                    IdContact = c.IdContact,
                    Contact = c.Contact,
                    EndDate = c.EndDate
                })
                 .AsQueryable().Distinct()
                 .AsQueryable().OrderBy(x => x.JobNumber).ThenBy(x => x.Address)
                 .ToList();


                string exportFilename = "ClosedProjectsWithOpenBillingReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                string exportFilePath = ExportToExcel(hearingDate, result1, exportFilename);

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

        private string ExportToExcel(string hearingDate, List<ClosedJobsWithOpenBillingDTO> result, string exportFilename)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
            //templateFileName = "ClosedJobsWithOpenBillingTemplate.xlsx";
            templateFileName = "ClosedProjectsWithOpenBillingTemplate - Copy.xlsx";


            string templateFilePath = templatePath + templateFileName;
            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateFilePath);
            ISheet sheet = templateWorkbook.GetSheet("Sheet1");

            XSSFFont myFont = (XSSFFont)templateWorkbook.CreateFont();
            myFont.FontHeightInPoints = (short)12;
            myFont.FontName = "Times New Roman";
            myFont.IsBold = false;
            XSSFFont myFontbig = (XSSFFont)templateWorkbook.CreateFont();
            myFontbig.FontHeightInPoints = (short)14;
            myFontbig.FontName = "Times New Roman";
            myFontbig.IsBold = true;

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

            XSSFCellStyle CenterAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            CenterAlignCellStyle.SetFont(myFontbig);
            //CenterAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            //CenterAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            //CenterAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            //CenterAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            CenterAlignCellStyle.WrapText = true;
            CenterAlignCellStyle.VerticalAlignment = VerticalAlignment.Center;
            CenterAlignCellStyle.Alignment = HorizontalAlignment.Center;

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
            iDateRow.GetCell(0).CellStyle = CenterAlignCellStyle;

            string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);
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
            foreach (var item in result)
            {
                IRow iRow = sheet.GetRow(index);
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
                        iRow.GetCell(3).SetCellValue(item.SpecialPlaceName);
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(item.SpecialPlaceName);
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.StartDate != null ? Convert.ToDateTime(item.StartDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.StartDate != null ? Convert.ToDateTime(item.StartDate).ToString(Common.ExportReportDateFormat) : string.Empty);
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
                        iRow.GetCell(6).SetCellValue(item.Contact);
                    }
                    else
                    {
                        iRow.CreateCell(6).SetCellValue(item.Contact);
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
                        iRow.GetCell(0).SetCellValue(item.JobNumber + " | " + item.Address);
                    }
                    else
                    {
                        iRow.CreateCell(0).SetCellValue(item.JobNumber + " | " + item.Address);
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
                        iRow.GetCell(3).SetCellValue(item.SpecialPlaceName);
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(item.SpecialPlaceName);
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.StartDate != null ? Convert.ToDateTime(item.StartDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.StartDate != null ? Convert.ToDateTime(item.StartDate).ToString(Common.ExportReportDateFormat) : string.Empty);
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
                        iRow.GetCell(6).SetCellValue(item.Contact);
                    }
                    else
                    {
                        iRow.CreateCell(6).SetCellValue(item.Contact);
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

            using (var file2 = new FileStream(exportFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                templateWorkbook.Write(file2);
                file2.Close();
            }

            return exportFilePath;
        }
        /// <summary>
        /// //// Get the report ofReportClosed JobsWith OpenBillings Report with filter and sorting  and export to pdf
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportClosedJobsWithOpenBillings/exporttopdf")]
        public IHttpActionResult PostExportReportClosedJobsWithOpenBillingsToPdf(ClosedJobsWithOpenBillingDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                List<int> idJobFeescheduleList = this.rpoContext.JobMilestoneServices.Include("Milestone.Job.RfpAddress")
                .Where(x => (x.Milestone.InvoiceNumber ?? string.Empty) == string.Empty && x.Milestone.Status == "Completed")
                .Select(x => x.IdJobFeeSchedule).ToList();

                var milestones = this.rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").Where(x =>
                   (x.InvoiceNumber ?? string.Empty) == string.Empty
                   // && x.Status == "Completed" 
                   && x.Job.Status == Model.Models.Enums.JobStatus.Close).AsQueryable();

                var scopes = this.rpoContext.JobFeeSchedules.Include("Job.RfpAddress.Borough").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x =>
                   (x.InvoiceNumber ?? string.Empty) == string.Empty && x.RfpWorkType.IsShowScope == false && x.IsRemoved == false
                   //&& x.Status == "Completed" 
                   && x.Job.Status == Model.Models.Enums.JobStatus.Close
                   && !idJobFeescheduleList.Contains(x.Id) && x.IsAdditionalService == true).AsQueryable();


                string hearingDate = string.Empty;
                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    milestones = milestones.Where(x => joNumbers.Contains(x.IdJob));

                    string filteredJob = string.Join(", ", rpoContext.Jobs.Where(x => joNumbers.Contains(x.Id)).Select(x => x.JobNumber));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredJob) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredJob) ? "Project #: " + filteredJob : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    scopes = scopes.Where(x => joNumbers.Contains(x.IdJob));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    milestones = milestones.Where(x => jobCompanyTeam.Contains((x.Job.IdCompany ?? 0)));

                    string filteredCompany = string.Join(", ", rpoContext.Companies.Where(x => jobCompanyTeam.Contains(x.Id)).Select(x => x.Name));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredCompany) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredCompany) ? "Company : " + filteredCompany : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
                {
                    List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    scopes = scopes.Where(x => jobCompanyTeam.Contains((x.Job.IdCompany ?? 0)));
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    milestones = milestones.Where(x => jobContactTeam.Contains((x.Job.IdContact.Value)));

                    string filteredContact = string.Join(", ", rpoContext.Contacts.Where(x => jobContactTeam.Contains(x.Id)).Select(x => (x.FirstName ?? "") + " " + (x.LastName ?? "")));
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredContact) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredContact) ? "Contact : " + filteredContact : string.Empty);
                }

                if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
                {
                    List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    scopes = scopes.Where(x => jobContactTeam.Contains((x.Job.IdContact.Value)));
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    milestones = milestones.Where(x => x.Job.Status == dataTableParameters.Status);

                    string filteredStatus = dataTableParameters.Status.ToString();
                    hearingDate = hearingDate + (!string.IsNullOrEmpty(hearingDate) && !string.IsNullOrEmpty(filteredStatus) ? " | " : string.Empty) + (!string.IsNullOrEmpty(filteredStatus) ? "Status : " + filteredStatus : string.Empty);
                }

                if (dataTableParameters != null && dataTableParameters.Status != null)
                {
                    scopes = scopes.Where(x => x.Job.Status == dataTableParameters.Status);
                }

                List<ClosedJobsWithOpenBillingDTO> completedScopeBillingPointDTOList = new List<ClosedJobsWithOpenBillingDTO>();

                completedScopeBillingPointDTOList.AddRange(milestones.AsEnumerable().Select(x => FormatJobMilestoneReport(x)));

                completedScopeBillingPointDTOList.AddRange(scopes.AsEnumerable().Select(x => FormatJobFeeScheduleReport(x)));

                List<ClosedJobsWithOpenBillingDTO> result1 = new List<ClosedJobsWithOpenBillingDTO>();

                var result = completedScopeBillingPointDTOList
                 .AsEnumerable()
                 .Select(c => new
                 {
                     IdJob = c.IdJob,
                     JobApplicationType = c.JobApplicationType,
                     JobNumber = c.JobNumber,
                     Address = c.Address,
                     Company = c.Company,
                     Apartment = c.Apartment,
                     FloorNumber = c.FloorNumber,
                     SpecialPlaceName = c.SpecialPlaceName,
                     StartDate = c.StartDate,
                     IdCompany = c.IdCompany,
                     IdContact = c.IdContact,
                     Contact = c.Contact,
                     EndDate = c.EndDate
                 })
                 .AsQueryable().Distinct()
                 .ToList();

                result1 = result.Select(c => new ClosedJobsWithOpenBillingDTO
                {
                    IdJob = c.IdJob,
                    JobApplicationType = c.JobApplicationType,
                    JobNumber = c.JobNumber,
                    Address = c.Address,
                    Company = c.Company,
                    Apartment = c.Apartment,
                    FloorNumber = c.FloorNumber,
                    SpecialPlaceName = c.SpecialPlaceName,
                    StartDate = c.StartDate,
                    IdCompany = c.IdCompany,
                    IdContact = c.IdContact,
                    Contact = c.Contact,
                    EndDate = c.EndDate
                })
                 .AsQueryable().Distinct()
                 .AsQueryable().OrderBy(x => x.JobNumber).ThenBy(x => x.Address)
                 .ToList();

                string exportFilename = "ClosedProjectsWithOpenBillingReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                string exportFilePath = ExportToPdf(hearingDate, result1, exportFilename);

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

        private string ExportToPdf(string hearingDate, List<ClosedJobsWithOpenBillingDTO> result, string exportFilename)
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

                cell = new PdfPCell(new Phrase("CLOSED PROJECTS WITH OPEN BILLING REPORT", font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 7;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(hearingDate, font_12_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 7;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

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
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project # | Address", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Apt", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Floor", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Special Place", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Start", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Company", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Contact", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                foreach (ClosedJobsWithOpenBillingDTO item in result)
                {
                    cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.Address, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Apartment, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.FloorNumber, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.SpecialPlaceName, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.StartDate != null ? Convert.ToDateTime(item.StartDate).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Company, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Contact, font_Table_Data));
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
        private ClosedJobsWithOpenBillingDTO FormatJobMilestoneReport(JobMilestone jobMilestone)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            string JobApplicationType = string.Empty;

            if (jobMilestone.Job != null && jobMilestone.IdJob != null)
            {
                JobApplicationType = string.Join(",", jobMilestone.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }

            return new ClosedJobsWithOpenBillingDTO
            {
                IdJob = jobMilestone.IdJob,
                JobNumber = jobMilestone != null && jobMilestone.Job != null ? jobMilestone.Job.JobNumber : string.Empty,
                JobApplicationType = JobApplicationType,
                Address = jobMilestone != null && jobMilestone.Job != null && jobMilestone.Job.RfpAddress != null ? (jobMilestone.Job.RfpAddress.HouseNumber + " " + (jobMilestone.Job.RfpAddress.Street != null ? jobMilestone.Job.RfpAddress.Street + ", " : string.Empty)
                                         + (jobMilestone.Job.RfpAddress.Borough != null ? jobMilestone.Job.RfpAddress.Borough.Description : string.Empty)) : string.Empty,
                Company = jobMilestone != null && jobMilestone.Job != null && jobMilestone.Job.Company != null ? jobMilestone.Job.Company.Name : string.Empty,
                Apartment = jobMilestone != null && jobMilestone.Job != null ? jobMilestone.Job.Apartment : string.Empty,
                FloorNumber = jobMilestone != null && jobMilestone.Job != null ? jobMilestone.Job.FloorNumber : string.Empty,
                SpecialPlaceName = jobMilestone != null && jobMilestone.Job != null ? jobMilestone.Job.SpecialPlace : string.Empty,
                StartDate = jobMilestone != null && jobMilestone.Job != null && jobMilestone.Job.StartDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobMilestone.Job.StartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobMilestone.Job.StartDate,
                EndDate = jobMilestone != null && jobMilestone.Job != null && jobMilestone.Job.EndDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobMilestone.Job.EndDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobMilestone.Job.EndDate,
                IdCompany = jobMilestone.Job.IdCompany,
                IdContact = jobMilestone != null && jobMilestone.Job != null && jobMilestone.Job.IdContact != null ? jobMilestone.Job.IdContact : null,
                Contact = jobMilestone != null && jobMilestone.Job != null && jobMilestone.Job.Contact != null ? jobMilestone.Job.Contact.FirstName + " " + jobMilestone.Job.Contact.LastName : string.Empty,
                //BillingPointName = jobMilestone.Name,
                //PONumber = jobMilestone.PONumber,
                //InvoiceNumber = jobMilestone.InvoiceNumber,
                //IsInvoiced = jobMilestone.IsInvoiced,
                //BillingCost = jobMilestone.Value,
                //FormattedBillingCost = jobMilestone.Value != null ? Convert.ToDouble(jobMilestone.Value).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                //Status = jobMilestone.Status
            };
        }

        private ClosedJobsWithOpenBillingDTO FormatJobFeeScheduleReport(JobFeeSchedule jobFeeSchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string JobApplicationType = string.Empty;

            if (jobFeeSchedule.Job != null && jobFeeSchedule.IdJob != null)
            {
                JobApplicationType = string.Join(",", jobFeeSchedule.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }

            return new ClosedJobsWithOpenBillingDTO
            {
                IdJob = jobFeeSchedule.IdJob,
                JobNumber = jobFeeSchedule != null && jobFeeSchedule.Job != null ? jobFeeSchedule.Job.JobNumber : string.Empty,
                JobApplicationType = JobApplicationType,
                Address = jobFeeSchedule != null && jobFeeSchedule.Job != null && jobFeeSchedule.Job.RfpAddress != null ? (jobFeeSchedule.Job.RfpAddress.HouseNumber + " " + (jobFeeSchedule.Job.RfpAddress.Street != null ? jobFeeSchedule.Job.RfpAddress.Street + ", " : string.Empty)
                                         + (jobFeeSchedule.Job.RfpAddress.Borough != null ? jobFeeSchedule.Job.RfpAddress.Borough.Description : string.Empty)) : string.Empty,
                Company = jobFeeSchedule != null && jobFeeSchedule.Job != null && jobFeeSchedule.Job.Company != null ? jobFeeSchedule.Job.Company.Name : string.Empty,
                Apartment = jobFeeSchedule != null && jobFeeSchedule.Job != null ? jobFeeSchedule.Job.Apartment : string.Empty,
                FloorNumber = jobFeeSchedule != null && jobFeeSchedule.Job != null ? jobFeeSchedule.Job.FloorNumber : string.Empty,
                SpecialPlaceName = jobFeeSchedule != null && jobFeeSchedule.Job != null ? jobFeeSchedule.Job.SpecialPlace : string.Empty,
                StartDate = jobFeeSchedule != null && jobFeeSchedule.Job != null && jobFeeSchedule.Job.StartDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobFeeSchedule.Job.StartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobFeeSchedule.Job.StartDate,
                EndDate = jobFeeSchedule != null && jobFeeSchedule.Job != null && jobFeeSchedule.Job.EndDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobFeeSchedule.Job.EndDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobFeeSchedule.Job.EndDate,
                IdCompany = jobFeeSchedule.Job.IdCompany,
                IdContact = jobFeeSchedule.Job.IdContact,
                Contact = jobFeeSchedule != null && jobFeeSchedule.Job != null && jobFeeSchedule.Job.Contact != null ? jobFeeSchedule.Job.Contact.FirstName + " " + jobFeeSchedule.Job.Contact.LastName : string.Empty,
                //BillingPointName = Common.GetServiceItemName(jobFeeSchedule),
                //PONumber = jobFeeSchedule.PONumber,
                //InvoiceNumber = jobFeeSchedule.InvoiceNumber,
                //IsInvoiced = jobFeeSchedule.IsInvoiced,
                //BillingCost = jobFeeSchedule.TotalCost,
                //FormattedBillingCost = jobFeeSchedule.TotalCost != null ? Convert.ToDouble(jobFeeSchedule.TotalCost).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                //Status = jobFeeSchedule.Status
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