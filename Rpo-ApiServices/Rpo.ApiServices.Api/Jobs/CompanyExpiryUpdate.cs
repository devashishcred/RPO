
namespace Rpo.ApiServices.Api.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Controllers.Companies;
    using Controllers.JobViolations;
    using Controllers.SystemSettings;
    using HtmlAgilityPack;
    using Model;
    using Model.Models;
    using Quartz;
    using Rpo.ApiServices.Api.Tools;
    using Controllers.Jobs;
    using System.Data.Entity;

    public class CompanyExpiryUpdate : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }

        public CompanyExpiryUpdate()
        {
            SendMailNotification();
        }

        public static void SendMailNotification()
        {
            ApplicationLog.WriteInformationLog("Company Expiry Update Result Send Mail Notification executed: " + DateTime.Now.ToLongDateString());

            //Get the summonsNoticeNumber from Database.
            using (var ctx = new Model.RpoContext())
            {
                List<int> ids = new List<int>() { 11, 13, 27 };

                //   List<Company> liCompany = ctx.Companies.Where(x => x.CompanyTypes.Any(c => ids.Contains(c.Id))).ToList();  // Company logic change next 15 days expiry those company list
                // List<Company> liCompany = ctx.Companies.Where(x => x.Id==4410).ToList();

                DateTime CurrentDate = DateTime.Now;
                DateTime Currentnext15daysDate = DateTime.Now.AddDays(15);

                List<Company> liCompany = ctx.Companies.Where(x => x.CompanyTypes.Any(c => ids.Contains(c.Id)) && (
                                                                  (DbFunctions.TruncateTime(x.TrackingExpiry) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.TrackingExpiry) <= DbFunctions.TruncateTime(Currentnext15daysDate))
                                                                ||
                                                                     (DbFunctions.TruncateTime(x.InsuranceWorkCompensation) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.InsuranceWorkCompensation) <= DbFunctions.TruncateTime(Currentnext15daysDate))
                                                                ||
                                                                     (DbFunctions.TruncateTime(x.InsuranceDisability) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.InsuranceDisability) <= DbFunctions.TruncateTime(Currentnext15daysDate))
                                                                ||
                                                                     (DbFunctions.TruncateTime(x.InsuranceGeneralLiability) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.InsuranceGeneralLiability) <= DbFunctions.TruncateTime(Currentnext15daysDate))
                                                                ||
                                                                     (DbFunctions.TruncateTime(x.SpecialInspectionAgencyExpiry) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.SpecialInspectionAgencyExpiry) <= DbFunctions.TruncateTime(Currentnext15daysDate))
                                                                ||
                                                                     (DbFunctions.TruncateTime(x.CTExpirationDate) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.CTExpirationDate) <= DbFunctions.TruncateTime(Currentnext15daysDate))

                                                                     //||
                                                                     //     (DbFunctions.TruncateTime(x.InsuranceObstructionBond) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.InsuranceObstructionBond) <= DbFunctions.TruncateTime(Currentnext15daysDate))
                                                                     //||
                                                                     //     (DbFunctions.TruncateTime(x.DOTInsuranceGeneralLiability) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.DOTInsuranceGeneralLiability) <= DbFunctions.TruncateTime(Currentnext15daysDate))
                                                                     //||
                                                                     //     (DbFunctions.TruncateTime(x.DOTInsuranceWorkCompensation) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.DOTInsuranceWorkCompensation) <= DbFunctions.TruncateTime(Currentnext15daysDate))

                                                                     )).ToList();


                //Iterate over each Summons Number and fetch the data from site
                foreach (var itemCompany in liCompany)
                {
                    int companyId = itemCompany.Id;
                    ApplicationLog.WriteInformationLog("Enter into Loop Company Id: " + Convert.ToString(companyId));
                    bool IsSpecialInspection = itemCompany.CompanyTypes.Any(c => c.Id == 11);
                    bool IsGeneralContractor = itemCompany.CompanyTypes.Any(c => c.Id == 13);
                    bool IsConcreteTestingLab = itemCompany.CompanyTypes.Any(c => c.Id == 27);

                    ApplicationLog.WriteInformationLog("IsSpecialInspection: " + Convert.ToString(IsSpecialInspection));
                    ApplicationLog.WriteInformationLog("IsGeneralContractor: " + Convert.ToString(IsGeneralContractor));
                    ApplicationLog.WriteInformationLog("IsConcreteTestingLab: " + Convert.ToString(IsConcreteTestingLab));


                    if (IsSpecialInspection) //I
                    {
                        if (!String.IsNullOrEmpty(itemCompany.SpecialInspectionAgencyNumber))
                        {
                            BisDTO returnSpecialInspectionObj = GetBis("I", itemCompany.SpecialInspectionAgencyNumber);

                            if (returnSpecialInspectionObj != null)
                            {

                                DateTime? previousSpecialInspectionAgencyExpiry = itemCompany.SpecialInspectionAgencyExpiry;

                                if (returnSpecialInspectionObj.SpecialInspectionAgencyExpiry != null
                                    && returnSpecialInspectionObj.SpecialInspectionAgencyExpiry != previousSpecialInspectionAgencyExpiry)
                                {
                                    itemCompany.SpecialInspectionAgencyExpiry = returnSpecialInspectionObj.SpecialInspectionAgencyExpiry;

                                    SendCompanyExpiryUpdatedNotification("Special Inspection Agency Expiry Date",
                                                                          companyId,
                                                                          itemCompany.Name,
                                                                          previousSpecialInspectionAgencyExpiry != null ? previousSpecialInspectionAgencyExpiry.Value.ToString("dd-MM-yyyy") : "-",
                                                                          returnSpecialInspectionObj.SpecialInspectionAgencyExpiry.Value.ToString("dd-MM-yyyy")
                                                                         );
                                }
                            }
                        }
                    }

                    if (IsGeneralContractor) //G
                    {
                        ApplicationLog.WriteInformationLog("In Is general Contractor: ");
                        if (!String.IsNullOrEmpty(itemCompany.TrackingNumber))
                        {
                            ApplicationLog.WriteInformationLog("Tracking Number: " + Convert.ToString(itemCompany.TrackingNumber));
                            BisDTO returnGeneralContractorObj = GetBis("G", itemCompany.TrackingNumber);

                            if (returnGeneralContractorObj != null)
                            {
                                ApplicationLog.WriteInformationLog("In return object");
                                DateTime? previousTrackingExpiry = itemCompany.TrackingExpiry;
                                DateTime? previousInsuranceWorkCompensation = itemCompany.InsuranceWorkCompensation;
                                DateTime? previousInsuranceDisability = itemCompany.InsuranceDisability;
                                DateTime? previousInsuranceGeneralLiability = itemCompany.InsuranceGeneralLiability;
                                DateTime? previousInsuranceObstructionBond = itemCompany.InsuranceObstructionBond;

                                ApplicationLog.WriteInformationLog(previousTrackingExpiry != null ?
                                                                    previousTrackingExpiry.Value.ToString("dd-MM-yyyy") : "No date found");
                                if (returnGeneralContractorObj.GeneralContractorExpiry != null &&
                                    returnGeneralContractorObj.GeneralContractorExpiry != previousTrackingExpiry)
                                {
                                    ApplicationLog.WriteInformationLog("In Previous Tracking Expiry");
                                    itemCompany.TrackingExpiry = returnGeneralContractorObj.GeneralContractorExpiry;
                                    ApplicationLog.WriteInformationLog("New Tracking Expiry" + itemCompany.TrackingExpiry.Value.ToString("dd-MM-yyyy"));
                                    //SendCompanyExpiryUpdatedNotification("Tracking Expiry",
                                    //                                      companyId,
                                    //                                      itemCompany.Name,
                                    //                                      previousTrackingExpiry != null ? previousTrackingExpiry.Value.ToString("dd-MM-yyyy") : "-",
                                    //                                      returnGeneralContractorObj.GeneralContractorExpiry.Value.ToString("dd-MM-yyyy")
                                    //                                     );
                                }


                                if (returnGeneralContractorObj.InsuranceWorkCompensation != null
                                    && returnGeneralContractorObj.InsuranceWorkCompensation != previousInsuranceWorkCompensation)
                                {
                                    itemCompany.InsuranceWorkCompensation = returnGeneralContractorObj.InsuranceWorkCompensation;
                                    //SendCompanyExpiryUpdatedNotification("Insurance Work Compensation",
                                    //                                      companyId,
                                    //                                      itemCompany.Name,
                                    //                                      previousInsuranceWorkCompensation != null ? previousInsuranceWorkCompensation.Value.ToString("dd-MM-yyyy") : "-",
                                    //                                      returnGeneralContractorObj.InsuranceWorkCompensation.Value.ToString("dd-MM-yyyy")
                                    //                                     );
                                }


                                if (returnGeneralContractorObj.InsuranceDisability != null &&
                                    returnGeneralContractorObj.InsuranceDisability != previousInsuranceDisability)
                                {
                                    itemCompany.InsuranceDisability = returnGeneralContractorObj.InsuranceDisability;
                                    //SendCompanyExpiryUpdatedNotification("Insurance Disability",
                                    //                                      companyId,
                                    //                                      itemCompany.Name,
                                    //                                      previousInsuranceDisability != null ? previousInsuranceDisability.Value.ToString("dd-MM-yyyy") : "-",
                                    //                                      returnGeneralContractorObj.InsuranceDisability.Value.ToString("dd-MM-yyyy")
                                    //                                     );
                                }


                                if (returnGeneralContractorObj.InsuranceGeneralLiability != null && returnGeneralContractorObj.InsuranceGeneralLiability != previousInsuranceGeneralLiability)
                                {
                                    itemCompany.InsuranceGeneralLiability = returnGeneralContractorObj.InsuranceGeneralLiability;
                                    //SendCompanyExpiryUpdatedNotification("Insurance General Liabiltiy",
                                    //                                      companyId,
                                    //                                      itemCompany.Name,
                                    //                                      previousInsuranceGeneralLiability != null ? previousInsuranceGeneralLiability.Value.ToString("dd-MM-yyyy") : "-",
                                    //                                      returnGeneralContractorObj.InsuranceGeneralLiability.Value.ToString("dd-MM-yyyy")
                                    //                                     );
                                }



                                if (returnGeneralContractorObj.InsuranceObstructionBond != null && returnGeneralContractorObj.InsuranceObstructionBond != previousInsuranceObstructionBond)
                                {
                                    itemCompany.InsuranceObstructionBond = returnGeneralContractorObj.InsuranceObstructionBond;
                                    //SendCompanyExpiryUpdatedNotification("Insurance Obstruction Bond",
                                    //                                      companyId,
                                    //                                      itemCompany.Name,
                                    //                                      previousInsuranceObstructionBond != null ? previousInsuranceObstructionBond.Value.ToString("dd-MM-yyyy") : "-",
                                    //                                      returnGeneralContractorObj.InsuranceObstructionBond.Value.ToString("dd-MM-yyyy")
                                    //                                     );
                                }
                                SendCompanyExpiryUpdatedNotification("",
                                                                          companyId,
                                                                          itemCompany.Name,
                                                                          "-",
                                                                          "-"
                                                                         );
                            }
                        }
                    }

                    if (IsConcreteTestingLab) //C
                    {
                        if (!String.IsNullOrEmpty(itemCompany.CTLicenseNumber))
                        {
                            BisDTO returnConcerteObj = GetBis("C", itemCompany.CTLicenseNumber);

                            if (returnConcerteObj != null)
                            {
                                DateTime? previousCTExpirationDate = itemCompany.CTExpirationDate;

                                if (returnConcerteObj.ConcreteTestingLabExpiry != null && returnConcerteObj.ConcreteTestingLabExpiry != previousCTExpirationDate)
                                {
                                    itemCompany.CTExpirationDate = returnConcerteObj.ConcreteTestingLabExpiry;
                                    SendCompanyExpiryUpdatedNotification("CT Expiry Date",
                                                                          companyId,
                                                                          itemCompany.Name,
                                                                          previousCTExpirationDate != null ? previousCTExpirationDate.Value.ToString("dd-MM-yyyy") : "-",
                                                                          returnConcerteObj.ConcreteTestingLabExpiry.Value.ToString("dd-MM-yyyy")
                                                                         );
                                }
                            }

                        }
                    }
                    //String TrackingNumber = itemCompany.TrackingNumber;
                    //String CTLicenseNumber = itemCompany.CTLicenseNumber;
                    //String SpecialInspectionAgencyNumber = itemCompany.SpecialInspectionAgencyNumber;



                    ////-----------------Fetch the data from Web Site-----------------//
                    //var request = (HttpWebRequest)WebRequest.Create("http://a820-ecbticketfinder.nyc.gov/getViolationbyID.action");

                    //string actualSummonsNumber = summonsNoticeNumber;
                    //summonsNoticeNumber = "0" + summonsNoticeNumber;

                    //var postData = "searchViolationObject.violationNo=" + summonsNoticeNumber;
                    //postData += "&searchViolationObject.searchOptions=All";
                    //postData += "&submit=Search";
                    //postData += "&searchViolationObject.searchType=violationNumber";

                    //var data = Encoding.ASCII.GetBytes(postData);

                    //request.Method = "POST";
                    //request.ContentType = "application/x-www-form-urlencoded";
                    //request.ContentLength = data.Length;

                    //using (var stream = request.GetRequestStream())
                    //{
                    //    stream.Write(data, 0, data.Length);
                    //}

                    //var response = (HttpWebResponse)request.GetResponse();

                    //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    //JobViolationGetInfoResponse violationGetInfoResponse = new JobViolationGetInfoResponse();

                    //var doc = new HtmlDocument();
                    //doc.LoadHtml(responseString);

                    //var descendants = doc.DocumentNode.Descendants();

                    //var summonsNumber = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Summons/Notice Number:	");
                    //var dateIssued = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Date Issued:");
                    //var issuingAgency = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Issuing Agency:");
                    //var respondentName = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Respondent Name:");

                    //var balanceDue = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Balance Due:");
                    //var inspectionLocation = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Inspection Location:");
                    //var respondentAddress = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Respondent Address:");
                    //var statusOfSummonsNotice = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Status of Summons/Notice:	");

                    //var hearingResult = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Hearing Result:	");
                    //var hearingLocation = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Hearing Location:	");
                    //var hearingDate = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Hearing Date:	");

                    //var code = descendants.FirstOrDefault(n => n.Name == "th" && n.InnerText == "Code");

                    //if (summonsNumber != null)
                    //{
                    //    violationGetInfoResponse.SummonsNumber = summonsNumber.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.SummonsNumber = !string.IsNullOrEmpty(violationGetInfoResponse.SummonsNumber) ? Regex.Replace(violationGetInfoResponse.SummonsNumber, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;

                    //}

                    //if (violationGetInfoResponse.SummonsNumber != summonsNoticeNumber)
                    //{
                    //    throw new RpoBusinessException(StaticMessages.ViolationNotFound);
                    //}

                    //summonsNoticeNumber = actualSummonsNumber;
                    //List<ExplanationOfCharge> explanationOfCharges = new List<ExplanationOfCharge>();
                    //if (code != null)
                    //{
                    //    HtmlNode table = doc.DocumentNode.SelectSingleNode("//*[@id='vioInfrAccordion']");
                    //    table = table.ChildNodes.FirstOrDefault(x => x.Id == "infraDetails");
                    //    table = table.ChildNodes.FirstOrDefault(x => x.Id == "details");
                    //    int rowindex = 0;

                    //    foreach (HtmlNode row in table.ChildNodes.Where(x => x.Name == "tr"))
                    //    {
                    //        if (rowindex != 0)
                    //        {
                    //            foreach (HtmlNode childRow in row.ChildNodes.Where(x => x.Name == "tr"))
                    //            {
                    //                HtmlNode item = childRow.ChildNodes.Where(x => x.Name == "tr") != null && childRow.ChildNodes.Where(x => x.Name == "tr").Count() > 0 ? childRow.ChildNodes.FirstOrDefault(x => x.Name == "tr") : childRow;

                    //                ExplanationOfCharge explanationOfCharge = new ExplanationOfCharge();
                    //                int cellindex = 0;
                    //                foreach (HtmlNode cell in item.ChildNodes.Where(x => x.Name == "td"))
                    //                {
                    //                    switch (cellindex)
                    //                    {
                    //                        case 0:
                    //                            explanationOfCharge.Code = cell.InnerText;
                    //                            explanationOfCharge.Code = !string.IsNullOrEmpty(explanationOfCharge.Code) ? Regex.Replace(explanationOfCharge.Code, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    //                            break;
                    //                        case 1:
                    //                            explanationOfCharge.CodeSection = cell.InnerText;
                    //                            explanationOfCharge.CodeSection = !string.IsNullOrEmpty(explanationOfCharge.CodeSection) ? Regex.Replace(explanationOfCharge.CodeSection, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    //                            break;
                    //                        case 2:
                    //                            explanationOfCharge.Description = cell.InnerText;
                    //                            explanationOfCharge.Description = !string.IsNullOrEmpty(explanationOfCharge.Description) ? Regex.Replace(explanationOfCharge.Description, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    //                            break;
                    //                        case 3:
                    //                            explanationOfCharge.PaneltyAmount = cell.InnerText;
                    //                            explanationOfCharge.PaneltyAmount = !string.IsNullOrEmpty(explanationOfCharge.PaneltyAmount) ? Regex.Replace(explanationOfCharge.PaneltyAmount, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    //                            explanationOfCharge.PaneltyAmount = explanationOfCharge.PaneltyAmount.Replace("$", "");
                    //                            break;
                    //                        default:
                    //                            break;
                    //                    }

                    //                    cellindex++;
                    //                }
                    //                explanationOfCharge.IsFromAuth = true;
                    //                explanationOfCharges.Add(explanationOfCharge);
                    //            }
                    //        }
                    //        rowindex++;
                    //    }
                    //    violationGetInfoResponse.ExplanationOfCharges = explanationOfCharges;
                    //}

                    //if (dateIssued != null)
                    //{
                    //    violationGetInfoResponse.DateIssued = dateIssued.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.DateIssued = !string.IsNullOrEmpty(violationGetInfoResponse.DateIssued) ? Regex.Replace(violationGetInfoResponse.DateIssued, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;


                    //}

                    //if (issuingAgency != null)
                    //{
                    //    violationGetInfoResponse.IssuingAgency = issuingAgency.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.IssuingAgency = !string.IsNullOrEmpty(violationGetInfoResponse.IssuingAgency) ? Regex.Replace(violationGetInfoResponse.IssuingAgency, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    //}

                    //if (respondentName != null)
                    //{
                    //    violationGetInfoResponse.RespondentName = respondentName.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.RespondentName = !string.IsNullOrEmpty(violationGetInfoResponse.RespondentName) ? Regex.Replace(violationGetInfoResponse.RespondentName, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    //}

                    //if (balanceDue != null)
                    //{
                    //    violationGetInfoResponse.BalanceDue = balanceDue.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.BalanceDue = !string.IsNullOrEmpty(violationGetInfoResponse.BalanceDue) ? Regex.Replace(violationGetInfoResponse.BalanceDue, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;

                    //    if (!string.IsNullOrEmpty(violationGetInfoResponse.BalanceDue) && violationGetInfoResponse.BalanceDue.Contains("$"))
                    //    {
                    //        violationGetInfoResponse.BalanceDue = violationGetInfoResponse.BalanceDue.Replace("$", "");
                    //    }

                    //    itemSummons.BalanceDue = String.IsNullOrEmpty(violationGetInfoResponse.BalanceDue) == true ? (Double?)null : Convert.ToDouble(violationGetInfoResponse.BalanceDue);
                    //}

                    //if (inspectionLocation != null)
                    //{
                    //    violationGetInfoResponse.InspectionLocation = inspectionLocation.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.InspectionLocation = !string.IsNullOrEmpty(violationGetInfoResponse.InspectionLocation) ? Regex.Replace(violationGetInfoResponse.InspectionLocation, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    //}

                    //if (respondentAddress != null)
                    //{
                    //    violationGetInfoResponse.RespondentAddress = respondentAddress.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.RespondentAddress = !string.IsNullOrEmpty(violationGetInfoResponse.RespondentAddress) ? Regex.Replace(violationGetInfoResponse.RespondentAddress, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    //}

                    //if (statusOfSummonsNotice != null)
                    //{
                    //    String Previous_StatusOfSummonsNotice = itemSummons.StatusOfSummonsNotice;

                    //    violationGetInfoResponse.StatusOfSummonsNotice = statusOfSummonsNotice.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.StatusOfSummonsNotice = !string.IsNullOrEmpty(violationGetInfoResponse.StatusOfSummonsNotice) ? Regex.Replace(violationGetInfoResponse.StatusOfSummonsNotice, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;

                    //    itemSummons.StatusOfSummonsNotice = violationGetInfoResponse.StatusOfSummonsNotice;
                    //    if (Convert.ToString(Previous_StatusOfSummonsNotice).ToUpper() != Convert.ToString(itemSummons.StatusOfSummonsNotice).ToUpper())
                    //    {
                    //        SendViolationUpdatedSNotification(Convert.ToInt32(itemSummons.IdJob), summonsNoticeNumber, Previous_StatusOfSummonsNotice, itemSummons.StatusOfSummonsNotice, true);
                    //    }
                    //}

                    //if (hearingResult != null)
                    //{
                    //    violationGetInfoResponse.HearingResult = hearingResult.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.HearingResult = !string.IsNullOrEmpty(violationGetInfoResponse.HearingResult) ? Regex.Replace(violationGetInfoResponse.HearingResult, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;

                    //    itemSummons.HearingResult = violationGetInfoResponse.HearingResult;
                    //}

                    //if (hearingLocation != null)
                    //{
                    //    violationGetInfoResponse.HearingLocation = hearingLocation.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.HearingLocation = !string.IsNullOrEmpty(violationGetInfoResponse.HearingLocation) ? Regex.Replace(violationGetInfoResponse.HearingLocation, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;

                    //    itemSummons.HearingLocation = violationGetInfoResponse.HearingLocation;
                    //}

                    //if (hearingDate != null)
                    //{
                    //    DateTime? HearingDateBeforeChange = itemSummons.HearingDate;
                    //    violationGetInfoResponse.HearingDate = hearingDate.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.HearingDate = !string.IsNullOrEmpty(violationGetInfoResponse.HearingDate) ? Regex.Replace(violationGetInfoResponse.HearingDate, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;

                    //    itemSummons.HearingDate = String.IsNullOrEmpty(violationGetInfoResponse.HearingDate) == true ? (DateTime?)null : Convert.ToDateTime(Common.ChangeDateTimeFormat(violationGetInfoResponse.HearingDate));

                    //    if ((HearingDateBeforeChange - itemSummons.HearingDate).Value.Days != 0)
                    //    {
                    //        SendViolationUpdatedSNotification(Convert.ToInt32(itemSummons.IdJob), summonsNoticeNumber, HearingDateBeforeChange.Value.ToString("dd-MM-yyyy"), itemSummons.HearingDate.Value.ToString("dd-MM-yyyy"), false);
                    //    }
                    //}

                    //string bisURL = $"http://a810-bisweb.nyc.gov/bisweb/ECBQueryByNumberServlet?ecbin={summonsNoticeNumber}&go7=+GO+&requestid=0";

                    //HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlWeb().Load(bisURL);

                    //var bisDescendants = htmlDocument.DocumentNode.Descendants();

                    //var certificationStatusLabel = bisDescendants.FirstOrDefault(n => n.Name == "td" && n.InnerHtml.Contains("Certification Status:"));
                    //var complianceOnLabel = bisDescendants.FirstOrDefault(n => n.Name == "td" && n.InnerHtml.Contains("Compliance On:"));
                    //var violationresolved = bisDescendants.FirstOrDefault(n => n.Name == "td" && n.InnerHtml.Contains("ECB Violation Summary"));

                    //if (certificationStatusLabel != null && certificationStatusLabel.ParentNode != null && certificationStatusLabel.ParentNode.ChildNodes != null
                    //    && certificationStatusLabel.ParentNode.ChildNodes.Count > 5)
                    //{
                    //    string certificationStatus = certificationStatusLabel.ParentNode.ChildNodes[5].InnerHtml;
                    //    violationGetInfoResponse.CertificationStatus = certificationStatus;
                    //    violationGetInfoResponse.CertificationStatus = !string.IsNullOrEmpty(violationGetInfoResponse.CertificationStatus) ? Regex.Replace(violationGetInfoResponse.CertificationStatus, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    //}

                    //if (complianceOnLabel != null)
                    //{
                    //    string complianceOn = complianceOnLabel.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    violationGetInfoResponse.ComplianceOn = complianceOn;
                    //    violationGetInfoResponse.ComplianceOn = !string.IsNullOrEmpty(violationGetInfoResponse.ComplianceOn) ? Regex.Replace(violationGetInfoResponse.ComplianceOn, @"<[^>]+>|&nbsp;", "").Trim() : string.Empty;
                    //}

                    //if (violationresolved != null)
                    //{
                    //    string resolvedStatus = violationresolved.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText;
                    //    if (resolvedStatus.Contains("RESOLVED"))
                    //    {
                    //        violationGetInfoResponse.IsFullyResolved = true;
                    //    }
                    //    else
                    //    {
                    //        violationGetInfoResponse.IsFullyResolved = false;
                    //    }
                    //}



                    //------------------End Fetch the data from Web Site-----------//




                }//ForEach Loop Over

                ctx.SaveChanges();
            }
        }

        public static BisDTO GetBis(string licensetype, string licno)
        {
            var result = new BisDTO();

            string urlAddress = $"http://a810-bisweb.nyc.gov/bisweb/LicenseQueryServlet?licensetype={licensetype}&licno={licno}&requestid=1";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            HtmlDocument doc = new HtmlWeb().Load(urlAddress);

            var descendants = doc.DocumentNode.Descendants();

            if (descendants.Any(n => n.InnerText == "LICENSE RECORD NOT FOUND"))
                return null;

            var insuranceGeneralLiability = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "General Liability");

            var insuranceWorkCompensation = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Workers' Compensation");

            var insuranceDisability = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText == "Disability");

            var generalContractorExpiry = descendants.FirstOrDefault(n => n.Name == "b" && n.InnerText == "Expiration:");

            var concreteTestingLabExpiry = descendants.FirstOrDefault(n => n.Name == "b" && n.InnerText == "Expiration:");

            var specialInspectionAgencyExpiry = descendants.FirstOrDefault(n => n.Name == "b" && n.InnerText == "Expiration:");

            var concreteTestingLabNumber = descendants.FirstOrDefault(n => n.Name == "b" && n.InnerText == "License #:");

            var specialInspectionAgencyNumber = descendants.FirstOrDefault(n => n.Name == "b" && n.InnerText == "License #:");

            var endorsementsDemolition = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText.Contains("DM - DEMOLITION"));
            var endorsementsConcrete = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText.Contains("CC - CONCRETE"));
            var endorsementsConstruction = descendants.FirstOrDefault(n => n.Name == "td" && n.InnerText.Contains("CN - CONSTRUCTION"));



            DateTime dt;

            if (insuranceGeneralLiability != null && DateTime.TryParseExact(insuranceGeneralLiability.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                result.InsuranceGeneralLiability = dt;
            }

            if (insuranceWorkCompensation != null && DateTime.TryParseExact(insuranceWorkCompensation.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                result.InsuranceWorkCompensation = dt;
            }

            if (insuranceDisability != null && DateTime.TryParseExact(insuranceDisability.ParentNode.ChildNodes.Last(c => c.Name == "td").InnerText, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                result.InsuranceDisability = dt;
            }

            if (licensetype == "G" && concreteTestingLabExpiry != null)
            {
                string generalContractor = Convert.ToString(generalContractorExpiry.ParentNode.InnerText).Replace("Expiration:&nbsp;&nbsp;", "");
                if (DateTime.TryParseExact(generalContractor, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    result.GeneralContractorExpiry = dt;
                }
            }

            if (licensetype == "C" && concreteTestingLabExpiry != null)
            {
                string concreteTestingLab = Convert.ToString(concreteTestingLabExpiry.ParentNode.InnerText).Replace("Expiration:&nbsp;&nbsp;", "");
                if (DateTime.TryParseExact(concreteTestingLab, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    result.ConcreteTestingLabExpiry = dt;
                }
            }

            if (licensetype == "C" && concreteTestingLabNumber != null)
            {
                string concreteTestingLab = Convert.ToString(concreteTestingLabNumber.ParentNode.InnerText).Replace("License #:&nbsp;&nbsp;", "");
                result.ConcreteTestingLabNumber = concreteTestingLab;
            }

            if (licensetype == "I" && specialInspectionAgencyExpiry != null)
            {
                string specialInspectionAgency = Convert.ToString(specialInspectionAgencyExpiry.ParentNode.InnerText).Replace("Expiration:&nbsp;&nbsp;", "");
                if (DateTime.TryParseExact(specialInspectionAgency, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    result.SpecialInspectionAgencyExpiry = dt;
                }
            }

            if (licensetype == "I" && specialInspectionAgencyNumber != null)
            {
                string specialInspectionAgency = Convert.ToString(specialInspectionAgencyNumber.ParentNode.InnerText).Replace("License #:&nbsp;&nbsp;", "");
                result.SpecialInspectionAgencyNumber = specialInspectionAgency;
            }

            if (endorsementsDemolition != null)
            {
                result.EndorsementsDemolition = true;
            }

            if (endorsementsConcrete != null)
            {
                result.EndorsementsConcrete = true;
            }

            if (endorsementsConstruction != null)
            {
                result.EndorsementsConstruction = true;
            }

            return result;


        }



        public static void SendCompanyExpiryUpdatedNotification(String DateName, int CompanyId, String CompanyName, string previousDate, string NewDate)
        {
            var rpoContext = new Model.RpoContext();
            // List<Job> jobsList = rpoContext.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == CompanyId).ToList();

            List<int> jobAssignList = new List<int>();
            jobAssignList.AddRange(rpoContext.Jobs.Where(x => x.Status == Model.Models.Enums.JobStatus.Active && x.IdCompany == CompanyId).Select(d => d.Id).ToList());
            jobAssignList.AddRange(rpoContext.JobContacts.Where(x => x.IdCompany == CompanyId).Select(d => d.IdJob).ToList());

            List<Job> jobfinalList = rpoContext.Jobs.Where(x => jobAssignList.Contains(x.Id)).Distinct().ToList();
            List<JobAssign> Teams = new List<JobAssign>();
            string jobLinks = string.Empty;

            foreach (var itemjob in jobfinalList)
            {
                string setRedirecturl = string.Empty;

                if (itemjob.Id != 0)
                {
                    setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + itemjob.Id + "" + "/application;idJobAppType=1";
                }

                jobLinks = jobLinks + "<a href=\"##RedirectionLinkjob##\" >##jobNumber##</a> ".Replace("##RedirectionLinkjob##", setRedirecturl).Replace("##jobNumber##", itemjob.JobNumber) + ",";

                Teams.AddRange(SendNotificationToJObProjectTeam(itemjob.Id));
            }


            jobLinks = !string.IsNullOrEmpty(jobLinks) ? jobLinks.Remove(jobLinks.Length - 1, 1) : string.Empty;

            string newJobScopeAddedSetting = InAppNotificationMessage._WhenCompanyExpiryDateUpdated
                        .Replace("##CompanyName##", CompanyName)
                        .Replace("##Jobs##", jobLinks)
                        .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "companydetail/" + CompanyId);

            foreach (var item in Teams)
            {
                if (item.IdEmployee != null)
                {
                    Common.SendInAppNotifications(item.IdEmployee.Value, newJobScopeAddedSetting, "companydetail/" + CompanyId);
                }
            }

            SystemSettingDetail systemSettingDetail;

            systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenCompanyExpiryDateUpdated);

            if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
            {
                foreach (Controllers.Employees.EmployeeDetail employeeDetail in systemSettingDetail.Value)
                {
                    var to = new List<KeyValuePair<string, string>>();

                    to.Add(new KeyValuePair<string, string>(employeeDetail.Email, employeeDetail.FirstName + " " + employeeDetail.LastName));
                    var cc = new List<KeyValuePair<string, string>>();

                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                    {
                        body = reader.ReadToEnd();
                    }

                    string emailBody = body;
                    string Attentation = "<p style='padding: 0px; font-family: sans-serif; font-size: 14px; color: rgb(0, 0, 0); text-align: left; line-height: 22px; margin: 15px 0px;'>Hi ##EmployeeName##,</p>";
                    Attentation = Attentation.Replace("##EmployeeName##", employeeDetail.FirstName + " " + employeeDetail.LastName);

                    emailBody = emailBody.Replace("##EmailBody##", Attentation + newJobScopeAddedSetting);

                    Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"),
                          to,
                          cc,
                          "Company of type (GC/SIA/CTL) Tracking# and Insurance dates updated by the system.",
                          emailBody,
                          true
                      );
                }
            }
        }

        private static List<JobAssign> SendNotificationToJObProjectTeam(int jobId)
        {
            List<JobAssign> jobAssignList = new List<JobAssign>();
            JobsController jobObj = new JobsController();
            using (var ctx = new Model.RpoContext())
            {
                Job jobs = ctx.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == jobId).FirstOrDefault();

                if (jobs.IdProjectManager != null && jobs.IdProjectManager > 0)
                {
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = jobs.IdProjectManager,
                        JobAplicationType = "DOB",
                    });
                }

                if (jobs.DOBProjectTeam != null && !string.IsNullOrEmpty(jobs.DOBProjectTeam))
                {
                    foreach (var item in jobs.DOBProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(new JobAssign
                        {
                            IdEmployee = employeeid,
                            JobAplicationType = "DOB",
                        });
                    }
                }
                if (jobs.DOTProjectTeam != null && !string.IsNullOrEmpty(jobs.DOTProjectTeam))
                {
                    foreach (var item in jobs.DOTProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(new JobAssign
                        {
                            IdEmployee = employeeid,
                            JobAplicationType = "DOT",
                        });
                    }
                }
                if (jobs.ViolationProjectTeam != null && !string.IsNullOrEmpty(jobs.ViolationProjectTeam))
                {
                    foreach (var item in jobs.ViolationProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(new JobAssign
                        {
                            IdEmployee = employeeid,
                            JobAplicationType = "Violation",
                        });
                    }
                }
                if (jobs.DEPProjectTeam != null && !string.IsNullOrEmpty(jobs.DEPProjectTeam))
                {
                    foreach (var item in jobs.DEPProjectTeam.Split(','))
                    {
                        int employeeid = Convert.ToInt32(item);
                        jobAssignList.Add(new JobAssign
                        {
                            IdEmployee = employeeid,
                            JobAplicationType = "DEP",
                        });
                    }
                }
                ctx.SaveChanges();
            }
            return jobAssignList;
        }
    }
}



