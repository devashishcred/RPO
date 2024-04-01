
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
    using System.IO;
    using NPOI.XSSF.UserModel;
    using NPOI.SS.UserModel;
    using System.Web;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    [Authorize]
    public class ReportCompletedScopeBillingPointsController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();
        /// <summary>
        /// /// Get the report of CompletedScopeBillingPoints Report with filter and sorting  
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetReportCompletedScopeBillingPoints([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewComplateScopeReport))
            {
                List<int> idJobFeescheduleList = this.rpoContext.JobMilestoneServices.Include("Milestone.Job.RfpAddress")
                .Where(x => (x.Milestone.InvoiceNumber ?? string.Empty) == string.Empty && x.Milestone.Status == "Completed")
                .Select(x => x.IdJobFeeSchedule).ToList();

            var milestones = this.rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").Where(x =>
               (x.InvoiceNumber ?? string.Empty) == string.Empty
               && x.Status == "Completed").AsQueryable();

            var scopes = this.rpoContext.JobFeeSchedules.Include("Job.RfpAddress.Borough").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x =>            
            (x.InvoiceNumber ?? string.Empty ) == string.Empty
               && x.Status == "Completed"
               && !idJobFeescheduleList.Contains(x.Id) && x.IsAdditionalService == true).AsQueryable();

            List<CompletedScopeBillingPointDTO> completedScopeBillingPointDTOList = new List<CompletedScopeBillingPointDTO>();

            completedScopeBillingPointDTOList.AddRange(milestones.AsEnumerable().Select(x => FormatJobMilestoneReport(x)));

            completedScopeBillingPointDTOList.AddRange(scopes.AsEnumerable().Select(x => FormatJobFeeScheduleReport(x)));

            var recordsTotal = completedScopeBillingPointDTOList.Count();
            var recordsFiltered = recordsTotal;

            var result = completedScopeBillingPointDTOList
                .AsEnumerable()
                .Select(c => c)
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
        /// /// Get the report of CompletedScopeBillingPoints Report with filter and sorting  and export to excel
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportCompletedScopeBillingPoints/exporttoexcel")]
        public IHttpActionResult PostExportReportCompletedScopeBillingPointsToExcel(DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                List<int> idJobFeescheduleList = this.rpoContext.JobMilestoneServices.Include("Milestone.Job.RfpAddress")
                    .Where(x => (x.Milestone.InvoiceNumber ?? string.Empty) == string.Empty && x.Milestone.Status == "Completed")
                    .Select(x => x.IdJobFeeSchedule).ToList();

                var milestones = this.rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").Where(x =>
                   (x.InvoiceNumber ?? string.Empty) == string.Empty
                   && x.Status == "Completed").AsQueryable();

                var scopes = this.rpoContext.JobFeeSchedules.Include("Job.RfpAddress.Borough").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x =>
                   (x.InvoiceNumber ?? string.Empty) == string.Empty
                   && x.Status == "Completed"
                   && !idJobFeescheduleList.Contains(x.Id) && x.IsAdditionalService == true).AsQueryable();

                List<CompletedScopeBillingPointDTO> completedScopeBillingPointDTOList = new List<CompletedScopeBillingPointDTO>();

                completedScopeBillingPointDTOList.AddRange(milestones.AsEnumerable().Select(x => FormatJobMilestoneReport(x)));

                completedScopeBillingPointDTOList.AddRange(scopes.AsEnumerable().Select(x => FormatJobFeeScheduleReport(x)));

                List<CompletedScopeBillingPointDTO> result = completedScopeBillingPointDTOList
                    .AsEnumerable()
                    .Select(c => c)
                    .AsQueryable().OrderBy(x => x.JobNumber).ThenBy(x => x.Address)
                    .ToList();

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
                        result = result.OrderByDescending(o => o.JobNumber).ToList();
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        result = result.OrderByDescending(o => o.Address).ToList();
                    if (orderBy.Trim().ToLower() == "Company".Trim().ToLower())
                        result = result.OrderByDescending(o => o.Company).ToList();
                    if (orderBy.Trim().ToLower() == "Contact".Trim().ToLower())
                        result = result.OrderByDescending(o => o.Contact).ToList();
                    if (orderBy.Trim().ToLower() == "BillingPointName".Trim().ToLower())
                        result = result.OrderByDescending(o => o.BillingPointName).ToList();
                    if (orderBy.Trim().ToLower() == "CompletedDate".Trim().ToLower())
                        result = result.OrderByDescending(o => o.CompletedDate).ToList();
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        result = result.OrderBy(o => o.JobNumber).ToList();
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        result = result.OrderBy(o => o.Address).ToList();
                    if (orderBy.Trim().ToLower() == "Company".Trim().ToLower())
                        result = result.OrderBy(o => o.Company).ToList();
                    if (orderBy.Trim().ToLower() == "Contact".Trim().ToLower())
                        result = result.OrderBy(o => o.Contact).ToList();
                    if (orderBy.Trim().ToLower() == "BillingPointName".Trim().ToLower())
                        result = result.OrderBy(o => o.BillingPointName).ToList();
                    if (orderBy.Trim().ToLower() == "CompletedDate".Trim().ToLower())
                        result = result.OrderBy(o => o.CompletedDate).ToList();
                }
                if (result != null && result.Count > 0)
                {
                    string exportFilename = "CompletedScopeBillingPointReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
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

        private string ExportToExcel(List<CompletedScopeBillingPointDTO> result, string exportFilename)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
          //  templateFileName = "CompletedScopeBillingPointTemplate.xlsx";
            templateFileName = "CompletedScopeBillingPointTemplate - Copy.xlsx";

            string templateFilePath = templatePath + templateFileName;
            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateFilePath);
            ISheet sheet = templateWorkbook.GetSheet("Sheet1");

            XSSFFont myFont = (XSSFFont)templateWorkbook.CreateFont();
            myFont.FontHeightInPoints = (short)12;
           // myFont.FontName = "Red Hat Display";
            myFont.FontName = "Times New Roman";
            myFont.IsBold = false;
            XSSFFont myFont_Bold = (XSSFFont)templateWorkbook.CreateFont();
            myFont_Bold.FontHeightInPoints = (short)12;
            myFont_Bold.FontName = "Times New Roman";
            myFont_Bold.IsBold = true;

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
            #region Column Header cell style
            XSSFCellStyle ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            ColumnHeaderCellStyle.SetFont(myFont_Bold);
            ColumnHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
            ColumnHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
            ColumnHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
            ColumnHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
            ColumnHeaderCellStyle.WrapText = true;
            ColumnHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
            ColumnHeaderCellStyle.Alignment = HorizontalAlignment.Left;
            ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
            //  ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(226, 239, 218));
            ColumnHeaderCellStyle.FillPattern = FillPattern.SolidForeground;
            #endregion
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
           IRow iRow = sheet.GetRow(index);
            if(iRow==null)
             iRow = sheet.CreateRow(index);

                if (iRow.GetCell(0) != null)
                {
                    iRow.GetCell(0).SetCellValue("Project # ");
                }
                else
                {
                    iRow.CreateCell(0).SetCellValue("Project # ");
                }
                if (iRow.GetCell(1) != null)
                {
                    iRow.GetCell(1).SetCellValue("Address");
                }
                else
                {
                    iRow.CreateCell(1).SetCellValue("Address");
                }

                if (iRow.GetCell(2) != null)
                {
                    iRow.GetCell(2).SetCellValue("Company");
                }
                else
                {
                    iRow.CreateCell(2).SetCellValue("Company");
                }

                if (iRow.GetCell(3) != null)
                {
                    iRow.GetCell(3).SetCellValue("Contact");
                }
                else
                {
                    iRow.CreateCell(3).SetCellValue("Contact");
                }

                if (iRow.GetCell(4) != null)
                {
                    iRow.GetCell(4).SetCellValue("Billing Point Name");
                }
                else
                {
                    iRow.CreateCell(4).SetCellValue("Billing Point Name");
                }

                if (iRow.GetCell(5) != null)
                {
                    iRow.GetCell(5).SetCellValue("Completed Date");
                }
                else
                {
                    iRow.CreateCell(5).SetCellValue("Completed Date");
                }

                if (iRow.GetCell(6) != null)
                {
                    iRow.GetCell(6).SetCellValue("Billing Cost");
                }
                else
                {
                    iRow.CreateCell(6).SetCellValue("Billing Cost");
                }

                iRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(1).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                iRow.GetCell(6).CellStyle = ColumnHeaderCellStyle;
          //  iReportDateRow.GetCell(6).CellStyle = rightAlignCellStyle;


             index = 6;
            foreach (CompletedScopeBillingPointDTO item in result)
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
                        iRow.GetCell(2).SetCellValue(item.Company);
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(item.Company);
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(item.Contact);
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(item.Contact);
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.BillingPointName);
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.BillingPointName);
                    }

                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(item.CompletedDate != null ? item.CompletedDate.Value.ToString("MM/dd/yyyy") : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(item.CompletedDate != null ? item.CompletedDate.Value.ToString("MM/dd/yyyy") : string.Empty);
                    }

                    if (iRow.GetCell(6) != null)
                    {
                        iRow.GetCell(6).SetCellValue(item.FormattedBillingCost);
                    }
                    else
                    {
                        iRow.CreateCell(6).SetCellValue(item.FormattedBillingCost);
                    }

                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(6).CellStyle = rightAlignCellStyle;
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
                        iRow.GetCell(2).SetCellValue(item.Company);
                    }
                    else
                    {
                        iRow.CreateCell(2).SetCellValue(item.Company);
                    }

                    if (iRow.GetCell(3) != null)
                    {
                        iRow.GetCell(3).SetCellValue(item.Contact);
                    }
                    else
                    {
                        iRow.CreateCell(3).SetCellValue(item.Contact);
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.BillingPointName);
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.BillingPointName);
                    }

                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(item.CompletedDate != null ? item.CompletedDate.Value.ToString("MM/dd/yyyy") : string.Empty);
                    }
                    else
                    {
                        iRow.CreateCell(5).SetCellValue(item.CompletedDate != null ? item.CompletedDate.Value.ToString("MM/dd/yyyy") : string.Empty);
                    }

                    if (iRow.GetCell(6) != null)
                    {
                        iRow.GetCell(6).SetCellValue(item.FormattedBillingCost);
                    }
                    else
                    {
                        iRow.CreateCell(6).SetCellValue(item.FormattedBillingCost);
                    }

                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(6).CellStyle = rightAlignCellStyle;
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
        ///  /// <summary>
        /// /// Get the report of CompletedScopeBillingPoints Report with filter and sorting  and export to pdf
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportCompletedScopeBillingPoints/exporttopdf")]
        public IHttpActionResult PostExportReportCompletedScopeBillingPointsToPdf(DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                List<int> idJobFeescheduleList = this.rpoContext.JobMilestoneServices.Include("Milestone.Job.RfpAddress")
                    .Where(x => (x.Milestone.InvoiceNumber ?? string.Empty) == string.Empty && x.Milestone.Status == "Completed")
                    .Select(x => x.IdJobFeeSchedule).ToList();

                var milestones = this.rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").Where(x =>
                   (x.InvoiceNumber ?? string.Empty) == string.Empty
                   && x.Status == "Completed").AsQueryable();

                var scopes = this.rpoContext.JobFeeSchedules.Include("Job.RfpAddress.Borough").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x =>
                   (x.InvoiceNumber ?? string.Empty) == string.Empty
                   && x.Status == "Completed"
                   && !idJobFeescheduleList.Contains(x.Id) && x.IsAdditionalService == true).AsQueryable();

                List<CompletedScopeBillingPointDTO> completedScopeBillingPointDTOList = new List<CompletedScopeBillingPointDTO>();

                completedScopeBillingPointDTOList.AddRange(milestones.AsEnumerable().Select(x => FormatJobMilestoneReport(x)));

                completedScopeBillingPointDTOList.AddRange(scopes.AsEnumerable().Select(x => FormatJobFeeScheduleReport(x)));

                List<CompletedScopeBillingPointDTO> result = completedScopeBillingPointDTOList
                    .AsEnumerable()
                    .Select(c => c)
                    .AsQueryable().OrderBy(x => x.JobNumber).ThenBy(x => x.Address)
                    .ToList();

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
                        result = result.OrderByDescending(o => o.JobNumber).ToList();
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        result = result.OrderByDescending(o => o.Address).ToList();
                    if (orderBy.Trim().ToLower() == "Company".Trim().ToLower())
                        result = result.OrderByDescending(o => o.Company).ToList();
                    if (orderBy.Trim().ToLower() == "Contact".Trim().ToLower())
                        result = result.OrderByDescending(o => o.Contact).ToList();
                    if (orderBy.Trim().ToLower() == "BillingPointName".Trim().ToLower())
                        result = result.OrderByDescending(o => o.BillingPointName).ToList();
                    if (orderBy.Trim().ToLower() == "CompletedDate".Trim().ToLower())
                        result = result.OrderByDescending(o => o.CompletedDate).ToList();
                }
                else
                {
                    if (orderBy.Trim().ToLower() == "JobNumber".Trim().ToLower())
                        result = result.OrderBy(o => o.JobNumber).ToList();
                    if (orderBy.Trim().ToLower() == "Address".Trim().ToLower())
                        result = result.OrderBy(o => o.Address).ToList();
                    if (orderBy.Trim().ToLower() == "Company".Trim().ToLower())
                        result = result.OrderBy(o => o.Company).ToList();
                    if (orderBy.Trim().ToLower() == "Contact".Trim().ToLower())
                        result = result.OrderBy(o => o.Contact).ToList();
                    if (orderBy.Trim().ToLower() == "BillingPointName".Trim().ToLower())
                        result = result.OrderBy(o => o.BillingPointName).ToList();
                    if (orderBy.Trim().ToLower() == "CompletedDate".Trim().ToLower())
                        result = result.OrderBy(o => o.CompletedDate).ToList();
                }
                if (result != null && result.Count > 0)
                {
                    string exportFilename = "CompletedScopeBillingPointReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    string exportFilePath = ExportToPdf(result, exportFilename);
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

        private string ExportToPdf(List<CompletedScopeBillingPointDTO> result, string exportFilename)
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

                cell = new PdfPCell(new Phrase("COMPLETED SCOPE, BILLING POINT REPORT", font_16_Bold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = PdfPCell.TOP_BORDER;
                cell.Colspan = 7;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Chunk.NEWLINE));
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
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project #", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Address", font_Table_Header));
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
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Contact", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Billing Point Name", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Completed Date", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Billing Cost", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                foreach (CompletedScopeBillingPointDTO item in result)
                {
                    cell = new PdfPCell(new Phrase(item.JobNumber, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Address, font_Table_Data));
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

                    cell = new PdfPCell(new Phrase(item.BillingPointName, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.CompletedDate != null ? item.CompletedDate.Value.ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.FormattedBillingCost, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
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

        private CompletedScopeBillingPointDTO FormatJobMilestoneReport(JobMilestone jobMilestone)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string JobApplicationType = string.Empty;

            if (jobMilestone.Job != null && jobMilestone.IdJob != null)
            {
                JobApplicationType = string.Join(",", jobMilestone.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }

            return new CompletedScopeBillingPointDTO
            {
                IdJob = jobMilestone.IdJob,
                JobNumber = jobMilestone.Job != null ? jobMilestone.Job.JobNumber : string.Empty,
                JobApplicationType = JobApplicationType,
                Address = jobMilestone.Job.RfpAddress != null ? (jobMilestone.Job.RfpAddress.HouseNumber + " " + (jobMilestone.Job.RfpAddress.Street != null ? jobMilestone.Job.RfpAddress.Street + ", " : string.Empty)
                                         + (jobMilestone.Job.RfpAddress.Borough != null ? jobMilestone.Job.RfpAddress.Borough.Description : string.Empty)) : string.Empty,
                Company = jobMilestone.Job.Company != null ? jobMilestone.Job.Company.Name : string.Empty,
                IdCompany = jobMilestone.Job.IdCompany,
                IdContact = jobMilestone.Job.IdContact,
                Contact = jobMilestone.Job.Contact != null ? jobMilestone.Job.Contact.FirstName + " " + jobMilestone.Job.Contact.LastName : string.Empty,
                BillingPointName = jobMilestone.Name,
                PONumber = jobMilestone.PONumber,
                InvoiceNumber = jobMilestone.InvoiceNumber,
                IsInvoiced = jobMilestone.IsInvoiced,
                BillingCost = jobMilestone.Value,
                FormattedBillingCost = jobMilestone.Value != null ? Convert.ToDouble(jobMilestone.Value).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "$ ") : string.Empty,
                Status = jobMilestone.Status,
                CompletedDate = jobMilestone.LastModified != null ? jobMilestone.LastModified : jobMilestone.CreatedDate
            };
        }

        private CompletedScopeBillingPointDTO FormatJobFeeScheduleReport(JobFeeSchedule jobFeeSchedule)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string JobApplicationType = string.Empty;

            if (jobFeeSchedule.Job != null && jobFeeSchedule.IdJob != null)
            {
                JobApplicationType = string.Join(",", jobFeeSchedule.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }
            return new CompletedScopeBillingPointDTO
            {
                IdJob = jobFeeSchedule.IdJob,
                JobNumber = jobFeeSchedule.Job != null ? jobFeeSchedule.Job.JobNumber : string.Empty,
                JobApplicationType = JobApplicationType,
                Address = jobFeeSchedule.Job.RfpAddress != null ? (jobFeeSchedule.Job.RfpAddress.HouseNumber + " " + (jobFeeSchedule.Job.RfpAddress.Street != null ? jobFeeSchedule.Job.RfpAddress.Street + ", " : string.Empty)
                                         + (jobFeeSchedule.Job.RfpAddress.Borough != null ? jobFeeSchedule.Job.RfpAddress.Borough.Description : string.Empty)) : string.Empty,
                Company = jobFeeSchedule.Job.Company != null ? jobFeeSchedule.Job.Company.Name : string.Empty,
                IdCompany = jobFeeSchedule.Job.IdCompany,
                IdContact = jobFeeSchedule.Job.IdContact,
                Contact = jobFeeSchedule.Job.Contact != null ? jobFeeSchedule.Job.Contact.FirstName + " " + jobFeeSchedule.Job.Contact.LastName : string.Empty,
                BillingPointName = Common.GetServiceItemName(jobFeeSchedule),
                PONumber = jobFeeSchedule.PONumber,
                InvoiceNumber = jobFeeSchedule.InvoiceNumber,
                IsInvoiced = jobFeeSchedule.IsInvoiced,
                BillingCost = Common.GetServiceItemCost(jobFeeSchedule),
                FormattedBillingCost = Common.GetServiceItemCost(jobFeeSchedule) != null ? Convert.ToDouble(Common.GetServiceItemCost(jobFeeSchedule)).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "") : string.Empty,
                Status = jobFeeSchedule.Status,
                CompletedDate = jobFeeSchedule.CompletedDate != null ? jobFeeSchedule.CompletedDate : null
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