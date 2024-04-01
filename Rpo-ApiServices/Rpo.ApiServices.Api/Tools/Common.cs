// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-30-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="Common.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Common.</summary>
// ***********************************************************************

/// <summary>
/// The Tools namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Controllers.Employees;
    using Controllers.Jobs;
    using Controllers.Permissions;
    using Controllers.SystemSettings;
    using Microsoft.AspNet.SignalR;
    using Model;
    using Model.Models;
    using Model.Models.Enums;
    using System.Net;
    using System.IO;

    /// <summary>
    /// Class Common.
    /// </summary>
    public class Common
    {

        public static String ChangeDateTimeFormat(String inputText)
        {
            if (!String.IsNullOrEmpty(inputText))
            {
                List<String> li = inputText.Split('/').ToList();
                return li[1] + "/" + li[0] + "/" + li[2];

            }
            return "";
        }

        public static string ExportReportDateFormat = "MM/dd/yyyy";

        public static string PW517ReportDateFormat = "MM/dd/yy";
        /// <summary>
        /// The current timezone header key
        /// </summary>
        public static string CurrentTimezoneHeaderKey = "currentTimeZone";

        /// <summary>
        /// Fetches the header values.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="headerKey">The header key.</param>
        /// <returns>System.String.</returns>
        public static string FetchHeaderValues(HttpRequestMessage request, string headerKey)
        {
            string currentTimezone = string.Empty;
            try
            {
                currentTimezone = (request.Headers.GetValues(headerKey)).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(currentTimezone))
                {
                    return (request.Headers.GetValues(headerKey)).FirstOrDefault();
                }
                else
                {
                    TimeZone localZone = TimeZone.CurrentTimeZone;
                    return localZone.StandardName;
                }
            }
            catch (Exception ex)
            {
                TimeZone localZone = TimeZone.CurrentTimeZone;
                return localZone.StandardName;
            }
        }

        /// <summary>
        /// Sends the in application notifications.
        /// </summary>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <param name="message">The message.</param>
        /// <param name="Hub">The hub.</param>
        /// <param name="redirectionUrl">The redirection URL.</param>
        public static void SendInAppNotifications(int idEmployee, string message, IHubContext Hub, string redirectionUrl)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                rpoContext.UserNotifications.Add(new UserNotification
                {
                    NotificationMessage = message,
                    IsRead = false,
                    IsView = false,
                    IdUserNotified = idEmployee,
                    NotificationDate = DateTime.UtcNow,
                    RedirectionUrl = Properties.Settings.Default.FrontEndUrl + redirectionUrl
                });
                rpoContext.SaveChanges();

                int userNotificationCount = rpoContext.UserNotifications.Where(x => x.IdUserNotified == idEmployee && x.IsView == false).Count();
                Hub.Clients.Group(idEmployee.ToString()).notificationcount(idEmployee, userNotificationCount);
            }
        }

        /// <summary>
        /// Sends the in application notifications.
        /// </summary>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <param name="message">The message.</param>
        /// <param name="redirectionUrl">The redirection URL.</param>
        public static void SendInAppNotifications(int idEmployee, string message, string redirectionUrl)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                rpoContext.UserNotifications.Add(new UserNotification
                {
                    NotificationMessage = message,
                    IsRead = false,
                    IsView = false,
                    IdUserNotified = idEmployee,
                    NotificationDate = DateTime.UtcNow,
                    RedirectionUrl = Properties.Settings.Default.FrontEndUrl + redirectionUrl
                });
                rpoContext.SaveChanges();

                int userNotificationCount = rpoContext.UserNotifications.Where(x => x.IdUserNotified == idEmployee && x.IsView == false).Count();
            }
        }
        /// <summary>
        /// Sends the customer in application notifications.
        /// </summary>
        /// <param name="idCustomer">The identifier customer.</param>
        /// <param name="message">The message.</param>
        /// <param name="redirectionUrl">The redirection URL.</param>
        public static void SendCustomerInAppNotifications(int idCustomer, string message, string redirectionUrl)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                rpoContext.CustomerNotifications.Add(new CustomerNotification
                {
                    NotificationMessage = message,
                    IsRead = false,
                    IsView = false,
                    IdCustomerNotified = idCustomer,
                    NotificationDate = DateTime.UtcNow,
                    RedirectionUrl = Properties.Settings.Default.FrontEndUrl + redirectionUrl
                });
                rpoContext.SaveChanges();

                int customerNotificationCount = rpoContext.CustomerNotifications.Where(x => x.IdCustomerNotified == idCustomer && x.IsView == false).Count();
            }
        }

        

        /// <summary>
        /// Sends the in application notifications.
        /// </summary>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <param name="Hub">The hub.</param>
        public static void SendInAppNotifications(int idEmployee, IHubContext Hub)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                int userNotificationCount = rpoContext.UserNotifications.Where(x => x.IdUserNotified == idEmployee && x.IsView == false).Count();
                Hub.Clients.Group(idEmployee.ToString()).notificationcount(idEmployee, userNotificationCount);
            }
        }
        /// <summary>
        /// Sends the Customer in application notifications.
        /// </summary>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <param name="Hub">The hub.</param>
        public static void SendCustomerInAppNotifications(int idCustomer, IHubContext Hub)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                int customerNotificationCount = rpoContext.CustomerNotifications.Where(x => x.IdCustomerNotified == idCustomer && x.IsView == false).Count();
                Hub.Clients.Group(idCustomer.ToString()).notificationcount(idCustomer, customerNotificationCount);
            }
        }
        /// <summary>
        /// Checks the user permission.
        /// </summary>
        /// <param name="employeePermission">The employee permission.</param>
        /// <param name="permission">The permission.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CheckUserPermission(string employeePermission, Enums.Permission permission)
        {
            bool isvalid = false;

            if (string.IsNullOrEmpty(employeePermission))
            {
                isvalid = false;
            }
            else
            {
                List<int> groupPermissions = employeePermission != null && !string.IsNullOrEmpty(employeePermission) ? (employeePermission.Split(',') != null && employeePermission.Split(',').Any() ? employeePermission.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                int idPermission = permission.GetHashCode();
                if (groupPermissions.Contains(idPermission))
                {
                    isvalid = true;
                }
                else
                {
                    isvalid = false;
                }
            }

            return isvalid;
        }

        /// <summary>
        /// Saves the task history.
        /// </summary>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <param name="message">The message.</param>
        /// <param name="idTask">The identifier task.</param>
        /// <param name="idJobFeeSchedule">The identifier job fee schedule.</param>
        public static void SaveTaskHistory(int idEmployee, string message, int idTask, int idJobFeeSchedule)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                int? idJobFeeScheduletmp = null;
                if (idJobFeeSchedule != 0)
                {
                    idJobFeeScheduletmp = idJobFeeSchedule;
                }
                rpoContext.TaskHistories.Add(new TaskHistory
                {
                    Description = message,
                    IdEmployee = idEmployee,
                    HistoryDate = DateTime.UtcNow,
                    IdTask = idTask,
                    IdJobFeeSchedule = idJobFeeScheduletmp
                });
                rpoContext.SaveChanges();
            }
        }

        public static void SaveTaskMilestoneHistory(int idEmployee, string message, int idTask, int idMilestone)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                int? idJobFeeScheduletmp = null;
                if (idMilestone != 0)
                {
                    idJobFeeScheduletmp = idMilestone;
                }
                rpoContext.TaskHistories.Add(new TaskHistory
                {
                    Description = message,
                    IdEmployee = idEmployee,
                    HistoryDate = DateTime.UtcNow,
                    IdTask = idTask,
                    IdMilestone = idMilestone
                });
                rpoContext.SaveChanges();
            }
        }

        /// <summary>
        /// Saves the time note history.
        /// </summary>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <param name="message">The message.</param>
        /// <param name="idJobTimeNote">The identifier job time note.</param>
        /// <param name="idJobFeeSchedule">The identifier job fee schedule.</param>
        public static void SaveTimeNoteHistory(int idEmployee, string message, int idJobTimeNote, int idJobFeeSchedule)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                rpoContext.JobTimeNoteHistories.Add(new JobTimeNoteHistory
                {
                    Description = message,
                    IdEmployee = idEmployee,
                    HistoryDate = DateTime.UtcNow,
                    IdJobTimeNote = idJobTimeNote,
                    IdJobFeeSchedule = idJobFeeSchedule
                });
                rpoContext.SaveChanges();
            }
        }

        /// <summary>
        /// Saves the task history.
        /// </summary>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <param name="message">The message.</param>
        /// <param name="idTask">The identifier task.</param>
        public static void SaveTaskHistory(int idEmployee, string message, int idTask)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                rpoContext.TaskHistories.Add(new TaskHistory
                {
                    Description = message,
                    IdEmployee = idEmployee,
                    HistoryDate = DateTime.UtcNow,
                    IdTask = idTask
                });
                rpoContext.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the system settings.
        /// </summary>
        /// <param name="systemSetting">The system setting identifier.</param>
        /// <returns>System.Collections.Generic.List&lt;System.Collections.Generic.KeyValuePair&lt;System.String, System.String&gt;&gt;.</returns>
        public static List<KeyValuePair<string, string>> GetSystemSettings(Enums.SystemSetting systemSetting)
        {
            RpoContext rpoContext = new RpoContext();

            int idSystemSetting = systemSetting.GetHashCode();
            var systemSettingIds = rpoContext.SystemSettings.Where(x => x.Id == idSystemSetting).Select(c => c.Value).FirstOrDefault();

            List<int> Ids = systemSettingIds != null && !string.IsNullOrEmpty(systemSettingIds) ? (systemSettingIds.Split(',') != null && systemSettingIds.Split(',').Any() ? systemSettingIds.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            List<KeyValuePair<string, string>> result = rpoContext.Employees
                            .Where(x => Ids.Contains(x.Id))
                            .AsEnumerable()
                            .Select(e => new KeyValuePair<string, string>(e.Email, e.FirstName + " " + e.LastName))
                            .ToList();
            return result;

        }

        /// <summary>
        /// Reads the system setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns>SystemSettingDetail.</returns>
        public static SystemSettingDetail ReadSystemSetting(Enums.SystemSetting setting)
        {
            RpoContext rpoContext = new RpoContext();

            int idSystemSetting = setting.GetHashCode();
            var systemSetting = rpoContext.SystemSettings.FirstOrDefault(x => x.Id == idSystemSetting);
            List<int> employeeValue = systemSetting != null && systemSetting.Value != null && !string.IsNullOrEmpty(systemSetting.Value) ? (systemSetting.Value.Split(',') != null && systemSetting.Value.Split(',').Any() ? systemSetting.Value.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            if (employeeValue != null && employeeValue.Count > 0)
            {
                return new SystemSettingDetail
                {
                    Id = systemSetting.Id,
                    Name = systemSetting.Name,
                    Value = rpoContext.Employees.Where(c => employeeValue.Contains(c.Id)).AsEnumerable().Select(c => new EmployeeDetail()
                    {
                        Id = c.Id,
                        EmployeeName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty),
                        ItemName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                        Email = c.Email,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        IdEmployee = c.Id,
                    }),
                };
            }
            else
            {
                return new SystemSettingDetail();
            }
        }

        //public static List<JobAssign> GetSystemSettings(Enums.SystemSetting systemSetting)
        //{
        //}

        /// <summary>
        /// Sends the in application permissions.
        /// </summary>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <param name="Hub">The hub.</param>
        public static void SendInAppPermissions(int idEmployee, IHubContext Hub)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                Employee employee = rpoContext.Employees.FirstOrDefault(x => x.Id == idEmployee);
                List<int> employeePermissions = employee.Permissions != null && !string.IsNullOrEmpty(employee.Permissions) ? (employee.Permissions.Split(',') != null && employee.Permissions.Split(',').Any() ? employee.Permissions.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
                var PermissionDetails = rpoContext.Permissions.Where(x => employeePermissions.Contains(x.Id)).Select(x => new PermissionsDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    GroupName = x.GroupName,
                    ModuleName = x.ModuleName,
                });

                Hub.Clients.Group(idEmployee.ToString()).employeepermission(idEmployee, PermissionDetails);
            }
        }

        /// <summary>
        /// Saves the job history.
        /// </summary>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="jobHistoryMessages">The job history messages.</param>
        /// <param name="jobHistoryType">Type of the job history.</param>
        public static void SaveJobHistory(int idEmployee, int idJob, string jobHistoryMessages, JobHistoryType jobHistoryType)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                JobHistory jobHistory = new JobHistory();
                jobHistory.Description = jobHistoryMessages;
                jobHistory.HistoryDate = DateTime.UtcNow;
                jobHistory.JobHistoryType = jobHistoryType;
                jobHistory.IdJob = idJob;
                if (idEmployee != 0)
                    jobHistory.IdEmployee = idEmployee;
                rpoContext.JobHistories.Add(jobHistory);
                rpoContext.SaveChanges();
            }
        }
         /// <summary>
        /// Gets the sent via defualt cc.
        /// </summary>
        /// <param name="idSentVia">The identifier sent via.</param>
        /// <returns>List&lt;TransmissionTypeDefaultCC&gt;.</returns>
        public static List<TransmissionTypeDefaultCC> GetSentViaDefualtCC(int idSentVia)
        {
            RpoContext rpoContext = new RpoContext();
            List<TransmissionTypeDefaultCC> transmissionTypeDefault = rpoContext.TransmissionTypes.Where(x => x.Id == idSentVia).Select(c => c.DefaultCC.ToList()).FirstOrDefault();
            return transmissionTypeDefault;

        }

        /// <summary>
        /// Updates the job last modified date.
        /// </summary>
        /// <param name="idJob">The identifier job.</param>
        /// <param name="Idemployee">The idemployee.</param>
        public static void UpdateJobLastModifiedDate(int idJob, int Idemployee)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                Job job = rpoContext.Jobs.FirstOrDefault(r => r.Id == idJob);
                job.LastModiefiedDate = DateTime.UtcNow;
                job.LastModifiedBy = Idemployee;

                rpoContext.SaveChanges();
            }
        }

        /// <summary>
        /// Updates the milestone status.
        /// </summary>
        /// <param name="idJobFeeSchedule">The identifier job fee schedule.</param>
        /// <param name="Hub">The hub.</param>
        public static void UpdateMilestoneStatus(int idJobFeeSchedule, IHubContext Hub)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                JobMilestoneService jobMilestoneService = rpoContext.JobMilestoneServices.Include("Milestone").Include("JobFeeSchedule").FirstOrDefault(x => x.IdJobFeeSchedule == idJobFeeSchedule);
                if (jobMilestoneService != null)
                {
                    if (jobMilestoneService.Milestone.Status != "Completed")
                    {
                        int totalServiceCount = rpoContext.JobMilestoneServices.Include("JobFeeSchedule").Where(x => x.IdMilestone == jobMilestoneService.IdMilestone && x.JobFeeSchedule.IsRemoved != true).Count();
                        int completedServiceCount = rpoContext.JobMilestoneServices.Include("JobFeeSchedule").Where(x => x.IdMilestone == jobMilestoneService.IdMilestone && x.JobFeeSchedule.IsRemoved != true && x.JobFeeSchedule.Status == "Completed").Count();
                        if (totalServiceCount > 0 && completedServiceCount > 0 && totalServiceCount == completedServiceCount)
                        {
                            JobMilestone jobMilestone = rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").FirstOrDefault(x => x.Id == jobMilestoneService.IdMilestone);
                            jobMilestone.Status = "Completed";
                            rpoContext.SaveChanges();

                            string houseStreetNameBorrough = jobMilestone != null && jobMilestone.Job.RfpAddress != null ? jobMilestone.Job.RfpAddress.HouseNumber + " " + jobMilestone.Job.RfpAddress.Street + (jobMilestone.Job.RfpAddress.Borough != null ? " " + jobMilestone.Job.RfpAddress.Borough.Description : string.Empty) : string.Empty;
                            string specialPlaceName = jobMilestone != null && jobMilestone.Job.SpecialPlace != null ? " - " + jobMilestone.Job.SpecialPlace : string.Empty;
                            NotificationMails.SendMilestoneCompletedMail(jobMilestone.Name, jobMilestone.Job.JobNumber, houseStreetNameBorrough, specialPlaceName, jobMilestone.IdJob,null, Hub);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves the job work permit history.
        /// </summary>
        /// <param name="idWorkPermit">The identifier work permit.</param>
        /// <param name="idJobApplication">The identifier job application.</param>
        /// <param name="description">The description.</param>
        /// <param name="createdBy">The created by.</param>
        public static void SaveJobWorkPermitHistory(int? idWorkPermit, int? idJobApplication, string oldNumber, string newNumber, int createdBy)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                rpoContext.JobWorkPermitHistories.Add(new JobWorkPermitHistory
                {
                    IdWorkPermit = idWorkPermit,
                    IdJobApplication = idJobApplication,
                    NewNumber = newNumber,
                    OldNumber = oldNumber,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.UtcNow
                });
                rpoContext.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the name of the service item.
        /// </summary>
        /// <param name="jobFeeSchedule">The job fee schedule.</param>
        /// <returns>System.String.</returns>
        public static string GetServiceItemName(JobFeeSchedule jobFeeSchedule)
        {

            string rfpServiceItem = jobFeeSchedule.RfpWorkType != null ? jobFeeSchedule.RfpWorkType.Name : string.Empty;
            string rfpServiceGroup = string.Empty;
            string rfpSubJobType = string.Empty;
            string rfpSubJobTypeCategory = string.Empty;
            string rfpJobType = string.Empty;

            if (jobFeeSchedule.RfpWorkType != null)
            {
                if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 4)
                {
                    rfpServiceGroup = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 3)
                    {
                        rfpSubJobType = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                        if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 2)
                        {
                            rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;

                            if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null)
                            {
                                rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent.Name : string.Empty;
                            }
                        }
                        else if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 1)
                        {
                            rfpSubJobTypeCategory = string.Empty;
                            rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                    {
                        rfpSubJobType = string.Empty;
                        rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                        if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                        {
                            rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                    {
                        rfpSubJobType = string.Empty;
                        rfpSubJobTypeCategory = string.Empty;
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                    }

                }
                else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 3)
                {
                    rfpServiceGroup = string.Empty;
                    rfpSubJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                    {
                        rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                        if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                        {
                            rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                    {
                        rfpSubJobTypeCategory = string.Empty;
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                    }

                }
                else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 2)
                {
                    rfpServiceGroup = string.Empty;

                    rfpSubJobType = string.Empty;
                    rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent != null)
                    {
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                    }

                }
                else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 1)
                {
                    rfpJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                    rfpSubJobTypeCategory = string.Empty;

                    rfpSubJobType = string.Empty;

                    rfpServiceGroup = string.Empty;

                }


                string itemNametmp =
                    (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
                    (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
                    (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
                    (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
                    (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);

                string stringitm = string.Empty;

                using (RpoContext rpoContext = new RpoContext())
                {
                    if (jobFeeSchedule.IdRfp != null && jobFeeSchedule.RfpWorkType.PartOf == null)
                    {

                        var result = rpoContext.RfpFeeSchedules
                    .Include("RfpWorkType")
                    .Include("RfpWorkTypeCategory")
                    .Include("ProjectDetail.RfpSubJobType")
                    .Include("ProjectDetail.RfpJobType")
                    .Include("ProjectDetail.RfpSubJobTypeCategory")
                    .Where(x => x.IdRfp == jobFeeSchedule.IdRfp && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false).Select(d => d).ToList();

                        string stringJoin = string.Empty;
                        string allitemsid = string.Empty;
                        string allid = string.Empty;
                        double? totalamt = 0;
                        foreach (var itemname in result)
                        {
                            stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                            allid = allid + itemname.RfpWorkType.Id + ",";
                            totalamt = totalamt + itemname.TotalCost;
                        }
                        if (!string.IsNullOrEmpty(stringJoin))
                        {
                            stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                            stringitm = itemNametmp + " (" + stringJoin.Trim() + ")";
                        }
                        else
                        {
                            stringitm = itemNametmp;
                        }
                    }
                    else if (jobFeeSchedule.IdRfp == null && jobFeeSchedule.RfpWorkType.PartOf == null && jobFeeSchedule.IsAdditionalService == false)
                    {
                        var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == jobFeeSchedule.IdJob && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false && jobFeeSchedule.IsAdditionalService == false).Select(d => d).ToList();

                        string stringJoin = string.Empty;
                        string allitemsid = string.Empty;
                        string allid = string.Empty;
                        double? totalamt = 0;
                        foreach (var itemname in result)
                        {
                            stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                            allid = allid + itemname.RfpWorkType.Id + ",";
                            totalamt = totalamt + itemname.TotalCost;
                        }
                        if (!string.IsNullOrEmpty(stringJoin))
                        {
                            stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                            stringitm = itemNametmp + " (" + stringJoin.Trim() + ")";
                        }
                        else
                        {
                            stringitm = itemNametmp;
                        }
                    }
                    else if (jobFeeSchedule.IdRfp == null && jobFeeSchedule.RfpWorkType.PartOf == null && jobFeeSchedule.IsAdditionalService == true && jobFeeSchedule.IdParentof != null)
                    {
                        var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == jobFeeSchedule.IdJob && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false && x.IsAdditionalService == true && x.IdParentof == jobFeeSchedule.Id).Select(d => d).ToList();

                        string stringJoin = string.Empty;
                        string allitemsid = string.Empty;
                        string allid = string.Empty;
                        double? totalamt = 0;
                        foreach (var itemname in result)
                        {
                            stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                            allid = allid + itemname.RfpWorkType.Id + ",";
                            totalamt = totalamt + itemname.TotalCost;
                        }

                        if (!string.IsNullOrEmpty(stringJoin))
                        {
                            stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                            stringitm = itemNametmp + " (" + stringJoin.Trim() + ")";
                        }
                        else
                        {
                            stringitm = itemNametmp;
                        }
                    }
                    else
                    {
                        stringitm = itemNametmp;
                    }
                }

                return stringitm;
            }
            else
            { return ""; }
        }


        public static double? GetServiceItemCost(JobFeeSchedule jobFeeSchedule)
        {

            string rfpServiceItem = jobFeeSchedule.RfpWorkType != null ? jobFeeSchedule.RfpWorkType.Name : string.Empty;
            string rfpServiceGroup = string.Empty;
            string rfpSubJobType = string.Empty;
            string rfpSubJobTypeCategory = string.Empty;
            string rfpJobType = string.Empty;

            if (jobFeeSchedule.RfpWorkType != null)
            {

                if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 4)
                {
                    rfpServiceGroup = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 3)
                    {
                        rfpSubJobType = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                        if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 2)
                        {
                            rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;

                            if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null)
                            {
                                rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent.Name : string.Empty;
                            }
                        }
                        else if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Level == 1)
                        {
                            rfpSubJobTypeCategory = string.Empty;
                            rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                    {
                        rfpSubJobType = string.Empty;
                        rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                        if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                        {
                            rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                    {
                        rfpSubJobType = string.Empty;
                        rfpSubJobTypeCategory = string.Empty;
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                    }

                }
                else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 3)
                {
                    rfpServiceGroup = string.Empty;
                    rfpSubJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 2)
                    {
                        rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;

                        if (jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null)
                        {
                            rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (jobFeeSchedule.RfpWorkType.Parent.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent.Level == 1)
                    {
                        rfpSubJobTypeCategory = string.Empty;
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                    }

                }
                else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 2)
                {
                    rfpServiceGroup = string.Empty;

                    rfpSubJobType = string.Empty;
                    rfpSubJobTypeCategory = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                    if (jobFeeSchedule.RfpWorkType.Parent.Parent != null)
                    {
                        rfpJobType = jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Parent.Name : string.Empty;
                    }

                }
                else if (jobFeeSchedule.RfpWorkType.Parent != null && jobFeeSchedule.RfpWorkType.Parent.Level == 1)
                {
                    rfpJobType = jobFeeSchedule.RfpWorkType.Parent != null ? jobFeeSchedule.RfpWorkType.Parent.Name : string.Empty;

                    rfpSubJobTypeCategory = string.Empty;

                    rfpSubJobType = string.Empty;

                    rfpServiceGroup = string.Empty;

                }


                string itemNametmp =
                    (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
                    (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
                    (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
                    (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
                    (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);

                string stringitm = string.Empty;
                double? totalamt = jobFeeSchedule.TotalCost;

                using (RpoContext rpoContext = new RpoContext())
                {
                    if (jobFeeSchedule.IdRfp != null && jobFeeSchedule.RfpWorkType.PartOf == null)
                    {
                        var result = rpoContext.RfpFeeSchedules
                    .Include("RfpWorkType")
                    .Include("RfpWorkTypeCategory")
                    .Include("ProjectDetail.RfpSubJobType")
                    .Include("ProjectDetail.RfpJobType")
                    .Include("ProjectDetail.RfpSubJobTypeCategory")
                    .Where(x => x.IdRfp == jobFeeSchedule.IdRfp && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false).Select(d => d).ToList();

                        string stringJoin = string.Empty;
                        string allitemsid = string.Empty;
                        string allid = string.Empty;

                        foreach (var itemname in result)
                        {
                            stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                            allid = allid + itemname.RfpWorkType.Id + ",";
                            totalamt = totalamt + itemname.TotalCost;
                        }
                        if (!string.IsNullOrEmpty(stringJoin))
                        {
                            stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                            stringitm = itemNametmp + " (" + stringJoin.Trim() + ")";
                        }
                        else
                        {
                            stringitm = itemNametmp;
                        }
                    }
                    else if (jobFeeSchedule.IdRfp == null && jobFeeSchedule.RfpWorkType.PartOf == null && jobFeeSchedule.IsAdditionalService == false)
                    {
                        var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == jobFeeSchedule.IdJob && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false && jobFeeSchedule.IsAdditionalService == false).Select(d => d).ToList();

                        string stringJoin = string.Empty;
                        string allitemsid = string.Empty;
                        string allid = string.Empty;
                        //double? totalamt = 0;
                        foreach (var itemname in result)
                        {
                            stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                            allid = allid + itemname.RfpWorkType.Id + ",";
                            totalamt = totalamt + itemname.TotalCost;
                        }
                        if (!string.IsNullOrEmpty(stringJoin))
                        {
                            stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                            stringitm = itemNametmp + " (" + stringJoin.Trim() + ")";
                        }
                        else
                        {
                            stringitm = itemNametmp;
                        }
                    }
                    else if (jobFeeSchedule.IdRfp == null && jobFeeSchedule.RfpWorkType.PartOf == null && jobFeeSchedule.IsAdditionalService == true)
                    {
                        var result = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").Where(x => x.IdJob == jobFeeSchedule.IdJob && x.RfpWorkType.PartOf == jobFeeSchedule.IdRfpWorkType && x.RfpWorkType.IsShowScope == false && x.IsAdditionalService == true && x.IdParentof == jobFeeSchedule.Id).Select(d => d).ToList();

                        string stringJoin = string.Empty;
                        string allitemsid = string.Empty;
                        string allid = string.Empty;
                        //double? totalamt = 0;
                        foreach (var itemname in result)
                        {
                            stringJoin = stringJoin + " " + itemname.RfpWorkType.Name + ",";
                            allid = allid + itemname.RfpWorkType.Id + ",";
                            totalamt = totalamt + itemname.TotalCost;
                        }

                        if (!string.IsNullOrEmpty(stringJoin))
                        {
                            stringJoin = stringJoin.Remove(stringJoin.Length - 1);
                            stringitm = itemNametmp + " (" + stringJoin.Trim() + ")";
                        }
                        else
                        {
                            stringitm = itemNametmp;
                        }
                    }
                    else
                    {
                        stringitm = itemNametmp;
                    }
                }

                return totalamt;
            }
            else
            { return null; }
        }

        /// <summary>
        /// Gets the name of the service item.
        /// </summary>
        /// <param name="jobFeeSchedule">The job fee schedule.</param>
        /// <returns>System.String.</returns>
        public static string GetServiceItemName(RfpJobType jobFeeSchedule)
        {
            string rfpServiceItem = jobFeeSchedule != null ? jobFeeSchedule.Name : string.Empty;
            string rfpServiceGroup = string.Empty;
            string rfpSubJobType = string.Empty;
            string rfpSubJobTypeCategory = string.Empty;
            string rfpJobType = string.Empty;

            if (jobFeeSchedule.Parent != null && jobFeeSchedule.Parent.Level == 4)
            {
                rfpServiceGroup = jobFeeSchedule.Parent != null ? jobFeeSchedule.Parent.Name : string.Empty;

                if (jobFeeSchedule.Parent.Parent != null && jobFeeSchedule.Parent.Parent.Level == 3)
                {
                    rfpSubJobType = jobFeeSchedule.Parent != null && jobFeeSchedule.Parent.Parent != null ? jobFeeSchedule.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.Parent.Parent.Parent != null && jobFeeSchedule.Parent.Parent.Parent.Level == 2)
                    {
                        rfpSubJobTypeCategory = jobFeeSchedule.Parent.Parent.Parent != null && jobFeeSchedule.Parent.Parent.Parent != null ? jobFeeSchedule.Parent.Parent.Parent.Name : string.Empty;

                        if (jobFeeSchedule.Parent.Parent.Parent.Parent != null)
                        {
                            rfpJobType = jobFeeSchedule.Parent.Parent.Parent.Parent != null ? jobFeeSchedule.Parent.Parent.Parent.Parent.Name : string.Empty;
                        }
                    }
                    else if (jobFeeSchedule.Parent.Parent.Parent != null && jobFeeSchedule.Parent.Parent.Parent.Level == 1)
                    {
                        rfpSubJobTypeCategory = string.Empty;
                        rfpJobType = jobFeeSchedule.Parent.Parent.Parent != null ? jobFeeSchedule.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.Parent.Parent != null && jobFeeSchedule.Parent.Parent.Level == 2)
                {
                    rfpSubJobType = string.Empty;
                    rfpSubJobTypeCategory = jobFeeSchedule.Parent != null && jobFeeSchedule.Parent.Parent != null ? jobFeeSchedule.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.Parent.Parent.Parent != null)
                    {
                        rfpJobType = jobFeeSchedule.Parent.Parent.Parent != null && jobFeeSchedule.Parent.Parent.Parent != null ? jobFeeSchedule.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.Parent.Parent != null && jobFeeSchedule.Parent.Parent.Level == 1)
                {
                    rfpSubJobType = string.Empty;
                    rfpSubJobTypeCategory = string.Empty;
                    rfpJobType = jobFeeSchedule.Parent.Parent != null ? jobFeeSchedule.Parent.Parent.Name : string.Empty;
                }

            }
            else if (jobFeeSchedule.Parent != null && jobFeeSchedule.Parent.Level == 3)
            {
                rfpServiceGroup = string.Empty;
                rfpSubJobType = jobFeeSchedule.Parent != null ? jobFeeSchedule.Parent.Name : string.Empty;

                if (jobFeeSchedule.Parent.Parent != null && jobFeeSchedule.Parent.Parent.Level == 2)
                {
                    rfpSubJobTypeCategory = jobFeeSchedule.Parent != null && jobFeeSchedule.Parent.Parent != null ? jobFeeSchedule.Parent.Parent.Name : string.Empty;

                    if (jobFeeSchedule.Parent.Parent.Parent != null)
                    {
                        rfpJobType = jobFeeSchedule.Parent.Parent.Parent != null && jobFeeSchedule.Parent.Parent.Parent != null ? jobFeeSchedule.Parent.Parent.Parent.Name : string.Empty;
                    }
                }
                else if (jobFeeSchedule.Parent.Parent != null && jobFeeSchedule.Parent.Parent.Level == 1)
                {
                    rfpSubJobTypeCategory = string.Empty;
                    rfpJobType = jobFeeSchedule.Parent.Parent != null ? jobFeeSchedule.Parent.Parent.Name : string.Empty;
                }

            }
            else if (jobFeeSchedule.Parent != null && jobFeeSchedule.Parent.Level == 2)
            {
                rfpServiceGroup = string.Empty;

                rfpSubJobType = string.Empty;
                rfpSubJobTypeCategory = jobFeeSchedule.Parent != null ? jobFeeSchedule.Parent.Name : string.Empty;

                if (jobFeeSchedule.Parent.Parent != null)
                {
                    rfpJobType = jobFeeSchedule.Parent != null && jobFeeSchedule.Parent.Parent != null ? jobFeeSchedule.Parent.Parent.Name : string.Empty;
                }

            }
            else if (jobFeeSchedule.Parent != null && jobFeeSchedule.Parent.Level == 1)
            {
                rfpJobType = jobFeeSchedule.Parent != null ? jobFeeSchedule.Parent.Name : string.Empty;

                rfpSubJobTypeCategory = string.Empty;

                rfpSubJobType = string.Empty;

                rfpServiceGroup = string.Empty;

            }


            return (!string.IsNullOrEmpty(rfpJobType) ? rfpJobType + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpSubJobTypeCategory) ? rfpSubJobTypeCategory + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpSubJobType) ? rfpSubJobType + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpServiceGroup) ? rfpServiceGroup + " - " : string.Empty) +
                (!string.IsNullOrEmpty(rfpServiceItem) ? rfpServiceItem : string.Empty);
        }

        public static Address GetContactAddressForJobDocument(JobContact jobContact)
        {
            Address address = new Address();

            //address = jobContact != null && jobContact.Contact != null && jobContact.Contact.Addresses != null ? jobContact.Contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;

            //if (address == null)
            //{
            //    address = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null && jobContact.Contact.Company.Addresses != null ? jobContact.Contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
            //}
            address = null;
            if (jobContact != null && jobContact.Contact.IsPrimaryCompanyAddress != null && jobContact.Contact.IsPrimaryCompanyAddress == true)
            {
                address = jobContact != null && jobContact.Contact != null && jobContact.Contact.Company != null && jobContact.Contact.Company.Addresses != null ? jobContact.Contact.Company.Addresses.Where(x => x.Id == jobContact.Contact.IdPrimaryCompanyAddress).OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
            }
            else if (address == null)
            {
                address = jobContact != null && jobContact.Contact != null && jobContact.Contact.Addresses != null ? jobContact.Contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
            }
            return address;
        }

        public static Address GetContactAddressForJobDocument(Contact contact)
        {
            Address address = new Address();

            address = null;

            if (contact != null && contact.IsPrimaryCompanyAddress != null && contact.IsPrimaryCompanyAddress == true)
            {
                address = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.Where(x => x.Id == contact.IdPrimaryCompanyAddress).OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
            }
            //address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;

            else if (address == null)
            {
                address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                //address = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
            }

            return address;
        }

        public static string GetContactPhoneNumberForJobDocument(Contact contact)
        {
            string phoneNumber = string.Empty;


            //Address address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
            //string addressPhoneNumber = address != null && !string.IsNullOrEmpty(address.Phone) ? address.Phone : string.Empty;
            //if (!string.IsNullOrEmpty(addressPhoneNumber))
            //{
            //    phoneNumber = addressPhoneNumber;
            //}
            //if (string.IsNullOrEmpty(phoneNumber))
            //{
            //    Address address1 = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
            //    phoneNumber = address1 != null && !string.IsNullOrEmpty(address1.Phone) ? address1.Phone : string.Empty;
            //}
            if (contact != null && contact.IsPrimaryCompanyAddress != null && contact.IsPrimaryCompanyAddress == true)
            {
                Address address1 = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.Where(x => x.Id == contact.IdPrimaryCompanyAddress).OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                phoneNumber = address1 != null && !string.IsNullOrEmpty(address1.Phone) ? address1.Phone : string.Empty;
            }
            if (string.IsNullOrEmpty(phoneNumber))
            {
                Address address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                phoneNumber = address != null && !string.IsNullOrEmpty(address.Phone) ? address.Phone : string.Empty;
            }
            if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(contact.WorkPhone))
            {
                //phoneNumber = contact.WorkPhone + (!string.IsNullOrEmpty(contact.WorkPhoneExt) ? " X" + contact.WorkPhoneExt : string.Empty);
                phoneNumber = contact.WorkPhone;
            }
            if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(contact.MobilePhone))
            {
                phoneNumber = contact.MobilePhone;
            }


            return phoneNumber;
        }

        public static string GetContactPhoneNumberForJobDocument(JobContact jobContact)
        {
            string phoneNumber = string.Empty;

            if (jobContact != null && jobContact.Contact != null)
            {
                Contact contact = jobContact.Contact;
                //Address address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                //string phoneNum = address != null && !string.IsNullOrEmpty(address.Phone) ? address.Phone : string.Empty;
                //if (!string.IsNullOrEmpty(phoneNum))
                //{
                //    //Address address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                //    phoneNumber = address != null && !string.IsNullOrEmpty(address.Phone) ? address.Phone : string.Empty;
                //}
                //if (string.IsNullOrEmpty(phoneNumber))
                //{
                //    Address address1 = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                //    phoneNumber = address1 != null && !string.IsNullOrEmpty(address1.Phone) ? address1.Phone : string.Empty;
                //}
                if (contact.IsPrimaryCompanyAddress != null && contact.IsPrimaryCompanyAddress == true)
                {
                    Address address1 = contact != null && contact.Company != null && contact.Company.Addresses != null ? contact.Company.Addresses.Where(x => x.Id == contact.IdPrimaryCompanyAddress).OrderBy(x => x.AddressType.DisplayOrder).FirstOrDefault() : null;
                    phoneNumber = address1 != null && !string.IsNullOrEmpty(address1.Phone) ? address1.Phone : string.Empty;
                }
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    Address address = contact != null && contact.Addresses != null ? contact.Addresses.FirstOrDefault(x => x.IsMainAddress == true) : null;
                    phoneNumber = address != null && !string.IsNullOrEmpty(address.Phone) ? address.Phone : string.Empty;
                }
                if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(contact.WorkPhone))
                {
                    //phoneNumber = contact.WorkPhone + (!string.IsNullOrEmpty(contact.WorkPhoneExt) ? " X" + contact.WorkPhoneExt : string.Empty);
                    phoneNumber = contact.WorkPhone;
                }
                if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(contact.MobilePhone))
                {
                    phoneNumber = contact.MobilePhone;
                }
            }

            return phoneNumber;
        }
        public static Employee GetEmployeeInformation(int IdImployee)
        {
            using (RpoContext rpoContext = new RpoContext())
            {
                Employee objEmployee = rpoContext.Employees.Include("AgentCertificates").Where(x => x.Id == IdImployee).FirstOrDefault();
                Employee employee = new Employee();
                employee.LastName = objEmployee.LastName;
                employee.FirstName = objEmployee.FirstName;
                employee.WorkPhone = "(212) 566-5110";
                employee.WorkPhoneExt = "";
                employee.Email = objEmployee.Email;
                employee.MobilePhone = objEmployee.MobilePhone;
                employee.Address1 = "146 WEST 29TH STREET 2ND FLOOR ";
                employee.Address2 = "";
                employee.City = "NEW YORK";
                if (employee.State == null)
                    employee.State = new State();
                employee.State.Acronym = "NY";
                employee.ZipCode = "10001";
                employee.AgentCertificates = objEmployee.AgentCertificates;
                return employee;
            }
        }

        public static string GetFulladdressforjob(Job job)
        {
            string jobRFPAddress = string.Empty;
            string jobspecialplace = string.Empty;
            string jobfloor = string.Empty;
            string fulladdr = string.Empty;
            jobRFPAddress = job != null && job.RfpAddress != null ? job.RfpAddress.HouseNumber + " " + job.RfpAddress.Street + (job.RfpAddress.Borough != null ? ", " + job.RfpAddress.Borough.Description : string.Empty) : string.Empty;
            jobfloor = (!string.IsNullOrEmpty(job.FloorNumber) ? "Floor:" + job.FloorNumber + " " : string.Empty) + (!string.IsNullOrEmpty(job.Apartment) ? "Apartment:" + job.Apartment : string.Empty);
            jobspecialplace = job.SpecialPlace != null ? job.SpecialPlace : string.Empty;
            fulladdr = jobRFPAddress + (jobfloor != string.Empty ? Environment.NewLine + jobfloor : string.Empty) + (jobspecialplace != string.Empty ? Environment.NewLine + jobspecialplace : string.Empty);


            return fulladdr;


        }

        public static void DownloadFile(String remoteFilename, String localFilename)
        {
            // Function will return the number of bytes processed
            // to the caller. Initialize to 0 here.
            int bytesProcessed = 0;

            // Assign values to these objects here so that they can
            // be referenced in the finally block
            Stream remoteStream = null;
            Stream localStream = null;
            WebResponse response = null;

            // Use a try/catch/finally block as both the WebRequest and Stream
            // classes throw exceptions upon error
            //try
            //{
            // Create a request for the specified remote file name
            WebRequest request = WebRequest.Create(remoteFilename);
            if (request != null)
            {
                // Send the request to the server and retrieve the
                // WebResponse object 
                response = request.GetResponse();
                if (response != null)
                {
                    // Once the WebResponse object has been retrieved,
                    // get the stream object associated with the response's data
                    remoteStream = response.GetResponseStream();

                    // Create the local file
                    localStream = File.Create(localFilename);

                    // Allocate a 1k buffer
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    // Simple do/while loop to read from stream until
                    // no bytes are returned
                    do
                    {
                        // Read data (up to 1k) from the stream
                        bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

                        // Write the data to the local file
                        localStream.Write(buffer, 0, bytesRead);

                        // Increment total bytes processed
                        bytesProcessed += bytesRead;
                    } while (bytesRead > 0);
                }
            }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //finally
            //{
            // Close the response and streams objects here 
            // to make sure they're closed even if an exception
            // is thrown at some point
            if (response != null) response.Close();
            if (remoteStream != null) remoteStream.Close();
            if (localStream != null) localStream.Close();
            //}

            // Return total bytes processed to caller.
            //return bytesProcessed;
        }
    }

}