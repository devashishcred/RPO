

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
    using Controllers.TaskReminders;
    using Quartz;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model.Models;

    public class TaskReminderJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            SendMailNotification();
        }
        //public TaskReminderJob()
        //{
        //    SendMailNotification();
        //}
        public static void SendMailNotification()
        {
            ApplicationLog.WriteInformationLog("Task Reminder Job Send Mail Notification executed : " + DateTime.Now.ToLongDateString());
            if (Convert.ToBoolean(Properties.Settings.Default.TaskReminderSchedulerStart))
            {
                ApplicationLog.WriteInformationLog("Task Reminder Job Send Mail Notification execution start : " + DateTime.Now.ToLongDateString());
                using (var ctx = new Model.RpoContext())
                {
                    //DateTime todayDate = DateTime.UtcNow;

                    DateTime todayDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

                    //List<TaskReminder> taskReminder = ctx.TaskReminders
                    //    .Include("LastModified")
                    //    .Include("Task")
                    //    .Where(r => DbFunctions.TruncateTime(r.ReminderDate) == DbFunctions.TruncateTime(todayDate)).Distinct()
                    //    .ToList();

                    List<TaskReminderDetail> taskReminderList = ctx.TaskReminders.Include("LastModified").Include("Task.TaskType").AsEnumerable()
                                                                         .Select(j => Format(j)).Where(r => DateTime.SpecifyKind(Convert.ToDateTime(r.TaskReminderDate.Date), DateTimeKind.Utc) == DateTime.SpecifyKind(Convert.ToDateTime(DateTime.UtcNow.Date), DateTimeKind.Utc)).Distinct().ToList();

                    //taskReminderList = taskReminderList.Where(r => DbFunctions.TruncateTime(r.TaskReminderDate) == DbFunctions.TruncateTime(todayDate)).Distinct().ToList();

                    //taskReminderList = (from item in taskReminderList
                    //                    where DbFunctions.TruncateTime(item.TaskReminderDate) == DbFunctions.TruncateTime(todayDate)
                    //                    select item).Distinct().ToList();

                    if (taskReminderList != null && taskReminderList.Count() > 0)
                    {
                        foreach (TaskReminderDetail taskReminderDetail in taskReminderList)
                        {
                            TaskReminder item = taskReminderDetail.TaskReminder;
                            string body = string.Empty;
                            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/TaskReminderTemplate.htm")))
                            {
                                body = reader.ReadToEnd();
                            }

                            if (!EmailReminderExists(item.IdTask, Convert.ToInt32(item.LastModifiedBy)))
                            {
                                string emailBody = body;
                                emailBody = emailBody.Replace("##EmployeeName##", item.LastModified != null ? item.LastModified.FirstName + " " + item.LastModified.LastName : string.Empty);
                                emailBody = emailBody.Replace("##Job##", item.Task.Job != null && item.Task.Job.JobNumber != null ? item.Task.Job.JobNumber : "Not Set");
                                emailBody = emailBody.Replace("##JobAddress##", item.Task.Job != null && item.Task.Job.RfpAddress != null ? item.Task.Job.RfpAddress.HouseNumber+item.Task.Job.RfpAddress.Street + item.Task.Job.RfpAddress.ZipCode : "Not Set");
                                emailBody = emailBody.Replace("##TaskNumber##", item.Task != null ? item.Task.TaskNumber : "Not Set");
                                emailBody = emailBody.Replace("##TaskType##", item.Task != null && item.Task.TaskType != null ? item.Task.TaskType.Name : "Not Set");
                                //emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/tasks");
                                emailBody = emailBody.Replace("##RedirectionLink##", "/tasks");
                               
                                emailBody = emailBody.Replace("##TaskDetails##", item.Task != null ? item.Task.GeneralNotes : string.Empty);


                                string notificationMessage = InAppNotificationMessage.TaskReminder;
                                notificationMessage = notificationMessage.Replace("##Job##", item.Task.Job != null && item.Task.Job.JobNumber != null ? item.Task.Job.JobNumber : "Not Set");
                                notificationMessage = notificationMessage.Replace("##JobAddress##", item.Task.Job != null && item.Task.Job.RfpAddress != null ? item.Task.Job.RfpAddress.HouseNumber + item.Task.Job.RfpAddress.Street + item.Task.Job.RfpAddress.ZipCode : "Not Set");
                                notificationMessage = notificationMessage.Replace("##TaskNumber##", item.Task != null ? item.Task.TaskNumber : "Not Set");
                                notificationMessage = notificationMessage.Replace("##TaskType##", item.Task != null && item.Task.TaskType != null ? item.Task.TaskType.Name : string.Empty);
                                notificationMessage = notificationMessage.Replace("##TaskDetails##", item.Task != null ? item.Task.GeneralNotes : string.Empty);
                                //notificationMessage = notificationMessage.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/tasks");
                                notificationMessage = notificationMessage.Replace("##RedirectionLink##", "/tasks");
                                if (!item.Task.TaskType.IsDisplayTime)
                                {
                                    // emailBody = emailBody.Replace("##DueDate##", item.Task != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.Task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString() + " EST" : string.Empty);
                                    emailBody = emailBody.Replace("##DueDate##", item.Task != null ? DateTime.SpecifyKind(Convert.ToDateTime(item.Task.CompleteBy), DateTimeKind.Utc).ToShortDateString() : string.Empty);
                                    //notificationMessage = notificationMessage.Replace("##DueDate##", item.Task != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.Task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString() + " EST" : string.Empty);
                                    notificationMessage = notificationMessage.Replace("##DueDate##", item.Task != null ? DateTime.SpecifyKind(Convert.ToDateTime(item.Task.CompleteBy), DateTimeKind.Utc).ToShortDateString() : string.Empty);
                                }
                                else
                                {
                                    //emailBody = emailBody.Replace("##DueDate##", item.Task != null ? Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.Task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))) + " EST" : string.Empty);
                                    //notificationMessage = notificationMessage.Replace("##DueDate##", item.Task != null ? Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.Task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))) + " EST" : string.Empty);
                                    emailBody = emailBody.Replace("##DueDate##", item.Task != null ? DateTime.SpecifyKind(Convert.ToDateTime(item.Task.CompleteBy), DateTimeKind.Utc).ToShortDateString() : string.Empty);
                                    notificationMessage = notificationMessage.Replace("##DueDate##", item.Task != null ? DateTime.SpecifyKind(Convert.ToDateTime(item.Task.CompleteBy), DateTimeKind.Utc).ToShortDateString() : string.Empty);
                                }

                                //emailBody = emailBody.Replace("##AssignedBy##", item.Task != null && item.Task.AssignedBy != null ? item.Task.AssignedBy.FirstName + " " + item.Task.AssignedBy.LastName : string.Empty);
                                //emailBody = emailBody.Replace("##AssignedTo##", item.Task != null && item.Task.AssignedTo != null ? item.Task.AssignedTo.FirstName + " " + item.Task.AssignedTo.LastName : string.Empty);
                                //emailBody = emailBody.Replace("##TaskDetail##", item.Task != null ? item.Task.GeneralNotes : string.Empty);

                                string taskReminderSubject = EmailNotificationSubject.TaskReminder;
                                taskReminderSubject = taskReminderSubject.Replace("##TaskType##", item.Task != null && item.Task.TaskType != null ? item.Task.TaskType.Name : string.Empty);
                                // taskReminderSubject = taskReminderSubject.Replace("##DueDate##", (item.Task != null ? (TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.Task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))).ToShortDateString() : string.Empty));
                                taskReminderSubject = taskReminderSubject.Replace("##DueDate##", (item.Task != null ? DateTime.SpecifyKind(Convert.ToDateTime(item.Task.CompleteBy), DateTimeKind.Utc).ToShortDateString() : string.Empty));
                                ctx.TaskEmailReminderLogs.Add(
                                    new TaskEmailReminderLog
                                    {
                                        EmailBody = emailBody,
                                        FromName = "RPO APP",
                                        FromEmail = Properties.Settings.Default.SmtpUserName,
                                        EmailSubject = taskReminderSubject,
                                        BccEmail = "",
                                        CcEmail = "",
                                        IdEmployee = Convert.ToInt32(item.LastModifiedBy),
                                        IdTask = item.IdTask,
                                        IsMailSent = false,
                                        ToEmail = item.LastModified.Email,
                                        ToName = item.LastModified.FirstName + " " + item.LastModified.LastName
                                    });

                                //string notificationMessage = InAppNotificationMessage.TaskIsDueBeforeTwoDays;
                                //notificationMessage = notificationMessage.Replace("##TaskNumber##", item.TaskNumber);
                                //notificationMessage = notificationMessage.Replace("##TaskType##", item.TaskType != null ? item.TaskType.Name : string.Empty);
                                //notificationMessage = notificationMessage.Replace("##TaskDetails##", item.GeneralNotes);
                                //if (!item.TaskType.IsDisplayTime)
                                //{
                                //    notificationMessage = notificationMessage.Replace("##DueDate##", TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString() + " EST");
                                //}
                                //else
                                //{
                                //    notificationMessage = notificationMessage.Replace("##DueDate##", Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))) + " EST");
                                //}
                                Common.SendInAppNotifications(Convert.ToInt32(item.LastModifiedBy), notificationMessage, "/tasks");

                                ctx.SaveChanges();
                            }
                        }
                    }
                }

                using (var ctx = new Model.RpoContext())
                {
                    //DateTime todayDate = DateTime.UtcNow.AddDays(1);

                   // DateTime todayDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(2), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                    DateTime todayDate = DateTime.SpecifyKind(Convert.ToDateTime(DateTime.UtcNow.AddDays(2)), DateTimeKind.Utc);

                    //List<Task> tasks = ctx.Tasks
                    //    .Include("AssignedTo")
                    //    .Include("AssignedBy")
                    //    .Where(r => DbFunctions.TruncateTime(r.CompleteBy) == DbFunctions.TruncateTime(todayDate)).Distinct()
                    //    .ToList();

                    List<TaskDetail> taskDetailList = ctx.Tasks.Include("AssignedTo")
                                                                         .Include("AssignedBy")
                                                                         .Include("TaskType")
                                                                         .Include("Rfp.RfpAddress.Borough")
                                                                         .Include("Job.RfpAddress.Borough")
                                                                         .AsEnumerable()
                                                                         .Select(j => FormatDetails(j))
                                                                         .Where(r => DateTime.SpecifyKind(r.TaskDueDate, DateTimeKind.Utc).Date == todayDate.Date).Distinct()
                                                                         .ToList();

                    //taskDetailList = taskDetailList.Where(r => DbFunctions.TruncateTime(r.TaskDueDate) == DbFunctions.TruncateTime(todayDate)).Distinct().AsQueryable();

                    //taskDetailList = (from item in taskDetailList
                    //                  where DbFunctions.TruncateTime(item.TaskDueDate) == DbFunctions.TruncateTime(todayDate)
                    //                  select item).Distinct().AsQueryable();

                    if (taskDetailList != null && taskDetailList.Count() > 0)
                    {
                        foreach (TaskDetail taskDetail in taskDetailList)
                        {
                            Task item = taskDetail.Task;

                            if (!EmailReminderExists(item.Id, Convert.ToInt32(item.IdAssignedBy)))
                            {
                                if (item.AssignedBy != null && !string.IsNullOrWhiteSpace(item.AssignedBy.Email))
                                {
                                  
                                    string notificationMessage = InAppNotificationMessage.TaskIsDueBeforeTwoDays;
                                    notificationMessage = notificationMessage.Replace("##JobNumber##", item.Job != null && item.Job.JobNumber != null ? item.Job.JobNumber : "Not Set");
                                    notificationMessage = notificationMessage.Replace("##JobAddress##", item.Job != null && item.Job.RfpAddress != null ? item.Job.RfpAddress.HouseNumber + item.Job.RfpAddress.Street + item.Job.RfpAddress.ZipCode : "Not Set");
                                    notificationMessage = notificationMessage.Replace("##TaskNumber##", item.TaskNumber);
                                    notificationMessage = notificationMessage.Replace("##TaskType##", item.TaskType != null ? item.TaskType.Name : "Not Set");
                                    notificationMessage = notificationMessage.Replace("##TaskDetails##", item.GeneralNotes);
                                    notificationMessage = notificationMessage.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/tasks");
                                    if (!item.TaskType.IsDisplayTime)
                                    {
                                        // notificationMessage = notificationMessage.Replace("##DueDate##", TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString() + " EST");
                                        notificationMessage = notificationMessage.Replace("##DueDate##", DateTime.SpecifyKind(Convert.ToDateTime(item.CompleteBy), DateTimeKind.Utc).ToShortDateString());
                                    }
                                    else
                                    {
                                        //notificationMessage = notificationMessage.Replace("##DueDate##", Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))) + " EST");
                                        notificationMessage = notificationMessage.Replace("##DueDate##", Convert.ToString(DateTime.SpecifyKind(Convert.ToDateTime(item.CompleteBy), DateTimeKind.Utc)));
                                    }
                                    Common.SendInAppNotifications(Convert.ToInt32(item.IdAssignedTo), notificationMessage, "/tasks");
                                }
                            }

                            if (!EmailReminderExists(item.Id, Convert.ToInt32(item.IdAssignedTo)))
                            {
                                if (item.AssignedTo != null && !string.IsNullOrWhiteSpace(item.AssignedTo.Email))
                                {
                                    //string body = string.Empty;
                                    //using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/TaskReminderTemplate.htm")))
                                    //{
                                    //    body = reader.ReadToEnd();
                                    //}

                                    //string emailBody = body;
                                    //emailBody = emailBody.Replace("##EmployeeName##", item.AssignedTo != null ? item.AssignedTo.FirstName + " " + item.AssignedTo.LastName : string.Empty);
                                    //emailBody = emailBody.Replace("##TaskType##", item.TaskType != null ? item.TaskType.Name : string.Empty);
                                    //string notificationMessage = StaticMessages.TaskReminderNotificationMessage.Replace("##TaskNumber##", item.TaskNumber);

                                    //if (!item.TaskType.IsDisplayTime)
                                    //{
                                    //    emailBody = emailBody.Replace("##DueDate##", TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString() + " EST");
                                    //    notificationMessage = notificationMessage.Replace("##DueDate##", TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString() + " EST");
                                    //}
                                    //else
                                    //{
                                    //    emailBody = emailBody.Replace("##DueDate##", Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))) + " EST");
                                    //    notificationMessage = notificationMessage.Replace("##DueDate##", Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))) + " EST");
                                    //}

                                    //emailBody = emailBody.Replace("##AssignedBy##", item.AssignedBy != null ? item.AssignedBy.FirstName + " " + item.AssignedBy.LastName : string.Empty);
                                    //emailBody = emailBody.Replace("##AssignedTo##", item.AssignedTo != null ? item.AssignedTo.FirstName + " " + item.AssignedTo.LastName : string.Empty);
                                    //emailBody = emailBody.Replace("##TaskDetail##", item.GeneralNotes);

                                    //ctx.TaskEmailReminderLogs.Add(
                                    //    new TaskEmailReminderLog
                                    //    {
                                    //        EmailBody = emailBody,
                                    //        FromName = "RPO APP",
                                    //        FromEmail = Properties.Settings.Default.SmtpUserName,
                                    //        EmailSubject = (item.TaskType != null ? item.TaskType.Name : string.Empty) + " task reminder due on " + (TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))).ToShortDateString(),
                                    //        BccEmail = "",
                                    //        CcEmail = "",
                                    //        IdEmployee = Convert.ToInt32(item.IdAssignedTo),
                                    //        IdTask = item.Id,
                                    //        IsMailSent = false,
                                    //        ToEmail = item.AssignedTo != null ? item.AssignedTo.Email : string.Empty,
                                    //        ToName = item.AssignedTo != null ? item.AssignedTo.FirstName + " " + item.AssignedTo.LastName : string.Empty
                                    //    });

                                    string notificationMessage = InAppNotificationMessage.TaskIsDueBeforeTwoDays;
                                    notificationMessage = notificationMessage.Replace("##Job##", item.Job != null && item.Job.JobNumber != null ? item.Job.JobNumber : "Not Set");
                                    notificationMessage = notificationMessage.Replace("##JobAddress##", item.Job != null && item.Job.RfpAddress != null ? item.Job.RfpAddress.HouseNumber + item.Job.RfpAddress.Street + item.Job.RfpAddress.ZipCode : "Not Set");
                                    notificationMessage = notificationMessage.Replace("##TaskNumber##", item.TaskNumber);
                                    notificationMessage = notificationMessage.Replace("##TaskType##", item.TaskType != null ? item.TaskType.Name : "Not Set");
                                    notificationMessage = notificationMessage.Replace("##TaskDetails##", item.GeneralNotes);
                                    notificationMessage = notificationMessage.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/tasks");
                                    if (!item.TaskType.IsDisplayTime)
                                    {
                                        notificationMessage = notificationMessage.Replace("##DueDate##", TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString());
                                    }
                                    else
                                    {
                                        notificationMessage = notificationMessage.Replace("##DueDate##", Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(item.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))) + " EST");

                                    }

                                    Common.SendInAppNotifications(Convert.ToInt32(item.IdAssignedTo), notificationMessage, "/tasks");
                                }

                            }
                        }
                    }
                }

                List<TaskEmailReminderLog> taskEmailReminderLogs = new List<TaskEmailReminderLog>();
                using (var ctx = new Model.RpoContext())
                {
                    taskEmailReminderLogs = ctx.TaskEmailReminderLogs
                        .Where(r => r.IsMailSent == false)
                        .ToList();

                }

                List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.TaskReminderMail);
                if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                {
                    foreach (EmployeeDetail item in systemSettingDetail.Value)
                    {
                        cc.Add(new KeyValuePair<string, string>(item.Email, item.Email));
                    }
                }

                foreach (TaskEmailReminderLog item in taskEmailReminderLogs)
                {
                    List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                    to.Add(new KeyValuePair<string, string>(item.ToEmail, item.ToName));

                    Tools.Mail.Send(new KeyValuePair<string, string>(item.FromEmail, item.FromName), to, cc, item.EmailSubject, item.EmailBody, true);
                    using (var ctx = new Model.RpoContext())
                    {
                        var taskEmailReminderLog = ctx.TaskEmailReminderLogs.Find(item.Id);
                        if (taskEmailReminderLog != null)
                        {
                            taskEmailReminderLog.IsMailSent = true;
                            ctx.SaveChanges();
                        }
                    }
                }

                using (var ctx = new Model.RpoContext())
                {
                    List<TaskEmailReminderLog> taskEmailReminder = ctx.TaskEmailReminderLogs
                        .Where(r => r.IsMailSent == true)
                        .ToList();

                    if (taskEmailReminder.Count > 0)
                    {
                        ctx.TaskEmailReminderLogs.RemoveRange(taskEmailReminder);
                        ctx.SaveChanges();
                    }
                }
            }
        }


        /// <summary>
        /// Emails the reminder exists.
        /// </summary>
        /// <param name="idTask">The identifier task.</param>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool EmailReminderExists(int idTask, int idEmployee)
        {
            using (var db = new Model.RpoContext())
            {
                return db.TaskEmailReminderLogs.Count(e => e.IdTask == idTask && e.IdEmployee == idEmployee) > 0;
            }
        }


        public static TaskReminderDetail Format(TaskReminder taskReminder)
        {
            return new TaskReminderDetail
            {
                TaskReminder = taskReminder,
                TaskReminderDate = taskReminder.ReminderDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taskReminder.ReminderDate), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")) : taskReminder.ReminderDate,
            };
        }

        public static TaskDetail FormatDetails(Task task)
        {
            return new TaskDetail
            {
                Task = task,
                TaskDueDate = task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")) : DateTime.MinValue,
            };
        }

    }
}