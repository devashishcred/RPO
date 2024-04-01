
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
    public class JobViolationUpdateResult : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }
        //public JobViolationUpdateResult()
        //{
        //    SendMailNotification();
        //}

        public static void SendMailNotification()
        {
            ApplicationLog.WriteInformationLog("ECB Violation Update Result Send Mail Notification executed: " + DateTime.Now.ToLongDateString());
            ApplicationLog.WriteInformationLog("StartTime : " + DateTime.UtcNow.ToString());
            WriteLogWebclient("start ECB Update Violation cronjob");
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
                                   where job.Status == JobStatus.Active && v.IsFullyResolved == false && v.Type_ECB_DOB == "ECB"
                                   select v.SummonsNumber).Distinct().ToList();

            WriteLogWebclient("Active ECB Violation IsFullyResolved count :" + objJobViolation.Count());


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
                                   where violation.IsFullyResolved == false && job.Status == JobStatus.Active && violation.SummonsNumber == objSummons.TrimEnd()
                                   && violation.Type_ECB_DOB == "ECB"
                                   select new { violation, job }).OrderByDescending(x => x.violation.Id).FirstOrDefault();
                try
                {
                    if (itemSummons != null)
                    {
                        ApplicationLog.WriteInformationLog("Job# : " + itemSummons != null && itemSummons.job != null ? itemSummons.job.Id.ToString() : string.Empty + " ");
                        ApplicationLog.WriteInformationLog("Violation# : " + itemSummons != null && itemSummons.violation != null ? itemSummons.violation.SummonsNumber.ToString() : string.Empty + " ");
                        WriteLogWebclient("Job# : " + itemSummons.job.Id.ToString() + " ");
                        WriteLogWebclient("Violation# : " + itemSummons.violation.SummonsNumber.ToString() + " ");

                        string actualSummonsNumber = itemSummons.violation.SummonsNumber;
                        string summonsNoticeNumber = "0" + itemSummons.violation.SummonsNumber;

                        string commanMessage = string.Empty;

                        string qry = string.Empty;
                        string qryBIS = string.Empty;

                        Tools.ClsApplicationStatus objapp = new Tools.ClsApplicationStatus();

                        var client = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
                        var dataset = client.GetResource<object>("jz4z-kudi"); //Previous method which is used on api :  gszd-efwt
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                           | SecurityProtocolType.Tls11
                                           | SecurityProtocolType.Tls12;
                        var rows = dataset.GetRows(limit: 1000);

                        qry = qry + "ticket_number='" + summonsNoticeNumber.Trim() + "'";

                        var soql = new SoqlQuery().Select("ticket_number", "hearing_date", "balance_due", "hearing_status", "hearing_result", "issuing_agency", "respondent_first_name", "respondent_last_name", "respondent_address_borough", "respondent_address_house", "respondent_address_street_name", "respondent_address_city", "respondent_address_zip_code", "respondent_address_state_name", "scheduled_hearing_location").Where(qry);

                        var results = dataset.Query(soql);

                        var clientBIS = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
                        var datasetBIS = clientBIS.GetResource<object>("6bgk-3dad");
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                           | SecurityProtocolType.Tls11
                                           | SecurityProtocolType.Tls12;
                        var rowsBIS = datasetBIS.GetRows(limit: 1000);

                        qryBIS = qryBIS + "ecb_violation_number='" + itemSummons.violation.SummonsNumber.Trim() + "'";

                        var soqlBIS = new SoqlQuery().Select("ecb_violation_number", "certification_status", "bin").Where(qryBIS);

                        var resultsBIS = datasetBIS.Query(soqlBIS);

                        string summonsNumber = string.Empty;
                        string dateIssued = string.Empty;
                        string issuingAgency = string.Empty;
                        string respondentName = string.Empty;
                        double balanceDue = 0;
                        string inspectionLocation = string.Empty;
                        string respondentAddress = string.Empty;
                        string statusOfSummonsNotice = string.Empty;
                        string HearingStatus = string.Empty;
                        string hearingResult = string.Empty;
                        string hearingLocation = string.Empty;
                        DateTime? hearingDate = null;
                        string code = string.Empty;
                        string binNumber = string.Empty;
                        string ecb_Certification_status = string.Empty;
                        CultureInfo provider = CultureInfo.InvariantCulture;

                        string StatusNotFormat = "Job# ##Job## - JobViolation# ##JobViolation## ";
                        string StatusFormat = "Job# ##Job## - JobViolation# ##JobViolation## -- ##hearingDate## ";

                        if (results != null && results.Count() > 0)
                        {
                            WriteLogWebclient("OATH API Get Result ");

                            foreach (var item in results)
                            {
                                string jsonstring = JsonConvert.SerializeObject(item);
                                var records = JsonConvert.DeserializeObject<ViolationJsonRes>(jsonstring);

                                if (records.ticket_number != summonsNoticeNumber)
                                {
                                    throw new RpoBusinessException(StaticMessages.ViolationNotFound);
                                }
                                string fullname = records.respondent_first_name + " " + records.respondent_last_name;

                                string fulladdress = records.respondent_address_house + " " + records.respondent_address_street_name + " " + records.respondent_address_city + " " + records.respondent_address_state_name + " " + records.respondent_address_zip_code + " " + records.respondent_address_borough;

                                summonsNumber = records.ticket_number;
                                issuingAgency = records.issuing_agency != null && !string.IsNullOrEmpty(records.issuing_agency) ? records.issuing_agency : string.Empty;
                                respondentName = fullname;
                                balanceDue = records.balance_due != 0 ? records.balance_due : 0;
                                WriteLogWebclient("balanceDue is : " + records.balance_due);
                                inspectionLocation = string.Empty;
                                respondentAddress = fulladdress;
                                hearingResult = records.hearing_result;
                                WriteLogWebclient("hearing_result is : " + records.hearing_result);
                                HearingStatus = records.hearing_status;
                                WriteLogWebclient("hearing_status is : " + records.hearing_status);
                                hearingLocation = string.Empty;
                                if (records.hearing_date != null && !string.IsNullOrEmpty(records.hearing_date))
                                {
                                    WriteLogWebclient("hearing_date is : " + records.hearing_date);
                                    hearingDate = Convert.ToDateTime(records.hearing_date, provider);
                                }

                                code = string.Empty;
                                hearingLocation = records.scheduled_hearing_location;

                            }
                            string newstr = StatusNotFormat.Replace("##Job##", itemSummons != null && itemSummons.job != null ? itemSummons.job.Id.ToString() : string.Empty)
                                                      .Replace("##JobViolation##", itemSummons != null && itemSummons != null ? itemSummons.violation.SummonsNumber : JobHistoryMessages.NoSetstring);
                            OATHYesScanRec = OATHYesScanRec + newstr + "<br>";

                            OATHYesScanCount = OATHYesScanCount + 1;
                        }
                        else
                        {
                            string newstr = StatusNotFormat.Replace("##Job##", itemSummons != null && itemSummons.job != null ? itemSummons.job.Id.ToString() : string.Empty)
                                                    .Replace("##JobViolation##", itemSummons != null && itemSummons != null ? itemSummons.violation.SummonsNumber : JobHistoryMessages.NoSetstring);
                            OATHNoScanRec = OATHNoScanRec + newstr + "<br>";

                            OATHNoScanCount = OATHNoScanCount + 1;
                        }

                       
                        if (resultsBIS != null && resultsBIS.Count() > 0)
                        {
                            foreach (var item in resultsBIS)
                            {
                                string jsonstring = JsonConvert.SerializeObject(item);
                                var records = JsonConvert.DeserializeObject<ViolationJsonRes>(jsonstring);

                                WriteLogWebclient("ecb_Certification_status is : " + records.certification_status);
                                ecb_Certification_status = records.certification_status;
                                binNumber = records.bin;
                                if (hearingDate == null && records.hearing_date != null && !string.IsNullOrEmpty(records.hearing_date))
                                {
                                    hearingDate = Convert.ToDateTime(records.hearing_date, provider);
                                }
                            }
                            ECBYesScanCount = ECBYesScanCount + 1;

                            string newstr = StatusNotFormat.Replace("##Job##", itemSummons != null && itemSummons.job != null ? itemSummons.job.Id.ToString() : string.Empty)
                                                      .Replace("##JobViolation##", itemSummons != null && itemSummons != null ? itemSummons.violation.SummonsNumber : JobHistoryMessages.NoSetstring);
                            ECBYesScanRec = ECBYesScanRec + newstr + "<br>";
                        }
                        else
                        {
                            ECBNoScanCount = ECBNoScanCount + 1;

                            string newstr = StatusNotFormat.Replace("##Job##", itemSummons != null && itemSummons.job != null ? itemSummons.job.Id.ToString() : string.Empty)
                                                    .Replace("##JobViolation##", itemSummons != null && itemSummons != null ? itemSummons.violation.SummonsNumber : JobHistoryMessages.NoSetstring);
                            ECBNoScanRec = ECBNoScanRec + newstr + "<br>";
                        }
                        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                        TextInfo textInfo = cultureInfo.TextInfo;
                        summonsNoticeNumber = actualSummonsNumber;

                        List<ExplanationOfCharge> explanationOfCharges = new List<ExplanationOfCharge>();                        

                        if (hearingDate != null && (itemSummons.violation.HearingDate == null || itemSummons.violation.HearingDate.ToString().Trim() == "" || NotEqualDate(hearingDate, itemSummons.violation.HearingDate)))
                        {
                            WriteLogWebclient("Not Equal HearingDate : " + hearingDate);
                            itemSummons.violation.HearingDate = hearingDate;
                            itemSummons.violation.LastModifiedBy = 2;
                            itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                            commanMessage = commanMessage + " " + "Hearing date  was updated to <'" + hearingDate.Value.ToString("MM/dd/yyyy") + "'>.";
                        }
                        if (HearingStatus != null && !string.IsNullOrEmpty(HearingStatus) && (string.IsNullOrEmpty(itemSummons.violation.StatusOfSummonsNotice) || itemSummons.violation.StatusOfSummonsNotice.ToLower().Trim() != HearingStatus.ToLower().Trim()))
                        {
                            WriteLogWebclient("Not Equal HearingStatus : " + HearingStatus);

                            itemSummons.violation.StatusOfSummonsNotice = textInfo.ToTitleCase(HearingStatus);
                            itemSummons.violation.LastModifiedBy = 2;
                            itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                            commanMessage = commanMessage + " " + "Status of Summons was updated to <'" + HearingStatus + "'>.";
                        }
                        if (itemSummons.violation.BalanceDue != balanceDue)
                        {
                            WriteLogWebclient("Not Equal HearingStatus : " + balanceDue.ToString());
                            itemSummons.violation.BalanceDue = balanceDue;
                            itemSummons.violation.LastModifiedBy = 2;
                            itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                            commanMessage = commanMessage + " " + "Balance Due was updated to $<" + balanceDue + ">.";

                        }
                        if (hearingResult != null && !string.IsNullOrEmpty(hearingResult) && (string.IsNullOrEmpty(itemSummons.violation.HearingResult) || itemSummons.violation.HearingResult.ToLower().Trim() != hearingResult.ToLower().Trim()))
                        {
                            WriteLogWebclient("Not Equal hearingResult : " + hearingResult);
                            itemSummons.violation.HearingResult = textInfo.ToTitleCase(hearingResult);
                            itemSummons.violation.LastModifiedBy = 2;
                            itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                            commanMessage = commanMessage + " " + "Hearing Result was updated to <'" + hearingResult + "'>.";

                        }                  
                        if (ecb_Certification_status != null && !string.IsNullOrEmpty(ecb_Certification_status) && (string.IsNullOrEmpty(itemSummons.violation.CertificationStatus) || itemSummons.violation.CertificationStatus.ToLower().Trim() != ecb_Certification_status.ToLower().Trim()))
                        {
                            WriteLogWebclient("Not Equal ecb_Certification_status : " + ecb_Certification_status);
                            commanMessage = commanMessage + " " + "Certification Status was updated to <'" + ecb_Certification_status + "'>.";
                            itemSummons.violation.CertificationStatus = textInfo.ToTitleCase(ecb_Certification_status);
                            itemSummons.violation.LastModifiedBy = 2;
                            itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                        }                    
                        if (binNumber != null && !string.IsNullOrEmpty(binNumber) && (string.IsNullOrEmpty(itemSummons.violation.BinNumber) || itemSummons.violation.BinNumber.ToLower().Trim() != binNumber.ToLower().Trim()))
                        {
                            WriteLogWebclient("Not Equal Bin Number : " + binNumber);
                            commanMessage = commanMessage + " " + "Bin Number was updated to <'" + binNumber + "'>.";
                            itemSummons.violation.BinNumber = binNumber;
                            itemSummons.violation.LastModifiedBy = 2;
                            itemSummons.violation.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.job.LastModiefiedDate = DateTime.UtcNow;
                        }

                        ctx.SaveChanges();
                        //  if (!string.IsNullOrEmpty(commanMessage) && (PreviousJob == 0 || PreviousJob != itemSummons.IdJob))
                        if (!string.IsNullOrEmpty(commanMessage))
                        {
                            string newstr = StatusFormat.Replace("##Job##", itemSummons != null && itemSummons.job != null ? itemSummons.job.Id.ToString() : string.Empty)
                                                                .Replace("##JobViolation##", itemSummons != null && itemSummons.violation != null ? itemSummons.violation.SummonsNumber : JobHistoryMessages.NoSetstring)
                                                                .Replace("##hearingDate##", !string.IsNullOrEmpty(commanMessage) ? commanMessage : JobHistoryMessages.NoSetstring);
                            ResultYes = ResultYes + newstr + "<br>";

                            BisScanCount = BisScanCount + 1;

                            PreviousJob = itemSummons.job.Id;
                            SendViolationUpdatedSNotification(itemSummons.job.Id, itemSummons.violation.SummonsNumber, commanMessage);
                        }
                        else
                        {
                            string newstr = StatusNotFormat.Replace("##Job##", itemSummons != null && itemSummons.job != null ? itemSummons.job.Id.ToString() : string.Empty)
                                                        .Replace("##JobViolation##", itemSummons != null && itemSummons.violation != null ? itemSummons.violation.SummonsNumber : JobHistoryMessages.NoSetstring);
                            ResultNot = ResultNot + newstr + "<br>";

                            totalno = totalno + 1;

                            BisScanNoCount = BisScanNoCount + 1;
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
                Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "ECB Violations Updated by CronJob", emailBody, true);
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
                Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), errorto, cc, commonsub + "ECB Violations Updated by CronJob error", "Error Message is : " + sb.ToString() + "", true);
            }

            ApplicationLog.WriteInformationLog("End ECB Update Violation cronjob");
            ApplicationLog.WriteInformationLog("EndTime : " + DateTime.UtcNow.ToString());

            WriteLogWebclient("End ECB Update Violation cronjob");
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

        public class ViolationJsonRes
        {
            public string ticket_number { get; set; }
            public string hearing_date { get; set; }
            public double balance_due { get; set; }
            public string hearing_status { get; set; }
            public string hearing_result { get; set; }
            public string issuing_agency { get; set; }
            public string respondent_first_name { get; set; }
            public string respondent_last_name { get; set; }
            public string respondent_address_borough { get; set; }
            public string respondent_address_house { get; set; }
            public string respondent_address_street_name { get; set; }
            public string respondent_address_city { get; set; }
            public string respondent_address_zip_code { get; set; }
            public string respondent_address_state_name { get; set; }
            public string scheduled_hearing_location { get; set; }
            public string certification_status { get; set; }
            public string ecb_violation_status { get; set; }
            public string bin { get; set; }
        }


        public static void WriteLogWebclient(string message)
        {
            string errorLogFilename = "ECBUpdateViolationLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

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




