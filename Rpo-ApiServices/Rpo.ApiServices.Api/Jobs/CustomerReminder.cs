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
    using System.Data.Entity;

    public class CustomerReminder : IJob
    {

        private RpoContext rpoContext = new RpoContext();
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }
        //public CustomerReminder()
        //{
        //    SendMailNotification();
        //}
        public static void SendMailNotification()
        {
            using (var ctx = new Model.RpoContext())
            {
                try
                {
                    string body = string.Empty;
                    string emailtemplatepath = AppDomain.CurrentDomain.BaseDirectory + "EmailTemplate/reminderMailer.html";
                    using (StreamReader reader = new StreamReader(emailtemplatepath))
                    {
                        body = reader.ReadToEnd();
                    }
                    string emailBody = body;
                    var customerInvitation = ctx.CustomerInvitationStatus.Where(x => x.CUI_Invitatuionstatus == 1 && (x.InvitationSentCount > 0 && x.InvitationSentCount < 3) && x.ReminderCount < 2).ToList();
                    if (customerInvitation.Count > 0)
                    {
                        foreach (var item in customerInvitation)
                        {
                            DateTime createDate = Convert.ToDateTime(item.CreatedDate);
                            DateTime UpdateDate = DateTime.UtcNow;
                            if (item.UpdatedDate.ToString() != null && item.UpdatedDate.ToString() != "")
                            {
                                UpdateDate = Convert.ToDateTime(item.UpdatedDate);
                            }
                            DateTime endTime = DateTime.Now.AddSeconds(75);

                            TimeSpan totalHours;
                            var contactInfo = (from ci in ctx.CustomerInvitationStatus
                                               join c in ctx.Contacts on ci.IdContact equals c.Id
                                               where ci.IdContact == item.IdContact
                                               select new { Name = c.FirstName + " " + c.LastName, Phone = c.WorkPhone }).FirstOrDefault();
                            int reminderCount = Convert.ToInt32(item.ReminderCount);
                            if (reminderCount == 0)
                            { totalHours = endTime.Subtract(createDate); }
                            else
                            { totalHours = endTime.Subtract(UpdateDate); }

                            if (totalHours.TotalHours > 48)
                            {
                                CustomerInvitationStatus customerInvitationStatus = new CustomerInvitationStatus();
                                if (reminderCount == 0)
                                {
                                    item.Id = item.Id != 0 ? item.Id : 0;
                                    item.ReminderCount = 1;
                                    item.UpdatedDate = DateTime.UtcNow;
                                }
                                else
                                {
                                    item.Id = item.Id != 0 ? item.Id : 0;
                                    item.ReminderCount = 2;
                                }
                                ctx.Entry(item).State = EntityState.Modified;
                                ctx.SaveChanges();

                                // string customerName = "<a style=\"text-decoration:none\" href=\"https://rpo.credencys.net/contactdetail/" + Convert.ToInt32(item.IdContact) + "\">" + contactInfo.Name + "</a>";
                                string customerName = "<a style=\"text-decoration:none\" href=\"" + Properties.Settings.Default.FrontEndUrl + + Convert.ToInt32(item.IdContact) + "\">" + contactInfo.Name + "</a>";

                                emailBody = emailBody.Replace("##Name##", customerName);
                                emailBody = emailBody.Replace("##Phone##", contactInfo.Phone);

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
                                if (!string.IsNullOrEmpty(item.EmailAddress))
                                {
                                    to.Add(new KeyValuePair<string, string>(item.EmailAddress, contactInfo.Name));
                                    //to.Add(new KeyValuePair<string, string>("meethalal.teli@credencys.com", contactInfo.Name));
                                }
                                List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                                if (to != null && to.Count > 0)
                                {
                                    Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, commonsub + "Customer Registration Reminder Notification", emailBody, true);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SendErrorToText(ex.ToString());
                }
            }
        }
        public static void SendErrorToText(string message)
        {
            string errorLogFilename = "CustomerReminderLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

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
    }
}