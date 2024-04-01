
using System;

namespace Rpo.ApiServices.Api.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Controllers.SystemSettings;
    using HtmlAgilityPack;
    using Model.Models;
    using Quartz;
    using Tools;
    using Model.Models.Enums;
    using Controllers.Jobs;
    using Controllers;
    using Controllers.JobDocument.Models;
    using System.Windows.Forms;
    using SODA;
    using Newtonsoft.Json;
    using System.IO;
    using System.Web;
    using System.Text;
    using System.Net;

    public class JobDOBApplicationPermitBISModel
    {
        /// <summary>
        /// Gets or sets the type of the Number Doc Type.
        /// </summary>
        /// <value>The type of the Number DocType.</value>
        public string NumberDocType { get; set; }

        /// <summary>
        /// Gets or sets the detail URL.
        /// </summary>
        /// <value>The detail URL.</value>
        public string DetailUrl { get; set; }
        /// <summary>
        /// Gets or sets the History.
        /// </summary>
        /// <value>The History.</value>
        public string History { get; set; }

        /// <summary>
        /// Gets or sets the SeqNo.
        /// </summary>
        /// <value>The SeqNo.</value>
        public string SeqNo { get; set; }

        /// <summary>
        /// Gets or sets the FirstIssueDate.
        /// </summary>
        /// <value>The First Issue Date.</value>
        public string FirstIssueDate { get; set; }

        /// <summary>
        /// Gets or sets the LastIssueDate.
        /// </summary>
        /// <value>The Last Issue Date.</value>
        public string LastIssueDate { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>Status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the statusApplcnt.
        /// </summary>
        /// <value>The Applcnt.</value>
        public string Applcnt { get; set; }

        public string IssueDate { get; set; }

        public string ExpiredDate { get; set; }

        public string NewStatus { get; set; }


    }

    public class JobDOBApplicationPermit
    {
        public string IssueDate { get; set; }

        public string ExpiredDate { get; set; }

        public string Status { get; set; }
    }

    public class JobDOBPermitStatusUpdateResult : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }

        //public JobDOBPermitStatusUpdateResult()
        //{
        //    SendMailNotification();
        //}
        public static void SendMailNotification()
        {
            ApplicationLog.WriteInformationLog("Job DOB Permit Status Send Mail Notification executed: " + DateTime.Now.ToLongDateString());
            ApplicationLog.WriteInformationLog("StartTime : " + DateTime.UtcNow.ToString());
            WriteLogWebclientPermits("start Job Permit Status cronjob");
            WriteLogWebclientPermits("StartTime : " + DateTime.UtcNow.ToString());
            using (var ctx = new Model.RpoContext())
            {
                StringBuilder sb = new StringBuilder();           
                string body = string.Empty;
                string emailtemplatepath = AppDomain.CurrentDomain.BaseDirectory + "EmailTemplate/DOBPermitStatusapiTemplate.htm";
                using (StreamReader reader = new StreamReader(emailtemplatepath))
                {
                    body = reader.ReadToEnd();
                }

                string emailBody = body;

                int[] jobtypearry = { 5, 6, 7, 8, 10, 11, 13, 26 };

                List<JobApplication> lstJobApplication = ctx.JobApplications.Include("JobApplicationType").Include("Job").Include("Job.RfpAddress").Include("Job.RfpAddress.Borough").Where(x => x.JobApplicationType.IdParent == 1 && !string.IsNullOrEmpty(x.ApplicationNumber) && jobtypearry.Contains(x.JobApplicationType.Id) && x.Job.Status == JobStatus.Active).ToList();

                int totaljobs = lstJobApplication.Select(d => d.IdJob).Count();
                var activejobs = lstJobApplication.Select(d => d.IdJob).Distinct().ToList();

                var timeUtc = DateTime.UtcNow;
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);
                DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

                emailBody = emailBody.Replace("##ExecuteDate##", easternTime.ToString("MM/dd/yyyy"));
                emailBody = emailBody.Replace("##ExecuteStartTime##", easternTime.ToString("hh:mm tt"));

                WriteLogWebclientPermits("Active jobs Permit count :" + totaljobs.ToString());
                emailBody = emailBody.Replace("##Activejobs##", totaljobs.ToString());


                var objDOBPullpermit = ctx.DOBPermitMappings.Where(d => activejobs.Contains(d.IdJob)).ToList();
                emailBody = emailBody.Replace("##Totalpermits##", objDOBPullpermit.Count.ToString());

                int totalno = 0;

                int ResultAPIYesCount = 0;
                int ResultAPINoCount = 0;

                string ResultAPINot = string.Empty;
                string ResultAPIYes = string.Empty;

                int DBUpdateYesCount = 0;
                int DBUpdateNoCount = 0;

                string ResultUpdateDBNot = string.Empty;
                string ResultUpdateDBYes = string.Empty;

                int ErrorCount = 0;
                string ErrorRec = string.Empty;
                string ErrorRecNotFormat = "Job# ##Job## - Application# ##Application## Permit# ##Permit## ";

                if (objDOBPullpermit != null && objDOBPullpermit.Count > 0)
                {
                    foreach (var item in objDOBPullpermit)
                    {
                        WriteLogWebclientPermits("Workpermit id : " + item.IdWorkPermit);

                        JobApplicationWorkPermitType jobApplicationWorkPermitType = ctx.JobApplicationWorkPermitTypes
                       .Include("JobApplication.JobApplicationType")
                       .Include("JobApplication.ApplicationStatus")
                       .Include("JobApplication.Job")
                       .Include("JobWorkType")
                       .Include("ContactResponsible")
                       .FirstOrDefault(r => r.Id == item.IdWorkPermit && r.SignedOff == null);
                        try
                        {
                            if (jobApplicationWorkPermitType != null)
                            {
                                string filter = string.Empty;

                                string Permit_Type = string.Empty;
                                string Permit_Sub_Type = string.Empty;
                                if (!string.IsNullOrEmpty(item.NumberDocType))
                                {
                                    string[] strsplit = item.NumberDocType.Split('-');
                                    if (strsplit != null && strsplit.Length > 1)
                                    {

                                    }
                                    if (strsplit != null && strsplit.Length > 2)
                                    {
                                        string[] strspl = strsplit[2].ToString().Split(' ');
                                        if (strspl != null && strspl.Length > 0)
                                        {
                                            Permit_Type = strspl[0].ToString();
                                            filter = filter + "&permit_type='" + Permit_Type + "'";
                                        }
                                        if (strspl != null && strspl.Length > 1)
                                        {
                                            Permit_Sub_Type = strspl[1].ToString();
                                            filter = filter + "&permit_subtype='" + Permit_Sub_Type + "'";
                                        }
                                    }
                                }

                                string qry = string.Empty;
                                var client = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
                                var dataset = client.GetResource<object>("ipu4-2q9a");  //ic3t-wcy2

                                ServicePointManager.Expect100Continue = true;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls12;

                                var rows = dataset.GetRows(limit: 1000);

                                string strseq = string.Empty;
                                int seq = 0;
                                if (!string.IsNullOrEmpty(item.Seq))
                                {
                                    strseq = item.Seq;
                                    seq = int.Parse(item.Seq);
                                }

                                string sequence = strseq.Length > 1 ? seq.ToString() : "0" + seq.ToString();

                                //  qry = qry + "job__='" + jobApplicationWorkPermitType.JobApplication.ApplicationNumber + "'&permit_sequence__='" + sequence + "'" + filter;

                                qry = qry + "job__='" + jobApplicationWorkPermitType.JobApplication.ApplicationNumber + "'" + filter;

                                WriteLogWebclientPermits("Soda API qry : " + qry);

                                var soql = new SoqlQuery().Select("job__", "job_doc___", "permit_status", "permit_type", "permit_subtype", "permit_sequence__", " filing_date", "issuance_date", "expiration_date", " permittee_s_business_name").Where(qry);

                                var results = dataset.Query(soql);
                                DOBApplicationJsonResList objressult = null;

                                WriteLogWebclientPermits("Soda API Result : " + results);
                                WriteLogWebclientPermits("Soda API Result count : " + results.Count());

                                string StatusNotFormat = "Job# ##Job## - Application# ##Application## Permit# ##Permit##";

                                string StatusFormat = "Job# ##Job## - Application# ##Application## - Permit# ##Permit## - ##expiration_date## ";

                                if (results != null && results.Count() > 0)
                                {
                                    int reccount = results.Count();

                                    string resultstring = JsonConvert.SerializeObject(results);

                                    WriteLogWebclientPermits("Permit Json Result : " + resultstring);

                                    int i = 0;
                                    foreach (var itemres in results)
                                    {
                                        int permitstatus = 0;
                                        string jsonstring = JsonConvert.SerializeObject(itemres);
                                        var records = JsonConvert.DeserializeObject<DOBApplicationPermitJsonRes>(jsonstring);
                                        string NumberDocType = string.Empty;
                                        string Updateddates = string.Empty;

                                        if (!string.IsNullOrEmpty(records.permit_sequence__))
                                        {
                                            permitstatus = int.Parse(records.permit_sequence__);
                                        }
                                        WriteLogWebclientPermits("DB sequense Number : " + seq);
                                        WriteLogWebclientPermits("Permit sequense Number : " + permitstatus);

                                        WriteLogWebclientPermits("API Result count : " + reccount);

                                        //   if (seq < permitstatus && permitstatus == reccount)
                                        if (seq+1 == permitstatus)
                                        {
                                            WriteLogWebclientPermits("Call inside the sequence");
                                            NumberDocType = records.job__ + "-" + records.job_doc___ + "-" + records.permit_type + (!string.IsNullOrEmpty(records.permit_subtype) ? " " + records.permit_subtype : string.Empty);

                                            if (records.filing_date != null && !string.IsNullOrEmpty(records.filing_date.ToString("MM/dd/yyyy")) && records.filing_date.ToString("MM/dd/yyyy") != "01/01/0001")
                                            {
                                                Updateddates = Updateddates + (records.filing_date != null ? " Filing Date: " + records.filing_date.ToString("MM/dd/yyyy") : string.Empty);
                                            }
                                            if (records.expiration_date != null && !string.IsNullOrEmpty(records.expiration_date.ToString("MM/dd/yyyy")) && records.expiration_date.ToString("MM/dd/yyyy") != "01/01/0001")
                                            {
                                                Updateddates = Updateddates + (records.expiration_date != null ? " Expiration Date: " + records.expiration_date.ToString("MM/dd/yyyy") : string.Empty);
                                            }
                                            if (records.issuance_date != null && !string.IsNullOrEmpty(records.issuance_date.ToString("MM/dd/yyyy")) && records.issuance_date.ToString("MM/dd/yyyy") != "01/01/0001")
                                            {
                                                Updateddates = Updateddates + (records.issuance_date != null ? " Issue Date: " + records.issuance_date.ToString("MM/dd/yyyy") : string.Empty);
                                            }

                                            string newstr = StatusFormat.Replace("##Job##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.IdJob.ToString() : string.Empty)
                                                                             .Replace("##Application##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
                                                                             .Replace("##Permit##", jobApplicationWorkPermitType != null ? jobApplicationWorkPermitType.Code : JobHistoryMessages.NoSetstring)
                                                                             .Replace("##expiration_date##", Updateddates);
                                            ResultAPIYes = ResultAPIYes + newstr + "<br>";
                                            ResultAPIYesCount = ResultAPIYesCount + 1;

                                            if (!string.IsNullOrEmpty(records.permittee_s_business_name) || records.filing_date != null || records.expiration_date != null || records.issuance_date != null)
                                            {
                                                WriteLogWebclientPermits("Filed: " + records.filing_date.ToString());
                                                WriteLogWebclientPermits("Expires: " + records.expiration_date.ToString());
                                                WriteLogWebclientPermits("Issued: " + records.issuance_date.ToString());

                                                jobApplicationWorkPermitType.Permittee = (!string.IsNullOrEmpty(records.permittee_s_business_name) ? records.permittee_s_business_name : string.Empty);
                                                jobApplicationWorkPermitType.Filed = records.filing_date != null && !string.IsNullOrEmpty(records.filing_date.ToString("MM/dd/yyyy")) && records.filing_date.ToString("MM/dd/yyyy") != "01/01/0001" ? records.filing_date : (DateTime?)null;
                                                jobApplicationWorkPermitType.Expires = records.expiration_date != null && !string.IsNullOrEmpty(records.expiration_date.ToString("MM/dd/yyyy")) && records.expiration_date.ToString("MM/dd/yyyy") != "01/01/0001" ? records.expiration_date : (DateTime?)null;
                                                jobApplicationWorkPermitType.Issued = records.issuance_date != null && !string.IsNullOrEmpty(records.issuance_date.ToString("MM/dd/yyyy")) && records.issuance_date.ToString("MM/dd/yyyy") != "01/01/0001" ? records.issuance_date : (DateTime?)null;

                                                var objprmit = ctx.DOBPermitMappings.Where(d => d.IdJob == jobApplicationWorkPermitType.JobApplication.IdJob && d.IdJobApplication == jobApplicationWorkPermitType.IdJobApplication && d.IdWorkPermit == jobApplicationWorkPermitType.Id).OrderByDescending(d => d.Seq).FirstOrDefault();
                                                if (objprmit != null)
                                                {
                                                    objprmit.IdJob = jobApplicationWorkPermitType.JobApplication.IdJob;
                                                    objprmit.IdJobApplication = jobApplicationWorkPermitType.IdJobApplication;
                                                    objprmit.IdWorkPermit = jobApplicationWorkPermitType.Id;
                                                    objprmit.NumberDocType = NumberDocType;
                                                    objprmit.Seq = records.permit_sequence__;
                                                    objprmit.Permit = records.permittee_s_business_name;
                                                    objprmit.PermitType = Permit_Type;
                                                    objprmit.PermitSubType = Permit_Sub_Type;
                                                    objprmit.EntryDate = DateTime.UtcNow;
                                                }
                                                else
                                                {
                                                    DOBPermitMapping objpermitmap = new DOBPermitMapping();
                                                    objpermitmap.IdJob = jobApplicationWorkPermitType.JobApplication.IdJob;
                                                    objpermitmap.IdJobApplication = jobApplicationWorkPermitType.IdJobApplication;
                                                    objpermitmap.IdWorkPermit = jobApplicationWorkPermitType.Id;
                                                    objpermitmap.NumberDocType = NumberDocType;
                                                    objpermitmap.Seq = records.permit_sequence__;
                                                    objpermitmap.Permit = records.permittee_s_business_name;
                                                    objpermitmap.EntryDate = DateTime.UtcNow;
                                                    objpermitmap.PermitType = Permit_Type;
                                                    objpermitmap.PermitSubType = Permit_Sub_Type;
                                                    ctx.DOBPermitMappings.Add(objpermitmap);
                                                }
                                                string addApplication_DOB_Permit = JobHistoryMessages.UpdateApplication_Permit_DOBCronjob
                                                                    .Replace("##jobnumber##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.IdJob.ToString() : string.Empty)
                                                                    .Replace("##jobaddress##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null && jobApplicationWorkPermitType.JobApplication.Job.RfpAddress != null ? (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.HouseNumber) ? jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.HouseNumber : string.Empty) + " " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.Street) ? jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.Street : string.Empty) + " " + (jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.Borough != null ? jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.Borough.Description : JobHistoryMessages.NoSetstring) + " " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.ZipCode) ? jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.ZipCode : string.Empty) + " " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.Job.SpecialPlace) ? "- " + jobApplicationWorkPermitType.JobApplication.Job.SpecialPlace : string.Empty) : JobHistoryMessages.NoSetstring)
                                                                    .Replace("##ApplicationType##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null && jobApplicationWorkPermitType.JobApplication.JobApplicationType != null && !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.JobApplicationType.Description) ? jobApplicationWorkPermitType.JobApplication.JobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                                    .Replace("##ApplicationNumber##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
                                                                    .Replace("##Permit##", jobApplicationWorkPermitType != null && !string.IsNullOrEmpty(jobApplicationWorkPermitType.Code) ? jobApplicationWorkPermitType.Code : JobHistoryMessages.NoSetstring)
                                                                    .Replace("##Updateddates##", Updateddates);

                                                SendNotificationToJObDOBTeam(jobApplicationWorkPermitType.JobApplication.IdJob, addApplication_DOB_Permit);

                                                ctx.SaveChanges();

                                                string newstryes = StatusFormat.Replace("##Job##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.IdJob.ToString() : string.Empty)
                                                                             .Replace("##Application##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
                                                                             .Replace("##Permit##", jobApplicationWorkPermitType != null ? jobApplicationWorkPermitType.Code : JobHistoryMessages.NoSetstring)
                                                                             .Replace("##expiration_date##", Updateddates);


                                                ResultUpdateDBYes = ResultUpdateDBYes + newstryes + "<br>";
                                                DBUpdateYesCount = DBUpdateYesCount + 1;

                                                //   break;
                                            }
                                            else
                                            {
                                                totalno = totalno + 1;
                                                string newstrold = StatusNotFormat.Replace("##Job##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.IdJob.ToString() : string.Empty)
                                                                               .Replace("##Application##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
                                                                               .Replace("##Permit##", jobApplicationWorkPermitType != null ? jobApplicationWorkPermitType.Code : JobHistoryMessages.NoSetstring);
                                                //ResultAPINot = ResultAPINot + newstr + "<br>";
                                                //ResultAPINoCount = ResultAPINoCount + 1;

                                                ResultUpdateDBNot = ResultUpdateDBNot + newstrold + "<br>";
                                                DBUpdateNoCount = DBUpdateNoCount + 1;
                                            }
                                        }
                                        //else
                                        //{
                                        //    totalno = totalno + 1;
                                        //    string newstr = StatusNotFormat.Replace("##Job##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.IdJob.ToString() : string.Empty)
                                        //                                   .Replace("##Application##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
                                        //                                   .Replace("##Permit##", jobApplicationWorkPermitType != null ? jobApplicationWorkPermitType.Code : JobHistoryMessages.NoSetstring);
                                        //    ResultAPINot = ResultAPINot + newstr + "<br>";
                                        //    ResultAPINoCount = ResultAPINoCount + 1;
                                        //}                                       
                                    }
                                }
                                else
                                {
                                    totalno = totalno + 1;
                                    string newstr = StatusNotFormat.Replace("##Job##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.IdJob.ToString() : string.Empty)
                                                                   .Replace("##Application##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
                                                                   .Replace("##Permit##", jobApplicationWorkPermitType != null ? jobApplicationWorkPermitType.Code : JobHistoryMessages.NoSetstring);
                                    ResultAPINot = ResultAPINot + newstr + "<br>";
                                    ResultAPINoCount = ResultAPINoCount + 1;

                                    // ResultUpdateDBNot = ResultUpdateDBNot + newstr + "<br>";
                                    // DBUpdateNoCount = DBUpdateNoCount + 1;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string message = string.Empty;
                            ErrorCount = ErrorCount + 1;

                            string newstr = ErrorRecNotFormat.Replace("##Job##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.IdJob.ToString() : string.Empty)
                                                                  .Replace("##Application##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
                                                                  .Replace("##Permit##", jobApplicationWorkPermitType != null ? jobApplicationWorkPermitType.Code : JobHistoryMessages.NoSetstring);
                            ErrorRec = ErrorRec + newstr + "<br>";

                            message = ex.Message;
                            string innerExceptionmessage = string.Empty;

                            if (ex.InnerException != null)
                            {
                                innerExceptionmessage = ex.InnerException.ToString();
                            }

                            sb.Append(Environment.NewLine + "------------------------------------------------------------");
                            sb.Append(Environment.NewLine + "Permits Code ---\n{0}" + ErrorRec);
                            sb.Append(Environment.NewLine + "ErrorMessage ---\n{0}" + message);
                            sb.Append(Environment.NewLine + "InnerExceptionMessage ---\n{0}" + innerExceptionmessage);
                            sb.Append(Environment.NewLine + "------------------------------------------------------------;");
                            sb.Append(Environment.NewLine + "Source ---\n{0}" + ex.Source);
                            sb.Append(Environment.NewLine + "StackTrace ---\n{0}" + ex.StackTrace);
                            sb.Append(Environment.NewLine + "TargetSite ---\n{0}" + ex.TargetSite);
                            sb.Append(Environment.NewLine + "------------------------------------------------------------;");
                        }
                    }

                    emailBody = emailBody.Replace("##PermitsFoundcount##", ResultAPIYesCount.ToString());

                    emailBody = emailBody.Replace("##PermitsNotFoundcount##", ResultAPINoCount.ToString());
                    emailBody = emailBody.Replace("##PermitsNotFoundList##", ResultAPINot.ToString());

                    emailBody = emailBody.Replace("##ErrorCount##", ErrorCount.ToString());
                    emailBody = emailBody.Replace("##ErrorpermitsList##", ErrorRec.ToString());


                    emailBody = emailBody.Replace("##PermitsNotUpdateCount##", DBUpdateNoCount.ToString());
                    emailBody = emailBody.Replace("##PermitsStatusNotUpdateList##", ResultUpdateDBNot.ToString());

                    emailBody = emailBody.Replace("##PermitsUpdatedCount##", DBUpdateYesCount.ToString());
                    emailBody = emailBody.Replace("##PermitsStatusUpdatedList##", ResultUpdateDBYes.ToString());

                }

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
                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenPermitsArePulledAutomaticallyBySystem);
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
                    Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "DOB Permit Status Updated by CronJob", emailBody, true);
                }
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
                    Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), errorto, cc, commonsub + "DOB Permit Status Updated by CronJob error", "Error Message is : " + sb.ToString() + "", true);
                }

                ApplicationLog.WriteInformationLog("End DOB Permit Status cronjob");
                ApplicationLog.WriteInformationLog("EndTime : " + DateTime.UtcNow.ToString());

                WriteLogWebclientPermits(" End DOB Permit Status cronjob");
                WriteLogWebclientPermits("EndTime : " + DateTime.UtcNow.ToString());
            }
        }
        public static void WriteLogWebclientPermits(string message)
        {
            string errorLogFilename = "PermitStatus_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

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

        }
        //public static void SendMailNotification()
        //{
        //    ApplicationLog.WriteInformationLog("Job DOB Permit Status Send Mail Notification executed: " + DateTime.Now.ToLongDateString());

        //    using (var ctx = new Model.RpoContext())
        //    {
        //        string body = string.Empty;
        //        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/DOBPermitStatusapiTemplate.htm")))
        //        {
        //            body = reader.ReadToEnd();
        //        }
        //        string emailBody = body;

        //        int[] jobtypearry = { 5, 6, 7, 8, 10, 11, 13, 26 };

        //        List<JobApplication> lstJobApplication = ctx.JobApplications.Include("JobApplicationType").Include("Job").Include("Job.RfpAddress").Include("Job.RfpAddress.Borough").Where(x => x.JobApplicationType.IdParent == 1 && jobtypearry.Contains(x.JobApplicationType.Id) && x.Job.Status == JobStatus.Active).ToList();

        //        int totaljobs = lstJobApplication.Select(d => d.IdJob).Count();
        //        var activejobs = lstJobApplication.Select(d => d.IdJob).Distinct().ToList();

        //        emailBody = emailBody.Replace("##Admin##", "Admin");

        //        emailBody = emailBody.Replace("##ExecuteDateTime##", DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm tt"));
        //        emailBody = emailBody.Replace("##Activejobs##", totaljobs.ToString());
        //        emailBody = emailBody.Replace("##TotalApplications##", lstJobApplication.Count.ToString());

        //        var objDOBPullpermit = ctx.DOBPermitMappings.Where(d=> activejobs.Contains(d.IdJob)).ToList();
        //        int totalno = 0;
        //        string ResultNot = string.Empty;
        //        string ResultYes = string.Empty;
        //        int BisScanCount = 0;
        //        int BisScanNoCount = 0;
        //        if (objDOBPullpermit != null && objDOBPullpermit.Count > 0)
        //        {
        //            foreach (var item in objDOBPullpermit)
        //            {
        //                int seq = 0;
        //                if (!string.IsNullOrEmpty(item.Seq))
        //                {
        //                    seq = int.Parse(item.Seq)+1;
        //                }
        //                string sequence = seq.ToString().Length > 1 ? seq.ToString() : "0" + seq.ToString();

        //                JobApplicationWorkPermitType jobApplicationWorkPermitType = ctx.JobApplicationWorkPermitTypes
        //               .Include("JobApplication.JobApplicationType")
        //               .Include("JobApplication.ApplicationStatus")
        //               .Include("JobApplication.Job")
        //               .Include("JobWorkType")
        //               .Include("ContactResponsible")
        //               .FirstOrDefault(r => r.Id == item.IdWorkPermit);

        //                string filter = string.Empty;

        //                string Permit_Type = string.Empty;
        //                string Permit_Sub_Type = string.Empty;
        //                if (!string.IsNullOrEmpty(item.NumberDocType))
        //                {
        //                    string[] strsplit = item.NumberDocType.Split('-');
        //                    if (strsplit != null && strsplit.Length > 1)
        //                    {

        //                    }
        //                    if (strsplit != null && strsplit.Length > 2)
        //                    {
        //                        string[] strspl = strsplit[2].ToString().Split(' ');
        //                        if (strspl != null && strspl.Length > 0)
        //                        {
        //                            Permit_Type = strspl[0].ToString();
        //                            filter = filter + "&permit_type='" + Permit_Type + "'";
        //                        }
        //                        if (strspl != null && strspl.Length > 1)
        //                        {
        //                            Permit_Sub_Type = strspl[1].ToString();
        //                            filter = filter + "&permit_subtype='" + Permit_Sub_Type + "'";
        //                        }
        //                    }
        //                }

        //                string qry = string.Empty;
        //                var client = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
        //                var dataset = client.GetResource<object>("ipu4-2q9a");  //ic3t-wcy2
        //                   var rows = dataset.GetRows(limit: 1000);

        //                qry = qry + "job__='" + jobApplicationWorkPermitType.JobApplication.ApplicationNumber + "'&permit_sequence__='" + sequence + "'" + filter;

        //                var soql = new SoqlQuery().Select("job__", "job_doc___", "permit_status", "permit_type", "permit_subtype", "permit_sequence__", " filing_date", "issuance_date", "expiration_date", " permittee_s_business_name").Where(qry);

        //                var results = dataset.Query(soql);
        //                DOBApplicationJsonResList objressult = null;


        //                string StatusNotFormat = "Job#<##Job##> - Application#<##Application##> Permit#<##Permit##>";

        //                string StatusFormat = "Job#<##Job##> - Application#<##Application##> - Permit#<##Permit##> - <##expiration_date##> ";


        //                if (results != null && results.Count() > 0)
        //                {
        //                    foreach (var itemres in results)
        //                    {
        //                        string jsonstring = JsonConvert.SerializeObject(itemres);
        //                        var records = JsonConvert.DeserializeObject<DOBApplicationPermitJsonRes>(jsonstring);

        //                        string NumberDocType = records.job__ + "-" + records.job_doc___ + "-" + records.permit_type + (!string.IsNullOrEmpty(records.permit_subtype) ? " " + records.permit_subtype : string.Empty);
        //                        string Updateddates = string.Empty;
        //                        if (records.filing_date != null)
        //                        {
        //                            Updateddates = Updateddates + (records.filing_date != null ? " Filing Date: " + records.filing_date.ToString("MM/dd/yyyy") : string.Empty);
        //                        }
        //                        if (records.expiration_date != null)
        //                        {
        //                            Updateddates = Updateddates + (records.expiration_date != null ? "Expiration Date: " + records.expiration_date.ToString("MM/dd/yyyy") : string.Empty);
        //                        }
        //                        if (records.issuance_date != null)
        //                        {
        //                            Updateddates = Updateddates + (records.issuance_date != null ? " Issue Date: " + records.issuance_date.ToString("MM/dd/yyyy") : string.Empty);
        //                        }

        //                        string newstr = StatusFormat.Replace("##Job##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.IdJob.ToString() : string.Empty)
        //                                                         .Replace("##Application##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
        //                                                          .Replace("##Permit##", jobApplicationWorkPermitType != null ? jobApplicationWorkPermitType.Code : JobHistoryMessages.NoSetstring)
        //                                                         .Replace("##expiration_date##", Updateddates);
        //                        ResultYes = ResultYes + newstr + "<br>";
        //                        BisScanCount = BisScanCount + 1;

        //                        jobApplicationWorkPermitType.Permittee = (!string.IsNullOrEmpty(records.permittee_s_business_name) ? records.permittee_s_business_name : string.Empty);
        //                        jobApplicationWorkPermitType.Filed = records.filing_date != null ? records.filing_date : (DateTime?)null;
        //                        jobApplicationWorkPermitType.Expires = records.expiration_date != null ? records.expiration_date : (DateTime?)null;
        //                        jobApplicationWorkPermitType.Issued = records.issuance_date != null ? records.issuance_date : (DateTime?)null;

        //                        var objprmit = ctx.DOBPermitMappings.Where(d => d.IdJob == jobApplicationWorkPermitType.JobApplication.IdJob && d.IdJobApplication == jobApplicationWorkPermitType.IdJobApplication && d.IdWorkPermit == jobApplicationWorkPermitType.Id).OrderByDescending(d => d.Seq).FirstOrDefault();
        //                        if (objprmit != null)
        //                        {
        //                            objprmit.IdJob = jobApplicationWorkPermitType.JobApplication.IdJob;
        //                            objprmit.IdJobApplication = jobApplicationWorkPermitType.IdJobApplication;
        //                            objprmit.IdWorkPermit = jobApplicationWorkPermitType.Id;
        //                            objprmit.NumberDocType = NumberDocType;
        //                            objprmit.Seq = records.permit_sequence__;
        //                            objprmit.Permit = records.permittee_s_business_name;
        //                            objprmit.PermitType = Permit_Type;
        //                            objprmit.PermitSubType = Permit_Sub_Type;
        //                            objprmit.EntryDate = DateTime.UtcNow;
        //                        }
        //                        else
        //                        {
        //                            DOBPermitMapping objpermitmap = new DOBPermitMapping();
        //                            objpermitmap.IdJob = jobApplicationWorkPermitType.JobApplication.IdJob;
        //                            objpermitmap.IdJobApplication = jobApplicationWorkPermitType.IdJobApplication;
        //                            objpermitmap.IdWorkPermit = jobApplicationWorkPermitType.Id;
        //                            objpermitmap.NumberDocType = NumberDocType;
        //                            objpermitmap.Seq = records.permit_sequence__;
        //                            objpermitmap.Permit = records.permittee_s_business_name;
        //                            objpermitmap.EntryDate = DateTime.UtcNow;
        //                            objpermitmap.PermitType = Permit_Type;
        //                            objpermitmap.PermitSubType = Permit_Sub_Type;
        //                            ctx.DOBPermitMappings.Add(objpermitmap);
        //                        }
        //                        string addApplication_DOB_Permit = JobHistoryMessages.UpdateApplication_Permit_DOBCronjob
        //                                            .Replace("##jobnumber##", jobApplicationWorkPermitType!=null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.IdJob.ToString() : string.Empty)
        //                                            .Replace("##jobaddress##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null && jobApplicationWorkPermitType.JobApplication.Job.RfpAddress != null ? (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.HouseNumber) ? jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.HouseNumber : string.Empty) + " " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.Street) ? jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.Street : string.Empty) + " " + (jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.Borough != null ? jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.Borough.Description : JobHistoryMessages.NoSetstring) + " " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.ZipCode) ? jobApplicationWorkPermitType.JobApplication.Job.RfpAddress.ZipCode : string.Empty) + " " + (!string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.Job.SpecialPlace) ? "-" + jobApplicationWorkPermitType.JobApplication.Job.SpecialPlace : string.Empty) : JobHistoryMessages.NoSetstring)
        //                                            //.Replace("##ApplicationType##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null && jobApplicationWorkPermitType.JobApplication.JobApplicationType != null && !string.IsNullOrEmpty(jobApplicationWorkPermitType.JobApplication.JobApplicationType.Description) ? jobApplicationWorkPermitType.JobApplication.JobApplicationType.Description : JobHistoryMessages.NoSetstring)
        //                                            .Replace("##ApplicationNumber##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
        //                                            .Replace("##Permit##", jobApplicationWorkPermitType != null && !string.IsNullOrEmpty(jobApplicationWorkPermitType.Code) ? jobApplicationWorkPermitType.PermitNumber : JobHistoryMessages.NoSetstring)
        //                                            .Replace("##Updateddates##", Updateddates);

        //                         SendNotificationToJObDOBTeam(jobApplicationWorkPermitType.JobApplication.IdJob, addApplication_DOB_Permit);

        //                        ctx.SaveChanges();
        //                    }
        //                }
        //                else
        //                {
        //                    totalno = totalno + 1;
        //                    string newstr = StatusNotFormat.Replace("##Job##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.IdJob.ToString() : string.Empty)
        //                                                   .Replace("##Application##", jobApplicationWorkPermitType != null && jobApplicationWorkPermitType.JobApplication != null ? jobApplicationWorkPermitType.JobApplication.ApplicationNumber : JobHistoryMessages.NoSetstring)
        //                                                   .Replace("##Permit##", jobApplicationWorkPermitType != null ? jobApplicationWorkPermitType.Code : JobHistoryMessages.NoSetstring);
        //                    ResultNot = ResultNot + newstr + "<br>";
        //                    BisScanNoCount = BisScanNoCount + 1;
        //                }
        //            }

        //            emailBody = emailBody.Replace("##TotalApplicationsNotFound##", totalno.ToString());
        //            emailBody = emailBody.Replace("##ApplicationsNotFound##", ResultNot.ToString());
        //            emailBody = emailBody.Replace("##JobApplicationStatusList##", ResultYes.ToString());
        //            emailBody = emailBody.Replace("##BISScanApplications##", BisScanCount.ToString());

        //            SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenPermitsArePulledAutomaticallyBySystem);
        //            if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
        //            {
        //                foreach (var item in systemSettingDetail.Value)
        //                {

        //                    List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
        //                    // to.Add(new KeyValuePair<string, string>(item.Email, item.FirstName + " " + item.LastName));
        //                    to.Add(new KeyValuePair<string, string>("vipul.patel@credencys.com", "Vipul Patel"));

        //                    List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
        //                    Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, "DOB Application Permit Updated by CronJob", emailBody, true);
        //                }
        //            }

        //        }
        //    }
        //}

        private static void SendNotificationToJObDOBTeam(int jobId, string message)
        {
            JobsController jobObj = new JobsController();
            jobObj.SendCronJobAssignMail(jobId, message);
        }
    }
}

public class DOBApplicationPermitJsonRes
{
    public string job__ { get; set; }
    public string job_doc___ { get; set; }
    public string permit_status { get; set; }
    public string permit_type { get; set; }
    public string permit_subtype { get; set; }
    public string permit_sequence__ { get; set; }
    public DateTime filing_date { get; set; }
    public DateTime issuance_date { get; set; }
    public DateTime expiration_date { get; set; }
    public string permittee_s_business_name { get; set; }

}