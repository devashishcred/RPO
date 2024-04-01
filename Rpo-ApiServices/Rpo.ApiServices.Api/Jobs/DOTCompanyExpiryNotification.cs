﻿using Microsoft.AspNet.SignalR;
using Quartz;
using Rpo.ApiServices.Api.Controllers.Jobs;
using Rpo.ApiServices.Api.Controllers.SystemSettings;
using Rpo.ApiServices.Api.Tools;
using Rpo.ApiServices.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Jobs
{
    public class DOTCompanyExpiryNotification : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }

        public DOTCompanyExpiryNotification()
        {
            //SendMailNotification();
        }
        public static void SendMailNotification()
        {
            ApplicationLog.WriteInformationLog("Company Expiry  DOT Insurance and active jobs Send Mail Notification executed: " + DateTime.Now.ToLongDateString());

            using (var ctx = new Model.RpoContext())
            {
                string addApplication_DOT = JobHistoryMessages.DOTExpiry;
                int generalContractor = 13;

                DateTime CurrentDate = DateTime.Now.AddDays(30);

                List<Job> jobList = ctx.Jobs.Where(x => x.Status == Model.Models.Enums.JobStatus.Active
                                    && x.Company.CompanyTypes.Where(y => y.Id == generalContractor).Count() > 0
                                                            && (
                                                                (DbFunctions.TruncateTime(x.Company.InsuranceObstructionBond) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.Company.InsuranceObstructionBond) <= DbFunctions.TruncateTime(CurrentDate))
                                                                ||
                                                                (DbFunctions.TruncateTime(x.Company.DOTInsuranceGeneralLiability) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.Company.DOTInsuranceGeneralLiability) <= DbFunctions.TruncateTime(CurrentDate))
                                                                ||
                                                                (DbFunctions.TruncateTime(x.Company.DOTInsuranceWorkCompensation) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.Company.DOTInsuranceWorkCompensation) <= DbFunctions.TruncateTime(CurrentDate)))).ToList();


                List<Company> objcompnames = new List<Company>();

                List<Job> objtmpjobs = new List<Job>();
                List<JobAssign> objAssignTotal = new List<JobAssign>();

                foreach (var item in jobList)
                {
                    objcompnames.Add(item.Company);
                    objtmpjobs.Add(item);

                    List<JobAssign> objAssignProjectTeam = new List<JobAssign>();

                    objAssignProjectTeam = SendNotificationToDOTProjectTeam(item.Id, "");
                    if (objAssignProjectTeam.Count > 0)
                    {
                        objAssignTotal.AddRange(objAssignProjectTeam);
                    }

                    List<JobContact> objcontact = ctx.JobContacts.Where(x => x.IdJob == item.Id && x.Company.CompanyTypes.Where(y => y.Id == generalContractor).Count() > 0
                                                             && (
                                                                (DbFunctions.TruncateTime(x.Company.InsuranceObstructionBond) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.Company.InsuranceObstructionBond) <= DbFunctions.TruncateTime(CurrentDate))
                                                                ||
                                                                (DbFunctions.TruncateTime(x.Company.DOTInsuranceGeneralLiability) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.Company.DOTInsuranceGeneralLiability) <= DbFunctions.TruncateTime(CurrentDate))
                                                                ||
                                                                (DbFunctions.TruncateTime(x.Company.DOTInsuranceWorkCompensation) >= DbFunctions.TruncateTime(CurrentDate) && DbFunctions.TruncateTime(x.Company.DOTInsuranceWorkCompensation) <= DbFunctions.TruncateTime(CurrentDate)))).ToList();

                    foreach (var itemCon in objcontact)
                    {
                        objcompnames.Add(item.Company);
                    }
                }

                var Companylist = objcompnames.Select(x => x.Id).Distinct();
                var jobstmp = objtmpjobs.Select(x => x.Id).Distinct();
                var employeelist = objAssignTotal.Select(x => x.IdEmployee).Distinct();

                foreach (var itemcmp in Companylist)
                {
                    int Compid = Convert.ToInt32(itemcmp);
                    Company objcom = ctx.Companies.Where(x => x.Id == Compid).FirstOrDefault();

                    string companyRedirectionlink = "<a href=\"##RedirectionLinkCompany##\">##CompanyName##</a>".Replace("##RedirectionLinkCompany##", Properties.Settings.Default.FrontEndUrl + "company").Replace("##CompanyName##", objcom != null ? objcom.Name : string.Empty);

                    foreach (var itememp in employeelist)
                    {
                        int employeeid = Convert.ToInt32(itememp);

                        List<JobAssign> objasign = objAssignTotal.Where(d => d.IdEmployee == employeeid).ToList();
                        string jobLinks = string.Empty;
                        foreach (var itemasign in objasign.Select(d => d.RedirectionLink))
                        {
                            if (!string.IsNullOrEmpty(itemasign))
                            {
                                jobLinks = jobLinks + itemasign + ",";
                            }
                        }
                        if (!string.IsNullOrEmpty(jobLinks))
                        {
                            jobLinks = jobLinks.Remove(jobLinks.Length - 1, 1);
                        }

                        addApplication_DOT = addApplication_DOT.Replace("##CompanyName##", companyRedirectionlink).Replace("##jobs##", jobLinks);

                        Employee employee = ctx.Employees.FirstOrDefault(x => x.Id == employeeid);

                        Rpo.ApiServices.Api.Tools.Common.SendInAppNotifications(employeeid, addApplication_DOT, Properties.Settings.Default.FrontEndUrl + "/company");


                    }

                    string Emailbody = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
                    {
                        Emailbody = reader.ReadToEnd();
                    }
                    Emailbody = Emailbody.Replace("##EmailBody##", "Hi,<br/><br/>" + addApplication_DOT);

                    SystemSettingDetail systemSettingDetail = Rpo.ApiServices.Api.Tools.Common.ReadSystemSetting(Enums.SystemSetting.DOTInsurancesAreAboutToBeExpired);
                    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                    {
                        foreach (var items in systemSettingDetail.Value)
                        {
                            List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                            to.Add(new KeyValuePair<string, string>(items.Email, items.FirstName + " " + items.LastName));

                            Tools.Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, "Company DOT Insurance Expiry Notification", Emailbody, null, true);
                        }
                    }
                }

            }
        }

        private static List<JobAssign> SendNotificationToDOTProjectTeam(int jobId, string message)
        {
            List<JobAssign> jobAssignList = new List<JobAssign>();
            using (var ctx = new Model.RpoContext())
            {
                Job jobs = ctx.Jobs.Include("RfpAddress.Borough").Include("Contact").Include("ProjectManager").Where(x => x.Id == jobId).FirstOrDefault();

                string setRedirecturl = string.Empty;
                string setRedirecturltmp = string.Empty;
                List<JobApplicationType> objapplicationTypes = jobs.JobApplicationTypes.ToList();

                if ((from d in objapplicationTypes where d.Id == 1 && d.Id == 2 select d).FirstOrDefault() != null)
                {
                    setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + jobs.Id + "" + "/application;idJobAppType=1";
                }
                else if ((from d in objapplicationTypes where d.Id == 1 select d).FirstOrDefault() != null)
                {
                    setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + jobs.Id + "" + "/application;idJobAppType=1";
                }
                else if ((from d in objapplicationTypes where d.Id == 2 select d).FirstOrDefault() != null)
                {
                    setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + jobs.Id + "" + "/dot;idJobAppType=2";
                }
                else
                {
                    setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + jobs.Id + "" + "/application;idJobAppType=1";
                }
                setRedirecturltmp = "<a href=\"##RedirectionLinkjob##\" >##jobNumber##</a> ".Replace("##RedirectionLinkjob##", setRedirecturl).Replace("##jobNumber##", jobs.JobNumber);


                if (jobs.IdProjectManager != null && jobs.IdProjectManager > 0)
                {
                    jobAssignList.Add(new JobAssign
                    {
                        IdEmployee = jobs.IdProjectManager,
                        JobAplicationType = "DOB",
                        IdJob = jobId,
                        JobNumber = jobs.JobNumber,
                        RedirectionLink = setRedirecturltmp,
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
                            IdJob = jobId,
                            JobNumber = jobs.JobNumber,
                            RedirectionLink = setRedirecturltmp,
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
                            IdJob = jobId,
                            JobNumber = jobs.JobNumber,
                            RedirectionLink = setRedirecturltmp,
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
                            JobAplicationType = "VIOLATION",
                            IdJob = jobId,
                            JobNumber = jobs.JobNumber,
                            RedirectionLink = setRedirecturltmp,
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
                            IdJob = jobId,
                            JobNumber = jobs.JobNumber,
                            RedirectionLink = setRedirecturltmp,
                        });
                    }
                }
                ctx.SaveChanges();
            }
            return jobAssignList;
        }
    }
}