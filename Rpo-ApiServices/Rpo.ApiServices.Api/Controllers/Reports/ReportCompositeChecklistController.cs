
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
    using CompositeChecklist.Models;

    [Authorize]
    public class ReportCompositeChecklistController : ApiController
    {

        RpoContext db = new RpoContext();
        private object CellStyle;
        /// <summary>
        ///  Get the report of ReportOverallPermitExpiry Report with filter and sorting  and export to excel
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/Checklist/ExportCompositeChecklistToExcel")]
        public IHttpActionResult PostExportCompositeChecklistToExcel(CompositeChecklistReportDatatableParameter dataTableParameters)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[3];

                spParameter[0] = new SqlParameter("@IdCompositeChecklist", SqlDbType.NVarChar);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = dataTableParameters.IdCompositeChecklist;

                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.OrderFlag;

                spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = !string.IsNullOrEmpty(dataTableParameters.SearchText) ? dataTableParameters.SearchText : string.Empty;

                int Idcompo = Convert.ToInt32(dataTableParameters.IdCompositeChecklist);

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "CompositeView", spParameter);
                List<CompositeChecklistReportDTO> headerlist = new List<CompositeChecklistReportDTO>();

                List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
                        }).Select(g => g.CopyToDataTable()).ToList();
                try

                {
                    foreach (var loopheader in distinctheaders)
                    {
                        var headers = dataTableParameters.lstexportChecklist.Select(x => x.jobChecklistHeaderId).ToList();
                        if (headers.Contains(Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"])))
                        {
                            CompositeChecklistReportDTO header = new CompositeChecklistReportDTO();
                            Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                            header.IdCompositeChecklist = loopheader.Rows[0]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdCompositeChecklist"]);
                            header.CompositeChecklistName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                            header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                            header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                            header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdJob"]);                           
                            header.IsCOProject = loopheader.Rows[0]["IsCOProject"] == DBNull.Value ? (bool)false : Convert.ToBoolean(loopheader.Rows[0]["IsCOProject"]);                           
                            header.IsParentCheckList = db.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == header.jobChecklistHeaderId && x.IdCompositeChecklist == Idcompo).Select(y => y.IsParentCheckList).FirstOrDefault();
                            var checklist = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId).FirstOrDefault();
                            if (header.IsParentCheckList == true)
                            {
                                header.CompositeOrder = 1;
                                string name = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                                header.CompositeChecklistName = "CC - " + name;
                            }
                            else
                            {
                                header.CompositeOrder = checklist.Displayorder + 1;
                                header.CompositeChecklistName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                            }
                            header.groups = new List<CompositeReportChecklistGroup>();
                            List<CompositeReportChecklistGroup> Unorderedgroups = new List<CompositeReportChecklistGroup>();
                            string s = null;
                            int id = checklistHeaderId;
                            var temp = db.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").Where(x => x.IdJobCheckListHeader == id).ToList();
                            var lstidjobworktype = temp[0].JobApplicationWorkPermitTypes.Where(x => x.Code.ToLower().Trim() == "pl" || x.Code.ToLower().Trim() == "sp" || x.Code.ToLower().Trim() == "sd").Select(y => y.IdJobWorkType).ToList();
                            foreach (var i in lstidjobworktype)
                            {
                                s += i.ToString() + ",";
                            }
                            if (s != null)
                                header.IdWorkPermits = s.Remove(s.Length - 1, 1);
                            if (s != null)
                                header.IdWorkPermits = s.Remove(s.Length - 1, 1);

                            var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                                .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();
                            foreach (var eachGroup in distinctGroup)
                            {                                
                                var selectedgroups = dataTableParameters.lstexportChecklist.Where(z => z.jobChecklistHeaderId == checklistHeaderId).Select(x => x.lstExportChecklistGroup.Select(y => y.jobChecklistGroupId).ToList()).FirstOrDefault();
                                if (selectedgroups.Contains(Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"])))
                                {
                                    int groupid = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    CompositeReportChecklistGroup group = new CompositeReportChecklistGroup();
                                    group.item = new List<CompositeReportItem>();
                                   
                                    Int32 IdJobChecklistGroup = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    group.checkListGroupName = eachGroup.Rows[0]["ChecklistGroupName"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["ChecklistGroupName"].ToString();
                                    group.jobChecklistGroupId = eachGroup.Rows[0]["IdJobChecklistGroup"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    group.checkListGroupType = eachGroup.Rows[0]["checklistGroupType"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["checklistGroupType"].ToString();
                                    var GroupInPayload = dataTableParameters.lstexportChecklist.Where(x => x.jobChecklistHeaderId == checklistHeaderId).Select(z => z.lstExportChecklistGroup).FirstOrDefault();
                                    var groupfordisplayorder = GroupInPayload.Where(x => x.jobChecklistGroupId == IdJobChecklistGroup).FirstOrDefault();
                                    group.DisplayOrder1 = groupfordisplayorder.displayOrder1;                                    
                                    if (eachGroup.Rows[0]["checklistGroupType"].ToString().ToLower().TrimEnd() != "pl")
                                    {
                                        var groupsitems = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
                                        for (int j = 0; j < groupsitems.Count; j++)
                                        {

                                            CompositeReportItem item = new CompositeReportItem();
                                            item.Details = new List<CompositeReportDetails>();

                                            #region Items
                                            Int32 IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                            if (IdChecklistItem != 0)
                                            {
                                                item.checklistItemName = groupsitems[j]["checklistItemName"] == DBNull.Value ? string.Empty : groupsitems[j]["checklistItemName"].ToString();
                                                item.IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                                item.jocbChecklistItemDetailsId = groupsitems[j]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["JocbChecklistItemDetailsId"]);
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
                                                item.CompositeName = groupsitems[j]["CompositeName"] == DBNull.Value ? string.Empty : groupsitems[j]["CompositeName"].ToString();
                                                item.IdCompositeChecklist = groupsitems[j]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCompositeChecklist"]);
                                                item.IsRequiredForTCO = groupsitems[j]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsRequiredForTCO"]);
                                                item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                                item.HasDocument = item.HasDocument = db.JobDocuments.Any(x => x.IdJobchecklistItemDetails == item.jocbChecklistItemDetailsId);
                                                item.IsParentCheckList = groupsitems[j]["IsParentCheckList"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsParentCheckList"]);

                                                if (item.idContact != null && item.idContact != 0)
                                                {
                                                    var c = db.Contacts.Find(item.idContact);
                                                    item.ContactName = c.FirstName + " " + c.LastName;
                                                    if (c.IdCompany != null && c.IdCompany != 0)
                                                    {
                                                        item.CompanyName = db.Companies.Where(x => x.Id == c.IdCompany).Select(y => y.Name).FirstOrDefault();
                                                    }
                                                }
                                                if (item.idDesignApplicant != null && item.idDesignApplicant != 0)
                                                {
                                                    var c = db.JobContacts.Find(item.idDesignApplicant);
                                                    if (c != null)
                                                    {
                                                        item.DesignApplicantName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                                    }
                                                }
                                                if (item.idInspector != null && item.idInspector != 0)
                                                {
                                                    var c = db.JobContacts.Find(item.idInspector);
                                                    if (c != null)
                                                    {
                                                        item.InspectorName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                                    }
                                                }
                                                #endregion
                                                if (dataTableParameters.OnlyTCOItems == true)
                                                {
                                                    if (item.IsRequiredForTCO == true)
                                                    {
                                                        group.item.Add(item);
                                                    }
                                                }
                                                else
                                                {
                                                    group.item.Add(item);
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {

                                        var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();

                                        var PlumbingCheckListFloors = groupsitems2.AsEnumerable().Select(a => a.Field<Int32?>("IdJobPlumbingCheckListFloors")).Distinct().ToList();

                                        for (int j = 0; j < PlumbingCheckListFloors.Count(); j++)
                                        {

                                            CompositeReportItem item = new CompositeReportItem();
                                            item.Details = new List<CompositeReportDetails>();
                                            var IdJobPlumbingCheckListFloors1 = PlumbingCheckListFloors[j];
                                            var detailItem = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobPlumbingCheckListFloors") == IdJobPlumbingCheckListFloors1).ToList();
                                            #region Items
                                            item.FloorNumber = detailItem[0]["FloorNumber"] == DBNull.Value ? string.Empty : detailItem[0]["FloorNumber"].ToString();
                                            item.idJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors1 != 0 ? IdJobPlumbingCheckListFloors1 : 0;
                                            item.FloorDisplayOrder = detailItem[0]["FloorDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[0]["FloorDisplayOrder"]);
                                            #endregion

                                            for (int i = 0; i < detailItem.Count; i++)
                                            {
                                                CompositeReportDetails detail = new CompositeReportDetails();
                                                #region Items details
                                                detail.checklistGroupType = detailItem[i]["checklistGroupType"] == DBNull.Value ? string.Empty : detailItem[i]["checklistGroupType"].ToString();
                                                detail.idJobPlumbingInspection = detailItem[i]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingInspection"]);
                                                detail.idJobPlumbingCheckListFloors = detailItem[i]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingCheckListFloors"]);
                                                detail.inspectionPermit = detailItem[i]["checklistItemName"] == DBNull.Value ? string.Empty : detailItem[i]["checklistItemName"].ToString();
                                                detail.workOrderNumber = detailItem[i]["WorkOrderNo"] == DBNull.Value ? string.Empty : detailItem[i]["WorkOrderNo"].ToString();
                                                detail.plComments = detailItem[i]["Comments"] == DBNull.Value ? string.Empty : detailItem[i]["Comments"].ToString();
                                                detail.DueDate = detailItem[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["DueDate"]);
                                                detail.nextInspection = detailItem[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["NextInspection"]);
                                                detail.result = detailItem[i]["Result"] == DBNull.Value ? string.Empty : detailItem[i]["Result"].ToString();
                                                detail.IsRequiredForTCO = detailItem[i]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(detailItem[i]["IsRequiredForTCO"]);
                                                detail.HasDocument = db.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                                if (dataTableParameters.OnlyTCOItems == true)
                                                {
                                                    if (detail.IsRequiredForTCO == true)
                                                    {
                                                        item.Details.Add(detail);                                                      
                                                    }
                                                }
                                                else
                                                {
                                                    item.Details.Add(detail);                                                    
                                                }                                                
                                                #endregion

                                            }
                                            group.item.Add(item);
                                        }
                                    }

                                    Unorderedgroups.Add(group);                                   
                                }
                            }
                            header.groups = Unorderedgroups.OrderBy(x => x.DisplayOrder1).ToList();
                            headerlist.Add(header);
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    return this.NotFound();
                }
              
                #region Excel
                var result = headerlist.OrderBy(x => x.CompositeOrder).ToList();
                if (result != null && result.Count > 0)
                {
                    string exportFilename = string.Empty;
                    exportFilename = "Composite_ChecklistReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                    string exportFilePath = ExportToExcel(result, exportFilename, dataTableParameters.IncludeViolations, dataTableParameters.OnlyTCOItems);

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

        private string ExportToExcel(List<CompositeChecklistReportDTO> result, string exportFilename, bool isViolation, bool OnlyTCOItems)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
            if (result[0].IsCOProject == true)
                templateFileName = "Composite_TCO_Template.xlsx";
            else
                templateFileName = "Composite_Template.xlsx";

            string templateFilePath = templatePath + templateFileName;
            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateFilePath);
            int ProjectNumber = result.Select(x => x.IdJob).FirstOrDefault().Value;
            int sheetIndex = 0;
            if (sheetIndex == 0)
            {
                templateWorkbook.SetSheetName(sheetIndex, "Composite-Checklist");
                           
                sheetIndex = sheetIndex + 1;
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
            leftAlignCellStyle.VerticalAlignment = VerticalAlignment.Top;
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
            right_JobDetailAlignCellStyle.SetFont(myFont_Bold);
            right_JobDetailAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.WrapText = true;
            right_JobDetailAlignCellStyle.VerticalAlignment = VerticalAlignment.Center;
            right_JobDetailAlignCellStyle.Alignment = HorizontalAlignment.Right;

            ISheet sheet = templateWorkbook.GetSheet("Composite-Checklist");

            Job job = db.Jobs.Include("RfpAddress.Borough").Include("Company").Include("ProjectManager").FirstOrDefault(x => x.Id == ProjectNumber);

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
            CellRangeAddress DOBCellRangeAddress;
            DOBCellRangeAddress = new CellRangeAddress(1, 1, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iprojectAddressRow.GetCell(1) != null)
            {
                iprojectAddressRow.GetCell(1).SetCellValue(projectAddress);
            }
            else
            {
                iprojectAddressRow.CreateCell(1).SetCellValue(projectAddress);
            }

            iprojectAddressRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iprojectAddressRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iprojectAddressRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;


            #endregion

            #region ProjectNumber           
            IRow iProjectNumberRow = sheet.GetRow(2);

            if (iProjectNumberRow == null)
            {
                iProjectNumberRow = sheet.CreateRow(2);
            }
            DOBCellRangeAddress = new CellRangeAddress(2, 2, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iProjectNumberRow.GetCell(1) != null)
            {
                iProjectNumberRow.GetCell(1).SetCellValue(ProjectNumber);
            }
            else
            {
                iProjectNumberRow.CreateCell(1).SetCellValue(ProjectNumber);
            }

            iProjectNumberRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iProjectNumberRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iProjectNumberRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;
            #endregion

            #region Client
            IRow iClientRow = sheet.GetRow(3);

            string clientName = job != null && job.Company != null ? job.Company.Name : string.Empty;
            if (iClientRow == null)
            {
                iClientRow = sheet.CreateRow(3);
            }
            DOBCellRangeAddress = new CellRangeAddress(3, 3, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iClientRow.GetCell(1) != null)
            {
                iClientRow.GetCell(1).SetCellValue(clientName);
            }
            else
            {
                iClientRow.CreateCell(1).SetCellValue(clientName);
            }

            iClientRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iClientRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iClientRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;

            #endregion

            #region Project Manager

            string ProjectManagerName = job.ProjectManager != null ? job.ProjectManager.FirstName + " " + job.ProjectManager.LastName + " | " + job.ProjectManager.Email : string.Empty;
            IRow iProjectManagerRow = sheet.GetRow(4);
            if (iProjectManagerRow == null)
            {
                iProjectManagerRow = sheet.CreateRow(4);
            }
            DOBCellRangeAddress = new CellRangeAddress(4, 4, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iProjectManagerRow.GetCell(1) != null)
            {
                iProjectManagerRow.GetCell(1).SetCellValue(ProjectManagerName);
            }
            else
            {
                iProjectManagerRow.CreateCell(1).SetCellValue(ProjectManagerName);
            }

            iProjectManagerRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iProjectManagerRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iProjectManagerRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;

            #endregion

            #region Document

            string DocumentName = "Project Report";

            IRow iDocumentRow = sheet.GetRow(5);
            if (iDocumentRow == null)
            {
                iDocumentRow = sheet.CreateRow(5);
            }
            DOBCellRangeAddress = new CellRangeAddress(5, 5, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iDocumentRow.GetCell(1) != null)
            {
                iDocumentRow.GetCell(1).SetCellValue(DocumentName);
            }
            else
            {
                iDocumentRow.CreateCell(1).SetCellValue(DocumentName);
            }

            iDocumentRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iDocumentRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iDocumentRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;

            #endregion

            #region Report Date

            string reportDate = DateTime.Today.ToString(Common.ExportReportDateFormat);
            IRow iDateRow = sheet.GetRow(6);
            if (iDateRow == null)
            {
                iDateRow = sheet.CreateRow(6);
            }
            DOBCellRangeAddress = new CellRangeAddress(6, 6, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iDateRow.GetCell(1) != null)
            {
                iDateRow.GetCell(1).SetCellValue(reportDate);
            }
            else
            {
                iDateRow.CreateCell(1).SetCellValue(reportDate);
            }

            iDateRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iDateRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iDateRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;

            #endregion
            int index = 8;
            foreach (var c in result)
            {

                if (c != null)
                {
                    #region Checklist Report Header

                    XSSFFont ChecklistReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    //  ChecklistReportHeaderFont.FontHeightInPoints = (short)12;
                    ChecklistReportHeaderFont.FontHeightInPoints = (short)14;
                    ChecklistReportHeaderFont.FontName = "Times New Roman";
                    // ChecklistReportHeaderFont.FontName = "sans-serif";
                    ChecklistReportHeaderFont.IsBold = true;

                    XSSFFont ChecklistColumnHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    ChecklistColumnHeaderFont.FontHeightInPoints = (short)12;
                    ChecklistColumnHeaderFont.FontName = "Times New Roman";
                    // ChecklistReportHeaderFont.FontName = "sans-serif";
                    ChecklistColumnHeaderFont.IsBold = true;

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
                    { DOB_ReportHeader = c.CompositeChecklistName + " - " + c.Others; }
                    else
                    { DOB_ReportHeader = c.CompositeChecklistName; }


                    //string DOB_ReportHeader = c.CompositeChecklistName;
                    IRow iDOB_ReportHeaderRow = sheet.GetRow(index);
                    if (iDOB_ReportHeaderRow == null)
                    {
                        iDOB_ReportHeaderRow = sheet.CreateRow(index);
                    }
                    iDOB_ReportHeaderRow.HeightInPoints = (float)19.50;

                    //  if (c.groups.Select(x => x.checkListGroupType).ToList().Contains("PL") && c.IsParentCheckList==false)
                    //  {
                    //if (iDOB_ReportHeaderRow.GetCell(0) != null)
                    //{
                    //    iDOB_ReportHeaderRow.GetCell(0).SetCellValue("Plumbing Checklist:- " + DOB_ReportHeader);
                    //}
                    //else
                    //{

                    //    iDOB_ReportHeaderRow.CreateCell(0).SetCellValue("Plumbing Checklist:- " + DOB_ReportHeader);
                    //}
                    //  }
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

                    if (c.IsCOProject == true)
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                    else
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    iDOB_ReportHeaderRow.GetCell(0).CellStyle = ChecklistReportHeaderCellStyle;

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    // index = index + 2;
                    index = index + 1;
                    #endregion


                    XSSFFont DOBHeaderFont = (XSSFFont)templateWorkbook.CreateFont();                   
                    DOBHeaderFont.FontHeightInPoints = (short)14;
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
                        if (c.IsCOProject == true)
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                        else
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
                        ColumnHeaderCellStyle.SetFont(ChecklistColumnHeaderFont);
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
                        if (g.checkListGroupType.ToLower().Trim() == "general")
                        {

                            #region header

                            IRow iDOB_HeaderRow = sheet.GetRow(index);                           

                            if (iDOB_HeaderRow == null)
                            {
                                iDOB_HeaderRow = sheet.CreateRow(index);
                            }
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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


                            if (iDOB_HeaderRow.GetCell(3) != null)
                            {
                                iDOB_HeaderRow.GetCell(3).SetCellValue("Party Responsible");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(3).SetCellValue("Party Responsible");
                            }
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 4, 5);
                            sheet.AddMergedRegion(DOBCellRangeAddress);
                            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            if (iDOB_HeaderRow.GetCell(4) != null)
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue("Comments");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("Comments");
                            }

                            if (iDOB_HeaderRow.GetCell(6) != null)
                            {
                                iDOB_HeaderRow.GetCell(6).SetCellValue("Target Date");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(6).SetCellValue("Target Date");
                            }

                            if (iDOB_HeaderRow.GetCell(7) != null)
                            {
                                iDOB_HeaderRow.GetCell(7).SetCellValue("Status");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("Status");
                            }

                            if (c.IsCOProject == true)
                            {
                                if (iDOB_HeaderRow.GetCell(8) != null)
                                {
                                    iDOB_HeaderRow.GetCell(8).SetCellValue("TCO?");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("TCO?");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
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
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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
                                if (iRow.GetCell(3) != null)
                                {
                                    if (item.PartyResponsible.ToLower() == "rpo user")
                                        iRow.GetCell(3).SetCellValue("RPO");

                                    else if (item.PartyResponsible.ToLower() == "contact")
                                    {
                                        if (item.CompanyName != null)
                                            iRow.GetCell(3).SetCellValue(item.ContactName + " - " + item.CompanyName);
                                        else
                                            iRow.GetCell(3).SetCellValue(item.ContactName);
                                    }                                   

                                    else if (item.PartyResponsible.ToLower() == "other")
                                        iRow.GetCell(3).SetCellValue(item.manualPartyResponsible);
                                    else
                                        iRow.GetCell(3).SetCellValue("");
                                }
                                else
                                {
                                    if (item.PartyResponsible.ToLower() == "rpo user")
                                        iRow.CreateCell(3).SetCellValue("RPO ");

                                    else if (item.PartyResponsible.ToLower() == "contact")
                                    {
                                        if (item.CompanyName != null)
                                            iRow.CreateCell(3).SetCellValue(item.ContactName + " - " + item.CompanyName);
                                        else
                                            iRow.CreateCell(3).SetCellValue(item.ContactName);
                                    }

                                    else if (item.PartyResponsible.ToLower() == "other")
                                        iRow.CreateCell(3).SetCellValue(item.manualPartyResponsible);
                                    else
                                        iRow.CreateCell(3).SetCellValue("");
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 4, 5);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iRow.GetCell(4) != null)
                                {
                                    iRow.GetCell(4).SetCellValue(item.comments);
                                }
                                else
                                {
                                    iRow.CreateCell(4).SetCellValue(item.comments);
                                }

                                // string workDescription = item.JobWorkTypeDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);

                                if (iRow.GetCell(6) != null)
                                {
                                    iRow.GetCell(6).SetCellValue(item.dueDate != null ? Convert.ToDateTime(item.dueDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                                }
                                else
                                {
                                    iRow.CreateCell(6).SetCellValue(item.dueDate != null ? Convert.ToDateTime(item.dueDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                                }
                                //Temporary commented
                                if (iRow.GetCell(7) != null)
                                {
                                    if (item.status == 1)
                                        iRow.GetCell(7).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.GetCell(7).SetCellValue("InProcess");
                                    else if (item.status == 3)
                                        iRow.GetCell(7).SetCellValue("Completed");
                                    else
                                        iRow.GetCell(7).SetCellValue("");
                                }
                                else
                                {
                                    if (item.status == 1)
                                        iRow.CreateCell(7).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.CreateCell(7).SetCellValue("InProcess");
                                    else if (item.status == 3)
                                        iRow.CreateCell(7).SetCellValue("Completed");
                                    else
                                        iRow.CreateCell(7).SetCellValue("");
                                }
                                if (c.IsCOProject == true)
                                {
                                    if (iRow.GetCell(8) != null)
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            iRow.GetCell(8).SetCellValue("YES");
                                        else
                                            iRow.GetCell(8).SetCellValue("NO");

                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
                                    else
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            iRow.CreateCell(8).SetCellValue("YES");
                                        else
                                            iRow.CreateCell(8).SetCellValue("NO");

                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
                                }
                                iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(7).CellStyle = leftAlignCellStyle;
                                //iRow.GetCell(8).CellStyle = leftAlignCellStyle;


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
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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

                            if (iDOB_HeaderRow.GetCell(3) != null)
                            {
                                iDOB_HeaderRow.GetCell(3).SetCellValue("Design Applicant");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(3).SetCellValue("Design Applicant");
                            }

                            if (iDOB_HeaderRow.GetCell(4) != null)
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue("Inspector");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("Inspector");
                            }
                            if (iDOB_HeaderRow.GetCell(5) != null)
                            {
                                iDOB_HeaderRow.GetCell(5).SetCellValue("Comments");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(5).SetCellValue("Comments");
                            }
                            if (iDOB_HeaderRow.GetCell(6) != null)
                            {
                                iDOB_HeaderRow.GetCell(6).SetCellValue("Stage");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(6).SetCellValue("Stage");
                            }

                            if (iDOB_HeaderRow.GetCell(7) != null)
                            {
                                iDOB_HeaderRow.GetCell(7).SetCellValue("Status");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("Status");
                            }
                           
                            if (c.IsCOProject == true)
                            {
                                if (iDOB_HeaderRow.GetCell(8) != null)
                                {
                                    iDOB_HeaderRow.GetCell(8).SetCellValue("TCO?");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("TCO?");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
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
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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
                                if (iRow.GetCell(3) != null)
                                {                                   
                                    iRow.GetCell(3).SetCellValue(item.DesignApplicantName);                                    
                                }
                                else
                                {
                                    iRow.CreateCell(3).SetCellValue(item.DesignApplicantName);
                                }

                                if (iRow.GetCell(4) != null)
                                {
                                    iRow.GetCell(4).SetCellValue(item.InspectorName);
                                }
                                else
                                {
                                    iRow.CreateCell(4).SetCellValue(item.InspectorName);
                                }
                                if (iRow.GetCell(5) != null)
                                {
                                    iRow.GetCell(5).SetCellValue(item.comments);
                                }
                                else
                                {
                                    iRow.CreateCell(5).SetCellValue(item.comments);
                                }
                               
                                if (iRow.GetCell(6) != null)
                                {                                   
                                    if (item.stage == "1")
                                        iRow.GetCell(6).SetCellValue("Prior to Approval");
                                    else if (item.stage == "2")
                                        iRow.GetCell(6).SetCellValue("Prior to Permit");
                                    else if (item.stage == "3")
                                        iRow.GetCell(6).SetCellValue("Prior to Sign Off");
                                    else
                                        iRow.GetCell(6).SetCellValue("");
                                }
                                else
                                {
                                    if (item.stage == "1")
                                        iRow.CreateCell(6).SetCellValue("Prior to Approval");
                                    else if (item.stage == "2")
                                        iRow.CreateCell(6).SetCellValue("Prior to Permit");
                                    else if (item.stage == "3")
                                        iRow.CreateCell(6).SetCellValue("Prior to Sign Off");
                                    else
                                        iRow.CreateCell(6).SetCellValue("");
                                }
                                //Temporary commented
                                if (iRow.GetCell(7) != null)
                                {                                   
                                    if (item.status == 1)
                                        iRow.GetCell(7).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.GetCell(7).SetCellValue("Completed");
                                    else
                                        iRow.GetCell(7).SetCellValue("");
                                }
                                else
                                {
                                    if (item.status == 1)
                                        iRow.CreateCell(7).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.CreateCell(7).SetCellValue("Completed");
                                    else
                                        iRow.CreateCell(7).SetCellValue("");
                                }

                                if (c.IsCOProject == true)
                                {
                                    if (iRow.GetCell(8) != null)
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            iRow.GetCell(8).SetCellValue("YES");
                                        else
                                            iRow.GetCell(8).SetCellValue("NO");

                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
                                    else
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            iRow.CreateCell(8).SetCellValue("YES");
                                        else
                                            iRow.CreateCell(8).SetCellValue("NO");

                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
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
                                if (c.IsCOProject == true)
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                                else
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                                sheet.AddMergedRegion(DOBCellRangeAddress);

                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iDOB_FloorRow.GetCell(0) != null)
                                {
                                    iDOB_FloorRow.GetCell(0).SetCellValue("Floor - " + item.FloorNumber);
                                }
                                else
                                {
                                    iDOB_FloorRow.CreateCell(0).SetCellValue("Floor - " + item.FloorNumber);
                                }
                                iDOB_FloorRow.GetCell(0).CellStyle = GroupHeaderCellStyle;
                                #endregion                             
                                index = index + 1;
                                IRow iDOB_HeaderRow = sheet.GetRow(index);

                                if (iDOB_HeaderRow == null)
                                {
                                    iDOB_HeaderRow = sheet.CreateRow(index);
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 3, 5);
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
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 6, 7);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iDOB_HeaderRow.GetCell(6) != null)
                                {
                                    iDOB_HeaderRow.GetCell(6).SetCellValue("Status");
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(6).SetCellValue("Status");
                                }
                                if (c.IsCOProject == true)
                                {
                                    if (iDOB_HeaderRow.GetCell(8) != null)
                                    {
                                        iDOB_HeaderRow.GetCell(8).SetCellValue("TCO?");
                                        iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                    }
                                    else
                                    {
                                        iDOB_HeaderRow.CreateCell(8).SetCellValue("TCO?");
                                        iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                    }
                                }
                                iDOB_HeaderRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(6).CellStyle = ColumnHeaderCellStyle;
                                index = index + 1;
                                foreach (var d in item.Details)
                                {
                                    #region Data                                   
                                    IRow iRow = sheet.GetRow(index);
                                    if (iRow == null)
                                    {
                                        iRow = sheet.CreateRow(index);
                                    }

                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 3, 5);
                                    sheet.AddMergedRegion(DOBCellRangeAddress);
                                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    if (iRow.GetCell(3) != null)
                                    {
                                        iRow.GetCell(3).SetCellValue(d.plComments);

                                    }
                                    else
                                    {
                                        iRow.CreateCell(3).SetCellValue(d.plComments);
                                    }                               
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 6, 7);
                                    //  }
                                    sheet.AddMergedRegion(DOBCellRangeAddress);
                                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    if (iRow.GetCell(6) != null)
                                    {
                                        if (d.result == "1")
                                            iRow.GetCell(6).SetCellValue("Pass");
                                        else if (d.result == "2")
                                            iRow.GetCell(6).SetCellValue("Failed");
                                        else if (d.result == "3")
                                            iRow.GetCell(6).SetCellValue("Pending");
                                        else if (d.result == "4")
                                            iRow.GetCell(6).SetCellValue("NA");
                                        else
                                            iRow.GetCell(6).SetCellValue(""); 
                                    }
                                    else
                                    {
                                        if (d.result == "1")
                                            iRow.CreateCell(6).SetCellValue("Pass");
                                        else if (d.result == "2")
                                            iRow.CreateCell(6).SetCellValue("Failed");
                                        else if (d.result == "3")
                                            iRow.CreateCell(6).SetCellValue("Pending");
                                        else if (d.result == "4")
                                            iRow.CreateCell(6).SetCellValue("NA");
                                        else
                                            iRow.CreateCell(6).SetCellValue("");
                                    }

                                    if (c.IsCOProject == true)
                                    {
                                        if (iRow.GetCell(8) != null)
                                        {
                                            if (d.IsRequiredForTCO == true)
                                                iRow.GetCell(8).SetCellValue("YES");
                                            else
                                                iRow.GetCell(8).SetCellValue("NO");

                                            iRow.GetCell(8).CellStyle = leftAlignCellStyle;

                                        }
                                        else
                                        {
                                            if (d.IsRequiredForTCO == true)
                                                iRow.CreateCell(8).SetCellValue("YES");
                                            else
                                                iRow.CreateCell(8).SetCellValue("NO");

                                            iRow.GetCell(8).CellStyle = leftAlignCellStyle;

                                        }
                                    }                                    
                                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(7).CellStyle = leftAlignCellStyle;                                    
                                    index = index + 1;
                                }

                            }
                        }
                    }

                }
                index = index + 1;

                #endregion
            }

            if (isViolation)
            {             
                int Idrfpaddress = db.Jobs.Find(ProjectNumber).IdRfpAddress;
                string BinNumber = db.RfpAddresses.Where(x => x.Id == Idrfpaddress).Select(y => y.BinNumber).FirstOrDefault();
                int? IdCompositeChecklist = result[0].IdCompositeChecklist;
                #region ECB Violations    
                List<JobViolation> lstjobECBviolation = new List<JobViolation>();
                var CompositeECBViolations = db.CompositeViolations.Where(x => x.IdCompositeChecklist == IdCompositeChecklist).ToList();
                //int IdEmployee;
                //IdEmployee = db.Employees.Where(x => x.FirstName.ToLower() == "super" && x.LastName.ToLower() == "admin").FirstOrDefault().Id;
                foreach (var c in CompositeECBViolations)
                {
                    JobViolation violation = new JobViolation();
                    if (OnlyTCOItems == true)
                    {
                        violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations && x.Type_ECB_DOB == "ECB" && x.TCOToggle == true).FirstOrDefault();
                    }
                    else
                    {
                        violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations && x.Type_ECB_DOB == "ECB").FirstOrDefault();
                    }
                    if (violation != null)
                        lstjobECBviolation.Add(violation);
                }
                if (lstjobECBviolation != null && lstjobECBviolation.Count > 0)
                {
                    XSSFFont ChecklistViolationReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();                   
                    ChecklistViolationReportHeaderFont.FontHeightInPoints = (short)14;
                    ChecklistViolationReportHeaderFont.FontName = "Times New Roman";                   
                    ChecklistViolationReportHeaderFont.IsBold = true;
                    #region group header
                    XSSFCellStyle GroupHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    GroupHeaderCellStyle.SetFont(ChecklistViolationReportHeaderFont);
                    GroupHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.WrapText = true;
                    GroupHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    GroupHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                    GroupHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.White);
                    GroupHeaderCellStyle.FillPattern = FillPattern.SolidForeground;                   
                    IRow Group_ReportHeaderRow = sheet.GetRow(index);
                    if (Group_ReportHeaderRow == null)
                    {
                        Group_ReportHeaderRow = sheet.CreateRow(index);
                    }
                    Group_ReportHeaderRow.HeightInPoints = (float)19.50;

                    if (Group_ReportHeaderRow.GetCell(0) != null)
                    {
                        Group_ReportHeaderRow.GetCell(0).SetCellValue("ECB/OATH Violations");
                    }
                    else
                    {
                        Group_ReportHeaderRow.CreateCell(0).SetCellValue("ECB/OATH Violations");
                    }
                    if (result[0].IsCOProject == true)
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                    else
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    Group_ReportHeaderRow.GetCell(0).CellStyle = GroupHeaderCellStyle;

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);                    
                    index = index + 1;
                    #endregion

                    #region column header

                    IRow iDOB_HeaderRow = sheet.GetRow(index);

                    if (iDOB_HeaderRow == null)
                    {
                        iDOB_HeaderRow = sheet.CreateRow(index);
                    }

                    if (iDOB_HeaderRow.GetCell(0) != null)
                    {
                        iDOB_HeaderRow.GetCell(0).SetCellValue("Issue Date");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(0).SetCellValue("Issue Date");
                    }

                    if (iDOB_HeaderRow.GetCell(1) != null)
                    {
                        iDOB_HeaderRow.GetCell(1).SetCellValue("ECB Violation No.");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(1).SetCellValue("ECB Violation No.");
                    }
                    //DOBCellRangeAddress = new CellRangeAddress(index, index, 1, 2);
                    //sheet.AddMergedRegion(DOBCellRangeAddress);
                    //RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    //RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    //RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    //RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    if (iDOB_HeaderRow.GetCell(2) != null)
                    {
                        iDOB_HeaderRow.GetCell(2).SetCellValue("Violation Description");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(2).SetCellValue("Violation Description");
                    }
                    DOBCellRangeAddress = new CellRangeAddress(index, index, 2, 3);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    if (iDOB_HeaderRow.GetCell(4) != null)
                    {
                        iDOB_HeaderRow.GetCell(4).SetCellValue("Party Responsible");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(4).SetCellValue("Party Responsible");
                    }
                   
                    if (iDOB_HeaderRow.GetCell(5) != null)
                    {
                        iDOB_HeaderRow.GetCell(5).SetCellValue("Comments");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(5).SetCellValue("Comments");
                    }
                    DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    if (iDOB_HeaderRow.GetCell(7) != null)
                    {
                        iDOB_HeaderRow.GetCell(7).SetCellValue("Violation Status");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(7).SetCellValue("Violation Status");
                    }
                    #region Column Header cell style
                    XSSFFont ChecklistColumnHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    ChecklistColumnHeaderFont.FontHeightInPoints = (short)12;
                    ChecklistColumnHeaderFont.FontName = "Times New Roman";                    
                    ChecklistColumnHeaderFont.IsBold = true;

                    XSSFCellStyle ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    ColumnHeaderCellStyle.SetFont(ChecklistColumnHeaderFont);
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
                    XSSFCellStyle GrayCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    GrayCellStyle.SetFont(ChecklistColumnHeaderFont);
                    GrayCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    GrayCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    GrayCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    GrayCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    GrayCellStyle.WrapText = true;
                    GrayCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    GrayCellStyle.Alignment = HorizontalAlignment.Left;
                    GrayCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(221, 221, 221));
                    //  ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(226, 239, 228));
                    GrayCellStyle.FillPattern = FillPattern.SolidForeground;
                    if (result[0].IsCOProject == true)
                    {
                        if (iDOB_HeaderRow.GetCell(8) != null)
                        {
                            iDOB_HeaderRow.GetCell(8).SetCellValue("TCO?");
                            iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(8).SetCellValue("TCO?");
                            iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                        }
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

                    lstjobECBviolation = lstjobECBviolation.OrderByDescending(x => x.DateIssued).ToList();
                    foreach (var jobViolation in lstjobECBviolation)
                    {

                        #region Data

                        iDOB_HeaderRow = sheet.GetRow(index);

                        if (iDOB_HeaderRow == null)
                        {
                            iDOB_HeaderRow = sheet.CreateRow(index);
                        }

                        if (iDOB_HeaderRow.GetCell(0) != null)
                        {

                            iDOB_HeaderRow.GetCell(0).SetCellValue(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(0).SetCellValue(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        //DOBCellRangeAddress = new CellRangeAddress(index, index, 1, 2);
                        //sheet.AddMergedRegion(DOBCellRangeAddress);
                        //RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                        if (iDOB_HeaderRow.GetCell(1) != null)
                        {
                            iDOB_HeaderRow.GetCell(1).SetCellValue(jobViolation.SummonsNumber != null ? jobViolation.SummonsNumber.ToString() : "");
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(1).SetCellValue(jobViolation.SummonsNumber != null ? jobViolation.SummonsNumber.ToString() : "");
                        }

                        if (iDOB_HeaderRow.GetCell(2) != null)
                        {
                            iDOB_HeaderRow.GetCell(2).SetCellValue(jobViolation.ViolationDescription); //violation description
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(2).SetCellValue(jobViolation.ViolationDescription);
                        }
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 2, 3);
                        sheet.AddMergedRegion(DOBCellRangeAddress);
                        RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                        if (iDOB_HeaderRow.GetCell(4) != null)
                        {
                            if (jobViolation.PartyResponsible == 1)
                                iDOB_HeaderRow.GetCell(4).SetCellValue("RPO");
                            else if (jobViolation.PartyResponsible == 3)
                                iDOB_HeaderRow.GetCell(4).SetCellValue(jobViolation.ManualPartyResponsible);
                            else
                                iDOB_HeaderRow.GetCell(4).SetCellValue("");
                        }
                        else
                        {
                            if (jobViolation.PartyResponsible == 1)
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("RPO");
                            else if (jobViolation.PartyResponsible == 3)
                                iDOB_HeaderRow.CreateCell(4).SetCellValue(jobViolation.ManualPartyResponsible);
                            else
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("");
                        }
                        var comment = (from cmt in db.ChecklistJobViolationComments
                                       where cmt.IdJobViolation == jobViolation.Id
                                       orderby cmt.LastModifiedDate descending
                                       select cmt.Description).FirstOrDefault();
                        if (iDOB_HeaderRow.GetCell(5) != null)
                        {

                            iDOB_HeaderRow.GetCell(5).SetCellValue(comment);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(5).SetCellValue(comment);
                        }
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                        sheet.AddMergedRegion(DOBCellRangeAddress);
                        RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                        //if (iDOB_HeaderRow.GetCell(5) != null)
                        //{
                        //    iDOB_HeaderRow.GetCell(5).SetCellValue(jobViolation.CertificationStatus);
                        //}
                        //else
                        //{
                        //    iDOB_HeaderRow.CreateCell(5).SetCellValue(jobViolation.CertificationStatus);
                        //}

                        //commented due to certification status change
                        //if (iDOB_HeaderRow.GetCell(7) != null)
                        //{
                        //    if (jobViolation.Status == 1)
                        //        iDOB_HeaderRow.GetCell(7).SetCellValue("Open");
                        //    else if (jobViolation.Status == 3)
                        //        iDOB_HeaderRow.GetCell(7).SetCellValue("Completed");
                        //    else
                        //        iDOB_HeaderRow.GetCell(7).SetCellValue("");
                        //}
                        //else
                        //{
                        //    if (jobViolation.Status == 1)
                        //        iDOB_HeaderRow.CreateCell(7).SetCellValue("Open");
                        //    else if (jobViolation.Status == 3)
                        //        iDOB_HeaderRow.CreateCell(7).SetCellValue("Completed");
                        //    else
                        //        iDOB_HeaderRow.CreateCell(7).SetCellValue("");
                        //}
                        if (iDOB_HeaderRow.GetCell(7) != null)
                        {
                            //if (!string.IsNullOrEmpty(jobViolation.CertificationStatus))
                                iDOB_HeaderRow.GetCell(7).SetCellValue(jobViolation.CertificationStatus);
                        }
                        else
                        {
                            //if (!string.IsNullOrEmpty(jobViolation.CertificationStatus))
                                iDOB_HeaderRow.CreateCell(7).SetCellValue(jobViolation.CertificationStatus);
                        }
                    
                        //if(jobViolation.CertificationStatus== "N/A-DISMISSED" || jobViolation.CertificationStatus == "CERTIFICATE ACCEPTED"||jobViolation.CertificationStatus== "CURE ACCEPTED"|| jobViolation.CertificationStatus=="COMPLIANCE INSP / DOC")
                        //iDOB_HeaderRow.GetCell(7).CellStyle = GrayCellStyle;
                      

                        #region Column Header cell style
                        ChecklistColumnHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                        ChecklistColumnHeaderFont.FontHeightInPoints = (short)12;
                        ChecklistColumnHeaderFont.FontName = "Times New Roman";                       
                        ChecklistColumnHeaderFont.IsBold = true;

                        ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                        ColumnHeaderCellStyle.SetFont(ChecklistColumnHeaderFont);
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
                        if (result[0].IsCOProject == true)
                        {

                            if (iDOB_HeaderRow.GetCell(8) != null)
                            {
                                if (OnlyTCOItems == true)
                                {
                                    if (jobViolation.TCOToggle == true)
                                        iDOB_HeaderRow.GetCell(8).SetCellValue("YES");
                                }
                                else
                                {
                                    if (jobViolation.TCOToggle == true)
                                        iDOB_HeaderRow.GetCell(8).SetCellValue("YES");
                                    else
                                        iDOB_HeaderRow.GetCell(8).SetCellValue("NO");
                                }
                                iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;
                            }
                            else
                            {
                                if (jobViolation.TCOToggle == true)
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("YES");
                                else
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("NO");
                                iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;
                            }
                        }
                        bool Isgray = false;
                        if (!string.IsNullOrEmpty(jobViolation.CertificationStatus))
                        {
                            // if (jobViolation.CertificationStatus.ToLower() == "n/a - dismissed" || jobViolation.CertificationStatus.ToLower() == "certificate accepted" || jobViolation.CertificationStatus.ToLower() == "cure accepted" || jobViolation.CertificationStatus.ToLower() == "no compliance recorded")
                            if (jobViolation.CertificationStatus.ToLower() == "n/a - dismissed" || jobViolation.CertificationStatus.ToLower() == "certificate accepted" || jobViolation.CertificationStatus.ToLower() == "cure accepted" || jobViolation.CertificationStatus.ToLower() == "compliance-insp/doc")
                            {
                                Isgray = true;
                            }
                        }
                           if(Isgray)
                           { 
                                iDOB_HeaderRow.GetCell(0).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(1).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(2).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(3).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(4).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(5).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(6).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(7).CellStyle = GrayCellStyle;
                                //iDOB_HeaderRow.GetCell(8).CellStyle = GrayCellStyle;
                            }
                            else
                            {
                                iDOB_HeaderRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(6).CellStyle = leftAlignCellStyle;                         
                                iDOB_HeaderRow.GetCell(7).CellStyle = leftAlignCellStyle;
                            }
              


                        index = index + 1;
                        #endregion                       

                    }
                }
                #endregion

                #region DOB Violations 
                List<JobViolation> lstjobDOBviolation = new List<JobViolation>();               
                var CompositeDOBViolations = db.CompositeViolations.Where(x => x.IdCompositeChecklist == IdCompositeChecklist).ToList();
                foreach (var c in CompositeDOBViolations)
                {
                    JobViolation violation = new JobViolation();
                    if (OnlyTCOItems == true)
                    {
                        violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations && x.Type_ECB_DOB == "DOB" && x.TCOToggle == true).FirstOrDefault();
                    }
                    else
                    {
                        violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations && x.Type_ECB_DOB == "DOB").FirstOrDefault();
                    }
                    //  var violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "DOB").Include("ChecklistJobViolationComment").FirstOrDefault();
                    //  var violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "DOB").FirstOrDefault();
                    if (violation != null)
                        lstjobDOBviolation.Add(violation);
                }
                if (lstjobDOBviolation != null && lstjobDOBviolation.Count > 0)
                {
                    //IRow ViolationheaderRow = sheet.GetRow(index);
                    //if (ViolationheaderRow == null)
                    //{
                    //    ViolationheaderRow = sheet.CreateRow(index);
                    //}
                    //ViolationheaderRow.HeightInPoints = (float)19.50;

                    XSSFFont ChecklistViolationReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();                   
                    ChecklistViolationReportHeaderFont.FontHeightInPoints = (short)14;
                    ChecklistViolationReportHeaderFont.FontName = "Times New Roman";                   
                    ChecklistViolationReportHeaderFont.IsBold = true;                   
                    XSSFCellStyle GroupHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    GroupHeaderCellStyle.SetFont(ChecklistViolationReportHeaderFont);
                    GroupHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.WrapText = true;
                    GroupHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    GroupHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                    GroupHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.White);
                    GroupHeaderCellStyle.FillPattern = FillPattern.SolidForeground;                    

                    //  ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(226, 239, 228));
                  
                    #region group header
                    IRow Group_ReportHeaderRow = sheet.GetRow(index);
                    if (Group_ReportHeaderRow == null)
                    {
                        Group_ReportHeaderRow = sheet.CreateRow(index);
                    }
                    Group_ReportHeaderRow.HeightInPoints = (float)19.50;

                    if (Group_ReportHeaderRow.GetCell(0) != null)
                    {
                        Group_ReportHeaderRow.GetCell(0).SetCellValue("DOB Violations");
                    }
                    else
                    {
                        Group_ReportHeaderRow.CreateCell(0).SetCellValue("DOB Violations");
                    }
                    if (result[0].IsCOProject == true)
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                    else
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    Group_ReportHeaderRow.GetCell(0).CellStyle = GroupHeaderCellStyle;

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    index = index + 1;
                    #endregion

                    #region column header DOB

                    IRow iDOB_HeaderRow = sheet.GetRow(index); 
                    if (iDOB_HeaderRow == null)
                    {
                        iDOB_HeaderRow = sheet.CreateRow(index);
                    }                    
                    if (iDOB_HeaderRow.GetCell(0) != null)
                    {

                        iDOB_HeaderRow.GetCell(0).SetCellValue("Issue Date");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(0).SetCellValue("Issue Date");
                    }

                    if (iDOB_HeaderRow.GetCell(1) != null)
                    {
                        iDOB_HeaderRow.GetCell(1).SetCellValue("DOB Violation No.");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(1).SetCellValue("DOB Violation No.");
                    }

                    if (iDOB_HeaderRow.GetCell(2) != null)
                    {
                        iDOB_HeaderRow.GetCell(2).SetCellValue("Related ECB No.");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(2).SetCellValue("Related ECB No.");
                    }                   

                    if (iDOB_HeaderRow.GetCell(3) != null)
                    {
                        iDOB_HeaderRow.GetCell(3).SetCellValue("Violation Description");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(3).SetCellValue("Violation Description");
                    }

                    if (iDOB_HeaderRow.GetCell(4) != null)
                    {
                        iDOB_HeaderRow.GetCell(4).SetCellValue("Party Responsible");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(4).SetCellValue("Party Responsible");
                    }
                    DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    if (iDOB_HeaderRow.GetCell(5) != null)
                    {
                        iDOB_HeaderRow.GetCell(5).SetCellValue("Comments");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(5).SetCellValue("Comments");
                    }
                    if (iDOB_HeaderRow.GetCell(7) != null)
                    {
                        iDOB_HeaderRow.GetCell(7).SetCellValue("Violation Status");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(7).SetCellValue("Violation Status");
                    }

                    #region Column Header cell style
                    XSSFFont ChecklistColumnHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    ChecklistColumnHeaderFont.FontHeightInPoints = (short)12;
                    ChecklistColumnHeaderFont.FontName = "Times New Roman";                   
                    ChecklistColumnHeaderFont.IsBold = true;

                    XSSFCellStyle ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    ColumnHeaderCellStyle.SetFont(ChecklistColumnHeaderFont);
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
                    XSSFCellStyle GrayCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    GrayCellStyle.SetFont(ChecklistColumnHeaderFont);
                    GrayCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    GrayCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    GrayCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    GrayCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    GrayCellStyle.WrapText = true;
                    GrayCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    GrayCellStyle.Alignment = HorizontalAlignment.Left;
                    GrayCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(221, 221, 221));
                    GrayCellStyle.FillPattern = FillPattern.SolidForeground;
                    if (result[0].IsCOProject == true)
                    {
                        if (iDOB_HeaderRow.GetCell(8) != null)
                        {
                            iDOB_HeaderRow.GetCell(8).SetCellValue("TCO?");
                            iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(8).SetCellValue("TCO?");
                            iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                        }
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

                    #region Data DOB
                    lstjobDOBviolation = lstjobDOBviolation.OrderByDescending(x => x.DateIssued).ToList();
                    foreach (var jobViolation in lstjobDOBviolation)
                    {

                        #region Data

                        iDOB_HeaderRow = sheet.GetRow(index);

                        if (iDOB_HeaderRow == null)
                        {
                            iDOB_HeaderRow = sheet.CreateRow(index);
                        }
                        //DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                        //sheet.AddMergedRegion(DOBCellRangeAddress);
                        //RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        if (iDOB_HeaderRow.GetCell(0) != null)
                        {

                            iDOB_HeaderRow.GetCell(0).SetCellValue(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(0).SetCellValue(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }


                        if (iDOB_HeaderRow.GetCell(1) != null)
                        {
                            iDOB_HeaderRow.GetCell(1).SetCellValue(jobViolation.SummonsNumber);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(1).SetCellValue(jobViolation.SummonsNumber);
                        }

                        if (iDOB_HeaderRow.GetCell(2) != null)
                        {
                            iDOB_HeaderRow.GetCell(2).SetCellValue(jobViolation.ECBnumber != null ? jobViolation.ECBnumber.ToString() : "");
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(2).SetCellValue(jobViolation.ECBnumber != null ? jobViolation.ECBnumber.ToString() : "");
                        }
                        //DOBCellRangeAddress = new CellRangeAddress(index, index, 3, 4);
                        //sheet.AddMergedRegion(DOBCellRangeAddress);
                        //RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                        if (iDOB_HeaderRow.GetCell(3) != null)
                        {
                            iDOB_HeaderRow.GetCell(3).SetCellValue(jobViolation.ViolationDescription);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(3).SetCellValue(jobViolation.ViolationDescription);
                        }
                        if (iDOB_HeaderRow.GetCell(4) != null)
                        {
                            if (jobViolation.PartyResponsible == 1)
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue("RPO");
                            }
                            else if (jobViolation.PartyResponsible == 3)
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue(jobViolation.ManualPartyResponsible);
                            }
                            else
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue("");
                            }
                        }
                        else
                        {
                            if (jobViolation.PartyResponsible == 1)
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("RPO Team");
                            }
                            else if (jobViolation.PartyResponsible == 3)
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue(jobViolation.ManualPartyResponsible);
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("");
                            }
                        }
                        var comment = (from cmt in db.ChecklistJobViolationComments
                                       where cmt.IdJobViolation == jobViolation.Id
                                       orderby cmt.LastModifiedDate descending
                                       select cmt.Description).FirstOrDefault();
                        if (iDOB_HeaderRow.GetCell(5) != null)
                        {

                            iDOB_HeaderRow.GetCell(5).SetCellValue(comment);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(5).SetCellValue(comment);
                        }
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                        sheet.AddMergedRegion(DOBCellRangeAddress);
                        RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);


                    //commented 29-1 for certification status
                    //if (iDOB_HeaderRow.GetCell(7) != null)
                    //{
                    //    if (jobViolation.Status == 1)
                    //        iDOB_HeaderRow.GetCell(7).SetCellValue("Open");
                    //    else if (jobViolation.Status == 3)
                    //        iDOB_HeaderRow.GetCell(7).SetCellValue("Completed");
                    //    else
                    //        iDOB_HeaderRow.GetCell(7).SetCellValue("");
                    //}
                    //else
                    //{
                    //    if (jobViolation.Status == 1)
                    //        iDOB_HeaderRow.CreateCell(7).SetCellValue("Open");
                    //    else if (jobViolation.Status == 3)
                    //        iDOB_HeaderRow.CreateCell(7).SetCellValue("Completed");
                    //    else
                    //        iDOB_HeaderRow.CreateCell(7).SetCellValue("");
                    //}
                    if (iDOB_HeaderRow.GetCell(7) != null)
                    {
                        iDOB_HeaderRow.GetCell(7).SetCellValue(jobViolation.violation_category);
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(7).SetCellValue(jobViolation.violation_category);
                    }
                        #region Column Header cell style
                        //ChecklistColumnHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                        //ChecklistColumnHeaderFont.FontHeightInPoints = (short)12;
                        //ChecklistColumnHeaderFont.FontName = "Times New Roman";
                        //// ChecklistReportHeaderFont.FontName = "sans-serif";
                        //ChecklistColumnHeaderFont.IsBold = true;

                        //ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                        //ColumnHeaderCellStyle.SetFont(ChecklistColumnHeaderFont);
                        //ColumnHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                        //ColumnHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                        //ColumnHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                        //ColumnHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                        //ColumnHeaderCellStyle.WrapText = true;
                        //ColumnHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                        //ColumnHeaderCellStyle.Alignment = HorizontalAlignment.Left;
                        //ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                        ////  ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(226, 239, 218));
                        //ColumnHeaderCellStyle.FillPattern = FillPattern.SolidForeground;
                        #endregion
                        if (result[0].IsCOProject == true)
                        {
                            if (iDOB_HeaderRow.GetCell(8) != null)
                            {

                                if (jobViolation.TCOToggle == true)
                                    iDOB_HeaderRow.GetCell(8).SetCellValue("YES");
                                else
                                    iDOB_HeaderRow.GetCell(8).SetCellValue("No");

                                iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;


                            }
                            else
                            {
                                if (jobViolation.TCOToggle == true)
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("YES");
                                else
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("No");


                                iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;
                            }
                        }
                        //iDOB_HeaderRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        //iDOB_HeaderRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        //iDOB_HeaderRow.GetCell(2).CellStyle = leftAlignCellStyle;

                        //iDOB_HeaderRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        //iDOB_HeaderRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        //iDOB_HeaderRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        //iDOB_HeaderRow.GetCell(6).CellStyle = leftAlignCellStyle;
                        //iDOB_HeaderRow.GetCell(7).CellStyle = leftAlignCellStyle;
                        bool isgrayed = false;
                        if (!string.IsNullOrEmpty(jobViolation.violation_category))
                        {
                            if (jobViolation.violation_category.ToLower().Contains("dismissed") || jobViolation.violation_category.ToLower().Contains("resolved"))
                            {
                                isgrayed = true;
                            }
                        }
                           if(isgrayed)
                            { 
                                iDOB_HeaderRow.GetCell(0).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(1).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(2).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(3).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(4).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(5).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(6).CellStyle = GrayCellStyle;
                                iDOB_HeaderRow.GetCell(7).CellStyle = GrayCellStyle;
                                //iDOB_HeaderRow.GetCell(8).CellStyle = GrayCellStyle;
                            }
                            else
                            {
                                iDOB_HeaderRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(6).CellStyle = leftAlignCellStyle;
                                iDOB_HeaderRow.GetCell(7).CellStyle = leftAlignCellStyle;
                            }
               
                        // iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;

                        //iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;


                        index = index + 1;
                        #endregion

                    }
                    #endregion


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
        /// Get the report of Checklist Report with filter and sorting  and export to PDF
        /// </summary>
        /// <param name="dataTableParameters"></param>
        /// <returns></returns>

        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/Checklist/ExportCompositeChecklistToPDF")]
        public IHttpActionResult ExportChecklistToPDF(CompositeChecklistReportDatatableParameter dataTableParameters)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
            var employee = this.db.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[3];

                spParameter[0] = new SqlParameter("@IdCompositeChecklist", SqlDbType.NVarChar);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = dataTableParameters.IdCompositeChecklist;

                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.OrderFlag;

                spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = !string.IsNullOrEmpty(dataTableParameters.SearchText) ? dataTableParameters.SearchText : string.Empty;

                int Idcompo = Convert.ToInt32(dataTableParameters.IdCompositeChecklist);

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "CompositeView", spParameter);
                List<CompositeChecklistReportDTO> headerlist = new List<CompositeChecklistReportDTO>();

                List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
                        }).Select(g => g.CopyToDataTable()).ToList();
                try

                {
                    foreach (var loopheader in distinctheaders)
                    {
                        var headers = dataTableParameters.lstexportChecklist.Select(x => x.jobChecklistHeaderId).ToList();
                        if (headers.Contains(Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"])))
                        {
                            CompositeChecklistReportDTO header = new CompositeChecklistReportDTO();
                            Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                            header.IdCompositeChecklist = loopheader.Rows[0]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdCompositeChecklist"]);                            
                            header.CompositeChecklistName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                            header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                            header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                            header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                            header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdJob"]);
                            header.IsCOProject = loopheader.Rows[0]["IsCOProject"] == DBNull.Value ? (bool)false : Convert.ToBoolean(loopheader.Rows[0]["IsCOProject"]);                           
                            header.IsParentCheckList = db.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == header.jobChecklistHeaderId && x.IdCompositeChecklist == Idcompo).Select(y => y.IsParentCheckList).FirstOrDefault();             
                            var checklist = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId).FirstOrDefault();
                            if (header.IsParentCheckList == true)
                            {
                                header.CompositeOrder = 1;
                                string name = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                                header.CompositeChecklistName = "CC - " + name;
                            }
                            else
                            {
                                header.CompositeOrder = checklist.Displayorder + 1;
                                header.CompositeChecklistName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                            }
                            header.groups = new List<CompositeReportChecklistGroup>();
                            List<CompositeReportChecklistGroup> Unorderedgroups = new List<CompositeReportChecklistGroup>();
                            string s = null;
                            int id = checklistHeaderId;
                            var temp = db.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").Where(x => x.IdJobCheckListHeader == id).ToList();
                            var lstidjobworktype = temp[0].JobApplicationWorkPermitTypes.Where(x => x.Code.ToLower().Trim() == "pl" || x.Code.ToLower().Trim() == "sp" || x.Code.ToLower().Trim() == "sd").Select(y => y.IdJobWorkType).ToList();
                            foreach (var i in lstidjobworktype)
                            {
                                s += i.ToString() + ",";
                            }
                            if (s != null)
                                header.IdWorkPermits = s.Remove(s.Length - 1, 1);
                            if (s != null)
                                header.IdWorkPermits = s.Remove(s.Length - 1, 1);

                            var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                                .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();
                            foreach (var eachGroup in distinctGroup)
                            {                               
                                var selectedgroups = dataTableParameters.lstexportChecklist.Where(z => z.jobChecklistHeaderId == checklistHeaderId).Select(x => x.lstExportChecklistGroup.Select(y => y.jobChecklistGroupId).ToList()).FirstOrDefault();
                                if (selectedgroups.Contains(Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"])))
                                {
                                    int groupid = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    CompositeReportChecklistGroup group = new CompositeReportChecklistGroup();
                                    group.item = new List<CompositeReportItem>();                                   
                                    Int32 IdJobChecklistGroup = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    group.checkListGroupName = eachGroup.Rows[0]["ChecklistGroupName"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["ChecklistGroupName"].ToString();
                                    group.jobChecklistGroupId = eachGroup.Rows[0]["IdJobChecklistGroup"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    group.checkListGroupType = eachGroup.Rows[0]["checklistGroupType"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["checklistGroupType"].ToString();
                                    var GroupInPayload = dataTableParameters.lstexportChecklist.Where(x => x.jobChecklistHeaderId == checklistHeaderId).Select(z => z.lstExportChecklistGroup).FirstOrDefault();
                                    var groupfordisplayorder = GroupInPayload.Where(x => x.jobChecklistGroupId == IdJobChecklistGroup).FirstOrDefault();
                                    group.DisplayOrder1 = groupfordisplayorder.displayOrder1;                                    
                                    if (eachGroup.Rows[0]["checklistGroupType"].ToString().ToLower().TrimEnd() != "pl")
                                    {
                                        var groupsitems = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
                                        for (int j = 0; j < groupsitems.Count; j++)
                                        {

                                            CompositeReportItem item = new CompositeReportItem();
                                            item.Details = new List<CompositeReportDetails>();

                                            #region Items
                                            Int32 IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                            if (IdChecklistItem != 0)
                                            {
                                                item.checklistItemName = groupsitems[j]["checklistItemName"] == DBNull.Value ? string.Empty : groupsitems[j]["checklistItemName"].ToString();
                                                item.IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                                item.jocbChecklistItemDetailsId = groupsitems[j]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["JocbChecklistItemDetailsId"]);
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
                                                item.CompositeName = groupsitems[j]["CompositeName"] == DBNull.Value ? string.Empty : groupsitems[j]["CompositeName"].ToString();
                                                item.IdCompositeChecklist = groupsitems[j]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCompositeChecklist"]);
                                                item.IsRequiredForTCO = groupsitems[j]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsRequiredForTCO"]);
                                                item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                                item.HasDocument = item.HasDocument = db.JobDocuments.Any(x => x.IdJobchecklistItemDetails == item.jocbChecklistItemDetailsId);
                                                item.IsParentCheckList = groupsitems[j]["IsParentCheckList"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsParentCheckList"]);



                                                if (item.idContact != null && item.idContact != 0)
                                                {
                                                    var c = db.Contacts.Find(item.idContact);
                                                    item.ContactName = c.FirstName + " " + c.LastName;
                                                    if (c.IdCompany != null && c.IdCompany != 0)
                                                    {
                                                        item.CompanyName = db.Companies.Where(x => x.Id == c.IdCompany).Select(y => y.Name).FirstOrDefault();
                                                    }
                                                }
                                                if (item.idDesignApplicant != null && item.idDesignApplicant != 0)
                                                {
                                                    var c = db.JobContacts.Find(item.idDesignApplicant);
                                                    if (c != null)
                                                    {
                                                        item.DesignApplicantName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                                    }
                                                }
                                                if (item.idInspector != null && item.idInspector != 0)
                                                {
                                                    var c = db.JobContacts.Find(item.idInspector);
                                                    if (c != null)
                                                    {
                                                        item.InspectorName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                                    }
                                                }
                                                #endregion
                                                if (dataTableParameters.OnlyTCOItems == true)
                                                {
                                                    if (item.IsRequiredForTCO == true)
                                                    {
                                                        group.item.Add(item);
                                                    }
                                                }
                                                else
                                                {
                                                    group.item.Add(item);
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {

                                        var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();

                                        var PlumbingCheckListFloors = groupsitems2.AsEnumerable().Select(a => a.Field<Int32?>("IdJobPlumbingCheckListFloors")).Distinct().ToList();

                                        for (int j = 0; j < PlumbingCheckListFloors.Count(); j++)
                                        {

                                            CompositeReportItem item = new CompositeReportItem();
                                            item.Details = new List<CompositeReportDetails>();
                                            var IdJobPlumbingCheckListFloors1 = PlumbingCheckListFloors[j];
                                            var detailItem = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobPlumbingCheckListFloors") == IdJobPlumbingCheckListFloors1).ToList();
                                            #region Items
                                            item.FloorNumber = detailItem[0]["FloorNumber"] == DBNull.Value ? string.Empty : detailItem[0]["FloorNumber"].ToString();
                                            item.idJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors1 != 0 ? IdJobPlumbingCheckListFloors1 : 0;
                                            item.FloorDisplayOrder = detailItem[0]["FloorDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[0]["FloorDisplayOrder"]);
                                            #endregion

                                            for (int i = 0; i < detailItem.Count; i++)
                                            {
                                                CompositeReportDetails detail = new CompositeReportDetails();
                                                #region Items details
                                                detail.checklistGroupType = detailItem[i]["checklistGroupType"] == DBNull.Value ? string.Empty : detailItem[i]["checklistGroupType"].ToString();
                                                detail.idJobPlumbingInspection = detailItem[i]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingInspection"]);
                                                detail.idJobPlumbingCheckListFloors = detailItem[i]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingCheckListFloors"]);
                                                detail.inspectionPermit = detailItem[i]["checklistItemName"] == DBNull.Value ? string.Empty : detailItem[i]["checklistItemName"].ToString();
                                                detail.workOrderNumber = detailItem[i]["WorkOrderNo"] == DBNull.Value ? string.Empty : detailItem[i]["WorkOrderNo"].ToString();
                                                detail.plComments = detailItem[i]["Comments"] == DBNull.Value ? string.Empty : detailItem[i]["Comments"].ToString();
                                                detail.DueDate = detailItem[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["DueDate"]);
                                                detail.nextInspection = detailItem[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["NextInspection"]);
                                                detail.result = detailItem[i]["Result"] == DBNull.Value ? string.Empty : detailItem[i]["Result"].ToString();
                                                detail.IsRequiredForTCO = detailItem[i]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(detailItem[i]["IsRequiredForTCO"]);
                                                detail.HasDocument = db.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                                if (dataTableParameters.OnlyTCOItems == true)
                                                {
                                                    if (detail.IsRequiredForTCO == true)
                                                    {
                                                        item.Details.Add(detail);                                                       
                                                    }
                                                }
                                                else
                                                {
                                                    item.Details.Add(detail);                                                    
                                                }                                                
                                                #endregion

                                            }
                                            group.item.Add(item);
                                        }
                                    }
                                    Unorderedgroups.Add(group);                                    
                                }
                            }
                            header.groups = Unorderedgroups.OrderBy(x => x.DisplayOrder1).ToList();
                            headerlist.Add(header);
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    return this.NotFound();
                }
               
                #region PDF
                var result = headerlist.OrderBy(x => x.CompositeOrder).ToList();

                if (result != null && result.Count > 0)
                {
                    string exportFilename = string.Empty;

                    exportFilename = "CompositeChecklistReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    string exportFilePath = ExportToPdf(result, exportFilename, dataTableParameters.IncludeViolations, dataTableParameters.OnlyTCOItems);
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
        private string ExportToPdf(List<CompositeChecklistReportDTO> result, string exportFilename, bool isViolation, bool OnlyTCOItems)
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
                int templateColumns;
                if (result[0].IsCOProject == true)
                { templateColumns = 13; }
                else
                { templateColumns = 12; }


                PdfPTable table = new PdfPTable(templateColumns);
                table.WidthPercentage = 100;
                table.SplitLate = false;
                Job job = db.Jobs.Include("RfpAddress.Borough").Include("Company").Include("ProjectManager").FirstOrDefault(x => x.Id == ProjectNumber);


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
                if (result[0].IsCOProject == true)
                    cell.PaddingLeft = -5;
                else
                    cell.PaddingLeft = -10;
                cell.Colspan = templateColumns;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                if (result[0].IsCOProject == true)
                    cell.PaddingLeft = -5;
                else
                    cell.PaddingLeft = -10;
                cell.Colspan = templateColumns;
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
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(projectAddress, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.Padding = -15;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                #endregion
                #region ProjectNumber

                cell = new PdfPCell(new Phrase("RPO Project Number: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(ProjectNumber), font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.Padding = -15;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Client
                string clientName = job != null && job.Company != null ? job.Company.Name : string.Empty;
                cell = new PdfPCell(new Phrase("Client: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(clientName, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.Padding = -15;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Project Manager
                string ProjectManagerName = job.ProjectManager != null ? job.ProjectManager.FirstName + " " + job.ProjectManager.LastName + " | " + job.ProjectManager.Email : string.Empty;
                cell = new PdfPCell(new Phrase("Project Manager: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(ProjectManagerName, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.Padding = -15;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Document
                cell = new PdfPCell(new Phrase("Document: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project Report", font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.Padding = -15;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Report Date
                string reportDate = DateTime.Today.ToString(Common.ExportReportDateFormat);
                cell = new PdfPCell(new Phrase("Report Generated on: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.PaddingBottom = 5;
                cell.Padding = -15;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion

                foreach (var c in result)
                {

                    if (c != null)
                    {
                        #region Checklist Report Header
                        string DOB_ReportHeader = string.Empty; ;
                        //string DOB_ReportHeader = string.Empty;
                        if (!string.IsNullOrEmpty(c.Others))
                        { DOB_ReportHeader = c.CompositeChecklistName + " - " + c.Others; }
                        else
                        { DOB_ReportHeader = c.CompositeChecklistName; }
                        //if (c.groups.Select(x => x.checkListGroupType).ToList().Contains("PL") && c.IsParentCheckList == false)
                        //{
                        //    DOB_ReportHeader = "Plumbing Checklist:- " + c.CompositeChecklistName;
                        //}
                        //else
                        //{ DOB_ReportHeader = c.CompositeChecklistName; }
                        cell = new PdfPCell(new Phrase(DOB_ReportHeader, font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.TOP_BORDER;
                        cell.Colspan = templateColumns;
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
                            cell.Colspan = templateColumns;
                            cell.PaddingBottom = 5;
                            table.AddCell(cell);
                            #endregion
                            if (g.checkListGroupType.ToLower().Trim() == "general")
                            {
                                #region header
                                cell = new PdfPCell(new Phrase("Item Name", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 4;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Party Responsible", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 3;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Comments", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 3;
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

                                if (c.IsCOProject == true)
                                {
                                    cell = new PdfPCell(new Phrase("TCO?", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);
                                }
                                #endregion
                                #region Data
                                foreach (var item in g.item)
                                {
                                    cell = new PdfPCell(new Phrase(item.checklistItemName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 4;
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
                                    cell.Colspan = 3;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.comments, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 3;
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

                                    string TCO = string.Empty;
                                    if (c.IsCOProject == true)
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            TCO = "YES";
                                        else
                                            TCO = "NO";
                                        cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 1;
                                        table.AddCell(cell);
                                    }

                                }
                                #endregion
                            }
                            else if (g.checkListGroupType.ToLower().Trim() == "tr")
                            {
                                #region header
                                cell = new PdfPCell(new Phrase("Item Name", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 3;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Design Applicant", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 2;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Inspector", font_Table_Header));
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

                                cell = new PdfPCell(new Phrase("Stage", font_Table_Header));
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

                                if (c.IsCOProject == true)
                                {
                                    cell = new PdfPCell(new Phrase("TCO?", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);
                                }
                                #endregion
                                #region Data
                                foreach (var item in g.item)
                                {
                                    cell = new PdfPCell(new Phrase(item.checklistItemName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 3;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.DesignApplicantName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.InspectorName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.comments, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
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
                                    cell.Colspan = 2;
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
                                    cell.Colspan = 1;
                                    table.AddCell(cell);

                                    string TCO = string.Empty;
                                    if (c.IsCOProject == true)
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            TCO = "YES";
                                        else
                                            TCO = "NO";
                                        cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 1;
                                        table.AddCell(cell);
                                    }
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
                                    cell.Colspan = templateColumns;
                                    cell.PaddingBottom = 5;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Inspection Type", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 4;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Comment", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 4;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Status", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 4;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);
                                    if (c.IsCOProject == true)
                                    {
                                        cell = new PdfPCell(new Phrase("TCO?", font_Table_Header));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 1;
                                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                        table.AddCell(cell);
                                    }
                                    foreach (var d in item.Details)
                                    {
                                        #region Data
                                        cell = new PdfPCell(new Phrase(d.inspectionPermit, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 4;
                                        table.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(d.plComments, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 4;
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
                                        cell.Colspan = 4;
                                        table.AddCell(cell);

                                        string TCO = string.Empty;
                                        if (c.IsCOProject == true)
                                        {
                                            if (d.IsRequiredForTCO == true)
                                                TCO = "YES";
                                            else
                                                TCO = "NO";
                                            cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                            cell.Border = PdfPCell.BOX;
                                            cell.Colspan = 1;
                                            table.AddCell(cell);
                                        }
                                        #endregion

                                    }

                                }
                                #endregion
                            }
                        }
                    }
                }
                if (isViolation)
                {
                    int Idrfpaddress = db.Jobs.Find(ProjectNumber).IdRfpAddress;
                    string BinNumber = db.RfpAddresses.Where(x => x.Id == Idrfpaddress).Select(y => y.BinNumber).FirstOrDefault();
                    int? IdCompositeChecklist = result[0].IdCompositeChecklist;
                    #region ECB Violations             

                    List<JobViolation> lstjobECBviolation = new List<JobViolation>();
                    var CompositeECBViolations = db.CompositeViolations.Where(x => x.IdCompositeChecklist == IdCompositeChecklist).ToList();
                    //int IdEmployee;
                    //IdEmployee = db.Employees.Where(x => x.FirstName.ToLower() == "super" && x.LastName.ToLower() == "admin").FirstOrDefault().Id;
                    foreach (var c in CompositeECBViolations)
                    {
                        JobViolation violation = new JobViolation();
                        if (OnlyTCOItems == true)
                        {
                            violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations && x.Type_ECB_DOB == "ECB" && x.TCOToggle == true).FirstOrDefault();
                        }
                        else
                        {
                            violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations && x.Type_ECB_DOB == "ECB").FirstOrDefault();
                        }
                        if (violation != null)
                            lstjobECBviolation.Add(violation);
                    }
                    if (lstjobECBviolation != null && lstjobECBviolation.Count > 0)
                    {
                        cell = new PdfPCell(new Phrase("ECB/OATH Violations", font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.TOP_BORDER;
                        cell.Colspan = templateColumns;
                        cell.PaddingBottom = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Issue Date", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                       
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("ECB Violation No.", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Violation Description", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 4;
                        //cell.PaddingLeft = -50;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Party Responsible", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Comments", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Violation Status", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        if (result[0].IsCOProject == true)
                        {
                            cell = new PdfPCell(new Phrase("TCO?", font_Table_Header));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                            table.AddCell(cell);
                        }
                        lstjobECBviolation = lstjobECBviolation.OrderByDescending(x => x.DateIssued).ToList();
                        foreach (var jobViolation in lstjobECBviolation)
                        {
                            #region Data
                            bool IsGray = false;
                            if (!string.IsNullOrEmpty(jobViolation.CertificationStatus))
                            {
                                if (jobViolation.CertificationStatus.ToLower() == "n/a - dismissed" || jobViolation.CertificationStatus.ToLower() == "certificate accepted" || jobViolation.CertificationStatus.ToLower() == "cure accepted" || jobViolation.CertificationStatus.ToLower() == "compliance-insp/doc")
                                {
                                    IsGray = true;
                                }                              
                            }
                            cell = new PdfPCell(new Phrase(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            if(IsGray)
                            {
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                            }
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(jobViolation.SummonsNumber, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 2;
                            if (IsGray)
                            {
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                            }
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(jobViolation.ViolationDescription, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 4;
                            //cell.PaddingLeft = -50;
                            if (IsGray)
                            {
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                            }
                            table.AddCell(cell);

                            string PartyResponsible = string.Empty;
                            if (jobViolation.PartyResponsible == 1) //3 means other 1 means RPOteam                              
                                PartyResponsible = "RPO";
                            else if (jobViolation.PartyResponsible == 3)
                            { PartyResponsible = jobViolation.ManualPartyResponsible; }
                            cell = new PdfPCell(new Phrase(PartyResponsible, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            if (IsGray)
                            {
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                            }
                            table.AddCell(cell);


                            var comment = (from cmt in db.ChecklistJobViolationComments
                                           where cmt.IdJobViolation == jobViolation.Id
                                           orderby cmt.LastModifiedDate descending
                                           select cmt.Description).FirstOrDefault();

                            cell = new PdfPCell(new Phrase(comment, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 2;
                            if (IsGray)
                            {
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                            }
                            table.AddCell(cell);

                            string Status = string.Empty;
                            //if (jobViolation.Status == 1)
                            //    Status = "Open";
                            //else if (jobViolation.Status == 2)
                            //    Status = "InProcess";
                            //else if (jobViolation.Status == 3)
                            //    Status = "Completed";
                            //else
                            //    Status = "";
                            Status = jobViolation.CertificationStatus;
                             cell = new PdfPCell(new Phrase(Status, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 2;
                            if (IsGray)
                            {
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                            }
                            table.AddCell(cell);

                            string TCO = string.Empty;
                            if (result[0].IsCOProject == true)
                            {
                                if (OnlyTCOItems == true)
                                {
                                    if (jobViolation.TCOToggle == true)
                                        TCO = "YES";
                                    cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    if (IsGray)
                                    {
                                        cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                                    }
                                    table.AddCell(cell);
                                }
                                else
                                {
                                    if (jobViolation.TCOToggle == true)
                                        TCO = "YES";
                                    else
                                        TCO = "No";
                                    cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    if (IsGray)
                                    {
                                        cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                                    }
                                    table.AddCell(cell);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region DOB Violations 
                    List<JobViolation> lstjobDOBviolation = new List<JobViolation>();
                    //var jobDOBViolations = db.JobViolations.Where(x => x.BinNumber == BinNumber && (x.IdJob == null || x.IdJob == 0) || x.IdJob == ProjectNumber).Where(y => y.Type_ECB_DOB == "DOB").Include("CreatedByEmployee").Include("LastModifiedByEmployee").Include("explanationOfCharges").ToList();
                    var CompositeDOBViolations = db.CompositeViolations.Where(x => x.IdCompositeChecklist == IdCompositeChecklist).ToList();                    
                   
                    foreach (var c in CompositeDOBViolations)
                    {
                        JobViolation violation = new JobViolation();
                        if (OnlyTCOItems == true)
                        {
                            violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations && x.Type_ECB_DOB == "DOB" && x.TCOToggle == true).FirstOrDefault();
                        }
                        else
                        {
                            violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations && x.Type_ECB_DOB == "DOB").FirstOrDefault();
                        }
                        if (violation != null)
                            lstjobDOBviolation.Add(violation);
                    }
                    if (lstjobDOBviolation != null && lstjobDOBviolation.Count > 0)
                    {
                        cell = new PdfPCell(new Phrase("DOB Violations", font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.TOP_BORDER;
                        cell.Colspan = templateColumns;
                        cell.PaddingBottom = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Issue Date", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("DOB Violation No.", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Related ECB No.", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Violation Description", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 3;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Party Responsible", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Comments", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 3;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Violation Status", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        if (result[0].IsCOProject == true)
                        {
                            cell = new PdfPCell(new Phrase("TCO?", font_Table_Header));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                            table.AddCell(cell);
                        }
                        lstjobDOBviolation = lstjobDOBviolation.OrderByDescending(x => x.DateIssued).ToList();
                        foreach (var jobViolation in lstjobDOBviolation)
                        {
                            #region Data
                            bool Isgrayed = false;
                            if (!string.IsNullOrEmpty(jobViolation.violation_category))
                            {
                                if (jobViolation.violation_category.ToLower().Contains("dismissed") || jobViolation.violation_category.ToLower().Contains("resolved"))
                                {
                                    Isgrayed = true;
                                }
                            }
                            cell = new PdfPCell(new Phrase(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            if(Isgrayed)
                            {
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                            }
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(jobViolation.SummonsNumber, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 2;
                            if (Isgrayed)
                            {
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                            }
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(jobViolation.ECBnumber, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            if (Isgrayed)
                            {
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                            }
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(jobViolation.ViolationDescription, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 3;
                            if (Isgrayed)
                            {
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(221, 221, 221);
                            }
                            table.AddCell(cell);

                            string PartyResponsible = string.Empty;
                            if (jobViolation.PartyResponsible == 1) //3 means other 2 means RPOteam
                                PartyResponsible = "RPO";
                            else if (jobViolation.PartyResponsible == 3)
                            {
                                PartyResponsible = jobViolation.ManualPartyResponsible;
                            }
                            cell = new PdfPCell(new Phrase(PartyResponsible, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            table.AddCell(cell);

                            var comment = (from cmt in db.ChecklistJobViolationComments
                                           where cmt.IdJobViolation == jobViolation.Id
                                           orderby cmt.LastModifiedDate descending
                                           select cmt.Description).FirstOrDefault();
                            cell = new PdfPCell(new Phrase(comment, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 3;
                            table.AddCell(cell);

                            string Status = string.Empty;
                            if (jobViolation.Status == 1)
                                Status = "Open";
                            else if (jobViolation.Status == 2)
                                Status = "InProcess";
                            else if (jobViolation.Status == 3)
                                Status = "Completed";
                            else
                                Status = "";
                            cell = new PdfPCell(new Phrase(Status, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            table.AddCell(cell);

                            string TCO = string.Empty;
                            if (result[0].IsCOProject == true)
                            {
                                if (jobViolation.TCOToggle == true)
                                    TCO = "YES";
                                else
                                    TCO = "NO";
                                cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                table.AddCell(cell);
                            }
                        }
                        #endregion
                    }
                    #endregion
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
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/Checklist/ExportCompositeChecklistToExcelForCustomer")]
        public IHttpActionResult PostExportCompositeChecklistToExcelForCustomer(CompositeChecklistReportDatatableParameter dataTableParameters)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
            var employee = this.db.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[4];

                spParameter[0] = new SqlParameter("@IdCompositeChecklist", SqlDbType.NVarChar);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = dataTableParameters.IdCompositeChecklist;

                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.OrderFlag;

                spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = !string.IsNullOrEmpty(dataTableParameters.SearchText) ? dataTableParameters.SearchText : string.Empty;

                spParameter[3] = new SqlParameter("@IdCustomer", SqlDbType.Int);
                spParameter[3].Direction = ParameterDirection.Input;
                spParameter[3].Value = employee.Id;

                int Idcompo = Convert.ToInt32(dataTableParameters.IdCompositeChecklist);

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "CompositeViewForCustomer", spParameter);
                List<CompositeChecklistReportDTO> headerlist = new List<CompositeChecklistReportDTO>();

                List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
                        }).Select(g => g.CopyToDataTable()).ToList();
                try

                {
                    foreach (var loopheader in distinctheaders)
                    {
                        var headers = dataTableParameters.lstexportChecklist.Select(x => x.jobChecklistHeaderId).ToList();
                        if (headers.Contains(Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"])))
                        {
                            CompositeChecklistReportDTO header = new CompositeChecklistReportDTO();
                            Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                            header.IdCompositeChecklist = loopheader.Rows[0]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdCompositeChecklist"]);                           
                            header.CompositeChecklistName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                            header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                            header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                            header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdJob"]);
                            header.IsCOProject = loopheader.Rows[0]["IsCOProject"] == DBNull.Value ? (bool)false : Convert.ToBoolean(loopheader.Rows[0]["IsCOProject"]);                           
                            header.IsParentCheckList = db.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == header.jobChecklistHeaderId && x.IdCompositeChecklist == Idcompo).Select(y => y.IsParentCheckList).FirstOrDefault();                           
                            var checklist = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId).FirstOrDefault();
                            if (header.IsParentCheckList == true)
                            {
                                header.CompositeOrder = 1;
                                string name = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                                header.CompositeChecklistName = "CC - " + name;
                            }
                            else
                            {
                                header.CompositeOrder = checklist.Displayorder + 1;
                                header.CompositeChecklistName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                            }
                            header.groups = new List<CompositeReportChecklistGroup>();
                            List<CompositeReportChecklistGroup> Unorderedgroups = new List<CompositeReportChecklistGroup>();
                            string s = null;
                            int id = checklistHeaderId;
                            var temp = db.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").Where(x => x.IdJobCheckListHeader == id).ToList();
                            var lstidjobworktype = temp[0].JobApplicationWorkPermitTypes.Where(x => x.Code.ToLower().Trim() == "pl" || x.Code.ToLower().Trim() == "sp" || x.Code.ToLower().Trim() == "sd").Select(y => y.IdJobWorkType).ToList();
                            foreach (var i in lstidjobworktype)
                            {
                                s += i.ToString() + ",";
                            }
                            if (s != null)
                                header.IdWorkPermits = s.Remove(s.Length - 1, 1);
                            if (s != null)
                                header.IdWorkPermits = s.Remove(s.Length - 1, 1);

                            var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                                .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();
                            foreach (var eachGroup in distinctGroup)
                            {                                
                                var selectedgroups = dataTableParameters.lstexportChecklist.Where(z => z.jobChecklistHeaderId == checklistHeaderId).Select(x => x.lstExportChecklistGroup.Select(y => y.jobChecklistGroupId).ToList()).FirstOrDefault();
                                if (selectedgroups.Contains(Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"])))
                                {
                                    int groupid = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    CompositeReportChecklistGroup group = new CompositeReportChecklistGroup();
                                    group.item = new List<CompositeReportItem>();                                   
                                    Int32 IdJobChecklistGroup = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    group.checkListGroupName = eachGroup.Rows[0]["ChecklistGroupName"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["ChecklistGroupName"].ToString();
                                    group.jobChecklistGroupId = eachGroup.Rows[0]["IdJobChecklistGroup"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    group.checkListGroupType = eachGroup.Rows[0]["checklistGroupType"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["checklistGroupType"].ToString();
                                    var GroupInPayload = dataTableParameters.lstexportChecklist.Where(x => x.jobChecklistHeaderId == checklistHeaderId).Select(z => z.lstExportChecklistGroup).FirstOrDefault();
                                    var groupfordisplayorder = GroupInPayload.Where(x => x.jobChecklistGroupId == IdJobChecklistGroup).FirstOrDefault();
                                    group.DisplayOrder1 = groupfordisplayorder.displayOrder1;                                  

                                    if (eachGroup.Rows[0]["checklistGroupType"].ToString().ToLower().TrimEnd() != "pl")
                                    {
                                        var groupsitems = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
                                        for (int j = 0; j < groupsitems.Count; j++)
                                        {

                                            CompositeReportItem item = new CompositeReportItem();
                                            item.Details = new List<CompositeReportDetails>();

                                            #region Items
                                            Int32 IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                            if (IdChecklistItem != 0)
                                            {
                                                item.checklistItemName = groupsitems[j]["checklistItemName"] == DBNull.Value ? string.Empty : groupsitems[j]["checklistItemName"].ToString();
                                                item.IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                                item.jocbChecklistItemDetailsId = groupsitems[j]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["JocbChecklistItemDetailsId"]);
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
                                                item.CompositeName = groupsitems[j]["CompositeName"] == DBNull.Value ? string.Empty : groupsitems[j]["CompositeName"].ToString();
                                                item.IdCompositeChecklist = groupsitems[j]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCompositeChecklist"]);
                                                item.IsRequiredForTCO = groupsitems[j]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsRequiredForTCO"]);
                                                item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                                item.HasDocument = item.HasDocument = db.JobDocuments.Any(x => x.IdJobchecklistItemDetails == item.jocbChecklistItemDetailsId);
                                                item.IsParentCheckList = groupsitems[j]["IsParentCheckList"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsParentCheckList"]);
                                                item.ClientNote= groupsitems[j]["ClientNotes"] == DBNull.Value ? string.Empty : groupsitems[j]["ClientNotes"].ToString();
                                                if (item.idContact != null && item.idContact != 0)
                                                {
                                                    var c = db.Contacts.Find(item.idContact);
                                                    item.ContactName = c.FirstName +" "+ c.LastName;
                                                    if (c.IdCompany != null && c.IdCompany != 0)
                                                    {
                                                        item.CompanyName = db.Companies.Where(x => x.Id == c.IdCompany).Select(y => y.Name).FirstOrDefault();
                                                    }
                                                }
                                                if (item.idDesignApplicant != null && item.idDesignApplicant != 0)
                                                {
                                                    var c = db.JobContacts.Find(item.idDesignApplicant);
                                                    if (c != null)
                                                    {
                                                        item.DesignApplicantName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                                    }

                                                }
                                                if (item.idInspector != null && item.idInspector != 0)
                                                {
                                                    var c = db.JobContacts.Find(item.idInspector);
                                                    if (c != null)
                                                    {
                                                        item.InspectorName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                                    }

                                                }
                                                #endregion
                                                if (dataTableParameters.OnlyTCOItems == true)
                                                {
                                                    if (item.IsRequiredForTCO == true)
                                                    {
                                                        group.item.Add(item);
                                                    }
                                                }
                                                else
                                                {
                                                    group.item.Add(item);
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {

                                        var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();

                                        var PlumbingCheckListFloors = groupsitems2.AsEnumerable().Select(a => a.Field<Int32?>("IdJobPlumbingCheckListFloors")).Distinct().ToList();

                                        for (int j = 0; j < PlumbingCheckListFloors.Count(); j++)
                                        {

                                            CompositeReportItem item = new CompositeReportItem();
                                            item.Details = new List<CompositeReportDetails>();
                                            var IdJobPlumbingCheckListFloors1 = PlumbingCheckListFloors[j];
                                            var detailItem = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobPlumbingCheckListFloors") == IdJobPlumbingCheckListFloors1).ToList();
                                            #region Items
                                            item.FloorNumber = detailItem[0]["FloorNumber"] == DBNull.Value ? string.Empty : detailItem[0]["FloorNumber"].ToString();
                                            item.idJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors1 != 0 ? IdJobPlumbingCheckListFloors1 : 0;
                                            item.FloorDisplayOrder = detailItem[0]["FloorDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[0]["FloorDisplayOrder"]);
                                            #endregion

                                            for (int i = 0; i < detailItem.Count; i++)
                                            {
                                                CompositeReportDetails detail = new CompositeReportDetails();
                                                #region Items details
                                                detail.checklistGroupType = detailItem[i]["checklistGroupType"] == DBNull.Value ? string.Empty : detailItem[i]["checklistGroupType"].ToString();
                                                detail.idJobPlumbingInspection = detailItem[i]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingInspection"]);
                                                detail.idJobPlumbingCheckListFloors = detailItem[i]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingCheckListFloors"]);
                                                detail.inspectionPermit = detailItem[i]["checklistItemName"] == DBNull.Value ? string.Empty : detailItem[i]["checklistItemName"].ToString();
                                                detail.workOrderNumber = detailItem[i]["WorkOrderNo"] == DBNull.Value ? string.Empty : detailItem[i]["WorkOrderNo"].ToString();
                                                detail.plComments = detailItem[i]["Comments"] == DBNull.Value ? string.Empty : detailItem[i]["Comments"].ToString();
                                                detail.DueDate = detailItem[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["DueDate"]);
                                                detail.nextInspection = detailItem[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["NextInspection"]);
                                                detail.result = detailItem[i]["Result"] == DBNull.Value ? string.Empty : detailItem[i]["Result"].ToString();
                                                detail.IsRequiredForTCO = detailItem[i]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(detailItem[i]["IsRequiredForTCO"]);
                                                detail.HasDocument = db.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                                detail.plCientNote= detailItem[i]["ClientNotes"] == DBNull.Value ? string.Empty : detailItem[i]["ClientNotes"].ToString();
                                                if (dataTableParameters.OnlyTCOItems == true)
                                                {
                                                    if (detail.IsRequiredForTCO == true)
                                                    {
                                                        item.Details.Add(detail);                                                        
                                                    }
                                                }
                                                else
                                                {
                                                    item.Details.Add(detail);                                                  
                                                }                                               
                                                #endregion

                                            }
                                            group.item.Add(item);
                                        }
                                    }

                                    Unorderedgroups.Add(group);                                    
                                }
                            }
                            header.groups = Unorderedgroups.OrderBy(x => x.DisplayOrder1).ToList();
                            headerlist.Add(header);
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    return this.NotFound();
                }                
                #region Excel
                var result = headerlist.OrderBy(x => x.CompositeOrder).ToList();
                if (result != null && result.Count > 0)
                {
                    string exportFilename = string.Empty;
                    exportFilename = "Composite_ChecklistReport_" + Convert.ToString(Guid.NewGuid()) + ".xlsx";
                    string exportFilePath = ExportToExcelForCustomer(result, exportFilename, dataTableParameters.IncludeViolations, dataTableParameters.OnlyTCOItems);

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

        private string ExportToExcelForCustomer(List<CompositeChecklistReportDTO> result, string exportFilename, bool isViolation, bool OnlyTCOItems)
        {
            string templatePath = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ExportToExcelTemplatePath);
            string path = HttpContext.Current.Server.MapPath("~\\" + Properties.Settings.Default.ReportExcelExportPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string templateFileName = string.Empty;
            if (result[0].IsCOProject == true)
                templateFileName = "Composite_TCO_Template-customer.xlsx";
            else
                templateFileName = "Composite_Template-customer.xlsx";

            string templateFilePath = templatePath + templateFileName;
            string exportFilePath = path + exportFilename;
            File.Delete(exportFilePath);

            XSSFWorkbook templateWorkbook = new XSSFWorkbook(templateFilePath);
            int ProjectNumber = result.Select(x => x.IdJob).FirstOrDefault().Value;
            int sheetIndex = 0;
            if (sheetIndex == 0)
            {
                templateWorkbook.SetSheetName(sheetIndex, "Composite-Checklist");

                sheetIndex = sheetIndex + 1;
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
            leftAlignCellStyle.VerticalAlignment = VerticalAlignment.Top;
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
            right_JobDetailAlignCellStyle.SetFont(myFont_Bold);
            right_JobDetailAlignCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
            right_JobDetailAlignCellStyle.WrapText = true;
            right_JobDetailAlignCellStyle.VerticalAlignment = VerticalAlignment.Center;
            right_JobDetailAlignCellStyle.Alignment = HorizontalAlignment.Right;

            ISheet sheet = templateWorkbook.GetSheet("Composite-Checklist");

            Job job = db.Jobs.Include("RfpAddress.Borough").Include("Company").Include("ProjectManager").FirstOrDefault(x => x.Id == ProjectNumber);

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
            CellRangeAddress DOBCellRangeAddress;
            DOBCellRangeAddress = new CellRangeAddress(1, 1, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iprojectAddressRow.GetCell(1) != null)
            {
                iprojectAddressRow.GetCell(1).SetCellValue(projectAddress);
            }
            else
            {
                iprojectAddressRow.CreateCell(1).SetCellValue(projectAddress);
            }

            iprojectAddressRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iprojectAddressRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iprojectAddressRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;


            #endregion

            #region ProjectNumber           
            IRow iProjectNumberRow = sheet.GetRow(2);

            if (iProjectNumberRow == null)
            {
                iProjectNumberRow = sheet.CreateRow(2);
            }
            DOBCellRangeAddress = new CellRangeAddress(2, 2, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iProjectNumberRow.GetCell(1) != null)
            {
                iProjectNumberRow.GetCell(1).SetCellValue(ProjectNumber);
            }
            else
            {
                iProjectNumberRow.CreateCell(1).SetCellValue(ProjectNumber);
            }

            iProjectNumberRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iProjectNumberRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iProjectNumberRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;
            #endregion

            #region Client
            IRow iClientRow = sheet.GetRow(3);

            string clientName = job != null && job.Company != null ? job.Company.Name : string.Empty;
            if (iClientRow == null)
            {
                iClientRow = sheet.CreateRow(3);
            }
            DOBCellRangeAddress = new CellRangeAddress(3, 3, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iClientRow.GetCell(1) != null)
            {
                iClientRow.GetCell(1).SetCellValue(clientName);
            }
            else
            {
                iClientRow.CreateCell(1).SetCellValue(clientName);
            }

            iClientRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iClientRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iClientRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;

            #endregion

            #region Project Manager

            string ProjectManagerName = job.ProjectManager != null ? job.ProjectManager.FirstName + " " + job.ProjectManager.LastName + " | " + job.ProjectManager.Email : string.Empty;
            IRow iProjectManagerRow = sheet.GetRow(4);
            if (iProjectManagerRow == null)
            {
                iProjectManagerRow = sheet.CreateRow(4);
            }
            DOBCellRangeAddress = new CellRangeAddress(4, 4, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iProjectManagerRow.GetCell(1) != null)
            {
                iProjectManagerRow.GetCell(1).SetCellValue(ProjectManagerName);
            }
            else
            {
                iProjectManagerRow.CreateCell(1).SetCellValue(ProjectManagerName);
            }

            iProjectManagerRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iProjectManagerRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iProjectManagerRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;

            #endregion

            #region Document

            string DocumentName = "Project Report";

            IRow iDocumentRow = sheet.GetRow(5);
            if (iDocumentRow == null)
            {
                iDocumentRow = sheet.CreateRow(5);
            }
            DOBCellRangeAddress = new CellRangeAddress(5, 5, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iDocumentRow.GetCell(1) != null)
            {
                iDocumentRow.GetCell(1).SetCellValue(DocumentName);
            }
            else
            {
                iDocumentRow.CreateCell(1).SetCellValue(DocumentName);
            }

            iDocumentRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iDocumentRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iDocumentRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;

            #endregion

            #region Report Date

            string reportDate = DateTime.Today.ToString(Common.ExportReportDateFormat);
            IRow iDateRow = sheet.GetRow(6);
            if (iDateRow == null)
            {
                iDateRow = sheet.CreateRow(6);
            }
            DOBCellRangeAddress = new CellRangeAddress(6, 6, 1, 3);
            sheet.AddMergedRegion(DOBCellRangeAddress);
            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
            if (iDateRow.GetCell(1) != null)
            {
                iDateRow.GetCell(1).SetCellValue(reportDate);
            }
            else
            {
                iDateRow.CreateCell(1).SetCellValue(reportDate);
            }

            iDateRow.GetCell(1).CellStyle = jobDetailAlignCellStyle;
            iDateRow.GetCell(2).CellStyle = jobDetailAlignCellStyle;
            iDateRow.GetCell(3).CellStyle = jobDetailAlignCellStyle;

            #endregion
            int index = 8;
            foreach (var c in result)
            {

                if (c != null)
                {
                    #region Checklist Report Header

                    XSSFFont ChecklistReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    //  ChecklistReportHeaderFont.FontHeightInPoints = (short)12;
                    ChecklistReportHeaderFont.FontHeightInPoints = (short)14;
                    ChecklistReportHeaderFont.FontName = "Times New Roman";
                    // ChecklistReportHeaderFont.FontName = "sans-serif";
                    ChecklistReportHeaderFont.IsBold = true;

                    XSSFFont ChecklistColumnHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    ChecklistColumnHeaderFont.FontHeightInPoints = (short)12;
                    ChecklistColumnHeaderFont.FontName = "Times New Roman";
                    // ChecklistReportHeaderFont.FontName = "sans-serif";
                    ChecklistColumnHeaderFont.IsBold = true;

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
                    { DOB_ReportHeader = c.CompositeChecklistName + " - " + c.Others; }
                    else
                    { DOB_ReportHeader = c.CompositeChecklistName; }
                    IRow iDOB_ReportHeaderRow = sheet.GetRow(index);
                    if (iDOB_ReportHeaderRow == null)
                    {
                        iDOB_ReportHeaderRow = sheet.CreateRow(index);
                    }
                    iDOB_ReportHeaderRow.HeightInPoints = (float)19.50;

                    //  if (c.groups.Select(x => x.checkListGroupType).ToList().Contains("PL") && c.IsParentCheckList==false)
                    //  {
                    //if (iDOB_ReportHeaderRow.GetCell(0) != null)
                    //{
                    //    iDOB_ReportHeaderRow.GetCell(0).SetCellValue("Plumbing Checklist:- " + DOB_ReportHeader);
                    //}
                    //else
                    //{

                    //    iDOB_ReportHeaderRow.CreateCell(0).SetCellValue("Plumbing Checklist:- " + DOB_ReportHeader);
                    //}
                    //  }
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

                    if (c.IsCOProject == true)
                    {
                        //DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 9);
                    }
                    else
                    {
                       // DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                    }
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    iDOB_ReportHeaderRow.GetCell(0).CellStyle = ChecklistReportHeaderCellStyle;

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    // index = index + 2;
                    index = index + 1;
                    #endregion


                    XSSFFont DOBHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    DOBHeaderFont.FontHeightInPoints = (short)14;
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
                        if (c.IsCOProject == true)
                        {
                          //  DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 9);
                        }
                        else
                        {
                          //  DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                        }
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
                        ColumnHeaderCellStyle.SetFont(ChecklistColumnHeaderFont);
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
                        if (g.checkListGroupType.ToLower().Trim() == "general")
                        {

                            #region header

                            IRow iDOB_HeaderRow = sheet.GetRow(index);

                            if (iDOB_HeaderRow == null)
                            {
                                iDOB_HeaderRow = sheet.CreateRow(index);
                            }
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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


                            if (iDOB_HeaderRow.GetCell(3) != null)
                            {
                                iDOB_HeaderRow.GetCell(3).SetCellValue("Party Responsible");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(3).SetCellValue("Party Responsible");
                            }
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 4, 5);
                            sheet.AddMergedRegion(DOBCellRangeAddress);
                            RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                            if (iDOB_HeaderRow.GetCell(4) != null)
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue("Comments");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("Comments");
                            }

                            if (iDOB_HeaderRow.GetCell(6) != null)
                            {
                                iDOB_HeaderRow.GetCell(6).SetCellValue("Target Date");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(6).SetCellValue("Target Date");
                            }

                            if (iDOB_HeaderRow.GetCell(7) != null)
                            {
                                iDOB_HeaderRow.GetCell(7).SetCellValue("Status");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("Status");
                            }

                            if (c.IsCOProject == true)
                            {
                                if (iDOB_HeaderRow.GetCell(8) != null)
                                {
                                    iDOB_HeaderRow.GetCell(8).SetCellValue("TCO?");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("TCO?");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
                                if (iDOB_HeaderRow.GetCell(9) != null)
                                {
                                    iDOB_HeaderRow.GetCell(9).SetCellValue("Client Note");
                                    iDOB_HeaderRow.GetCell(9).CellStyle = ColumnHeaderCellStyle;
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(9).SetCellValue("Client Note");
                                    iDOB_HeaderRow.GetCell(9).CellStyle = ColumnHeaderCellStyle;
                                }
                            }
                            else
                            {
                                if (iDOB_HeaderRow.GetCell(8) != null)
                                {
                                    iDOB_HeaderRow.GetCell(8).SetCellValue("Client Note");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("Client Note");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
                            }
                     
                            iDOB_HeaderRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(1).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(6).CellStyle = ColumnHeaderCellStyle;
                            iDOB_HeaderRow.GetCell(7).CellStyle = ColumnHeaderCellStyle;
                          //  iDOB_HeaderRow.GetCell(9).CellStyle = ColumnHeaderCellStyle;
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
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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
                                if (iRow.GetCell(3) != null)
                                {
                                    if (item.PartyResponsible.ToLower() == "rpo user")
                                        iRow.GetCell(3).SetCellValue("RPO");

                                    else if (item.PartyResponsible.ToLower() == "contact")
                                    {
                                        if (item.CompanyName != null)
                                            iRow.GetCell(3).SetCellValue(item.ContactName + " - " + item.CompanyName);
                                        else
                                            iRow.GetCell(3).SetCellValue(item.ContactName);
                                    }

                                    else if (item.PartyResponsible.ToLower() == "other")
                                        iRow.GetCell(3).SetCellValue(item.manualPartyResponsible);
                                    else
                                        iRow.GetCell(3).SetCellValue("");
                                }
                                else
                                {
                                    if (item.PartyResponsible.ToLower() == "rpo user")
                                        iRow.CreateCell(3).SetCellValue("RPO");

                                    else if (item.PartyResponsible.ToLower() == "contact")
                                    {
                                        if (item.CompanyName != null)
                                            iRow.CreateCell(3).SetCellValue(item.ContactName + " - " + item.CompanyName);
                                        else
                                            iRow.CreateCell(3).SetCellValue(item.ContactName);
                                    }

                                    else if (item.PartyResponsible.ToLower() == "other")
                                        iRow.CreateCell(3).SetCellValue(item.manualPartyResponsible);
                                    else
                                        iRow.CreateCell(3).SetCellValue("");
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 4, 5);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iRow.GetCell(4) != null)
                                {
                                    iRow.GetCell(4).SetCellValue(item.comments);
                                }
                                else
                                {
                                    iRow.CreateCell(4).SetCellValue(item.comments);
                                }

                                // string workDescription = item.JobWorkTypeDescription + (!string.IsNullOrEmpty(item.EquipmentType) ? " " + item.EquipmentType : string.Empty);

                                if (iRow.GetCell(6) != null)
                                {
                                    iRow.GetCell(6).SetCellValue(item.dueDate != null ? Convert.ToDateTime(item.dueDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                                }
                                else
                                {
                                    iRow.CreateCell(6).SetCellValue(item.dueDate != null ? Convert.ToDateTime(item.dueDate).ToString(Common.ExportReportDateFormat) : string.Empty);
                                }
                                //Temporary commented
                                if (iRow.GetCell(7) != null)
                                {
                                    if (item.status == 1)
                                        iRow.GetCell(7).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.GetCell(7).SetCellValue("InProcess");
                                    else if (item.status == 3)
                                        iRow.GetCell(7).SetCellValue("Completed");
                                    else
                                        iRow.GetCell(7).SetCellValue("");
                                }
                                else
                                {
                                    if (item.status == 1)
                                        iRow.CreateCell(7).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.CreateCell(7).SetCellValue("InProcess");
                                    else if (item.status == 3)
                                        iRow.CreateCell(7).SetCellValue("Completed");
                                    else
                                        iRow.CreateCell(7).SetCellValue("");
                                }
                                if (c.IsCOProject == true)
                                {
                                    if (iRow.GetCell(8) != null)
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            iRow.GetCell(8).SetCellValue("YES");
                                        else
                                            iRow.GetCell(8).SetCellValue("NO");

                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
                                    else
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            iRow.CreateCell(8).SetCellValue("YES");
                                        else
                                            iRow.CreateCell(8).SetCellValue("NO");

                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
                                    if (iRow.GetCell(9) != null)
                                    {
                                        iRow.GetCell(9).SetCellValue(item.ClientNote);
                                        iRow.GetCell(9).CellStyle = leftAlignCellStyle;
                                    }
                                    else
                                    {
                                        iRow.CreateCell(9).SetCellValue(item.ClientNote);
                                        iRow.GetCell(9).CellStyle = leftAlignCellStyle;
                                    }
                                }
                                else
                                {
                                    if (iRow.GetCell(8) != null)
                                    {
                                        iRow.GetCell(8).SetCellValue(item.ClientNote);
                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
                                    else
                                    {
                                        iRow.CreateCell(8).SetCellValue(item.ClientNote);
                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
                                }
                                iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                                iRow.GetCell(7).CellStyle = leftAlignCellStyle;
                               
                                //iRow.GetCell(8).CellStyle = leftAlignCellStyle;


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
                            DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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

                            if (iDOB_HeaderRow.GetCell(3) != null)
                            {
                                iDOB_HeaderRow.GetCell(3).SetCellValue("Design Applicant");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(3).SetCellValue("Design Applicant");
                            }

                            if (iDOB_HeaderRow.GetCell(4) != null)
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue("Inspector");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("Inspector");
                            }
                            if (iDOB_HeaderRow.GetCell(5) != null)
                            {
                                iDOB_HeaderRow.GetCell(5).SetCellValue("Comments");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(5).SetCellValue("Comments");
                            }
                            if (iDOB_HeaderRow.GetCell(6) != null)
                            {
                                iDOB_HeaderRow.GetCell(6).SetCellValue("Stage");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(6).SetCellValue("Stage");
                            }

                            if (iDOB_HeaderRow.GetCell(7) != null)
                            {
                                iDOB_HeaderRow.GetCell(7).SetCellValue("Status");
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("Status");
                            }

                            if (c.IsCOProject == true)
                            {
                                if (iDOB_HeaderRow.GetCell(8) != null)
                                {
                                    iDOB_HeaderRow.GetCell(8).SetCellValue("TCO?");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("TCO?");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
                                if (iDOB_HeaderRow.GetCell(9) != null)
                                {
                                    iDOB_HeaderRow.GetCell(9).SetCellValue("Client Note");
                                    iDOB_HeaderRow.GetCell(9).CellStyle = ColumnHeaderCellStyle;
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(9).SetCellValue("Client Note");
                                    iDOB_HeaderRow.GetCell(9).CellStyle = ColumnHeaderCellStyle;
                                }
                            }
                            else
                            {
                                if (iDOB_HeaderRow.GetCell(8) != null)
                                {
                                    iDOB_HeaderRow.GetCell(8).SetCellValue("Client Note");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("Client Note");
                                    iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                }
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
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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
                                if (iRow.GetCell(3) != null)
                                {
                                    iRow.GetCell(3).SetCellValue(item.DesignApplicantName);
                                }
                                else
                                {
                                    iRow.CreateCell(3).SetCellValue(item.DesignApplicantName);
                                }

                                if (iRow.GetCell(4) != null)
                                {
                                    iRow.GetCell(4).SetCellValue(item.InspectorName);
                                }
                                else
                                {
                                    iRow.CreateCell(4).SetCellValue(item.InspectorName);
                                }
                                if (iRow.GetCell(5) != null)
                                {
                                    iRow.GetCell(5).SetCellValue(item.comments);
                                }
                                else
                                {
                                    iRow.CreateCell(5).SetCellValue(item.comments);
                                }

                                if (iRow.GetCell(6) != null)
                                {
                                    if (item.stage == "1")
                                        iRow.GetCell(6).SetCellValue("Prior to Approval");
                                    else if (item.stage == "2")
                                        iRow.GetCell(6).SetCellValue("Prior to Permit");
                                    else if (item.stage == "3")
                                        iRow.GetCell(6).SetCellValue("Prior to Sign Off");
                                    else
                                        iRow.GetCell(6).SetCellValue("");
                                }
                                else
                                {
                                    if (item.stage == "1")
                                        iRow.CreateCell(6).SetCellValue("Prior to Approval");
                                    else if (item.stage == "2")
                                        iRow.CreateCell(6).SetCellValue("Prior to Permit");
                                    else if (item.stage == "3")
                                        iRow.CreateCell(6).SetCellValue("Prior to Sign Off");
                                    else
                                        iRow.CreateCell(6).SetCellValue("");
                                }
                                //Temporary commented
                                if (iRow.GetCell(7) != null)
                                {
                                    if (item.status == 1)
                                        iRow.GetCell(7).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.GetCell(7).SetCellValue("Completed");
                                    else
                                        iRow.GetCell(7).SetCellValue("");
                                }
                                else
                                {
                                    if (item.status == 1)
                                        iRow.CreateCell(7).SetCellValue("Open");
                                    else if (item.status == 2)
                                        iRow.CreateCell(7).SetCellValue("Completed");
                                    else
                                        iRow.CreateCell(7).SetCellValue("");
                                }

                                if (c.IsCOProject == true)
                                {
                                    if (iRow.GetCell(8) != null)
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            iRow.GetCell(8).SetCellValue("YES");
                                        else
                                            iRow.GetCell(8).SetCellValue("NO");

                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
                                    else
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            iRow.CreateCell(8).SetCellValue("YES");
                                        else
                                            iRow.CreateCell(8).SetCellValue("NO");

                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
                                    if (iRow.GetCell(9) != null)
                                    {
                                        iRow.GetCell(9).SetCellValue(item.ClientNote);
                                        iRow.GetCell(9).CellStyle = leftAlignCellStyle;
                                    }
                                    else
                                    {
                                        iRow.CreateCell(9).SetCellValue(item.ClientNote);
                                        iRow.GetCell(9).CellStyle = leftAlignCellStyle;
                                    }
                                }
                                else
                                {
                                    if (iRow.GetCell(8) != null)
                                    {
                                        iRow.GetCell(8).SetCellValue(item.ClientNote);
                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
                                    else
                                    {
                                        iRow.CreateCell(8).SetCellValue(item.ClientNote);
                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
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
                                if (c.IsCOProject == true)
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                                else
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                                sheet.AddMergedRegion(DOBCellRangeAddress);

                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iDOB_FloorRow.GetCell(0) != null)
                                {
                                    iDOB_FloorRow.GetCell(0).SetCellValue("Floor - " + item.FloorNumber);
                                }
                                else
                                {
                                    iDOB_FloorRow.CreateCell(0).SetCellValue("Floor - " + item.FloorNumber);
                                }
                                iDOB_FloorRow.GetCell(0).CellStyle = GroupHeaderCellStyle;
                                #endregion                             
                                index = index + 1;
                                IRow iDOB_HeaderRow = sheet.GetRow(index);

                                if (iDOB_HeaderRow == null)
                                {
                                    iDOB_HeaderRow = sheet.CreateRow(index);
                                }
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 3, 5);
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
                                DOBCellRangeAddress = new CellRangeAddress(index, index, 6, 7);
                                sheet.AddMergedRegion(DOBCellRangeAddress);
                                RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                if (iDOB_HeaderRow.GetCell(6) != null)
                                {
                                    iDOB_HeaderRow.GetCell(6).SetCellValue("Status");
                                }
                                else
                                {
                                    iDOB_HeaderRow.CreateCell(6).SetCellValue("Status");
                                }
                                if (c.IsCOProject == true)
                                {
                                    if (iDOB_HeaderRow.GetCell(8) != null)
                                    {
                                        iDOB_HeaderRow.GetCell(8).SetCellValue("TCO?");
                                        iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                    }
                                    else
                                    {
                                        iDOB_HeaderRow.CreateCell(8).SetCellValue("TCO?");
                                        iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                    }
                                    if (iDOB_HeaderRow.GetCell(9) != null)
                                    {
                                        iDOB_HeaderRow.GetCell(9).SetCellValue("Client Note");
                                        iDOB_HeaderRow.GetCell(9).CellStyle = ColumnHeaderCellStyle;
                                    }
                                    else
                                    {
                                        iDOB_HeaderRow.CreateCell(9).SetCellValue("Client Note");
                                        iDOB_HeaderRow.GetCell(9).CellStyle = ColumnHeaderCellStyle;
                                    }
                                }
                                else
                                {
                                    if (iDOB_HeaderRow.GetCell(8) != null)
                                    {
                                        iDOB_HeaderRow.GetCell(8).SetCellValue("Client Note");
                                        iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                    }
                                    else
                                    {
                                        iDOB_HeaderRow.CreateCell(8).SetCellValue("Client Note");
                                        iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                    }
                                }
                                iDOB_HeaderRow.GetCell(0).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(2).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(3).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(4).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(5).CellStyle = ColumnHeaderCellStyle;
                                iDOB_HeaderRow.GetCell(6).CellStyle = ColumnHeaderCellStyle;
                               // iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                                
                                index = index + 1;
                                foreach (var d in item.Details)
                                {
                                    #region Data                                   
                                    IRow iRow = sheet.GetRow(index);
                                    if (iRow == null)
                                    {
                                        iRow = sheet.CreateRow(index);
                                    }

                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 2);
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
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 3, 5);
                                    sheet.AddMergedRegion(DOBCellRangeAddress);
                                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    if (iRow.GetCell(3) != null)
                                    {
                                        iRow.GetCell(3).SetCellValue(d.plComments);

                                    }
                                    else
                                    {
                                        iRow.CreateCell(3).SetCellValue(d.plComments);
                                    }
                                    DOBCellRangeAddress = new CellRangeAddress(index, index, 6, 7);
                                    //  }
                                    sheet.AddMergedRegion(DOBCellRangeAddress);
                                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                                    if (iRow.GetCell(6) != null)
                                    {
                                        if (d.result == "1")
                                            iRow.GetCell(6).SetCellValue("Pass");
                                        else if (d.result == "2")
                                            iRow.GetCell(6).SetCellValue("Failed");
                                        else if (d.result == "3")
                                            iRow.GetCell(6).SetCellValue("Pending");
                                        else if (d.result == "4")
                                            iRow.GetCell(6).SetCellValue("NA");
                                        else
                                            iRow.GetCell(6).SetCellValue("");
                                    }
                                    else
                                    {
                                        if (d.result == "1")
                                            iRow.CreateCell(6).SetCellValue("Pass");
                                        else if (d.result == "2")
                                            iRow.CreateCell(6).SetCellValue("Failed");
                                        else if (d.result == "3")
                                            iRow.CreateCell(6).SetCellValue("Pending");
                                        else if (d.result == "4")
                                            iRow.CreateCell(6).SetCellValue("NA");
                                        else
                                            iRow.CreateCell(6).SetCellValue("");
                                    }

                                    if (c.IsCOProject == true)
                                    {
                                        if (iRow.GetCell(8) != null)
                                        {
                                            if (d.IsRequiredForTCO == true)
                                                iRow.GetCell(8).SetCellValue("YES");
                                            else
                                                iRow.GetCell(8).SetCellValue("NO");

                                            iRow.GetCell(8).CellStyle = leftAlignCellStyle;

                                        }
                                        else
                                        {
                                            if (d.IsRequiredForTCO == true)
                                                iRow.CreateCell(8).SetCellValue("YES");
                                            else
                                                iRow.CreateCell(8).SetCellValue("NO");

                                            iRow.GetCell(8).CellStyle = leftAlignCellStyle;

                                        }
                                        if (iRow.GetCell(9) != null)
                                        {
                                            iRow.GetCell(9).SetCellValue(d.plCientNote);
                                            iRow.GetCell(9).CellStyle = leftAlignCellStyle;

                                        }
                                        else
                                        {
                                            iRow.CreateCell(9).SetCellValue(d.plCientNote);
                                            iRow.GetCell(9).CellStyle = leftAlignCellStyle;
                                        }
                                    }
                                    if (iRow.GetCell(8) != null)
                                    {
                                        iRow.GetCell(8).SetCellValue(d.plCientNote);
                                        iRow.GetCell(7).CellStyle = leftAlignCellStyle;
                                    }
                                    else
                                    {
                                        iRow.CreateCell(8).SetCellValue(d.plCientNote);
                                        iRow.GetCell(8).CellStyle = leftAlignCellStyle;
                                    }
                                    iRow.GetCell(0).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(2).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(1).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(3).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(4).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(5).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(6).CellStyle = leftAlignCellStyle;
                                    iRow.GetCell(7).CellStyle = leftAlignCellStyle;
                                    
                                    index = index + 1;
                                }

                            }
                        }
                    }

                }
                index = index + 1;

                #endregion
            }

            if (isViolation)
            {
                int Idrfpaddress = db.Jobs.Find(ProjectNumber).IdRfpAddress;
                string BinNumber = db.RfpAddresses.Where(x => x.Id == Idrfpaddress).Select(y => y.BinNumber).FirstOrDefault();
                int? IdCompositeChecklist = result[0].IdCompositeChecklist;
                #region ECB Violations    
                List<JobViolation> lstjobECBviolation = new List<JobViolation>();
                var CompositeECBViolations = db.CompositeViolations.Where(x => x.IdCompositeChecklist == IdCompositeChecklist).ToList();
                foreach (var c in CompositeECBViolations)
                {
                    JobViolation violation = new JobViolation();
                    if (OnlyTCOItems == true)
                    {
                        violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "ECB" & x.TCOToggle == true).FirstOrDefault();
                    }
                    else
                    {
                        violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "ECB").FirstOrDefault();
                    }
                    if (violation != null)
                        lstjobECBviolation.Add(violation);
                }
                if (lstjobECBviolation != null && lstjobECBviolation.Count > 0)
                {
                    XSSFFont ChecklistViolationReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    ChecklistViolationReportHeaderFont.FontHeightInPoints = (short)14;
                    ChecklistViolationReportHeaderFont.FontName = "Times New Roman";
                    ChecklistViolationReportHeaderFont.IsBold = true;
                    #region group header
                    XSSFCellStyle GroupHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    GroupHeaderCellStyle.SetFont(ChecklistViolationReportHeaderFont);
                    GroupHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.WrapText = true;
                    GroupHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    GroupHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                    GroupHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.White);
                    GroupHeaderCellStyle.FillPattern = FillPattern.SolidForeground;
                    IRow Group_ReportHeaderRow = sheet.GetRow(index);
                    if (Group_ReportHeaderRow == null)
                    {
                        Group_ReportHeaderRow = sheet.CreateRow(index);
                    }
                    Group_ReportHeaderRow.HeightInPoints = (float)19.50;

                    if (Group_ReportHeaderRow.GetCell(0) != null)
                    {
                        Group_ReportHeaderRow.GetCell(0).SetCellValue("ECB/OATH Violations");
                    }
                    else
                    {
                        Group_ReportHeaderRow.CreateCell(0).SetCellValue("ECB/OATH Violations");
                    }
                    if (result[0].IsCOProject == true)
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                    else
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    Group_ReportHeaderRow.GetCell(0).CellStyle = GroupHeaderCellStyle;

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    index = index + 1;
                    #endregion

                    #region column header

                    IRow iDOB_HeaderRow = sheet.GetRow(index);

                    if (iDOB_HeaderRow == null)
                    {
                        iDOB_HeaderRow = sheet.CreateRow(index);
                    }

                    if (iDOB_HeaderRow.GetCell(0) != null)
                    {
                        iDOB_HeaderRow.GetCell(0).SetCellValue("Issue Date");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(0).SetCellValue("Issue Date");
                    }

                    if (iDOB_HeaderRow.GetCell(1) != null)
                    {
                        iDOB_HeaderRow.GetCell(1).SetCellValue("ECB Violation No.");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(1).SetCellValue("ECB Violation No.");
                    }
                    DOBCellRangeAddress = new CellRangeAddress(index, index, 1, 2);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    if (iDOB_HeaderRow.GetCell(3) != null)
                    {
                        iDOB_HeaderRow.GetCell(3).SetCellValue("Violation Description");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(3).SetCellValue("Violation Description");
                    }

                    if (iDOB_HeaderRow.GetCell(4) != null)
                    {
                        iDOB_HeaderRow.GetCell(4).SetCellValue("Party Responsible");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(4).SetCellValue("Party Responsible");
                    }

                    if (iDOB_HeaderRow.GetCell(5) != null)
                    {
                        iDOB_HeaderRow.GetCell(5).SetCellValue("Comments");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(5).SetCellValue("Comments");
                    }
                    DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    if (iDOB_HeaderRow.GetCell(7) != null)
                    {
                        iDOB_HeaderRow.GetCell(7).SetCellValue("Status");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(7).SetCellValue("Status");
                    }
                    #region Column Header cell style
                    XSSFFont ChecklistColumnHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    ChecklistColumnHeaderFont.FontHeightInPoints = (short)12;
                    ChecklistColumnHeaderFont.FontName = "Times New Roman";
                    ChecklistColumnHeaderFont.IsBold = true;

                    XSSFCellStyle ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    ColumnHeaderCellStyle.SetFont(ChecklistColumnHeaderFont);
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
                    if (result[0].IsCOProject == true)
                    {
                        if (iDOB_HeaderRow.GetCell(8) != null)
                        {
                            iDOB_HeaderRow.GetCell(8).SetCellValue("TCO?");
                            iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(8).SetCellValue("TCO?");
                            iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                        }
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


                    foreach (var jobViolation in lstjobECBviolation)
                    {

                        #region Data

                        iDOB_HeaderRow = sheet.GetRow(index);

                        if (iDOB_HeaderRow == null)
                        {
                            iDOB_HeaderRow = sheet.CreateRow(index);
                        }

                        if (iDOB_HeaderRow.GetCell(0) != null)
                        {

                            iDOB_HeaderRow.GetCell(0).SetCellValue(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(0).SetCellValue(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 1, 2);
                        sheet.AddMergedRegion(DOBCellRangeAddress);
                        RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                        if (iDOB_HeaderRow.GetCell(1) != null)
                        {
                            iDOB_HeaderRow.GetCell(1).SetCellValue(jobViolation.SummonsNumber != null ? jobViolation.SummonsNumber.ToString() : "");
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(1).SetCellValue(jobViolation.SummonsNumber != null ? jobViolation.SummonsNumber.ToString() : "");
                        }

                        if (iDOB_HeaderRow.GetCell(3) != null)
                        {
                            iDOB_HeaderRow.GetCell(3).SetCellValue(jobViolation.ViolationDescription); //violation description
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(3).SetCellValue(jobViolation.ViolationDescription);
                        }

                        if (iDOB_HeaderRow.GetCell(4) != null)
                        {
                            if (jobViolation.PartyResponsible == 1)
                                iDOB_HeaderRow.GetCell(4).SetCellValue("RPO Team");
                            else if (jobViolation.PartyResponsible == 3)
                                iDOB_HeaderRow.GetCell(4).SetCellValue(jobViolation.ManualPartyResponsible);
                            else
                                iDOB_HeaderRow.GetCell(4).SetCellValue("");
                        }
                        else
                        {
                            if (jobViolation.PartyResponsible == 1)
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("RPO Team");
                            else if (jobViolation.PartyResponsible == 3)
                                iDOB_HeaderRow.CreateCell(4).SetCellValue(jobViolation.ManualPartyResponsible);
                            else
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("");
                        }
                        var comment = (from cmt in db.ChecklistJobViolationComments
                                       where cmt.IdJobViolation == jobViolation.Id
                                       orderby cmt.LastModifiedDate descending
                                       select cmt.Description).FirstOrDefault();
                        if (iDOB_HeaderRow.GetCell(5) != null)
                        {

                            iDOB_HeaderRow.GetCell(5).SetCellValue(comment);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(5).SetCellValue(comment);
                        }
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                        sheet.AddMergedRegion(DOBCellRangeAddress);
                        RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        if (iDOB_HeaderRow.GetCell(7) != null)
                        {
                            if (jobViolation.Status == 1)
                                iDOB_HeaderRow.GetCell(7).SetCellValue("Open");
                            else if (jobViolation.Status == 3)
                                iDOB_HeaderRow.GetCell(7).SetCellValue("Completed");
                            else
                                iDOB_HeaderRow.GetCell(7).SetCellValue("");
                        }
                        else
                        {
                            if (jobViolation.Status == 1)
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("Open");
                            else if (jobViolation.Status == 3)
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("Completed");
                            else
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("");
                        }
                        #region Column Header cell style
                        ChecklistColumnHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                        ChecklistColumnHeaderFont.FontHeightInPoints = (short)12;
                        ChecklistColumnHeaderFont.FontName = "Times New Roman";
                        ChecklistColumnHeaderFont.IsBold = true;

                        ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                        ColumnHeaderCellStyle.SetFont(ChecklistColumnHeaderFont);
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
                        if (result[0].IsCOProject == true)
                        {

                            if (iDOB_HeaderRow.GetCell(8) != null)
                            {
                                if (OnlyTCOItems == true)
                                {
                                    if (jobViolation.TCOToggle == true)
                                        iDOB_HeaderRow.GetCell(8).SetCellValue("YES");
                                }
                                else
                                {
                                    if (jobViolation.TCOToggle == true)
                                        iDOB_HeaderRow.GetCell(8).SetCellValue("YES");
                                    else
                                        iDOB_HeaderRow.GetCell(8).SetCellValue("NO");
                                }
                                iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;
                            }
                            else
                            {
                                if (jobViolation.TCOToggle == true)
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("YES");
                                else
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("NO");
                                iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;
                            }
                        }
                        iDOB_HeaderRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(2).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(6).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(7).CellStyle = leftAlignCellStyle;
                        index = index + 1;
                        #endregion                       

                    }
                }
                #endregion

                #region DOB Violations 
                List<JobViolation> lstjobDOBviolation = new List<JobViolation>();
                var CompositeDOBViolations = db.CompositeViolations.Where(x => x.IdCompositeChecklist == IdCompositeChecklist).ToList();
                foreach (var c in CompositeDOBViolations)
                {
                    JobViolation violation = new JobViolation();
                    if (OnlyTCOItems == true)
                    {
                        violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "DOB" & x.TCOToggle == true).FirstOrDefault();
                    }
                    else
                    {
                        violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "DOB").FirstOrDefault();
                    }
                    if (violation != null)
                        lstjobDOBviolation.Add(violation);
                }
                if (lstjobDOBviolation != null && lstjobDOBviolation.Count > 0)
                {

                    XSSFFont ChecklistViolationReportHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    ChecklistViolationReportHeaderFont.FontHeightInPoints = (short)14;
                    ChecklistViolationReportHeaderFont.FontName = "Times New Roman";
                    ChecklistViolationReportHeaderFont.IsBold = true;
                    XSSFCellStyle GroupHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    GroupHeaderCellStyle.SetFont(ChecklistViolationReportHeaderFont);
                    GroupHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                    GroupHeaderCellStyle.WrapText = true;
                    GroupHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    GroupHeaderCellStyle.Alignment = HorizontalAlignment.Center;
                    GroupHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.White);
                    GroupHeaderCellStyle.FillPattern = FillPattern.SolidForeground;

                    #region group header
                    IRow Group_ReportHeaderRow = sheet.GetRow(index);
                    if (Group_ReportHeaderRow == null)
                    {
                        Group_ReportHeaderRow = sheet.CreateRow(index);
                    }
                    Group_ReportHeaderRow.HeightInPoints = (float)19.50;

                    if (Group_ReportHeaderRow.GetCell(0) != null)
                    {
                        Group_ReportHeaderRow.GetCell(0).SetCellValue("DOB Violations");
                    }
                    else
                    {
                        Group_ReportHeaderRow.CreateCell(0).SetCellValue("DOB Violations");
                    }
                    if (result[0].IsCOProject == true)
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 8);
                    else
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 7);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    Group_ReportHeaderRow.GetCell(0).CellStyle = GroupHeaderCellStyle;

                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    index = index + 1;
                    #endregion

                    #region column header DOB

                    IRow iDOB_HeaderRow = sheet.GetRow(index);
                    if (iDOB_HeaderRow == null)
                    {
                        iDOB_HeaderRow = sheet.CreateRow(index);
                    }
                    if (iDOB_HeaderRow.GetCell(0) != null)
                    {

                        iDOB_HeaderRow.GetCell(0).SetCellValue("Issue Date");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(0).SetCellValue("Issue Date");
                    }

                    if (iDOB_HeaderRow.GetCell(1) != null)
                    {
                        iDOB_HeaderRow.GetCell(1).SetCellValue("DOB Violation No.");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(1).SetCellValue("DOB Violation No.");
                    }

                    if (iDOB_HeaderRow.GetCell(2) != null)
                    {
                        iDOB_HeaderRow.GetCell(2).SetCellValue("Related ECB No.");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(2).SetCellValue("Related ECB No.");
                    }

                    if (iDOB_HeaderRow.GetCell(3) != null)
                    {
                        iDOB_HeaderRow.GetCell(3).SetCellValue("Violation Description");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(3).SetCellValue("Violation Description");
                    }

                    if (iDOB_HeaderRow.GetCell(4) != null)
                    {
                        iDOB_HeaderRow.GetCell(4).SetCellValue("Party Responsible");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(4).SetCellValue("Party Responsible");
                    }
                    DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                    sheet.AddMergedRegion(DOBCellRangeAddress);
                    RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                    RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                    if (iDOB_HeaderRow.GetCell(5) != null)
                    {
                        iDOB_HeaderRow.GetCell(5).SetCellValue("Comments");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(5).SetCellValue("Comments");
                    }
                    if (iDOB_HeaderRow.GetCell(7) != null)
                    {
                        iDOB_HeaderRow.GetCell(7).SetCellValue("Status");
                    }
                    else
                    {
                        iDOB_HeaderRow.CreateCell(7).SetCellValue("Status");
                    }

                    #region Column Header cell style
                    XSSFFont ChecklistColumnHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                    ChecklistColumnHeaderFont.FontHeightInPoints = (short)12;
                    ChecklistColumnHeaderFont.FontName = "Times New Roman";
                    ChecklistColumnHeaderFont.IsBold = true;

                    XSSFCellStyle ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                    ColumnHeaderCellStyle.SetFont(ChecklistColumnHeaderFont);
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
                    if (result[0].IsCOProject == true)
                    {
                        if (iDOB_HeaderRow.GetCell(8) != null)
                        {
                            iDOB_HeaderRow.GetCell(8).SetCellValue("TCO?");
                            iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(8).SetCellValue("TCO?");
                            iDOB_HeaderRow.GetCell(8).CellStyle = ColumnHeaderCellStyle;
                        }
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

                    #region Data DOB
                    foreach (var jobViolation in lstjobDOBviolation)
                    {

                        #region Data

                        iDOB_HeaderRow = sheet.GetRow(index);

                        if (iDOB_HeaderRow == null)
                        {
                            iDOB_HeaderRow = sheet.CreateRow(index);
                        }
                        //DOBCellRangeAddress = new CellRangeAddress(index, index, 0, 1);
                        //sheet.AddMergedRegion(DOBCellRangeAddress);
                        //RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        if (iDOB_HeaderRow.GetCell(0) != null)
                        {

                            iDOB_HeaderRow.GetCell(0).SetCellValue(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(0).SetCellValue(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString("MM/dd/yyyy") : string.Empty);
                        }


                        if (iDOB_HeaderRow.GetCell(1) != null)
                        {
                            iDOB_HeaderRow.GetCell(1).SetCellValue(jobViolation.SummonsNumber);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(1).SetCellValue(jobViolation.SummonsNumber);
                        }

                        if (iDOB_HeaderRow.GetCell(2) != null)
                        {
                            iDOB_HeaderRow.GetCell(2).SetCellValue(jobViolation.ECBnumber != null ? jobViolation.ECBnumber.ToString() : "");
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(2).SetCellValue(jobViolation.ECBnumber != null ? jobViolation.ECBnumber.ToString() : "");
                        }
                        //DOBCellRangeAddress = new CellRangeAddress(index, index, 3, 4);
                        //sheet.AddMergedRegion(DOBCellRangeAddress);
                        //RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        //RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);

                        if (iDOB_HeaderRow.GetCell(3) != null)
                        {
                            iDOB_HeaderRow.GetCell(3).SetCellValue(jobViolation.ViolationDescription);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(3).SetCellValue(jobViolation.ViolationDescription);
                        }
                        if (iDOB_HeaderRow.GetCell(4) != null)
                        {
                            if (jobViolation.PartyResponsible == 1)
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue("RPO Team");
                            }
                            else if (jobViolation.PartyResponsible == 3)
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue(jobViolation.ManualPartyResponsible);
                            }
                            else
                            {
                                iDOB_HeaderRow.GetCell(4).SetCellValue("");
                            }
                        }
                        else
                        {
                            if (jobViolation.PartyResponsible == 1)
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("RPO Team");
                            }
                            else if (jobViolation.PartyResponsible == 3)
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue(jobViolation.ManualPartyResponsible);
                            }
                            else
                            {
                                iDOB_HeaderRow.CreateCell(4).SetCellValue("");
                            }
                        }
                        var comment = (from cmt in db.ChecklistJobViolationComments
                                       where cmt.IdJobViolation == jobViolation.Id
                                       orderby cmt.LastModifiedDate descending
                                       select cmt.Description).FirstOrDefault();
                        if (iDOB_HeaderRow.GetCell(5) != null)
                        {

                            iDOB_HeaderRow.GetCell(5).SetCellValue(comment);
                        }
                        else
                        {
                            iDOB_HeaderRow.CreateCell(5).SetCellValue(comment);
                        }
                        DOBCellRangeAddress = new CellRangeAddress(index, index, 5, 6);
                        sheet.AddMergedRegion(DOBCellRangeAddress);
                        RegionUtil.SetBorderTop(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderBottom(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderLeft(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);
                        RegionUtil.SetBorderRight(BorderStyle.Medium.GetHashCode(), DOBCellRangeAddress, sheet, templateWorkbook);



                        if (iDOB_HeaderRow.GetCell(7) != null)
                        {
                            if (jobViolation.Status == 1)
                                iDOB_HeaderRow.GetCell(7).SetCellValue("Open");
                            else if (jobViolation.Status == 3)
                                iDOB_HeaderRow.GetCell(7).SetCellValue("Completed");
                            else
                                iDOB_HeaderRow.GetCell(7).SetCellValue("");
                        }
                        else
                        {
                            if (jobViolation.Status == 1)
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("Open");
                            else if (jobViolation.Status == 3)
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("Completed");
                            else
                                iDOB_HeaderRow.CreateCell(7).SetCellValue("");
                        }
                        #region Column Header cell style
                        //ChecklistColumnHeaderFont = (XSSFFont)templateWorkbook.CreateFont();
                        //ChecklistColumnHeaderFont.FontHeightInPoints = (short)12;
                        //ChecklistColumnHeaderFont.FontName = "Times New Roman";
                        //// ChecklistReportHeaderFont.FontName = "sans-serif";
                        //ChecklistColumnHeaderFont.IsBold = true;

                        //ColumnHeaderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                        //ColumnHeaderCellStyle.SetFont(ChecklistColumnHeaderFont);
                        //ColumnHeaderCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
                        //ColumnHeaderCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
                        //ColumnHeaderCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
                        //ColumnHeaderCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
                        //ColumnHeaderCellStyle.WrapText = true;
                        //ColumnHeaderCellStyle.VerticalAlignment = VerticalAlignment.Center;
                        //ColumnHeaderCellStyle.Alignment = HorizontalAlignment.Left;
                        //ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(230, 243, 227));
                        ////  ColumnHeaderCellStyle.FillForegroundXSSFColor = new XSSFColor(System.Drawing.Color.FromArgb(226, 239, 218));
                        //ColumnHeaderCellStyle.FillPattern = FillPattern.SolidForeground;
                        #endregion
                        if (result[0].IsCOProject == true)
                        {
                            if (iDOB_HeaderRow.GetCell(8) != null)
                            {

                                if (jobViolation.TCOToggle == true)
                                    iDOB_HeaderRow.GetCell(8).SetCellValue("YES");
                                else
                                    iDOB_HeaderRow.GetCell(8).SetCellValue("No");

                                iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;


                            }
                            else
                            {
                                if (jobViolation.TCOToggle == true)
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("YES");
                                else
                                    iDOB_HeaderRow.CreateCell(8).SetCellValue("No");


                                iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;
                            }
                        }
                        iDOB_HeaderRow.GetCell(0).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(1).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(2).CellStyle = leftAlignCellStyle;

                        iDOB_HeaderRow.GetCell(3).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(4).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(5).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(6).CellStyle = leftAlignCellStyle;
                        iDOB_HeaderRow.GetCell(7).CellStyle = leftAlignCellStyle;

                        // iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;

                        //iDOB_HeaderRow.GetCell(8).CellStyle = leftAlignCellStyle;


                        index = index + 1;
                        #endregion

                    }
                    #endregion


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
      
        [Authorize]
        [RpoAuthorize]
        [HttpPost]
        [Route("api/Checklist/ExportCompositeChecklistToPDFForCustomer")]
        public IHttpActionResult ExportChecklistToPDFForCustomer(CompositeChecklistReportDatatableParameter dataTableParameters)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
            var employee = this.db.Customers.FirstOrDefault(e => e.EmailAddress == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEditChecklist)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteChecklist))
            {
                DataSet ds = new DataSet();
                SqlParameter[] spParameter = new SqlParameter[4];

                spParameter[0] = new SqlParameter("@IdCompositeChecklist", SqlDbType.NVarChar);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = dataTableParameters.IdCompositeChecklist;

                spParameter[1] = new SqlParameter("@OrderFlag", SqlDbType.VarChar);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = dataTableParameters.OrderFlag;

                spParameter[2] = new SqlParameter("@SearchText", SqlDbType.VarChar);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value = !string.IsNullOrEmpty(dataTableParameters.SearchText) ? dataTableParameters.SearchText : string.Empty;

                spParameter[3] = new SqlParameter("@IdCustomer", SqlDbType.Int);
                spParameter[3].Direction = ParameterDirection.Input;
                spParameter[3].Value = employee.Id;

                int Idcompo = Convert.ToInt32(dataTableParameters.IdCompositeChecklist);

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "CompositeViewForCustomer", spParameter);
                List<CompositeChecklistReportDTO> headerlist = new List<CompositeChecklistReportDTO>();

                List<DataTable> distinctheaders = ds.Tables[0].AsEnumerable()
                        .GroupBy(row => new
                        {
                            JobChecklistHeaderId = row.Field<int>("JobChecklistHeaderId"),
                        }).Select(g => g.CopyToDataTable()).ToList();
                try

                {
                    foreach (var loopheader in distinctheaders)
                    {
                        var headers = dataTableParameters.lstexportChecklist.Select(x => x.jobChecklistHeaderId).ToList();
                        if (headers.Contains(Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"])))
                        {
                            CompositeChecklistReportDTO header = new CompositeChecklistReportDTO();
                            Int32 checklistHeaderId = Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                            header.IdCompositeChecklist = loopheader.Rows[0]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdCompositeChecklist"]);
                            header.CompositeChecklistName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                            header.jobChecklistHeaderId = loopheader.Rows[0]["JobChecklistHeaderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["JobChecklistHeaderId"]);
                            header.Others = loopheader.Rows[0]["Others"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["Others"].ToString();
                            header.IdJob = loopheader.Rows[0]["IdJob"] == DBNull.Value ? (int?)null : Convert.ToInt32(loopheader.Rows[0]["IdJob"]);
                            header.IsCOProject = loopheader.Rows[0]["IsCOProject"] == DBNull.Value ? (bool)false : Convert.ToBoolean(loopheader.Rows[0]["IsCOProject"]);
                            header.IsParentCheckList = db.CompositeChecklistDetails.Where(x => x.IdJobChecklistHeader == header.jobChecklistHeaderId && x.IdCompositeChecklist == Idcompo).Select(y => y.IsParentCheckList).FirstOrDefault();
                            var checklist = dataTableParameters.lstexportChecklist.Where(y => y.jobChecklistHeaderId == header.jobChecklistHeaderId).FirstOrDefault();
                            if (header.IsParentCheckList == true)
                            {
                                header.CompositeOrder = 1;
                                string name = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                                header.CompositeChecklistName = "CC - " + name;
                            }
                            else
                            {
                                header.CompositeOrder = checklist.Displayorder + 1;
                                header.CompositeChecklistName = loopheader.Rows[0]["CheckListName"] == DBNull.Value ? string.Empty : loopheader.Rows[0]["CheckListName"].ToString();
                            }
                            header.groups = new List<CompositeReportChecklistGroup>();
                            List<CompositeReportChecklistGroup> Unorderedgroups = new List<CompositeReportChecklistGroup>();
                            string s = null;
                            int id = checklistHeaderId;
                            var temp = db.JobChecklistHeaders.Include("JobApplicationWorkPermitTypes").Where(x => x.IdJobCheckListHeader == id).ToList();
                            var lstidjobworktype = temp[0].JobApplicationWorkPermitTypes.Where(x => x.Code.ToLower().Trim() == "pl" || x.Code.ToLower().Trim() == "sp" || x.Code.ToLower().Trim() == "sd").Select(y => y.IdJobWorkType).ToList();
                            foreach (var i in lstidjobworktype)
                            {
                                s += i.ToString() + ",";
                            }
                            if (s != null)
                                header.IdWorkPermits = s.Remove(s.Length - 1, 1);
                            if (s != null)
                                header.IdWorkPermits = s.Remove(s.Length - 1, 1);

                            var distinctGroup = ds.Tables[0].AsEnumerable().Where(i => i.Field<int>("JobChecklistHeaderId") == checklistHeaderId)
                                .GroupBy(r => new { group = r.Field<int>("IdJobChecklistGroup") }).Select(g => g.CopyToDataTable()).ToList();
                            foreach (var eachGroup in distinctGroup)
                            {
                                var selectedgroups = dataTableParameters.lstexportChecklist.Where(z => z.jobChecklistHeaderId == checklistHeaderId).Select(x => x.lstExportChecklistGroup.Select(y => y.jobChecklistGroupId).ToList()).FirstOrDefault();
                                if (selectedgroups.Contains(Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"])))
                                {
                                    int groupid = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    CompositeReportChecklistGroup group = new CompositeReportChecklistGroup();
                                    group.item = new List<CompositeReportItem>();
                                    Int32 IdJobChecklistGroup = Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    group.checkListGroupName = eachGroup.Rows[0]["ChecklistGroupName"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["ChecklistGroupName"].ToString();
                                    group.jobChecklistGroupId = eachGroup.Rows[0]["IdJobChecklistGroup"] == DBNull.Value ? (int?)null : Convert.ToInt32(eachGroup.Rows[0]["IdJobChecklistGroup"]);
                                    group.checkListGroupType = eachGroup.Rows[0]["checklistGroupType"] == DBNull.Value ? string.Empty : eachGroup.Rows[0]["checklistGroupType"].ToString();
                                    var GroupInPayload = dataTableParameters.lstexportChecklist.Where(x => x.jobChecklistHeaderId == checklistHeaderId).Select(z => z.lstExportChecklistGroup).FirstOrDefault();
                                    var groupfordisplayorder = GroupInPayload.Where(x => x.jobChecklistGroupId == IdJobChecklistGroup).FirstOrDefault();
                                    group.DisplayOrder1 = groupfordisplayorder.displayOrder1;

                                    if (eachGroup.Rows[0]["checklistGroupType"].ToString().ToLower().TrimEnd() != "pl")
                                    {
                                        var groupsitems = ds.Tables[0].AsEnumerable().Where(l => l.Field<int>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();
                                        for (int j = 0; j < groupsitems.Count; j++)
                                        {

                                            CompositeReportItem item = new CompositeReportItem();
                                            item.Details = new List<CompositeReportDetails>();

                                            #region Items
                                            Int32 IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                            if (IdChecklistItem != 0)
                                            {
                                                item.checklistItemName = groupsitems[j]["checklistItemName"] == DBNull.Value ? string.Empty : groupsitems[j]["checklistItemName"].ToString();
                                                item.IdChecklistItem = groupsitems[j]["IdChecklistItem"] == DBNull.Value ? 0 : Convert.ToInt32(groupsitems[j]["IdChecklistItem"]);
                                                item.jocbChecklistItemDetailsId = groupsitems[j]["JocbChecklistItemDetailsId"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["JocbChecklistItemDetailsId"]);
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
                                                item.CompositeName = groupsitems[j]["CompositeName"] == DBNull.Value ? string.Empty : groupsitems[j]["CompositeName"].ToString();
                                                item.IdCompositeChecklist = groupsitems[j]["IdCompositeChecklist"] == DBNull.Value ? (int?)null : Convert.ToInt32(groupsitems[j]["IdCompositeChecklist"]);
                                                item.IsRequiredForTCO = groupsitems[j]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsRequiredForTCO"]);
                                                item.PartyResponsible = groupsitems[j]["PartyResponsible"] == DBNull.Value ? string.Empty : groupsitems[j]["PartyResponsible"].ToString();
                                                item.HasDocument = item.HasDocument = db.JobDocuments.Any(x => x.IdJobchecklistItemDetails == item.jocbChecklistItemDetailsId);
                                                item.IsParentCheckList = groupsitems[j]["IsParentCheckList"] == DBNull.Value ? (bool)false : Convert.ToBoolean(groupsitems[j]["IsParentCheckList"]);
                                                item.ClientNote = groupsitems[j]["ClientNotes"] == DBNull.Value ? string.Empty : groupsitems[j]["ClientNotes"].ToString();
                                                if (item.idContact != null && item.idContact != 0)
                                                {
                                                    var c = db.Contacts.Find(item.idContact);
                                                    item.ContactName = c.FirstName +" "+ c.LastName;
                                                    if (c.IdCompany != null && c.IdCompany != 0)
                                                    {
                                                        item.CompanyName = db.Companies.Where(x => x.Id == c.IdCompany).Select(y => y.Name).FirstOrDefault();
                                                    }
                                                }
                                                if (item.idDesignApplicant != null && item.idDesignApplicant != 0)
                                                {
                                                    var c = db.JobContacts.Find(item.idDesignApplicant);
                                                    if (c != null)
                                                    {
                                                        item.DesignApplicantName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                                    }

                                                }
                                                if (item.idInspector != null && item.idInspector != 0)
                                                {
                                                    var c = db.JobContacts.Find(item.idInspector);
                                                    if (c != null)
                                                    {
                                                        item.InspectorName = c.Contact != null ? c.Contact.FirstName + " " + c.Contact.LastName : string.Empty;
                                                    }

                                                }
                                                #endregion
                                                if (dataTableParameters.OnlyTCOItems == true)
                                                {
                                                    if (item.IsRequiredForTCO == true)
                                                    {
                                                        group.item.Add(item);
                                                    }
                                                }
                                                else
                                                {
                                                    group.item.Add(item);
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {

                                        var groupsitems2 = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobChecklistGroup") == IdJobChecklistGroup).ToList();

                                        var PlumbingCheckListFloors = groupsitems2.AsEnumerable().Select(a => a.Field<Int32?>("IdJobPlumbingCheckListFloors")).Distinct().ToList();

                                        for (int j = 0; j < PlumbingCheckListFloors.Count(); j++)
                                        {

                                            CompositeReportItem item = new CompositeReportItem();
                                            item.Details = new List<CompositeReportDetails>();
                                            var IdJobPlumbingCheckListFloors1 = PlumbingCheckListFloors[j];
                                            var detailItem = ds.Tables[0].AsEnumerable().Where(l => l.Field<Int32?>("IdJobPlumbingCheckListFloors") == IdJobPlumbingCheckListFloors1).ToList();
                                            #region Items
                                            item.FloorNumber = detailItem[0]["FloorNumber"] == DBNull.Value ? string.Empty : detailItem[0]["FloorNumber"].ToString();
                                            item.idJobPlumbingCheckListFloors = IdJobPlumbingCheckListFloors1 != 0 ? IdJobPlumbingCheckListFloors1 : 0;
                                            item.FloorDisplayOrder = detailItem[0]["FloorDisplayOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[0]["FloorDisplayOrder"]);
                                            #endregion

                                            for (int i = 0; i < detailItem.Count; i++)
                                            {
                                                CompositeReportDetails detail = new CompositeReportDetails();
                                                #region Items details
                                                detail.checklistGroupType = detailItem[i]["checklistGroupType"] == DBNull.Value ? string.Empty : detailItem[i]["checklistGroupType"].ToString();
                                                detail.idJobPlumbingInspection = detailItem[i]["IdJobPlumbingInspection"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingInspection"]);
                                                detail.idJobPlumbingCheckListFloors = detailItem[i]["IdJobPlumbingCheckListFloors"] == DBNull.Value ? (int?)null : Convert.ToInt32(detailItem[i]["IdJobPlumbingCheckListFloors"]);
                                                detail.inspectionPermit = detailItem[i]["checklistItemName"] == DBNull.Value ? string.Empty : detailItem[i]["checklistItemName"].ToString();
                                                detail.workOrderNumber = detailItem[i]["WorkOrderNo"] == DBNull.Value ? string.Empty : detailItem[i]["WorkOrderNo"].ToString();
                                                detail.plComments = detailItem[i]["Comments"] == DBNull.Value ? string.Empty : detailItem[i]["Comments"].ToString();
                                                detail.DueDate = detailItem[i]["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["DueDate"]);
                                                detail.nextInspection = detailItem[i]["NextInspection"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(detailItem[i]["NextInspection"]);
                                                detail.result = detailItem[i]["Result"] == DBNull.Value ? string.Empty : detailItem[i]["Result"].ToString();
                                                detail.IsRequiredForTCO = detailItem[i]["IsRequiredForTCO"] == DBNull.Value ? (bool)false : Convert.ToBoolean(detailItem[i]["IsRequiredForTCO"]);
                                                detail.HasDocument = db.JobDocuments.Any(x => x.IdJobPlumbingInspections == detail.idJobPlumbingInspection);
                                                detail.plCientNote = detailItem[i]["ClientNotes"] == DBNull.Value ? string.Empty : detailItem[i]["ClientNotes"].ToString();
                                                if (dataTableParameters.OnlyTCOItems == true)
                                                {
                                                    if (detail.IsRequiredForTCO == true)
                                                    {
                                                        item.Details.Add(detail);
                                                    }
                                                }
                                                else
                                                {
                                                    item.Details.Add(detail);
                                                }
                                                #endregion

                                            }
                                            group.item.Add(item);
                                        }
                                    }

                                    Unorderedgroups.Add(group);
                                }
                            }
                            header.groups = Unorderedgroups.OrderBy(x => x.DisplayOrder1).ToList();
                            headerlist.Add(header);
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    return this.NotFound();
                }
                #region PDF
                var result = headerlist.OrderBy(x => x.CompositeOrder).ToList();

                if (result != null && result.Count > 0)
                {
                    string exportFilename = string.Empty;

                    exportFilename = "CompositeChecklistCustomerReport_" + Convert.ToString(Guid.NewGuid()) + ".pdf";
                    string exportFilePath = ExportToPdfForCustomer(result, exportFilename, dataTableParameters.IncludeViolations, dataTableParameters.OnlyTCOItems);
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

        private string ExportToPdfForCustomer(List<CompositeChecklistReportDTO> result, string exportFilename, bool isViolation, bool OnlyTCOItems)
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
                int templateColumns;
                if (result[0].IsCOProject == true)
                { templateColumns = 14; }
                else
                { templateColumns = 13; }


                PdfPTable table = new PdfPTable(templateColumns);
                table.WidthPercentage = 100;
                table.SplitLate = false;
                Job job = db.Jobs.Include("RfpAddress.Borough").Include("Company").Include("ProjectManager").FirstOrDefault(x => x.Id == ProjectNumber);


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
                if (result[0].IsCOProject == true)
                    cell.PaddingLeft = 0;
                else
                    cell.PaddingLeft = -5;
                cell.Colspan = templateColumns;
                cell.PaddingTop = -5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportHeader, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                if (result[0].IsCOProject == true)
                    cell.PaddingLeft = 0;
                else
                    cell.PaddingLeft = -5;
                cell.Colspan = templateColumns;
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
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(projectAddress, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.Padding = -15;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                #endregion
                #region ProjectNumber

                cell = new PdfPCell(new Phrase("RPO Project Number: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(ProjectNumber), font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.Padding = -15;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Client
                string clientName = job != null && job.Company != null ? job.Company.Name : string.Empty;
                cell = new PdfPCell(new Phrase("Client: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(clientName, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.Padding = -15;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Project Manager
                string ProjectManagerName = job.ProjectManager != null ? job.ProjectManager.FirstName + " " + job.ProjectManager.LastName + " | " + job.ProjectManager.Email : string.Empty;
                cell = new PdfPCell(new Phrase("Project Manager: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(ProjectManagerName, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.Padding = -15;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Document
                cell = new PdfPCell(new Phrase("Document: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Project Report", font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.Padding = -15;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);
                #endregion
                #region Report Date
                string reportDate = DateTime.Today.ToString(Common.ExportReportDateFormat);
                cell = new PdfPCell(new Phrase("Report Generated on: ", font_10_Bold));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = 2;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(reportDate, font_10_Normal));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.NO_BORDER;
                cell.Colspan = templateColumns;
                cell.PaddingBottom = 5;
                cell.Padding = -15;
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
                        { DOB_ReportHeader = c.CompositeChecklistName + " - " + c.Others; }
                        else
                        { DOB_ReportHeader = c.CompositeChecklistName; }
                        cell = new PdfPCell(new Phrase(DOB_ReportHeader, font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.TOP_BORDER;
                        cell.Colspan = templateColumns;
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
                            cell.Colspan = templateColumns;
                            cell.PaddingBottom = 5;
                            table.AddCell(cell);
                            #endregion
                            if (g.checkListGroupType.ToLower().Trim() == "general")
                            {
                                #region header
                                cell = new PdfPCell(new Phrase("Item Name", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 4;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Party Responsible", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 2;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Comments", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 3;
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

                                if (c.IsCOProject == true)
                                {
                                    cell = new PdfPCell(new Phrase("TCO?", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);
                                }
                                cell = new PdfPCell(new Phrase("Client Note", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 2;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);
                                #endregion
                                #region Data
                                foreach (var item in g.item)
                                {
                                    cell = new PdfPCell(new Phrase(item.checklistItemName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 4;
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
                                    cell.Colspan = 2;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.comments, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 3;
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

                                    string TCO = string.Empty;
                                    if (c.IsCOProject == true)
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            TCO = "YES";
                                        else
                                            TCO = "NO";
                                        cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 1;
                                        table.AddCell(cell);
                                    }

                                    cell = new PdfPCell(new Phrase(item.ClientNote, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
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
                                cell.Colspan = 2;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Design Applicant", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 2;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                table.AddCell(cell);

                                cell = new PdfPCell(new Phrase("Inspector", font_Table_Header));
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

                                cell = new PdfPCell(new Phrase("Stage", font_Table_Header));
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

                                if (c.IsCOProject == true)
                                {
                                    cell = new PdfPCell(new Phrase("TCO?", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);
                                }
                                cell = new PdfPCell(new Phrase("Client Note", font_Table_Header));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 2;
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

                                    cell = new PdfPCell(new Phrase(item.DesignApplicantName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.InspectorName, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase(item.comments, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
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
                                    cell.Colspan = 2;
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
                                    cell.Colspan = 1;
                                    table.AddCell(cell);

                                    string TCO = string.Empty;
                                    if (c.IsCOProject == true)
                                    {
                                        if (item.IsRequiredForTCO == true)
                                            TCO = "YES";
                                        else
                                            TCO = "NO";
                                        cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 1;
                                        table.AddCell(cell);
                                    }

                                    cell = new PdfPCell(new Phrase(item.ClientNote, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 2;
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
                                    cell.Colspan = templateColumns;
                                    cell.PaddingBottom = 5;
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Inspection Type", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 4;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Comment", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 4;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);

                                    cell = new PdfPCell(new Phrase("Status", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 4;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);
                                    if (c.IsCOProject == true)
                                    {
                                        cell = new PdfPCell(new Phrase("TCO?", font_Table_Header));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 1;
                                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                        table.AddCell(cell);
                                    }
                                    cell = new PdfPCell(new Phrase("Client Note", font_Table_Header));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 4;
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                                    table.AddCell(cell);
                                    foreach (var d in item.Details)
                                    {
                                        #region Data
                                        cell = new PdfPCell(new Phrase(d.inspectionPermit, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 4;
                                        table.AddCell(cell);

                                        cell = new PdfPCell(new Phrase(d.plComments, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 4;
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
                                        cell.Colspan = 4;
                                        table.AddCell(cell);

                                        string TCO = string.Empty;
                                        if (c.IsCOProject == true)
                                        {
                                            if (d.IsRequiredForTCO == true)
                                                TCO = "YES";
                                            else
                                                TCO = "NO";
                                            cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                            cell.Border = PdfPCell.BOX;
                                            cell.Colspan = 1;
                                            table.AddCell(cell);
                                        }
                                        cell = new PdfPCell(new Phrase(item.ClientNote, font_Table_Data));
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        cell.Border = PdfPCell.BOX;
                                        cell.Colspan = 4;
                                        table.AddCell(cell);
                                        #endregion

                                    }

                                }
                                #endregion
                            }
                        }
                    }
                }
                if (isViolation)
                {
                    int Idrfpaddress = db.Jobs.Find(ProjectNumber).IdRfpAddress;
                    string BinNumber = db.RfpAddresses.Where(x => x.Id == Idrfpaddress).Select(y => y.BinNumber).FirstOrDefault();
                    int? IdCompositeChecklist = result[0].IdCompositeChecklist;
                    #region ECB Violations             

                    List<JobViolation> lstjobECBviolation = new List<JobViolation>();
                    var CompositeECBViolations = db.CompositeViolations.Where(x => x.IdCompositeChecklist == IdCompositeChecklist).ToList();
                    foreach (var c in CompositeECBViolations)
                    {
                        JobViolation violation = new JobViolation();
                        if (OnlyTCOItems == true)
                        {
                            violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "ECB" & x.TCOToggle == true).FirstOrDefault();
                        }
                        else
                        {
                            violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "ECB").FirstOrDefault();
                        }
                        if (violation != null)
                            lstjobECBviolation.Add(violation);
                    }
                    if (lstjobECBviolation != null && lstjobECBviolation.Count > 0)
                    {
                        cell = new PdfPCell(new Phrase("ECB/OATH Violations", font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.TOP_BORDER;
                        cell.Colspan = templateColumns;
                        cell.PaddingBottom = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Issue Date", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("ECB Violation No.", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 3;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Violation Description", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 3;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Party Responsible", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Comments", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 3;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Status", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        if (result[0].IsCOProject == true)
                        {
                            cell = new PdfPCell(new Phrase("TCO?", font_Table_Header));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                            table.AddCell(cell);
                        }

                        foreach (var jobViolation in lstjobECBviolation)
                        {
                            #region Data
                            cell = new PdfPCell(new Phrase(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(jobViolation.SummonsNumber, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 3;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(jobViolation.ViolationDescription, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 3;
                            table.AddCell(cell);

                            string PartyResponsible = string.Empty;
                            if (jobViolation.PartyResponsible == 1) //3 means other 1 means RPOteam                              
                                PartyResponsible = "RPO Team";
                            else if (jobViolation.PartyResponsible == 3)
                            { PartyResponsible = jobViolation.ManualPartyResponsible; }
                            cell = new PdfPCell(new Phrase(PartyResponsible, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            table.AddCell(cell);


                            var comment = (from cmt in db.ChecklistJobViolationComments
                                           where cmt.IdJobViolation == jobViolation.Id
                                           orderby cmt.LastModifiedDate descending
                                           select cmt.Description).FirstOrDefault();

                            cell = new PdfPCell(new Phrase(comment, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 3;
                            table.AddCell(cell);

                            string Status = string.Empty;
                            if (jobViolation.Status == 1)
                                Status = "Open";
                            else if (jobViolation.Status == 2)
                                Status = "InProcess";
                            else if (jobViolation.Status == 3)
                                Status = "Completed";
                            else
                                Status = "";
                            cell = new PdfPCell(new Phrase(Status, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 2;
                            table.AddCell(cell);

                            string TCO = string.Empty;
                            if (result[0].IsCOProject == true)
                            {
                                if (OnlyTCOItems == true)
                                {
                                    if (jobViolation.TCOToggle == true)
                                        TCO = "YES";
                                    cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    table.AddCell(cell);
                                }
                                else
                                {
                                    if (jobViolation.TCOToggle == true)
                                        TCO = "YES";
                                    else
                                        TCO = "No";
                                    cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell.Border = PdfPCell.BOX;
                                    cell.Colspan = 1;
                                    table.AddCell(cell);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region DOB Violations 
                    List<JobViolation> lstjobDOBviolation = new List<JobViolation>();
                    var CompositeDOBViolations = db.CompositeViolations.Where(x => x.IdCompositeChecklist == IdCompositeChecklist).ToList();
                    foreach (var c in CompositeDOBViolations)
                    {
                        JobViolation violation = new JobViolation();
                        if (OnlyTCOItems == true)
                        {
                            violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "DOB" & x.TCOToggle == true).FirstOrDefault();
                        }
                        else
                        {
                            violation = db.JobViolations.Where(x => x.Id == c.IdJobViolations & x.Type_ECB_DOB == "DOB").FirstOrDefault();
                        }
                        if (violation != null)
                            lstjobDOBviolation.Add(violation);
                    }
                    if (lstjobDOBviolation != null && lstjobDOBviolation.Count > 0)
                    {
                        cell = new PdfPCell(new Phrase("DOB Violations", font_12_Bold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.TOP_BORDER;
                        cell.Colspan = templateColumns;
                        cell.PaddingBottom = 5;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Issue Date", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("DOB Violation No.", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Related ECB No.", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Violation Description", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 3;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Party Responsible", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Comments", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 3;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Status", font_Table_Header));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = PdfPCell.BOX;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                        table.AddCell(cell);

                        if (result[0].IsCOProject == true)
                        {
                            cell = new PdfPCell(new Phrase("TCO?", font_Table_Header));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            cell.BackgroundColor = new iTextSharp.text.BaseColor(226, 239, 218);
                            table.AddCell(cell);
                        }

                        foreach (var jobViolation in lstjobDOBviolation)
                        {
                            #region Data
                            cell = new PdfPCell(new Phrase(jobViolation.DateIssued != null ? Convert.ToDateTime(jobViolation.DateIssued).ToString(Common.ExportReportDateFormat) : string.Empty, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(jobViolation.SummonsNumber, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 2;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(jobViolation.ECBnumber, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(jobViolation.ViolationDescription, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 3;
                            table.AddCell(cell);

                            string PartyResponsible = string.Empty;
                            if (jobViolation.PartyResponsible == 1) //3 means other 2 means RPOteam
                                PartyResponsible = "RPO Team";
                            else if (jobViolation.PartyResponsible == 3)
                            {
                                PartyResponsible = jobViolation.ManualPartyResponsible;
                            }
                            cell = new PdfPCell(new Phrase(PartyResponsible, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 1;
                            table.AddCell(cell);

                            var comment = (from cmt in db.ChecklistJobViolationComments
                                           where cmt.IdJobViolation == jobViolation.Id
                                           orderby cmt.LastModifiedDate descending
                                           select cmt.Description).FirstOrDefault();
                            cell = new PdfPCell(new Phrase(comment, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 3;
                            table.AddCell(cell);

                            string Status = string.Empty;
                            if (jobViolation.Status == 1)
                                Status = "Open";
                            else if (jobViolation.Status == 2)
                                Status = "InProcess";
                            else if (jobViolation.Status == 3)
                                Status = "Completed";
                            else
                                Status = "";
                            cell = new PdfPCell(new Phrase(Status, font_Table_Data));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Border = PdfPCell.BOX;
                            cell.Colspan = 2;
                            table.AddCell(cell);

                            string TCO = string.Empty;
                            if (result[0].IsCOProject == true)
                            {
                                if (jobViolation.TCOToggle == true)
                                    TCO = "YES";
                                else
                                    TCO = "NO";
                                cell = new PdfPCell(new Phrase(TCO, font_Table_Data));
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Border = PdfPCell.BOX;
                                cell.Colspan = 1;
                                table.AddCell(cell);
                            }
                        }
                        #endregion
                    }
                    #endregion
                }

                document.Add(table);
                document.Close();

                writer.Close();
            }

            return exportFilePath;
        }
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