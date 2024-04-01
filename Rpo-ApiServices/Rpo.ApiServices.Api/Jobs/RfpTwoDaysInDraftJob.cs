
namespace Rpo.ApiServices.Api.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Quartz;
    using Rpo.ApiServices.Api.Tools;

    public class RfpTwoDaysInDraftJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }

        public static void SendMailNotification()
        {
            ApplicationLog.WriteInformationLog("RFP In Draft To Admin Job Send Mail Notification executed: " + DateTime.Now.ToLongDateString());

            if (Convert.ToBoolean(Properties.Settings.Default.RFPInDraftNotifyAdminSchedulerStart))
            {
                ApplicationLog.WriteInformationLog("RFP In Draft To Admin Job Send Mail Notification execution start: " + DateTime.Now.ToLongDateString());

                using (var ctx = new Model.RpoContext())
                {
                    int statusInDraft = RfpStatusMaster.In_Draft.GetHashCode();
                    var twoDaysAgo = DateTime.Today.AddDays(-2);
                    var rfps = ctx.Rfps
                        .Where(r => r.IdRfpStatus == statusInDraft && r.LastModifiedDate <= twoDaysAgo)
                        .ToList();

                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/RFPInDraftNotifyAdminTemplate.htm")))
                    {
                        body = reader.ReadToEnd();
                    }

                    if (rfps.Any())
                    {
                        var to = ctx.Employees
                            .Where(e => e.IdGroup == 1)
                            .OrderBy(e => e.Id)
                            .AsEnumerable()
                            .Select(e => new KeyValuePair<string, string>(e.Email, e.FirstName + " " + e.LastName))
                            .ToList();

                        if (to.Any())
                        {
                            var from = new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP");
                            foreach (KeyValuePair<string, string> item in to)
                            {
                                string bodyemailBody = string.Join("<br />", rfps.Select(r => "<a href=\""+Properties.Settings.Default.FrontEndUrl + "/editSiteInformation/"+ r.Id +" \">RFP# "+ r.RfpNumber + "</a>"));
                                body = body.Replace("##EmployeeName##", item.Value);
                                body = body.Replace("##RFPNumberList##", bodyemailBody);
                                Mail.Send(from, item, "RFP pending since 2 days", body, true);
                            }
                        }
                    }
                }
            }
        }
    }
}