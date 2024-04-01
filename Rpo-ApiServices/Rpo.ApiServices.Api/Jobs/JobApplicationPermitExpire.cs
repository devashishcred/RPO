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
    using System.Data.Entity;
    using System.IO;
    using System.Web;
    public class JobApplicationPermitExpire : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }

        public static void SendMailNotification()
        {
            ApplicationLog.WriteInformationLog("Job Applicatoin Permit Expiry Send Mail Notification executed: " + DateTime.Now.ToLongDateString());

            DateTime CurrentDateFrom = DateTime.Now;
            DateTime CurrentDateTO = DateTime.Now.AddDays(30);

            using (var ctx = new Model.RpoContext())
            {
                List<JobApplicationWorkPermitType> jobApplicationWorkPermitTypesDOB = ctx.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType").Include("JobWorkType")
                                                                                   .Where(x => x.JobApplication.JobApplicationType.IdParent == 1 && x.JobApplication.Job.Status == JobStatus.Active &&
                                                                                   (DbFunctions.TruncateTime(x.Expires) > DbFunctions.TruncateTime(CurrentDateFrom) &&
                                                                                     DbFunctions.TruncateTime(x.Expires) < DbFunctions.TruncateTime(CurrentDateTO))).ToList();

                List<JobApplicationWorkPermitType> jobApplicationWorkPermitTypesDOT = ctx.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType").Include("JobWorkType")
                                                                                  .Where(x => x.JobApplication.JobApplicationType.IdParent == 2 && x.JobApplication.Job.Status == JobStatus.Active &&
                                                                                   (DbFunctions.TruncateTime(x.Expires) > DbFunctions.TruncateTime(CurrentDateFrom) &&
                                                                                     DbFunctions.TruncateTime(x.Expires) < DbFunctions.TruncateTime(CurrentDateTO))).ToList();

                List<JobApplicationWorkPermitType> jobApplicationWorkPermitTypesDEP = ctx.JobApplicationWorkPermitTypes.Include("JobApplication.JobApplicationType").Include("JobWorkType")
                                                                                  .Where(x => x.JobApplication.JobApplicationType.IdParent == 4 && x.JobApplication.Job.Status == JobStatus.Active &&
                                                                                   (DbFunctions.TruncateTime(x.Expires) > DbFunctions.TruncateTime(CurrentDateFrom) &&
                                                                                     DbFunctions.TruncateTime(x.Expires) < DbFunctions.TruncateTime(CurrentDateTO))).ToList();

            #region DOBPermintExpiry
                string addApplication_DOB = InAppNotificationMessage._DOBPermit_Expiry;
                foreach (var itemDOB in jobApplicationWorkPermitTypesDOB)
                {
                    string setRedirecturl = string.Empty;
                    if (itemDOB.Id != 0)
                    {
                        setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + itemDOB.JobApplication.IdJob + "" + "/dot;idJobAppType=1";
                    }
                    addApplication_DOB = addApplication_DOB.Replace("##JobNumber##", itemDOB.JobApplication.IdJob.ToString()).Replace("##RedirectionLink##", setRedirecturl);
                    addApplication_DOB = addApplication_DOB.Replace("##PermitCode##", itemDOB.JobWorkType.Code);
                    addApplication_DOB = addApplication_DOB.Replace("##ApplicationType##", itemDOB.JobApplication.JobApplicationType.Description);
                    addApplication_DOB = addApplication_DOB.Replace("##ApplicationNo##", itemDOB.JobApplication.ApplicationNumber);

                    List<JobAssign> objAssignDOBTeam = JObDOBTeam(itemDOB.JobApplication.IdJob, addApplication_DOB);

                    var employeelist = objAssignDOBTeam.Select(x => x.IdEmployee).Distinct();

                    foreach (var itememp in employeelist)
                    {
                        int employeeid = Convert.ToInt32(itememp);
                        Employee employee = ctx.Employees.FirstOrDefault(x => x.Id == employeeid);
                        Rpo.ApiServices.Api.Tools.Common.SendInAppNotifications(employeeid, addApplication_DOB, Properties.Settings.Default.FrontEndUrl + "/job");
                    }

                    string Emailbody = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                    {
                        Emailbody = reader.ReadToEnd();
                    }

                    SystemSettingDetail systemSettingDetail = Rpo.ApiServices.Api.Tools.Common.ReadSystemSetting(Enums.SystemSetting.DOTInsurancesAreAboutToBeExpired);
                    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                    {
                        foreach (var items in systemSettingDetail.Value)
                        {
                            string Attentation = "<p style='padding: 0px; font-family: sans-serif; font-size: 14px; color: rgb(0, 0, 0); text-align: left; line-height: 22px; margin: 15px 0px;'>Hi ##EmployeeName##,</p>";
                            Attentation = Attentation.Replace("##EmployeeName##", items.FirstName + " " + items.LastName);
                            Emailbody = Emailbody.Replace("##EmailBody##", Attentation + addApplication_DOB);

                            List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                            to.Add(new KeyValuePair<string, string>(items.Email, items.FirstName + " " + items.LastName));
                            Tools.Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, "Company DOT Insurance Expiry Notifiction", Emailbody, null, true);
                        }
                    }
                }

                #endregion
                //#region DOTPermintExpiry
                //string addApplication_DOT = InAppNotificationMessage._DOTPermit_Expiry;
                //foreach (var itemDOT in jobApplicationWorkPermitTypesDOT)
                //{
                //    string setRedirecturl = string.Empty;
                //    if (itemDOT.Id != 0)
                //    {
                //        setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + itemDOT.JobApplication.IdJob + "" + "/dot;idJobAppType=2";
                //    }
                //    addApplication_DOB = addApplication_DOB.Replace("##JobNumber##", itemDOB.JobApplication.IdJob.ToString()).Replace("##RedirectionLink##", setRedirecturl);
                //    addApplication_DOB = addApplication_DOB.Replace("##PermitCode##", itemDOB.JobWorkType.Code);
                //    addApplication_DOB = addApplication_DOB.Replace("##ApplicationType##", itemDOB.JobApplication.JobApplicationType.Description);
                //    addApplication_DOB = addApplication_DOB.Replace("##ApplicationNo##", itemDOB.JobApplication.ApplicationNumber);

                //    List<JobAssign> objAssignDOBTeam = JObDOBTeam(itemDOB.JobApplication.IdJob, addApplication_DOB);

                //    var employeelist = objAssignDOBTeam.Select(x => x.IdEmployee).Distinct();

                //    foreach (var itememp in employeelist)
                //    {
                //        int employeeid = Convert.ToInt32(itememp);
                //        Employee employee = ctx.Employees.FirstOrDefault(x => x.Id == employeeid);
                //        Rpo.ApiServices.Api.Tools.Common.SendInAppNotifications(employeeid, addApplication_DOB, Properties.Settings.Default.FrontEndUrl + "/job");
                //    }

                //    string Emailbody = string.Empty;
                //    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                //    {
                //        Emailbody = reader.ReadToEnd();
                //    }

                //    SystemSettingDetail systemSettingDetail = Rpo.ApiServices.Api.Tools.Common.ReadSystemSetting(Enums.SystemSetting.DOTInsurancesAreAboutToBeExpired);
                //    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                //    {
                //        foreach (var items in systemSettingDetail.Value)
                //        {
                //            string Attentation = "<p style='padding: 0px; font-family: sans-serif; font-size: 14px; color: rgb(0, 0, 0); text-align: left; line-height: 22px; margin: 15px 0px;'>Hi ##EmployeeName##,</p>";
                //            Attentation = Attentation.Replace("##EmployeeName##", items.FirstName + " " + items.LastName);
                //            Emailbody = Emailbody.Replace("##EmailBody##", Attentation + addApplication_DOB);

                //            List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                //            to.Add(new KeyValuePair<string, string>(items.Email, items.FirstName + " " + items.LastName));
                //            Tools.Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, "Company DOT Insurance Expiry Notifiction", Emailbody, null, true);
                //        }
                //    }
                //}

                //#endregion

                ctx.SaveChanges();
            }
        }

        private static List<JobAssign> JObDOBTeam(int jobId, string message)
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
                ctx.SaveChanges();
            }
            return jobAssignList;
        }
    }
}
