using Quartz;
using Quartz.Impl;
using Rpo.ApiServices.Model.Models.Enums;
using Rpo.ApiServices.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Rpo.ApiServices.Model.Models;
using System.IO;
using System.Web;

/// <summary>
/// The Jobs namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Jobs
{
    public static class RfpJobs
    {
        public static void Register()
         {
            //http://www.cronmaker.com/
            //ITrigger trigger = TriggerBuilder.Create()
            //   .WithIdentity("everyMidnightTrigger", "everyMidnightGroup")
            //   .StartNow()
            //   .WithCronSchedule("0 0 0 * * ?")
            //   .Build();

            //IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            //scheduler.Start();

            //IJobDetail job = JobBuilder.Create<RfpTwoDaysInDraftJob>()
            //    .WithIdentity("rfpTwoDaysInDraft", "everyMidnightGroup")
            //    .Build();

            //scheduler.ScheduleJob(job, trigger);

            RegisterRfpInDraftAdminScheduler();
            //  RegisterRfpInDraftUserScheduler();
            RegisterRfpSubmittedUserScheduler();
            RegisterTaskReminderScheduler();
            RegisterJJobDOBPermitStatusUpdateScheduler();
            RegisterECBJobViolationUpdateScheduler();// cron job for ECBViolations
            RegisterDOBJobViolationUpdateScheduler(); // cron job for DOBViolations
            RegisterJobViolationUpdateScheduler(); //Cronjob stop on 14-May-2019..Due to Bis site call error
            //RegisterCompanyExpiryUpdateScheduler();  //Cronjob stop on 14-May-2019..Due to Bis site call error
            RegisterJJobDOBApplicationStatusUpdateScheduler();  //Cronjob stop on 14-May-2019.Due to Bis site call error
            // RegisterJobVARPMTStatusUpdateScheduler(); //Cronjob stop on 14-May-2019..Due to Bis site call error
            RegisterDOBCompanyExpiry();
            RegisterDOTCompanyExpiry();
            RegisterDOTDOBCompanyNotificationExpiry();
            RegisterJJobDOBNOWApplicationStatusUpdateScheduler();
            RegisterJobDOBUpdateScheduler();
            RegisterJobCustomerReminderScheduler();
        }

        public static void RegisterRfpInDraftAdminScheduler()
        {
            ApplicationLog.WriteInformationLog("RFP In Draft To Admin Job Registered : " + DateTime.Now.ToLongDateString());

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.RFPInDraftNotifyAdminSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.RFPInDraftNotifyAdminSchedulerMinute))
                                                         .InTimeZone(timeZoneInfo);

            ITrigger rFPInDraftNotifyAdminSchedulerTrigger = TriggerBuilder.Create()
               .WithIdentity("RFPInDraftNotifyAdminSchedulerTrigger", "everyMidnightGroup")
               .StartNow()
                // .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.RFPInDraftNotifyAdminSchedulerHour),Convert.ToInt32(Properties.Settings.Default.RFPInDraftNotifyAdminSchedulerMinute)))
                .WithSchedule(cronScheduleBuilder)
               .Build();

            IScheduler rFPInDraftNotifyAdminScheduler = StdSchedulerFactory.GetDefaultScheduler();

            rFPInDraftNotifyAdminScheduler.Start();

            IJobDetail rFPInDraftNotifyAdminSchedulerJob = JobBuilder.Create<RfpTwoDaysInDraftJob>()
                .WithIdentity("RFPInDraftNotifyAdminSchedulerJob", "everyMidnightGroup")
                .Build();

            rFPInDraftNotifyAdminScheduler.ScheduleJob(rFPInDraftNotifyAdminSchedulerJob, rFPInDraftNotifyAdminSchedulerTrigger);
        }

        public static void RegisterRfpInDraftUserScheduler()
        {
            ApplicationLog.WriteInformationLog("RFP In Draft To User Job Registered : " + DateTime.Now.ToLongDateString());

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.RfpTwoDaysInDraftUserSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.RfpTwoDaysInDraftUserSchedulerMinute))
                                                         .InTimeZone(timeZoneInfo);

            ITrigger rfpTwoDaysInDraftUserSchedulerTrigger = TriggerBuilder.Create()
               .WithIdentity("RfpTwoDaysInDraftUserSchedulerTrigger", "everyMidnightGroup")
               .StartNow()
               // .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.RfpTwoDaysInDraftUserSchedulerHour), Convert.ToInt32(Properties.Settings.Default.RfpTwoDaysInDraftUserSchedulerMinute)))
               .WithSchedule(cronScheduleBuilder)
               .Build();

            IScheduler rfpTwoDaysInDraftUserScheduler = StdSchedulerFactory.GetDefaultScheduler();

            rfpTwoDaysInDraftUserScheduler.Start();

            IJobDetail rFPInDraftNotifyAdminSchedulerJob = JobBuilder.Create<RfpTwoDaysInDraftUserNotificationJob>()
                .WithIdentity("RfpTwoDaysInDraftUserSchedulerJob", "everyMidnightGroup")
                .Build();

            rfpTwoDaysInDraftUserScheduler.ScheduleJob(rFPInDraftNotifyAdminSchedulerJob, rfpTwoDaysInDraftUserSchedulerTrigger);
        }

        public static void RegisterRfpSubmittedUserScheduler()
        {
            ApplicationLog.WriteInformationLog("RFP Submitted To User Job Registered : " + DateTime.Now.ToLongDateString());


            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.RfpTwoDaysSubmittedUserSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.RfpTwoDaysSubmittedUserSchedulerMinute))
                                                         .InTimeZone(timeZoneInfo);

            ITrigger rfpTwoDaysSubmittedUserSchedulerTrigger = TriggerBuilder.Create()
               .WithIdentity("RfpTwoDaysSubmittedUserSchedulerTrigger", "everyMidnightGroup")
               .StartNow()
               // .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.RfpTwoDaysSubmittedUserSchedulerHour), Convert.ToInt32(Properties.Settings.Default.RfpTwoDaysSubmittedUserSchedulerMinute)))
               .WithSchedule(cronScheduleBuilder)
               .Build();

            IScheduler rfpTwoDaysSubmittedUserScheduler = StdSchedulerFactory.GetDefaultScheduler();

            rfpTwoDaysSubmittedUserScheduler.Start();

            IJobDetail rFPInDraftNotifyAdminSchedulerJob = JobBuilder.Create<RfpTwoDaysInSubmittedUserNotificationJob>()
                .WithIdentity("RfpTwoDaysSubmittedUserSchedulerJob", "everyMidnightGroup")
                .Build();

            rfpTwoDaysSubmittedUserScheduler.ScheduleJob(rFPInDraftNotifyAdminSchedulerJob, rfpTwoDaysSubmittedUserSchedulerTrigger);
        }
        public static void RegisterJobCustomerReminderScheduler()
        {
            ApplicationLog.WriteInformationLog("Method call at : " + DateTime.Now.ToLongDateString());

            WriteLogWebclient("To Intialize the Customer Reminder cron job");
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobCustomerReminderSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.JobCustomerReminderSchedulerHourMinute))
                                                         .InTimeZone(timeZoneInfo);
           
            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
            .WithIdentity("CustomerReminderSchedulerTrigger", "everyMidnightGroup")
            .StartNow()
            .WithSchedule(cronScheduleBuilder)
            .Build();

            string executetimes = taskReminderSchedulerTrigger.StartTimeUtc.ToString();

            ApplicationLog.WriteInformationLog("Customer Reminder execute cronjob : " + executetimes);

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<CustomerReminder>()
                .WithIdentity("CustomerReminderSchedulerJob", "everyMidnightGroup")
                .Build();
            //CustomerReminder objCustomerReminder = new CustomerReminder();

            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
            WriteLogWebclient("To End the Cutomer Reminder cron job");
        }

        public static void RegisterTaskReminderScheduler()
        {
            ApplicationLog.WriteInformationLog("Task Reminder Job Registered : " + DateTime.Now.ToLongDateString());

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerMinute))
                                                         .InTimeZone(timeZoneInfo);

            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
              .WithIdentity("TaskReminderSchedulerTrigger", "everyMidnightGroup")
              .StartNow()
              //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerHour), Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerMinute)))
              .WithSchedule(cronScheduleBuilder)
              .Build();

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<TaskReminderJob>()
                .WithIdentity("TaskReminderSchedulerJob", "everyMidnightGroup")
                .Build();
             //TaskReminderJob test = new TaskReminderJob();
            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
        }

        #region Job Violation Update Scheduler
        public static void RegisterJobViolationUpdateScheduler()
        {
            ApplicationLog.WriteInformationLog("Job Violation Update Scheduler Registered : " + DateTime.Now.ToLongDateString());


            WriteLogWebclient("To Intialize the job Violation cron job");


            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);
            //var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZone.CurrentTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobViolationUpdateSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.JobViolationUpdateSchedulerHourMinute))
                                                         .InTimeZone(timeZoneInfo);
            //var trigger = TriggerBuilder.Create()
            //                            .StartNow()
            //                            .WithSchedule(cronScheduleBuilder)
            //                            .Build();

            //ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
            //  .WithIdentity("JobViolationSchedulerTrigger", "everyMidnightGroup")
            //  .StartNow().
            //  WithDailyTimeIntervalSchedule(
            //    x=>x.WithIntervalInMinutes(1))
            //  //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobViolationUpdateSchedulerHour), Convert.ToInt32(Properties.Settings.Default.JobViolationUpdateSchedulerHourMinute)))
            // // .WithSchedule(cronScheduleBuilder)
            //  .Build();



            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
            .WithIdentity("JobViolationSchedulerTrigger", "everyMidnightGroup")
            .StartNow()
            //WithDailyTimeIntervalSchedule(
            //  x => x.WithIntervalInMinutes(1))
            //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobViolationUpdateSchedulerHour), Convert.ToInt32(Properties.Settings.Default.JobViolationUpdateSchedulerHourMinute)))
             .WithSchedule(cronScheduleBuilder)
            .Build();

            string executetimes = taskReminderSchedulerTrigger.StartTimeUtc.ToString();

            ApplicationLog.WriteInformationLog("Job Violation execute cronjob : " + executetimes);

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<JobViolationUpdateResult>()
                .WithIdentity("JobViolationSchedulerJob", "everyMidnightGroup")
                .Build();
            //JobViolationUpdateResult objviolation = new JobViolationUpdateResult();
            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
            WriteLogWebclient("To End the job Violation cron job");
        }
        #endregion
        #region Job DOB Update Scheduler
        public static void RegisterJobDOBUpdateScheduler()
        {
            ApplicationLog.WriteInformationLog("DOB Violation Update Scheduler Registered : " + DateTime.Now.ToLongDateString());


            WriteLogWebclient("To Intialize the DOB Violation cron job");


            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);
            //var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZone.CurrentTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobDOBUpdateSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.JobDOBUpdateSchedulerHourMinute))
                                                         .InTimeZone(timeZoneInfo);
           
            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
            .WithIdentity("JobDOBSchedulerTrigger", "everyMidnightGroup")
            .StartNow().WithSchedule(cronScheduleBuilder)
            .Build();

            string executetimes = taskReminderSchedulerTrigger.StartTimeUtc.ToString();

            ApplicationLog.WriteInformationLog("Job Violation execute cronjob : " + executetimes);

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<JobDOBUpdateResult>()
                .WithIdentity("JobDOBSchedulerJob", "everyMidnightGroup")
                .Build();
            //JobDOBUpdateResult objviolation = new JobDOBUpdateResult();
            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
            WriteLogWebclient("To End the DOB Violation cron job");
        }
        #endregion
        #region Job Ecb Violation Update Scheduler
        public static void RegisterECBJobViolationUpdateScheduler()
        {
            ApplicationLog.WriteInformationLog("Job Violation Update Scheduler Registered : " + DateTime.Now.ToLongDateString());


            WriteLogWebclient("To Intialize the job Violation cron job");


            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);
            //var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZone.CurrentTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobECBViolationUpdateSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.JobECBViolationUpdateSchedulerHourMinute))
                                                         .InTimeZone(timeZoneInfo);
            //var trigger = TriggerBuilder.Create()
            //                            .StartNow()
            //                            .WithSchedule(cronScheduleBuilder)
            //                            .Build();

            //ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
            //  .WithIdentity("JobECBViolationSchedulerTrigger", "everyMidnightGroup")
            //  .StartNow().
            //  WithDailyTimeIntervalSchedule(
            //    x=>x.WithIntervalInMinutes(1))
            //  //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobECBViolationUpdateSchedulerHour)))
            // // .WithSchedule(cronScheduleBuilder)
            //  .Build();



            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
            .WithIdentity("JobECBViolationSchedulerTrigger", "everyMidnightGroup")
            .StartNow()
            //WithDailyTimeIntervalSchedule(
            //  x => x.WithIntervalInMinutes(1))
            //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobECBViolationUpdateSchedulerHour)))
             .WithSchedule(cronScheduleBuilder)
            .Build();

            string executetimes = taskReminderSchedulerTrigger.StartTimeUtc.ToString();

            ApplicationLog.WriteInformationLog("Job ECB Violation execute cronjob : " + executetimes);

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<JobECBViolationUpdateResult>()
                .WithIdentity("JobECBViolationSchedulerJob", "everyMidnightGroup")
                .Build();
            //JobECBViolationUpdateResult objviolation = new JobECBViolationUpdateResult();
            
            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
            WriteLogWebclient("To End the job Violation cron job");
        }
        #endregion

        #region Job DOB Violation Update Scheduler
        public static void RegisterDOBJobViolationUpdateScheduler()
            {
            ApplicationLog.WriteInformationLog("Job Violation Update Scheduler Registered : " + DateTime.Now.ToLongDateString());


            WriteLogWebclient("To Intialize the job Violation cron job");


            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobDOBViolationUpdateSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.JobDOBViolationUpdateSchedulerHourMinute))
                                                         .InTimeZone(timeZoneInfo);
            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
            .WithIdentity("JobDOBViolationSchedulerTrigger", "everyMidnightGroup")
            .StartNow()
             .WithSchedule(cronScheduleBuilder)
            .Build();

            string executetimes = taskReminderSchedulerTrigger.StartTimeUtc.ToString();

            ApplicationLog.WriteInformationLog("Job DOB Violation execute cronjob : " + executetimes);

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<JobDOBViolationUpdateResult>()
                .WithIdentity("JobDOBViolationSchedulerJob", "everyMidnightGroup")
                .Build();
            
            //JobDOBViolationUpdateResult objdobviolation = new JobDOBViolationUpdateResult();
            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
            WriteLogWebclient("To End the job Violation cron job");
            }
        #endregion


        #region Job DOB Application Status Update Scheduler
        public static void RegisterJJobDOBApplicationStatusUpdateScheduler()
        {
            ApplicationLog.WriteInformationLog("Job DOB Applicatoin Status Update Scheduler Registered : " + DateTime.Now.ToLongDateString());


            WriteLogWebclient("To Intialize the job DOB Applicatoin Status cron job:"  + DateTime.Now.ToLongDateString());


            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);
            //var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZone.CurrentTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobDobApplicationUpdateSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.JobDobApplicationUpdateSchedulerHourMinute))
                                                         .InTimeZone(timeZoneInfo);

            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
            .WithIdentity("JobDOBApplicatoinStatusSchedulerTrigger", "everyMidnightGroup")
            .StartNow()
            //WithDailyTimeIntervalSchedule(
            //  x => x.WithIntervalInMinutes(1))
            //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobViolationUpdateSchedulerHour), Convert.ToInt32(Properties.Settings.Default.JobViolationUpdateSchedulerHourMinute)))
             .WithSchedule(cronScheduleBuilder)
            .Build();

            string executetimes = taskReminderSchedulerTrigger.StartTimeUtc.ToString();

            ApplicationLog.WriteInformationLog("Job DOB Applicatoin Status execute cronjob : " + executetimes);

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<JobDOBApplicationStatusUpdateResult>()
                .WithIdentity("JobDOBApplicationStatusUpdateSchedulerJob", "everyMidnightGroup")
                .Build();
           // JobDOBApplicationStatusUpdateResult objviolation = new JobDOBApplicationStatusUpdateResult();
            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
            WriteLogWebclient("To End the job DOB Applicatoin Status cron job");
        }


        #endregion
        #region Job DOB Now Application Status Update Scheduler
        public static void RegisterJJobDOBNOWApplicationStatusUpdateScheduler()
        {
            ApplicationLog.WriteInformationLog("Job DOB Now Applicatoin Status Update Scheduler Registered : " + DateTime.Now.ToLongDateString());


            WriteLogWebclient("To Intialize the job DOB Now Applicatoin Status cron job:" + DateTime.Now.ToLongDateString());


            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobDobNowApplicationUpdateSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.JobDobNowApplicationUpdateSchedulerHourMinute))
                                                         .InTimeZone(timeZoneInfo);

            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
            .WithIdentity("JobDOBNowApplicatoinStatusSchedulerTrigger", "everyMidnightGroup")
            .StartNow()
            .WithSchedule(cronScheduleBuilder)
            .Build();

            string executetimes = taskReminderSchedulerTrigger.StartTimeUtc.ToString();

            ApplicationLog.WriteInformationLog("Job DOB Now Applicatoin Status execute cronjob : " + executetimes);

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<JobDOBNowApplicationStatusUpdateResult>()
                .WithIdentity("JobDOBNowApplicationStatusUpdateSchedulerJob", "everyMidnightGroup")
                .Build();
            //JobDOBNowApplicationStatusUpdateResult objviolation = new JobDOBNowApplicationStatusUpdateResult();
            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
            WriteLogWebclient("To End the job DOB Now Applicatoin Status cron job");
        }
        #endregion
        #region Job DOB Permit Status Update Scheduler
        public static void RegisterJJobDOBPermitStatusUpdateScheduler()
        {
            ApplicationLog.WriteInformationLog("Job DOB Permit Status Update Scheduler Registered : " + DateTime.Now.ToLongDateString());


            WriteLogWebclient("To Intialize the job DOB Permit Status cron job:" + DateTime.Now.ToLongDateString());


            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);
            //var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeZone.CurrentTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobPermitStatusSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.JobPermitStatusSchedulerHourMinute))
                                                         .InTimeZone(timeZoneInfo);

            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
            .WithIdentity("JobDOBPermitStatusSchedulerTrigger", "everyMidnightGroup")
            .StartNow()
            //WithDailyTimeIntervalSchedule(
            //  x => x.WithIntervalInMinutes(1))
            //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobViolationUpdateSchedulerHour), Convert.ToInt32(Properties.Settings.Default.JobViolationUpdateSchedulerHourMinute)))
             .WithSchedule(cronScheduleBuilder)
            .Build();

            string executetimes = taskReminderSchedulerTrigger.StartTimeUtc.ToString();

            ApplicationLog.WriteInformationLog("Job DOB Applicatoin Status execute cronjob : " + executetimes);

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<JobDOBPermitStatusUpdateResult>()
                .WithIdentity("JobDOBPermitStatusUpdateSchedulerJob", "everyMidnightGroup")
                .Build();
            // JobDOBApplicationStatusUpdateResult objviolation = new JobDOBApplicationStatusUpdateResult();
            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
            WriteLogWebclient("To End the job DOB Applicatoin Status cron job");
        }


        #endregion
        //#region Job DOB Permit Status Update Scheduler
        //public static void RegisterJobDOBPermitStatusUpdateScheduler()
        //{
        //    ApplicationLog.WriteInformationLog("Job DOB Applicatoin's Permit Status Update Scheduler Registered : " + DateTime.Now.ToLongDateString());
        //    var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

        //    var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobPermitStatusSchedulerHour),
        //        Convert.ToInt32(Properties.Settings.Default.JobPermitStatusSchedulerHourMinute))
        //                                                 .InTimeZone(timeZoneInfo);


        //    ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
        //      .WithIdentity("JobDOBPermitStatusUpdateSchedulerTrigger", "everyMidnightGroup")
        //      .StartNow()
        //      .WithSchedule(cronScheduleBuilder)
        //     // .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobDobApplicationUpdateSchedulerHour), Convert.ToInt32(Properties.Settings.Default.JobDobApplicationUpdateSchedulerHourMinute)))
        //      .Build();

        //    IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

        //    taskReminderScheduler.Start();

        //    IJobDetail taskReminderSchedulerJob = JobBuilder.Create<JobDOBPermitStatusUpdateResult>()
        //        .WithIdentity("JobDOBPermitStatusUpdateSchedulerJob", "everyMidnightGroup")
        //        .Build();

        //   //JobDOBPermitStatusUpdateResult obj = new JobDOBPermitStatusUpdateResult();

        //    taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
        //}
        //#endregion

        #region Company Expiry Update Scheduler Job
        public static void RegisterCompanyExpiryUpdateScheduler()
        {
            ApplicationLog.WriteInformationLog("Company Expiry Update Scheduler Registered : " + DateTime.Now.ToLongDateString());

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.CompanyExpiryUpdateSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.CompanyExpiryUpdateSchedulerMinute))
                                                         .InTimeZone(timeZoneInfo);

            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
              .WithIdentity("CompanyExpiryUpdateSchedulerTrigger", "everyMidnightGroup")
              .StartNow()
             // .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.CompanyExpiryUpdateSchedulerHour), Convert.ToInt32(Properties.Settings.Default.CompanyExpiryUpdateSchedulerMinute)))
            .WithSchedule(cronScheduleBuilder)
              .Build();

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<CompanyExpiryUpdate>()
                .WithIdentity("CompanyExpiryUpdateSchedulerJob", "everyMidnightGroup")
                .Build();

            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
        }
        #endregion


        #region Job VARPMT Status Update Scheduler
        public static void RegisterJobVARPMTStatusUpdateScheduler()
        {
            ApplicationLog.WriteInformationLog("Job DOB Applicatoin's Permit Status Update Scheduler Registered : " + DateTime.Now.ToLongDateString());
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobDobApplicationUpdateSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.JobDobApplicationUpdateSchedulerHourMinute))
                                                         .InTimeZone(timeZoneInfo);

            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
              .WithIdentity("RfpTwoDaysVARPMTTrigger", "everyMidnightGroup")
              .StartNow()
              //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.JobDobApplicationUpdateSchedulerHour), Convert.ToInt32(Properties.Settings.Default.JobDobApplicationUpdateSchedulerHourMinute)))
              .WithSchedule(cronScheduleBuilder)
              .Build();

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<RfpTwoDaysVARPMT>()
                .WithIdentity("RfpTwoDaysVARPMT", "everyMidnightGroup")
                .Build();

            RfpTwoDaysVARPMT obj = new RfpTwoDaysVARPMT();

            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
        }
        #endregion

        #region DOB Expiry 
        public static void RegisterDOBCompanyExpiry()
        {
            ApplicationLog.WriteInformationLog("DOB Expire Job Registered : " + DateTime.Now.ToLongDateString());
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerMinute))
                                                         .InTimeZone(timeZoneInfo);
            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
              .WithIdentity("DOBCompanyExpirySchedulerTrigger", "everyMidnightGroup")
              .StartNow()
              .WithSchedule(cronScheduleBuilder)
              //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerHour), Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerMinute)))
              .Build();

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<DOBCompanyExpiryNotification>()
                .WithIdentity("DOBCompanyExpiryNotification", "everyMidnightGroup")
                .Build();
           // DOBCompanyExpiryNotification test = new DOBCompanyExpiryNotification();
            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
        }
        #endregion

        #region DOT Expiry 
        public static void RegisterDOTCompanyExpiry()
        {
            ApplicationLog.WriteInformationLog("DOT Expiry Job Registered : " + DateTime.Now.ToLongDateString());
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerMinute))
                                                         .InTimeZone(timeZoneInfo);

            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
              .WithIdentity("DOTCompanyExpirySchedulerTrigger", "everyMidnightGroup")
              .StartNow()
              .WithSchedule(cronScheduleBuilder)
             // .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerHour), Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerMinute)))
              .Build();

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<DOTCompanyExpiryNotification>()
                .WithIdentity("DOTCompanyExpiryNotification", "everyMidnightGroup")
                .Build();
            //DOTCompanyExpiryNotification test = new DOTCompanyExpiryNotification();
            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
        }
        #endregion

        #region DOT BOB Company Expiry 
        public static void RegisterDOTDOBCompanyNotificationExpiry()
        {
            ApplicationLog.WriteInformationLog("Company Expiry Job Notifications Registered : " + DateTime.Now.ToLongDateString());
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(Properties.Settings.Default.CronjobTimeZone);

            var cronScheduleBuilder = CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerHour),
                Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerMinute))
                                                         .InTimeZone(timeZoneInfo);


            ITrigger taskReminderSchedulerTrigger = TriggerBuilder.Create()
              .WithIdentity("DOTCompanyNotificationExpirySchedulerTrigger", "everyMidnightGroup")
              .StartNow()
              .WithSchedule(cronScheduleBuilder)
              // .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerHour), Convert.ToInt32(Properties.Settings.Default.TaskReminderSchedulerMinute)))
              .Build();

            IScheduler taskReminderScheduler = StdSchedulerFactory.GetDefaultScheduler();

            taskReminderScheduler.Start();

            IJobDetail taskReminderSchedulerJob = JobBuilder.Create<JobDOTDOBExpiry>()
                .WithIdentity("JobDOTDOBExpiry", "everyMidnightGroup")
                .Build();
            //JobDOTDOBExpiry test = new JobDOTDOBExpiry();
            taskReminderScheduler.ScheduleJob(taskReminderSchedulerJob, taskReminderSchedulerTrigger);
        }
        #endregion


        public static void WriteLogWebclient(string message)
        {
            string errorLogFilename = "AllcronjobLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

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


            //var attachments = new List<string>();
            //if (File.Exists(path))
            //{
            //    attachments.Add(path);
            //}

            //var to = new List<KeyValuePair<string, string>>();

            //to.Add(new KeyValuePair<string, string>("rposupportgroup@credencys.com", "RPO Team"));

            //string body = string.Empty;
            //using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
            //{
            //    body = reader.ReadToEnd();
            //}

            //var cc = new List<KeyValuePair<string, string>>();

            //string emailBody = body;
            //emailBody = emailBody.Replace("##EmailBody##", "Please correct the issue.");

            //Mail.Send(
            //           new KeyValuePair<string, string>("noreply@rpoinc.com", "RPO Log"),
            //           to,
            //           cc,
            //           "[UAT]-Violation Cronjob Log",
            //           emailBody,
            //           attachments
            //       );
        }
    }

}