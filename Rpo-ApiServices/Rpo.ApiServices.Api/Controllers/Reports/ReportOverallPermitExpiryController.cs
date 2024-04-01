
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
    using NPOI.SS.Util;
    [Authorize]
    public class ReportOverallPermitExpiryController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();
        private object CellStyle;

        //[ResponseType(typeof(DataTableResponse))]
        //[Authorize]
        //[RpoAuthorize]
        //[HttpGet]
        //public IHttpActionResult GetReportOverallPermitExpiry([FromUri] PermitsExpiryDataTableParameters dataTableParameters)
        //{
        //    var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
        //    if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewReport))
        //    {
        //        if (dataTableParameters != null && dataTableParameters.PermitCode == "DOB")
        //        {
        //            dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
        //            dataTableParameters.PermitCode = null;
        //        }
        //        else if (dataTableParameters != null && dataTableParameters.PermitCode == "DOT")
        //        {
        //            dataTableParameters.IdJobType = Enums.ApplicationType.DOT.GetHashCode();
        //            dataTableParameters.PermitCode = null;
        //        }
        //        else if (dataTableParameters != null && dataTableParameters.PermitCode == "DEP")
        //        {
        //            dataTableParameters.IdJobType = Enums.ApplicationType.DEP.GetHashCode();
        //            dataTableParameters.PermitCode = null;
        //        }
        //        else if (dataTableParameters != null && dataTableParameters.PermitCode == "AHV")
        //        {
        //            dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
        //        }
        //        else if (dataTableParameters != null && dataTableParameters.PermitCode == "TCO")
        //        {
        //            dataTableParameters.IdJobType = Enums.ApplicationType.DOB.GetHashCode();
        //        }

        //        var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
        //                                        .Include("JobApplication.JobApplicationType")
        //                                        .Include("JobApplication.ApplicationStatus")
        //                                        .Include("JobApplication.Job.RfpAddress.Borough")
        //                                        .Include("JobApplication.Job.Company")
        //                                        .Include("JobApplication.Job.Contact")
        //                                        .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
        //                                        .Where(x =>
        //                                                   x.JobApplication.JobApplicationType.IdParent == dataTableParameters.IdJobType
        //                                                   && ((DbFunctions.TruncateTime(x.Expires) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
        //                                                   && DbFunctions.TruncateTime(x.Expires) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
        //                                                   ))
        //                                        .AsQueryable();

        //        var recordsTotal = jobApplicationWorkPermitTypes.Count();
        //        var recordsFiltered = recordsTotal;
        //        string permitCode = string.Empty;
        //        switch (dataTableParameters.IdJobType)
        //        {
        //            case 1:
        //                permitCode = "DOB";
        //                break;
        //            case 2:
        //                permitCode = "DOT";
        //                break;
        //            case 4:
        //                permitCode = "DEP";
        //                break;
        //        }

        //        if (!string.IsNullOrEmpty(dataTableParameters.IdBorough))
        //        {
        //            List<int> boroughs = dataTableParameters.IdBorough != null && !string.IsNullOrEmpty(dataTableParameters.IdBorough) ? (dataTableParameters.IdBorough.Split('-') != null && dataTableParameters.IdBorough.Split('-').Any() ? dataTableParameters.IdBorough.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
        //            jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => boroughs.Contains(x.JobApplication.Job.RfpAddress.IdBorough ?? 0));
        //        }

        //        if (!string.IsNullOrEmpty(dataTableParameters.IdProjectManager))
        //        {
        //            List<int> projectManagers = dataTableParameters.IdProjectManager != null && !string.IsNullOrEmpty(dataTableParameters.IdProjectManager) ? (dataTableParameters.IdProjectManager.Split('-') != null && dataTableParameters.IdProjectManager.Split('-').Any() ? dataTableParameters.IdProjectManager.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
        //            jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => projectManagers.Contains(x.JobApplication.Job.IdProjectManager ?? 0));
        //        }

        //        if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
        //        {
        //            List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
        //            jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.JobApplication.IdJob));
        //        }

        //        if (dataTableParameters != null && dataTableParameters.PermitCode != null)
        //        {
        //            permitCode = dataTableParameters.PermitCode;
        //            jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code == dataTableParameters.PermitCode);
        //        }
        //        else
        //        {
        //            jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobWorkType.Code != "AHV" && x.JobWorkType.Code != "TCO");
        //        }

        //        if (dataTableParameters != null && dataTableParameters.Status != null)
        //        {
        //            jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => x.JobApplication.Job.Status == dataTableParameters.Status);
        //        }

        //        if (!string.IsNullOrEmpty(dataTableParameters.IdCompany))
        //        {
        //            List<int> jobCompanyTeam = dataTableParameters.IdCompany != null && !string.IsNullOrEmpty(dataTableParameters.IdCompany) ? (dataTableParameters.IdCompany.Split('-') != null && dataTableParameters.IdCompany.Split('-').Any() ? dataTableParameters.IdCompany.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
        //            jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobCompanyTeam.Contains((x.JobApplication.Job.IdCompany ?? 0)));
        //        }

        //        if (!string.IsNullOrEmpty(dataTableParameters.IdContact))
        //        {
        //            List<int> jobContactTeam = dataTableParameters.IdContact != null && !string.IsNullOrEmpty(dataTableParameters.IdContact) ? (dataTableParameters.IdContact.Split('-') != null && dataTableParameters.IdContact.Split('-').Any() ? dataTableParameters.IdContact.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
        //            jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => jobContactTeam.Contains((x.JobApplication.Job.IdContact)));
        //        }

        //        var result = jobApplicationWorkPermitTypes
        //            .AsEnumerable()
        //            .Select(c => this.FormatDOBPermitsExpiryReport(c, permitCode))
        //            .AsQueryable()
        //            .DataTableParameters(dataTableParameters, out recordsFiltered)
        //            .ToArray();

        //        return this.Ok(new DataTableResponse
        //        {
        //            Draw = dataTableParameters.Draw,
        //            RecordsFiltered = recordsFiltered,
        //            RecordsTotal = recordsTotal,
        //            Data = result.OrderByDescending(x => x.IdJob)
        //        });
        //    }
        //    else
        //    {
        //        throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
        //    }
        //}
        /// <summary>
        ///  Get the report of ReportOverallPermitExpiry Report with filter and sorting  and export to excel
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportOverallPermitExpiry/exporttoexcel")]
        public IHttpActionResult PostExportOverallPermitExpiryToExcel(OverallPermitDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                #region Filter

                DateTime ExpiresFromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                dataTableParameters.ExpiresFromDate = TimeZoneInfo.ConvertTimeToUtc(ExpiresFromDate, TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                dataTableParameters.ExpiresToDate = Convert.ToDateTime(dataTableParameters.ExpiresFromDate).AddDays(30);

                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                    .Include("JobApplication.JobApplicationType")
                                                    .Include("JobApplication.ApplicationStatus")
                                                    .Include("JobApplication.Job.RfpAddress.Borough")
                                                    .Include("JobApplication.Job.Company")
                                                    .Include("JobApplication.Job.Contact")
                                                    .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                    .Where(x => x.Expires != null
                                                               //((DbFunctions.TruncateTime(x.Expires) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
                                                               //&& DbFunctions.TruncateTime(x.Expires) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
                                                               //)
                                                               )
                                                    .AsQueryable();

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.JobApplication.IdJob));
                }

                List<PermitsExpiryDTO> result = jobApplicationWorkPermitTypes
                    .AsEnumerable()
                    .Select(c => this.FormatOverallPermitExpiryReport(c))
                    .AsQueryable().OrderBy(x => x.JobNumber).ThenBy(x => x.Expires)
                    .ToList();

                #endregion

                if (result != null && result.Count > 0)
                {
                    string exportFilename = string.Empty;
                    exportFilename = "ConsolidatedStatusReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                    string exportFilePath = ExportToExcel(result, exportFilename);

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

        private string ExportToExcel(List<PermitsExpiryDTO> result, string exportFilename)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
            templateFileName = "Overall_PermitExpirationTemplate - Copy.xlsx";
            string templateFilePath = templatePath + templateFileName;
            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateFilePath);
            List<string> jobNumberList = result.Select(x => x.JobNumber).Distinct().ToList();
            if (jobNumberList != null)
            {
                jobNumberList.AddRange(rpoContext.JobViolations.Where(x => x.IsFullyResolved == false && jobNumberList.Contains(x.IdJob.ToString())).Select(x => x.Job.JobNumber).ToList());
            }
            else
            {
                jobNumberList = rpoContext.JobViolations.Where(x => x.IsFullyResolved == false && jobNumberList.Contains(x.IdJob.ToString())).Select(x => x.Job.JobNumber).ToList();
            }

            jobNumberList = jobNumberList.Select(x => x).OrderBy(x => x).Distinct().ToList();


            int sheetIndex = 0;
            foreach (var item in jobNumberList)
            {
                if (sheetIndex == 0)
                {
                    templateWorkbook.SetSheetName(sheetIndex, "Project #" + item);
                    sheetIndex = sheetIndex + 1;
                }
                else
                {
                    templateWorkbook.CloneSheet(0);
                    templateWorkbook.SetSheetName(sheetIndex, "Project #" + item);
                    sheetIndex = sheetIndex + 1;
                }
            }

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
            leftAlignCellStyle.VerticalAlignment = VerticalAlignment.Center;
            leftAlignCellStyle.Alignment = HorizontalAlignment.Left;

            XSSFCellStyle rightAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            rightAlignCellStyle.SetFont(myFont);
            rightAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            rightAlignCellStyle.WrapText = true;
            rightAlignCellStyle.VerticalAlignment = VerticalAlignment.Center;
            rightAlignCellStyle.Alignment = HorizontalAlignment.Right;

            XSSFCellStyle jobDetailAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            jobDetailAlignCellStyle.SetFont(myFont);
            jobDetailAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
            jobDetailAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.None;
            jobDetailAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
            jobDetailAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
            jobDetailAlignCellStyle.WrapText = true;
            jobDetailAlignCellStyle.VerticalAlignment = VerticalAlignment.Center;
            jobDetailAlignCellStyle.Alignment = HorizontalAlignment.Left;


            XSSFFont myFont_Bold = (XSSFFont)templateWorkbook.CreateFont();
            myFont_Bold.FontHeightInPoints = (short)12;
            myFont_Bold.FontName = "Times New Roman";
            myFont_Bold.IsBold = true;

            XSSFCellStyle right_JobDetailAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            right_JobDetailAlignCellStyle.SetFont(myFont);
            right_JobDetailAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.WrapText = true;
            right_JobDetailAlignCellStyle.VerticalAlignment = VerticalAlignment.Center;
            right_JobDetailAlignCellStyle.Alignment = HorizontalAlignment.Right;

            foreach (var jobNumber in jobNumberList)
            {
                ISheet sheet = templateWorkbook.GetSheet("Project #" + jobNumber);

                Job job = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Company").Include("ProjectManager").FirstOrDefault(x => x.JobNumber == jobNumber);

                #region Project Address

                string projectAddress = job != null
                     && job.RfpAddress != null
                     ? job.RfpAddress.HouseNumber + " " + (job.RfpAddress.Street != null ? job.RfpAddress.Street + ", " : string.Empty)
                       + (job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : string.Empty)
                    : string.Empty;
                IRow iprojectAddressRow = sheet.GetRow(2);
                if (iprojectAddressRow == null)
                {
                    iprojectAddressRow = sheet.CreateRow(2);

                }

                if (iprojectAddressRow.GetCell(1) != null)
                {
                    iprojectAddressRow.GetCell(1).SetCellValue(projectAddress);
                }
                else
                {
                    iprojectAddressRow.CreateCell(1).SetCellValue(projectAddress);
                }

                iprojectAddressRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;


                #endregion

                #region Special Place Name
                string specialPlaceName = job != null ? job.SpecialPlace : string.Empty;
                IRow iSpecialPlaceNameRow = sheet.GetRow(3);
                if (iSpecialPlaceNameRow == null)
                {
                    iSpecialPlaceNameRow = sheet.CreateRow(3);
                }

                if (iSpecialPlaceNameRow.GetCell(1) != null)
                {
                    iSpecialPlaceNameRow.GetCell(1).SetCellValue(specialPlaceName);
                }
                else
                {
                    iSpecialPlaceNameRow.CreateCell(1).SetCellValue(specialPlaceName);
                }

                iSpecialPlaceNameRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
                #endregion

                #region Client

                string clientName = job != null && job.Company != null ? job.Company.Name : string.Empty;

                IRow iClientRow = sheet.GetRow(4);
                if (iClientRow == null)
                {
                    iClientRow = sheet.CreateRow(4);
                }

                if (iClientRow.GetCell(1) != null)
                {
                    iClientRow.GetCell(1).SetCellValue(clientName);
                }
                else
                {
                    iClientRow.CreateCell(1).SetCellValue(clientName);
                }

                iClientRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;

                #endregion

                #region Project Manager

                string ProjectManagerName = job.ProjectManager != null ? job.ProjectManager.FirstName + " " + job.ProjectManager.LastName + " | " + job.ProjectManager.Email : string.Empty;
                IRow iProjectManagerRow = sheet.GetRow(5);
                if (iProjectManagerRow == null)
                {
                    iProjectManagerRow = sheet.CreateRow(5);
                }

                if (iProjectManagerRow.GetCell(1) != null)
                {
                    iProjectManagerRow.GetCell(1).SetCellValue(ProjectManagerName);
                }
                else
                {
                    iProjectManagerRow.CreateCell(1).SetCellValue(ProjectManagerName);
                }

                iProjectManagerRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;

                #endregion

                #region Job Number

                string jobNumberValue = "Project #" + jobNumber;
                IRow iJobNumberRow = sheet.GetRow(2);
                if (iJobNumberRow == null)
                {
                    iJobNumberRow = sheet.CreateRow(2);
                }

                if (iJobNumberRow.GetCell(4) != null)
                {
                    iJobNumberRow.GetCell(4).SetCellValue(jobNumberValue);
                }
                else
                {
                    iJobNumberRow.CreateCell(4).SetCellValue(jobNumberValue);
                }

                iJobNumberRow.GetCell(4).CellStyle = right_JobDetailAlignCellStyle;
                #endregion

                #region Report Date

                string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);
                IRow iDateRow = sheet.GetRow(3);
                if (iDateRow == null)
                {
                    iDateRow = sheet.CreateRow(3);
                }

                if (iDateRow.GetCell(4) != null)
                {
                    iDateRow.GetCell(4).SetCellValue(reportDate);
                }
                else
                {
                    iDateRow.CreateCell(4).SetCellValue(reportDate);
                }

                iDateRow.GetCell(4).CellStyle = right_JobDetailAlignCellStyle;

                #endregion

                #region DOB

                int DOBJobType = Enums.ApplicationType.DOB.GetHashCode();
                List<PermitsExpiryDTO> DOBPermitList = result.Where(x => x.JobNumber == jobNumber
                                                                      && x.IdJobType == DOBJobType
                                                                      && x.JobWorkTypeCode != "AHV"
                                                                      && x.JobWorkTypeCode != "TCO").OrderBy(x => x.JobNumber).ThenBy(x => x.Expires).ToList();

                int index = 7;
                if (DOBPermitList != null && DOBPermitList.Count > 0)
                {
                    #region DOB Report Header

                    XSSFFont DOBReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    DOBReportHeaderFont.FontHeightInPoints = (short)14;
                    DOBReportHeaderFont.FontName = "Times New Roman";
                    DOBReportHeaderFont.IsBold = true;

                    XSSFCellStyle DOBReportHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    DOBReportHeaderCellStyle.SetFont(DOBReportHeaderFont);
                    DOBReportHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    DOBReportHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    DOBReportHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    DOBReportHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    DOBReportHeaderCellStyle.WrapText = true;
                    DOBReportHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    DOBReportHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                    DOBReportHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.White);
                    DOBReportHeaderCellStyle.FillPattern = FillPattern.SolidForeground;

                   

                    string DOB_ReportHeader = "DOB PERMIT EXPIRATION";
                    IRow iDOB_ReportHeaderRow = sheet.GetRow(index);
                    if (iDOB_ReportHeaderRow == null)
                    {
                        iDOB_ReportHeaderRow = sheet.CreateRow(index);
                    }
                    iDOB_ReportHeaderRow.HeightInPoints = (float)19.50;

                    if (iDOB_ReportHeaderRow.GetCell(0) != null)
                    {
                        iDOB_ReportHeaderRow.GetCell(0).SetCellValue(DOB_ReportHeader);
                    }
                    else
                    {
                        iDOB_ReportHeaderRow.CreateCell(0).SetCellValue(DOB_ReportHeader);
                    }

                    CellRangeAddress DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 4);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    iDOB_ReportHeaderRow.GetCell(0).CellStyle = DOBReportHeaderCellStyle;

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    index = index + 1;

                    #endregion

                    #region Table Header

                    XSSFFont DOBHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    DOBHeaderFont.FontHeightInPoints = (short)12;
                    DOBHeaderFont.FontName = "Times New Roman";
                    DOBHeaderFont.IsBold = true;

                    XSSFCellStyle DOBHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    DOBHeaderCellStyle.SetFont(DOBHeaderFont);
                    DOBHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    DOBHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    DOBHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    DOBHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    DOBHeaderCellStyle.WrapText = true;
                    DOBHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    DOBHeaderCellStyle.Alignment = HorizontalAlignment.Left;
                    DOBHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                    //  ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(226, 239, 218));
                    DOBHeaderCellStyle.FillPattern = FillPattern.SolidForeground;
                    IRow iDOB_HeaderRow = sheet.GetRow(index);
                    if (iDOB_HeaderRow == null)
                    {
                        iDOB_HeaderRow = sheet.CreateRow(index);
                    }

                    if (iDOB_HeaderRow.GetCell(0) != null)
                    {
                        iDOB_HeaderRow.GetCell(0).SetCellValue("Application #");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(0).SetCellValue("Application #");
                    }

                    if (iDOB_HeaderRow.GetCell(1) != null)
                    {
                        iDOB_HeaderRow.GetCell(1).SetCellValue("App Type");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(1).SetCellValue("App Type");
                    }

                    if (iDOB_HeaderRow.GetCell(2) != null)
                    {
                        iDOB_HeaderRow.GetCell(2).SetCellValue("Permit #");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(2).SetCellValue("Permit #");
                    }

                    if (iDOB_HeaderRow.GetCell(3) != null)
                    {
                        iDOB_HeaderRow.GetCell(3).SetCellValue("Work Type | Permit Type");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(3).SetCellValue("Work Type | Permit Type");
                    }

                    if (iDOB_HeaderRow.GetCell(4) != null)
                    {
                        iDOB_HeaderRow.GetCell(4).SetCellValue("Expiration Date");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(4).SetCellValue("Expiration Date");
                    }

                    iDOB_HeaderRow.GetCell(0).CellStyle = DOBHeaderCellStyle;
                    iDOB_HeaderRow.GetCell(1).CellStyle = DOBHeaderCellStyle;
                    iDOB_HeaderRow.GetCell(2).CellStyle = DOBHeaderCellStyle;
                    iDOB_HeaderRow.GetCell(3).CellStyle = DOBHeaderCellStyle;
                    iDOB_HeaderRow.GetCell(4).CellStyle = DOBHeaderCellStyle;

                    index = index + 1;
                    #endregion

                    foreach (PermitsExpiryDTO item in DOBPermitList)
                    {
                        IRow iRow = sheet.GetRow(index);
                        if (iRow == null)
                        {
                            iRow = sheet.CreateRow(index);
                        }

                        if (iRow.GetCell(0) != null)
                        {
                            iRow.GetCell(0).SetCellValue(item.JobApplicationNumber);
                        }
                        else
                        {
                            iRow.CreateCell(0).SetCellValue(item.JobApplicationNumber);
                        }

                        if (iRow.GetCell(1) != null)
                        {
                            iRow.GetCell(1).SetCellValue(item.JobApplicationTypeName);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.JobApplicationTypeName);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.PermitNumber);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.PermitNumber);
                        }

                        string workDescription = item.JobWorkTypeDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);
                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(workDescription);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(workDescription);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;

                        index = index + 1;
                    }
                }

                #endregion

                #region AHV

                List<PermitsExpiryDTO> AHVPermitList = result.Where(x => x.JobNumber == jobNumber
                                                                      && x.JobWorkTypeCode == "AHV").OrderBy(x => x.JobNumber).ThenBy(x => x.Expires).ToList();
                if (AHVPermitList != null && AHVPermitList.Count > 0)
                {
                    #region AHV Report Header

                    XSSFFont AHVReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    AHVReportHeaderFont.FontHeightInPoints = (short)14;
                    AHVReportHeaderFont.FontName = "Times New Roman";
                    AHVReportHeaderFont.IsBold = true;

                    XSSFCellStyle AHVReportHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    AHVReportHeaderCellStyle.SetFont(AHVReportHeaderFont);
                    AHVReportHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    AHVReportHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    AHVReportHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    AHVReportHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    AHVReportHeaderCellStyle.WrapText = true;
                    AHVReportHeaderCellStyle.VerticalAlignment = VerticalAlignment.Top;
                    AHVReportHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                   // AHVReportHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                  //  new XSSFColor(System.Drawing.Color.Yellow);
                   // AHVReportHeaderCellStyle.FillPattern = FillPattern.SolidForeground;

                    index = index + 1;
                    string AHV_ReportHeader = "AHV PERMIT EXPIRATION";
                    IRow iAHV_ReportHeaderRow = sheet.GetRow(index);
                    if (iAHV_ReportHeaderRow == null)
                    {
                        iAHV_ReportHeaderRow = sheet.CreateRow(index);
                    }

                    iAHV_ReportHeaderRow.HeightInPoints = (float)19.50;
                    if (iAHV_ReportHeaderRow.GetCell(0) != null)
                    {
                        iAHV_ReportHeaderRow.GetCell(0).SetCellValue(AHV_ReportHeader);
                    }
                    else
                    {
                        iAHV_ReportHeaderRow.CreateCell(0).SetCellValue(AHV_ReportHeader);
                    }

                    iAHV_ReportHeaderRow.GetCell(0).CellStyle = AHVReportHeaderCellStyle;

                    CellRangeAddress AHVCellRangeAddress = new NPOI.SS.Util.CellRangeAddress(index, index, 0, 4);
                    sheet.AddMergedRegion(AHVCellRangeAddress);

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), AHVCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), AHVCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), AHVCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), AHVCellRangeAddress, sheet, templateWorkbook);

                    index = index + 1;

                    #endregion

                    #region Table Header

                    XSSFFont AHVHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    AHVHeaderFont.FontHeightInPoints = (short)12;
                    AHVHeaderFont.FontName = "Times New Roman";
                    AHVHeaderFont.IsBold = true;

                    XSSFCellStyle AHVHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    AHVHeaderCellStyle.SetFont(AHVHeaderFont);
                    AHVHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    AHVHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    AHVHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    AHVHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    AHVHeaderCellStyle.WrapText = true;
                    AHVHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    AHVHeaderCellStyle.Alignment = HorizontalAlignment.Left;
                    AHVHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                    //  ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(226, 239, 218));
                    AHVHeaderCellStyle.FillPattern = FillPattern.SolidForeground;


                    IRow iAHV_HeaderRow = sheet.GetRow(index);
                    if (iAHV_HeaderRow == null)
                    {
                        iAHV_HeaderRow = sheet.CreateRow(index);
                    }

                    if (iAHV_HeaderRow.GetCell(0) != null)
                    {
                        iAHV_HeaderRow.GetCell(0).SetCellValue("Application #");
                    }
                    else
                    {
                        iAHV_HeaderRow.CreateCell(0).SetCellValue("Application #");
                    }

                    if (iAHV_HeaderRow.GetCell(1) != null)
                    {
                        iAHV_HeaderRow.GetCell(1).SetCellValue("App Type");
                    }
                    else
                    {
                        iAHV_HeaderRow.CreateCell(1).SetCellValue("App Type");
                    }

                    if (iAHV_HeaderRow.GetCell(2) != null)
                    {
                        iAHV_HeaderRow.GetCell(2).SetCellValue("Permit #");
                    }
                    else
                    {
                        iAHV_HeaderRow.CreateCell(2).SetCellValue("Permit #");
                    }

                    if (iAHV_HeaderRow.GetCell(3) != null)
                    {
                        iAHV_HeaderRow.GetCell(3).SetCellValue("Description of Work");
                    }
                    else
                    {
                        iAHV_HeaderRow.CreateCell(3).SetCellValue("Description of Work");
                    }

                    if (iAHV_HeaderRow.GetCell(4) != null)
                    {
                        iAHV_HeaderRow.GetCell(4).SetCellValue("Expiration Date");
                    }
                    else
                    {
                        iAHV_HeaderRow.CreateCell(4).SetCellValue("Expiration Date");
                    }

                    iAHV_HeaderRow.GetCell(0).CellStyle = AHVHeaderCellStyle;
                    iAHV_HeaderRow.GetCell(1).CellStyle = AHVHeaderCellStyle;
                    iAHV_HeaderRow.GetCell(2).CellStyle = AHVHeaderCellStyle;
                    iAHV_HeaderRow.GetCell(3).CellStyle = AHVHeaderCellStyle;
                    iAHV_HeaderRow.GetCell(4).CellStyle = AHVHeaderCellStyle;

                    index = index + 1;

                    #endregion

                    foreach (PermitsExpiryDTO item in AHVPermitList)
                    {
                        IRow iRow = sheet.GetRow(index);
                        if (iRow == null)
                        {
                            iRow = sheet.CreateRow(index);
                        }

                        if (iRow.GetCell(0) != null)
                        {
                            iRow.GetCell(0).SetCellValue(item.JobApplicationNumber);
                        }
                        else
                        {
                            iRow.CreateCell(0).SetCellValue(item.JobApplicationNumber);
                        }

                        if (iRow.GetCell(1) != null)
                        {
                            iRow.GetCell(1).SetCellValue(item.JobApplicationTypeName);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.JobApplicationTypeName);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.PermitNumber);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.PermitNumber);
                        }

                        string workDescription = item.WorkDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);
                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(workDescription);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(workDescription);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;

                        index = index + 1;
                    }
                }
                #endregion

                #region DOT

                int DOTJobType = Enums.ApplicationType.DOT.GetHashCode();
                List<PermitsExpiryDTO> DOTPermitList = result.Where(x => x.JobNumber == jobNumber
                                                                      && x.IdJobType == DOTJobType
                                                                      && x.JobWorkTypeCode != "AHV"
                                                                      && x.JobWorkTypeCode != "TCO").OrderBy(x => x.JobNumber).ThenBy(x => x.Expires).ToList();

                if (DOTPermitList != null && DOTPermitList.Count > 0)
                {

                    #region DOT Report Header

                    XSSFFont DOTReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    DOTReportHeaderFont.FontHeightInPoints = (short)14;
                    DOTReportHeaderFont.FontName = "Times New Roman";
                    DOTReportHeaderFont.IsBold = true;

                    XSSFCellStyle DOTReportHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    DOTReportHeaderCellStyle.SetFont(DOTReportHeaderFont);
                    DOTReportHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    DOTReportHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    DOTReportHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    DOTReportHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    DOTReportHeaderCellStyle.WrapText = true;
                    DOTReportHeaderCellStyle.VerticalAlignment = VerticalAlignment.Top;
                    DOTReportHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                   // DOTReportHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                    //new XSSFColor(System.Drawing.Color.Yellow);
                  //  DOTReportHeaderCellStyle.FillPattern = FillPattern.SolidForeground;

                    index = index + 1;
                    string DOT_ReportHeader = "DOT PERMIT EXPIRATION";
                    IRow iDOT_ReportHeaderRow = sheet.GetRow(index);
                    if (iDOT_ReportHeaderRow == null)
                    {
                        iDOT_ReportHeaderRow = sheet.CreateRow(index);
                    }

                    iDOT_ReportHeaderRow.HeightInPoints = (float)19.50;

                    if (iDOT_ReportHeaderRow.GetCell(0) != null)
                    {
                        iDOT_ReportHeaderRow.GetCell(0).SetCellValue(DOT_ReportHeader);
                    }
                    else
                    {
                        iDOT_ReportHeaderRow.CreateCell(0).SetCellValue(DOT_ReportHeader);
                    }

                    iDOT_ReportHeaderRow.GetCell(0).CellStyle = DOTReportHeaderCellStyle;

                    CellRangeAddress DOTCellRangeAddress = new NPOI.SS.Util.CellRangeAddress(index, index, 0, 4);
                    sheet.AddMergedRegion(DOTCellRangeAddress);

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOTCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOTCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOTCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOTCellRangeAddress, sheet, templateWorkbook);

                    index = index + 1;

                    #endregion

                    #region Table Header

                    XSSFFont DOTHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    DOTHeaderFont.FontHeightInPoints = (short)12;
                    DOTHeaderFont.FontName = "Times New Roman";
                    DOTHeaderFont.IsBold = true;

                    XSSFCellStyle DOTHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    DOTHeaderCellStyle.SetFont(DOTHeaderFont);
                    DOTHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    DOTHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    DOTHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    DOTHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    DOTHeaderCellStyle.WrapText = true;
                    DOTHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    DOTHeaderCellStyle.Alignment = HorizontalAlignment.Left;
                    DOTHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                    //  ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(226, 239, 218));
                    DOTHeaderCellStyle.FillPattern = FillPattern.SolidForeground;

                    IRow iDOT_HeaderRow = sheet.GetRow(index);
                    if (iDOT_HeaderRow == null)
                    {
                        iDOT_HeaderRow = sheet.CreateRow(index);
                    }

                    if (iDOT_HeaderRow.GetCell(0) != null)
                    {
                        iDOT_HeaderRow.GetCell(0).SetCellValue("Permit #");
                    }
                    else
                    {
                        iDOT_HeaderRow.CreateCell(0).SetCellValue("Permit #");
                    }

                    if (iDOT_HeaderRow.GetCell(1) != null)
                    {
                        iDOT_HeaderRow.GetCell(1).SetCellValue("Street On/From/To");
                    }
                    else
                    {
                        iDOT_HeaderRow.CreateCell(1).SetCellValue("Street On/From/To");
                    }

                    if (iDOT_HeaderRow.GetCell(2) != null)
                    {
                        iDOT_HeaderRow.GetCell(2).SetCellValue("");
                    }
                    else
                    {
                        iDOT_HeaderRow.CreateCell(2).SetCellValue("");
                    }

                    if (iDOT_HeaderRow.GetCell(3) != null)
                    {
                        iDOT_HeaderRow.GetCell(3).SetCellValue("Work Type | Permit Type | Equipment Type");
                    }
                    else
                    {
                        iDOT_HeaderRow.CreateCell(3).SetCellValue("Work Type | Permit Type | Equipment Type");
                    }

                    if (iDOT_HeaderRow.GetCell(4) != null)
                    {
                        iDOT_HeaderRow.GetCell(4).SetCellValue("Expiration Date");
                    }
                    else
                    {
                        iDOT_HeaderRow.CreateCell(4).SetCellValue("Expiration Date");
                    }

                    iDOT_HeaderRow.GetCell(0).CellStyle = DOTHeaderCellStyle;
                    iDOT_HeaderRow.GetCell(1).CellStyle = DOTHeaderCellStyle;
                    iDOT_HeaderRow.GetCell(2).CellStyle = DOTHeaderCellStyle;
                    iDOT_HeaderRow.GetCell(3).CellStyle = DOTHeaderCellStyle;
                    iDOT_HeaderRow.GetCell(4).CellStyle = DOTHeaderCellStyle;

                    index = index + 1;

                    #endregion

                    foreach (PermitsExpiryDTO item in DOTPermitList)
                    {
                        IRow iRow = sheet.GetRow(index);
                        if (iRow == null)
                        {
                            iRow = sheet.CreateRow(index);
                        }

                        if (iRow.GetCell(0) != null)
                        {
                            iRow.GetCell(0).SetCellValue(item.PermitNumber);
                        }
                        else
                        {
                            iRow.CreateCell(0).SetCellValue(item.PermitNumber);
                        }

                        if (iRow.GetCell(1) != null)
                        {
                            iRow.GetCell(1).SetCellValue(item.JobApplicationStreetWorkingOn + " | " + item.JobApplicationStreetFrom + " | " + item.JobApplicationStreetTo);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.JobApplicationStreetWorkingOn + " | " + item.JobApplicationStreetFrom + " | " + item.JobApplicationStreetTo);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(string.Empty);
                        }

                        string workDescription = item.PermitType + (!string.IsNullOrEmpty(item.EquipmentType) ? " | " + item.EquipmentType : string.Empty);
                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(workDescription);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(workDescription);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;

                        index = index + 1;
                    }
                }
                #endregion

                #region DEP

                int DEPJobType = Enums.ApplicationType.DEP.GetHashCode();
                List<PermitsExpiryDTO> DEPPermitList = result.Where(x => x.JobNumber == jobNumber
                                                                      && x.IdJobType == DEPJobType
                                                                      && x.JobWorkTypeCode != "AHV"
                                                                      && x.JobWorkTypeCode != "TCO").OrderBy(x => x.JobNumber).ThenBy(x => x.Expires).ToList();

                if (DEPPermitList != null && DEPPermitList.Count > 0)
                {

                    #region DEP Report Header

                    XSSFFont DEPReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    DEPReportHeaderFont.FontHeightInPoints = (short)14;
                    DEPReportHeaderFont.FontName = "Times New Roman";
                    DEPReportHeaderFont.IsBold = true;

                    XSSFCellStyle DEPReportHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    DEPReportHeaderCellStyle.SetFont(DEPReportHeaderFont);
                    DEPReportHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    DEPReportHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    DEPReportHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    DEPReportHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    DEPReportHeaderCellStyle.WrapText = true;
                    DEPReportHeaderCellStyle.VerticalAlignment = VerticalAlignment.Top;
                    DEPReportHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                   // DEPReportHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                    /* new XSSFColor(System.Drawing.Color.Yellow);*/
                   // DEPReportHeaderCellStyle.FillPattern = FillPattern.SolidForeground;

                    index = index + 1;
                    string DEP_ReportHeader = "DEP PERMIT EXPIRATION";
                    IRow iDEP_ReportHeaderRow = sheet.GetRow(index);
                    if (iDEP_ReportHeaderRow == null)
                    {
                        iDEP_ReportHeaderRow = sheet.CreateRow(index);
                    }

                    iDEP_ReportHeaderRow.HeightInPoints = (float)19.50;

                    if (iDEP_ReportHeaderRow.GetCell(0) != null)
                    {
                        iDEP_ReportHeaderRow.GetCell(0).SetCellValue(DEP_ReportHeader);
                    }
                    else
                    {
                        iDEP_ReportHeaderRow.CreateCell(0).SetCellValue(DEP_ReportHeader);
                    }

                    iDEP_ReportHeaderRow.GetCell(0).CellStyle = DEPReportHeaderCellStyle;

                    CellRangeAddress DEPCellRangeAddress = new NPOI.SS.Util.CellRangeAddress(index, index, 0, 4);
                    sheet.AddMergedRegion(DEPCellRangeAddress);

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DEPCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DEPCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DEPCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DEPCellRangeAddress, sheet, templateWorkbook);

                    index = index + 1;

                    #endregion

                    #region Table Header

                    XSSFFont DEPHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    DEPHeaderFont.FontHeightInPoints = (short)12;
                    DEPHeaderFont.FontName = "Times New Roman";
                    DEPHeaderFont.IsBold = true;

                    XSSFCellStyle DEPHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    DEPHeaderCellStyle.SetFont(DEPHeaderFont);
                    DEPHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    DEPHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    DEPHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    DEPHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    DEPHeaderCellStyle.WrapText = true;
                    DEPHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    DEPHeaderCellStyle.Alignment = HorizontalAlignment.Left;
                    DEPHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                    //  DEPHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(226, 239, 218));
                    DEPHeaderCellStyle.FillPattern = FillPattern.SolidForeground;
                    IRow iDEP_HeaderRow = sheet.GetRow(index);
                    if (iDEP_HeaderRow == null)
                    {
                        iDEP_HeaderRow = sheet.CreateRow(index);
                    }

                    if (iDEP_HeaderRow.GetCell(0) != null)
                    {
                        iDEP_HeaderRow.GetCell(0).SetCellValue("Application Type");
                    }
                    else
                    {
                        iDEP_HeaderRow.CreateCell(0).SetCellValue("Application Type");
                    }

                    if (iDEP_HeaderRow.GetCell(1) != null)
                    {
                        iDEP_HeaderRow.GetCell(1).SetCellValue("Street On/From/To");
                    }
                    else
                    {
                        iDEP_HeaderRow.CreateCell(1).SetCellValue("Street On/From/To");
                    }

                    if (iDEP_HeaderRow.GetCell(2) != null)
                    {
                        iDEP_HeaderRow.GetCell(2).SetCellValue("Permit #");
                    }
                    else
                    {
                        iDEP_HeaderRow.CreateCell(2).SetCellValue("Permit #");
                    }

                    if (iDEP_HeaderRow.GetCell(3) != null)
                    {
                        iDEP_HeaderRow.GetCell(3).SetCellValue("Work Type | Permit Type");
                    }
                    else
                    {
                        iDEP_HeaderRow.CreateCell(3).SetCellValue("Work Type | Permit Type");
                    }

                    if (iDEP_HeaderRow.GetCell(4) != null)
                    {
                        iDEP_HeaderRow.GetCell(4).SetCellValue("Expiration Date");
                    }
                    else
                    {
                        iDEP_HeaderRow.CreateCell(4).SetCellValue("Expiration Date");
                    }

                    iDEP_HeaderRow.GetCell(0).CellStyle = DEPHeaderCellStyle;
                    iDEP_HeaderRow.GetCell(1).CellStyle = DEPHeaderCellStyle;
                    iDEP_HeaderRow.GetCell(2).CellStyle = DEPHeaderCellStyle;
                    iDEP_HeaderRow.GetCell(3).CellStyle = DEPHeaderCellStyle;
                    iDEP_HeaderRow.GetCell(4).CellStyle = DEPHeaderCellStyle;

                    index = index + 1;

                    #endregion

                    foreach (PermitsExpiryDTO item in DEPPermitList)
                    {
                        IRow iRow = sheet.GetRow(index);
                        if (iRow == null)
                        {
                            iRow = sheet.CreateRow(index);
                        }

                        if (iRow.GetCell(0) != null)
                        {
                            iRow.GetCell(0).SetCellValue(item.JobApplicationTypeName);
                        }
                        else
                        {
                            iRow.CreateCell(0).SetCellValue(item.JobApplicationTypeName);
                        }

                        if (iRow.GetCell(1) != null)
                        {
                            iRow.GetCell(1).SetCellValue(item.JobApplicationStreetWorkingOn + " | " + item.JobApplicationStreetFrom + " | " + item.JobApplicationStreetTo);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.JobApplicationStreetWorkingOn + " | " + item.JobApplicationStreetFrom + " | " + item.JobApplicationStreetTo);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.PermitNumber);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.PermitNumber);
                        }

                        string workDescription = item.JobWorkTypeDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(workDescription);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(workDescription);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;

                        index = index + 1;
                    }
                }
                #endregion

                #region VIOLATION

                List<JobViolation> JobViolationList = rpoContext.JobViolations.Where(x => x.Job.JobNumber == jobNumber
                                                                      && x.IsFullyResolved == false).OrderBy(x => x.Job.JobNumber).ThenBy(x => x.HearingDate).ToList();
                if (JobViolationList != null && JobViolationList.Count > 0)
                {
                    #region VIOLATION Report Header

                    XSSFFont VIOLATIONReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    VIOLATIONReportHeaderFont.FontHeightInPoints = (short)14;
                    VIOLATIONReportHeaderFont.FontName = "Times New Roman";
                    VIOLATIONReportHeaderFont.IsBold = true;

                    XSSFCellStyle VIOLATIONReportHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    VIOLATIONReportHeaderCellStyle.SetFont(VIOLATIONReportHeaderFont);
                    VIOLATIONReportHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    VIOLATIONReportHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    VIOLATIONReportHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    VIOLATIONReportHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    VIOLATIONReportHeaderCellStyle.WrapText = true;
                    VIOLATIONReportHeaderCellStyle.VerticalAlignment = VerticalAlignment.Top;
                    VIOLATIONReportHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                  //  VIOLATIONReportHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                    //new XSSFColor(System.Drawing.Color.SkyBlue);
                  // VIOLATIONReportHeaderCellStyle.FillPattern = FillPattern.SolidForeground;

                    index = index + 1;
                    string VIOLATION_ReportHeader = "PENDING VIOLATION";
                    IRow iVIOLATION_ReportHeaderRow = sheet.GetRow(index);
                    if (iVIOLATION_ReportHeaderRow == null)
                    {
                        iVIOLATION_ReportHeaderRow = sheet.CreateRow(index);
                    }

                    iVIOLATION_ReportHeaderRow.HeightInPoints = (float)19.50;

                    if (iVIOLATION_ReportHeaderRow.GetCell(0) != null)
                    {
                        iVIOLATION_ReportHeaderRow.GetCell(0).SetCellValue(VIOLATION_ReportHeader);
                    }
                    else
                    {
                        iVIOLATION_ReportHeaderRow.CreateCell(0).SetCellValue(VIOLATION_ReportHeader);
                    }

                    iVIOLATION_ReportHeaderRow.GetCell(0).CellStyle = VIOLATIONReportHeaderCellStyle;

                    CellRangeAddress VIOLATIONCellRangeAddress = new NPOI.SS.Util.CellRangeAddress(index, index, 0, 4);
                    sheet.AddMergedRegion(VIOLATIONCellRangeAddress);

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), VIOLATIONCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), VIOLATIONCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), VIOLATIONCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), VIOLATIONCellRangeAddress, sheet, templateWorkbook);

                    index = index + 1;

                    #endregion

                    #region Table Header

                    XSSFFont VIOLATIONHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    VIOLATIONHeaderFont.FontHeightInPoints = (short)12;
                    VIOLATIONHeaderFont.FontName = "Times New Roman";
                    VIOLATIONHeaderFont.IsBold = true;

                    XSSFCellStyle VIOLATIONHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    VIOLATIONHeaderCellStyle.SetFont(VIOLATIONHeaderFont);
                    VIOLATIONHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    VIOLATIONHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    VIOLATIONHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    VIOLATIONHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    VIOLATIONHeaderCellStyle.WrapText = true;
                    VIOLATIONHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    VIOLATIONHeaderCellStyle.Alignment = HorizontalAlignment.Left;
                    VIOLATIONHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                    //  ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(226, 239, 218));
                    VIOLATIONHeaderCellStyle.FillPattern = FillPattern.SolidForeground;

                    IRow iVIOLATION_HeaderRow = sheet.GetRow(index);
                    if (iVIOLATION_HeaderRow == null)
                    {
                        iVIOLATION_HeaderRow = sheet.CreateRow(index);
                    }

                    if (iVIOLATION_HeaderRow.GetCell(0) != null)
                    {
                        iVIOLATION_HeaderRow.GetCell(0).SetCellValue("Violation #");
                    }
                    else
                    {
                        iVIOLATION_HeaderRow.CreateCell(0).SetCellValue("Violation #");
                    }

                    if (iVIOLATION_HeaderRow.GetCell(1) != null)
                    {
                        iVIOLATION_HeaderRow.GetCell(1).SetCellValue("Issuing Agency");
                    }
                    else
                    {
                        iVIOLATION_HeaderRow.CreateCell(1).SetCellValue("Issuing Agency");
                    }

                    if (iVIOLATION_HeaderRow.GetCell(2) != null)
                    {
                        iVIOLATION_HeaderRow.GetCell(2).SetCellValue("Date Issued");
                    }
                    else
                    {
                        iVIOLATION_HeaderRow.CreateCell(2).SetCellValue("Date Issued");
                    }

                    if (iVIOLATION_HeaderRow.GetCell(3) != null)
                    {
                        iVIOLATION_HeaderRow.GetCell(3).SetCellValue("Hearing Date");
                    }
                    else
                    {
                        iVIOLATION_HeaderRow.CreateCell(3).SetCellValue("Hearing Date");
                    }

                    if (iVIOLATION_HeaderRow.GetCell(4) != null)
                    {
                        iVIOLATION_HeaderRow.GetCell(4).SetCellValue("Status");
                    }
                    else
                    {
                        iVIOLATION_HeaderRow.CreateCell(4).SetCellValue("Status");
                    }

                    iVIOLATION_HeaderRow.GetCell(0).CellStyle = VIOLATIONHeaderCellStyle;
                    iVIOLATION_HeaderRow.GetCell(1).CellStyle = VIOLATIONHeaderCellStyle;
                    iVIOLATION_HeaderRow.GetCell(2).CellStyle = VIOLATIONHeaderCellStyle;
                    iVIOLATION_HeaderRow.GetCell(3).CellStyle = VIOLATIONHeaderCellStyle;
                    iVIOLATION_HeaderRow.GetCell(4).CellStyle = VIOLATIONHeaderCellStyle;

                    index = index + 1;

                    #endregion

                    foreach (JobViolation item in JobViolationList)
                    {
                        IRow iRow = sheet.GetRow(index);
                        if (iRow == null)
                        {
                            iRow = sheet.CreateRow(index);
                        }

                        if (iRow.GetCell(0) != null)
                        {
                            iRow.GetCell(0).SetCellValue(item.SummonsNumber);
                        }
                        else
                        {
                            iRow.CreateCell(0).SetCellValue(item.SummonsNumber);
                        }

                        if (iRow.GetCell(1) != null)
                        {
                            iRow.GetCell(1).SetCellValue(item.IssuingAgency);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.IssuingAgency);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(item.HearingDate != null ? Convert.ToDateTime(item.HearingDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(item.HearingDate != null ? Convert.ToDateTime(item.HearingDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.StatusOfSummonsNotice);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.StatusOfSummonsNotice);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;

                        index = index + 1;
                    }
                }
                #endregion

                #region TCO

                List<PermitsExpiryDTO> TCOPermitList = result.Where(x => x.JobNumber == jobNumber
                                                                      && x.JobWorkTypeCode == "TCO").OrderBy(x => x.JobNumber).ThenBy(x => x.Expires).ToList();

                if (TCOPermitList != null && TCOPermitList.Count > 0)
                {
                    #region TCO Report Header

                    XSSFFont TCOReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    TCOReportHeaderFont.FontHeightInPoints = (short)14;
                    TCOReportHeaderFont.FontName = "Times New Roman";
                    TCOReportHeaderFont.IsBold = true;

                    XSSFCellStyle TCOReportHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    TCOReportHeaderCellStyle.SetFont(TCOReportHeaderFont);
                    TCOReportHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    TCOReportHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    TCOReportHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    TCOReportHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    TCOReportHeaderCellStyle.WrapText = true;
                    TCOReportHeaderCellStyle.VerticalAlignment = VerticalAlignment.Top;
                    TCOReportHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                    TCOReportHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                    //new XSSFColor(System.Drawing.Color.GreenYellow);
                    //TCOReportHeaderCellStyle.FillForegroundColor = IndexedColors.GreenYellow.Index;
                    //TCOReportHeaderCellStyle.FillPattern = FillPattern.SolidForeground;

                    index = index + 1;
                    string TCO_ReportHeader = "TCO PERMIT EXPIRATION";
                    IRow iTCO_ReportHeaderRow = sheet.GetRow(index);
                    if (iTCO_ReportHeaderRow == null)
                    {
                        iTCO_ReportHeaderRow = sheet.CreateRow(index);
                    }

                    iTCO_ReportHeaderRow.HeightInPoints = (float)19.50;

                    if (iTCO_ReportHeaderRow.GetCell(0) != null)
                    {
                        iTCO_ReportHeaderRow.GetCell(0).SetCellValue(TCO_ReportHeader);
                    }
                    else
                    {
                        iTCO_ReportHeaderRow.CreateCell(0).SetCellValue(TCO_ReportHeader);
                    }

                    iTCO_ReportHeaderRow.GetCell(0).CellStyle = TCOReportHeaderCellStyle;

                    CellRangeAddress TCOCellRangeAddress = new NPOI.SS.Util.CellRangeAddress(index, index, 0, 4);
                    sheet.AddMergedRegion(TCOCellRangeAddress);

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), TCOCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), TCOCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), TCOCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), TCOCellRangeAddress, sheet, templateWorkbook);

                    index = index + 1;

                    #endregion

                    #region Table Header

                    XSSFFont TCOHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    TCOHeaderFont.FontHeightInPoints = (short)12;
                    TCOHeaderFont.FontName = "Times New Roman";
                    TCOHeaderFont.IsBold = true;

                    XSSFCellStyle TCOHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    TCOHeaderCellStyle.SetFont(TCOHeaderFont);
                    TCOHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    TCOHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    TCOHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    TCOHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    TCOHeaderCellStyle.WrapText = true;
                    TCOHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    TCOHeaderCellStyle.Alignment = HorizontalAlignment.Left;
                    TCOHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                    TCOHeaderCellStyle.FillPattern = FillPattern.SolidForeground;
                    IRow iTCO_HeaderRow = sheet.GetRow(index);
                    if (iTCO_HeaderRow == null)
                    {
                        iTCO_HeaderRow = sheet.CreateRow(index);
                    }

                    if (iTCO_HeaderRow.GetCell(0) != null)
                    {
                        iTCO_HeaderRow.GetCell(0).SetCellValue("Application #");
                    }
                    else
                    {
                        iTCO_HeaderRow.CreateCell(0).SetCellValue("Application #");
                    }

                    if (iTCO_HeaderRow.GetCell(1) != null)
                    {
                        iTCO_HeaderRow.GetCell(1).SetCellValue("Application Type");
                    }
                    else
                    {
                        iTCO_HeaderRow.CreateCell(1).SetCellValue("Application Type");
                    }

                    if (iTCO_HeaderRow.GetCell(2) != null)
                    {
                        iTCO_HeaderRow.GetCell(2).SetCellValue("");
                    }
                    else
                    {
                        iTCO_HeaderRow.CreateCell(2).SetCellValue("");
                    }

                    if (iTCO_HeaderRow.GetCell(3) != null)
                    {
                        iTCO_HeaderRow.GetCell(3).SetCellValue("");
                    }
                    else
                    {
                        iTCO_HeaderRow.CreateCell(3).SetCellValue("");
                    }

                    if (iTCO_HeaderRow.GetCell(4) != null)
                    {
                        iTCO_HeaderRow.GetCell(4).SetCellValue("Expiration Date");
                    }
                    else
                    {
                        iTCO_HeaderRow.CreateCell(4).SetCellValue("Expiration Date");
                    }

                    iTCO_HeaderRow.GetCell(0).CellStyle = TCOHeaderCellStyle;
                    iTCO_HeaderRow.GetCell(1).CellStyle = TCOHeaderCellStyle;
                    iTCO_HeaderRow.GetCell(2).CellStyle = TCOHeaderCellStyle;
                    iTCO_HeaderRow.GetCell(3).CellStyle = TCOHeaderCellStyle;
                    iTCO_HeaderRow.GetCell(4).CellStyle = TCOHeaderCellStyle;

                    index = index + 1;

                    #endregion

                    foreach (PermitsExpiryDTO item in TCOPermitList)
                    {
                        IRow iRow = sheet.GetRow(index);
                        if (iRow == null)
                        {
                            iRow = sheet.CreateRow(index);
                        }

                        if (iRow.GetCell(0) != null)
                        {
                            iRow.GetCell(0).SetCellValue(item.JobApplicationNumber);
                        }
                        else
                        {
                            iRow.CreateCell(0).SetCellValue(item.JobApplicationNumber);
                        }

                        if (iRow.GetCell(1) != null)
                        {
                            iRow.GetCell(1).SetCellValue(item.JobApplicationTypeName);
                        }
                        else
                        {
                            iRow.CreateCell(1).SetCellValue(item.JobApplicationTypeName);
                        }

                        if (iRow.GetCell(2) != null)
                        {
                            iRow.GetCell(2).SetCellValue(string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(2).SetCellValue(string.Empty);
                        }

                        if (iRow.GetCell(3) != null)
                        {
                            iRow.GetCell(3).SetCellValue(string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(3).SetCellValue(string.Empty);
                        }

                        if (iRow.GetCell(4) != null)
                        {
                            iRow.GetCell(4).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }
                        else
                        {
                            iRow.CreateCell(4).SetCellValue(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty);
                        }

                        iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iRow.GetCell(4).CellStyle = leftAlignCellStyle;

                        index = index + 1;
                    }
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
        /// Get the report of ReportOverallPermitExpiry Report with filter and sorting  and export to pdf
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportOverallPermitExpiry/exporttopdf")]
        public IHttpActionResult PostExportOverallPermitExpiryToPdf(OverallPermitDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                #region Filter

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                DateTime ExpiresFromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                dataTableParameters.ExpiresFromDate = TimeZoneInfo.ConvertTimeToUtc(ExpiresFromDate, TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                dataTableParameters.ExpiresToDate = Convert.ToDateTime(dataTableParameters.ExpiresFromDate).AddDays(30);

                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                    .Include("JobApplication.JobApplicationType")
                                                    .Include("JobApplication.ApplicationStatus")
                                                    .Include("JobApplication.Job.RfpAddress.Borough")
                                                    .Include("JobApplication.Job.Company")
                                                    .Include("JobApplication.Job.Contact")
                                                    .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                    .Where(x => x.Expires != null
                                                               //((DbFunctions.TruncateTime(x.Expires) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
                                                               //&& DbFunctions.TruncateTime(x.Expires) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
                                                               //)
                                                               )
                                                    .AsQueryable();

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.JobApplication.IdJob));
                }

                List<PermitsExpiryDTO> result = jobApplicationWorkPermitTypes
                    .AsEnumerable()
                    .Select(c => this.FormatOverallPermitExpiryReport(c))
                    .AsQueryable().OrderBy(x => x.JobNumber).ThenBy(x => x.Expires)
                    .ToList();

                #endregion

                if (result != null && result.Count > 0)
                {
                    string exportFilename = string.Empty;
                    string exportFilePath = string.Empty;

                    exportFilename = "ConsolidatedStatusReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf(result, exportFilename);


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
        [Route("api/ReportOverallPermitExpiry/exporttoexcelemail")]
        public IHttpActionResult PostExportOverallPermitExpiryToExcelEmail(OverallPermitDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                #region Filter

                DateTime ExpiresFromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                dataTableParameters.ExpiresFromDate = TimeZoneInfo.ConvertTimeToUtc(ExpiresFromDate, TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                dataTableParameters.ExpiresToDate = Convert.ToDateTime(dataTableParameters.ExpiresFromDate).AddDays(30);

                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                    .Include("JobApplication.JobApplicationType")
                                                    .Include("JobApplication.ApplicationStatus")
                                                    .Include("JobApplication.Job.RfpAddress.Borough")
                                                    .Include("JobApplication.Job.Company")
                                                    .Include("JobApplication.Job.Contact")
                                                    .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                    .Where(x => x.Expires != null
                                                               //((DbFunctions.TruncateTime(x.Expires) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
                                                               //&& DbFunctions.TruncateTime(x.Expires) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
                                                               //)
                                                               )
                                                    .AsQueryable();

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.JobApplication.IdJob));
                }


                List<PermitsExpiryDTO> result = jobApplicationWorkPermitTypes
                    .AsEnumerable()
                    .Select(c => this.FormatOverallPermitExpiryReport(c))
                    .AsQueryable().OrderBy(x => x.JobNumber).ThenBy(x => x.Expires)
                    .ToList();

                #endregion

                if (result != null && result.Count > 0)
                {
                    string exportFilename = string.Empty;
                    exportFilename = "ConsolidatedStatusReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                    string exportFilePath = ExportToExcel(result, exportFilename);

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
        [Route("api/ReportOverallPermitExpiry/exporttopdfemail")]
        public IHttpActionResult PostExportOverallPermitExpiryToPdfEmail(OverallPermitDataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                #region Filter

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                DateTime ExpiresFromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                dataTableParameters.ExpiresFromDate = TimeZoneInfo.ConvertTimeToUtc(ExpiresFromDate, TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                dataTableParameters.ExpiresToDate = Convert.ToDateTime(dataTableParameters.ExpiresFromDate).AddDays(30);

                var jobApplicationWorkPermitTypes = this.rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType")
                                                    .Include("JobApplication.JobApplicationType")
                                                    .Include("JobApplication.ApplicationStatus")
                                                    .Include("JobApplication.Job.RfpAddress.Borough")
                                                    .Include("JobApplication.Job.Company")
                                                    .Include("JobApplication.Job.Contact")
                                                    .Include("CreatedByEmployee").Include("LastModifiedByEmployee")
                                                    .Where(x => x.Expires != null
                                                               //((DbFunctions.TruncateTime(x.Expires) >= DbFunctions.TruncateTime(dataTableParameters.ExpiresFromDate)
                                                               //&& DbFunctions.TruncateTime(x.Expires) <= DbFunctions.TruncateTime(dataTableParameters.ExpiresToDate))
                                                               //)
                                                               )
                                                    .AsQueryable();

                if (!string.IsNullOrEmpty(dataTableParameters.IdJob))
                {
                    List<int> joNumbers = dataTableParameters.IdJob != null && !string.IsNullOrEmpty(dataTableParameters.IdJob) ? (dataTableParameters.IdJob.Split('-') != null && dataTableParameters.IdJob.Split('-').Any() ? dataTableParameters.IdJob.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                    jobApplicationWorkPermitTypes = jobApplicationWorkPermitTypes.Where(x => joNumbers.Contains(x.JobApplication.IdJob));
                }

                List<PermitsExpiryDTO> result = jobApplicationWorkPermitTypes
                    .AsEnumerable()
                    .Select(c => this.FormatOverallPermitExpiryReport(c))
                    .AsQueryable().OrderBy(x => x.JobNumber).ThenBy(x => x.Expires)
                    .ToList();

                #endregion

                if (result != null && result.Count > 0)
                {
                    string exportFilename = string.Empty;
                    string exportFilePath = string.Empty;

                    exportFilename = "ConsolidatedStatusReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    exportFilePath = ExportToPdf(result, exportFilename);

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


        private string ExportToPdf(List<PermitsExpiryDTO> result, string exportFilename)
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

                List<string> jobNumberList = result.Select(x => x.JobNumber).Distinct().ToList();

                if (jobNumberList != null)
                {
                    jobNumberList.AddRange(rpoContext.JobViolations.Where(x => x.IsFullyResolved == false && jobNumberList.Contains(x.IdJob.ToString())).Select(x => x.Job.JobNumber).ToList());
                }
                else
                {
                    jobNumberList = rpoContext.JobViolations.Where(x => x.IsFullyResolved == false && jobNumberList.Contains(x.IdJob.ToString())).Select(x => x.Job.JobNumber).ToList();
                }

                jobNumberList = jobNumberList.Select(x => x).OrderBy(x => x).Distinct().ToList();

                int jobIndex = 0;
                foreach (var jobNumber in jobNumberList)
                {
                    if (jobIndex != 0)
                    {
                        document.NewPage();
                    }

                    jobIndex = jobIndex + 1;

                    PdfPTable table = new PdfPTable(5);
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
                    //cell.Colspan = 4;
                    //cell.PaddingLeft = -60;
                    //table.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(reportHeader, font_11_Normal));
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.Border = PdfPCell.NO_BORDER;
                    //cell.Colspan = 4;
                    //cell.PaddingLeft = -60;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //table.AddCell(cell);

                    //cell = new PdfPCell(SnapCorLogo);
                    //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //cell.Border = PdfPCell.NO_BORDER;
                    //cell.Colspan = 5;
                    //cell.PaddingTop = -60;
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
                    cell.PaddingLeft = -90;
                    cell.Colspan = 5;
                    cell.PaddingTop = -5;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.PaddingLeft = -90;
                    cell.Colspan = 5;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.PaddingBottom = -10;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("CONSOLIDATED REPORT OF RPO PROJECT RESPONSIBILITIES", font_16_Bold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = PdfPCell.TOP_BORDER;
                    cell.Colspan = 5;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    #region Job Header
                    Job job = rpoContext.Jobs.Include("RfpAddress.Borough").FirstOrDefault(x => x.JobNumber == jobNumber);

                    cell = new PdfPCell(new Phrase("Project Address:", font_10_Bold));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.TOP_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    string projectAddress = job != null
                     && job.RfpAddress != null
                     ? job.RfpAddress.HouseNumber + " " + (job.RfpAddress.Street != null ? job.RfpAddress.Street + ", " : string.Empty)
                       + (job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : string.Empty)
                    : string.Empty;
                    cell = new PdfPCell(new Phrase(projectAddress, font_10_Normal));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.TOP_BORDER;
                    cell.Colspan = 2;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.TOP_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    string jobNumberValue = "Project #" + jobNumber;
                    cell = new PdfPCell(new Phrase(jobNumberValue, font_10_Bold));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = PdfPCell.TOP_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Special Place Name:", font_10_Bold));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    string specialPlaceName = job != null ? job.SpecialPlace : string.Empty;
                    cell = new PdfPCell(new Phrase(specialPlaceName, font_10_Normal));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Colspan = 2;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);
                    cell = new PdfPCell(new Phrase(reportDate, font_10_Bold));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Client:", font_10_Bold));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    string clientName = job != null && job.Company != null ? job.Company.Name.Trim() : string.Empty;
                    cell = new PdfPCell(new Phrase(clientName, font_10_Normal));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Colspan = 3;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Project Manager:", font_10_Bold));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    string ProjectManagerName = job.ProjectManager != null ? job.ProjectManager.FirstName + " " + job.ProjectManager.LastName + " | " + job.ProjectManager.Email : string.Empty;
                    cell = new PdfPCell(new Phrase(ProjectManagerName, font_10_Normal));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Colspan = 3;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Colspan = 5;
                    table.AddCell(cell);
                    #endregion

                    #region DOB
                    int DOBJobType = Enums.ApplicationType.DOB.GetHashCode();
                    List<PermitsExpiryDTO> DOBPermitList = result.Where(x => x.JobNumber == jobNumber
                                                                          && x.IdJobType == DOBJobType
                                                                          && x.JobWorkTypeCode != "AHV"
                                                                          && x.JobWorkTypeCode != "TCO").OrderBy(x => x.JobNumber).ThenBy(x => x.Expires).ToList();
                    if (DOBPermitList != null && DOBPermitList.Count > 0)
                    {
                        cell = new PdfPCell(new Phrase("DOB PERMIT EXPIRATION", font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 5;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Application #", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("App Type", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Permit #", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Work Type | Permit Type", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        table.AddCell(cell);

                        foreach (PermitsExpiryDTO item in DOBPermitList)
                        {
                            cell = new PdfPCell(new Phrase(item.JobApplicationNumber, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.JobApplicationTypeName, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.PermitNumber, font_Table_Data));
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

                            cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);
                        }

                        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.NO_BORDER;
                        cell.Colspan = 5;
                        table.AddCell(cell);
                    }
                    #endregion

                    #region AHV
                    List<PermitsExpiryDTO> AHVPermitList = result.Where(x => x.JobNumber == jobNumber
                                                                          && x.JobWorkTypeCode == "AHV").OrderBy(x => x.JobNumber).ThenBy(x => x.Expires).ToList();

                    if (AHVPermitList != null && AHVPermitList.Count > 0)
                    {
                        cell = new PdfPCell(new Phrase("AHV PERMIT EXPIRATION", font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 5;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Application #", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("App Type", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Permit #", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Description of Work", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        table.AddCell(cell);



                        foreach (PermitsExpiryDTO item in AHVPermitList)
                        {
                            cell = new PdfPCell(new Phrase(item.JobWorkTypeNumber, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.JobApplicationTypeName, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.PermitNumber, font_Table_Data));
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

                            cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);
                        }

                        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.NO_BORDER;
                        cell.Colspan = 5;
                        table.AddCell(cell);
                    }

                    #endregion

                    #region DOT
                    int DOTJobType = Enums.ApplicationType.DOT.GetHashCode();
                    List<PermitsExpiryDTO> DOTPermitList = result.Where(x => x.JobNumber == jobNumber
                                                                          && x.IdJobType == DOTJobType
                                                                          && x.JobWorkTypeCode != "AHV"
                                                                          && x.JobWorkTypeCode != "TCO").OrderBy(x => x.JobNumber).ThenBy(x => x.Expires).ToList();

                    if (DOTPermitList != null && DOTPermitList.Count > 0)
                    {
                        cell = new PdfPCell(new Phrase("DOT PERMIT EXPIRATION", font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 5;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Permit #", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Street On/From/To", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Work Type | Permit Type | Equipment Type", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        table.AddCell(cell);

                        foreach (PermitsExpiryDTO item in DOTPermitList)
                        {
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

                            cell = new PdfPCell(new Phrase("", font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            string workDescription = item.PermitType + (!string.IsNullOrEmpty(item.EquipmentType) ? " | " + item.EquipmentType : string.Empty);

                            cell = new PdfPCell(new Phrase(workDescription, font_Table_Data));
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

                        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.NO_BORDER;
                        cell.Colspan = 5;
                        table.AddCell(cell);
                    }

                    #endregion

                    #region DEP

                    int DEPJobType = Enums.ApplicationType.DEP.GetHashCode();
                    List<PermitsExpiryDTO> DEPPermitList = result.Where(x => x.JobNumber == jobNumber
                                                                          && x.IdJobType == DEPJobType
                                                                          && x.JobWorkTypeCode != "AHV"
                                                                          && x.JobWorkTypeCode != "TCO").OrderBy(x => x.JobNumber).ThenBy(x => x.Expires).ToList();
                    if (DEPPermitList != null && DEPPermitList.Count > 0)
                    {

                        cell = new PdfPCell(new Phrase("DEP PERMIT EXPIRATION", font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 5;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Application Type", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Street On/From/To", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Permit #", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Work Type | Permit Type", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        table.AddCell(cell);

                        foreach (PermitsExpiryDTO item in DEPPermitList)
                        {
                            cell = new PdfPCell(new Phrase(item.JobApplicationTypeName, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.JobApplicationStreetWorkingOn + " | " + item.JobApplicationStreetFrom + " | " + item.JobApplicationStreetTo, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.PermitNumber, font_Table_Data));
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

                            cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);
                        }

                        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.NO_BORDER;
                        cell.Colspan = 5;
                        table.AddCell(cell);
                    }

                    #endregion

                    #region VIOLATION

                    List<JobViolation> JobViolationList = rpoContext.JobViolations.Where(x => x.Job.JobNumber == jobNumber
                                                                     && x.IsFullyResolved == false).OrderBy(x => x.Job.JobNumber).ThenBy(x => x.HearingDate).ToList();
                    if (JobViolationList != null && JobViolationList.Count > 0)
                    {
                        cell = new PdfPCell(new Phrase("PENDING VIOLATION", font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 5;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Violation #", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Issuing Agency", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Date Issued", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Hearing Date", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Status", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        table.AddCell(cell);



                        foreach (JobViolation item in JobViolationList)
                        {
                            cell = new PdfPCell(new Phrase(item.SummonsNumber, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.IssuingAgency, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.DateIssued != null ? Convert.ToDateTime(item.DateIssued).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.HearingDate != null ? Convert.ToDateTime(item.HearingDate).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.StatusOfSummonsNotice, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);
                        }

                        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.NO_BORDER;
                        cell.Colspan = 5;
                        table.AddCell(cell);
                    }

                    #endregion

                    #region TCO
                    List<PermitsExpiryDTO> TCOPermitList = result.Where(x => x.JobNumber == jobNumber
                                                                          && x.JobWorkTypeCode == "TCO").OrderBy(x => x.JobNumber).ThenBy(x => x.Expires).ToList();

                    if (TCOPermitList != null && TCOPermitList.Count > 0)
                    {
                        cell = new PdfPCell(new Phrase("TCO EXPIRATION", font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 5;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //cell.BackgroundColor = new iTextSharp.text.BaseColor(146, 208, 80);
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Application #", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Application Type", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        table.AddCell(cell);

                        foreach (PermitsExpiryDTO item in TCOPermitList)
                        {
                            cell = new PdfPCell(new Phrase(item.JobApplicationNumber, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(item.JobApplicationTypeName, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(string.Empty, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(string.Empty, font_Table_Data));
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
                    }

                    #endregion

                    document.Add(table);
                }
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }

        //private string ExportToPdf_DOT(string permitExpire, List<PermitsExpiryDTO> result, string permitCode, string exportFilename)
        //{

        //    string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
        //    string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    string exportFilePath = path + exportFilename;
        //    File.Delete(exportFilePath);

        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        Document document = new Document(PageSize.LETTER.Rotate());
        //        document.SetMargins(18, 18, 12, 16);
        //        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(exportFilePath, FileMode.Create));
        //        document.Open();

        //        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\logo.jpg"));
        //        logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
        //        logo.ScaleToFit(120, 60);
        //        logo.SetAbsolutePosition(260, 760);

        //        Font font_11_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, 0);
        //        Font font_12_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 0);
        //        Font font_18_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 0);

        //        Font font_11_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, 1);
        //        Font font_12_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);
        //        Font font_18_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);

        //        Font font_Table_Header = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, 1);
        //        Font font_Table_Data = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, 0);

        //        PdfPTable table = new PdfPTable(6);
        //        table.WidthPercentage = 100;
        //        table.SplitLate = false;

        //        PdfPCell cell = new PdfPCell(logo);
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Rowspan = 2;
        //        table.AddCell(cell);

        //        string reportHeader = "NEW YORK CITY AGENCY FILINGS, APPROVALS & PERMITS"
        //            + Environment.NewLine + "146 WEST 29TH STREET, SUITE 2E"
        //            + Environment.NewLine + "NEW YORK, NY 10001"
        //            + Environment.NewLine + "TEL: (212) 566-5110"
        //            + Environment.NewLine + "FAX: (212) 566-5112";

        //        cell = new PdfPCell(new Phrase("RPO INCORPORATED", font_11_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 5;
        //        cell.PaddingLeft = -60;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(reportHeader, font_11_Normal));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 5;
        //        cell.PaddingLeft = -60;
        //        cell.VerticalAlignment = Element.ALIGN_TOP;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("DOT PERMIT EXPIRATION REPORT", font_18_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.TOP_BORDER;
        //        cell.Colspan = 6;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(permitExpire, font_11_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.TOP_BORDER;
        //        cell.Colspan = 6;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 5;
        //        table.AddCell(cell);

        //        string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

        //        cell = new PdfPCell(new Phrase(reportDate, font_11_Normal));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 1;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Job # | Address", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Special Place Name", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Permit #", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Street On | From | To", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Work Type | Permit Type", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);


        //        foreach (PermitsExpiryDTO item in result)
        //        {
        //            cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.JobAddress, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.SpecialPlace, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.PermitNumber, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.JobApplicationStreetWorkingOn + " | " + item.JobApplicationStreetFrom + " | " + item.JobApplicationStreetTo, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.PermitType, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);
        //        }

        //        document.Add(table);
        //        document.Close();

        //        writer.Close();
        //    }

        //    return exportFilePath;
        //}

        //private string ExportToPdf_DEP(string permitExpire, List<PermitsExpiryDTO> result, string permitCode, string exportFilename)
        //{

        //    string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
        //    string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    string exportFilePath = path + exportFilename;
        //    File.Delete(exportFilePath);

        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        Document document = new Document(PageSize.LETTER.Rotate());
        //        document.SetMargins(18, 18, 12, 16);
        //        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(exportFilePath, FileMode.Create));
        //        document.Open();

        //        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\logo.jpg"));
        //        logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
        //        logo.ScaleToFit(120, 60);
        //        logo.SetAbsolutePosition(260, 760);

        //        Font font_11_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, 0);
        //        Font font_12_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 0);
        //        Font font_18_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 0);

        //        Font font_11_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, 1);
        //        Font font_12_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);
        //        Font font_18_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);

        //        Font font_Table_Header = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, 1);
        //        Font font_Table_Data = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, 0);

        //        PdfPTable table = new PdfPTable(7);
        //        table.WidthPercentage = 100;
        //        table.SplitLate = false;

        //        PdfPCell cell = new PdfPCell(logo);
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Rowspan = 2;
        //        table.AddCell(cell);

        //        string reportHeader = "NEW YORK CITY AGENCY FILINGS, APPROVALS & PERMITS"
        //            + Environment.NewLine + "146 WEST 29TH STREET, SUITE 2E"
        //            + Environment.NewLine + "NEW YORK, NY 10001"
        //            + Environment.NewLine + "TEL: (212) 566-5110"
        //            + Environment.NewLine + "FAX: (212) 566-5112";

        //        cell = new PdfPCell(new Phrase("RPO INCORPORATED", font_11_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 6;
        //        cell.PaddingLeft = -60;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(reportHeader, font_11_Normal));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 6;
        //        cell.PaddingLeft = -60;
        //        cell.VerticalAlignment = Element.ALIGN_TOP;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("DEP PERMIT EXPIRATION REPORT", font_18_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.TOP_BORDER;
        //        cell.Colspan = 7;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(permitExpire, font_11_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.TOP_BORDER;
        //        cell.Colspan = 7;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 6;
        //        table.AddCell(cell);

        //        string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

        //        cell = new PdfPCell(new Phrase(reportDate, font_11_Normal));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 1;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Job # | Address", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Special Place Name", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Application Type", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Application #", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Work Type", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Permittee", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);


        //        foreach (PermitsExpiryDTO item in result)
        //        {
        //            cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.JobAddress, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.SpecialPlace, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.JobApplicationTypeName, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.JobApplicationNumber, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.JobWorkTypeDescription, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.Permittee, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);
        //        }

        //        document.Add(table);
        //        document.Close();

        //        writer.Close();
        //    }

        //    return exportFilePath;
        //}

        //private string ExportToPdf_AHV(string permitExpire, List<PermitsExpiryDTO> result, string permitCode, string exportFilename)
        //{

        //    string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
        //    string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    string exportFilePath = path + exportFilename;
        //    File.Delete(exportFilePath);

        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        Document document = new Document(PageSize.LETTER.Rotate());
        //        document.SetMargins(18, 18, 12, 16);
        //        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(exportFilePath, FileMode.Create));
        //        document.Open();

        //        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\logo.jpg"));
        //        logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
        //        logo.ScaleToFit(120, 60);
        //        logo.SetAbsolutePosition(260, 760);

        //        Font font_11_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, 0);
        //        Font font_12_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 0);
        //        Font font_18_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 0);

        //        Font font_11_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, 1);
        //        Font font_12_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);
        //        Font font_18_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);

        //        Font font_Table_Header = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, 1);
        //        Font font_Table_Data = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, 0);

        //        PdfPTable table = new PdfPTable(7);
        //        table.WidthPercentage = 100;
        //        table.SplitLate = false;

        //        PdfPCell cell = new PdfPCell(logo);
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Rowspan = 2;
        //        table.AddCell(cell);

        //        string reportHeader = "NEW YORK CITY AGENCY FILINGS, APPROVALS & PERMITS"
        //            + Environment.NewLine + "146 WEST 29TH STREET, SUITE 2E"
        //            + Environment.NewLine + "NEW YORK, NY 10001"
        //            + Environment.NewLine + "TEL: (212) 566-5110"
        //            + Environment.NewLine + "FAX: (212) 566-5112";

        //        cell = new PdfPCell(new Phrase("RPO INCORPORATED", font_11_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 6;
        //        cell.PaddingLeft = -60;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(reportHeader, font_11_Normal));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 6;
        //        cell.PaddingLeft = -60;
        //        cell.VerticalAlignment = Element.ALIGN_TOP;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("AHV PERMIT EXPIRATION REPORT", font_18_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.TOP_BORDER;
        //        cell.Colspan = 7;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(permitExpire, font_11_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.TOP_BORDER;
        //        cell.Colspan = 7;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 6;
        //        table.AddCell(cell);

        //        string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

        //        cell = new PdfPCell(new Phrase(reportDate, font_11_Normal));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 1;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Job # | Address", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Special Place Name", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Application Type", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Application #", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Work Type", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Permittee", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Expiration Date", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);


        //        foreach (PermitsExpiryDTO item in result)
        //        {
        //            cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.JobAddress, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.SpecialPlace, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.JobApplicationTypeName, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.JobApplicationNumber, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.JobWorkTypeDescription, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.Permittee, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString("MM/dd/yyyy") : string.Empty, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);
        //        }

        //        document.Add(table);
        //        document.Close();

        //        writer.Close();
        //    }

        //    return exportFilePath;
        //}

        //private string ExportToPdf_TCO(string permitExpire, List<PermitsExpiryDTO> result, string permitCode, string exportFilename)
        //{

        //    string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
        //    string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    string exportFilePath = path + exportFilename;
        //    File.Delete(exportFilePath);

        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        Document document = new Document(PageSize.LETTER.Rotate());
        //        document.SetMargins(18, 18, 12, 16);
        //        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(exportFilePath, FileMode.Create));
        //        document.Open();

        //        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\logo.jpg"));
        //        logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
        //        logo.ScaleToFit(120, 60);
        //        logo.SetAbsolutePosition(260, 760);

        //        Font font_11_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, 0);
        //        Font font_12_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 0);
        //        Font font_18_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 0);

        //        Font font_11_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, 1);
        //        Font font_12_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);
        //        Font font_18_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);

        //        Font font_Table_Header = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, 1);
        //        Font font_Table_Data = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, 0);

        //        PdfPTable table = new PdfPTable(8);
        //        table.WidthPercentage = 100;
        //        table.SplitLate = false;

        //        PdfPCell cell = new PdfPCell(logo);
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Rowspan = 2;
        //        table.AddCell(cell);

        //        string reportHeader = "NEW YORK CITY AGENCY FILINGS, APPROVALS & PERMITS"
        //            + Environment.NewLine + "146 WEST 29TH STREET, SUITE 2E"
        //            + Environment.NewLine + "NEW YORK, NY 10001"
        //            + Environment.NewLine + "TEL: (212) 566-5110"
        //            + Environment.NewLine + "FAX: (212) 566-5112";

        //        cell = new PdfPCell(new Phrase("RPO INCORPORATED", font_11_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 7;
        //        cell.PaddingLeft = -60;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(reportHeader, font_11_Normal));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 7;
        //        cell.PaddingLeft = -60;
        //        cell.VerticalAlignment = Element.ALIGN_TOP;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("TCO PERMIT EXPIRATION REPORT", font_18_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.TOP_BORDER;
        //        cell.Colspan = 8;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(permitExpire, font_11_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.TOP_BORDER;
        //        cell.Colspan = 8;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 7;
        //        table.AddCell(cell);

        //        string reportDate = "Date:" + DateTime.Today.ToString(Common.ExportReportDateFormat);

        //        cell = new PdfPCell(new Phrase(reportDate, font_11_Bold));
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        cell.Border = PdfPCell.NO_BORDER;
        //        cell.Colspan = 1;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Job # | Address", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Special Place Name", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Application #", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Construction s/o", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Temp. Elevator", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Final Elevator", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("Plumbing s/o", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        cell = new PdfPCell(new Phrase("TCO expiration", font_Table_Header));
        //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell.Border = PdfPCell.BOX;
        //        cell.Colspan = 1;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        table.AddCell(cell);

        //        foreach (PermitsExpiryDTO item in result)
        //        {
        //            cell = new PdfPCell(new Phrase(item.JobNumber + " | " + item.JobAddress, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.SpecialPlace, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.JobApplicationNumber, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.ConstructionSignedOff != null ? Convert.ToDateTime(item.ConstructionSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.TempElevator != null ? Convert.ToDateTime(item.TempElevator).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.FinalElevator != null ? Convert.ToDateTime(item.FinalElevator).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.PlumbingSignedOff != null ? Convert.ToDateTime(item.PlumbingSignedOff).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);

        //            cell = new PdfPCell(new Phrase(item.Expires != null ? Convert.ToDateTime(item.Expires).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
        //            cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //            cell.Border = PdfPCell.BOX;
        //            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            table.AddCell(cell);
        //        }

        //        document.Add(table);
        //        document.Close();

        //        writer.Close();
        //    }

        //    return exportFilePath;
        //}

        private PermitsExpiryDTO FormatOverallPermitExpiryReport(JobApplicationWorkPermitType jobApplicationWorkPermitResponse)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            return new PermitsExpiryDTO
            {
                Id = jobApplicationWorkPermitResponse.Id,
                IdJobApplication = jobApplicationWorkPermitResponse.IdJobApplication,
                IdJob = jobApplicationWorkPermitResponse.JobApplication.IdJob,
                IdJobType = jobApplicationWorkPermitResponse.JobApplication != null && jobApplicationWorkPermitResponse.JobApplication.JobApplicationType != null ? jobApplicationWorkPermitResponse.JobApplication.JobApplicationType.IdParent : null,
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
                JobApplicationStreetWorkingOn = jobApplicationWorkPermitResponse.JobApplication.StreetWorkingOn,
                JobApplicationStreetFrom = jobApplicationWorkPermitResponse.JobApplication.StreetFrom,
                JobApplicationStreetTo = jobApplicationWorkPermitResponse.JobApplication.StreetTo,
                PlumbingSignedOff = jobApplicationWorkPermitResponse.PlumbingSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.PlumbingSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.PlumbingSignedOff,
                FinalElevator = jobApplicationWorkPermitResponse.FinalElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.FinalElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.FinalElevator,
                TempElevator = jobApplicationWorkPermitResponse.TempElevator != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.TempElevator), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.TempElevator,
                ConstructionSignedOff = jobApplicationWorkPermitResponse.ConstructionSignedOff != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jobApplicationWorkPermitResponse.ConstructionSignedOff), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobApplicationWorkPermitResponse.ConstructionSignedOff,
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