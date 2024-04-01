
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
    using static Dropbox.Api.TeamLog.EventCategory;
    using Quartz.Impl.AdoJobStore.Common;
    using System.Windows.Forms;
    using System.Net.Mail;

    public class JobECBViolationUpdateResult : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }
        //public JobECBViolationUpdateResult()
        //{
        //    SendMailNotification();
        //}

        public static void SendMailNotification()
        {
            using (var ctx = new Model.RpoContext())
            {
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
                //var activeJobsCount = query.Count();
                var objBinList = query.Select(x => x.BinNumber).Distinct().ToList();


                Dictionary<string, bool> lstECBViolationsResponse = new Dictionary<string, bool>();
                List<string> lstECBViolationse = new List<string>();
                foreach (var objBinNumber in objBinList)
                {
                    var clientBIS = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
                    var datasetBIS = clientBIS.GetResource<object>("6bgk-3dad");
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                      SecurityProtocolType.Tls11 |
                      SecurityProtocolType.Tls12;
                    var rowsBIS = datasetBIS.GetRows(limit: 1000);
                    string qryBIS = string.Empty;
                    qryBIS = qryBIS + "bin='" + objBinNumber + "'" + " and " + "ecb_violation_status='" + "ACTIVE" + "'";
                    var soqlBIS = new SoqlQuery().Select("*").Where(qryBIS);
                    var resultsBIS = datasetBIS.Query(soqlBIS);
                    string summonsNumber = string.Empty;
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (resultsBIS != null && resultsBIS.Count() > 0)
                    {
                        foreach (var item in resultsBIS)
                        {
                            string jsonstring = JsonConvert.SerializeObject(item);
                            var records = JsonConvert.DeserializeObject<ViolationECBJsonRes>(jsonstring);
                            summonsNumber = records.ecb_violation_number;
                            JobViolation jobViolation = null;
                            bool isNotExist = false;
                            lstECBViolationse.Add(summonsNumber);
                            jobViolation = ctx.JobViolations.FirstOrDefault(x => x.SummonsNumber == summonsNumber);
                            if (jobViolation == null)
                            {
                                if (records.bin != "1000000" && records.bin != "2000000" && records.bin != "3000000" && records.bin != "4000000" && records.bin != "5000000" && records.bin != "6000000")
                                {
                                    isNotExist = true;
                                    jobViolation = new JobViolation();
                                    string fulladdress = records.respondent_city + " " + records.respondent_house_number + " " + records.respondent_street + " " + records.respondent_zip;
                                    string timeString = records.hearing_time;
                                    if (timeString.Length == 3)
                                    {
                                        timeString = "0" + timeString;
                                    }
                                    DateTime HearingDate = DateTime.ParseExact(records.hearing_date, "yyyyMMdd", CultureInfo.InvariantCulture);
                                    int hour = int.Parse(timeString.Substring(0, 2));
                                    int minute = int.Parse(timeString.Substring(2, 2));
                                    DateTime HearingTime = HearingDate.AddHours(hour).AddMinutes(minute);
                                    DateTime dateTime1 = DateTime.ParseExact(records.issue_date, "yyyyMMdd", CultureInfo.InvariantCulture);
                                    jobViolation.RespondentAddress = fulladdress;
                                    jobViolation.CertificationStatus = Convert.ToString(records.certification_status);
                                    jobViolation.BalanceDue = Convert.ToDouble(records.balance_due);
                                    jobViolation.HearingDate = HearingDate;
                                    jobViolation.HearingTime = HearingTime;
                                    jobViolation.DateIssued = dateTime1;
                                    jobViolation.violation_type = Convert.ToString(records.violation_type);
                                    jobViolation.aggravated_level = Convert.ToString(records.aggravated_level);
                                    jobViolation.RespondentName = Convert.ToString(records.respondent_name);
                                    jobViolation.RespondentAddress = fulladdress;
                                    jobViolation.ViolationDescription = Convert.ToString(records.violation_description);
                                    jobViolation.Type_ECB_DOB = "ECB";
                                    jobViolation.IsManually = false;
                                    jobViolation.IsNewMailsent = false;
                                    jobViolation.SummonsNumber = summonsNumber;
                                    jobViolation.BinNumber = records.bin;
                                    jobViolation.CreatedBy = 2;
                                    jobViolation.CreatedDate = DateTime.UtcNow;
                                    jobViolation.PartyResponsible = 3;
                                    jobViolation.Status = 1;
                                    jobViolation.ISNViolation = Convert.ToString(records.isn_dob_bis_extract);
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

                                    lstECBViolationsResponse.Add(summonsNumber, isNotExist);

                                    List<int> jobNumbers = ctx.Jobs.Where(job => ctx.RfpAddresses
                                               .Where(address => address.BinNumber == objBinNumber)
                                               .Select(address => address.Id)
                                               .Contains(job.IdRfpAddress) && job.Status == JobStatus.Active)
                                               .Select(x => x.Id)
                                               .ToList();

                                    SendViolationUpdatedSNotification(jobNumbers, summonsNumber, isNotExist, false, "");
                                }
                                //else
                                //{
                                //    throw new RpoBusinessException("Scrapping does not work for dummy BIN " + records.bin);
                                //}
                            }
                        }
                    }
                }
                UpdateECBCronJob(lstECBViolationse, lstECBViolationsResponse);

            }
        }
        public static void UpdateECBCronJob(List<string> lstECBViolationse, Dictionary<string, bool> lstECBViolationsResponse)
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

                string ResultNot = string.Empty;
                string ResultYes = string.Empty;
                int OATHYesScanCount = 0;
                int OATHNoScanCount = 0;

                string OATHYesScanRec = string.Empty;
                string OATHNoScanRec = string.Empty;

                int ECBYesScanCount = 0;
                int ECBNoScanCount = 0;

                string ECBYesScanRec = string.Empty;
                string ECBNoScanRec = string.Empty;

                Dictionary<string, bool> lstDOBViolationsResponse = new Dictionary<string, bool>();
                List<ViolationExplanationOfCharges> lstViolationExplanationOfCharges = new List<ViolationExplanationOfCharges>();
                //Iterate over each Summons Number and fetch the data from site
                foreach (var objSummons in lstECBViolationse)
                {
                    JobViolation itemSummons = ctx.JobViolations.Where(x => x.IsFullyResolved == false && x.SummonsNumber == objSummons && x.Type_ECB_DOB == "ECB").OrderByDescending(d => d.Id).FirstOrDefault();

                    if (itemSummons != null)
                    {
                        string actualSummonsNumber = itemSummons.SummonsNumber;
                        string summonsNoticeNumber = "0" + itemSummons.SummonsNumber;

                        string commanMessage = string.Empty;
                        string InspectionLocation = string.Empty;
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

                        qry = qry + "ticket_number='" + summonsNoticeNumber + "'";

                        var soql = new SoqlQuery().Select("*").Where(qry);

                        var results = dataset.Query(soql);

                        var clientBIS = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
                        var datasetBIS = clientBIS.GetResource<object>("6bgk-3dad");
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                           | SecurityProtocolType.Tls11
                                           | SecurityProtocolType.Tls12;
                        var rowsBIS = datasetBIS.GetRows(limit: 1000);

                        qryBIS = qryBIS + "ecb_violation_number='" + itemSummons.SummonsNumber + "'";

                        var soqlBIS = new SoqlQuery().Select("ecb_violation_number", "certification_status", "isn_dob_bis_extract", "violation_description", "bin").Where(qryBIS);

                        var resultsBIS = datasetBIS.Query(soqlBIS);

                        string summonsNumber = string.Empty;
                        double PaneltyAmount = 0;
                        string dateIssued = string.Empty;
                        string issuingAgency = string.Empty;
                        string respondentName = string.Empty;
                        double balanceDue = 0;
                        string inspectionLocation = string.Empty;
                        string respondentAddress = string.Empty;
                        string statusOfSummonsNotice = string.Empty;
                        string HearingStatus = string.Empty;
                        string hearingResult = string.Empty;
                        string binNumber = string.Empty;
                        string violation_description = string.Empty;
                        string hearingLocation = string.Empty;
                        DateTime? hearingDate = null;
                        string code = string.Empty;
                        string isnViolation = string.Empty;
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        string StatusNotFormat = "Job# ##Job## - JobViolation# ##JobViolation## ";
                        if (results != null && results.Count() > 0)
                        {
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

                                PaneltyAmount = Convert.ToInt32(records.penalty_imposed);
                                respondentAddress = fulladdress;
                                hearingResult = records.hearing_result;
                                HearingStatus = records.hearing_status;
                                hearingLocation = string.Empty;

                                if (records.violation_location_house != null)
                                { InspectionLocation = InspectionLocation + " " + records.violation_location_house; }
                                if (records.violation_location_street_name != null)
                                { InspectionLocation = InspectionLocation + " " + records.violation_location_street_name; }
                                if (records.violation_location_city != null)
                                { InspectionLocation = InspectionLocation + " " + records.violation_location_city; }
                                if (records.violation_location_state_name != null)
                                { InspectionLocation = InspectionLocation + " " + records.violation_location_state_name; }
                                if (records.violation_location_zip_code != null)
                                { InspectionLocation = InspectionLocation + " " + records.violation_location_zip_code; }

                                if (records.hearing_date != null && !string.IsNullOrEmpty(records.hearing_date))
                                {
                                    hearingDate = Convert.ToDateTime(records.hearing_date, provider);
                                }

                                code = string.Empty;
                                hearingLocation = records.scheduled_hearing_location;

                                ViolationExplanationOfCharges ViolationExplanationOfChargesDTO = new ViolationExplanationOfCharges();
                                ViolationExplanationOfChargesDTO.PaneltyAmount = records.penalty_imposed;
                                ViolationExplanationOfChargesDTO.Code = records.charge_1_code;
                                ViolationExplanationOfChargesDTO.CodeSection = records.charge_1_code_section;
                                ViolationExplanationOfChargesDTO.Description = records.charge_1_code_description;
                                lstViolationExplanationOfCharges.Add(ViolationExplanationOfChargesDTO);

                            }
                            string newstr = StatusNotFormat.Replace("##Job##", itemSummons != null ? itemSummons.IdJob.ToString() : string.Empty)
                                                      .Replace("##JobViolation##", itemSummons != null && itemSummons != null ? itemSummons.SummonsNumber : JobHistoryMessages.NoSetstring);
                            OATHYesScanRec = OATHYesScanRec + newstr + "<br>";

                            OATHYesScanCount = OATHYesScanCount + 1;
                        }
                        else
                        {
                            string newstr = StatusNotFormat.Replace("##Job##", itemSummons != null ? itemSummons.IdJob.ToString() : string.Empty)
                                                    .Replace("##JobViolation##", itemSummons != null && itemSummons != null ? itemSummons.SummonsNumber : JobHistoryMessages.NoSetstring);
                            OATHNoScanRec = OATHNoScanRec + newstr + "<br>";

                            OATHNoScanCount = OATHNoScanCount + 1;
                        }

                        string ecb_Certification_status = string.Empty;
                        if (resultsBIS != null && resultsBIS.Count() > 0)
                        {
                            foreach (var item in resultsBIS)
                            {
                                string jsonstring = JsonConvert.SerializeObject(item);
                                var records = JsonConvert.DeserializeObject<ViolationJsonRes>(jsonstring);

                                WriteLogWebclient("ecb_Certification_status is : " + records.certification_status);
                                ecb_Certification_status = records.certification_status;
                                isnViolation = records.isn_dob_bis_extract;
                                if (hearingDate == null && records.hearing_date != null && !string.IsNullOrEmpty(records.hearing_date))
                                {
                                    hearingDate = Convert.ToDateTime(records.hearing_date, provider);
                                }
                                if (records.violation_description != null && !string.IsNullOrEmpty(records.violation_description))
                                {
                                    violation_description = records.violation_description;
                                }
                                binNumber = records.bin;

                            }
                            ECBYesScanCount = ECBYesScanCount + 1;

                            string newstr = StatusNotFormat.Replace("##Job##", itemSummons != null ? itemSummons.IdJob.ToString() : string.Empty)
                                                      .Replace("##JobViolation##", itemSummons != null && itemSummons != null ? itemSummons.SummonsNumber : JobHistoryMessages.NoSetstring);
                            ECBYesScanRec = ECBYesScanRec + newstr + "<br>";
                        }
                        else
                        {
                            ECBNoScanCount = ECBNoScanCount + 1;

                            string newstr = StatusNotFormat.Replace("##Job##", itemSummons != null ? itemSummons.IdJob.ToString() : string.Empty)
                                                    .Replace("##JobViolation##", itemSummons != null && itemSummons != null ? itemSummons.SummonsNumber : JobHistoryMessages.NoSetstring);
                            ECBNoScanRec = ECBNoScanRec + newstr + "<br>";
                        }
                        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                        TextInfo textInfo = cultureInfo.TextInfo;
                        summonsNoticeNumber = actualSummonsNumber;

                        if (hearingDate != null && (itemSummons.HearingDate == null || itemSummons.HearingDate.ToString().Trim() == "" || NotEqualDate(hearingDate, itemSummons.HearingDate)))
                        {
                            itemSummons.HearingDate = hearingDate;
                            itemSummons.LastModifiedBy = 2;
                            itemSummons.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.IsUpdateMailsent = false;
                            commanMessage = commanMessage + " " + "Hearing Date  was updated to <'" + hearingDate.Value.ToString("MM/dd/yyyy") + "'>.";
                        }
                        if (binNumber != null && !string.IsNullOrEmpty(binNumber) && (string.IsNullOrEmpty(itemSummons.BinNumber) || itemSummons.BinNumber.ToLower().Trim() != binNumber.ToLower().Trim()))
                        {
                            commanMessage = commanMessage + " " + "Bin Number was updated to <'" + binNumber + "'>.";

                            itemSummons.BinNumber = Convert.ToString(binNumber);
                            itemSummons.LastModifiedBy = 2;
                            itemSummons.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.IsUpdateMailsent = false;
                        }
                        if (HearingStatus != null && !string.IsNullOrEmpty(HearingStatus) && (string.IsNullOrEmpty(itemSummons.StatusOfSummonsNotice) || itemSummons.StatusOfSummonsNotice.ToLower().Trim() != HearingStatus.ToLower().Trim()))
                        {
                            itemSummons.StatusOfSummonsNotice = textInfo.ToTitleCase(HearingStatus);
                            itemSummons.LastModifiedBy = 2;
                            itemSummons.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.IsUpdateMailsent = false;
                            commanMessage = commanMessage + " " + "Status of Summons was updated to <'" + HearingStatus + "'>.";
                        }
                        if (itemSummons.BalanceDue != balanceDue)
                        {
                            itemSummons.BalanceDue = balanceDue;
                            itemSummons.LastModifiedBy = 2;
                            itemSummons.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.IsUpdateMailsent = false;
                            commanMessage = commanMessage + " " + "Balance Due was updated to $<" + balanceDue + ">.";

                        }
                        if (hearingResult != null && !string.IsNullOrEmpty(hearingResult) && (string.IsNullOrEmpty(itemSummons.HearingResult) || itemSummons.HearingResult.ToLower().Trim() != hearingResult.ToLower().Trim()))
                        {
                            itemSummons.HearingResult = textInfo.ToTitleCase(hearingResult);
                            itemSummons.LastModifiedBy = 2;
                            itemSummons.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.IsUpdateMailsent = false;
                            commanMessage = commanMessage + " " + "Hearing Result was updated to <'" + hearingResult + "'>.";

                        }
                        if (ecb_Certification_status != null && !string.IsNullOrEmpty(ecb_Certification_status) && (string.IsNullOrEmpty(itemSummons.CertificationStatus) || itemSummons.CertificationStatus.ToLower().Trim() != ecb_Certification_status.ToLower().Trim()))
                        {
                            commanMessage = commanMessage + " " + "Certification Status was updated to <'" + ecb_Certification_status + "'>.";
                            itemSummons.CertificationStatus = textInfo.ToTitleCase(ecb_Certification_status);
                            itemSummons.LastModifiedBy = 2;
                            itemSummons.LastModifiedDate = DateTime.UtcNow;
                            itemSummons.IsUpdateMailsent = false;
                        }
                        itemSummons.InspectionLocation = InspectionLocation;
                        itemSummons.ViolationDescription = violation_description;
                        itemSummons.ISNViolation = isnViolation;
                        itemSummons.IssuingAgency = issuingAgency;
                        itemSummons.StatusOfSummonsNotice = HearingStatus;
                        itemSummons.HearingResult = hearingResult != null ? textInfo.ToTitleCase(hearingResult) : string.Empty;
                        itemSummons.BalanceDue = balanceDue;
                        itemSummons.CreatedBy = 2;
                        itemSummons.PaneltyAmount = PaneltyAmount;
                        itemSummons.CreatedDate = DateTime.UtcNow;
                        itemSummons.LastModifiedDate = DateTime.UtcNow;
                        ctx.SaveChanges();

                        if (lstViolationExplanationOfCharges != null)
                        {
                            foreach (var item in lstViolationExplanationOfCharges)
                            {
                                JobViolationExplanationOfCharges jobViolationExplanationOfCharges = null;

                                jobViolationExplanationOfCharges = ctx.JobViolationExplanationOfCharges.FirstOrDefault(x => x.IdViolation == itemSummons.Id);

                                if (jobViolationExplanationOfCharges == null)
                                {

                                    JobViolationExplanationOfCharges explanationOfChargesDTO = new JobViolationExplanationOfCharges();
                                    explanationOfChargesDTO.IdViolation = itemSummons.Id;
                                    explanationOfChargesDTO.Code = item.Code;
                                    explanationOfChargesDTO.CodeSection = item.CodeSection;
                                    explanationOfChargesDTO.Description = item.Description;
                                    explanationOfChargesDTO.PaneltyAmount = item.PaneltyAmount;
                                    explanationOfChargesDTO.IsFromAuth = true;
                                    explanationOfChargesDTO.LastModifiedDate = DateTime.UtcNow;
                                    explanationOfChargesDTO.CreatedDate = DateTime.UtcNow;
                                    explanationOfChargesDTO.CreatedBy = 2;
                                    ctx.JobViolationExplanationOfCharges.Add(explanationOfChargesDTO);
                                    ctx.SaveChanges();
                                }

                            }
                            lstViolationExplanationOfCharges.Clear();
                        }
                        if (!string.IsNullOrEmpty(commanMessage))
                        {
                            if (itemSummons.PartyResponsible != 3)
                            {
                                lstDOBViolationsResponse.Add(itemSummons.SummonsNumber, false);

                                List<int> jobNumbers = ctx.Jobs.Where(job => ctx.RfpAddresses
                                           .Where(address => address.BinNumber == itemSummons.BinNumber)
                                           .Select(address => address.Id)
                                           .Contains(job.IdRfpAddress) && job.Status == JobStatus.Active)
                                           .Select(x => x.Id)
                                           .ToList();
                                SendViolationUpdatedSNotification(jobNumbers, itemSummons.SummonsNumber, false, true, commanMessage);
                            }
                        }
                    }
                }
                string ECBUpdateViolations = string.Empty;
                foreach (var item in lstDOBViolationsResponse)
                {
                    string binNumber = ctx.JobViolations.Where(i => i.SummonsNumber == item.Key).Select(i => i.BinNumber).FirstOrDefault();
                    var address = ctx.RfpAddresses.Where(b => b.BinNumber == binNumber).ToList();
                    var descritption = ctx.JobViolations.Where(x => x.SummonsNumber == item.Key).Select(x => x.ViolationDescription).FirstOrDefault();
                    var issuingagency = ctx.JobViolations.Where(x => x.SummonsNumber == item.Key).Select(x => x.IssuingAgency).FirstOrDefault();
                    var firstaddress = ctx.RfpAddresses.Where(b => b.BinNumber == binNumber).FirstOrDefault();

                    if (firstaddress != null && !string.IsNullOrWhiteSpace(issuingagency) && !string.IsNullOrWhiteSpace(descritption))
                    {
                        string boroughes = ctx.Boroughes.Where(i => i.Id == firstaddress.IdBorough).Select(i => i.Description).FirstOrDefault();
                        ECBUpdateViolations += "<p>Violation <b> " + item.Key + "</b> has been updated for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                        + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                        + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> , Issuing Agency : " + (!string.IsNullOrWhiteSpace(issuingagency) ? issuingagency : "")
                                        + ", Violation Description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                    }
                    else if (string.IsNullOrWhiteSpace(issuingagency))
                    {
                        string boroughes = ctx.Boroughes.Where(i => i.Id == firstaddress.IdBorough).Select(i => i.Description).FirstOrDefault();
                        ECBUpdateViolations += "<p>Violation <b> " + item.Key + "</b> has been updated for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                       + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                       + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> "
                                       + ", Violation Description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                    }
                    else if (string.IsNullOrWhiteSpace(descritption))
                    {
                        string boroughes = ctx.Boroughes.Where(i => i.Id == firstaddress.IdBorough).Select(i => i.Description).FirstOrDefault();
                        ECBUpdateViolations += "<p>Violation <b> " + item.Key + "</b> has been updated for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                                                      + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                                                      + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> , Issuing Agency : " + (!string.IsNullOrWhiteSpace(issuingagency) ? issuingagency : "");
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
                        project += "<b><a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobid + "/violation?highlighted=" + item.Key + "\">" + jobid + "</a></b>,";
                    }
                    project = project.Remove(project.Length - 1, 1);
                    ECBUpdateViolations += project;
                }

                emailBody = emailBody.Replace("##ViolationNumber##", ECBUpdateViolations);

                //new violations
                string NewECBViolations = string.Empty;
                if (lstECBViolationsResponse.Count > 0)
                {
                    NewECBViolations = "<p>SnapCor has detected the following violation(s). Click on the link(s) below and navigate to the Violations tab in the Project for further details.</p>";
                    foreach (var item in lstECBViolationsResponse)
                    {

                        string binNumber = ctx.JobViolations.Where(i => i.SummonsNumber == item.Key).Select(i => i.BinNumber).FirstOrDefault();
                        var address = ctx.RfpAddresses.Where(b => b.BinNumber == binNumber).ToList();
                        var firstaddress = ctx.RfpAddresses.Where(b => b.BinNumber == binNumber).FirstOrDefault();
                        var descritption = ctx.JobViolations.Where(x => x.SummonsNumber == item.Key).Select(x => x.ViolationDescription).FirstOrDefault();
                        var issuingagency = ctx.JobViolations.Where(x => x.SummonsNumber == item.Key).Select(x => x.IssuingAgency).FirstOrDefault();
                        if (item.Value == true)
                        {
                            if (firstaddress != null)
                            {
                                string boroughes = ctx.Boroughes.Where(i => i.Id == firstaddress.IdBorough).Select(i => i.Description).FirstOrDefault();
                                //NewECBViolations += "<p>Violation <b> " + item.Key + "</b> is been issued for <b>" + firstaddress.HouseNumber + "," + firstaddress.Street + "," + boroughes + "," + firstaddress.ZipCode + "</b> ";
                                // NewECBViolations += "<p>Violation <b> " + item.Key + "</b> has been issued for <b>" + firstaddress.HouseNumber + "," + firstaddress.Street + "," + boroughes + "," + firstaddress.ZipCode + "</b>";
                                if (!string.IsNullOrWhiteSpace(issuingagency) && !string.IsNullOrWhiteSpace(descritption))
                                {
                                    NewECBViolations += "<p>Violation <b> " + item.Key + "</b> has been issued for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                                    + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                                    + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> , Issuing Agency : " + (!string.IsNullOrWhiteSpace(issuingagency) ? issuingagency : "")
                                                    + ", Violation Description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                                }
                                else if (string.IsNullOrWhiteSpace(issuingagency))
                                {
                                    NewECBViolations += "<p>Violation <b> " + item.Key + "</b> has been issued for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                                   + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                                   + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> "
                                                   + ", Violation Description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                                }
                                else if (string.IsNullOrWhiteSpace(descritption))
                                {
                                    NewECBViolations += "<p>Violation <b> " + item.Key + "</b> has been issued for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                                                                  + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                                                                  + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> , Issuing Agency : " + (!string.IsNullOrWhiteSpace(issuingagency) ? issuingagency : "");
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
                                    project += "<b><a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobid + "/violation?highlighted=" + item.Key + "\">" + jobid + "</a></b>,";
                                }
                                project = project.Remove(project.Length - 1, 1);
                                NewECBViolations += project;
                            }
                        }
                    }
                }
                NewemailBody = NewemailBody.Replace("##ViolationNumber##", NewECBViolations);
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
                    if (NewECBViolations != null && NewECBViolations != "")
                    {

                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "New ECB/OATH Violations has been issued - Action may be required", NewemailBody, true);
                        SendNewMailPersonWise(lstECBViolationsResponse);
                    }
                    if (ECBUpdateViolations != null && ECBUpdateViolations != "")
                    {
                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "ECB/OATH Violations Updates Received - Action may be required", emailBody, true);
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
                finalviolations = rpoContext.JobViolations.Where(x => binnumbers.Contains(x.BinNumber) && x.Type_ECB_DOB == "ECB").ToList();
                //finalviolations = rpoContext.JobViolations.Where(x => binnumbers.Contains(x.BinNumber) && (DbFunctions.TruncateTime(x.LastModifiedDate) == DbFunctions.TruncateTime(DateTime.UtcNow)) && x.Type_ECB_DOB == "ECB" && x.PartyResponsible == 1).ToList();

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
                                            + ", Violation Description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                        }
                        else if (string.IsNullOrWhiteSpace(issuingagency) && string.IsNullOrWhiteSpace(descritption))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been issued for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                      + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                      + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> "
                                      + ", Violation Description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                        }
                        else if (string.IsNullOrWhiteSpace(issuingagency))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been issued for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                           + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                           + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> "
                                           + ", Violation Description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
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
                            project += "<b><a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobid + "/violation?highlighted=" + item.SummonsNumber + "\">" + jobid + "</a></b>,";
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
                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "New ECB/OATH Violations has been issued - Action may be required", NewemailBody, true);
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
                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), toCustomer, cc, commonsub + "New ECB/OATH Violations Issued - Action may be required", NewemailBody, true);
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
            //if (Type == "ECB")
            //{
            //    violations = rpoContext.JobViolations.Where(x => x.Type_ECB_DOB == "ECB" && lstDOBViolationsResponse && x.PartyResponsible == 1).Where(x => x.IsManually != true).Where(x => x.IsUpdateMailsent == false).Select(x => x.BinNumber).ToList();
            //}
            //else if (Type == "DOB")
            //{
            //    violations = rpoContext.JobViolations.Where(x => x.Type_ECB_DOB == "DOB" && x.PartyResponsible == 1).Where(x => x.IsManually != true).Where(x => x.IsUpdateMailsent == false).Select(x => x.BinNumber).ToList();
            //}
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

                finalviolations = rpoContext.JobViolations.Where(x => binnumbers.Contains(x.BinNumber) && x.Type_ECB_DOB == "ECB" && x.PartyResponsible == 1).ToList();
                //finalviolations = rpoContext.JobViolations.Where(x => binnumbers.Contains(x.BinNumber) && (DbFunctions.TruncateTime(x.LastModifiedDate) == DbFunctions.TruncateTime(DateTime.UtcNow)) && x.Type_ECB_DOB == "ECB" && x.PartyResponsible == 1).ToList();

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
                                            + ", Violation Description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                        }
                        else if (string.IsNullOrWhiteSpace(issuingagency) && string.IsNullOrWhiteSpace(descritption))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been updated for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                      + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                      + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> "
                                      + ", Violation Description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
                        }
                        else if (string.IsNullOrWhiteSpace(issuingagency))
                        {
                            NewECBViolations += "<p>Violation <b> " + item.SummonsNumber + "</b> has been updated for <b>" + (!string.IsNullOrWhiteSpace(firstaddress.HouseNumber) ? firstaddress.HouseNumber : "") + " "
                                           + (!string.IsNullOrWhiteSpace(firstaddress.Street) ? firstaddress.Street : "") + " " + (!string.IsNullOrWhiteSpace(boroughes) ? boroughes : "") + " "
                                           + (!string.IsNullOrWhiteSpace(firstaddress.ZipCode) ? firstaddress.ZipCode : "") + "</b> "
                                           + ", Violation Description : " + (!string.IsNullOrWhiteSpace(descritption) ? descritption : "");
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
                            project += "<b><a href=" + Properties.Settings.Default.FrontEndUrl + "/job/" + jobid + "/violation?highlighted=" + item.SummonsNumber + "\">" + jobid + "</a></b>,";
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
                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "ECB/OATH Violations Updates Received - Action may be required", NewemailBody, true);
                        finalviolations.ForEach(x => x.IsUpdateMailsent = true);
                        rpoContext.SaveChanges();
                    }
                }
                #endregion
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
            public float penalty_imposed { get; set; }
            public string charge_1_code { get; set; }
            public string charge_1_code_section { get; set; }
            public string charge_1_code_description { get; set; }
            public string violation_location_house { get; set; }
            public string violation_location_street_name { get; set; }
            public string violation_location_city { get; set; }
            public string violation_location_state_name { get; set; }
            public string violation_location_zip_code { get; set; }
            public string isn_dob_bis_extract { get; set; }
            public string violation_description { get; set; }
            public string bin { get; set; }

        }

        public class ViolationECBJsonRes
        {
            public string bin { get; set; }
            public string balance_due { get; set; }
            public string issue_date { get; set; }
            public string respondent_name { get; set; }
            public string respondent_house_number { get; set; }
            public string respondent_street { get; set; }
            public string respondent_city { get; set; }
            public string respondent_zip { get; set; }
            public string certification_status { get; set; }
            public string violation_type { get; set; }
            public string aggravated_level { get; set; }
            public string violation_description { get; set; }
            public string ecb_violation_number { get; set; }
            public DateTime violation_date { get; set; }
            public string hearing_result { get; set; }
            public string hearing_date { get; set; }
            public string hearing_time { get; set; }
            public float penalty_imposed { get; set; }
            public string infraction_code1 { get; set; }
            public string section_law_description1 { get; set; }
            public string infraction_code2 { get; set; }
            public string section_law_description2 { get; set; }
            public string infraction_code3 { get; set; }
            public string section_law_description3 { get; set; }
            public string infraction_code4 { get; set; }
            public string section_law_description4 { get; set; }
            public string infraction_code5 { get; set; }
            public string section_law_description5 { get; set; }
            public string infraction_code6 { get; set; }
            public string section_law_description6 { get; set; }
            public string infraction_code7 { get; set; }
            public string section_law_description7 { get; set; }
            public string infraction_code8 { get; set; }
            public string section_law_description8 { get; set; }
            public string infraction_code9 { get; set; }
            public string section_law_description9 { get; set; }
            public string infraction_code10 { get; set; }
            public string section_law_description10 { get; set; }
            public string isn_dob_bis_extract { get; set; }
        }
        public class ViolationExplanationOfCharges
        {
            public int IdViolation { get; set; }

            public string Code { get; set; }

            public string CodeSection { get; set; }

            public string Description { get; set; }

            public double? PaneltyAmount { get; set; }


            public bool IsFromAuth { get; set; }

            public int? CreatedBy { get; set; }

            public DateTime? CreatedDate { get; set; }

            public int? LastModifiedBy { get; set; }

            public DateTime? LastModifiedDate { get; set; }
        }

        public static void WriteLogWebclient(string message)
        {
            string errorLogFilename = "ECBViolationLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

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




