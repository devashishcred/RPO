namespace Rpo.ApiServices.Api.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Controllers.JobViolations;
    using Controllers.SystemSettings;
    using HtmlAgilityPack;
    using Model;
    using Model.Models;
    using Quartz;
    using Rpo.ApiServices.Api.Tools;
    using Model.Models.Enums;
    using SODA;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Threading;
    public class JobDOBUpdateResult : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }
        //public JobDOBUpdateResult()
        //    {
        //    SendMailNotification();
        //    }

        public static void SendMailNotification()
        {
            ApplicationLog.WriteInformationLog("DOB Violation Update Result Send Mail Notification executed: " + DateTime.Now.ToLongDateString());
            ApplicationLog.WriteInformationLog("StartTime : " + DateTime.UtcNow.ToString());
            WriteLogWebclient("start DOB Update Violation cronjob");
            WriteLogWebclient("StartTime : " + DateTime.UtcNow.ToString());
            StringBuilder sb = new StringBuilder();

            //Get the summonsNoticeNumber from Database.
            var ctx = new Model.RpoContext();
            string body = string.Empty;
            string emailtemplatepath = AppDomain.CurrentDomain.BaseDirectory + "EmailTemplate/JobViolationsStatusapiTemplate.htm";
            using (StreamReader reader = new StreamReader(emailtemplatepath))
            {
                body = reader.ReadToEnd();
            }
            string emailBody = body;

            var objJobViolation = (from v in ctx.JobViolations
                                   join rfps in ctx.RfpAddresses on v.BinNumber equals rfps.BinNumber
                                   join job in ctx.Jobs on rfps.Id equals job.IdRfpAddress
                                   where job.Status == JobStatus.Active && v.Type_ECB_DOB == "DOB"
                                   select v.SummonsNumber).Distinct().ToList();

            WriteLogWebclient("Active DOB Violation IsFullyResolved count :" + objJobViolation.Count());


            int? PreviousJob = 0;
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

            emailBody = emailBody.Replace("##ExecuteDate##", easternTime.ToString("MM/dd/yyyy"));
            emailBody = emailBody.Replace("##ExecuteStartTime##", easternTime.ToString("hh:mm tt"));
            emailBody = emailBody.Replace("##NotFullyresolved##", objJobViolation.Count().ToString());

            int totalno = 0;
            string ResultNot = string.Empty;
            string ResultYes = string.Empty;
            int BisScanCount = 0;
            int BisScanNoCount = 0;

            int OATHYesScanCount = 0;
            int OATHNoScanCount = 0;

            string OATHYesScanRec = string.Empty;
            string OATHNoScanRec = string.Empty;

            int ECBYesScanCount = 0;
            int ECBNoScanCount = 0;

            int ErrorCount = 0;
            string ErrorRec = string.Empty;
            string ErrorRecNotFormat = "Job# ##Job## - JobViolation# ##JobViolation## ";

            string ECBYesScanRec = string.Empty;
            string ECBNoScanRec = string.Empty;

            foreach (var objSummons in objJobViolation)
            {
                var itemSummons = (from violation in ctx.JobViolations
                                   join rfps in ctx.RfpAddresses on violation.BinNumber equals rfps.BinNumber
                                   join job in ctx.Jobs on rfps.Id equals job.IdRfpAddress
                                   where job.Status == JobStatus.Active && violation.SummonsNumber == objSummons.TrimEnd()
                                   && violation.Type_ECB_DOB == "DOB"
                                   select new { violation, job }).OrderByDescending(x => x.violation.Id).FirstOrDefault();
                try
                {
                    if (itemSummons != null)
                    {
                        ApplicationLog.WriteInformationLog("Job# : " + itemSummons != null && itemSummons.job != null ? itemSummons.job.Id.ToString() : string.Empty + " ");
                        ApplicationLog.WriteInformationLog("Violation# : " + itemSummons != null && itemSummons.violation != null ? itemSummons.violation.SummonsNumber.ToString() : string.Empty + " ");
                        WriteLogWebclient("Job# : " + itemSummons.job.Id.ToString() + " ");
                        WriteLogWebclient("Violation# : " + itemSummons.violation.SummonsNumber.ToString() + " ");

                        string commanMessage = string.Empty;
                        string qryDOB = string.Empty;

                        Tools.ClsApplicationStatus objapp = new Tools.ClsApplicationStatus();

                        var clientBIS = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
                        var datasetBIS = clientBIS.GetResource<object>("3h2n-5cm9");
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                          SecurityProtocolType.Tls11 |
                          SecurityProtocolType.Tls12;
                        var rowsBIS = datasetBIS.GetRows(limit: 1000);

                        qryDOB = qryDOB + "number='" + itemSummons.violation.SummonsNumber.TrimEnd() + "'";

                        var soqlDOB = new SoqlQuery().Select("*").Where(qryDOB);
                        var resultsDOB = datasetBIS.Query(soqlDOB);

                        string summonsNumber = string.Empty;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        string StatusNotFormat = "Job# ##Job## - JobViolation# ##JobViolation## ";
                        string StatusFormat = "Job# ##Job## - JobViolation# ##JobViolation## -- ##hearingDate## ";

                        if (resultsDOB != null && resultsDOB.Count() > 0)
                        {
                            WriteLogWebclient("DOB API Get Result ");
                            foreach (var item in resultsDOB)
                            {
                                string jsonstring = JsonConvert.SerializeObject(item);
                                var records = JsonConvert.DeserializeObject<ViolationDOBJsonRes>(jsonstring);
                                if (records.disposition_comments != null && records.disposition_comments.ToLower().Contains("deleted"))
                                {
                                }
                                else
                                {
                                    if (records.violation_type != null && !string.IsNullOrEmpty(records.violation_type) && (string.IsNullOrEmpty(itemSummons.violation.violation_type) || itemSummons.violation.violation_type.ToLower().Trim() != records.violation_type.ToLower().Trim()))
                                    {
                                        commanMessage = commanMessage + " " + "Violation Type was updated to <'" + records.violation_type + "'>.";

                                        itemSummons.violation.violation_type = Convert.ToString(records.violation_type);
                                        itemSummons.violation.LastModifiedBy = 2;
                                        itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                                        itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                                    }
                                    //WriteLogWebclient("bin Number of Database is : " + itemSummons.BinNumber);
                                    //WriteLogWebclient("bin Number of API is : " + records.bin);
                                    //if (records.bin != null && !string.IsNullOrEmpty(records.bin) && (string.IsNullOrEmpty(itemSummons.BinNumber) || itemSummons.BinNumber.ToLower().Trim() != records.bin.ToLower().Trim()))
                                    //{
                                    //    commanMessage = commanMessage + " " + "Bin Number was updated to <'" + records.violation_type + "'>.";

                                    //    itemSummons.BinNumber = Convert.ToString(records.bin);
                                    //    itemSummons.LastModifiedBy = 2;
                                    //    itemSummons.LastModifiedDate = DateTime.UtcNow;
                                    //    itemSummons.IsUpdateMailsent = false;
                                    //}
                                    if (records.description != null && !string.IsNullOrEmpty(records.description) && (string.IsNullOrEmpty(itemSummons.violation.ViolationDescription) || itemSummons.violation.ViolationDescription.ToLower().Trim() != records.description.ToLower().Trim()))
                                    {
                                        commanMessage = commanMessage + " " + "Violation Description was updated to <'" + records.description + "'>.";
                                        itemSummons.violation.ViolationDescription = Convert.ToString(records.description);
                                        itemSummons.violation.LastModifiedBy = 2;
                                        itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                                        itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                                    }
                                    DateTime disposition_date;
                                    if (records.disposition_date != null)
                                    {
                                        disposition_date = DateTime.ParseExact(records.disposition_date, "yyyyMMdd", CultureInfo.InvariantCulture);
                                        DateTime disposition_date2 = Convert.ToDateTime(itemSummons.violation.Disposition_Date);
                                        if (disposition_date != null && disposition_date2 != null || disposition_date2 != disposition_date)
                                        {
                                            itemSummons.violation.Disposition_Date = disposition_date;
                                            itemSummons.violation.LastModifiedBy = 2;
                                            itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                                            itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                                            commanMessage = commanMessage + " " + "Disposition Date was updated to <'" + disposition_date + "'>.";
                                        }
                                    }
                                    if (records.violation_category != null && !string.IsNullOrEmpty(records.violation_category) && (string.IsNullOrEmpty(itemSummons.violation.violation_category) || itemSummons.violation.violation_category.ToLower().Trim() != records.violation_category.ToLower().Trim()))
                                    {
                                        itemSummons.violation.violation_category = Convert.ToString(records.violation_category);
                                        itemSummons.violation.LastModifiedBy = 2;
                                        itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                                        itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                                        commanMessage = commanMessage + " " + "Violation Category was updated to <'" + Convert.ToString(records.violation_category) + "'>.";
                                    }
                                    if (records.violation_type_code != null && !string.IsNullOrEmpty(records.violation_type_code) && (string.IsNullOrEmpty(itemSummons.violation.violation_type_code) || itemSummons.violation.violation_type_code.ToLower().Trim() != records.violation_type_code.ToLower().Trim()))
                                    {
                                        itemSummons.violation.violation_type_code = Convert.ToString(records.violation_type_code);
                                        itemSummons.violation.LastModifiedBy = 2;
                                        itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                                        itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                                        commanMessage = commanMessage + " " + "Violation Type Code was updated to <'" + Convert.ToString(records.violation_type_code) + "'>.";

                                    }
                                    if (records.disposition_comments != null && !string.IsNullOrEmpty(records.disposition_comments) && (string.IsNullOrEmpty(itemSummons.violation.Disposition_Comments) || itemSummons.violation.Disposition_Comments.ToLower().Trim() != records.disposition_comments.ToLower().Trim()))
                                    {
                                        itemSummons.violation.Disposition_Comments = Convert.ToString(records.disposition_comments);
                                        itemSummons.violation.LastModifiedBy = 2;
                                        itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                                        itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                                        commanMessage = commanMessage + " " + "Disposition Comments was updated to <'" + records.disposition_comments + "'>.";

                                    }
                                    if (records.device_number != null && !string.IsNullOrEmpty(records.device_number) && (string.IsNullOrEmpty(itemSummons.violation.device_number) || itemSummons.violation.device_number.ToLower().Trim() != records.device_number.ToLower().Trim()))
                                    {
                                        itemSummons.violation.device_number = Convert.ToString(records.device_number);
                                        itemSummons.violation.LastModifiedBy = 2;
                                        itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                                        itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                                        commanMessage = commanMessage + " " + "Device Number was updated to <'" + records.device_number + "'>.";
                                    }
                                    itemSummons.violation.ISNViolation = Convert.ToString(records.isn_dob_bis_viol);
                                    itemSummons.violation.ECBnumber = Convert.ToString(records.ecb_number);
                                    ctx.SaveChanges();

                                    //  if (!string.IsNullOrEmpty(commanMessage) && (PreviousJob == 0 || PreviousJob != itemSummons.IdJob))
                                    if (!string.IsNullOrEmpty(commanMessage))
                                    {
                                        string newstr = StatusFormat.Replace("##Job##", itemSummons != null && itemSummons.job != null ? itemSummons.job.Id.ToString() : string.Empty)
                                                                            .Replace("##JobViolation##", itemSummons != null && itemSummons != null ? itemSummons.violation.SummonsNumber : JobHistoryMessages.NoSetstring)
                                                                            .Replace("##hearingDate##", !string.IsNullOrEmpty(commanMessage) ? commanMessage : JobHistoryMessages.NoSetstring);
                                        ResultYes = ResultYes + newstr + "<br>";

                                        BisScanCount = BisScanCount + 1;

                                        PreviousJob = itemSummons.job.Id;
                                        SendViolationUpdatedSNotification(itemSummons.job.Id, itemSummons.violation.SummonsNumber, commanMessage);
                                    }
                                    else
                                    {
                                        string newstr = StatusNotFormat.Replace("##Job##", itemSummons != null && itemSummons.job != null ? itemSummons.job.Id.ToString() : string.Empty)
                                                                    .Replace("##JobViolation##", itemSummons != null && itemSummons != null ? itemSummons.violation.SummonsNumber : JobHistoryMessages.NoSetstring);
                                        ResultNot = ResultNot + newstr + "<br>";

                                        totalno = totalno + 1;

                                        BisScanNoCount = BisScanNoCount + 1;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    System.Data.SqlClient.SqlException s = e.InnerException.InnerException as System.Data.SqlClient.SqlException;

                }
                catch (System.Data.SqlClient.SqlException exception)
                {
                    for (int i = 0; i < exception.Errors.Count; i++)
                    {
                        sb.Append("Index #" + i + "\n" +
                            "Error: " + exception.Errors[i].ToString() + "\n");
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Empty;
                    ErrorCount = ErrorCount + 1;

                    string newstr = ErrorRecNotFormat.Replace("##Job##", itemSummons != null && itemSummons.job != null ? itemSummons.job.Id.ToString() : string.Empty)
                                                  .Replace("##JobViolation##", itemSummons != null && itemSummons != null ? itemSummons.violation.SummonsNumber : JobHistoryMessages.NoSetstring);
                    ErrorRec = ErrorRec + newstr + "<br>";

                    message = ex.Message;
                    string innerExceptionmessage = string.Empty;

                    if (ex.InnerException != null)
                    {
                        innerExceptionmessage = ex.InnerException.ToString();
                    }

                    sb.Append(Environment.NewLine + "------------------------------------------------------------");
                    sb.Append(Environment.NewLine + "Violation No ---\n{0}" + itemSummons.violation.SummonsNumber);
                    sb.Append(Environment.NewLine + "ErrorMessage ---\n{0}" + message);
                    sb.Append(Environment.NewLine + "InnerExceptionMessage ---\n{0}" + innerExceptionmessage);
                    sb.Append(Environment.NewLine + "------------------------------------------------------------;");
                    sb.Append(Environment.NewLine + "Source ---\n{0}" + ex.Source);
                    sb.Append(Environment.NewLine + "StackTrace ---\n{0}" + ex.StackTrace);
                    sb.Append(Environment.NewLine + "TargetSite ---\n{0}" + ex.TargetSite);
                    sb.Append(Environment.NewLine + "------------------------------------------------------------;");
                }
            }//ForEach Loop Over

            emailBody = emailBody.Replace("##OATHApicount##", OATHYesScanCount.ToString());
            emailBody = emailBody.Replace("##OATHResponsecount##", OATHNoScanCount.ToString());
            emailBody = emailBody.Replace("##ECBApicount##", ECBYesScanCount.ToString());
            emailBody = emailBody.Replace("##ECBResponsecount##", ECBNoScanCount.ToString());

            emailBody = emailBody.Replace("##OATHJobViolationStatusNoList##", OATHNoScanRec.ToString());
            emailBody = emailBody.Replace("##ECBJobViolationStatusNoList##", ECBNoScanRec.ToString());

            emailBody = emailBody.Replace("##DBUpdatesNocount##", BisScanNoCount.ToString());
            emailBody = emailBody.Replace("##DBUpdatesYescount##", BisScanCount.ToString());

            emailBody = emailBody.Replace("##NotDBJobViolationStatusNoList##", ResultNot.ToString());
            emailBody = emailBody.Replace("##DBJobViolationStatusYesList##", ResultYes.ToString());

            emailBody = emailBody.Replace("##ErrorCount##", ErrorCount.ToString());
            emailBody = emailBody.Replace("##ErrorJobViolationList##", ErrorRec.ToString());

            var EndtimeUtc = DateTime.UtcNow;
            TimeZoneInfo EndeasternZone = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);
            DateTime EndeasternTime = TimeZoneInfo.ConvertTimeFromUtc(EndtimeUtc, easternZone);

            emailBody = emailBody.Replace("##ExecuteEndTime##", EndeasternTime.ToString("hh:mm tt"));


            TimeSpan difference = EndeasternTime - easternTime;

            int hours = difference.Hours;
            int minutes = difference.Minutes;

            emailBody = emailBody.Replace("##TotalHours##", hours.ToString());
            emailBody = emailBody.Replace("##TotalMinutes##", minutes.ToString());

            string commonsub = string.Empty;
            if (Properties.Settings.Default.IsSnapcor == "Yes")
            {
                commonsub = "[Snapcor] ";
            }
            if (Properties.Settings.Default.IsUAT == "Yes")
            {
                commonsub = "[UAT] ";
            }

            List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
            SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenHearingDateIsUpdatedBySystem);
            if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
            {
                foreach (var item in systemSettingDetail.Value)
                {
                    if (!string.IsNullOrEmpty(item.Email))
                    {
                        to.Add(new KeyValuePair<string, string>(item.Email, item.FirstName + " " + item.LastName));
                    }
                }
            }
            List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
            if (to != null && to.Count > 0)
            {
                Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "DOB Violations Updated by CronJob", emailBody, true);
            }
            // ctx.SaveChanges();

            List<KeyValuePair<string, string>> errorto = new List<KeyValuePair<string, string>>();
            string[] developeemail = Properties.Settings.Default.CronjobUpdateDeveloperEmail.Split(';');
            foreach (string itm in developeemail)
            {
                if (!string.IsNullOrEmpty(itm))
                {
                    errorto.Add(new KeyValuePair<string, string>(itm, "RPO APP"));
                }
            }

            if (sb.Length > 0)
            {
                Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), errorto, cc, commonsub + "DOB Violations Updated by CronJob error", "Error Message is : " + sb.ToString() + "", true);
            }
            ApplicationLog.WriteInformationLog("End DOB Update Violation cronjob");
            ApplicationLog.WriteInformationLog("EndTime : " + DateTime.UtcNow.ToString());

            WriteLogWebclient("End DOB Update Violation cronjob");
            WriteLogWebclient("EndTime : " + DateTime.UtcNow.ToString());

        }

        public static void SendViolationUpdatedSNotification(int? id, string violationNo, string commonmessage)
        {
            var rpoContext = new Model.RpoContext();
            Job jobs = rpoContext.Jobs.Include("RfpAddress").Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == id).FirstOrDefault();

            List<int> jobAssignList = new List<int>();


            if (jobs.IdProjectManager != null && jobs.IdProjectManager > 0)
            {
                int idProjectManager = Convert.ToInt32(jobs.IdProjectManager);
                jobAssignList.Add(idProjectManager);
            }

            if (jobs.DOBProjectTeam != null && !string.IsNullOrEmpty(jobs.DOBProjectTeam))
            {
                foreach (var item in jobs.DOBProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(employeeid);
                }
            }

            if (jobs.DOTProjectTeam != null && !string.IsNullOrEmpty(jobs.DOTProjectTeam))
            {
                foreach (var item in jobs.DOTProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(employeeid);
                }
            }

            if (jobs.DEPProjectTeam != null && !string.IsNullOrEmpty(jobs.DEPProjectTeam))
            {
                foreach (var item in jobs.DEPProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(employeeid);
                }
            }

            if (jobs.ViolationProjectTeam != null && !string.IsNullOrEmpty(jobs.ViolationProjectTeam))
            {
                foreach (var item in jobs.ViolationProjectTeam.Split(','))
                {
                    int employeeid = Convert.ToInt32(item);
                    jobAssignList.Add(employeeid);
                }
            }

            var employeelist = jobAssignList.Distinct();

            foreach (var item in employeelist)
            {
                int employeeid = Convert.ToInt32(item);
                Employee employee = rpoContext.Employees.FirstOrDefault(x => x.Id == employeeid);

                string newJobScopeAddedSetting = InAppNotificationMessage.UpdateViolation_Cronjob
                                    .Replace("##jobnumber##", jobs != null ? jobs.Id.ToString() : string.Empty)
                                    .Replace("##jobaddress##", jobs != null && jobs.RfpAddress != null ? (!string.IsNullOrEmpty(jobs.RfpAddress.HouseNumber) ? jobs.RfpAddress.HouseNumber : string.Empty) + " " + (!string.IsNullOrEmpty(jobs.RfpAddress.Street) ? jobs.RfpAddress.Street : string.Empty) + " " + (jobs.RfpAddress.Borough != null ? jobs.RfpAddress.Borough.Description : JobHistoryMessages.NoSetstring) + " " + (!string.IsNullOrEmpty(jobs.RfpAddress.ZipCode) ? jobs.RfpAddress.ZipCode : string.Empty) + " " + (!string.IsNullOrEmpty(jobs.SpecialPlace) ? "-" + jobs.SpecialPlace : string.Empty) : JobHistoryMessages.NoSetstring)
                                    .Replace("##ViolationNumber##", violationNo)
                                    .Replace("##CommanMessage##", commonmessage);

                Common.SendInAppNotifications(employee.Id, newJobScopeAddedSetting, "/job/" + jobs.Id + "/scope");

            }
        }
        public static bool NotEqualDate(DateTime? dt1, DateTime? dt2)
        {
            return dt1.Value.Year != dt2.Value.Year || dt1.Value.Month != dt2.Value.Month || dt1.Value.Day != dt2.Value.Day;
        }

        public class ViolationDOBJsonRes
        {
            public string issue_date { get; set; }
            public string violation_type { get; set; }
            public string violation_category { get; set; }
            public string violation_type_code { get; set; }
            public string device_number { get; set; }
            public string isn_dob_bis_viol { get; set; }
            public string house_number { get; set; }
            public string street { get; set; }
            public string disposition_date { get; set; }
            public string disposition_comments { get; set; }
            public string number { get; set; }
            public string bin { get; set; }
            public string description { get; set; }
            public string ecb_number { get; set; }
        }

        public static void WriteLogWebclient(string message)
        {
            string errorLogFilename = "DOBUpdateViolationLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

            string directory = AppDomain.CurrentDomain.BaseDirectory + "Log";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = AppDomain.CurrentDomain.BaseDirectory + "Log/" + errorLogFilename;

            if (File.Exists(path))
            {
                using (StreamWriter stwriter = new StreamWriter(path, true))
                {
                    stwriter.WriteLine("-------------------Web Client Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                    stwriter.WriteLine("Message: " + message);
                    stwriter.WriteLine("-------------------End----------------------------");
                    stwriter.Close();
                }
            }
            else
            {
                StreamWriter stwriter = File.CreateText(path);
                stwriter.WriteLine("-------------------Web Client Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                stwriter.WriteLine("Message: " + message);
                stwriter.WriteLine("-------------------End----------------------------");
                stwriter.Close();
            }


            //var attachments = new List<string>();
            //if (File.Exists(path))
            //{
            //    attachments.Add(path);
            //}

            //var to = new List<KeyValuePair<string, string>>();

            //to.Add(new KeyValuePair<string, string>("vipul.patel@credencys.com", "Vipul Patel"));

            //string body = string.Empty;
            //using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
            //{
            //    body = reader.ReadToEnd();
            //}

            //var cc = new List<KeyValuePair<string, string>>();

            //string emailBody = body;
            //emailBody = emailBody.Replace("##EmailBody##", "Please correct the issue.");

            //Mail.Send(
            //           new KeyValuePair<string, string>("noreply@rpoinc.com", "RPO Log"),
            //           to,
            //           cc,
            //           "[Stagin]-Violation Cronjob Log",
            //           emailBody,
            //           attachments
            //       );
        }
    }
}




