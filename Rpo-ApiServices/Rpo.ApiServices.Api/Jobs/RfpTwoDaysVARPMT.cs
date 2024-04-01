using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using Rpo.ApiServices.Model.Models;
using System.Web;
using Rpo.ApiServices.Api.Controllers.JobDocument.Models;
using Rpo.ApiServices.Api.Enums;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.IO;
using Rpo.ApiServices.Api.Controllers.SystemSettings;
using Rpo.ApiServices.Api.Controllers.Employees;
using Rpo.ApiServices.Api.Tools;
using System.Net;

namespace Rpo.ApiServices.Api.Jobs
{
    public class RfpTwoDaysVARPMT : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            CheckVARPMT();
        }
        public static void CheckVARPMT()
        {
            ApplicationLog.WriteInformationLog("VARPMT Reminder Job Send Mail Notification executed : " + DateTime.Now.ToLongDateString());
            if (Convert.ToBoolean(Properties.Settings.Default.TaskReminderSchedulerStart))
            {
                ApplicationLog.WriteInformationLog("VARPMT Reminder Job Send Mail Notification execution start : " + DateTime.Now.ToLongDateString());
                using (var ctx = new Model.RpoContext())
                {
                    //DateTime todayDate = DateTime.UtcNow;

                    DateTime twoDaysDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(-1), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

                    List<JobDocument> jobDocumentList = (from d in ctx.JobDocuments where d.CreatedDate.Value > twoDaysDate select d).ToList();

                    foreach (var item in jobDocumentList)
                    {
                        List<JobDocumentField> jobDocumentFieldList = (from d in ctx.JobDocumentFields where d.IdJobDocument == item.Id select d).ToList();

                        var objApplicationType = (from d in ctx.JobApplicationTypes where d.Id == item.IdJobApplication select d.Description).FirstOrDefault();

                        string jobApplicationNumber = item.JobApplication.Id.ToString();
                        string ApplicationType = objApplicationType != null ? objApplicationType : string.Empty;
                        string PermitNo = string.Empty;
                        string AHVReferenceNumber = string.Empty;
                        string Type = "Initial";
                        string AHVStatus = "ISSUED";
                        string urlAddress = $"http://a810-bisweb.nyc.gov/bisweb/AHVPermitsQueryByNumberServlet?requestid=4&allkey={jobApplicationNumber}A&passjobnumber={jobApplicationNumber}";

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                           | SecurityProtocolType.Tls11
                                           | SecurityProtocolType.Tls12;

                        HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
                        var descendants = doc.DocumentNode.Descendants();

                        var block = descendants.FirstOrDefault(n => n.HasClass("centercolhdg") && n.InnerHtml.Contains("Reference <br>Number"));
                        var blockParent = block.ParentNode.ParentNode;
                        int i = 0;

                        foreach (var itemDocumentField in jobDocumentFieldList)
                        {
                            if (itemDocumentField.DocumentField.Field.Id == int.Parse(DocumentPlaceHolderField.VarPMT_Work_Permits.ToString()))
                            {
                                PermitNo = itemDocumentField.Value;
                            }
                            if (itemDocumentField.DocumentField.Field.Id == int.Parse(DocumentPlaceHolderField.AHVReferenceNumber.ToString()))
                            {
                                AHVReferenceNumber = itemDocumentField.Value;
                            }

                            if (AHVReferenceNumber != null && AHVReferenceNumber != "")
                            {
                                foreach (var itemnode in blockParent.ChildNodes)
                                {
                                    if (itemnode.NodeType == HtmlNodeType.Element && i > 2 && itemnode.InnerHtml.Contains(AHVReferenceNumber) && itemnode.InnerHtml.Contains(AHVStatus))
                                    {
                                        JobDocumentVARPMTPermit jobDocumentVARPMTPermit = new JobDocumentVARPMTPermit();

                                        int columnIndex = 1;
                                       
                                        foreach (var childItem in itemnode.ChildNodes)
                                        {
                                            if (childItem.NodeType == HtmlNodeType.Element)
                                            {
                                                switch (columnIndex)
                                                {
                                                    case 1:
                                                        if (childItem.InnerText != null && childItem.InnerText.Trim() == AHVReferenceNumber.Trim())
                                                        {
                                                            jobDocumentVARPMTPermit.ReferenceNumber = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                        }
                                                        if (!string.IsNullOrEmpty(jobDocumentVARPMTPermit.ReferenceNumber) && childItem != null && childItem.FirstChild != null && childItem.FirstChild.Attributes != null)
                                                        {
                                                            jobDocumentVARPMTPermit.DetailUrl = childItem.FirstChild.Attributes.FirstOrDefault(x => x.Name == "href" && x.Value.Contains("AHVPermitDetailsServlet")).Value;
                                                        }
                                                        break;
                                                    case 2:
                                                        jobDocumentVARPMTPermit.EntryDate = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                        break;
                                                    case 3:
                                                        jobDocumentVARPMTPermit.Status = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                        break;
                                                    case 4:
                                                        jobDocumentVARPMTPermit.StartDate = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                        break;
                                                    case 5:
                                                        jobDocumentVARPMTPermit.EndDate = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                        break;
                                                    case 6:
                                                        jobDocumentVARPMTPermit.PermissibleDaysforeRenewal = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                        break;
                                                    case 7:
                                                        jobDocumentVARPMTPermit.Applicant = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Replace("GC - 002305//-->", "").Trim() : string.Empty;
                                                        break;
                                                    case 8:
                                                        jobDocumentVARPMTPermit.Type = !string.IsNullOrEmpty(childItem.InnerHtml) ? Regex.Replace(childItem.InnerHtml, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                                                        break;
                                                }
                                                columnIndex = columnIndex + 1;
                                            }
                                        }

                                        if (jobDocumentVARPMTPermit.Status.Trim() == AHVStatus)
                                        {
                                            string ApprovedDays = ReadVARPMTPermitDays(jobDocumentVARPMTPermit.DetailUrl);

                                            List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                                            SystemSettingDetail systemSettingDetail = Rpo.ApiServices.Api.Tools.Common.ReadSystemSetting(Enums.SystemSetting.WhenRFPIsIndraftForMoreThan2days);
                                            if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                                            {
                                                foreach (EmployeeDetail itemEmployee in systemSettingDetail.Value)
                                                {
                                                    cc.Add(new KeyValuePair<string, string>(itemEmployee.Email, itemEmployee.Email));
                                                }
                                            }

                                            string body = string.Empty;
                                            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/VARPMTEmailTemplate.htm")))
                                            {
                                                body = reader.ReadToEnd();
                                            }

                                            var from = new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP");

                                            var employeedetail = (from d in ctx.Employees where d.Id == item.CreatedBy select d).FirstOrDefault();
                                            if (employeedetail != null)
                                            {
                                                var to = new KeyValuePair<string, string>(employeedetail.Email, employeedetail.FirstName + " " + employeedetail.LastName);
                                                
                                                body = body.Replace("##User##", employeedetail.FirstName);
                                                body = body.Replace("##ApplicationNumber##", jobApplicationNumber);
                                                body = body.Replace("##WorkType##", ApplicationType);
                                                body = body.Replace("##Status##",  AHVStatus);
                                                body = body.Replace("##ApprovedFor##", ApprovedDays);

                                                string subject = EmailNotificationSubject.VARPMTApproved.Replace("##AHVReferenceNo##", AHVReferenceNumber).Replace("##ApplicationNo##", jobApplicationNumber).Replace("##Status##", jobApplicationNumber);
                                                Mail.Send(from, to, cc, subject, body, true);                                                
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public static string ReadVARPMTPermitDays(string urlAddress)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(urlAddress);
            var descendants = doc.DocumentNode.Descendants();
            var block = descendants.FirstOrDefault(n => n.HasClass("centerlabel"));

            var mainblock = block.ParentNode.ParentNode;

            var blockcentercontent = descendants.Where(n => n.HasClass("centercontent")).ToList();
            string mainstring = string.Empty;

            mainstring = "<table> <tr> <td>Start Day:</td><td>Days:</td><td>Hours From:</td><td>Hours To:</td></tr><tr>";

            int i = 0, j = 0;
            foreach (var item in blockcentercontent)
            {
                if (j > 4)
                {
                    if (i > 3)
                    {
                        i = 0;
                        mainstring = mainstring + "<tr>";
                    }

                    if (i == 0)
                    {
                        mainstring = mainstring +"<td>"+ item.InnerText+"</td>";
                    }
                    if (i == 1)
                    {
                        mainstring = mainstring + "<td>" + item.InnerText + "</td>";
                    }
                    if (i == 2)
                    {
                        mainstring = mainstring + "<td>" + item.InnerText + "</td>";
                    }
                    if (i == 3)
                    {
                        mainstring = mainstring + "<td>" + item.InnerText + "</td></tr>";
                    }
                    i++;
                }
                j++;
            }

            if (j!=0)
            {
                mainstring = mainstring + "</table>";
            }
            else
            {
                mainstring = mainstring + "</tr></table>";
            }

            return mainstring;
        }
    }
}