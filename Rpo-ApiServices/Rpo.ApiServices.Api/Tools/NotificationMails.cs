// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-27-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-24-2018
// ***********************************************************************
// <copyright file="NotificationMails.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Notification Mails.</summary>
// ***********************************************************************

/// <summary>
/// The Tools namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Tools
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNet.SignalR;
    using Model;
    using Model.Models;
    using Rpo.ApiServices.Api.Controllers.SystemSettings;

    /// <summary>
    /// Class Notification Mails.
    /// </summary>
    public class NotificationMails
    {
        /// <summary>
        /// Sends the milestone completed mail.
        /// </summary>
        /// <param name="milestoneName">Name of the milestone.</param>
        /// <param name="jobNumber">The job number.</param>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="Hub">The hub.</param>
        public static void SendMilestoneCompletedMail(string milestoneName, string jobNumber, string houseStreetNameBorrough, string specialPlaceName, int idJob, int? taskId, IHubContext Hub)
        {
            RpoContext rpoContext = new RpoContext();
            //SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenJobMilestoneIsCompleted);
            //if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
            //{
            //    foreach (var item in systemSettingDetail.Value)
            //    {
            //        string body = string.Empty;
            //        string htmTemplate = "NewJobMilestoneCompletedTemplate.htm";
            //        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/" + htmTemplate)))
            //        {
            //            body = reader.ReadToEnd();
            //        }
            //        string emailBody = body;
            //        emailBody = emailBody.Replace("##EmployeeName##", item.EmployeeName);
            //        emailBody = emailBody.Replace("##BillingPointName##", milestoneName);
            //        emailBody = emailBody.Replace("##JobNumber##", jobNumber);
            //        emailBody = emailBody.Replace("##HouseStreetNameBorrough##", houseStreetNameBorrough);
            //        emailBody = emailBody.Replace("##SpecialPlaceName##", specialPlaceName);
            //        emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + idJob + "/scope");

            //        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), new KeyValuePair<string, string>(item.Email, item.EmployeeName), "Job milestone is completed", emailBody, true);

            //        string jobMilestoneCompleted = InAppNotificationMessage.JobMilestoneCompleted
            //                                       .Replace("##EmployeeName##", item.EmployeeName)
            //                                       .Replace("##BillingPointName##", milestoneName)
            //                                       .Replace("##JobNumber##", jobNumber)
            //                                       .Replace("##HouseStreetNameBorrough##", houseStreetNameBorrough)
            //                                       .Replace("##SpecialPlaceName##", specialPlaceName)
            //                                       .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + idJob + "/scope");

            //        Common.SendInAppNotifications(item.Id, jobMilestoneCompleted, Hub, "job/" + idJob + "/scope");
            //    }
            //}

            Job job = rpoContext.Jobs.Include("RfpAddress").Include("RfpAddress.Borough").FirstOrDefault(x => x.Id == idJob);

            string JobAddress = job != null && job.RfpAddress != null ? (!string.IsNullOrEmpty(job.RfpAddress.HouseNumber) ? job.RfpAddress.HouseNumber : string.Empty) + " " + (!string.IsNullOrEmpty(job.RfpAddress.Street) ? job.RfpAddress.Street : string.Empty) + " " + (job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : JobHistoryMessages.NoSetstring) + " " + (!string.IsNullOrEmpty(job.RfpAddress.ZipCode) ? job.RfpAddress.ZipCode : string.Empty) + " " + (!string.IsNullOrEmpty(job.SpecialPlace) ? "-" + job.SpecialPlace : string.Empty):string.Empty;

            string jobMilestoneCompletedCommon = InAppNotificationMessage.JobMilestoneCompleted
                                           //.Replace("##EmployeeName##", item.EmployeeName)
                                           .Replace("##BillingPointName##", milestoneName)
                                           .Replace("##JobNumber##", jobNumber)
                                           .Replace("##HouseStreetNameBorrough##", houseStreetNameBorrough)
                                           .Replace("##SpecialPlaceName##", specialPlaceName)
                                           .Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/job/" + idJob + "/scope");


            SystemSettingDetail systemSettingDetail1 = Common.ReadSystemSetting(Enums.SystemSetting.ScopeBilling);
            if (systemSettingDetail1 != null && systemSettingDetail1.Value != null && systemSettingDetail1.Value.Count() > 0)
            {
                foreach (var item in systemSettingDetail1.Value)
                {
                    string tskmsg = string.Empty;
                    if (taskId != null)
                    {
                        string tasksetRedirecturl = string.Empty;
                        if (idJob != 0)
                        {
                            tasksetRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + idJob + "" + "/jobtask";
                        }
                        else
                        {
                            tasksetRedirecturl = Properties.Settings.Default.FrontEndUrl + "tasks";
                        }

                        tskmsg = "with reference to Task# <a id='linktask' data-type='task' data-id='"+ taskId + "' class='taskHistory_Click' href=\""+ tasksetRedirecturl + "\" rel='noreferrer'> " + taskId + "</a>";
                    }
                    string scopeclick = "<a href=\"##RedirectionLinkscope##\">Click here</a>".Replace("##RedirectionLinkscope##", Properties.Settings.Default.FrontEndUrl + "job/" + idJob + "/scope");
                    string body = string.Empty;
                    string jobredirect = "<a href=\""+ Properties.Settings.Default.FrontEndUrl + "job/" + idJob + "/application" + "\">" + jobNumber + "</a>";

                    string htmTemplate = "NewJobMilestoneCompletedTemplate.htm";
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/" + htmTemplate)))
                    {
                        body = reader.ReadToEnd();
                    }
                    string emailBody = body;
                    emailBody = emailBody.Replace("##EmployeeName##", item.EmployeeName);
                    emailBody = emailBody.Replace("##BillingPointName##", milestoneName);
                    emailBody = emailBody.Replace("##JobNumber##", jobredirect);
                    emailBody = emailBody.Replace("##TaskMessage##", tskmsg);
                    emailBody = emailBody.Replace("##HouseStreetNameBorrough##", houseStreetNameBorrough);
                    emailBody = emailBody.Replace("##SpecialPlaceName##", specialPlaceName);
                    emailBody = emailBody.Replace("##Clickhere##", scopeclick);

                    Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), new KeyValuePair<string, string>(item.Email, item.EmployeeName), EmailNotificationSubject.JobMilestoneCompleted.Replace("##BillingPointName##", milestoneName).Replace("##Job##", idJob != null && idJob != 0 ? idJob.ToString() : string.Empty).Replace("##JobAddress##", JobAddress), emailBody, true);
                    Common.SendInAppNotifications(item.Id, jobMilestoneCompletedCommon, Hub, "job/" + idJob + "/scope");
                }
            }

            List<int> dobProjectTeam = job != null && job.DOBProjectTeam != null && !string.IsNullOrEmpty(job.DOBProjectTeam) ? (job.DOBProjectTeam.Split(',') != null && job.DOBProjectTeam.Split(',').Any() ? job.DOBProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> dotProjectTeam = job != null && job.DOTProjectTeam != null && !string.IsNullOrEmpty(job.DOTProjectTeam) ? (job.DOTProjectTeam.Split(',') != null && job.DOTProjectTeam.Split(',').Any() ? job.DOTProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> violationProjectTeam = job != null && job.ViolationProjectTeam != null && !string.IsNullOrEmpty(job.ViolationProjectTeam) ? (job.ViolationProjectTeam.Split(',') != null && job.ViolationProjectTeam.Split(',').Any() ? job.ViolationProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<int> depProjectTeam = job != null && job.DEPProjectTeam != null && !string.IsNullOrEmpty(job.DEPProjectTeam) ? (job.DEPProjectTeam.Split(',') != null && job.DEPProjectTeam.Split(',').Any() ? job.DEPProjectTeam.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            var resultproject = rpoContext.Employees.Where(x => dotProjectTeam.Contains(x.Id)
                                                        || violationProjectTeam.Contains(x.Id)
                                                        || depProjectTeam.Contains(x.Id)
                                                        || dobProjectTeam.Contains(x.Id)
                                                        || x.Id == job.IdProjectManager
                                                        ).Select(x => new
                                                        {
                                                            Id = x.Id,
                                                            ItemName = x.Email

                                                        }).ToArray();
            foreach (var itemcomp in resultproject)
            {
                Common.SendInAppNotifications(itemcomp.Id, jobMilestoneCompletedCommon, Hub, "job/" + idJob + "/scope");
            }
        }

    }
}