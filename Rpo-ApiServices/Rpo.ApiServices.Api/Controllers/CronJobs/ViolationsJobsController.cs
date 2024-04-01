
namespace Rpo.ApiServices.Api.Controllers.CronJobs
{
    using JobViolations;
    using Model;
    using Newtonsoft.Json;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using SODA;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mail;
    using System.Text;
    using System.Threading;
    using System.Web.Http;
    using SystemSettings;

    public class ViolationsJobsController : ApiController
    {
        [HttpGet]
        [Route("api/ViolationJobs/GetECBCronJob/{binnumber}")]
        public void SendECBNotification(string binnumber)
        {
            StringBuilder sb = new StringBuilder();
            using (var ctx = new Model.RpoContext())
            {

                Dictionary<string, bool> lstECBViolationsResponse = new Dictionary<string, bool>();
                List<string> lstECBViolationse = new List<string>();

                var clientBIS = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
                var datasetBIS = clientBIS.GetResource<object>("6bgk-3dad");
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                  SecurityProtocolType.Tls11 |
                  SecurityProtocolType.Tls12;
                var rowsBIS = datasetBIS.GetRows(limit: 1000);
                string qryBIS = string.Empty;
                qryBIS = qryBIS + "bin='" + binnumber.TrimEnd() + "'" + " and " + "ecb_violation_status='" + "ACTIVE" + "'";
                //qryBIS = qryBIS + "ecb_violation_number='" + "39522910K" + "'" + " and " + "ecb_violation_status='" + "ACTIVE" + "'";
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
                        jobViolation = ctx.JobViolations.FirstOrDefault(x => x.SummonsNumber == summonsNumber);
                        lstECBViolationse.Add(summonsNumber);
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
                                var Idrfpaddresses = ctx.RfpAddresses.Where(x => x.BinNumber == binnumber).Select(y => y.Id).ToList();
                                foreach (var r in Idrfpaddresses)
                                {
                                    var jobs = ctx.Jobs.Where(x => x.IdRfpAddress == r).Select(y => y.Id).ToList();
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
                            }
                            else
                            { throw new RpoBusinessException("BIN Anomaly recognized, Violations unable to fetch. Please confirm address details!"); }
                        }
                    }
                }
                UpdateECBCronJob(lstECBViolationse);

            }
        }
        [HttpGet]
        [Route("api/ViolationJobs/GetDOBCronJob/{binnumber}")]
        public void SendDOBNotification(string binnumber)
        {
            using (var ctx = new Model.RpoContext())
            {
                var clientBIS = new SodaClient("https://data.cityofnewyork.us", "7tPQriTSRiloiKdWIJoODgReN");
                var datasetBIS = clientBIS.GetResource<object>("3h2n-5cm9");
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                  SecurityProtocolType.Tls11 |
                  SecurityProtocolType.Tls12;
                var rowsBIS = datasetBIS.GetRows(limit: 1000);
                string qryBIS = string.Empty;
                qryBIS = qryBIS + "bin='" + binnumber.TrimEnd() + "'" + " and " + "violation_category  like '" + "%ACTIVE%" + "'";
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
                                    var Idrfpaddresses = ctx.RfpAddresses.Where(x => x.BinNumber == binnumber).Select(y => y.Id).ToList();
                                    foreach (var r in Idrfpaddresses)
                                    {
                                        var jobs = ctx.Jobs.Where(x => x.IdRfpAddress == r).Select(y => y.Id).ToList();
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
                                }
                                else
                                { throw new RpoBusinessException("BIN Anomaly recognized, Violations unable to fetch. Please confirm address details!"); }
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
                                    commanMessage = commanMessage + " " + "Bin Number was updated to <'" + records.bin + "'>.";

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
                            }
                        }
                    }
                }
            }
        }

        public void UpdateECBCronJob(List<string> lstECBViolationse)
        {
            using (var ctx = new Model.RpoContext())
            {

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
                        string hearingLocation = string.Empty;
                        string violation_description = string.Empty;
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
                    }
                }
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
            public string isn_dob_bis_extract { get; set; }
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

    }
}
