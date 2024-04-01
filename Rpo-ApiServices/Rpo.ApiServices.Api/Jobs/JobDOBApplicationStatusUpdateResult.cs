
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
    using SODA;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Web;
    using System.Text;
    using System.Net;

    public class JobDOBApplicationStatusUpdateResult : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }

        //public JobDOBApplicationStatusUpdateResult()
        //{
        //    SendMailNotification();
        //}

        public static void SendMailNotification()
        {
            ApplicationLog.WriteInformationLog("Job Application Status Update Result Send Mail Notification executed: " + DateTime.Now.ToLongDateString());
            ApplicationLog.WriteInformationLog("StartTime : " + DateTime.UtcNow.ToString());
            WriteLogWebclientapplication("start Job Application Status cronjob");
            WriteLogWebclientapplication("StartTime : " + DateTime.UtcNow.ToString());

            string body = string.Empty;
            string emailtemplatepath = AppDomain.CurrentDomain.BaseDirectory + "EmailTemplate/DOBApplicationStatusapiTemplate.htm";
            using (StreamReader reader = new StreamReader(emailtemplatepath))
            {
                body = reader.ReadToEnd();
            }
            string emailBody = body;
            StringBuilder sb = new StringBuilder();
            // emailBody = emailBody.Replace("##EmailBody##", Attentation + newJobScopeAddedSetting);
            using (var ctx = new Model.RpoContext())
            {
                int[] jobtypearry = { 5, 6, 7, 8, 10, 11, 13, 26 };

               //  int[] strjoblist = { 7637};

               // List<JobApplication> lstJobApplication = ctx.JobApplications.Include("JobApplicationType").Include("Job").Include("Job.RfpAddress").Include("Job.RfpAddress.Borough").Where(x => strjoblist.Contains(x.IdJob) &&  x.JobApplicationType.IdParent == 1 && !string.IsNullOrEmpty(x.ApplicationNumber) && jobtypearry.Contains(x.JobApplicationType.Id) && x.Job.Status == JobStatus.Active).ToList();
                List<JobApplication> lstJobApplication = ctx.JobApplications.Include("JobApplicationType").Include("Job").Include("Job.RfpAddress").Include("Job.RfpAddress.Borough").Where(x =>x.JobApplicationType.IdParent == 1 && !string.IsNullOrEmpty(x.ApplicationNumber) && jobtypearry.Contains(x.JobApplicationType.Id) && x.Job.Status == JobStatus.Active).ToList();

                int totaljobs = lstJobApplication.Select(d => d.IdJob).Distinct().Count();

                var timeUtc = DateTime.UtcNow;
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);
                DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

                emailBody = emailBody.Replace("##ExecuteDate##", easternTime.ToString("MM/dd/yyyy"));
                emailBody = emailBody.Replace("##ExecuteStartTime##", easternTime.ToString("hh:mm tt"));

                WriteLogWebclientapplication("Active jobs Application count :" + totaljobs.ToString());
                emailBody = emailBody.Replace("##Activejobs##", totaljobs.ToString());
                emailBody = emailBody.Replace("##TotalApplications##", lstJobApplication.Count.ToString());

                string ResultNot = string.Empty;
                string ResultNotAPI = string.Empty;
                string ResultYes = string.Empty;
                string StatusNotFormat = "Job#<##Job##> - Application# ##Application##";

                string StatusFormat = "Job#<##Job##> - Application# ##Application## - Old Status: ##OldStatus## - New Status: ##NewStatus##";

                int ErrorCount = 0;
                string ErrorRec = string.Empty;
                string ErrorRecNotFormat = "Job# ##Job## - Application# ##Application##";


                int totalno = 0;
                int DBStatusYesCount = 0;
                int DBStatusNoCount = 0;
                int DobStatusApiResultCount = 0;
                int DobStatusApiResultNotCount = 0;
                string DobStatusApiResultCountNo = string.Empty;

                foreach (var jobApplication in lstJobApplication)
                {
                    int jobNumber = jobApplication.IdJob;
                    string applicationNumber = jobApplication.ApplicationNumber;

                    ApplicationLog.WriteInformationLog("Job# : " + jobNumber + " ");
                    ApplicationLog.WriteInformationLog("ApplicationNumber# : " + applicationNumber + " ");
                    WriteLogWebclientapplication("Job# : " + jobNumber + " ");
                    WriteLogWebclientapplication("ApplicationNumber# : " + applicationNumber + " ");
                    try
                    {
                        //-----------------Fetch the data from Web Site-----------------//
                        if (!string.IsNullOrEmpty(applicationNumber))
                        {
                            string qry = string.Empty;
                            // string strapplicationStatusinBIS = string.Empty;

                            JobApplication jobApplicationobj = null;
                            jobApplicationobj = jobApplication;

                            Tools.ClsApplicationStatus objapp = new Tools.ClsApplicationStatus();

                            var client = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
                            var dataset = client.GetResource<object>("ic3t-wcy2");

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                               | SecurityProtocolType.Tls11
                                               | SecurityProtocolType.Tls12;

                            var rows = dataset.GetRows(limit: 1000);

                            qry = qry + "job__='" + jobApplication.ApplicationNumber + "' AND doc__='01'";

                            var soql = new SoqlQuery().Select("job__", "doc__", "job_status", "job_status_descrp", "latest_action_date").Where(qry);

                            var results = dataset.Query(soql);
                            DOBApplicationJsonResList objressult = null;
                         
                            if (results != null && results.Count() > 0)
                            {
                                DobStatusApiResultCount = DobStatusApiResultCount + 1;
                                foreach (var item in results)
                                {
                                    string jsonstring = JsonConvert.SerializeObject(item);
                                    var records = JsonConvert.DeserializeObject<DOBApplicationJsonRes>(jsonstring);
                                    string oldStatus = string.Empty;
                                    oldStatus = jobApplication.JobApplicationStatus;

                                    string Applicationstatus = records.job_status_descrp + " " + (records.latest_action_date != null ? records.latest_action_date.ToShortDateString() : string.Empty) + (!string.IsNullOrEmpty(records.job_status) ? " (" + records.job_status + ")" : string.Empty);
                                   

                                    WriteLogWebclientapplication("Old Applicationstatus is : " + oldStatus);
                                    WriteLogWebclientapplication("New Applicationstatus is : " + Applicationstatus);

                                    if (records != null && !string.IsNullOrEmpty(records.job_status_descrp) && oldStatus != Applicationstatus)
                                    {
                                        //string Applicationstatus = records.job_status_descrp + " " + (records.latest_action_date != null ? records.latest_action_date.ToShortDateString() : string.Empty) + (!string.IsNullOrEmpty(records.job_status) ? "(" + records.job_status + ")" : string.Empty);
                                        jobApplication.JobApplicationStatus = Applicationstatus;
                                        if (Applicationstatus.ToLower().Contains("signed off") || Applicationstatus.ToLower().Contains("loc issued"))
                                        { jobApplication.SignOff = true; }                                        
                                        jobApplication.LastModifiedBy = 2;
                                        jobApplication.LastModifiedDate = DateTime.UtcNow;

                                        jobApplication.Job.LastModiefiedDate = DateTime.UtcNow;

                                        string addApplication_DOB = JobHistoryMessages.UpdateApplication_DOBCronjob
                                                                    .Replace("##jobnumber##", jobApplication != null ? jobApplication.IdJob.ToString() : string.Empty)
                                                                    .Replace("##jobaddress##", jobApplication != null && jobApplication.Job.RfpAddress != null ? (!string.IsNullOrEmpty(jobApplication.Job.RfpAddress.HouseNumber) ? jobApplication.Job.RfpAddress.HouseNumber : string.Empty) + " " + (!string.IsNullOrEmpty(jobApplication.Job.RfpAddress.Street) ? jobApplication.Job.RfpAddress.Street : string.Empty) + " " + (jobApplication.Job.RfpAddress.Borough != null ? jobApplication.Job.RfpAddress.Borough.Description : JobHistoryMessages.NoSetstring) + " " + (!string.IsNullOrEmpty(jobApplication.Job.RfpAddress.ZipCode) ? jobApplication.Job.RfpAddress.ZipCode : string.Empty) + " " + (!string.IsNullOrEmpty(jobApplication.Job.SpecialPlace) ? "-" + jobApplication.Job.SpecialPlace : string.Empty) : JobHistoryMessages.NoSetstring)
                                                                    .Replace("##ApplicationType##", jobApplication != null && jobApplication.JobApplicationType != null && !string.IsNullOrEmpty(jobApplication.JobApplicationType.Description) ? jobApplication.JobApplicationType.Description : JobHistoryMessages.NoSetstring)
                                                                    .Replace("##ApplicationNumber##", jobApplication != null ? applicationNumber : JobHistoryMessages.NoSetstring)
                                                                    .Replace("##oldstatus##", !string.IsNullOrEmpty(oldStatus) ? oldStatus : JobHistoryMessages.NoSetstring)
                                                                    .Replace("##newstatus##", Applicationstatus);
                                        Common.SaveJobHistory(0, jobApplication.IdJob, addApplication_DOB, JobHistoryType.Applications);
                                        ctx.SaveChanges();
                                        SendNotificationToJObDOBTeam(jobApplication.IdJob, addApplication_DOB);

                               

                                        string newstr = StatusFormat.Replace("##Job##", jobApplication != null ? jobApplication.IdJob.ToString() : string.Empty)
                                                                      .Replace("##Application##", jobApplication != null ? applicationNumber : JobHistoryMessages.NoSetstring)
                                                                      .Replace("##OldStatus##", !string.IsNullOrEmpty(oldStatus) ? oldStatus : JobHistoryMessages.NoSetstring)
                                                                    .Replace("##NewStatus##", Applicationstatus);
                                        ResultYes = ResultYes + newstr + "<br>";
                                        DBStatusYesCount = DBStatusYesCount + 1;
                                    }
                                    else
                                    {
                                        totalno = totalno + 1;
                                        string newstr = StatusNotFormat.Replace("##Job##", jobApplication != null ? jobApplication.IdJob.ToString() : string.Empty)
                                                                       .Replace("##Application##", jobApplication != null ? applicationNumber : JobHistoryMessages.NoSetstring);
                                        ResultNot = ResultNot + newstr + "<br>";
                                        DBStatusNoCount = DBStatusNoCount + 1;
                                    }
                                }
                            }
                            else
                            {
                                totalno = totalno + 1;
                                string newstr = StatusNotFormat.Replace("##Job##", jobApplication != null ? jobApplication.IdJob.ToString() : string.Empty)
                                                               .Replace("##Application##", jobApplication != null ? applicationNumber : JobHistoryMessages.NoSetstring);
                                ResultNotAPI = ResultNotAPI + newstr + "<br>";
                                DobStatusApiResultNotCount = DobStatusApiResultNotCount + 1;
                            }
                            
                        }
                        else
                        {
                            string newstr = StatusNotFormat.Replace("##Job##", jobApplication != null ? jobApplication.IdJob.ToString() : string.Empty)
                                                             .Replace("##Application##", "Application Number is null JobApplication Id#: "+ jobApplication.Id.ToString() + "");
                            ResultNotAPI = ResultNotAPI + newstr + "<br>";
                            DobStatusApiResultNotCount = DobStatusApiResultNotCount + 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = string.Empty;
                        ErrorCount = ErrorCount + 1;

                        string newstr = StatusNotFormat.Replace("##Job##", jobApplication != null ? jobApplication.IdJob.ToString() : string.Empty)
                                                                .Replace("##Application##", jobApplication != null ? applicationNumber : JobHistoryMessages.NoSetstring);
                        ErrorRec = ErrorRec + newstr + "<br>";

                        message = ex.Message;
                        string innerExceptionmessage = string.Empty;

                        if (ex.InnerException != null)
                        {
                            innerExceptionmessage = ex.InnerException.ToString();
                        }

                        sb.Append(Environment.NewLine + "------------------------------------------------------------");
                        sb.Append(Environment.NewLine + "Application No ---\n{0}" + applicationNumber);
                        sb.Append(Environment.NewLine + "ErrorMessage ---\n{0}" + message);
                        sb.Append(Environment.NewLine + "InnerExceptionMessage ---\n{0}" + innerExceptionmessage);
                        sb.Append(Environment.NewLine + "------------------------------------------------------------;");
                        sb.Append(Environment.NewLine + "Source ---\n{0}" + ex.Source);
                        sb.Append(Environment.NewLine + "StackTrace ---\n{0}" + ex.StackTrace);
                        sb.Append(Environment.NewLine + "TargetSite ---\n{0}" + ex.TargetSite);
                        sb.Append(Environment.NewLine + "------------------------------------------------------------;");
                    }
                    //------------------End Fetch the data from Web Site-----------//
                }//ForEach Loop Over

                ctx.SaveChanges();

                var EndtimeUtc = DateTime.UtcNow;
                TimeZoneInfo EndeasternZone = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);
                DateTime EndeasternTime = TimeZoneInfo.ConvertTimeFromUtc(EndtimeUtc, easternZone);

                emailBody = emailBody.Replace("##TotalFoundFromAPicount##", DobStatusApiResultCount.ToString());
                emailBody = emailBody.Replace("##BISScanApplicationsNotfound##", DobStatusApiResultNotCount.ToString());
                emailBody = emailBody.Replace("##BISScanApplicationsNotcount##", DBStatusNoCount.ToString());
                emailBody = emailBody.Replace("##ApplicationsNotFoundList##", ResultNotAPI.ToString());
                

                emailBody = emailBody.Replace("##BISScanApplicationscount##", DBStatusYesCount.ToString());
                emailBody = emailBody.Replace("##JobApplicationStatusList##", ResultYes.ToString());
                emailBody = emailBody.Replace("##JobApplicationStatusNotFoundList##", ResultNot.ToString());

                emailBody = emailBody.Replace("##ErrorCount##", ErrorCount.ToString());
                emailBody = emailBody.Replace("##ErrorJobApplicationList##", ErrorRec.ToString());

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
                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenApplicationStatusArePulledAutomaticallyBySystem);
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
                    Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub+"DOB Application Status Updated by CronJob", emailBody, true);
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
                    Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), errorto, cc, commonsub + "DOB Application Status Updated by CronJob error", "Error Message is : " + sb.ToString() + "", true);
                }


                ApplicationLog.WriteInformationLog("End DOB Application Status cronjob");
                ApplicationLog.WriteInformationLog("EndTime : " + DateTime.UtcNow.ToString());

                WriteLogWebclientapplication(" End DOB Application Status cronjob");
                WriteLogWebclientapplication("EndTime : " + DateTime.UtcNow.ToString());
            }
        }

        private static void SendNotificationToJObDOBTeam(int jobId, string message)
        {
            JobsController jobObj = new JobsController();
            jobObj.SendCronJobAssignMail(jobId, message);
        }

        public static void SendViolationUpdatedSNotification(int id, string violationNo, string PreviousHearingDate, String NewHearingDate, bool isSummonsStatusChanged)
        {
            var rpoContext = new Model.RpoContext();
            Job jobs = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == id).FirstOrDefault();

            List<int> jobAssignList = new List<int>();

            if (!isSummonsStatusChanged)
            {
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
                SystemSettingDetail systemSettingDetail;
                if (!isSummonsStatusChanged)
                {
                    systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenHearingDateIsUpdatedBySystem);

                    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                    {
                        foreach (Controllers.Employees.EmployeeDetail employeeDetail in systemSettingDetail.Value)
                        {
                            if (employeeDetail.Email != employee.Email)
                            {
                                string newJobScopeAddedSetting = InAppNotificationMessage.HearingDateIsUpdatedBySystem
                                    .Replace("##Job##", jobs.JobNumber)
                                    .Replace("##ViolationNumber##", violationNo)
                                    .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                    .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                    .Replace("##PreviousHearingDate##", PreviousHearingDate)
                                    .Replace("##NewHearingDate##", NewHearingDate)
                                    .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/violation");


                                Common.SendInAppNotifications(employeeDetail.Id, newJobScopeAddedSetting, "/job/" + jobs.Id + "/scope");
                            }
                        }
                    }
                }
                else
                {
                    systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenStatusOfSummonsIsUpdatedBySystem);

                    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                    {
                        foreach (Controllers.Employees.EmployeeDetail employeeDetail in systemSettingDetail.Value)
                        {
                            if (employeeDetail.Email != employee.Email)
                            {
                                string newJobScopeAddedSetting = InAppNotificationMessage.StatusOfSummonsIsUpdatedBySystem
                                    .Replace("##Job##", jobs.JobNumber)
                                    .Replace("##ViolationNumber##", violationNo)
                                    .Replace("##HouseStreetNameBorrough##", jobs.RfpAddress != null ? jobs.RfpAddress.HouseNumber + " " + jobs.RfpAddress.Street + (jobs.RfpAddress.Borough != null ? " " + jobs.RfpAddress.Borough.Description : string.Empty) : string.Empty)
                                    .Replace("##SpecialPlaceName##", jobs.SpecialPlace != null ? " - " + jobs.SpecialPlace : string.Empty)
                                    .Replace("##PreviousStatus##", PreviousHearingDate)
                                    .Replace("##NewStatus##", NewHearingDate)
                                    .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + jobs.Id + "/violation");

                                Common.SendInAppNotifications(employeeDetail.Id, newJobScopeAddedSetting, "/job/" + jobs.Id + "/scope");
                            }
                        }
                    }
                }
            }
        }

        public static void WriteLogWebclientapplication(string message)
        {
            string errorLogFilename = "DOBApplicationStatus_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

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
            //           "[UAT]-DOB Application Status Cronjob Log",
            //           emailBody,
            //           attachments
            //       );
        }
    }

    public class DOBApplicationJsonRes
    {
        public string job__ { get; set; }
        public string doc__ { get; set; }
        public string job_status { get; set; }
        public string job_status_descrp { get; set; }
        public DateTime latest_action_date { get; set; }
    }
    public class DOBApplicationJsonResList
    {
        public List<DOBApplicationJsonRes> ApplicationList { get; set; }
    }

}