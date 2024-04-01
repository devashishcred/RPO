
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
    using System.Configuration;
    using Microsoft.ApplicationBlocks.Data;
    using System.Data;
    using System.Data.SqlClient;
    using Models;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Api.Controllers.Reports.Models;
    using CheckListApplicationDropDown.Models;



    [Authorize]
    public class ReportChecklistController : ApiController
    {
        RpoContext rpoContext = new RpoContext();
        private object CellStyle;


        /// <summary>
        ///  Get the report of ReportOverallPermitExpiry Report with filter and sorting  and export to excel
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/Checklist/ExportChecklistToExcel")]
        public IHttpActionResult PostExportChecklistToExcel(ChecklistReportDatatableParameter dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[3];

                spParameter[0] = new SqlParameter("@IdJobCheckListHeader", SqlDbType.NVarChar);
                spParameter[0].Direction = ParameterDirection.Input;
                var lstheaders = dataTableParameters.lstexportChecklist.Select(x => x.jobChecklistHeaderId).ToList();
                string headerid = null;
                foreach (var l in lstheaders)
                {
                    headerid += l + ",";
                }
                //  headerid.Remove(headerid.Length - 1, 1);
                spParameter[0].Value = headerid.Remove(headerid.Length - 1, 1);


                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.Displayorder;
                // spParameter[1].Value = dataTableParameters.lstexportChecklist.Select(x=>x.Displayorder);

                spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = string.Empty;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "ChecklistView", spParameter);
                List<CheckListApplicationDropDown.Models.ChecklistReportDTO> headerlist = new List<ChecklistReportDTO>();

                List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
                            //Name = row.Field<string>("NAME")
                        }).Select(g => g.CopyToDataTable()).ToList();
                // .CopyToDataTable();

                foreach (var loopheader in distinctheaders)
                {
                    ChecklistReportDTO header = new ChecklistReportDTO();
                    List<ReportChecklistGroup> Unorderedgroups = new List<ReportChecklistGroup>();
                    Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                    header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                    header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                    header.applicationName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                    header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                    header.groups = new List<ReportChecklistGroup>();
                    header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["Idjob"]);
                    var checklist = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId).FirstOrDefault();
                    header.DisplayOrder = checklist.Displayorder;
                    var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                        .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();
                    var payloadgroup = checklist.lstExportChecklistGroup.Select(x => x.jobChecklistGroupId).ToList();
                    // foreach (var eachGroup in payloadgroup)
                    foreach (var eachGroup in distinctGroup)
                    {
                        if (payloadgroup.Contains(Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"])))
                        {
                            ReportChecklistGroup group = new ReportChecklistGroup();
                            group.item = new List<ReportItem>();

                            Int32 IdJobChecklistGroup = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                            group.checkListGroupName = eachGroup.Rows[0]["ChecklistGroupName"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["ChecklistGroupName"].ToString();
                            group.jobChecklistGroupId = eachGroup.Rows[0]["IdJobChecklistGroup"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                            group.checkListGroupType = eachGroup.Rows[0]["checklistGroupType"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["checklistGroupType"].ToString();
                            group.DisplayOrder1 = checklist.lstExportChecklistGroup.Where(y => y.jobChecklistGroupId == IdJobChecklistGroup).Select(x => x.displayOrder1).FirstOrDefault();

                            if (eachGroup.Rows[0]["checklistGroupType"].ToString().ToLower().TrimEnd() != "pl")
                            {
                                var groupsitems = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
                                for (int j = 0; j < groupsitems.Count; j++)
                                {

                                    ReportItem item = new ReportItem();
                                    item.Details = new List<ReportDetails>();

                                    #region Items
                                    Int32 IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                    if (IdChecklistItem != 0)
                                    {
                                        item.checklistItemName = groupsitems[j]["checklistItemName"] == DBNull.Value ? string.Empty : groupsitems[j]["checklistItemName"].ToString();
                                        item.IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                        item.jocbChecklistItemDetailsId = groupsitems[j]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["JocbChecklistItemDetailsId"]);
                                        int itemid = Convert.ToInt32(item.jocbChecklistItemDetailsId);
                                        item.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobchecklistItemDetails == itemid);
                                        item.comments = groupsitems[j]["Comments"] == DBNull.Value ? string.Empty : groupsitems[j]["Comments"].ToString();
                                        item.idDesignApplicant = groupsitems[j]["IdDesignApplicant"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdDesignApplicant"]);
                                        item.idInspector = groupsitems[j]["IdInspector"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdInspector"]);
                                        item.stage = groupsitems[j]["Stage"] == DBNull.Value ? string.Empty : groupsitems[j]["Stage"].ToString();
                                        item.partyResponsible1 = groupsitems[j]["PartyResponsible1"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["PartyResponsible1"]);
                                        item.manualPartyResponsible = groupsitems[j]["ManualPartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["ManualPartyResponsible"].ToString();
                                        item.idContact = groupsitems[j]["IdContact"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdContact"]);
                                        item.referenceDocumentId = groupsitems[j]["IdReferenceDocument"] == DBNull.Value ? string.Empty : groupsitems[j]["IdReferenceDocument"].ToString();
                                        item.dueDate = groupsitems[j]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["DueDate"]);
                                        item.status = groupsitems[j]["Status"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Status"]);
                                        item.checkListLastModifiedDate = groupsitems[j]["checklistItemLastDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["checklistItemLastDate"]);
                                        item.idCreateFormDocument = groupsitems[j]["IdCreateFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCreateFormDocument"]);
                                        item.idUploadFormDocument = groupsitems[j]["IdUploadFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdUploadFormDocument"]);
                                        item.Displayorder = groupsitems[j]["Displayorder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Displayorder"]);
                                        item.ChecklistDetailDisplayOrder = groupsitems[j]["ChecklistDetailDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["ChecklistDetailDisplayOrder"]);
                                        item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                        if (item.idContact != null && item.idContact != 0)
                                        {
                                            var c = rpoContext.Contacts.Find(item.idContact);
                                            item.ContactName = c.FirstName + " " + c.LastName;
                                            if (c.IdCompany != null && c.IdCompany != 0)
                                            {
                                                item.CompanyName = rpoContext.Companies.Where(x => x.Id == c.IdCompany).Select(y => y.Name).FirstOrDefault();
                                            }
                                        }
                                        if (item.idDesignApplicant != null && item.idDesignApplicant != 0)
                                        {
                                            var c = rpoContext.JobContacts.Find(item.idDesignApplicant);
                                            if (c != null)
                                            {
                                                item.DesignApplicantName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty; ;
                                            }
                                        }
                                        if (item.idInspector != null && item.idInspector != 0)
                                        {
                                            var c = rpoContext.JobContacts.Find(item.idInspector);
                                            if (c != null)
                                            {
                                                item.InspectorName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty; ;
                                            }
                                        }
                                        #endregion

                                        group.item.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();

                                var PlumbingCheckListFloors = groupsitems2.AsEnumerable().Select(a => a.Field<Int32?>("IdJobPlumbingCheckListFloors")).Distinct().ToList();

                                for (int j = 0; j < PlumbingCheckListFloors.Count(); j++)
                                {

                                    ReportItem item = new ReportItem();
                                    item.Details = new List<ReportDetails>();
                                    var IdJobPlumbingCheckListFloors1 = PlumbingCheckListFloors[j];
                                    var detailItem = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobPlumbingCheckListFloors") == IdJobPlumbingCheckListFloors1).ToList();
                                    #region Items
                                    item.FloorNumber = detailItem[0]["FloorNumber"] == DBNull.Value ? string.Empty : detailItem[0]["FloorNumber"].ToString();
                                    item.idJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors1 != 0 ? IdJobPlumbingCheckListFloors1 : 0;
                                    #endregion

                                    for (int i = 0; i < detailItem.Count; i++)
                                    {
                                        ReportDetails detail = new ReportDetails();
                                        #region Items details
                                        detail.checklistGroupType = detailItem[i]["checklistGroupType"] == DBNull.Value ? string.Empty : detailItem[i]["checklistGroupType"].ToString();
                                        detail.idJobPlumbingInspection = detailItem[i]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingInspection"]);
                                        detail.idJobPlumbingCheckListFloors = detailItem[i]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingCheckListFloors"]);
                                        detail.inspectionPermit = detailItem[i]["checklistItemName"] == DBNull.Value ? string.Empty : detailItem[i]["checklistItemName"].ToString();
                                        //detail.floorName = ds.Tables[0].Rows[k]["FloorName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["FloorName"].ToString();
                                        detail.workOrderNumber = detailItem[i]["WorkOrderNo"] == DBNull.Value ? string.Empty : detailItem[i]["WorkOrderNo"].ToString();
                                        detail.plComments = detailItem[i]["Comments"] == DBNull.Value ? string.Empty : detailItem[i]["Comments"].ToString();
                                        detail.DueDate = detailItem[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["DueDate"]);
                                        detail.nextInspection = detailItem[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["NextInspection"]);
                                        detail.result = detailItem[i]["Result"] == DBNull.Value ? string.Empty : detailItem[i]["Result"].ToString();
                                        detail.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                        item.Details.Add(detail);
                                        #endregion

                                    }
                                    group.item.Add(item);
                                }
                            }
                            ////copy done
                            Unorderedgroups.Add(group);

                            //  header.groups.Add(group);
                        }
                        header.groups = Unorderedgroups.OrderBy(x => x.DisplayOrder1).ToList();
                        //  header.groups.OrderBy(x => x.DisplayOrder1).ToList();
                    }

                    headerlist.Add(header);
                }



                var result = headerlist.OrderBy(x => x.DisplayOrder).ToList();
                //  return Ok(headerlist);
                #region Excel
                if (result != null && result.Count > 0)
                {
                    string exportFilename = string.Empty;
                    exportFilename = "ChecklistReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
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
                #endregion
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private string ExportToExcel(List<ChecklistReportDTO> result, string exportFilename)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
            templateFileName = "Checklist_Template_Working.xlsx";
            string templateFilePath = templatePath + templateFileName;
            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateFilePath);
            int ProjectNumber = result.Select(x => x.IdJob).FirstOrDefault().Value;
            int sheetIndex = 0;
            if (sheetIndex == 0)
            {
                templateWorkbook.SetSheetName(sheetIndex, "Checklist");
                //   templateWorkbook.SetSheetName(sheetIndex, "Job #" + item);
                sheetIndex = sheetIndex + 1;
            }
            //else
            //{
            //    templateWorkbook.CloneSheet(0);
            //    templateWorkbook.SetSheetName(sheetIndex, "Job #" + item);
            //    sheetIndex = sheetIndex + 1;
            //}
            //  }

            //XSSFFont myFont = (XSSFFont)templateWorkbook.CreateFont();
            //myFont.FontHeightInPoints = (short)12;
            //myFont.FontName = "Red Hat Display";
            ////myFont.IsBold = false;
            XSSFFont myFont = (XSSFFont)templateWorkbook.CreateFont();
            myFont.FontHeightInPoints = (short)12;
            myFont.FontName = "Times New Roman";
            //myFont.FontName = "Red Hat Display";
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
            //  myFont.FontName = "Red Hat Display";
            myFont_Bold.FontName = "Times New Roman";
            myFont_Bold.IsBold = true;

            //XSSFFont myFont_HeaderBold = (XSSFFont)templateWorkbook.CreateFont();
            //myFont_HeaderBold.FontHeightInPoints = (short)14;
            ////  myFont.FontName = "Red Hat Display";
            //myFont_HeaderBold.FontName = "Times New Roman";
            //myFont_HeaderBold.IsBold = true;


            XSSFCellStyle right_JobDetailAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            right_JobDetailAlignCellStyle.SetFont(myFont);
            right_JobDetailAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.WrapText = true;
            right_JobDetailAlignCellStyle.VerticalAlignment = VerticalAlignment.Center;
            right_JobDetailAlignCellStyle.Alignment = HorizontalAlignment.Right;

            ISheet sheet = templateWorkbook.GetSheet("Checklist");

            Job job = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Company").Include("ProjectManager").FirstOrDefault(x => x.Id == ProjectNumber);

            #region Project Address

            string projectAddress = job != null
                 && job.RfpAddress != null
                 ? job.RfpAddress.HouseNumber + " " + (job.RfpAddress.Street != null ? job.RfpAddress.Street + ", " : string.Empty)
                   + (job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : string.Empty)
                : string.Empty;
            IRow iprojectAddressRow = sheet.GetRow(1);
            if (iprojectAddressRow == null)
            {
                iprojectAddressRow = sheet.CreateRow(1);

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

            #region ProjectNumber
            //   string specialPlaceName = job != null ? job.SpecialPlace : string.Empty;
            //    IRow iSpecialPlaceNameRow = sheet.GetRow(3);
            IRow iProjectNumberRow = sheet.GetRow(2);
            if (iProjectNumberRow == null)
            {
                iProjectNumberRow = sheet.CreateRow(2);
            }

            if (iProjectNumberRow.GetCell(1) != null)
            {
                iProjectNumberRow.GetCell(1).SetCellValue(ProjectNumber);
            }
            else
            {
                iProjectNumberRow.CreateCell(1).SetCellValue(ProjectNumber);
            }

            iProjectNumberRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            #endregion

            #region Client

            string clientName = job != null && job.Company != null ? job.Company.Name : string.Empty;

            IRow iClientRow = sheet.GetRow(3);
            if (iClientRow == null)
            {
                iClientRow = sheet.CreateRow(3);
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
            IRow iProjectManagerRow = sheet.GetRow(4);
            if (iProjectManagerRow == null)
            {
                iProjectManagerRow = sheet.CreateRow(4);
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

            #region Document

            string DocumentName = "Project Report";

            IRow iDocumentRow = sheet.GetRow(5);
            if (iDocumentRow == null)
            {
                iDocumentRow = sheet.CreateRow(5);
            }

            if (iDocumentRow.GetCell(1) != null)
            {
                iDocumentRow.GetCell(1).SetCellValue(DocumentName);
            }
            else
            {
                iDocumentRow.CreateCell(1).SetCellValue(DocumentName);
            }

            iDocumentRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;

            #endregion

            #region Report Date

            string reportDate = DateTime.Today.ToString(Common.ExportReportDateFormat);
            IRow iDateRow = sheet.GetRow(6);
            if (iDateRow == null)
            {
                iDateRow = sheet.CreateRow(6);
            }

            if (iDateRow.GetCell(1) != null)
            {
                iDateRow.GetCell(1).SetCellValue(reportDate);
            }
            else
            {
                iDateRow.CreateCell(1).SetCellValue(reportDate);
            }

            iDateRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;

            #endregion
            int index = 8;

            foreach (var c in result)
            {

                if (c != null)
                {
                    #region Checklist Report Header

                    XSSFFont ChecklistReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    ChecklistReportHeaderFont.FontHeightInPoints = (short)14;
                    ChecklistReportHeaderFont.FontName = "Times New Roman";
                    ChecklistReportHeaderFont.IsBold = true;

                    XSSFCellStyle ChecklistReportHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    ChecklistReportHeaderCellStyle.SetFont(ChecklistReportHeaderFont);
                    ChecklistReportHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    ChecklistReportHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    ChecklistReportHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    ChecklistReportHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    ChecklistReportHeaderCellStyle.WrapText = true;
                    ChecklistReportHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    //ChecklistReportHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                    ChecklistReportHeaderCellStyle.Alignment = HorizontalAlignment.Left;
                    ChecklistReportHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.White);
                    ChecklistReportHeaderCellStyle.FillPattern = FillPattern.SolidForeground;

                    string DOB_ReportHeader = string.Empty;
                    if (!string.IsNullOrEmpty(c.Others))
                    { DOB_ReportHeader = c.applicationName + " - " + c.Others; }
                    else
                    { DOB_ReportHeader = c.applicationName; }


                    IRow iDOB_ReportHeaderRow = sheet.GetRow(index);
                    if (iDOB_ReportHeaderRow == null)
                    {
                        iDOB_ReportHeaderRow = sheet.CreateRow(index);
                    }
                    iDOB_ReportHeaderRow.HeightInPoints = (float)19.50;

                    //if (c.groups.Select(x => x.checkListGroupType).ToList().Contains("PL"))
                    //{
                    //    if (iDOB_ReportHeaderRow.GetCell(0) != null)
                    //    {
                    //        iDOB_ReportHeaderRow.GetCell(0).SetCellValue("Plumbing Checklist:- "+DOB_ReportHeader);
                    //    }
                    //    else
                    //    {

                    //        iDOB_ReportHeaderRow.CreateCell(0).SetCellValue("Plumbing Checklist:- "+DOB_ReportHeader);
                    //    }
                    //}
                    // else
                    // {
                    if (iDOB_ReportHeaderRow.GetCell(0) != null)
                    {
                        iDOB_ReportHeaderRow.GetCell(0).SetCellValue(DOB_ReportHeader);
                    }
                    else
                    {

                        iDOB_ReportHeaderRow.CreateCell(0).SetCellValue(DOB_ReportHeader);
                    }
                    // }

                    CellRangeAddress DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 6);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    iDOB_ReportHeaderRow.GetCell(0).CellStyle = ChecklistReportHeaderCellStyle;

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    index = index + 1;
                    #endregion


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

                    foreach (var g in c.groups)
                    {

                        #region group header
                        XSSFCellStyle GroupHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                        GroupHeaderCellStyle.SetFont(ChecklistReportHeaderFont);
                        GroupHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                        GroupHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                        GroupHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                        GroupHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                        GroupHeaderCellStyle.WrapText = true;
                        GroupHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                        GroupHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                        GroupHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.White);
                        GroupHeaderCellStyle.FillPattern = FillPattern.SolidForeground;


                        string Group_ReportHeader = g.checkListGroupName;
                        IRow Group_ReportHeaderRow = sheet.GetRow(index);
                        if (Group_ReportHeaderRow == null)
                        {
                            Group_ReportHeaderRow = sheet.CreateRow(index);
                        }
                        Group_ReportHeaderRow.HeightInPoints = (float)19.50;

                        if (Group_ReportHeaderRow.GetCell(0) != null)
                        {
                            Group_ReportHeaderRow.GetCell(0).SetCellValue(Group_ReportHeader);
                        }
                        else
                        {
                            Group_ReportHeaderRow.CreateCell(0).SetCellValue(Group_ReportHeader);
                        }

                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 6);
                        sheet.AddMergedRegion(DOBCellRangeAddress);
                        Group_ReportHeaderRow.GetCell(0).CellStyle = GroupHeaderCellStyle;

                        RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        index = index + 1;
                        #endregion
                        #region Column Header cell style
                        XSSFCellStyle ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                        ColumnHeaderCellStyle.SetFont(ChecklistReportHeaderFont);
                        ColumnHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                        ColumnHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                        ColumnHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                        ColumnHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                        ColumnHeaderCellStyle.WrapText = true;
                        ColumnHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                        ColumnHeaderCellStyle.Alignment = HorizontalAlignment.Center;

                        ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                        ColumnHeaderCellStyle.FillPattern = FillPattern.SolidForeground;
                        #endregion
                        if (g.checkListGroupType.ToLower().Trim() == "general")
                        {

                            #region header

                            IRow iDOB_HeaderRow = sheet.GetRow(index);
                            //  var x = iDOB_HeaderRow.GetCell(0).CellStyle.FillBackgroundColorColor;
                            //int color_n = System.Convert.ToInt32((sheet.Cell[9,0].Interior.Color);
                            //Color color = System.Drawing.ColorTranslator.FromOle(color_n);
                            //  var d = iDOB_HeaderRow.GetCell(0).CellStyle.FillBackgroundColorColor;

                            if (iDOB_HeaderRow == null)
                            {
                                iDOB_HeaderRow = sheet.CreateRow(index);
                            }
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                            sheet.AddMergedRegion(DOBCellRangeAddress);
                            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            if (iDOB_HeaderRow.GetCell(0) != null)
                            {

                                iDOB_HeaderRow.GetCell(0).SetCellValue("Item Name");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(0).SetCellValue("Item Name");
                            }


                            if (iDOB_HeaderRow.GetCell(2) != null)
                            {
                                iDOB_HeaderRow.GetCell(2).SetCellValue("Party Responsible");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(2).SetCellValue("Party Responsible");
                            }
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 3, 4);
                            sheet.AddMergedRegion(DOBCellRangeAddress);
                            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            if (iDOB_HeaderRow.GetCell(3) != null)
                            {
                                iDOB_HeaderRow.GetCell(3).SetCellValue("Comments");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(3).SetCellValue("Comments");
                            }

                            if (iDOB_HeaderRow.GetCell(5) != null)
                            {
                                iDOB_HeaderRow.GetCell(5).SetCellValue("Target Date");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(5).SetCellValue("Target Date");
                            }

                            if (iDOB_HeaderRow.GetCell(6) != null)
                            {
                                iDOB_HeaderRow.GetCell(6).SetCellValue("Status");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(6).SetCellValue("Status");
                            }

                            iDOB_HeaderRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(1).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(6).CellStyle = ColumnHeaderCellStyle;

                            index = index + 1;
                            #endregion
                            #region Data
                            foreach (var item in g.item)
                            {
                                IRow iRow = sheet.GetRow(index);


                                if (iRow == null)
                                {
                                    iRow = sheet.CreateRow(index);
                                }
                                if (iRow.GetCell(0) != null)
                                {
                                    iRow.GetCell(0).SetCellValue(item.checklistItemName);
                                }
                                else
                                {
                                    iRow.CreateCell(0).SetCellValue(item.checklistItemName);
                                    // iRow.CreateCell(1);
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                //DOBCellRangeAddress = new CellRangeAddress(index, index, 2, 3);
                                //sheet.AddMergedRegion(DOBCellRangeAddress);
                                //RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                //RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                //RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                //RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iRow.GetCell(2) != null)
                                {
                                    if (item.PartyResponsible.ToLower() == "rpo user")
                                        iRow.GetCell(2).SetCellValue("RPO");

                                    else if (item.PartyResponsible.ToLower() == "contact")
                                    {
                                        if (item.CompanyName != null)
                                            iRow.GetCell(2).SetCellValue(item.ContactName + " - " + item.CompanyName);
                                        else
                                            iRow.GetCell(2).SetCellValue(item.ContactName);
                                    }
                                    else if (item.PartyResponsible.ToLower() == "other")
                                        iRow.GetCell(2).SetCellValue(item.manualPartyResponsible);
                                    else
                                        iRow.GetCell(2).SetCellValue("");
                                }
                                else
                                {
                                    if (item.PartyResponsible.ToLower() == "rpo user")
                                        iRow.CreateCell(2).SetCellValue("RPO");

                                    else if (item.PartyResponsible.ToLower() == "contact")
                                    {
                                        if (item.CompanyName != null)
                                            iRow.CreateCell(2).SetCellValue(item.ContactName + " - " + item.CompanyName);
                                        else
                                            iRow.CreateCell(2).SetCellValue(item.ContactName);
                                    }
                                    else if (item.PartyResponsible.ToLower() == "other")
                                        iRow.CreateCell(2).SetCellValue(item.manualPartyResponsible);
                                    else
                                        iRow.CreateCell(2).SetCellValue("");
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 3, 4);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iRow.GetCell(3) != null)
                                {
                                    iRow.GetCell(3).SetCellValue(item.comments);
                                }
                                else
                                {
                                    iRow.CreateCell(3).SetCellValue(item.comments);
                                }

                                // string workDescription = item.JobWorkTypeDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);

                                if (iRow.GetCell(5) != null)
                                {
                                    iRow.GetCell(5).SetCellValue(item.dueDate != null ? Convert.ToDateTime(item.dueDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                                }
                                else
                                {
                                    iRow.CreateCell(5).SetCellValue(item.dueDate != null ? Convert.ToDateTime(item.dueDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                                }
                                //Temporary commented
                                if (iRow.GetCell(6) != null)
                                {
                                    if (item.status == 1)
                                        iRow.GetCell(6).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.GetCell(6).SetCellValue("InProcess");
                                    else if (item.status == 3)
                                        iRow.GetCell(6).SetCellValue("Completed");
                                    else
                                        iRow.GetCell(6).SetCellValue("");
                                }
                                else
                                {
                                    if (item.status == 1)
                                        iRow.CreateCell(6).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.CreateCell(6).SetCellValue("InProcess");
                                    else if (item.status == 3)
                                        iRow.CreateCell(6).SetCellValue("Completed");
                                    else
                                        iRow.CreateCell(6).SetCellValue("");
                                }

                                iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(6).CellStyle = leftAlignCellStyle;

                                index = index + 1;
                            }
                            #endregion
                        }
                        else if (g.checkListGroupType.ToLower().Trim() == "tr")
                        {
                            IRow iDOB_HeaderRow = sheet.GetRow(index);

                            if (iDOB_HeaderRow == null)
                            {
                                iDOB_HeaderRow = sheet.CreateRow(index);
                            }
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                            sheet.AddMergedRegion(DOBCellRangeAddress);
                            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            if (iDOB_HeaderRow.GetCell(0) != null)
                            {
                                iDOB_HeaderRow.GetCell(0).SetCellValue("Item Name");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(0).SetCellValue("Item Name");
                            }

                            if (iDOB_HeaderRow.GetCell(2) != null)
                            {
                                iDOB_HeaderRow.GetCell(2).SetCellValue("Design Applicant");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(2).SetCellValue("Design Applicant");
                            }

                            if (iDOB_HeaderRow.GetCell(3) != null)
                            {
                                iDOB_HeaderRow.GetCell(3).SetCellValue("Inspector");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(3).SetCellValue("Inspector");
                            }
                            if (iDOB_HeaderRow.GetCell(4) != null)
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue("Comments");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("Comments");
                            }
                            if (iDOB_HeaderRow.GetCell(5) != null)
                            {
                                iDOB_HeaderRow.GetCell(5).SetCellValue("Stage");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(5).SetCellValue("Stage");
                            }

                            if (iDOB_HeaderRow.GetCell(6) != null)
                            {
                                iDOB_HeaderRow.GetCell(6).SetCellValue("Status");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(6).SetCellValue("Status");
                            }

                            iDOB_HeaderRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(1).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(6).CellStyle = ColumnHeaderCellStyle;

                            index = index + 1;
                            #region Data
                            foreach (var item in g.item)
                            {
                                IRow iRow = sheet.GetRow(index);
                                if (iRow == null)
                                {
                                    iRow = sheet.CreateRow(index);
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iRow.GetCell(0) != null)
                                {
                                    iRow.GetCell(0).SetCellValue(item.checklistItemName);
                                }
                                else
                                {
                                    iRow.CreateCell(0).SetCellValue(item.checklistItemName);
                                }
                                if (iRow.GetCell(2) != null)
                                {
                                    // if (item.PartyResponsible.ToLower() == "rpo user")
                                    iRow.GetCell(2).SetCellValue(item.DesignApplicantName);

                                    //if (item.PartyResponsible.ToLower() == "contact")
                                    //    iRow.GetCell(1).SetCellValue(item.ContactName);
                                    //if (item.PartyResponsible.ToLower() == "other")
                                    //    iRow.GetCell(1).SetCellValue(item.manualPartyResponsible);
                                }
                                else
                                {
                                    iRow.CreateCell(2).SetCellValue(item.DesignApplicantName);
                                }

                                if (iRow.GetCell(3) != null)
                                {
                                    iRow.GetCell(3).SetCellValue(item.InspectorName);
                                }
                                else
                                {
                                    iRow.CreateCell(3).SetCellValue(item.InspectorName);
                                }
                                if (iRow.GetCell(4) != null)
                                {
                                    iRow.GetCell(4).SetCellValue(item.comments);
                                }
                                else
                                {
                                    iRow.CreateCell(4).SetCellValue(item.comments);
                                }

                                // string workDescription = item.JobWorkTypeDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);

                                if (iRow.GetCell(5) != null)
                                {
                                    //iRow.GetCell(3).SetCellValue(item.stage);
                                    if (item.stage == "1")
                                        iRow.GetCell(5).SetCellValue("Prior to Approval");
                                    else if (item.stage == "2")
                                        iRow.GetCell(5).SetCellValue("Prior to Permit");
                                    else if (item.stage == "3")
                                        iRow.GetCell(5).SetCellValue("Prior to Sign Off");
                                    else
                                        iRow.GetCell(5).SetCellValue("");
                                }
                                else
                                {
                                    if (item.stage == "1")
                                        iRow.CreateCell(5).SetCellValue("Prior to Approval");
                                    else if (item.stage == "2")
                                        iRow.CreateCell(5).SetCellValue("Prior to Permit");
                                    else if (item.stage == "3")
                                        iRow.CreateCell(5).SetCellValue("Prior to Sign Off");
                                    else
                                        iRow.CreateCell(5).SetCellValue("");
                                }
                                //Temporary commented
                                if (iRow.GetCell(6) != null)
                                {
                                    //iRow.GetCell(4).SetCellValue(item.status);
                                    if (item.status == 1)
                                        iRow.GetCell(6).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.GetCell(6).SetCellValue("Completed");
                                    else
                                        iRow.GetCell(6).SetCellValue("");
                                }
                                else
                                {
                                    if (item.status == 1)
                                        iRow.CreateCell(6).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.CreateCell(6).SetCellValue("Completed");
                                    else
                                        iRow.CreateCell(6).SetCellValue("");
                                }

                                iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                // if (iRow.GetCell(3) != null)
                                iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                //  if (iRow.GetCell(4) != null)
                                iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(6).CellStyle = leftAlignCellStyle;

                                index = index + 1;
                            }
                            #endregion
                        }
                        else if (g.checkListGroupType.ToLower().Trim() == "pl")
                        {
                            #region floor header
                            //index = index + 1;
                            foreach (var item in g.item)
                            {
                                IRow iDOB_FloorRow = sheet.GetRow(index);
                                if (iDOB_FloorRow == null)
                                {
                                    iDOB_FloorRow = sheet.CreateRow(index);
                                }

                                if (iDOB_FloorRow.GetCell(0) != null)
                                {
                                    iDOB_FloorRow.GetCell(0).SetCellValue("Floor - " + item.FloorNumber);
                                }
                                else
                                {
                                    iDOB_FloorRow.CreateCell(0).SetCellValue("Floor - " + item.FloorNumber);
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 6);
                                sheet.AddMergedRegion(DOBCellRangeAddress);


                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                iDOB_FloorRow.GetCell(0).CellStyle = GroupHeaderCellStyle;
                                index = index + 1;
                                IRow iDOB_HeaderRow = sheet.GetRow(index);

                                if (iDOB_HeaderRow == null)
                                {
                                    iDOB_HeaderRow = sheet.CreateRow(index);
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iDOB_HeaderRow.GetCell(0) != null)
                                {
                                    iDOB_HeaderRow.GetCell(0).SetCellValue("Inspection Type");
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(0).SetCellValue("Inspection Type");
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 2, 4);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                                if (iDOB_HeaderRow.GetCell(2) != null)
                                {
                                    iDOB_HeaderRow.GetCell(2).SetCellValue("Comment");
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(2).SetCellValue("Comment");
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iDOB_HeaderRow.GetCell(5) != null)
                                {
                                    iDOB_HeaderRow.GetCell(5).SetCellValue("Status");
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(5).SetCellValue("Status");
                                }
                                iDOB_HeaderRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(1).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                                index = index + 1;
                                foreach (var d in item.Details)
                                {
                                    #region Data                                   
                                    IRow iRow = sheet.GetRow(index);
                                    if (iRow == null)
                                    {
                                        iRow = sheet.CreateRow(index);
                                    }
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                                    sheet.AddMergedRegion(DOBCellRangeAddress);
                                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    if (iRow.GetCell(0) != null)
                                    {
                                        iRow.GetCell(0).SetCellValue(d.inspectionPermit);
                                    }
                                    else
                                    {
                                        iRow.CreateCell(0).SetCellValue(d.inspectionPermit);
                                    }
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 2, 4);
                                    sheet.AddMergedRegion(DOBCellRangeAddress);
                                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    if (iRow.GetCell(2) != null)
                                    {
                                        iRow.GetCell(2).SetCellValue(d.plComments);

                                    }
                                    else
                                    {
                                        iRow.CreateCell(2).SetCellValue(d.plComments);
                                    }

                                    if (iRow.GetCell(5) != null)
                                    {
                                        if (d.result == "1")
                                            iRow.GetCell(5).SetCellValue("Pass");
                                        else if (d.result == "2")
                                            iRow.GetCell(5).SetCellValue("Failed");
                                        else if (d.result == "3")
                                            iRow.GetCell(5).SetCellValue("Pending");
                                        else if (d.result == "4")
                                            iRow.GetCell(5).SetCellValue("NA");
                                        else
                                            iRow.GetCell(5).SetCellValue("");
                                        //iRow.GetCell(4).SetCellValue(item.status);                                  

                                    }
                                    else
                                    {
                                        if (d.result == "1")
                                            iRow.CreateCell(5).SetCellValue("Pass");
                                        else if (d.result == "2")
                                            iRow.CreateCell(5).SetCellValue("Failed");
                                        else if (d.result == "3")
                                            iRow.CreateCell(5).SetCellValue("Pending");
                                        else if (d.result == "4")
                                            iRow.CreateCell(5).SetCellValue("NA");
                                        else
                                            iRow.CreateCell(5).SetCellValue("");
                                    }
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                                    sheet.AddMergedRegion(DOBCellRangeAddress);
                                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    //iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                    //iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                    //iRow.GetCell(3).CellStyle = leftAlignCellStyle;

                                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                                    //iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                    // iRow.GetCell(4).CellStyle = leftAlignCellStyle;

                                    index = index + 1;
                                }
                                #endregion
                            }
                        }




                    }

                }
                index = index + 1;

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
        /// Get the report of Checklist Report with filter and sorting  and export to PDF
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>

        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/Checklist/ExportChecklistToPDF")]
        public IHttpActionResult ExportChecklistToPDF(ChecklistReportDatatableParameter dataTableParameters)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            {
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[3];

                spParameter[0] = new SqlParameter("@IdJobCheckListHeader", SqlDbType.NVarChar);
                spParameter[0].Direction = ParameterDirection.Input;
                var lstheaders = dataTableParameters.lstexportChecklist.Select(x => x.jobChecklistHeaderId).ToList();
                string headerid = null;
                foreach (var l in lstheaders)
                {
                    headerid += l + ",";
                }
                //  headerid.Remove(headerid.Length - 1, 1);
                spParameter[0].Value = headerid.Remove(headerid.Length - 1, 1);


                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.Displayorder;
                // spParameter[1].Value = dataTableParameters.lstexportChecklist.Select(x=>x.Displayorder);

                spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = string.Empty;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "ChecklistView", spParameter);
                List<CheckListApplicationDropDown.Models.ChecklistReportDTO> headerlist = new List<ChecklistReportDTO>();

                List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
                            //Name = row.Field<string>("NAME")
                        }).Select(g => g.CopyToDataTable()).ToList();
                // .CopyToDataTable();

                foreach (var loopheader in distinctheaders)
                {
                    ChecklistReportDTO header = new ChecklistReportDTO();
                    List<ReportChecklistGroup> Unorderedgroups = new List<ReportChecklistGroup>();
                    Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                    header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                    header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                    header.applicationName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                    header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                    header.groups = new List<ReportChecklistGroup>();
                    header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["Idjob"]);
                    var checklist = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId).FirstOrDefault();
                    header.DisplayOrder = checklist.Displayorder;
                    //  header.DisplayOrder = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId);
                    // .Select(x => x.jobChecklistHeaderId).ToList();
                    var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                        .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();

                    // var headersgroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId).ToList();
                    var payloadgroup = checklist.lstExportChecklistGroup.Select(x => x.jobChecklistGroupId).ToList();
                    foreach (var eachGroup in distinctGroup)
                    // foreach (var eachGroup in payloadgroup)
                    {
                        if (payloadgroup.Contains(Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"])))
                        {
                            ReportChecklistGroup group = new ReportChecklistGroup();
                            group.item = new List<ReportItem>();
                            //if (Convert.ToInt32(ds.Tables[0].Rows[i]["JobChecklistHeaderId"]) == checklistHeaderId)
                            {
                                Int32 IdJobChecklistGroup = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                group.checkListGroupName = eachGroup.Rows[0]["ChecklistGroupName"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["ChecklistGroupName"].ToString();
                                group.jobChecklistGroupId = eachGroup.Rows[0]["IdJobChecklistGroup"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                group.checkListGroupType = eachGroup.Rows[0]["checklistGroupType"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["checklistGroupType"].ToString();
                                // group.DisplayOrder1 = eachGroup.Rows[0]["DisplayOrder1"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["DisplayOrder1"]);
                                group.DisplayOrder1 = checklist.lstExportChecklistGroup.Where(y => y.jobChecklistGroupId == IdJobChecklistGroup).Select(x => x.displayOrder1).FirstOrDefault();
                                //var GroupInPayload = dataTableParameters.lstexportChecklist.Where(x => x.jobChecklistHeaderId == checklistHeaderId).Select(z => z.lstExportChecklistGroup).FirstOrDefault();

                                //foreach (var gp in GroupInPayload)
                                //{
                                //    if (gp.jobChecklistGroupId == IdJobChecklistGroup)
                                //    {
                                //        group.DisplayOrder1 = gp.displayOrder1;
                                //    }
                                //}

                                if (eachGroup.Rows[0]["checklistGroupType"].ToString().ToLower().TrimEnd() != "pl")
                                {
                                    var groupsitems = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
                                    //for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                    for (int j = 0; j < groupsitems.Count; j++)
                                    {
                                        //if (Convert.ToInt32(ds.Tables[0].Rows[j]["IdJobChecklistGroup"]) == IdJobChecklistGroup && Convert.ToInt32(ds.Tables[0].Rows[j]["JobChecklistHeaderId"]) == checklistHeaderId)
                                        //{

                                        ReportItem item = new ReportItem();
                                        item.Details = new List<ReportDetails>();

                                        #region Items
                                        Int32 IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                        if (IdChecklistItem != 0)
                                        {
                                            item.checklistItemName = groupsitems[j]["checklistItemName"] == DBNull.Value ? string.Empty : groupsitems[j]["checklistItemName"].ToString();
                                            item.IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                            item.jocbChecklistItemDetailsId = groupsitems[j]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["JocbChecklistItemDetailsId"]);
                                            int itemid = Convert.ToInt32(item.jocbChecklistItemDetailsId);
                                            item.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobchecklistItemDetails == itemid);
                                            item.comments = groupsitems[j]["Comments"] == DBNull.Value ? string.Empty : groupsitems[j]["Comments"].ToString();
                                            item.idDesignApplicant = groupsitems[j]["IdDesignApplicant"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdDesignApplicant"]);
                                            item.idInspector = groupsitems[j]["IdInspector"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdInspector"]);
                                            item.stage = groupsitems[j]["Stage"] == DBNull.Value ? string.Empty : groupsitems[j]["Stage"].ToString();
                                            item.partyResponsible1 = groupsitems[j]["PartyResponsible1"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["PartyResponsible1"]);
                                            item.manualPartyResponsible = groupsitems[j]["ManualPartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["ManualPartyResponsible"].ToString();
                                            item.idContact = groupsitems[j]["IdContact"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdContact"]);
                                            item.referenceDocumentId = groupsitems[j]["IdReferenceDocument"] == DBNull.Value ? string.Empty : groupsitems[j]["IdReferenceDocument"].ToString();
                                            item.dueDate = groupsitems[j]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["DueDate"]);
                                            item.status = groupsitems[j]["Status"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Status"]);
                                            item.checkListLastModifiedDate = groupsitems[j]["checklistItemLastDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["checklistItemLastDate"]);
                                            item.idCreateFormDocument = groupsitems[j]["IdCreateFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCreateFormDocument"]);
                                            item.idUploadFormDocument = groupsitems[j]["IdUploadFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdUploadFormDocument"]);
                                            item.Displayorder = groupsitems[j]["Displayorder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Displayorder"]);
                                            item.ChecklistDetailDisplayOrder = groupsitems[j]["ChecklistDetailDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["ChecklistDetailDisplayOrder"]);
                                            item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                            if (item.idContact != null && item.idContact != 0)
                                            {
                                                var c = rpoContext.Contacts.Find(item.idContact);
                                                item.ContactName = c.FirstName + " " + c.LastName;
                                                if (c.IdCompany != null && c.IdCompany != 0)
                                                {
                                                    item.CompanyName = rpoContext.Companies.Where(x => x.Id == c.IdCompany).Select(y => y.Name).FirstOrDefault();
                                                }
                                            }

                                            if (item.idDesignApplicant != null && item.idDesignApplicant != 0)
                                            {
                                                //var c = rpoContext.Contacts.Find(item.idDesignApplicant);
                                                var c = rpoContext.JobContacts.Find(item.idDesignApplicant);
                                                if (c != null)
                                                {
                                                    item.DesignApplicantName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                                }
                                            }
                                            if (item.idInspector != null && item.idInspector != 0)
                                            {
                                                //var c = rpoContext.Contacts.Find(item.idInspector);
                                                var c = rpoContext.JobContacts.Find(item.idInspector);
                                                if (c != null) {
                                                item.InspectorName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                                }

                                            }
                                            #endregion

                                            group.item.Add(item);
                                        }
                                    }
                                }
                                else
                                {
                                    var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();

                                    var PlumbingCheckListFloors = groupsitems2.AsEnumerable().Select(a => a.Field<Int32?>("IdJobPlumbingCheckListFloors")).Distinct().ToList();

                                    for (int j = 0; j < PlumbingCheckListFloors.Count(); j++)
                                    {

                                        ReportItem item = new ReportItem();
                                        item.Details = new List<ReportDetails>();
                                        var IdJobPlumbingCheckListFloors1 = PlumbingCheckListFloors[j];
                                        var detailItem = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobPlumbingCheckListFloors") == IdJobPlumbingCheckListFloors1).ToList();
                                        #region Items
                                        item.FloorNumber = detailItem[0]["FloorNumber"] == DBNull.Value ? string.Empty : detailItem[0]["FloorNumber"].ToString();
                                        item.idJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors1 != 0 ? IdJobPlumbingCheckListFloors1 : 0;
                                        #endregion

                                        for (int i = 0; i < detailItem.Count; i++)
                                        {
                                            ReportDetails detail = new ReportDetails();
                                            #region Items details
                                            detail.checklistGroupType = detailItem[i]["checklistGroupType"] == DBNull.Value ? string.Empty : detailItem[i]["checklistGroupType"].ToString();
                                            detail.idJobPlumbingInspection = detailItem[i]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingInspection"]);
                                            detail.idJobPlumbingCheckListFloors = detailItem[i]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingCheckListFloors"]);
                                            detail.inspectionPermit = detailItem[i]["checklistItemName"] == DBNull.Value ? string.Empty : detailItem[i]["checklistItemName"].ToString();
                                            //detail.floorName = ds.Tables[0].Rows[k]["FloorName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["FloorName"].ToString();
                                            detail.workOrderNumber = detailItem[i]["WorkOrderNo"] == DBNull.Value ? string.Empty : detailItem[i]["WorkOrderNo"].ToString();
                                            detail.plComments = detailItem[i]["Comments"] == DBNull.Value ? string.Empty : detailItem[i]["Comments"].ToString();
                                            detail.DueDate = detailItem[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["DueDate"]);
                                            detail.nextInspection = detailItem[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["NextInspection"]);
                                            detail.result = detailItem[i]["Result"] == DBNull.Value ? string.Empty : detailItem[i]["Result"].ToString();
                                            detail.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                            item.Details.Add(detail);
                                            #endregion

                                        }
                                        group.item.Add(item);
                                    }
                                }
                            }
                            ////copy done
                            Unorderedgroups.Add(group);

                            //  header.groups.Add(group);
                        }
                        header.groups = Unorderedgroups.OrderBy(x => x.DisplayOrder1).ToList();
                        //  header.groups.OrderBy(x => x.DisplayOrder1).ToList();
                    }

                    headerlist.Add(header);
                }



                var result = headerlist.OrderBy(x => x.DisplayOrder).ToList();
                //  return Ok(headerlist);
                #region PDF
                if (result != null && result.Count > 0)
                {
                    string exportFilename = string.Empty;
                    string hearingDate = string.Empty;
                    exportFilename = "GeneralChecklistReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
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
                    throw new RpoBusinessException(StaticMessages.NoRecordFoundMessage);
                }
                #endregion
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        private string ExportToPdf(string hearingDate, List<ChecklistReportDTO> result, string exportFilename)
        {

            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);
            int ProjectNumber = result.Select(x => x.IdJob).FirstOrDefault().Value;

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

                //string fontBlack = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\RedHatDisplay-Black.ttf");
                //string fontRegular = HttpContext.Current.Server.MapPath("~\\HTML\\fonts\\RedHatDisplay-Regular.ttf");
                //BaseFont customfontBlack = BaseFont.CreateFont(fontBlack, BaseFont.CP1252, BaseFont.EMBEDDED);
                //BaseFont customfontRegular = BaseFont.CreateFont(fontRegular, BaseFont.CP1252, BaseFont.EMBEDDED);
                //Font font_Table_Header = new Font(customfontRegular,9,1);
                //Font font_Table_Data = new Font(customfontRegular, 9);

                //Font font_8_Normal = new Font(customfontRegular, 8);
                //Font font_16_Bold = new Font(customfontRegular, 16,1);
                //Font font_10_Normal = new Font(customfontRegular, 10);
                //Font font_10_Bold = new Font(customfontRegular, 10, 1);
                //Font font_12_Bold = new Font(customfontRegular, 12, 1);

                Font font_10_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 0);
                Font font_12_Normal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 0);

                Font font_10_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1);
                Font font_12_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, 1);
                Font font_16_Bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 16, 1);

                Font font_Table_Header = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1);
                Font font_Table_Data = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 0);

                // Phrase phrase = new Phrase();
                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SplitLate = false;
                Job job = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Company").Include("ProjectManager").FirstOrDefault(x => x.Id == ProjectNumber);

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
                cell.PaddingLeft = -65;
                cell.Colspan = 6;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -65;
                cell.Colspan = 6;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = -10;
                table.AddCell(cell);

                #region Project Address
                string projectAddress = job != null
                && job.RfpAddress != null
                ? job.RfpAddress.HouseNumber + " " + (job.RfpAddress.Street != null ? job.RfpAddress.Street + ", " : string.Empty)
                  + (job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : string.Empty)
               : string.Empty;


                //var Address = new Phrase();
                //Address.Add(new Chunk("Project Address:        ", font_12_Bold));
                //Address.Add(new Chunk(projectAddress, font_12_Normal));

                //cell = new PdfPCell(new Phrase(Address));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 6;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project Address: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(projectAddress, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                #endregion
                #region ProjectNumber

                //var ProjectNo = new Phrase();
                //ProjectNo.Add(new Chunk("RPO Project Number: ", font_10_Bold));
                //ProjectNo.Add(new Chunk(Convert.ToString(ProjectNumber), font_10_Bold));

                //cell = new PdfPCell(new Phrase(ProjectNo));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 6;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //table.AddCell(cell);
                cell = new PdfPCell(new Phrase("RPO Project Number: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(ProjectNumber), font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Client
                string clientName = job != null && job.Company != null ? job.Company.Name : string.Empty;
                //var CompanyName = new Phrase();
                //CompanyName.Add(new Chunk("Client: ", font_10_Bold));
                //CompanyName.Add(new Chunk(clientName, font_10_Bold));

                //cell = new PdfPCell(new Phrase(CompanyName));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 6;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Client: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(clientName, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Project Manager
                string ProjectManagerName = job.ProjectManager != null ? job.ProjectManager.FirstName + " " + job.ProjectManager.LastName + " | " + job.ProjectManager.Email : string.Empty;

                //var ProjectManager = new Phrase();
                //ProjectManager.Add(new Chunk("Project Manager: ", font_10_Bold));
                //ProjectManager.Add(new Chunk(ProjectManagerName, font_10_Bold));

                //cell = new PdfPCell(new Phrase(ProjectManager));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 6;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Project Manager: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(ProjectManagerName, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Document
                //string DocumentName = "Project Report";
                //var Document = new Phrase();
                //Document.Add(new Chunk("Document: ", font_10_Bold));
                //Document.Add(new Chunk(DocumentName, font_10_Bold));

                //cell = new PdfPCell(new Phrase(Document));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 6;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Document: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project Report", font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Report Date
                string reportDate = DateTime.Today.ToString(Common.ExportReportDateFormat);
                //var ReportOn = new Phrase();
                //ReportOn.Add(new Chunk("Report Generated on: ", font_10_Bold));
                //ReportOn.Add(new Chunk(reportDate, font_10_Bold));

                //cell = new PdfPCell(new Phrase(ReportOn));
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = PdfPCell.NO_BORDER;
                //cell.Colspan = 6;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.PaddingBottom = 10;
                //table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Report Generated on: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 5;
                cell.PaddingBottom = 5;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion

                foreach (var c in result)
                {

                    if (c != null)
                    {
                        #region Checklist Report Header
                        
                        string DOB_ReportHeader = string.Empty;
                        if (!string.IsNullOrEmpty(c.Others))
                        { DOB_ReportHeader = c.applicationName + " - " + c.Others; }
                        else
                        { DOB_ReportHeader = c.applicationName; }
                        
                        cell = new PdfPCell(new Phrase(DOB_ReportHeader, font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.TOP_BORDER;
                        cell.Colspan = 6;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.PaddingTop = 5;
                        cell.PaddingBottom = 10;
                        table.AddCell(cell);
                        #endregion
                        foreach (var g in c.groups)
                        {
                            #region group header
                            string Group_ReportHeader = g.checkListGroupName;
                            cell = new PdfPCell(new Phrase(Group_ReportHeader, font_12_Bold));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.Border = PdfPCell.TOP_BORDER;
                            cell.Colspan = 6;
                            cell.PaddingBottom = 5;
                            table.AddCell(cell);
                            #endregion
                            if (g.checkListGroupType.ToLower().Trim() == "general")
                            {
                                #region header
                                cell = new PdfPCell(new Phrase("Item Name", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 2;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Party Responsible", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Comment", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Target Date", font_Table_Header));
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
                                #endregion
                                #region Data
                                foreach (var item in g.item)
                                {
                                    cell = new PdfPCell(new Phrase(item.checklistItemName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    table.AddCell(cell);

                                    string PartyResponsible = string.Empty;
                                    if (item.PartyResponsible.ToLower() == "rpo user")
                                        PartyResponsible = "RPO";

                                    else if (item.PartyResponsible.ToLower() == "contact")
                                    {
                                        if (item.CompanyName != null)
                                            PartyResponsible = item.ContactName + " - " + item.CompanyName;
                                        else
                                            PartyResponsible = item.ContactName;
                                    }
                                    else if (item.PartyResponsible.ToLower() == "other")
                                        PartyResponsible = item.manualPartyResponsible;
                                    else
                                        PartyResponsible = "";

                                    cell = new PdfPCell(new Phrase(PartyResponsible, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.comments, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.dueDate != null ? Convert.ToDateTime(item.dueDate).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    table.AddCell(cell);

                                    string Status = string.Empty;

                                    if (item.status == 1)
                                        Status = "Open";
                                    else if (item.status == 2)
                                        Status = "InProcess";
                                    else if (item.status == 3)
                                        Status = "Completed";
                                    else
                                        Status = "";

                                    cell = new PdfPCell(new Phrase(Status, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    table.AddCell(cell);

                                }
                                #endregion
                            }
                            else if (g.checkListGroupType.ToLower().Trim() == "tr")
                            {
                                #region header
                                cell = new PdfPCell(new Phrase("Item Name", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Design Applicant", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Inspector", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Comment", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Stage", font_Table_Header));
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
                                #endregion
                                #region Data
                                foreach (var item in g.item)
                                {
                                    cell = new PdfPCell(new Phrase(item.checklistItemName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.DesignApplicantName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.InspectorName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.comments, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    string Stage = string.Empty;
                                    if (item.stage == "1")
                                        Stage = "Prior to Approval";

                                    else if (item.stage == "2")
                                        Stage = "Prior to Permit";

                                    else if (item.stage == "3")
                                        Stage = "Prior to Sign Off";
                                    else
                                        Stage = "";
                                    cell = new PdfPCell(new Phrase(Stage, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    string Status = string.Empty;

                                    if (item.status == 1)
                                        Status = "Open";
                                    else if (item.status == 2)
                                        Status = "Completed";
                                    else
                                        Status = "";

                                    cell = new PdfPCell(new Phrase(Status, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);
                                }
                                #endregion
                            }
                            else if (g.checkListGroupType.ToLower().Trim() == "pl")
                            {
                                #region floor header
                                foreach (var item in g.item)
                                {

                                    cell = new PdfPCell(new Phrase("Floor - " + item.FloorNumber, font_12_Bold));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cell.Border = PdfPCell.TOP_BORDER;
                                    cell.Colspan = 6;
                                    cell.PaddingBottom = 5;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Inspection Type", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Comment", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Status", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);
                                    foreach (var d in item.Details)
                                    {
                                        #region Data
                                        cell = new PdfPCell(new Phrase(d.inspectionPermit, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 2;
                                        table.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(d.plComments, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 2;
                                        table.AddCell(cell);
                                        string PLStatus = string.Empty;
                                        if (d.result == "1")
                                            PLStatus = "Pass";
                                        else if (d.result == "2")
                                            PLStatus = "Failed";
                                        else if (d.result == "3")
                                            PLStatus = "Pending";
                                        else if (d.result == "4")
                                            PLStatus = "NA";
                                        else
                                            PLStatus = "";
                                        cell = new PdfPCell(new Phrase(PLStatus, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 2;
                                        table.AddCell(cell);
                                        #endregion

                                    }

                                }
                                #endregion
                            }
                        }
                    }
                }

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }

        /// <summary>
        ///  Get the report of ReportOverallPermitExpiry Report with filter and sorting  and export to excel
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        //[Authorize]
        //[RpoAuthorize]
        //[HttpPost]
        //[Route("api/Checklist/ExportChecklistToExcelForCustommer")]
        //public IHttpActionResult PostExportChecklistToExcelForCustomer(ChecklistReportDatatableParameter dataTableParameters)
        //{
        //    var employee = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
        //  //  if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
        //  //  {
        //        string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
        //        string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

        //        DataSet ds = new DataSet();
        //        SqlParameter[] spParameter = new SqlParameter[3];

        //        spParameter[0] = new SqlParameter("@IdJobCheckListHeader", SqlDbType.NVarChar);
        //        spParameter[0].Direction = ParameterDirection.Input;
        //        var lstheaders = dataTableParameters.lstexportChecklist.Select(x => x.jobChecklistHeaderId).ToList();
        //        string headerid = null;
        //        foreach (var l in lstheaders)
        //        {
        //            headerid += l + ",";
        //        }
        //        //  headerid.Remove(headerid.Length - 1, 1);
        //        spParameter[0].Value = headerid.Remove(headerid.Length - 1, 1);


        //        spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
        //        spParameter[1].Direction = ParameterDirection.Input;
        //        spParameter[1].Value = dataTableParameters.Displayorder;
        //        // spParameter[1].Value = dataTableParameters.lstexportChecklist.Select(x=>x.Displayorder);

        //        spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
        //        spParameter[2].Direction = ParameterDirection.Input;
        //        spParameter[2].Value = string.Empty;

        //        ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "ChecklistView", spParameter);
        //        List<CheckListApplicationDropDown.Models.ChecklistReportDTO> headerlist = new List<ChecklistReportDTO>();

        //        List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
        //                .GroupBy(row => new
        //                {
        //                    JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
        //                    //Name = row.Field<string>("NAME")
        //                }).Select(g => g.CopyToDataTable()).ToList();
        //        // .CopyToDataTable();

        //        foreach (var loopheader in distinctheaders)
        //        {
        //            ChecklistReportDTO header = new ChecklistReportDTO();
        //            List<ReportChecklistGroup> Unorderedgroups = new List<ReportChecklistGroup>();
        //            Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
        //            header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
        //            header.applicationName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
        //            header.groups = new List<ReportChecklistGroup>();
        //            header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["Idjob"]);
        //            var checklist = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId).FirstOrDefault();
        //            header.DisplayOrder = checklist.Displayorder;
        //            //  header.DisplayOrder = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId);
        //            // .Select(x => x.jobChecklistHeaderId).ToList();
        //            var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
        //                .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();

        //            // var headersgroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId).ToList();
        //            var payloadgroup = checklist.lstExportChecklistGroup.Select(x => x.jobChecklistGroupId).ToList();
        //            foreach (var eachGroup in distinctGroup)
        //            // foreach (var eachGroup in payloadgroup)
        //            {
        //                if (payloadgroup.Contains(Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"])))
        //                {
        //                    ReportChecklistGroup group = new ReportChecklistGroup();
        //                    group.item = new List<ReportItem>();
        //                    //if (Convert.ToInt32(ds.Tables[0].Rows[i]["JobChecklistHeaderId"]) == checklistHeaderId)
        //                    {
        //                        Int32 IdJobChecklistGroup = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
        //                        group.checkListGroupName = eachGroup.Rows[0]["ChecklistGroupName"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["ChecklistGroupName"].ToString();
        //                        group.jobChecklistGroupId = eachGroup.Rows[0]["IdJobChecklistGroup"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
        //                        group.checkListGroupType = eachGroup.Rows[0]["checklistGroupType"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["checklistGroupType"].ToString();
        //                        // group.DisplayOrder1 = eachGroup.Rows[0]["DisplayOrder1"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["DisplayOrder1"]);
        //                        group.DisplayOrder1 = checklist.lstExportChecklistGroup.Where(y => y.jobChecklistGroupId == IdJobChecklistGroup).Select(x => x.displayOrder1).FirstOrDefault();
        //                        //var GroupInPayload = dataTableParameters.lstexportChecklist.Where(x => x.jobChecklistHeaderId == checklistHeaderId).Select(z => z.lstExportChecklistGroup).FirstOrDefault();

        //                        //foreach (var gp in GroupInPayload)
        //                        //{
        //                        //    if (gp.jobChecklistGroupId == IdJobChecklistGroup)
        //                        //    {
        //                        //        group.DisplayOrder1 = gp.displayOrder1;
        //                        //    }
        //                        //}

        //                        if (eachGroup.Rows[0]["checklistGroupType"].ToString().ToLower().TrimEnd() != "pl")
        //                        {
        //                            var groupsitems = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
        //                            //for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
        //                            for (int j = 0; j < groupsitems.Count; j++)
        //                            {
        //                                //if (Convert.ToInt32(ds.Tables[0].Rows[j]["IdJobChecklistGroup"]) == IdJobChecklistGroup && Convert.ToInt32(ds.Tables[0].Rows[j]["JobChecklistHeaderId"]) == checklistHeaderId)
        //                                //{

        //                                ReportItem item = new ReportItem();
        //                                item.Details = new List<ReportDetails>();

        //                                #region Items
        //                                Int32 IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
        //                                if (IdChecklistItem != 0)
        //                                {
        //                                    item.checklistItemName = groupsitems[j]["checklistItemName"] == DBNull.Value ? string.Empty : groupsitems[j]["checklistItemName"].ToString();
        //                                    item.IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
        //                                    item.jocbChecklistItemDetailsId = groupsitems[j]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["JocbChecklistItemDetailsId"]);
        //                                    int itemid = Convert.ToInt32(item.jocbChecklistItemDetailsId);
        //                                    item.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobchecklistItemDetails == itemid);
        //                                    item.comments = groupsitems[j]["Comments"] == DBNull.Value ? string.Empty : groupsitems[j]["Comments"].ToString();
        //                                    item.idDesignApplicant = groupsitems[j]["IdDesignApplicant"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdDesignApplicant"]);
        //                                    item.idInspector = groupsitems[j]["IdInspector"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdInspector"]);
        //                                    item.stage = groupsitems[j]["Stage"] == DBNull.Value ? string.Empty : groupsitems[j]["Stage"].ToString();
        //                                    item.partyResponsible1 = groupsitems[j]["PartyResponsible1"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["PartyResponsible1"]);
        //                                    item.manualPartyResponsible = groupsitems[j]["ManualPartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["ManualPartyResponsible"].ToString();
        //                                    item.idContact = groupsitems[j]["IdContact"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdContact"]);
        //                                    item.referenceDocumentId = groupsitems[j]["IdReferenceDocument"] == DBNull.Value ? string.Empty : groupsitems[j]["IdReferenceDocument"].ToString();
        //                                    item.dueDate = groupsitems[j]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["DueDate"]);
        //                                    item.status = groupsitems[j]["Status"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Status"]);
        //                                    item.checkListLastModifiedDate = groupsitems[j]["checklistItemLastDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["checklistItemLastDate"]);
        //                                    item.idCreateFormDocument = groupsitems[j]["IdCreateFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCreateFormDocument"]);
        //                                    item.idUploadFormDocument = groupsitems[j]["IdUploadFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdUploadFormDocument"]);
        //                                    item.Displayorder = groupsitems[j]["Displayorder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Displayorder"]);
        //                                    item.ChecklistDetailDisplayOrder = groupsitems[j]["ChecklistDetailDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["ChecklistDetailDisplayOrder"]);
        //                                    item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
        //                                    if (item.idContact != null && item.idContact != 0)
        //                                    {
        //                                        var c = rpoContext.Contacts.Find(item.idContact);
        //                                        item.ContactName = c.FirstName + c.LastName;
        //                                    }
        //                                    if (item.idDesignApplicant != null && item.idDesignApplicant != 0)
        //                                    {
        //                                        var c = rpoContext.Contacts.Find(item.idDesignApplicant);
        //                                        item.DesignApplicantName = c.FirstName + c.LastName;

        //                                    }
        //                                    if (item.idInspector != null && item.idInspector != 0)
        //                                    {
        //                                        var c = rpoContext.Contacts.Find(item.idInspector);
        //                                        item.InspectorName = c.FirstName + c.LastName;

        //                                    }
        //                                    #endregion

        //                                    group.item.Add(item);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();

        //                            var PlumbingCheckListFloors = groupsitems2.AsEnumerable().Select(a => a.Field<Int32?>("IdJobPlumbingCheckListFloors")).Distinct().ToList();

        //                            for (int j = 0; j < PlumbingCheckListFloors.Count(); j++)
        //                            {

        //                                ReportItem item = new ReportItem();
        //                                item.Details = new List<ReportDetails>();
        //                                var IdJobPlumbingCheckListFloors1 = PlumbingCheckListFloors[j];
        //                                var detailItem = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobPlumbingCheckListFloors") == IdJobPlumbingCheckListFloors1).ToList();
        //                                #region Items
        //                                item.FloorNumber = detailItem[0]["FloorNumber"] == DBNull.Value ? string.Empty : detailItem[0]["FloorNumber"].ToString();
        //                                item.idJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors1 != 0 ? IdJobPlumbingCheckListFloors1 : 0;
        //                                #endregion

        //                                for (int i = 0; i < detailItem.Count; i++)
        //                                {
        //                                    ReportDetails detail = new ReportDetails();
        //                                    #region Items details
        //                                    detail.checklistGroupType = detailItem[i]["checklistGroupType"] == DBNull.Value ? string.Empty : detailItem[i]["checklistGroupType"].ToString();
        //                                    detail.idJobPlumbingInspection = detailItem[i]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingInspection"]);
        //                                    detail.idJobPlumbingCheckListFloors = detailItem[i]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingCheckListFloors"]);
        //                                    detail.inspectionPermit = detailItem[i]["checklistItemName"] == DBNull.Value ? string.Empty : detailItem[i]["checklistItemName"].ToString();
        //                                    //detail.floorName = ds.Tables[0].Rows[k]["FloorName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["FloorName"].ToString();
        //                                    detail.workOrderNumber = detailItem[i]["WorkOrderNo"] == DBNull.Value ? string.Empty : detailItem[i]["WorkOrderNo"].ToString();
        //                                    detail.plComments = detailItem[i]["Comments"] == DBNull.Value ? string.Empty : detailItem[i]["Comments"].ToString();
        //                                    detail.DueDate = detailItem[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["DueDate"]);
        //                                    detail.nextInspection = detailItem[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["NextInspection"]);
        //                                    detail.result = detailItem[i]["Result"] == DBNull.Value ? string.Empty : detailItem[i]["Result"].ToString();
        //                                    detail.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
        //                                    item.Details.Add(detail);
        //                                    #endregion

        //                                }
        //                                group.item.Add(item);
        //                            }
        //                        }
        //                    }
        //                    ////copy done
        //                    Unorderedgroups.Add(group);

        //                    //  header.groups.Add(group);
        //                }
        //                header.groups = Unorderedgroups.OrderBy(x => x.DisplayOrder1).ToList();
        //                //  header.groups.OrderBy(x => x.DisplayOrder1).ToList();
        //            }

        //            headerlist.Add(header);
        //        }



        //        var result = headerlist.OrderBy(x => x.DisplayOrder).ToList();
        //        //  return Ok(headerlist);
        //        #region Excel
        //        if (result != null && result.Count > 0)
        //        {
        //            string exportFilename = string.Empty;
        //            exportFilename = "ChecklistReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
        //            string exportFilePath = ExportToExcel(result, exportFilename);

        //            FileInfo fileinfo = new FileInfo(exportFilePath);
        //            long fileinfoSize = fileinfo.Length;

        //            var downloadFilePath = Properties.Settings.Default.APIUrl + "/" + Properties.Settings.Default.ReportExcelExportPath + "/" + exportFilename;
        //            List<KeyValuePair<string, string>> fileResult = new List<KeyValuePair<string, string>>();
        //            fileResult.Add(new KeyValuePair<string, string>("exportFilePath", downloadFilePath));
        //            fileResult.Add(new KeyValuePair<string, string>("exportFilesize", Convert.ToString(fileinfoSize)));

        //            return Ok(fileResult);
        //        }
        //        else
        //        {
        //            throw new RpoBusinessException(StaticMessages.NoRecordFoundMessage);
        //        }
        //        #endregion
        //  //  }
        //   // else
        //   // {
        //  //      throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
        //  //  }
        //}


        /// <summary>
        /// Class Header.
        /// </summary>
        /// <seealso cref="iTextSharp.text.pdf.PdfPageEventHelper" />
        public partial class Header : PdfPageEventHelper
        {

            /// <summary>
            /// Called when [start page].
            /// </summary>
            /// <param name="writer">The writer.</param>
            /// <param name="doc">The document.</param>
            public override void OnStartPage(PdfWriter writer, Document doc)
            {
                int pageNumber = writer.PageNumber - 1;

                if (pageNumber > 0)
                {
                    Image logo = Image.GetInstance(HttpContext.Current.Server.MapPath("~\\HTML\\images\\logo-new.jpg"));
                    logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                    logo.ScaleToFit(120, 60);
                    logo.SetAbsolutePosition(260, 760);

                    PdfPTable table = new PdfPTable(5);
                    table.WidthPercentage = 100;


                    PdfPCell cell = new PdfPCell(logo);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.Rowspan = 2;
                    cell.PaddingBottom = 5;
                    cell.Colspan = 5;
                    table.AddCell(cell);

                    doc.Add(table);

                }

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
                table.WidthPercentage = 20;

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

        /// <summary>
        ///  Get the report of ReportOverallPermitExpiry Report with filter and sorting  and export to excel
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/Checklist/ExportChecklistToExcelForCustomer")]
        public IHttpActionResult PostExportChecklistToExcelForCustomer(ChecklistReportDatatableParameter dataTableParameters)
        {
            var employee = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            // if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            // {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

            DataSet ds = new DataSet();
            SqlParameter[] spParameter = new SqlParameter[4];

            spParameter[0] = new SqlParameter("@IdJobCheckListHeader", SqlDbType.NVarChar);
            spParameter[0].Direction = ParameterDirection.Input;
            var lstheaders = dataTableParameters.lstexportChecklist.Select(x => x.jobChecklistHeaderId).ToList();
            string headerid = null;
            foreach (var l in lstheaders)
            {
                headerid += l + ",";
            }
            //  headerid.Remove(headerid.Length - 1, 1);
            spParameter[0].Value = headerid.Remove(headerid.Length - 1, 1);


            spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
            spParameter[1].Direction = ParameterDirection.Input;
            spParameter[1].Value = dataTableParameters.Displayorder;
            // spParameter[1].Value = dataTableParameters.lstexportChecklist.Select(x=>x.Displayorder);

            spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
            spParameter[2].Direction = ParameterDirection.Input;
            spParameter[2].Value = string.Empty;

            spParameter[3] = new SqlParameter("@IdCustomer", SqlDbType.Int);
            spParameter[3].Direction = ParameterDirection.Input;
            spParameter[3].Value = employee.Id;

            ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "ChecklistViewForCustomer", spParameter);
            List<CheckListApplicationDropDown.Models.ChecklistReportDTO> headerlist = new List<ChecklistReportDTO>();

            List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                    .GroupBy(row => new
                    {
                        JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
                            //Name = row.Field<string>("NAME")
                        }).Select(g => g.CopyToDataTable()).ToList();
            // .CopyToDataTable();

            foreach (var loopheader in distinctheaders)
            {
                ChecklistReportDTO header = new ChecklistReportDTO();
                List<ReportChecklistGroup> Unorderedgroups = new List<ReportChecklistGroup>();
                Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                header.applicationName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                header.groups = new List<ReportChecklistGroup>();
                header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["Idjob"]);
                var checklist = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId).FirstOrDefault();
                header.DisplayOrder = checklist.Displayorder;
                //  header.DisplayOrder = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId);
                // .Select(x => x.jobChecklistHeaderId).ToList();
                var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                    .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();

                // var headersgroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId).ToList();
                var payloadgroup = checklist.lstExportChecklistGroup.Select(x => x.jobChecklistGroupId).ToList();
                foreach (var eachGroup in distinctGroup)
                // foreach (var eachGroup in payloadgroup)
                {
                    if (payloadgroup.Contains(Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"])))
                    {
                        ReportChecklistGroup group = new ReportChecklistGroup();
                        group.item = new List<ReportItem>();
                        //if (Convert.ToInt32(ds.Tables[0].Rows[i]["JobChecklistHeaderId"]) == checklistHeaderId)
                        {
                            Int32 IdJobChecklistGroup = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                            group.checkListGroupName = eachGroup.Rows[0]["ChecklistGroupName"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["ChecklistGroupName"].ToString();
                            group.jobChecklistGroupId = eachGroup.Rows[0]["IdJobChecklistGroup"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                            group.checkListGroupType = eachGroup.Rows[0]["checklistGroupType"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["checklistGroupType"].ToString();
                            // group.DisplayOrder1 = eachGroup.Rows[0]["DisplayOrder1"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["DisplayOrder1"]);
                            group.DisplayOrder1 = checklist.lstExportChecklistGroup.Where(y => y.jobChecklistGroupId == IdJobChecklistGroup).Select(x => x.displayOrder1).FirstOrDefault();
                            //var GroupInPayload = dataTableParameters.lstexportChecklist.Where(x => x.jobChecklistHeaderId == checklistHeaderId).Select(z => z.lstExportChecklistGroup).FirstOrDefault();

                            //foreach (var gp in GroupInPayload)
                            //{
                            //    if (gp.jobChecklistGroupId == IdJobChecklistGroup)
                            //    {
                            //        group.DisplayOrder1 = gp.displayOrder1;
                            //    }
                            //}

                            if (eachGroup.Rows[0]["checklistGroupType"].ToString().ToLower().TrimEnd() != "pl")
                            {
                                var groupsitems = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
                                //for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                for (int j = 0; j < groupsitems.Count; j++)
                                {
                                    //if (Convert.ToInt32(ds.Tables[0].Rows[j]["IdJobChecklistGroup"]) == IdJobChecklistGroup && Convert.ToInt32(ds.Tables[0].Rows[j]["JobChecklistHeaderId"]) == checklistHeaderId)
                                    //{

                                    ReportItem item = new ReportItem();
                                    item.Details = new List<ReportDetails>();

                                    #region Items
                                    Int32 IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                    if (IdChecklistItem != 0)
                                    {
                                        item.checklistItemName = groupsitems[j]["checklistItemName"] == DBNull.Value ? string.Empty : groupsitems[j]["checklistItemName"].ToString();
                                        item.IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                        item.jocbChecklistItemDetailsId = groupsitems[j]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["JocbChecklistItemDetailsId"]);
                                        int itemid = Convert.ToInt32(item.jocbChecklistItemDetailsId);
                                        item.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobchecklistItemDetails == itemid);
                                        item.comments = groupsitems[j]["Comments"] == DBNull.Value ? string.Empty : groupsitems[j]["Comments"].ToString();
                                        item.idDesignApplicant = groupsitems[j]["IdDesignApplicant"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdDesignApplicant"]);
                                        item.idInspector = groupsitems[j]["IdInspector"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdInspector"]);
                                        item.stage = groupsitems[j]["Stage"] == DBNull.Value ? string.Empty : groupsitems[j]["Stage"].ToString();
                                        item.partyResponsible1 = groupsitems[j]["PartyResponsible1"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["PartyResponsible1"]);
                                        item.manualPartyResponsible = groupsitems[j]["ManualPartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["ManualPartyResponsible"].ToString();
                                        item.idContact = groupsitems[j]["IdContact"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdContact"]);
                                        item.referenceDocumentId = groupsitems[j]["IdReferenceDocument"] == DBNull.Value ? string.Empty : groupsitems[j]["IdReferenceDocument"].ToString();
                                        item.dueDate = groupsitems[j]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["DueDate"]);
                                        item.status = groupsitems[j]["Status"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Status"]);
                                        item.checkListLastModifiedDate = groupsitems[j]["checklistItemLastDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["checklistItemLastDate"]);
                                        item.idCreateFormDocument = groupsitems[j]["IdCreateFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCreateFormDocument"]);
                                        item.idUploadFormDocument = groupsitems[j]["IdUploadFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdUploadFormDocument"]);
                                        item.Displayorder = groupsitems[j]["Displayorder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Displayorder"]);
                                        item.ChecklistDetailDisplayOrder = groupsitems[j]["ChecklistDetailDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["ChecklistDetailDisplayOrder"]);
                                        item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                        item.ClientNotes = groupsitems[j]["ClientNotes"] == DBNull.Value ? string.Empty : groupsitems[j]["ClientNotes"].ToString();
                                        if (item.idContact != null && item.idContact != 0)
                                        {
                                            var c = rpoContext.Contacts.Find(item.idContact);
                                            item.ContactName = c.FirstName +" "+ c.LastName;
                                            if (c.IdCompany != null && c.IdCompany != 0)
                                            {
                                                item.CompanyName = rpoContext.Companies.Where(x => x.Id == c.IdCompany).Select(y => y.Name).FirstOrDefault();
                                            }
                                        }
                                        if (item.idDesignApplicant != null && item.idDesignApplicant != 0)
                                        {
                                            var c = rpoContext.JobContacts.Find(item.idDesignApplicant);
                                            if (c != null)
                                            {
                                                item.DesignApplicantName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                            }

                                        }
                                        if (item.idInspector != null && item.idInspector != 0)
                                        {
                                            var c = rpoContext.JobContacts.Find(item.idInspector);
                                            if (c != null)
                                            {
                                                item.InspectorName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                            }

                                        }
                                        #endregion

                                        group.item.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();

                                var PlumbingCheckListFloors = groupsitems2.AsEnumerable().Select(a => a.Field<Int32?>("IdJobPlumbingCheckListFloors")).Distinct().ToList();

                                for (int j = 0; j < PlumbingCheckListFloors.Count(); j++)
                                {

                                    ReportItem item = new ReportItem();
                                    item.Details = new List<ReportDetails>();
                                    var IdJobPlumbingCheckListFloors1 = PlumbingCheckListFloors[j];
                                    var detailItem = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobPlumbingCheckListFloors") == IdJobPlumbingCheckListFloors1).ToList();
                                    #region Items
                                    item.FloorNumber = detailItem[0]["FloorNumber"] == DBNull.Value ? string.Empty : detailItem[0]["FloorNumber"].ToString();
                                    item.idJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors1 != 0 ? IdJobPlumbingCheckListFloors1 : 0;
                                    #endregion

                                    for (int i = 0; i < detailItem.Count; i++)
                                    {
                                        ReportDetails detail = new ReportDetails();
                                        #region Items details
                                        detail.checklistGroupType = detailItem[i]["checklistGroupType"] == DBNull.Value ? string.Empty : detailItem[i]["checklistGroupType"].ToString();
                                        detail.idJobPlumbingInspection = detailItem[i]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingInspection"]);
                                        detail.idJobPlumbingCheckListFloors = detailItem[i]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingCheckListFloors"]);
                                        detail.inspectionPermit = detailItem[i]["checklistItemName"] == DBNull.Value ? string.Empty : detailItem[i]["checklistItemName"].ToString();
                                        //detail.floorName = ds.Tables[0].Rows[k]["FloorName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["FloorName"].ToString();
                                        detail.workOrderNumber = detailItem[i]["WorkOrderNo"] == DBNull.Value ? string.Empty : detailItem[i]["WorkOrderNo"].ToString();
                                        detail.plComments = detailItem[i]["Comments"] == DBNull.Value ? string.Empty : detailItem[i]["Comments"].ToString();
                                        detail.DueDate = detailItem[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["DueDate"]);
                                        detail.nextInspection = detailItem[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["NextInspection"]);
                                        detail.result = detailItem[i]["Result"] == DBNull.Value ? string.Empty : detailItem[i]["Result"].ToString();
                                        detail.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                        detail.ClientNotes = detailItem[i]["ClientNotes"] == DBNull.Value ? string.Empty : detailItem[i]["ClientNotes"].ToString();
                                        item.Details.Add(detail);
                                        #endregion

                                    }
                                    group.item.Add(item);
                                }
                            }
                        }
                        ////copy done
                        Unorderedgroups.Add(group);

                        //  header.groups.Add(group);
                    }
                    header.groups = Unorderedgroups.OrderBy(x => x.DisplayOrder1).ToList();
                    //  header.groups.OrderBy(x => x.DisplayOrder1).ToList();
                }

                headerlist.Add(header);
            }



            var result = headerlist.OrderBy(x => x.DisplayOrder).ToList();
            //  return Ok(headerlist);
            #region Excel
            if (result != null && result.Count > 0)
            {
                string exportFilename = string.Empty;
                exportFilename = "ChecklistReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                string exportFilePath = ExportToExcelForCustomer(result, exportFilename);

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
            #endregion
            // }
            // else
            // {
            //  throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            //  }
        }

        private string ExportToExcelForCustomer(List<ChecklistReportDTO> result, string exportFilename)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
            templateFileName = "Checklist_Template_Working-Customer.xlsx";
            string templateFilePath = templatePath + templateFileName;
            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateFilePath);
            int ProjectNumber = result.Select(x => x.IdJob).FirstOrDefault().Value;
            int sheetIndex = 0;
            if (sheetIndex == 0)
            {
                templateWorkbook.SetSheetName(sheetIndex, "Checklist");
                //   templateWorkbook.SetSheetName(sheetIndex, "Job #" + item);
                sheetIndex = sheetIndex + 1;
            }
            //else
            //{
            //    templateWorkbook.CloneSheet(0);
            //    templateWorkbook.SetSheetName(sheetIndex, "Job #" + item);
            //    sheetIndex = sheetIndex + 1;
            //}
            //  }

            //XSSFFont myFont = (XSSFFont)templateWorkbook.CreateFont();
            //myFont.FontHeightInPoints = (short)12;
            //myFont.FontName = "Red Hat Display";
            ////myFont.IsBold = false;
            XSSFFont myFont = (XSSFFont)templateWorkbook.CreateFont();
            myFont.FontHeightInPoints = (short)12;
            myFont.FontName = "Times New Roman";
            //myFont.FontName = "Red Hat Display";
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
            //  myFont.FontName = "Red Hat Display";
            myFont_Bold.FontName = "Times New Roman";
            myFont_Bold.IsBold = true;

            //XSSFFont myFont_HeaderBold = (XSSFFont)templateWorkbook.CreateFont();
            //myFont_HeaderBold.FontHeightInPoints = (short)14;
            ////  myFont.FontName = "Red Hat Display";
            //myFont_HeaderBold.FontName = "Times New Roman";
            //myFont_HeaderBold.IsBold = true;


            XSSFCellStyle right_JobDetailAlignCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            right_JobDetailAlignCellStyle.SetFont(myFont);
            right_JobDetailAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.WrapText = true;
            right_JobDetailAlignCellStyle.VerticalAlignment = VerticalAlignment.Center;
            right_JobDetailAlignCellStyle.Alignment = HorizontalAlignment.Right;

            ISheet sheet = templateWorkbook.GetSheet("Checklist");

            Job job = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Company").Include("ProjectManager").FirstOrDefault(x => x.Id == ProjectNumber);

            #region Project Address

            string projectAddress = job != null
                 && job.RfpAddress != null
                 ? job.RfpAddress.HouseNumber + " " + (job.RfpAddress.Street != null ? job.RfpAddress.Street + ", " : string.Empty)
                   + (job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : string.Empty)
                : string.Empty;
            IRow iprojectAddressRow = sheet.GetRow(1);
            if (iprojectAddressRow == null)
            {
                iprojectAddressRow = sheet.CreateRow(1);

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

            #region ProjectNumber
            //   string specialPlaceName = job != null ? job.SpecialPlace : string.Empty;
            //    IRow iSpecialPlaceNameRow = sheet.GetRow(3);
            IRow iProjectNumberRow = sheet.GetRow(2);
            if (iProjectNumberRow == null)
            {
                iProjectNumberRow = sheet.CreateRow(2);
            }

            if (iProjectNumberRow.GetCell(1) != null)
            {
                iProjectNumberRow.GetCell(1).SetCellValue(ProjectNumber);
            }
            else
            {
                iProjectNumberRow.CreateCell(1).SetCellValue(ProjectNumber);
            }

            iProjectNumberRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            #endregion

            #region Client

            string clientName = job != null && job.Company != null ? job.Company.Name : string.Empty;

            IRow iClientRow = sheet.GetRow(3);
            if (iClientRow == null)
            {
                iClientRow = sheet.CreateRow(3);
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
            IRow iProjectManagerRow = sheet.GetRow(4);
            if (iProjectManagerRow == null)
            {
                iProjectManagerRow = sheet.CreateRow(4);
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

            #region Document

            string DocumentName = "Project Report";

            IRow iDocumentRow = sheet.GetRow(5);
            if (iDocumentRow == null)
            {
                iDocumentRow = sheet.CreateRow(5);
            }

            if (iDocumentRow.GetCell(1) != null)
            {
                iDocumentRow.GetCell(1).SetCellValue(DocumentName);
            }
            else
            {
                iDocumentRow.CreateCell(1).SetCellValue(DocumentName);
            }

            iDocumentRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;

            #endregion

            #region Report Date

            string reportDate = DateTime.Today.ToString(Common.ExportReportDateFormat);
            IRow iDateRow = sheet.GetRow(6);
            if (iDateRow == null)
            {
                iDateRow = sheet.CreateRow(6);
            }

            if (iDateRow.GetCell(1) != null)
            {
                iDateRow.GetCell(1).SetCellValue(reportDate);
            }
            else
            {
                iDateRow.CreateCell(1).SetCellValue(reportDate);
            }

            iDateRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;

            #endregion
            int index = 8;

            foreach (var c in result)
            {

                if (c != null)
                {
                    #region Checklist Report Header

                    XSSFFont ChecklistReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    ChecklistReportHeaderFont.FontHeightInPoints = (short)14;
                    ChecklistReportHeaderFont.FontName = "Times New Roman";
                    ChecklistReportHeaderFont.IsBold = true;

                    XSSFCellStyle ChecklistReportHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    ChecklistReportHeaderCellStyle.SetFont(ChecklistReportHeaderFont);
                    ChecklistReportHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    ChecklistReportHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    ChecklistReportHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    ChecklistReportHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    ChecklistReportHeaderCellStyle.WrapText = true;
                    ChecklistReportHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    //ChecklistReportHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                    ChecklistReportHeaderCellStyle.Alignment = HorizontalAlignment.Left;
                    ChecklistReportHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.White);
                    ChecklistReportHeaderCellStyle.FillPattern = FillPattern.SolidForeground;


                    //string DOB_ReportHeader = c.applicationName;
                    string DOB_ReportHeader = string.Empty;
                    if (!string.IsNullOrEmpty(c.Others))
                    { DOB_ReportHeader = c.applicationName + " - " + c.Others; }
                    else
                    { DOB_ReportHeader = c.applicationName; }

                    IRow iDOB_ReportHeaderRow = sheet.GetRow(index);
                    if (iDOB_ReportHeaderRow == null)
                    {
                        iDOB_ReportHeaderRow = sheet.CreateRow(index);
                    }
                    iDOB_ReportHeaderRow.HeightInPoints = (float)19.50;

                    //if (c.groups.Select(x => x.checkListGroupType).ToList().Contains("PL"))
                    //{
                    //    if (iDOB_ReportHeaderRow.GetCell(0) != null)
                    //    {
                    //        iDOB_ReportHeaderRow.GetCell(0).SetCellValue("Plumbing Checklist:- "+DOB_ReportHeader);
                    //    }
                    //    else
                    //    {

                    //        iDOB_ReportHeaderRow.CreateCell(0).SetCellValue("Plumbing Checklist:- "+DOB_ReportHeader);
                    //    }
                    //}
                    // else
                    // {
                    if (iDOB_ReportHeaderRow.GetCell(0) != null)
                    {
                        iDOB_ReportHeaderRow.GetCell(0).SetCellValue(DOB_ReportHeader);
                    }
                    else
                    {

                        iDOB_ReportHeaderRow.CreateCell(0).SetCellValue(DOB_ReportHeader);
                    }
                    // }

                    // CellRangeAddress DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 6);
                    CellRangeAddress DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    iDOB_ReportHeaderRow.GetCell(0).CellStyle = ChecklistReportHeaderCellStyle;

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    index = index + 1;
                    #endregion


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

                    foreach (var g in c.groups)
                    {

                        #region group header
                        XSSFCellStyle GroupHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                        GroupHeaderCellStyle.SetFont(ChecklistReportHeaderFont);
                        GroupHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                        GroupHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                        GroupHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                        GroupHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                        GroupHeaderCellStyle.WrapText = true;
                        GroupHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                        GroupHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                        GroupHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.White);
                        GroupHeaderCellStyle.FillPattern = FillPattern.SolidForeground;


                        string Group_ReportHeader = g.checkListGroupName;
                        IRow Group_ReportHeaderRow = sheet.GetRow(index);
                        if (Group_ReportHeaderRow == null)
                        {
                            Group_ReportHeaderRow = sheet.CreateRow(index);
                        }
                        Group_ReportHeaderRow.HeightInPoints = (float)19.50;

                        if (Group_ReportHeaderRow.GetCell(0) != null)
                        {
                            Group_ReportHeaderRow.GetCell(0).SetCellValue(Group_ReportHeader);
                        }
                        else
                        {
                            Group_ReportHeaderRow.CreateCell(0).SetCellValue(Group_ReportHeader);
                        }

                        // DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 6);
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                        sheet.AddMergedRegion(DOBCellRangeAddress);
                        Group_ReportHeaderRow.GetCell(0).CellStyle = GroupHeaderCellStyle;

                        RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        index = index + 1;
                        #endregion
                        #region Column Header cell style
                        XSSFCellStyle ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                        ColumnHeaderCellStyle.SetFont(ChecklistReportHeaderFont);
                        ColumnHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                        ColumnHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                        ColumnHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                        ColumnHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                        ColumnHeaderCellStyle.WrapText = true;
                        ColumnHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                        ColumnHeaderCellStyle.Alignment = HorizontalAlignment.Center;

                        ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                        ColumnHeaderCellStyle.FillPattern = FillPattern.SolidForeground;
                        #endregion
                        if (g.checkListGroupType.ToLower().Trim() == "general")
                        {

                            #region header

                            IRow iDOB_HeaderRow = sheet.GetRow(index);
                            //  var x = iDOB_HeaderRow.GetCell(0).CellStyle.FillBackgroundColorColor;
                            //int color_n = System.Convert.ToInt32((sheet.Cell[9,0].Interior.Color);
                            //Color color = System.Drawing.ColorTranslator.FromOle(color_n);
                            //  var d = iDOB_HeaderRow.GetCell(0).CellStyle.FillBackgroundColorColor;

                            if (iDOB_HeaderRow == null)
                            {
                                iDOB_HeaderRow = sheet.CreateRow(index);
                            }
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                            sheet.AddMergedRegion(DOBCellRangeAddress);
                            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            if (iDOB_HeaderRow.GetCell(0) != null)
                            {

                                iDOB_HeaderRow.GetCell(0).SetCellValue("Item Name");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(0).SetCellValue("Item Name");
                            }


                            if (iDOB_HeaderRow.GetCell(2) != null)
                            {
                                iDOB_HeaderRow.GetCell(2).SetCellValue("Party Responsible");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(2).SetCellValue("Party Responsible");
                            }
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 3, 4);
                            sheet.AddMergedRegion(DOBCellRangeAddress);
                            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            if (iDOB_HeaderRow.GetCell(3) != null)
                            {
                                iDOB_HeaderRow.GetCell(3).SetCellValue("Comments");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(3).SetCellValue("Comments");
                            }

                            if (iDOB_HeaderRow.GetCell(5) != null)
                            {
                                iDOB_HeaderRow.GetCell(5).SetCellValue("Target Date");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(5).SetCellValue("Target Date");
                            }

                            if (iDOB_HeaderRow.GetCell(6) != null)
                            {
                                iDOB_HeaderRow.GetCell(6).SetCellValue("Status");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(6).SetCellValue("Status");
                            }
                            if (iDOB_HeaderRow.GetCell(7) != null)
                            {
                                iDOB_HeaderRow.GetCell(7).SetCellValue("Client Note");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("Client Note");
                            }

                            iDOB_HeaderRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(1).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(6).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(7).CellStyle = ColumnHeaderCellStyle;

                            index = index + 1;
                            #endregion
                            #region Data
                            foreach (var item in g.item)
                            {
                                IRow iRow = sheet.GetRow(index);


                                if (iRow == null)
                                {
                                    iRow = sheet.CreateRow(index);
                                }
                                if (iRow.GetCell(0) != null)
                                {
                                    iRow.GetCell(0).SetCellValue(item.checklistItemName);
                                }
                                else
                                {
                                    iRow.CreateCell(0).SetCellValue(item.checklistItemName);
                                    // iRow.CreateCell(1);
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                //DOBCellRangeAddress = new CellRangeAddress(index, index, 2, 3);
                                //sheet.AddMergedRegion(DOBCellRangeAddress);
                                //RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                //RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                //RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                //RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iRow.GetCell(2) != null)
                                {
                                    if (item.PartyResponsible.ToLower() == "rpo user")
                                        iRow.GetCell(2).SetCellValue("RPO");

                                    else if (item.PartyResponsible.ToLower() == "contact")
                                    {
                                        if (item.CompanyName != null)
                                            iRow.GetCell(2).SetCellValue(item.ContactName + " - " + item.CompanyName);
                                        else
                                            iRow.GetCell(2).SetCellValue(item.ContactName);
                                    }

                                    else if (item.PartyResponsible.ToLower() == "other")
                                        iRow.GetCell(2).SetCellValue(item.manualPartyResponsible);
                                    else
                                        iRow.GetCell(2).SetCellValue("");
                                }
                                else
                                {
                                    if (item.PartyResponsible.ToLower() == "rpo user")
                                        iRow.CreateCell(2).SetCellValue("RPO");

                                    else if (item.PartyResponsible.ToLower() == "contact")                                        
                                    {
                                        if (item.CompanyName != null)
                                            iRow.CreateCell(2).SetCellValue(item.ContactName + " - " + item.CompanyName);
                                        else
                                            iRow.CreateCell(2).SetCellValue(item.ContactName);
                                    }

                                    else if (item.PartyResponsible.ToLower() == "other")
                                        iRow.CreateCell(2).SetCellValue(item.manualPartyResponsible);
                                    else
                                        iRow.CreateCell(2).SetCellValue("");
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 3, 4);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iRow.GetCell(3) != null)
                                {
                                    iRow.GetCell(3).SetCellValue(item.comments);
                                }
                                else
                                {
                                    iRow.CreateCell(3).SetCellValue(item.comments);
                                }

                                // string workDescription = item.JobWorkTypeDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);

                                if (iRow.GetCell(5) != null)
                                {
                                    iRow.GetCell(5).SetCellValue(item.dueDate != null ? Convert.ToDateTime(item.dueDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                                }
                                else
                                {
                                    iRow.CreateCell(5).SetCellValue(item.dueDate != null ? Convert.ToDateTime(item.dueDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                                }
                                //Temporary commented
                                if (iRow.GetCell(6) != null)
                                {
                                    if (item.status == 1)
                                        iRow.GetCell(6).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.GetCell(6).SetCellValue("InProcess");
                                    else if (item.status == 3)
                                        iRow.GetCell(6).SetCellValue("Completed");
                                    else
                                        iRow.GetCell(6).SetCellValue("");
                                }
                                else
                                {
                                    if (item.status == 1)
                                        iRow.CreateCell(6).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.CreateCell(6).SetCellValue("InProcess");
                                    else if (item.status == 3)
                                        iRow.CreateCell(6).SetCellValue("Completed");
                                    else
                                        iRow.CreateCell(6).SetCellValue("");
                                }
                                if (iRow.GetCell(7) != null)
                                {
                                    iRow.GetCell(7).SetCellValue(item.ClientNotes);
                                }
                                else
                                {
                                    iRow.CreateCell(7).SetCellValue(item.ClientNotes);
                                }


                                iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(7).CellStyle = leftAlignCellStyle;

                                index = index + 1;
                            }
                            #endregion
                        }
                        else if (g.checkListGroupType.ToLower().Trim() == "tr")
                        {
                            IRow iDOB_HeaderRow = sheet.GetRow(index);

                            if (iDOB_HeaderRow == null)
                            {
                                iDOB_HeaderRow = sheet.CreateRow(index);
                            }
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                            sheet.AddMergedRegion(DOBCellRangeAddress);
                            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            if (iDOB_HeaderRow.GetCell(0) != null)
                            {
                                iDOB_HeaderRow.GetCell(0).SetCellValue("Item Name");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(0).SetCellValue("Item Name");
                            }

                            if (iDOB_HeaderRow.GetCell(2) != null)
                            {
                                iDOB_HeaderRow.GetCell(2).SetCellValue("Design Applicant");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(2).SetCellValue("Design Applicant");
                            }

                            if (iDOB_HeaderRow.GetCell(3) != null)
                            {
                                iDOB_HeaderRow.GetCell(3).SetCellValue("Inspector");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(3).SetCellValue("Inspector");
                            }
                            if (iDOB_HeaderRow.GetCell(4) != null)
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue("Comments");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("Comments");
                            }
                            if (iDOB_HeaderRow.GetCell(5) != null)
                            {
                                iDOB_HeaderRow.GetCell(5).SetCellValue("Stage");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(5).SetCellValue("Stage");
                            }

                            if (iDOB_HeaderRow.GetCell(6) != null)
                            {
                                iDOB_HeaderRow.GetCell(6).SetCellValue("Status");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(6).SetCellValue("Status");
                            }
                            if (iDOB_HeaderRow.GetCell(7) != null)
                            {
                                iDOB_HeaderRow.GetCell(7).SetCellValue("Client Note");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("Client Note");
                            }

                            iDOB_HeaderRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(1).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(6).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(7).CellStyle = ColumnHeaderCellStyle;

                            index = index + 1;
                            #region Data
                            foreach (var item in g.item)
                            {
                                IRow iRow = sheet.GetRow(index);
                                if (iRow == null)
                                {
                                    iRow = sheet.CreateRow(index);
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iRow.GetCell(0) != null)
                                {
                                    iRow.GetCell(0).SetCellValue(item.checklistItemName);
                                }
                                else
                                {
                                    iRow.CreateCell(0).SetCellValue(item.checklistItemName);
                                }
                                if (iRow.GetCell(2) != null)
                                {
                                    // if (item.PartyResponsible.ToLower() == "rpo user")
                                    iRow.GetCell(2).SetCellValue(item.DesignApplicantName);

                                    //if (item.PartyResponsible.ToLower() == "contact")
                                    //    iRow.GetCell(1).SetCellValue(item.ContactName);
                                    //if (item.PartyResponsible.ToLower() == "other")
                                    //    iRow.GetCell(1).SetCellValue(item.manualPartyResponsible);
                                }
                                else
                                {
                                    iRow.CreateCell(2).SetCellValue(item.DesignApplicantName);
                                }

                                if (iRow.GetCell(3) != null)
                                {
                                    iRow.GetCell(3).SetCellValue(item.InspectorName);
                                }
                                else
                                {
                                    iRow.CreateCell(3).SetCellValue(item.InspectorName);
                                }
                                if (iRow.GetCell(4) != null)
                                {
                                    iRow.GetCell(4).SetCellValue(item.comments);
                                }
                                else
                                {
                                    iRow.CreateCell(4).SetCellValue(item.comments);
                                }

                                // string workDescription = item.JobWorkTypeDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);

                                if (iRow.GetCell(5) != null)
                                {
                                    //iRow.GetCell(3).SetCellValue(item.stage);
                                    if (item.stage == "1")
                                        iRow.GetCell(5).SetCellValue("Prior to Approval");
                                    else if (item.stage == "2")
                                        iRow.GetCell(5).SetCellValue("Prior to Permit");
                                    else if (item.stage == "3")
                                        iRow.GetCell(5).SetCellValue("Prior to Sign Off");
                                    else
                                        iRow.GetCell(5).SetCellValue("");
                                }
                                else
                                {
                                    if (item.stage == "1")
                                        iRow.CreateCell(5).SetCellValue("Prior to Approval");
                                    else if (item.stage == "2")
                                        iRow.CreateCell(5).SetCellValue("Prior to Permit");
                                    else if (item.stage == "3")
                                        iRow.CreateCell(5).SetCellValue("Prior to Sign Off");
                                    else
                                        iRow.CreateCell(5).SetCellValue("");
                                }
                                //Temporary commented
                                if (iRow.GetCell(6) != null)
                                {
                                    //iRow.GetCell(4).SetCellValue(item.status);
                                    if (item.status == 1)
                                        iRow.GetCell(6).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.GetCell(6).SetCellValue("Completed");
                                    else
                                        iRow.GetCell(6).SetCellValue("");
                                }
                                else
                                {
                                    if (item.status == 1)
                                        iRow.CreateCell(6).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.CreateCell(6).SetCellValue("Completed");
                                    else
                                        iRow.CreateCell(6).SetCellValue("");
                                }
                                if (iRow.GetCell(7) != null)
                                {
                                    iRow.GetCell(7).SetCellValue(item.ClientNotes);
                                }
                                else
                                {
                                    iRow.CreateCell(7).SetCellValue(item.ClientNotes);
                                }

                                iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                // if (iRow.GetCell(3) != null)
                                iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                //  if (iRow.GetCell(4) != null)
                                iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(7).CellStyle = leftAlignCellStyle;

                                index = index + 1;
                            }
                            #endregion
                        }
                        else if (g.checkListGroupType.ToLower().Trim() == "pl")
                        {
                            #region floor header
                            //index = index + 1;
                            foreach (var item in g.item)
                            {
                                IRow iDOB_FloorRow = sheet.GetRow(index);
                                if (iDOB_FloorRow == null)
                                {
                                    iDOB_FloorRow = sheet.CreateRow(index);
                                }

                                if (iDOB_FloorRow.GetCell(0) != null)
                                {
                                    iDOB_FloorRow.GetCell(0).SetCellValue("Floor - " + item.FloorNumber);
                                }
                                else
                                {
                                    iDOB_FloorRow.CreateCell(0).SetCellValue("Floor - " + item.FloorNumber);
                                }
                                //DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 6);
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                                sheet.AddMergedRegion(DOBCellRangeAddress);


                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                iDOB_FloorRow.GetCell(0).CellStyle = GroupHeaderCellStyle;
                                index = index + 1;
                                IRow iDOB_HeaderRow = sheet.GetRow(index);

                                if (iDOB_HeaderRow == null)
                                {
                                    iDOB_HeaderRow = sheet.CreateRow(index);
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iDOB_HeaderRow.GetCell(0) != null)
                                {
                                    iDOB_HeaderRow.GetCell(0).SetCellValue("Inspection Type");
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(0).SetCellValue("Inspection Type");
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 2, 4);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                                if (iDOB_HeaderRow.GetCell(2) != null)
                                {
                                    iDOB_HeaderRow.GetCell(2).SetCellValue("Comment");
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(2).SetCellValue("Comment");
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iDOB_HeaderRow.GetCell(5) != null)
                                {
                                    iDOB_HeaderRow.GetCell(5).SetCellValue("Status");
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(5).SetCellValue("Status");
                                }
                                if (iDOB_HeaderRow.GetCell(7) != null)
                                {
                                    iDOB_HeaderRow.GetCell(7).SetCellValue("Client Note");
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(7).SetCellValue("Client Note");
                                }
                                iDOB_HeaderRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(1).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(7).CellStyle = ColumnHeaderCellStyle;
                                index = index + 1;
                                foreach (var d in item.Details)
                                {
                                    #region Data                                   
                                    IRow iRow = sheet.GetRow(index);
                                    if (iRow == null)
                                    {
                                        iRow = sheet.CreateRow(index);
                                    }
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                                    sheet.AddMergedRegion(DOBCellRangeAddress);
                                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    if (iRow.GetCell(0) != null)
                                    {
                                        iRow.GetCell(0).SetCellValue(d.inspectionPermit);
                                    }
                                    else
                                    {
                                        iRow.CreateCell(0).SetCellValue(d.inspectionPermit);
                                    }
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 2, 4);
                                    sheet.AddMergedRegion(DOBCellRangeAddress);
                                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    if (iRow.GetCell(2) != null)
                                    {
                                        iRow.GetCell(2).SetCellValue(d.plComments);

                                    }
                                    else
                                    {
                                        iRow.CreateCell(2).SetCellValue(d.plComments);
                                    }

                                    if (iRow.GetCell(5) != null)
                                    {
                                        if (d.result == "1")
                                            iRow.GetCell(5).SetCellValue("Pass");
                                        else if (d.result == "2")
                                            iRow.GetCell(5).SetCellValue("Failed");
                                        else if (d.result == "3")
                                            iRow.GetCell(5).SetCellValue("Pending");
                                        else if (d.result == "4")
                                            iRow.GetCell(5).SetCellValue("NA");
                                        else
                                            iRow.GetCell(5).SetCellValue("");
                                        //iRow.GetCell(4).SetCellValue(item.status);                                  

                                    }
                                    else
                                    {
                                        if (d.result == "1")
                                            iRow.CreateCell(5).SetCellValue("Pass");
                                        else if (d.result == "2")
                                            iRow.CreateCell(5).SetCellValue("Failed");
                                        else if (d.result == "3")
                                            iRow.CreateCell(5).SetCellValue("Pending");
                                        else if (d.result == "4")
                                            iRow.CreateCell(5).SetCellValue("NA");
                                        else
                                            iRow.CreateCell(5).SetCellValue("");
                                    }
                                    if (iRow.GetCell(7) != null)
                                    {
                                        iRow.GetCell(7).SetCellValue(d.ClientNotes);

                                    }
                                    else
                                    {
                                        iRow.CreateCell(7).SetCellValue(d.ClientNotes);
                                    }
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                                    sheet.AddMergedRegion(DOBCellRangeAddress);
                                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    //iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                    //iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                    //iRow.GetCell(3).CellStyle = leftAlignCellStyle;

                                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(7).CellStyle = leftAlignCellStyle;
                                    //iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                    // iRow.GetCell(4).CellStyle = leftAlignCellStyle;

                                    index = index + 1;
                                }
                                #endregion
                            }
                        }




                    }

                }
                index = index + 1;

                #endregion
            }

            using (var file2 = new FileStream(exportFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                templateWorkbook.Write(file2);
                file2.Close();
            }

            return exportFilePath;

        }

        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/Checklist/ExportChecklistToPDFForCustomer")]
        public IHttpActionResult PostExportChecklistToPDFForCustomer(ChecklistReportDatatableParameter dataTableParameters)
        {
            var employee = this.rpoContext.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ExportReport))
            //{
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();

            DataSet ds = new DataSet();
            SqlParameter[] spParameter = new SqlParameter[4];

            spParameter[0] = new SqlParameter("@IdJobCheckListHeader", SqlDbType.NVarChar);
            spParameter[0].Direction = ParameterDirection.Input;
            var lstheaders = dataTableParameters.lstexportChecklist.Select(x => x.jobChecklistHeaderId).ToList();
            string headerid = null;
            foreach (var l in lstheaders)
            {
                headerid += l + ",";
            }
            //  headerid.Remove(headerid.Length - 1, 1);
            spParameter[0].Value = headerid.Remove(headerid.Length - 1, 1);


            spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
            spParameter[1].Direction = ParameterDirection.Input;
            spParameter[1].Value = dataTableParameters.Displayorder;
            // spParameter[1].Value = dataTableParameters.lstexportChecklist.Select(x=>x.Displayorder);

            spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
            spParameter[2].Direction = ParameterDirection.Input;
            spParameter[2].Value = string.Empty;

            spParameter[3] = new SqlParameter("@IdCustomer", SqlDbType.Int);
            spParameter[3].Direction = ParameterDirection.Input;
            spParameter[3].Value = employee.Id;

            ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "ChecklistViewForCustomer", spParameter);
            List<CheckListApplicationDropDown.Models.ChecklistReportDTO> headerlist = new List<ChecklistReportDTO>();

            List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                    .GroupBy(row => new
                    {
                        JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
                            //Name = row.Field<string>("NAME")
                        }).Select(g => g.CopyToDataTable()).ToList();
            // .CopyToDataTable();

            foreach (var loopheader in distinctheaders)
            {
                ChecklistReportDTO header = new ChecklistReportDTO();
                List<ReportChecklistGroup> Unorderedgroups = new List<ReportChecklistGroup>();
                Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                header.applicationName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                header.groups = new List<ReportChecklistGroup>();
                header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["Idjob"]);
                var checklist = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId).FirstOrDefault();
                header.DisplayOrder = checklist.Displayorder;
                //  header.DisplayOrder = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId);
                // .Select(x => x.jobChecklistHeaderId).ToList();
                var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                    .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();

                // var headersgroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId).ToList();
                var payloadgroup = checklist.lstExportChecklistGroup.Select(x => x.jobChecklistGroupId).ToList();
                foreach (var eachGroup in distinctGroup)
                // foreach (var eachGroup in payloadgroup)
                {
                    if (payloadgroup.Contains(Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"])))
                    {
                        ReportChecklistGroup group = new ReportChecklistGroup();
                        group.item = new List<ReportItem>();
                        //if (Convert.ToInt32(ds.Tables[0].Rows[i]["JobChecklistHeaderId"]) == checklistHeaderId)
                        {
                            Int32 IdJobChecklistGroup = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                            group.checkListGroupName = eachGroup.Rows[0]["ChecklistGroupName"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["ChecklistGroupName"].ToString();
                            group.jobChecklistGroupId = eachGroup.Rows[0]["IdJobChecklistGroup"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                            group.checkListGroupType = eachGroup.Rows[0]["checklistGroupType"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["checklistGroupType"].ToString();
                            // group.DisplayOrder1 = eachGroup.Rows[0]["DisplayOrder1"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["DisplayOrder1"]);
                            group.DisplayOrder1 = checklist.lstExportChecklistGroup.Where(y => y.jobChecklistGroupId == IdJobChecklistGroup).Select(x => x.displayOrder1).FirstOrDefault();
                            //var GroupInPayload = dataTableParameters.lstexportChecklist.Where(x => x.jobChecklistHeaderId == checklistHeaderId).Select(z => z.lstExportChecklistGroup).FirstOrDefault();

                            //foreach (var gp in GroupInPayload)
                            //{
                            //    if (gp.jobChecklistGroupId == IdJobChecklistGroup)
                            //    {
                            //        group.DisplayOrder1 = gp.displayOrder1;
                            //    }
                            //}

                            if (eachGroup.Rows[0]["checklistGroupType"].ToString().ToLower().TrimEnd() != "pl")
                            {
                                var groupsitems = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
                                //for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                for (int j = 0; j < groupsitems.Count; j++)
                                {
                                    //if (Convert.ToInt32(ds.Tables[0].Rows[j]["IdJobChecklistGroup"]) == IdJobChecklistGroup && Convert.ToInt32(ds.Tables[0].Rows[j]["JobChecklistHeaderId"]) == checklistHeaderId)
                                    //{

                                    ReportItem item = new ReportItem();
                                    item.Details = new List<ReportDetails>();

                                    #region Items
                                    Int32 IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                    if (IdChecklistItem != 0)
                                    {
                                        item.checklistItemName = groupsitems[j]["checklistItemName"] == DBNull.Value ? string.Empty : groupsitems[j]["checklistItemName"].ToString();
                                        item.IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                        item.jocbChecklistItemDetailsId = groupsitems[j]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["JocbChecklistItemDetailsId"]);
                                        int itemid = Convert.ToInt32(item.jocbChecklistItemDetailsId);
                                        item.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobchecklistItemDetails == itemid);
                                        item.comments = groupsitems[j]["Comments"] == DBNull.Value ? string.Empty : groupsitems[j]["Comments"].ToString();
                                        item.idDesignApplicant = groupsitems[j]["IdDesignApplicant"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdDesignApplicant"]);
                                        item.idInspector = groupsitems[j]["IdInspector"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdInspector"]);
                                        item.stage = groupsitems[j]["Stage"] == DBNull.Value ? string.Empty : groupsitems[j]["Stage"].ToString();
                                        item.partyResponsible1 = groupsitems[j]["PartyResponsible1"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["PartyResponsible1"]);
                                        item.manualPartyResponsible = groupsitems[j]["ManualPartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["ManualPartyResponsible"].ToString();
                                        item.idContact = groupsitems[j]["IdContact"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdContact"]);
                                        item.referenceDocumentId = groupsitems[j]["IdReferenceDocument"] == DBNull.Value ? string.Empty : groupsitems[j]["IdReferenceDocument"].ToString();
                                        item.dueDate = groupsitems[j]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["DueDate"]);
                                        item.status = groupsitems[j]["Status"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Status"]);
                                        item.checkListLastModifiedDate = groupsitems[j]["checklistItemLastDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(groupsitems[j]["checklistItemLastDate"]);
                                        item.idCreateFormDocument = groupsitems[j]["IdCreateFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCreateFormDocument"]);
                                        item.idUploadFormDocument = groupsitems[j]["IdUploadFormDocument"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdUploadFormDocument"]);
                                        item.Displayorder = groupsitems[j]["Displayorder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["Displayorder"]);
                                        item.ChecklistDetailDisplayOrder = groupsitems[j]["ChecklistDetailDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["ChecklistDetailDisplayOrder"]);
                                        item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                        item.ClientNotes = groupsitems[j]["ClientNotes"] == DBNull.Value ? string.Empty : groupsitems[j]["ClientNotes"].ToString();
                                        if (item.idContact != null && item.idContact != 0)
                                        {
                                            var c = rpoContext.Contacts.Find(item.idContact);
                                            item.ContactName = c.FirstName +" "+ c.LastName;
                                            if (c.IdCompany != null && c.IdCompany != 0)
                                            {
                                                item.CompanyName = rpoContext.Companies.Where(x => x.Id == c.IdCompany).Select(y => y.Name).FirstOrDefault();
                                            }
                                        }
                                        if (item.idDesignApplicant != null && item.idDesignApplicant != 0)
                                        {
                                            var c = rpoContext.JobContacts.Find(item.idDesignApplicant);
                                            if (c != null)
                                            {
                                                item.DesignApplicantName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                            }

                                        }
                                        if (item.idInspector != null && item.idInspector != 0)
                                        {
                                            var c = rpoContext.JobContacts.Find(item.idInspector);
                                            if (c != null)
                                            {
                                                item.InspectorName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                            }

                                        }
                                        #endregion

                                        group.item.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();

                                var PlumbingCheckListFloors = groupsitems2.AsEnumerable().Select(a => a.Field<Int32?>("IdJobPlumbingCheckListFloors")).Distinct().ToList();

                                for (int j = 0; j < PlumbingCheckListFloors.Count(); j++)
                                {

                                    ReportItem item = new ReportItem();
                                    item.Details = new List<ReportDetails>();
                                    var IdJobPlumbingCheckListFloors1 = PlumbingCheckListFloors[j];
                                    var detailItem = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobPlumbingCheckListFloors") == IdJobPlumbingCheckListFloors1).ToList();
                                    #region Items
                                    item.FloorNumber = detailItem[0]["FloorNumber"] == DBNull.Value ? string.Empty : detailItem[0]["FloorNumber"].ToString();
                                    item.idJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors1 != 0 ? IdJobPlumbingCheckListFloors1 : 0;
                                    #endregion

                                    for (int i = 0; i < detailItem.Count; i++)
                                    {
                                        ReportDetails detail = new ReportDetails();
                                        #region Items details
                                        detail.checklistGroupType = detailItem[i]["checklistGroupType"] == DBNull.Value ? string.Empty : detailItem[i]["checklistGroupType"].ToString();
                                        detail.idJobPlumbingInspection = detailItem[i]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingInspection"]);
                                        detail.idJobPlumbingCheckListFloors = detailItem[i]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingCheckListFloors"]);
                                        detail.inspectionPermit = detailItem[i]["checklistItemName"] == DBNull.Value ? string.Empty : detailItem[i]["checklistItemName"].ToString();
                                        //detail.floorName = ds.Tables[0].Rows[k]["FloorName"] == DBNull.Value ? string.Empty : ds.Tables[0].Rows[k]["FloorName"].ToString();
                                        detail.workOrderNumber = detailItem[i]["WorkOrderNo"] == DBNull.Value ? string.Empty : detailItem[i]["WorkOrderNo"].ToString();
                                        detail.plComments = detailItem[i]["Comments"] == DBNull.Value ? string.Empty : detailItem[i]["Comments"].ToString();
                                        detail.DueDate = detailItem[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["DueDate"]);
                                        detail.nextInspection = detailItem[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["NextInspection"]);
                                        detail.result = detailItem[i]["Result"] == DBNull.Value ? string.Empty : detailItem[i]["Result"].ToString();
                                        detail.HasDocument = rpoContext.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                        detail.ClientNotes = detailItem[i]["ClientNotes"] == DBNull.Value ? string.Empty : detailItem[i]["ClientNotes"].ToString();
                                        item.Details.Add(detail);
                                        #endregion

                                    }
                                    group.item.Add(item);
                                }
                            }
                        }
                        ////copy done
                        Unorderedgroups.Add(group);

                        //  header.groups.Add(group);
                    }
                    header.groups = Unorderedgroups.OrderBy(x => x.DisplayOrder1).ToList();
                    //  header.groups.OrderBy(x => x.DisplayOrder1).ToList();
                }

                headerlist.Add(header);
            }


            var result = headerlist.OrderBy(x => x.DisplayOrder).ToList();
            //  return Ok(headerlist);
            #region PDF
            if (result != null && result.Count > 0)
            {
                string exportFilename = string.Empty;
                string hearingDate = string.Empty;
                exportFilename = "GeneralChecklistCustomerReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                string exportFilePath = ExportToPdfForCustomer(hearingDate, result, exportFilename);
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
            #endregion
            //}
            //else
            //{
            //    throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            //}
        }

        private string ExportToPdfForCustomer(string hearingDate, List<ChecklistReportDTO> result, string exportFilename)
        {

            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);
            int ProjectNumber = result.Select(x => x.IdJob).FirstOrDefault().Value;

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

                // Phrase phrase = new Phrase();
                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SplitLate = false;
                Job job = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Company").Include("ProjectManager").FirstOrDefault(x => x.Id == ProjectNumber);

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
                cell.PaddingLeft = -55;
                cell.Colspan = 7;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.PaddingLeft = -55;
                cell.Colspan = 7;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = -10;
                table.AddCell(cell);

                #region Project Address
                string projectAddress = job != null
                && job.RfpAddress != null
                ? job.RfpAddress.HouseNumber + " " + (job.RfpAddress.Street != null ? job.RfpAddress.Street + ", " : string.Empty)
                  + (job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : string.Empty)
               : string.Empty;                

                cell = new PdfPCell(new Phrase("Project Address: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(projectAddress, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 6;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                #endregion
                #region ProjectNumber
                
                cell = new PdfPCell(new Phrase("RPO Project Number: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(ProjectNumber), font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 6;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Client
                string clientName = job != null && job.Company != null ? job.Company.Name : string.Empty;
               
                cell = new PdfPCell(new Phrase("Client: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(clientName, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 6;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Project Manager
                string ProjectManagerName = job.ProjectManager != null ? job.ProjectManager.FirstName + " " + job.ProjectManager.LastName + " | " + job.ProjectManager.Email : string.Empty;
                             
                cell = new PdfPCell(new Phrase("Project Manager: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(ProjectManagerName, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 6;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Document
               
                cell = new PdfPCell(new Phrase("Document: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project Report", font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 6;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Report Date
                string reportDate = DateTime.Today.ToString(Common.ExportReportDateFormat);
                
                cell = new PdfPCell(new Phrase("Report Generated on: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 1;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 6;
                cell.PaddingBottom = 5;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion

                foreach (var c in result)
                {

                    if (c != null)
                    {
                        #region Checklist Report Header                        
                        string DOB_ReportHeader = string.Empty;
                        if (!string.IsNullOrEmpty(c.Others))
                        { DOB_ReportHeader = c.applicationName + " - " + c.Others; }
                        else
                        { DOB_ReportHeader = c.applicationName; }

                        cell = new PdfPCell(new Phrase(DOB_ReportHeader, font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.TOP_BORDER;
                        cell.Colspan = 7;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.PaddingTop = 5;
                        cell.PaddingBottom = 10;
                        table.AddCell(cell);
                        #endregion
                        foreach (var g in c.groups)
                        {
                            #region group header
                            string Group_ReportHeader = g.checkListGroupName;
                            cell = new PdfPCell(new Phrase(Group_ReportHeader, font_12_Bold));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.Border = PdfPCell.TOP_BORDER;
                            cell.Colspan = 7;
                            cell.PaddingBottom = 5;
                            table.AddCell(cell);
                            #endregion
                            if (g.checkListGroupType.ToLower().Trim() == "general")
                            {
                                #region header
                                cell = new PdfPCell(new Phrase("Item Name", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 2;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Party Responsible", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Comment", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Target Date", font_Table_Header));
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

                                cell = new PdfPCell(new Phrase("Client Note", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                #endregion
                                #region Data
                                foreach (var item in g.item)
                                {
                                    cell = new PdfPCell(new Phrase(item.checklistItemName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    table.AddCell(cell);

                                    string PartyResponsible = string.Empty;
                                    if (item.PartyResponsible.ToLower() == "rpo user")
                                        PartyResponsible = "RPO";

                                    else if (item.PartyResponsible.ToLower() == "contact")
                                    {
                                        if (item.CompanyName != null)
                                            PartyResponsible = item.ContactName + " - " + item.CompanyName;
                                        else
                                            PartyResponsible = item.ContactName;
                                    }

                                    else if (item.PartyResponsible.ToLower() == "other")
                                        PartyResponsible = item.manualPartyResponsible;
                                    else
                                        PartyResponsible = "";

                                    cell = new PdfPCell(new Phrase(PartyResponsible, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.comments, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.dueDate != null ? Convert.ToDateTime(item.dueDate).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    table.AddCell(cell);

                                    string Status = string.Empty;

                                    if (item.status == 1)
                                        Status = "Open";
                                    else if (item.status == 2)
                                        Status = "InProcess";
                                    else if (item.status == 3)
                                        Status = "Completed";
                                    else
                                        Status = "";

                                    cell = new PdfPCell(new Phrase(Status, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.ClientNotes, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    table.AddCell(cell);

                                }
                                #endregion
                            }
                            else if (g.checkListGroupType.ToLower().Trim() == "tr")
                            {
                                #region header
                                cell = new PdfPCell(new Phrase("Item Name", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Design Applicant", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Inspector", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Comment", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Stage", font_Table_Header));
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

                                cell = new PdfPCell(new Phrase("Client Note", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);
                                #endregion
                                #region Data
                                foreach (var item in g.item)
                                {
                                    cell = new PdfPCell(new Phrase(item.checklistItemName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.DesignApplicantName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.InspectorName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.comments, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    string Stage = string.Empty;
                                    if (item.stage == "1")
                                        Stage = "Prior to Approval";

                                    else if (item.stage == "2")
                                        Stage = "Prior to Permit";

                                    else if (item.stage == "3")
                                        Stage = "Prior to Sign Off";
                                    else
                                        Stage = "";
                                    cell = new PdfPCell(new Phrase(Stage, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    string Status = string.Empty;

                                    if (item.status == 1)
                                        Status = "Open";
                                    else if (item.status == 2)
                                        Status = "Completed";
                                    else
                                        Status = "";

                                    cell = new PdfPCell(new Phrase(Status, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.ClientNotes, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    table.AddCell(cell);
                                }
                                #endregion
                            }
                            else if (g.checkListGroupType.ToLower().Trim() == "pl")
                            {
                                #region floor header
                                foreach (var item in g.item)
                                {

                                    cell = new PdfPCell(new Phrase("Floor - " + item.FloorNumber, font_12_Bold));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cell.Border = PdfPCell.TOP_BORDER;
                                    cell.Colspan = 7;
                                    cell.PaddingBottom = 5;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Inspection Type", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Comment", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Status", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Client Note", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);

                                    foreach (var d in item.Details)
                                    {
                                        #region Data
                                        cell = new PdfPCell(new Phrase(d.inspectionPermit, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 2;
                                        table.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(d.plComments, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 2;
                                        table.AddCell(cell);
                                        string PLStatus = string.Empty;
                                        if (d.result == "1")
                                            PLStatus = "Pass";
                                        else if (d.result == "2")
                                            PLStatus = "Failed";
                                        else if (d.result == "3")
                                            PLStatus = "Pending";
                                        else if (d.result == "4")
                                            PLStatus = "NA";
                                        else
                                            PLStatus = "";
                                        cell = new PdfPCell(new Phrase(PLStatus, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 1;
                                        table.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(item.ClientNotes, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 2;
                                        table.AddCell(cell);
                                        #endregion

                                    }

                                }
                                #endregion
                            }
                        }
                    }
                }

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }

    }
}