
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
    using System.IO;
    using NPOI.XSSF.UserModel;
    using NPOI.SS.UserModel;
    using System.Web;
    using System.Globalization;
    using iTextSharp.text.pdf;
    using iTextSharp.text;
    [Authorize]
    public class ReportRfpsController : ApiController
    {
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Get the report of RFPs Report with filter and sorting 
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [ResponseType(typeof(DataTableResponse))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetReportRfps([FromUri] DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewProposalsReport))
            {
                int statusInDraft = RfpStatusMaster.In_Draft.GetHashCode();
                int statusOnHold = RfpStatusMaster.On_Hold.GetHashCode();
                int statusPendingReview = RfpStatusMaster.Pending_Review_By_RPO.GetHashCode();
                int statusReviewedAtRPO = RfpStatusMaster.Reviewed_At_RPO.GetHashCode();

                var rfps = this.rpoContext.Rfps.Include("RfpAddress.Borough").Include("RfpStatus").Where(x =>
                   x.IdRfpStatus == statusInDraft
                || x.IdRfpStatus == statusOnHold
                || x.IdRfpStatus == statusPendingReview
                || x.IdRfpStatus == statusReviewedAtRPO).AsQueryable();

                var recordsTotal = rfps.Count();
                var recordsFiltered = recordsTotal;

                var result = rfps
                    .AsEnumerable()
                    .Select(c => this.FormatRfpReport(c))
                    .AsQueryable()
                    .DataTableParameters(dataTableParameters, out recordsFiltered)
                    .ToArray();

                return this.Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Draw,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsTotal,
                    Data = result.OrderByDescending(x => x.LastModifiedDate)
                });
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Get the report of RFPs Report with filter and sorting  and export into excel
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportRfps/exporttoexcel")]
        public IHttpActionResult PostExportReportRfpsToExcel(DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                int statusInDraft = RfpStatusMaster.In_Draft.GetHashCode();
                int statusOnHold = RfpStatusMaster.On_Hold.GetHashCode();
                int statusPendingReview = RfpStatusMaster.Pending_Review_By_RPO.GetHashCode();
                int statusReviewedAtRPO = RfpStatusMaster.Reviewed_At_RPO.GetHashCode();

                var rfps = this.rpoContext.Rfps.Include("RfpAddress.Borough").Include("RfpStatus").Where(x =>
                   x.IdRfpStatus == statusInDraft
                || x.IdRfpStatus == statusOnHold
                || x.IdRfpStatus == statusPendingReview
                || x.IdRfpStatus == statusReviewedAtRPO).AsQueryable();

                List<RfpReportDTO> result = rfps
                    .AsEnumerable()
                    .Select(c => this.FormatRfpReport(c))
                    .AsQueryable().OrderBy(x => x.RfpNumber).ThenBy(x => x.Address)
                    .ToList();


                string exportFilename = "ProposalReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
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
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private string ExportToExcel(List<RfpReportDTO> result, string exportFilename)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
            templateFileName = "ProposalReportTemplate - Copy.xlsx";
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
            foreach (RfpReportDTO item in result)
            {
                IRow iRow = sheet.GetRow(index);
                if (iRow != null)
                {
                    if (iRow.GetCell(0) != null)
                    {
                        iRow.GetCell(0).SetCellValue(item.RfpNumber + " | " + item.Address);
                    }
                    else
                    {
                        iRow.CreateCell(0).SetCellValue(item.RfpNumber + " | " + item.Address);
                    }

                    if (iRow.GetCell(1) != null)
                    {
                        iRow.GetCell(1).SetCellValue(item.SpecialPlaceName);
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(item.SpecialPlaceName);
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
                        iRow.GetCell(3).SetCellValue(item.FormattedCost);
                    }
                    else {
                        iRow.CreateCell(3).SetCellValue(item.FormattedCost);
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.CreatedByEmployee);
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.CreatedByEmployee);
                    }

                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(item.LastModifiedDate != null ? Convert.ToDateTime(item.LastModifiedDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else {
                        iRow.CreateCell(5).SetCellValue(item.LastModifiedDate != null ? Convert.ToDateTime(item.LastModifiedDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(6) != null)
                    {
                        iRow.GetCell(6).SetCellValue(item.RfpStatus);
                    }
                    else {
                        iRow.CreateCell(6).SetCellValue(item.RfpStatus);
                    }

                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(3).CellStyle = rightAlignCellStyle;
                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                }
                else
                {
                    iRow = sheet.CreateRow(index);

                    if (iRow.GetCell(0) != null)
                    {
                        iRow.GetCell(0).SetCellValue(item.RfpNumber + " | " + item.Address);
                    }
                    else
                    {
                        iRow.CreateCell(0).SetCellValue(item.RfpNumber + " | " + item.Address);
                    }

                    if (iRow.GetCell(1) != null)
                    {
                        iRow.GetCell(1).SetCellValue(item.SpecialPlaceName);
                    }
                    else
                    {
                        iRow.CreateCell(1).SetCellValue(item.SpecialPlaceName);
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
                        iRow.GetCell(3).SetCellValue(item.FormattedCost);
                    }
                    else {
                        iRow.CreateCell(3).SetCellValue(item.FormattedCost);
                    }

                    if (iRow.GetCell(4) != null)
                    {
                        iRow.GetCell(4).SetCellValue(item.CreatedByEmployee);
                    }
                    else
                    {
                        iRow.CreateCell(4).SetCellValue(item.CreatedByEmployee);
                    }

                    if (iRow.GetCell(5) != null)
                    {
                        iRow.GetCell(5).SetCellValue(item.LastModifiedDate != null ? Convert.ToDateTime(item.LastModifiedDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }
                    else {
                        iRow.CreateCell(5).SetCellValue(item.LastModifiedDate != null ? Convert.ToDateTime(item.LastModifiedDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                    }

                    if (iRow.GetCell(6) != null)
                    {
                        iRow.GetCell(6).SetCellValue(item.RfpStatus);
                    }
                    else {
                        iRow.CreateCell(6).SetCellValue(item.RfpStatus);
                    }

                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                    iRow.GetCell(3).CellStyle = rightAlignCellStyle;
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
        /// Get the report of RFPs Report with filter and sorting  and export inot pdf
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/ReportRfps/exporttopdf")]
        public IHttpActionResult PostExportReportRfpsToPdf(DataTableParameters dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                int statusInDraft = RfpStatusMaster.In_Draft.GetHashCode();
                int statusOnHold = RfpStatusMaster.On_Hold.GetHashCode();
                int statusPendingReview = RfpStatusMaster.Pending_Review_By_RPO.GetHashCode();
                int statusReviewedAtRPO = RfpStatusMaster.Reviewed_At_RPO.GetHashCode();

                var rfps = this.rpoContext.Rfps.Include("RfpAddress.Borough").Include("RfpStatus").Where(x =>
                   x.IdRfpStatus == statusInDraft
                || x.IdRfpStatus == statusOnHold
                || x.IdRfpStatus == statusPendingReview
                || x.IdRfpStatus == statusReviewedAtRPO).AsQueryable();

                List<RfpReportDTO> result = rfps
                    .AsEnumerable()
                    .Select(c => this.FormatRfpReport(c))
                    .AsQueryable().OrderBy(x => x.RfpNumber).ThenBy(x => x.Address)
                    .ToList();


                string exportFilename = "ProposalReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
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
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private string ExportToPdf(List<RfpReportDTO> result, string exportFilename)
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
                //cell.Colspan = 6;
                //cell.PaddingLeft = -60;
                //table.AddCell(cell);

                //cell = new PdfPCell(new Phrase(reportHeader, font_11_Normal));
                //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 6;
                //cell.PaddingLeft = -60;
                //cell.VerticalAlignment = Element.ALIGN_TOP;
                //table.AddCell(cell);

                //cell = new PdfPCell(SnapCorLogo);
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 7;
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


                cell = new PdfPCell(new Phrase("PROPOSALS REPORT", font_16_Bold));
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

                cell = new PdfPCell(new Phrase("Project # | Address", font_Table_Header));
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

                cell = new PdfPCell(new Phrase("Company", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Cost", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Created By", font_Table_Header));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.BOX;
                cell.Colspan = 1;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Last Modified On", font_Table_Header));
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

                foreach (RfpReportDTO item in result)
                {
                    cell = new PdfPCell(new Phrase(item.RfpNumber + " | " + item.Address, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.SpecialPlaceName, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.Company, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.FormattedCost, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.CreatedByEmployee, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.LastModifiedDate != null ? Convert.ToDateTime(item.LastModifiedDate).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(item.RfpStatus, font_Table_Data));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.BOX;
                    table.AddCell(cell);
                }

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }

        private RfpReportDTO FormatRfpReport(Rfp rfp)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new RfpReportDTO
            {
                Id = rfp.Id,
                RfpNumber = rfp.RfpNumber,
                Address1 = rfp.Address1,
                Address2 = rfp.Address2,
                Apartment = rfp.Apartment,
                Block = rfp.Block,
                Address = rfp.RfpAddress != null ? (rfp.RfpAddress.HouseNumber + " " + (rfp.RfpAddress.Street != null ? rfp.RfpAddress.Street + ", " : string.Empty)
                                         + (rfp.RfpAddress.Borough != null ? rfp.RfpAddress.Borough.Description : string.Empty)) : string.Empty,
                SpecialPlaceName = rfp.SpecialPlace,
                Company = rfp.Company != null ? rfp.Company.Name : string.Empty,
                Cost = rfp.Cost,
                FormattedCost = rfp.Cost != null ? Convert.ToDouble(rfp.Cost).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")).Replace("$", "$ ") : string.Empty,
                IdRfpStatus = rfp.IdRfpStatus,
                RfpStatus = rfp.RfpStatus != null ? rfp.RfpStatus.Name : string.Empty,
                IdCompany = rfp.IdCompany,
                LastUpdatedStep = rfp.LastUpdatedStep,
                IdContact = rfp.IdContact,
                CreatedDate = rfp.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfp.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfp.CreatedDate,
                LastModifiedDate = rfp.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(rfp.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : rfp.LastModifiedDate,
                CreatedByEmployee = rfp.CreatedBy != null ? rfp.CreatedBy.FirstName + " " + rfp.CreatedBy.LastName : string.Empty,
                LastModifiedByEmployee = rfp.LastModifiedBy != null ? rfp.LastModifiedBy.FirstName + " " + rfp.LastModifiedBy.LastName : (rfp.CreatedBy != null ? rfp.CreatedBy.FirstName + " " + rfp.CreatedBy.LastName : string.Empty),
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