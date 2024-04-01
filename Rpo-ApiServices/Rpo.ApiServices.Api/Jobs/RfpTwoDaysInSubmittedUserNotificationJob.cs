
namespace Rpo.ApiServices.Api.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Controllers.Employees;
    using Controllers.SystemSettings;
    using Quartz;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model.Models;

    public class RfpTwoDaysInSubmittedUserNotificationJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }

        public static void SendMailNotification()
        {
            ApplicationLog.WriteInformationLog("RFP Submitted To User Job Send Mail Notification executed : " + DateTime.Now.ToLongDateString());
            if (Convert.ToBoolean(Properties.Settings.Default.RfpTwoDaysSubmittedUserSchedulerStart))
            {
                ApplicationLog.WriteInformationLog("RFP Submitted To User Job Send Mail Notification execution start : " + DateTime.Now.ToLongDateString());
                using (var ctx = new Model.RpoContext())
                {
                    int statusSubmitted = RfpStatusMaster.Submitted_To_Client.GetHashCode();
                    var twoDaysAgo = DateTime.UtcNow.AddDays(-7);
                    var rfps = ctx.Rfps.Include("CreatedBy")
                        .Where(r => r.IdRfpStatus == statusSubmitted && r.StatusChangedDate <= DbFunctions.TruncateTime(twoDaysAgo))
                        .ToList();

                    if (rfps.Any())
                    {
                        List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                        SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenRFPIsSubmittedToClientForMoreThanAweek);
                        if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                        {
                            foreach (EmployeeDetail item in systemSettingDetail.Value)
                            {
                                cc.Add(new KeyValuePair<string, string>(item.Email, item.Email));
                            }
                        }

                        foreach (Rfp item in rfps)
                        {
                            string body = string.Empty;
                            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/RFPSubmittedNotifyToUserTemplate.htm")))
                            {
                                body = reader.ReadToEnd();
                            }

                            var from = new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP");
                            var to = new KeyValuePair<string, string>(item.CreatedBy.Email, item.CreatedBy.FirstName + " " + item.CreatedBy.LastName);
                            //string bodyemailBody = string.Join("<br />", rfps.Select(r => r.RfpNumber));
                            body = body.Replace("##EmployeeName##", item.CreatedBy.FirstName + " " + item.CreatedBy.LastName);
                            body = body.Replace("##RfpNumber##", item.RfpNumber);
                            body = body.Replace("##SubmittedDate##", TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.StatusChangedDate), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString() + " EST");
                            body = body.Replace("##CompanyName##", item.Company != null ? item.Company.Name : "**Individual**");
                            body = body.Replace("##ContactName##", item.Contact != null ? item.Contact.FirstName + " " + item.Contact.LastName : string.Empty);
                            body = body.Replace("##PhoneNumber##", item.Phone != null ? item.Phone : string.Empty);
                            body = body.Replace("##RFPFullAddress##", item.RfpAddress != null ? item.RfpAddress.HouseNumber + " " + item.RfpAddress.Street + (item.RfpAddress.Borough != null ? " " + item.RfpAddress.Borough.Description : string.Empty) + " " + item.RfpAddress.ZipCode : string.Empty);
                            body = body.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + item.Id);


                            string subject = EmailNotificationSubject.FollowupWithClient.Replace("##RFPNumber##", item.RfpNumber);
                            Mail.Send(from, to, cc, subject, body, true);

                            string followupWithClient = InAppNotificationMessage.FollowupWithClient;
                            followupWithClient = followupWithClient.Replace("##RfpNumber##", item.RfpNumber);
                            followupWithClient = followupWithClient.Replace("##SubmittedDate##", TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.StatusChangedDate), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString() + " EST");
                            followupWithClient = followupWithClient.Replace("##CompanyName##", item.Company != null ? item.Company.Name : "**Individual**");
                            followupWithClient = followupWithClient.Replace("##ContactName##", item.Contact != null ? item.Contact.FirstName + " " + item.Contact.LastName : string.Empty);
                            followupWithClient = followupWithClient.Replace("##RFPFullAddress##", item.RfpAddress != null ? item.RfpAddress.HouseNumber + " " + item.RfpAddress.Street + (item.RfpAddress.Borough != null ? " " + item.RfpAddress.Borough.Description : string.Empty) + " " + item.RfpAddress.ZipCode : string.Empty);
                            followupWithClient = followupWithClient.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + item.Id);

                            Common.SendInAppNotifications(item.CreatedBy.Id, followupWithClient, "/editSiteInformation/" + item.Id);
                        }
                    }
                }
            }
        }
    }
}