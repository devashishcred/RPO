namespace Rpo.ApiServices.Api.Jobs
{
    using Newtonsoft.Json;
    using Quartz;
    using Rpo.ApiServices.Api.Controllers.JobViolations;
    using Rpo.ApiServices.Api.Controllers.SystemSettings;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using SODA;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Web;


    public class JobDOBViolationUpdateResult : IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }
        //public JobDOBViolationUpdateResult()
        //{
        //    SendMailNotification();
        //}

        public static void SendMailNotification()
        {
            StringBuilder sb = new StringBuilder();
            using (var ctx = new Model.RpoContext())
            {
                string body = string.Empty;
                string emailtemplatepath = AppDomain.CurrentDomain.BaseDirectory + "EmailTemplate/violationMailer.html";
                using (StreamReader reader = new StreamReader(emailtemplatepath))
                {
                    body = reader.ReadToEnd();
                }
                string emailBody = body;
                string NewemailBody = body;
                var query = ctx.Jobs
                  .Join(ctx.RfpAddresses,
                    j => j.IdRfpAddress,
                    r => r.Id,
                    (j, r) => new
                    {
                        Job = j,
                        RfpAddress = r
                    })
                   .Where(x => x.RfpAddress.BinNumber != null && x.RfpAddress.BinNumber != "" && x.Job.Status == JobStatus.Active)
                  //.Where(x => x.RfpAddress.BinNumber == binnumber.Trim() && x.Job.Status == JobStatus.Active)
                  .Select(x => new
                  {
                      x.Job.Id,
                      x.Job.IdRfpAddress,
                      x.RfpAddress.BinNumber
                  }).Distinct()
                  .ToList();
                var objBinList = query.Select(x => x.BinNumber).Distinct().ToList();

                Dictionary<string, bool> lstDOBViolationsResponse = new Dictionary<string, bool>();

                foreach (var objBinNumber in objBinList)
                {
                    var clientBIS = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
                    var datasetBIS = clientBIS.GetResource<object>("3h2n-5cm9");
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                      SecurityProtocolType.Tls11 |
                      SecurityProtocolType.Tls12;
                    var rowsBIS = datasetBIS.GetRows(limit: 1000);
                    string qryBIS = string.Empty;
                    qryBIS = qryBIS + "bin='" + objBinNumber + "'" + " and " + "violation_category  like '" + "%ACTIVE%" + "'";

                    var soqlBIS = new SoqlQuery().Select("*").Where(qryBIS);
                    var resultsBIS = datasetBIS.Query(soqlBIS);
                    string summonsNumber = string.Empty;
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (resultsBIS != null && resultsBIS.Count() > 0)
                    {
                        foreach (var item in resultsBIS)
                        {
                            string jsonstring = JsonConvert.SerializeObject(item);
                            var records = JsonConvert.DeserializeObject<ViolationDOBJsonRes>(jsonstring);
                            if (records.disposition_comments != null && records.disposition_comments.ToLower().Contains("deleted"))
                            {

                            }
                            else
                            {
                                summonsNumber = records.number;
                                JobViolation jobViolation = null;
                                bool isNotExist = false;
                                jobViolation = ctx.JobViolations.FirstOrDefault(x => x.SummonsNumber == summonsNumber);
                                if (jobViolation == null)
                                {
                                    if (records.bin != "1000000" && records.bin != "2000000" && records.bin != "3000000" && records.bin != "4000000" && records.bin != "5000000" && records.bin != "6000000")
                                    {
                                        isNotExist = true;
                                        jobViolation = new JobViolation();
                                        string fulladdress = records.house_number + " " + records.street + " ";
                                        DateTime dateTime1 = DateTime.ParseExact(records.issue_date, "yyyyMMdd", CultureInfo.InvariantCulture);

                                        jobViolation.DateIssued = dateTime1;
                                        jobViolation.violation_type = Convert.ToString(records.violation_type);
                                        jobViolation.violation_category = Convert.ToString(records.violation_category);
                                        jobViolation.violation_type_code = Convert.ToString(records.violation_type_code);
                                        jobViolation.ViolationDescription = Convert.ToString(records.description);
                                        jobViolation.Disposition_Comments = Convert.ToString(records.disposition_comments);
                                        DateTime dateTime3;
                                        if (records.disposition_date != null)
                                        {
                                            dateTime3 = DateTime.ParseExact(records.disposition_date, "yyyyMMdd", CultureInfo.InvariantCulture);
                                            jobViolation.Disposition_Date = dateTime3;
                                        }
                                        jobViolation.device_number = Convert.ToString(records.device_number);
                                        jobViolation.RespondentAddress = fulladdress;
                                        jobViolation.Type_ECB_DOB = "DOB";
                                        jobViolation.SummonsNumber = summonsNumber;
                                        jobViolation.BinNumber = records.bin;
                                        jobViolation.IsManually = false;
                                        jobViolation.IsNewMailsent = false;
                                        jobViolation.CreatedBy = 2;
                                        jobViolation.CreatedDate = DateTime.UtcNow;
                                        jobViolation.PartyResponsible = 3;
                                        jobViolation.Status = 1;
                                        jobViolation.ECBnumber = Convert.ToString(records.ecb_number);
                                        jobViolation.ISNViolation = Convert.ToString(records.isn_dob_bis_viol);
                                        if (isNotExist)
                                        {
                                            ctx.JobViolations.Add(jobViolation);
                                        }

                                        ctx.SaveChanges();
                                        #region save this violation in compositeviolation                                                  
                                        var Idrfpaddresses = ctx.RfpAddresses.Where(x => x.BinNumber == objBinNumber).Select(y => y.Id).ToList();
                                        foreach (var r in Idrfpaddresses)
                                        {
                                            var jobs = ctx.Jobs.Where(x => x.IdRfpAddress == r && x.Status == JobStatus.Active).Select(y => y.Id).ToList();
                                            foreach (var j in jobs)
                                            {
                                                var compositechecklists = ctx.CompositeChecklists.Where(x => x.ParentJobId == j).Select(y => y.Id).ToList();
                                                foreach (var c in compositechecklists)
                                                {
                                                    CompositeViolations compositeViolations = new CompositeViolations();
                                                    compositeViolations.IdJobViolations = jobViolation.Id;
                                                    compositeViolations.IdCompositeChecklist = c;
                                                    ctx.CompositeViolations.Add(compositeViolations);
                                                    ctx.SaveChanges();
                                                }
                                            }
                                        }
                                        #endregion
                                        lstDOBViolationsResponse.Add(summonsNumber, isNotExist);

                                        List<int> jobNumbers = ctx.Jobs.Where(job => ctx.RfpAddresses
                                                   .Where(address => address.BinNumber == objBinNumber)
                                                   .Select(address => address.Id)
                                                   .Contains(job.IdRfpAddress) && job.Status == JobStatus.Active)
                                                   .Select(x => x.Id)
                                                   .ToList();

                                        SendViolationUpdatedSNotification(jobNumbers, summonsNumber, isNotExist, true, "");
                                    }
                                }
                                else
                                {
                                    string commanMessage = string.Empty;
                                    if (records.violation_type != null && !string.IsNullOrEmpty(records.violation_type) && (string.IsNullOrEmpty(jobViolation.violation_type) || jobViolation.violation_type.ToLower().Trim() != records.violation_type.ToLower().Trim()))
                                    {
                                        commanMessage = commanMessage + " " + "Violation Type was updated to <'" + records.violation_type + "'>.";

                                        jobViolation.violation_type = Convert.ToString(records.violation_type);
                                        jobViolation.LastModifiedBy = 2;
                                        jobViolation.LastModifiedDate = DateTime.UtcNow;
                                        jobViolation.IsUpdateMailsent = false;
                                    }
                                    if (records.bin != null && !string.IsNullOrEmpty(records.bin) && (string.IsNullOrEmpty(jobViolation.BinNumber) || jobViolation.BinNumber.ToLower().Trim() != records.bin.ToLower().Trim()))
                                    {
                                        commanMessage = commanMessage + " " + "Bin Number was updated to <'" + records.violation_type + "'>.";

                                        jobViolation.BinNumber = Convert.ToString(records.bin);
                                        jobViolation.LastModifiedBy = 2;
                                        jobViolation.LastModifiedDate = DateTime.UtcNow;
                                        jobViolation.IsUpdateMailsent = false;
                                    }
                                    if (records.description != null && !string.IsNullOrEmpty(records.description) && (string.IsNullOrEmpty(jobViolation.ViolationDescription) || jobViolation.ViolationDescription.ToLower().Trim() != records.description.ToLower().Trim()))
                                    {
                                        commanMessage = commanMessage + " " + "Violation Description was updated to <'" + records.description + "'>.";
                                        jobViolation.ViolationDescription = Convert.ToString(records.description);
                                        jobViolation.LastModifiedBy = 2;
                                        jobViolation.LastModifiedDate = DateTime.UtcNow;
                                        jobViolation.IsUpdateMailsent = false;
                                    }
                                    DateTime disposition_date;
                                    if (records.disposition_date != null)
                                    {
                                        disposition_date = DateTime.ParseExact(records.disposition_date, "yyyyMMdd", CultureInfo.InvariantCulture);
                                        DateTime disposition_date2 = Convert.ToDateTime(jobViolation.Disposition_Date);
                                        if (disposition_date != null && disposition_date2 != null || disposition_date2 != disposition_date)
                                        {
                                            jobViolation.Disposition_Date = disposition_date;
                                            jobViolation.LastModifiedBy = 2;
                                            jobViolation.LastModifiedDate = DateTime.UtcNow;
                                            jobViolation.IsUpdateMailsent = false;
                                            commanMessage = commanMessage + " " + "Disposition Date was updated to <'" + disposition_date + "'>.";
                                        }
                                    }
                                    if (records.violation_category != null && !string.IsNullOrEmpty(records.violation_category) && (string.IsNullOrEmpty(jobViolation.violation_category) || jobViolation.violation_category.ToLower().Trim() != records.violation_category.ToLower().Trim()))
                                    {
                                        jobViolation.violation_category = Convert.ToString(records.violation_category);
                                        jobViolation.LastModifiedBy = 2;
                                        jobViolation.LastModifiedDate = DateTime.UtcNow;
                                        jobViolation.IsUpdateMailsent = false;
                                        commanMessage = commanMessage + " " + "Violation Category was updated to <'" + Convert.ToString(records.violation_category) + "'>.";
                                    }
                                    if (records.violation_type_code != null && !string.IsNullOrEmpty(records.violation_type_code) && (string.IsNullOrEmpty(jobViolation.violation_type_code) || jobViolation.violation_type_code.ToLower().Trim() != records.violation_type_code.ToLower().Trim()))
                                    {
                                        jobViolation.violation_type_code = Convert.ToString(records.violation_type_code);
                                        jobViolation.LastModifiedBy = 2;
                                        jobViolation.LastModifiedDate = DateTime.UtcNow;
                                        jobViolation.IsUpdateMailsent = false;
                                        commanMessage = commanMessage + " " + "Violation Type Code was updated to <'" + Convert.ToString(records.violation_type_code) + "'>.";

                                    }
                                    if (records.disposition_comments != null && !string.IsNullOrEmpty(records.disposition_comments) && (string.IsNullOrEmpty(jobViolation.Disposition_Comments) || jobViolation.Disposition_Comments.ToLower().Trim() != records.disposition_comments.ToLower().Trim()))
                                    {
                                        jobViolation.Disposition_Comments = Convert.ToString(records.disposition_comments);
                                        jobViolation.LastModifiedBy = 2;
                                        jobViolation.LastModifiedDate = DateTime.UtcNow;
                                        jobViolation.IsUpdateMailsent = false;
                                        commanMessage = commanMessage + " " + "Disposition Comments was updated to <'" + records.disposition_comments + "'>.";

                                    }
                                    if (records.device_number != null && !string.IsNullOrEmpty(records.device_number) && (string.IsNullOrEmpty(jobViolation.device_number) || jobViolation.device_number.ToLower().Trim() != records.device_number.ToLower().Trim()))
                                    {
                                        jobViolation.device_number = Convert.ToString(records.device_number);
                                        jobViolation.LastModifiedBy = 2;
                                        jobViolation.LastModifiedDate = DateTime.UtcNow;
                                        jobViolation.IsUpdateMailsent = false;
                                        commanMessage = commanMessage + " " + "Device Number was updated to <'" + records.device_number + "'>.";
                                    }
                                    jobViolation.ISNViolation = Convert.ToString(records.isn_dob_bis_viol);
                                    ctx.SaveChanges();

                                    if (!string.IsNullOrEmpty(commanMessage))
                                    {
                                        if (jobViolation.PartyResponsible != 3)
                                        {
                                            lstDOBViolationsResponse.Add(summonsNumber, false);

                                            List<int> jobNumbers = ctx.Jobs.Where(job => ctx.RfpAddresses
                                                       .Where(address => address.BinNumber == objBinNumber)
                                                       .Select(address => address.Id)
                                                       .Contains(job.IdRfpAddress) && job.Status == JobStatus.Active)
                                                       .Select(x => x.Id)
                                                       .ToList();

                                            SendViolationUpdatedSNotification(jobNumbers, summonsNumber, false, true, commanMessage);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                string NewDOBViolations = string.Empty;
                string DOBViolations = string.Empty;
                if (lstDOBViolationsResponse.Count > 0)
                {
                    NewDOBViolations = "<p>SnapCor has detected the following violation(s). Click on the link(s) below and navigate to the Violations tab in the Project for further details.</p>";
                    foreach (var item in lstDOBViolationsResponse)
                    {
                        string binNumber = ctx.JobViolations.Where(i => i.SummonsNumber == item.Key).Select(i => i.BinNumber).FirstOrDefault();
                        var address = ctx.RfpAddresses.Where(b => b.BinNumber == binNumber).ToList();
                        var descritption = ctx.JobViolations.Where(x => x.SummonsNumber == item.Key).Select(x => x.ViolationDescription).FirstOrDefault();
                        var issuingagency = ctx.JobViolations.Where(x => x.SummonsNumber == item.Key).Select(x => x.IssuingAgency).FirstOrDefault();
                        var firstaddress = ctx.RfpAddresses.Where(b => b.BinNumber == binNumber).FirstOrDefault();
                        if (item.Value == true)
                        {
                            if (firstaddress != null)
                            {
                                string boroughes = ctx.Boroughes.Where(i => i.Id == firstaddress.IdBorough).Select(i => i.Description).FirstOrDefault();
                                NewDOBViolations += "<p>Violation <b> " + item.Key + "</b> has been issued for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                                + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                                + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b>";
                            }
                            string project = " and is linked to Project # ";
                            List<int> lstjobids = new List<int>();
                            foreach (var a in address)
                            {
                                List<int> jobids = ctx.Jobs.Where(j => j.IdRfpAddress == a.Id && j.Status == JobStatus.Active).Select(i => i.Id).ToList();
                                lstjobids.AddRange(jobids);
                            }
                            foreach (var jobid in lstjobids)
                            {
                                project += "<b><a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobid + "/violation?highlighted=" + item.Key + "&isDob=true" + "\">" + jobid + "</a></b>,";
                            }
                            project = project.Remove(project.Length - 1, 1);
                            NewDOBViolations += project;
                        }
                        else
                        {
                            if (firstaddress != null)
                            {
                                string boroughes = ctx.Boroughes.Where(i => i.Id == firstaddress.IdBorough).Select(i => i.Description).FirstOrDefault();

                                DOBViolations += "<p>Violation <b> " + item.Key + "</b> has been updated for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                              + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                              + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b>";
                            }
                            string project = " is recorded at Project# ";
                            List<int> lstjobids = new List<int>();
                            foreach (var a in address)
                            {
                                List<int> jobids = ctx.Jobs.Where(j => j.IdRfpAddress == a.Id && j.Status == JobStatus.Active).Select(i => i.Id).ToList();
                                lstjobids.AddRange(jobids);
                            }
                            foreach (var jobid in lstjobids)
                            {
                                project += "<b><a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobid + "/violation?highlighted=" + item.Key + "&isDob=true" + "\">" + jobid + "</a></b>, ";
                            }
                            project = project.Remove(project.Length - 1, 1);
                            DOBViolations += project;
                        }

                    }
                }

                emailBody = emailBody.Replace("##ViolationNumber##", DOBViolations);
                NewemailBody = NewemailBody.Replace("##ViolationNumber##", NewDOBViolations);

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
                    if (NewDOBViolations != null && NewDOBViolations != "")
                    {
                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "New DOB Violations has been issued - Action may be required", NewemailBody, true);
                        SendNewMailPersonWise(lstDOBViolationsResponse);
                    }
                    if (DOBViolations != null && DOBViolations != "")
                    {
                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "DOB Violations Updates Received - Action may be required", emailBody, true);
                        SendUpdateMailPersonWise(lstDOBViolationsResponse);
                    }
                }
            }

        }
        public static void SendViolationUpdatedSNotification(List<int> jobNumbers, string violationNo, bool isNotExits, bool ViolationType, string commonmessage)
        {
            string lnkJobNumbers = string.Empty;
            var rpoContext = new Model.RpoContext();
            List<Job> jobs = rpoContext.Jobs
                             .Include("RfpAddress")
                             .Include("RfpAddress.Borough")
                             .Include("Contact")
                             .Include("ProjectManager")
                             .Where(x => jobNumbers.Contains(x.Id))
                             .ToList();

            List<int> jobAssignList = new List<int>();

            foreach (Job job in jobs)
            {
                if (ViolationType)
                    lnkJobNumbers = lnkJobNumbers + (lnkJobNumbers.Length > 0 ? ", " : "") +
                        "<b><a class='dob-job-id' href='" + Properties.Settings.Default.FrontEndUrl + "/job/" + job.Id + "/violation?highlighted=" + violationNo + "&isDob=true" + "'>" + job.Id.ToString() + "</a></b>";
                else
                {
                    lnkJobNumbers = lnkJobNumbers + (lnkJobNumbers.Length > 0 ? ", " : "") +
                          "<b><a class='job-id' href='" + Properties.Settings.Default.FrontEndUrl + "/job/" + job.Id + "/violation?highlighted=" + violationNo + "'>" + job.Id.ToString() + "</a></b>";
                }
                if (job.IdProjectManager != null && job.IdProjectManager > 0)
                {
                    int idProjectManager = Convert.ToInt32(job.IdProjectManager);
                    jobAssignList.Add(idProjectManager);
                }

                if (job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam))
                {
                    foreach (var item in job.DOBProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }

                if (job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam))
                {
                    foreach (var item in job.DOTProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }

                if (job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam))
                {
                    foreach (var item in job.DEPProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }

                if (job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam))
                {
                    foreach (var item in job.ViolationProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }

            }

            var employeelist = jobAssignList.Distinct();

            foreach (var item in employeelist)
            {
                int employeeid = Convert.ToInt32(item);
                Employee employee = rpoContext.Employees.FirstOrDefault(x => x.Id == employeeid);

                string newJobScopeAddedSetting = string.Empty;
                if (isNotExits)
                {
                    newJobScopeAddedSetting = InAppNotificationMessage.Violation_NewCronjob
                                   .Replace("##jobnumbers##", lnkJobNumbers)
                                   .Replace("##ViolationNumber##", "<b class='violation-number'>" + violationNo + "</b>");

                    Common.SendInAppNotifications(employee.Id, newJobScopeAddedSetting, "/job/" + jobNumbers.FirstOrDefault() + "/scope");
                }
                else
                {
                    newJobScopeAddedSetting = InAppNotificationMessage.Violation_UpdateCronjob
                                    .Replace("##jobnumber##", lnkJobNumbers)
                                    .Replace("##jobaddress##", "<b>" + jobs != null && jobs.FirstOrDefault().RfpAddress != null ? (!string.IsNullOrEmpty(jobs.FirstOrDefault().RfpAddress.HouseNumber) ? jobs.FirstOrDefault().RfpAddress.HouseNumber : string.Empty) + " " + (!string.IsNullOrEmpty(jobs.FirstOrDefault().RfpAddress.Street) ? jobs.FirstOrDefault().RfpAddress.Street : string.Empty) + " " + (jobs.FirstOrDefault().RfpAddress.Borough != null ? jobs.FirstOrDefault().RfpAddress.Borough.Description : JobHistoryMessages.NoSetstring) + " " + (!string.IsNullOrEmpty(jobs.FirstOrDefault().RfpAddress.ZipCode) ? jobs.FirstOrDefault().RfpAddress.ZipCode : string.Empty) + " " + (!string.IsNullOrEmpty(jobs.FirstOrDefault().SpecialPlace) ? "-" + jobs.FirstOrDefault().SpecialPlace : string.Empty + "</b>") : JobHistoryMessages.NoSetstring)
                                    .Replace("##ViolationNumber##", "<b class='violation-number'>" + violationNo + "</b>")
                                    .Replace("##CommanMessage##", commonmessage);
                    Common.SendInAppNotifications(employee.Id, newJobScopeAddedSetting, "/job/" + jobNumbers.FirstOrDefault() + "/scope");
                }


            }
            //customer in-app violation notification

            if (isNotExits)
            {
                string linkJobNumbers = string.Empty;
                List<CustomerJobAccess> customerJobAccess = rpoContext.CustomerJobAccess
                                        .Include("Job")
                                        .Include("Customer")
                                        .Where(x => jobNumbers.Contains(x.IdJob)).ToList();

                List<int> customerList = new List<int>();
                foreach (var c in customerJobAccess)
                {
                    if (ViolationType)
                        linkJobNumbers = linkJobNumbers + (linkJobNumbers.Length > 0 ? ", " : "") +
                            "<b><a class='dob-job-id' href='" + Properties.Settings.Default.FrontEndUrl + "/job/" + c.IdJob + "/violation?highlighted=" + violationNo + "&isDob=true" + "'>" + c.IdJob.ToString() + "</a></b>";
                    else
                    {
                        linkJobNumbers = linkJobNumbers + (linkJobNumbers.Length > 0 ? ", " : "") +
                              "<b><a class='job-id' href='" + Properties.Settings.Default.FrontEndUrl + "/job/" + c.IdJob + "/violation?highlighted=" + violationNo + "'>" + c.IdJob.ToString() + "</a></b>";
                    }
                    if (c.IdCustomer != null && c.IdCustomer > 0)
                    {
                        int idCustomer = Convert.ToInt32(c.IdCustomer);
                        customerList.Add(idCustomer);
                    }


                }
                var customerlist = customerList.Distinct();

                foreach (var item in customerlist)
                {
                    int customerid = Convert.ToInt32(item);
                    Customer customer = rpoContext.Customers.FirstOrDefault(x => x.Id == customerid);

                    CustomerNotificationSetting customerNotificationSettings = rpoContext.CustomerNotificationSettings.Where(x => x.IdCustomer == customer.Id).FirstOrDefault();
                    if (customerNotificationSettings.ViolationInapp == true)
                    {
                        string newJobScopeAddedSetting = string.Empty;
                        if (isNotExits)
                        {
                            newJobScopeAddedSetting = InAppNotificationMessage.Violation_NewCronjob
                                           .Replace("##jobnumbers##", linkJobNumbers)
                                           .Replace("##ViolationNumber##", "<b class='violation-number'>" + violationNo + "</b>");

                            Common.SendCustomerInAppNotifications(customer.Id, newJobScopeAddedSetting, "/job/" + jobNumbers.FirstOrDefault() + "/scope");
                        }
                    }
                }
            }
        }

        public static void SendNewMailPersonWise(Dictionary<string, bool> lstECBViolationsResponse)
        {
            var rpoContext = new Model.RpoContext();


            var violations = lstECBViolationsResponse.ToList(); //rpoContext.JobViolations.Select(x => x.BinNumber).ToList();         

            List<Job> lstJobids = new List<Job>();
            foreach (var v in violations.Distinct())
            {
                var Idviolationnumber = rpoContext.JobViolations.Where(x => x.SummonsNumber == v.Key && x.IsManually != true && x.IsNewMailsent == false).Select(x => x.BinNumber).FirstOrDefault();//fetch today's violations            

                var Idrfpaddress = rpoContext.RfpAddresses.Where(x => x.BinNumber == Idviolationnumber).Select(x => x.Id).ToList();//fetch today's violations            
                foreach (var r in Idrfpaddress)
                {
                    var idjob = rpoContext.Jobs.Where(x => x.IdRfpAddress == r && x.Status == JobStatus.Active).ToList();
                    lstJobids.AddRange(idjob); //list of jobs which are having violation of today
                }
            }
            #region customer assign jobs
            //customer in-app violation notification
            List<CustomerWiseJobs> lstCustomerwisejobs = new List<CustomerWiseJobs>();
            foreach (Job job in lstJobids.Distinct())
            {
                List<CustomerJobAccess> customerJobAccess = rpoContext.CustomerJobAccess
                                    .Include("Job")
                                    .Include("Customer")
                                    .Where(x => x.IdJob == job.Id).ToList();

                foreach (var item in customerJobAccess)
                {
                    CustomerWiseJobs customerwisejobs = new CustomerWiseJobs();

                    customerwisejobs.IdJob = item.IdJob;
                    customerwisejobs.IdCustomer = item.IdCustomer;
                    lstCustomerwisejobs.Add(customerwisejobs);
                }
            }
            #endregion
            #region Team Member assign jobs
            List<int> jobAssignList = new List<int>();
            List<int> MailsentTo = new List<int>();
            foreach (Job job in lstJobids.Distinct())
            {
                #region create list of teammember for particular job one by one
                if (job.IdProjectManager != null && job.IdProjectManager > 0)
                {
                    int idProjectManager = Convert.ToInt32(job.IdProjectManager);
                    jobAssignList.Add(idProjectManager);
                }

                if (job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam))
                {
                    foreach (var item in job.DOBProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }

                if (job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam))
                {
                    foreach (var item in job.DOTProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }

                if (job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam))
                {
                    foreach (var item in job.DEPProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }

                if (job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam))
                {
                    foreach (var item in job.ViolationProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }
                #endregion
            }
            //jobAssignList = (List<int>)jobAssignList.Distinct();
            //List<int> lstmemberwisejobs = new List<int>();
            List<MemberWiseJobs> lstmemberwisejobs = new List<MemberWiseJobs>();

            foreach (var TeamMember in jobAssignList.Distinct())
            {

                foreach (Job job in lstJobids) //get jobs in which this member is assigned in lstmemberwisejobs
                {
                    MemberWiseJobs memberwisejobs = new MemberWiseJobs();
                    if (job.IdProjectManager != null && job.IdProjectManager > 0)
                    {
                        int idProjectManager = Convert.ToInt32(job.IdProjectManager);
                        if (TeamMember == idProjectManager)
                        {
                            memberwisejobs.Id = job.Id;
                            memberwisejobs.IdManager = idProjectManager;
                            lstmemberwisejobs.Add(memberwisejobs);
                            continue;
                        }
                    }

                    if (job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam))
                    {
                        foreach (var item in job.DOBProjectTeam.Split(','))
                        {
                            int employeeid = Convert.ToInt32(item);
                            if (TeamMember == employeeid)
                            {
                                memberwisejobs.Id = job.Id;
                                memberwisejobs.IdManager = employeeid;
                                lstmemberwisejobs.Add(memberwisejobs);
                                continue;
                            }
                        }
                    }

                    if (job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam))
                    {
                        foreach (var item in job.DOTProjectTeam.Split(','))
                        {
                            int employeeid = Convert.ToInt32(item);
                            if (TeamMember == employeeid)
                            {
                                memberwisejobs.Id = job.Id;
                                memberwisejobs.IdManager = employeeid;
                                lstmemberwisejobs.Add(memberwisejobs);
                                continue;
                            }
                        }
                    }

                    if (job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam))
                    {
                        foreach (var item in job.DEPProjectTeam.Split(','))
                        {
                            int employeeid = Convert.ToInt32(item);
                            if (TeamMember == employeeid)
                            {
                                memberwisejobs.Id = job.Id;
                                memberwisejobs.IdManager = employeeid;
                                lstmemberwisejobs.Add(memberwisejobs);
                                continue;
                            }
                        }
                    }

                    if (job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam))
                    {
                        foreach (var item in job.ViolationProjectTeam.Split(','))
                        {
                            int employeeid = Convert.ToInt32(item);
                            if (TeamMember == employeeid)
                            {
                                memberwisejobs.Id = job.Id;
                                memberwisejobs.IdManager = employeeid;
                                lstmemberwisejobs.Add(memberwisejobs);
                                continue;
                            }
                        }
                    }
                }
                #endregion
                List<JobViolation> finalviolations = new List<JobViolation>();

                List<int> idrfps = new List<int>();
                foreach (var item in lstmemberwisejobs.Distinct())
                {
                    //var rfps = rpoContext.Jobs.Where(x => x.Id == item.Id && (x.IdProjectManager == item.IdManager ||
                    //x.DOBProjectTeam == Convert.ToString(item.IdManager) || x.DEPProjectTeam == Convert.ToString(item.IdManager) 
                    //|| x.ViolationProjectTeam == Convert.ToString(item.IdManager)
                    //|| x.DOTProjectTeam == Convert.ToString(item.IdManager))).Select(x => x.IdRfpAddress).ToList();//
                    var rfps = rpoContext.Jobs.Where(x => x.Id == item.Id && x.IdProjectManager == item.IdManager && x.Status == JobStatus.Active).Select(x => x.IdRfpAddress).ToList();//
                    idrfps.AddRange(rfps);
                }
                lstmemberwisejobs.Clear();
                //var rfps = rpoContext.Jobs.Where(x => lstmemberwisejobs.Select(y=>y.Id).Contains(x.Id) && lstmemberwisejobs.Select(z => z.IdManager).Contains(x.IdProjectManager)).Select(x => x.IdRfpAddress).ToList();//
                var binnumbers = rpoContext.RfpAddresses.Where(x => idrfps.Contains(x.Id)).Select(x => x.BinNumber).ToList();
                //finalviolations = rpoContext.JobViolations.Where(x => binnumbers.Contains(x.BinNumber) && x.IsUpdateMailsent == false && x.Type_ECB_DOB == "DOB" && x.PartyResponsible == 1).ToList();
                finalviolations = rpoContext.JobViolations.Where(x => binnumbers.Contains(x.BinNumber) && x.Type_ECB_DOB == "DOB").ToList();

                #region Mail prepare
                string NewECBViolations = string.Empty;
                NewECBViolations = "<p>SnapCor has detected the following violation(s). Click on the link(s) below and navigate to the Violations tab in the Project for further details.</p>";
                foreach (var item in finalviolations)
                {
                    string binNumber = rpoContext.JobViolations.Where(i => i.SummonsNumber == item.SummonsNumber).Select(i => i.BinNumber).FirstOrDefault();
                    var address = rpoContext.RfpAddresses.Where(b => b.BinNumber == binNumber).ToList();
                    var firstaddress = rpoContext.RfpAddresses.Where(b => b.BinNumber == binNumber).FirstOrDefault();
                    var descritption = rpoContext.JobViolations.Where(x => x.SummonsNumber == item.SummonsNumber).Select(x => x.ViolationDescription).FirstOrDefault();
                    var issuingagency = rpoContext.JobViolations.Where(x => x.SummonsNumber == item.SummonsNumber).Select(x => x.IssuingAgency).FirstOrDefault();
                    //if (item.Value == true) /not needed because every violation is newly created
                    //{
                    if (firstaddress != null)
                    {
                        string boroughes = rpoContext.Boroughes.Where(i => i.Id == firstaddress.IdBorough).Select(i => i.Description).FirstOrDefault();
                        //NewECBViolations += "<p>Violation <b> " + item.Key + "</b> is been issued for <b>" + firstaddress.HouseNumber + "," + firstaddress.Street + "," + boroughes + "," + firstaddress.ZipCode + "</b> ";
                        // NewECBViolations += "<p>Violation <b> " + item.Key + "</b> has been issued for <b>" + firstaddress.HouseNumber + "," + firstaddress.Street + "," + boroughes + "," + firstaddress.ZipCode + "</b>";
                        if (!string.IsNullOrWhiteSpace(issuingagency) && !string.IsNullOrWhiteSpace(descritption))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been issued for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                            + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                            + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> , Issuing Agency : " + (!string.IsNullOrWhiteSpace(issuingagency) ? issuingagency : "")
                                            + ", Violation description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                        }
                        else if (string.IsNullOrWhiteSpace(issuingagency) && string.IsNullOrWhiteSpace(descritption))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been issued for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                      + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                      + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> "
                                      + ", Violation description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                        }
                        else if (string.IsNullOrWhiteSpace(issuingagency))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been issued for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                           + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                           + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> "
                                           + ", Violation description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                        }
                        else if (string.IsNullOrWhiteSpace(descritption))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been issued for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                                                          + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                                                          + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> , Issuing Agency : " + (!string.IsNullOrWhiteSpace(issuingagency) ? issuingagency : "");
                        }
                        string project = " and is linked to Project # ";
                        List<int> lstjobids = new List<int>();
                        foreach (var a in address)
                        {
                            List<int> jobids = rpoContext.Jobs.Where(j => j.IdRfpAddress == a.Id && j.Status == JobStatus.Active).Select(i => i.Id).ToList();
                            lstjobids.AddRange(jobids);
                        }
                        foreach (var jobid in lstjobids)
                        {
                            //project += "<b><a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobid + " / violation?highlighted=" + item.SummonsNumber + "\">" + jobid + "</a></b>,";
                            project += "<b><a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobid + "/violation?highlighted=" + item.SummonsNumber + "&isDob=true" + "\">" + jobid + "</a></b>, ";

                        }
                        project = project.Remove(project.Length - 1, 1);
                        NewECBViolations += project;
                    }
                    // }


                }
                string body = string.Empty;
                string emailtemplatepath = AppDomain.CurrentDomain.BaseDirectory + "EmailTemplate/violationMailer.html";
                using (StreamReader reader = new StreamReader(emailtemplatepath))
                {
                    body = reader.ReadToEnd();
                }
                string NewemailBody = body;
                NewemailBody = NewemailBody.Replace("##ViolationNumber##", NewECBViolations);
                #endregion
                #region Send Mail
                List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                //foreach (var item in jobAssignList.Distinct())
                //{
                var Mailvalue = rpoContext.Employees.Where(x => x.Id == TeamMember).FirstOrDefault();
                if (Mailvalue != null)
                    to.Add(new KeyValuePair<string, string>(Mailvalue.Email, Mailvalue.FirstName + " " + Mailvalue.LastName));

                // }



                List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                string commonsub = string.Empty;
                if (Properties.Settings.Default.IsSnapcor == "Yes")
                {
                    commonsub = "[Snapcor] ";
                }
                if (Properties.Settings.Default.IsUAT == "Yes")
                {
                    commonsub = "[UAT] ";
                }


                if (to != null && to.Count > 0)
                {
                    if (NewECBViolations != null && NewECBViolations != "")
                    {
                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "New DOB Violations has been issued - Action may be required", NewemailBody, true);
                        finalviolations.ForEach(x => x.IsNewMailsent = true);
                        rpoContext.SaveChanges();
                    }
                }
                //send email to customer

                #region send email to customer
                List<KeyValuePair<string, string>> toCustomer = new List<KeyValuePair<string, string>>();
                foreach (var item in lstCustomerwisejobs.Distinct())
                {

                    var CustomerMailvalue = rpoContext.Customers.Where(x => x.Id == item.IdCustomer).FirstOrDefault();
                    CustomerNotificationSetting customerNotificationSettings = rpoContext.CustomerNotificationSettings.Where(x => x.IdCustomer == item.IdCustomer).FirstOrDefault();
                    if (customerNotificationSettings.ViolationEmail == true)
                    {
                        if (Mailvalue != null)
                            toCustomer.Add(new KeyValuePair<string, string>(CustomerMailvalue.EmailAddress, CustomerMailvalue.FirstName + " " + CustomerMailvalue.LastName));
                    }
                }
                toCustomer.Distinct();
                if (toCustomer != null && toCustomer.Count > 0)
                {
                    if (NewECBViolations != null && NewECBViolations != "")
                    {  
                            Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toCustomer, cc, commonsub + "New DOB Violations Issued - Action may be required", NewemailBody, true);
                    }
                }
                #endregion
                #endregion
                //var employeelist = jobAssignList.Distinct();

            }
        }
        public static void SendUpdateMailPersonWise(Dictionary<string, bool> lstDOBViolationsResponse)
        {
            var rpoContext = new Model.RpoContext();
            var violations = lstDOBViolationsResponse.ToList(); //rpoContext.JobViolations.Select(x => x.BinNumber).ToList();         

            List<Job> lstJobids = new List<Job>();
            foreach (var v in violations.Distinct())
            {
                var Idviolationnumber = rpoContext.JobViolations.Where(x => x.SummonsNumber == v.Key).Select(x => x.BinNumber).FirstOrDefault();//fetch today's violations            

                var Idrfpaddress = rpoContext.RfpAddresses.Where(x => x.BinNumber == Idviolationnumber).Select(x => x.Id).ToList();//fetch today's violations            
                foreach (var r in Idrfpaddress)
                {
                    var idjob = rpoContext.Jobs.Where(x => x.IdRfpAddress == r && x.Status == JobStatus.Active).ToList();
                    lstJobids.AddRange(idjob); //list of jobs which are having violation of today
                }
            }

            List<int> jobAssignList = new List<int>();
            List<int> MailsentTo = new List<int>();
            foreach (Job job in lstJobids.Distinct())
            {
                #region create list of team member for particular job one by one
                if (job.IdProjectManager != null && job.IdProjectManager > 0)
                {
                    int idProjectManager = Convert.ToInt32(job.IdProjectManager);
                    jobAssignList.Add(idProjectManager);
                }

                if (job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam))
                {
                    foreach (var item in job.DOBProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }

                if (job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam))
                {
                    foreach (var item in job.DOTProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }

                if (job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam))
                {
                    foreach (var item in job.DEPProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }

                if (job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam))
                {
                    foreach (var item in job.ViolationProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(employeeid);
                    }
                }
                #endregion
            }
            //jobAssignList = (List<int>)jobAssignList.Distinct();
            //List<int> lstmemberwisejobs = new List<int>();
            List<MemberWiseJobs> lstmemberwisejobs = new List<MemberWiseJobs>();
            foreach (var TeamMember in jobAssignList.Distinct())
            {

                foreach (Job job in lstJobids) //get jobs in which this member is assigned in lstmemberwisejobs
                {
                    MemberWiseJobs memberwisejobs = new MemberWiseJobs();
                    if (job.IdProjectManager != null && job.IdProjectManager > 0)
                    {
                        int idProjectManager = Convert.ToInt32(job.IdProjectManager);
                        if (TeamMember == idProjectManager)
                        {
                            memberwisejobs.Id = job.Id;
                            memberwisejobs.IdManager = idProjectManager;
                            lstmemberwisejobs.Add(memberwisejobs);
                            continue;
                        }
                    }

                    if (job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam))
                    {
                        foreach (var item in job.DOBProjectTeam.Split(','))
                        {
                            int employeeid = Convert.ToInt32(item);
                            if (TeamMember == employeeid)
                            {
                                memberwisejobs.Id = job.Id;
                                memberwisejobs.IdManager = employeeid;
                                lstmemberwisejobs.Add(memberwisejobs);
                                continue;
                            }
                        }
                    }

                    if (job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam))
                    {
                        foreach (var item in job.DOTProjectTeam.Split(','))
                        {
                            int employeeid = Convert.ToInt32(item);
                            if (TeamMember == employeeid)
                            {
                                memberwisejobs.Id = job.Id;
                                memberwisejobs.IdManager = employeeid;
                                lstmemberwisejobs.Add(memberwisejobs);
                                continue;
                            }
                        }
                    }
                    if (job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam))
                    {
                        foreach (var item in job.DEPProjectTeam.Split(','))
                        {
                            int employeeid = Convert.ToInt32(item);
                            if (TeamMember == employeeid)
                            {
                                memberwisejobs.Id = job.Id;
                                memberwisejobs.IdManager = employeeid;
                                lstmemberwisejobs.Add(memberwisejobs);
                                continue;
                            }
                        }
                    }

                    if (job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam))
                    {
                        foreach (var item in job.ViolationProjectTeam.Split(','))
                        {
                            int employeeid = Convert.ToInt32(item);
                            if (TeamMember == employeeid)
                            {
                                memberwisejobs.Id = job.Id;
                                memberwisejobs.IdManager = employeeid;
                                lstmemberwisejobs.Add(memberwisejobs);
                                continue;
                            }
                        }
                    }

                }

                List<int> idrfps = new List<int>();
                foreach (var item in lstmemberwisejobs)
                {
                    //var rfps = rpoContext.Jobs.Where(x => x.Id == item.Id && (x.IdProjectManager == item.IdManager ||
                    //x.DOBProjectTeam == Convert.ToString(item.IdManager) || x.DEPProjectTeam == Convert.ToString(item.IdManager) 
                    //|| x.ViolationProjectTeam == Convert.ToString(item.IdManager)
                    //|| x.DOTProjectTeam == Convert.ToString(item.IdManager))).Select(x => x.IdRfpAddress).ToList();//
                    var rfps = rpoContext.Jobs.Where(x => x.Id == item.Id && x.IdProjectManager == item.IdManager && x.Status == JobStatus.Active).Select(x => x.IdRfpAddress).ToList();//
                    idrfps.AddRange(rfps);
                }
                lstmemberwisejobs.Clear();

                //var rfps = rpoContext.Jobs.Where(x => lstmemberwisejobs.Contains(x.Id)).Select(x => x.IdRfpAddress).ToList();//
                var binnumbers = rpoContext.RfpAddresses.Where(x => idrfps.Contains(x.Id)).Select(x => x.BinNumber).ToList();
                List<JobViolation> finalviolations = new List<JobViolation>();
                finalviolations = rpoContext.JobViolations.Where(x => binnumbers.Contains(x.BinNumber) && x.Type_ECB_DOB == "DOB" && x.PartyResponsible == 1).ToList();

                #region Mail prepare
                string NewECBViolations = string.Empty;
                foreach (var item in finalviolations)
                {
                    string binNumber = rpoContext.JobViolations.Where(i => i.SummonsNumber == item.SummonsNumber).Select(i => i.BinNumber).FirstOrDefault();
                    var address = rpoContext.RfpAddresses.Where(b => b.BinNumber == binNumber).ToList();
                    var firstaddress = rpoContext.RfpAddresses.Where(b => b.BinNumber == binNumber).FirstOrDefault();
                    var descritption = rpoContext.JobViolations.Where(x => x.SummonsNumber == item.SummonsNumber).Select(x => x.ViolationDescription).FirstOrDefault();
                    var issuingagency = rpoContext.JobViolations.Where(x => x.SummonsNumber == item.SummonsNumber).Select(x => x.IssuingAgency).FirstOrDefault();
                    //if (item.Value == true) /not needed because every violation is newly created
                    //{
                    if (firstaddress != null)
                    {
                        string boroughes = rpoContext.Boroughes.Where(i => i.Id == firstaddress.IdBorough).Select(i => i.Description).FirstOrDefault();
                        //NewECBViolations += "<p>Violation <b> " + item.Key + "</b> is been issued for <b>" + firstaddress.HouseNumber + "," + firstaddress.Street + "," + boroughes + "," + firstaddress.ZipCode + "</b> ";
                        // NewECBViolations += "<p>Violation <b> " + item.Key + "</b> has been issued for <b>" + firstaddress.HouseNumber + "," + firstaddress.Street + "," + boroughes + "," + firstaddress.ZipCode + "</b>";
                        if (!string.IsNullOrWhiteSpace(issuingagency) && !string.IsNullOrWhiteSpace(descritption))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been updated for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                            + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                            + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> , Issuing Agency : " + (!string.IsNullOrWhiteSpace(issuingagency) ? issuingagency : "")
                                            + ", Violation description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                        }
                        else if (string.IsNullOrWhiteSpace(issuingagency) && string.IsNullOrWhiteSpace(descritption))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been updated for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                      + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                      + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> "
                                      + ", Violation description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                        }
                        else if (string.IsNullOrWhiteSpace(issuingagency))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been updated for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                           + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                           + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> "
                                           + ", Violation description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                        }
                        else if (string.IsNullOrWhiteSpace(descritption))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been updated for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                                                          + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                                                          + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> , Issuing Agency : " + (!string.IsNullOrWhiteSpace(issuingagency) ? issuingagency : "");
                        }
                        string project = " is recorded at Project# ";
                        List<int> lstjobids = new List<int>();
                        foreach (var a in address)
                        {
                            List<int> jobids = rpoContext.Jobs.Where(j => j.IdRfpAddress == a.Id && j.Status == JobStatus.Active).Select(i => i.Id).ToList();
                            lstjobids.AddRange(jobids);
                        }
                        foreach (var jobid in lstjobids)
                        {
                            //project += "<b><a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobid + "/violation?highlighted=" + item.SummonsNumber + "\">" + jobid + "</a></b>,";
                            project += "<b><a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobid + "/violation?highlighted=" + item.SummonsNumber + "&isDob=true" + "\">" + jobid + "</a></b>, ";

                        }
                        project = project.Remove(project.Length - 1, 1);
                        NewECBViolations += project;
                    }
                    // }


                }
                string body = string.Empty;
                string emailtemplatepath = AppDomain.CurrentDomain.BaseDirectory + "EmailTemplate/violationMailer.html";
                using (StreamReader reader = new StreamReader(emailtemplatepath))
                {
                    body = reader.ReadToEnd();
                }
                string NewemailBody = body;
                NewemailBody = NewemailBody.Replace("##ViolationNumber##", NewECBViolations);
                #endregion

                #region Send Mail
                List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();

                //foreach (var item in jobAssignList.Distinct())
                //{
                var Mailvalue = rpoContext.Employees.Where(x => x.Id == TeamMember).FirstOrDefault();
                if (Mailvalue != null)
                    to.Add(new KeyValuePair<string, string>(Mailvalue.Email, Mailvalue.FirstName + " " + Mailvalue.LastName));
                //}
                List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                string commonsub = string.Empty;
                if (Properties.Settings.Default.IsSnapcor == "Yes")
                {
                    commonsub = "[Snapcor] ";
                }
                if (Properties.Settings.Default.IsUAT == "Yes")
                {
                    commonsub = "[UAT] ";
                }

                if (to != null && to.Count > 0)
                {
                    if (NewECBViolations != null && NewECBViolations != "")
                    {
                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "DOB Violations Updates Received - Action may be required", NewemailBody, true);
                        finalviolations.ForEach(x => x.IsUpdateMailsent = true);
                        rpoContext.SaveChanges();
                    }
                }
                #endregion

                //var employeelist = jobAssignList.Distinct();
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
            string errorLogFilename = "DOBViolationLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

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
        public class MemberWiseJobs
        {
            public int? Id { get; set; }
            public int? IdManager { get; set; }
        }
        public class CustomerWiseJobs
        {
            public int? IdJob { get; set; }
            public int? IdCustomer { get; set; }
        }
    }
}