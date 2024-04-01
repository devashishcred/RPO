
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

    public class RfpTwoDaysInDraftUserNotificationJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }

        public static void SendMailNotification()
        {
            string javascript = "click=\"redirectFromNotification(j)\"";
            ApplicationLog.WriteInformationLog("RFP In Draft To User Job  Send Mail Notification executed : " + DateTime.Now.ToLongDateString());

            if (Convert.ToBoolean(Properties.Settings.Default.RfpTwoDaysInDraftUserSchedulerStart))
            {
                ApplicationLog.WriteInformationLog("RFP In Draft To User Job  Send Mail Notification execution start : " + DateTime.Now.ToLongDateString());

                using (var ctx = new Model.RpoContext())
                {
                    int statusInDraft = RfpStatusMaster.In_Draft.GetHashCode();
                    int statusOnHold = RfpStatusMaster.On_Hold.GetHashCode();
                    int statusPendingReview = RfpStatusMaster.Pending_Review_By_RPO.GetHashCode();
                    var twoDaysAgo = DateTime.UtcNow.AddDays(-2);
                    var rfps = ctx.Rfps.Include("CreatedBy").Include("RfpStatus")
                        .Where(r => (r.IdRfpStatus == statusInDraft || r.IdRfpStatus == statusOnHold || r.IdRfpStatus == statusPendingReview) && r.StatusChangedDate <= DbFunctions.TruncateTime(twoDaysAgo))

                        .ToList();

                    if (rfps.Any())
                    {
                        List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                        SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenRFPIsIndraftForMoreThan2days);
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
                            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/RFPInDraftNotifyToUserTemplate.htm")))
                            {
                                body = reader.ReadToEnd();
                            }

                            var from = new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP");
                            var to = new KeyValuePair<string, string>(item.CreatedBy.Email, item.CreatedBy.FirstName + " " + item.CreatedBy.LastName);
                            body = body.Replace("##EmployeeName##", item.CreatedBy.FirstName);
                            body = body.Replace("##RfpNumber##", item.RfpNumber);
                            body = body.Replace("##CompanyName##", item.Company != null ? item.Company.Name : "**Individual**");
                            body = body.Replace("##ContactName##", item.Contact != null ? item.Contact.FirstName + " " + item.Contact.LastName : string.Empty);
                            body = body.Replace("##RFPFullAddress##", item.RfpAddress != null ? item.RfpAddress.HouseNumber + " " + item.RfpAddress.Street + (item.RfpAddress.Borough != null ? " " + item.RfpAddress.Borough.Description : string.Empty) + " " + item.RfpAddress.ZipCode : string.Empty);
                            body = body.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/" + item.Id);

                            string subject = EmailNotificationSubject.RFPIsInDraftForMoreThan2Days.Replace("##RfpNumber##", item.RfpNumber);
                            Mail.Send(from, to, cc, subject, body, true);


                            string rfpIsInDraftForMoreThan2Days = InAppNotificationMessage._RFPIsInDraftForMoreThan2Days
                                        .Replace("##RfpNumber##", item.RfpNumber)
                                        .Replace("##CompanyName##", item.Company != null ? item.Company.Name : "**Individual**")
                                        .Replace("##ContactName##", item.Contact != null ? item.Contact.FirstName + " " + item.Contact.LastName : string.Empty)
                                        .Replace("##RFPFullAddress##", item.RfpAddress != null ? item.RfpAddress.HouseNumber + " " + item.RfpAddress.Street + (item.RfpAddress.Borough != null ? " " + item.RfpAddress.Borough.Description : string.Empty) + " " + item.RfpAddress.ZipCode : string.Empty)
                                        .Replace("##RedirectionLink##", javascript); 

                            Common.SendInAppNotifications(item.CreatedBy.Id, rfpIsInDraftForMoreThan2Days, "editSiteInformation/" + item.Id);

                        }
                    }
                }
            }
        }
    }
}