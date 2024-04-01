// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-24-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="TasksController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Tasks Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Tasks namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Api.Jobs;
    using Enums;
    using Filters;
    using Hubs;
    using JobMilestones;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using SystemSettings;
    using TaskNotes;
    using GlobalSearch;

    /// <summary>
    /// Class Tasks Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class TasksController : HubApiController<GroupHub>
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the list of  tasks.
        /// </summary>
        /// <param name="advancedSearchParameters">The advanced search parameters.</param>
        /// <returns> Gets the list of  tasks.</returns>
        //[Authorize]
        //[RpoAuthorize]
        //[ResponseType(typeof(DataTableResponse))]
        //public IHttpActionResult GetTasks([FromUri] TaskAdvancedSearchParameters advancedSearchParameters) 
        //{
        //    string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

        //    var recordsTotal = rpoContext.Tasks.Count();
        //    var recordsFiltered = recordsTotal;

        //    IQueryable<Task> tasks = rpoContext.Tasks
        //                                .Include("Rfp.RfpAddress.Borough")
        //                                .Include("Job.RfpAddress.Borough")
        //                                .Include("AssignedTo")
        //                                .Include("AssignedBy")
        //                                .Include("TaskType")
        //                                .Include("TaskStatus")
        //                                .Include("Notes")
        //                                .Include("JobFeeSchedule")
        //                                .Include("Examiner")
        //                                .Include("JobApplication.JobApplicationType")
        //                                .Include("JobViolation")
        //                                .Include("LastModified")
        //                                .Include("CreatedByEmployee").AsQueryable();



        //    if (advancedSearchParameters.IdBorough != null && advancedSearchParameters.IdBorough > 0)
        //    {
        //        tasks = tasks.Where(t => t.Rfp.RfpAddress.IdBorough == advancedSearchParameters.IdBorough || t.Job.RfpAddress.IdBorough == advancedSearchParameters.IdBorough);
        //    }

        //    if (!string.IsNullOrWhiteSpace(advancedSearchParameters.HouseNumber))
        //    {
        //        tasks = tasks.Where(t => t.Rfp.RfpAddress.HouseNumber.Contains(advancedSearchParameters.HouseNumber) || t.Job.RfpAddress.HouseNumber.Contains(advancedSearchParameters.HouseNumber));
        //    }

        //    if (!string.IsNullOrWhiteSpace(advancedSearchParameters.Street))
        //    {
        //        tasks = tasks.Where(t => t.Rfp.RfpAddress.Street.Contains(advancedSearchParameters.Street) || t.Job.RfpAddress.Street.Contains(advancedSearchParameters.Street));
        //    }

        //    if (advancedSearchParameters.IdTaskType != null && advancedSearchParameters.IdTaskType > 0)
        //    {
        //        tasks = tasks.Where(t => t.IdTaskType == advancedSearchParameters.IdTaskType);
        //    }

        //    if (advancedSearchParameters.IdTaskStatus != null && advancedSearchParameters.IdTaskStatus > 0)
        //    {
        //        tasks = tasks.Where(t => t.IdTaskStatus == advancedSearchParameters.IdTaskStatus);
        //    }

        //    if (!string.IsNullOrEmpty(advancedSearchParameters.IdAssignedBy))
        //    {
        //        List<int> assignedByTeam = advancedSearchParameters.IdAssignedBy != null && !string.IsNullOrEmpty(advancedSearchParameters.IdAssignedBy) ? (advancedSearchParameters.IdAssignedBy.Split('-') != null && advancedSearchParameters.IdAssignedBy.Split('-').Any() ? advancedSearchParameters.IdAssignedBy.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

        //        tasks = tasks.Where(t => assignedByTeam.Contains((t.IdAssignedBy ?? 0)));
        //    }

        //    if (!string.IsNullOrEmpty(advancedSearchParameters.IdAssignedTo))
        //    {
        //        List<int> assignedToTeam = advancedSearchParameters.IdAssignedTo != null && !string.IsNullOrEmpty(advancedSearchParameters.IdAssignedTo) ? (advancedSearchParameters.IdAssignedTo.Split('-') != null && advancedSearchParameters.IdAssignedTo.Split('-').Any() ? advancedSearchParameters.IdAssignedTo.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

        //        tasks = tasks.Where(t => assignedToTeam.Contains((t.IdAssignedTo ?? 0)));
        //    }

        //    if (advancedSearchParameters.AssignedFromDate != null)
        //    {
        //        advancedSearchParameters.AssignedFromDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.AssignedFromDate), DateTimeKind.Utc);
        //        //   advancedSearchParameters.AssignedFromDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.AssignedFromDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
        //        tasks = tasks.Where(t => DbFunctions.TruncateTime(t.AssignedDate) >= DbFunctions.TruncateTime(advancedSearchParameters.AssignedFromDate));
        //    }

        //    if (advancedSearchParameters.AssignedToDate != null)
        //    {
        //        advancedSearchParameters.AssignedToDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.AssignedToDate), DateTimeKind.Utc);
        //        //advancedSearchParameters.AssignedToDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.AssignedToDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
        //        tasks = tasks.Where(t => DbFunctions.TruncateTime(t.AssignedDate) <= DbFunctions.TruncateTime(advancedSearchParameters.AssignedToDate));
        //    }

        //    if (advancedSearchParameters.CompletedFromDate != null)
        //    {
        //        advancedSearchParameters.CompletedFromDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.CompletedFromDate), DateTimeKind.Utc);
        //        //  advancedSearchParameters.CompletedFromDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.CompletedFromDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
        //        tasks = tasks.Where(t => DbFunctions.TruncateTime(t.CompleteBy) >= DbFunctions.TruncateTime(advancedSearchParameters.CompletedFromDate));
        //    }

        //    if (advancedSearchParameters.CompletedToDate != null)
        //    {
        //        advancedSearchParameters.CompletedToDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.CompletedToDate), DateTimeKind.Utc);
        //        //advancedSearchParameters.CompletedToDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.CompletedToDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
        //        tasks = tasks.Where(t => DbFunctions.TruncateTime(t.CompleteBy) <= DbFunctions.TruncateTime(advancedSearchParameters.CompletedToDate));
        //    }

        //    if (advancedSearchParameters.ClosedFromDate != null)
        //    {
        //        advancedSearchParameters.ClosedFromDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.ClosedFromDate), DateTimeKind.Utc);
        //        // advancedSearchParameters.ClosedFromDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.ClosedFromDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
        //        tasks = tasks.Where(t => DbFunctions.TruncateTime(t.ClosedDate) >= DbFunctions.TruncateTime(advancedSearchParameters.ClosedFromDate));
        //    }

        //    if (advancedSearchParameters.ClosedToDate != null)
        //    {
        //        advancedSearchParameters.ClosedToDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.ClosedToDate), DateTimeKind.Utc);
        //        // advancedSearchParameters.ClosedToDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.ClosedToDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
        //        tasks = tasks.Where(t => DbFunctions.TruncateTime(t.ClosedDate) <= DbFunctions.TruncateTime(advancedSearchParameters.ClosedToDate));
        //    }
        //    List<int> jobstatus = new List<int>();

        //    if (advancedSearchParameters.IsActiveJob)
        //    {
        //        jobstatus.Add(JobStatus.Active.GetHashCode());
        //    }
        //    if (advancedSearchParameters.IsHoldJob)
        //    {
        //        jobstatus.Add(JobStatus.Hold.GetHashCode());
        //    }
        //    if (advancedSearchParameters.IsCompletedJob)
        //    {
        //        jobstatus.Add(JobStatus.Close.GetHashCode());
        //    }
        //    //Generic task changes
        //    tasks = tasks.Where(x =>
        //            (x.IdJob != null || x.IsGeneric == true && (advancedSearchParameters.IsActiveJob || advancedSearchParameters.IsHoldJob || advancedSearchParameters.IsCompletedJob)) ||
        //            (x.IdRfp != null && advancedSearchParameters.IsRfp) ||
        //            (x.IdCompany != null && advancedSearchParameters.IsCompany) ||
        //            (x.IdContact != null && advancedSearchParameters.IsContact) ||
        //            (!advancedSearchParameters.IsActiveJob
        //            && !advancedSearchParameters.IsHoldJob
        //            && !advancedSearchParameters.IsCompletedJob
        //            && !advancedSearchParameters.IsRfp
        //            && !advancedSearchParameters.IsCompany
        //            && !advancedSearchParameters.IsContact));

        //    if (advancedSearchParameters.IdJob != null && advancedSearchParameters.IdJob > 0)
        //    {
        //        tasks = tasks.Where(t => t.IdJob == advancedSearchParameters.IdJob);
        //    }

        //    if (advancedSearchParameters.IdRfp != null && advancedSearchParameters.IdRfp > 0)
        //    {
        //        tasks = tasks.Where(t => t.IdRfp == advancedSearchParameters.IdRfp);
        //    }

        //    if (advancedSearchParameters.IdContact != null && advancedSearchParameters.IdContact > 0)
        //    {
        //        tasks = tasks.Where(t => t.IdContact == advancedSearchParameters.IdContact);
        //    }

        //    if (advancedSearchParameters.IdCompany != null && advancedSearchParameters.IdCompany > 0)
        //    {
        //        tasks = tasks.Where(t => t.IdCompany == advancedSearchParameters.IdCompany);
        //    }


        //    IQueryable<TaskDetailsDTO> tasklists = tasks.AsEnumerable().Select(j => Format(j)).AsQueryable();

        //    if (advancedSearchParameters.ClosedFromDate != null)
        //    {
        //        tasklists = tasklists.AsEnumerable().Where(x => x.ClosedDate.Date >= Convert.ToDateTime(advancedSearchParameters.ClosedFromDate).Date && x.ClosedDate.Date != DateTime.MinValue.Date).AsQueryable();
        //    }
        //    if (advancedSearchParameters.ClosedToDate != null)
        //    {
        //        tasklists = tasklists.AsEnumerable().Where(x => x.ClosedDate.Date <= Convert.ToDateTime(advancedSearchParameters.ClosedToDate).Date && x.ClosedDate.Date != DateTime.MinValue.Date).AsQueryable();
        //    }
        //    //Generic Task Changes
        //    if (jobstatus != null && jobstatus.Count > 0)
        //    {
        //        tasklists = tasklists.AsEnumerable().Where(x => jobstatus.Contains(x.IdJobStatus.Value) || x.IsGeneric == true || ((x.IdRfp != null && advancedSearchParameters.IsRfp) ||
        //            (x.IdCompany != null && advancedSearchParameters.IsCompany) ||
        //            (x.IdContact != null && advancedSearchParameters.IsContact))).AsQueryable();
        //    }

        //    var result = tasklists
        //                  .AsEnumerable()
        //                  .Select(j => j)
        //                  .AsQueryable()
        //                  .DataTableParameters(advancedSearchParameters, out recordsFiltered)
        //                  .ToList();
        //    //.OrderBy(x => x.ClosedDate).ThenBy(x => x.CompleteBy);

        //    result = OrderTaskDetails(result);

        //    List<TaskDetailsDTO> objfinal = new List<TaskDetailsDTO>();

        //    var res = result.Where(d => d.BadgeClass == "red" && d.BadgeClass != "orange" && d.BadgeClass != "blue" && d.BadgeClass != "yellow" && d.BadgeClass != "green" && d.BadgeClass != "grey" && d.IdTaskStatus != 3).OrderBy(d => d.CompleteBy).ToList();
        //    objfinal.AddRange(res);

        //    var res1 = result.Where(d => d.BadgeClass != "red" && d.BadgeClass == "orange" && d.BadgeClass != "blue" && d.BadgeClass != "yellow" && d.BadgeClass != "green" && d.BadgeClass != "grey" && d.IdTaskStatus != 3).OrderBy(d => d.CompleteBy).ToList();
        //    objfinal.AddRange(res1);

        //    var res2 = result.Where(d => d.BadgeClass != "red" && d.BadgeClass != "orange" && d.BadgeClass == "blue" && d.BadgeClass != "yellow" && d.BadgeClass != "green" && d.BadgeClass != "grey" && d.IdTaskStatus != 3).OrderBy(d => d.CompleteBy).ToList();
        //    objfinal.AddRange(res2);

        //    var res3 = result.Where(d => d.BadgeClass != "red" && d.BadgeClass != "orange" && d.BadgeClass != "blue" && d.BadgeClass == "yellow" && d.BadgeClass != "green" && d.BadgeClass != "grey" && d.IdTaskStatus != 3).OrderBy(d => d.CompleteBy).ToList();
        //    objfinal.AddRange(res3);

        //    var res4 = result.Where(d => d.BadgeClass != "red" && d.BadgeClass != "orange" && d.BadgeClass != "blue" && d.BadgeClass != "yellow" && d.BadgeClass == "green" && d.BadgeClass != "grey" && d.IdTaskStatus != 3).OrderBy(d => d.CompleteBy).ToList();
        //    objfinal.AddRange(res4);
        //    var res5 = result.Where(d => d.BadgeClass != "red" && d.BadgeClass != "orange" && d.BadgeClass != "blue" && d.BadgeClass != "yellow" && d.BadgeClass != "green" && d.BadgeClass == "grey" && (d.IdTaskStatus == 3 || d.IdTaskStatus == 4)).OrderBy(d => d.CompleteBy).ToList();
        //    objfinal.AddRange(res5);

        //    List<TaskDetailsDTO> sortJobs = new List<TaskDetailsDTO>();
        //    if (advancedSearchParameters.IdJob == null || advancedSearchParameters.IdJob != 0)
        //    {
        //        var minimumjobsno = objfinal.Where(d => d.IdTaskStatus == 1).OrderBy(d => d.CompleteBy).Select(d => d.IdJob).Distinct().ToList();


        //        foreach (var itemjob in minimumjobsno)
        //        {
        //            var resjobs = objfinal.Where(d => d.IdJob == itemjob && d.IdTaskStatus == 1).OrderBy(d => d.IdJob).ThenBy(d => d.CompleteBy).ToList();
        //            sortJobs.AddRange(resjobs);
        //        }

        //        var minimumcompjobsno = objfinal.Where(d => (d.IdTaskStatus == 3 || d.IdTaskStatus == 4)).OrderBy(d => d.CompleteBy).Select(d => d.IdJob).Distinct().ToList();

        //        foreach (var itemjob in minimumcompjobsno)
        //        {
        //            var resjobs = objfinal.Where(d => d.IdJob == itemjob && d.BadgeClass != "red" && d.BadgeClass != "orange" && d.BadgeClass != "blue" && d.BadgeClass != "yellow" && d.BadgeClass != "green" && d.BadgeClass == "grey" && (d.IdTaskStatus == 3 || d.IdTaskStatus == 4)).OrderBy(d => d.IdJob).ThenBy(d => d.CompleteBy).ToList();
        //            sortJobs.AddRange(resjobs);
        //        }
        //    }
        //    else
        //    {
        //        sortJobs.AddRange(objfinal);
        //    }
        //    return Ok(new DataTableResponse
        //    {
        //        Draw = advancedSearchParameters.Draw,
        //        RecordsFiltered = recordsFiltered,
        //        RecordsTotal = recordsTotal,
        //        Data = sortJobs.Distinct()
        //    });
        //}


        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetTasks([FromUri] TaskAdvancedSearchParameters advancedSearchParameters)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var recordsTotal = rpoContext.Tasks.Count();
            var recordsFiltered = recordsTotal;

            IQueryable<Task> tasks = rpoContext.Tasks
                                        .Include("Rfp.RfpAddress.Borough")
                                        .Include("Job.RfpAddress.Borough")
                                        .Include("AssignedTo")
                                        .Include("AssignedBy")
                                        .Include("TaskType")
                                        .Include("TaskStatus")
                                        .Include("Notes")
                                        .Include("JobFeeSchedule")
                                        .Include("Examiner")
                                        .Include("JobApplication.JobApplicationType")
                                       .Include("JobViolation")
                                        .Include("LastModified")
                                        .Include("CreatedByEmployee").AsQueryable();



            if (advancedSearchParameters.GlobalSearchType != null && advancedSearchParameters.GlobalSearchType > 0)
            {
                switch ((GlobalSearchType)advancedSearchParameters.GlobalSearchType)
                {
                    case GlobalSearchType.Task:
                        tasks = tasks.Where(r => r.TaskNumber.Contains(advancedSearchParameters.GlobalSearchText.Trim()));
                        break;
                }
            }
            if (advancedSearchParameters.IdBorough != null && advancedSearchParameters.IdBorough > 0)
            {
                tasks = tasks.Where(t => t.Rfp.RfpAddress.IdBorough == advancedSearchParameters.IdBorough || t.Job.RfpAddress.IdBorough == advancedSearchParameters.IdBorough);
            }

            if (!string.IsNullOrWhiteSpace(advancedSearchParameters.HouseNumber))
            {
                tasks = tasks.Where(t => t.Rfp.RfpAddress.HouseNumber.Contains(advancedSearchParameters.HouseNumber) || t.Job.RfpAddress.HouseNumber.Contains(advancedSearchParameters.HouseNumber));
            }

            if (!string.IsNullOrWhiteSpace(advancedSearchParameters.Street))
            {
                tasks = tasks.Where(t => t.Rfp.RfpAddress.Street.Contains(advancedSearchParameters.Street) || t.Job.RfpAddress.Street.Contains(advancedSearchParameters.Street));
            }

            if (advancedSearchParameters.IdTaskType != null && advancedSearchParameters.IdTaskType > 0)
            {
                tasks = tasks.Where(t => t.IdTaskType == advancedSearchParameters.IdTaskType);
            }

            if (advancedSearchParameters.IdTaskStatus != null && advancedSearchParameters.IdTaskStatus > 0)
            {
                tasks = tasks.Where(t => t.IdTaskStatus == advancedSearchParameters.IdTaskStatus);
            }

            if (!string.IsNullOrEmpty(advancedSearchParameters.IdAssignedBy))
            {
                List<int> assignedByTeam = advancedSearchParameters.IdAssignedBy != null && !string.IsNullOrEmpty(advancedSearchParameters.IdAssignedBy) ? (advancedSearchParameters.IdAssignedBy.Split('-') != null && advancedSearchParameters.IdAssignedBy.Split('-').Any() ? advancedSearchParameters.IdAssignedBy.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                tasks = tasks.Where(t => assignedByTeam.Contains((t.IdAssignedBy ?? 0)));
            }

            if (!string.IsNullOrEmpty(advancedSearchParameters.IdAssignedTo))
            {
                List<int> assignedToTeam = advancedSearchParameters.IdAssignedTo != null && !string.IsNullOrEmpty(advancedSearchParameters.IdAssignedTo) ? (advancedSearchParameters.IdAssignedTo.Split('-') != null && advancedSearchParameters.IdAssignedTo.Split('-').Any() ? advancedSearchParameters.IdAssignedTo.Split('-').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                tasks = tasks.Where(t => assignedToTeam.Contains((t.IdAssignedTo ?? 0)));
            }

            if (advancedSearchParameters.AssignedFromDate != null)
            {
                advancedSearchParameters.AssignedFromDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.AssignedFromDate), DateTimeKind.Utc);
                //   advancedSearchParameters.AssignedFromDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.AssignedFromDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                tasks = tasks.Where(t => DbFunctions.TruncateTime(t.AssignedDate) >= DbFunctions.TruncateTime(advancedSearchParameters.AssignedFromDate));
            }

            if (advancedSearchParameters.AssignedToDate != null)
            {
                advancedSearchParameters.AssignedToDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.AssignedToDate), DateTimeKind.Utc);
                //advancedSearchParameters.AssignedToDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.AssignedToDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                tasks = tasks.Where(t => DbFunctions.TruncateTime(t.AssignedDate) <= DbFunctions.TruncateTime(advancedSearchParameters.AssignedToDate));
            }

            if (advancedSearchParameters.CompletedFromDate != null)
            {
                advancedSearchParameters.CompletedFromDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.CompletedFromDate), DateTimeKind.Utc);
                //  advancedSearchParameters.CompletedFromDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.CompletedFromDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                tasks = tasks.Where(t => DbFunctions.TruncateTime(t.CompleteBy) >= DbFunctions.TruncateTime(advancedSearchParameters.CompletedFromDate));
            }

            if (advancedSearchParameters.CompletedToDate != null)
            {
                advancedSearchParameters.CompletedToDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.CompletedToDate), DateTimeKind.Utc);
                //advancedSearchParameters.CompletedToDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.CompletedToDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                tasks = tasks.Where(t => DbFunctions.TruncateTime(t.CompleteBy) <= DbFunctions.TruncateTime(advancedSearchParameters.CompletedToDate));
            }

            if (advancedSearchParameters.ClosedFromDate != null)
            {
                advancedSearchParameters.ClosedFromDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.ClosedFromDate), DateTimeKind.Utc);
                // advancedSearchParameters.ClosedFromDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.ClosedFromDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                tasks = tasks.Where(t => DbFunctions.TruncateTime(t.ClosedDate) >= DbFunctions.TruncateTime(advancedSearchParameters.ClosedFromDate));
            }

            if (advancedSearchParameters.ClosedToDate != null)
            {
                advancedSearchParameters.ClosedToDate = DateTime.SpecifyKind(Convert.ToDateTime(advancedSearchParameters.ClosedToDate), DateTimeKind.Utc);
                // advancedSearchParameters.ClosedToDate = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(advancedSearchParameters.ClosedToDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
                tasks = tasks.Where(t => DbFunctions.TruncateTime(t.ClosedDate) <= DbFunctions.TruncateTime(advancedSearchParameters.ClosedToDate));
            }
            List<int> jobstatus = new List<int>();

            if (advancedSearchParameters.IsActiveJob)
            {
                jobstatus.Add(JobStatus.Active.GetHashCode());
            }
            if (advancedSearchParameters.IsHoldJob)
            {
                jobstatus.Add(JobStatus.Hold.GetHashCode());
            }
            if (advancedSearchParameters.IsCompletedJob)
            {
                jobstatus.Add(JobStatus.Close.GetHashCode());
            }
            //Generic task changes
            tasks = tasks.Where(x =>
                    (x.IdJob != null || x.IsGeneric == true && (advancedSearchParameters.IsActiveJob || advancedSearchParameters.IsHoldJob || advancedSearchParameters.IsCompletedJob)) ||
                    (x.IdRfp != null && advancedSearchParameters.IsRfp) ||
                    (x.IdCompany != null && advancedSearchParameters.IsCompany) ||
                    (x.IdContact != null && advancedSearchParameters.IsContact) ||
                    (!advancedSearchParameters.IsActiveJob
                    && !advancedSearchParameters.IsHoldJob
                    && !advancedSearchParameters.IsCompletedJob
                    && !advancedSearchParameters.IsRfp
                    && !advancedSearchParameters.IsCompany
                    && !advancedSearchParameters.IsContact));

            if (advancedSearchParameters.IdJob != null && advancedSearchParameters.IdJob > 0)
            {
                tasks = tasks.Where(t => t.IdJob == advancedSearchParameters.IdJob);
            }

            if (advancedSearchParameters.IdRfp != null && advancedSearchParameters.IdRfp > 0)
            {
                tasks = tasks.Where(t => t.IdRfp == advancedSearchParameters.IdRfp);
            }

            if (advancedSearchParameters.IdContact != null && advancedSearchParameters.IdContact > 0)
            {
                tasks = tasks.Where(t => t.IdContact == advancedSearchParameters.IdContact);
            }

            if (advancedSearchParameters.IdCompany != null && advancedSearchParameters.IdCompany > 0)
            {
                tasks = tasks.Where(t => t.IdCompany == advancedSearchParameters.IdCompany);
            }

            //comeented by M.B
            //IQueryable<TaskDetailsDTO> tasklists = tasks.AsEnumerable().Select(j => Format1(j)).AsQueryable();
            IQueryable<TasklistDTO> tasklists = tasks.AsEnumerable().Select(j => Format1(j)).AsQueryable();


            if (advancedSearchParameters.ClosedFromDate != null)
            {
                tasklists = tasklists.AsEnumerable().Where(x => x.ClosedDate.Date >= Convert.ToDateTime(advancedSearchParameters.ClosedFromDate).Date && x.ClosedDate.Date != DateTime.MinValue.Date).AsQueryable();
            }
            if (advancedSearchParameters.ClosedToDate != null)
            {
                tasklists = tasklists.AsEnumerable().Where(x => x.ClosedDate.Date <= Convert.ToDateTime(advancedSearchParameters.ClosedToDate).Date && x.ClosedDate.Date != DateTime.MinValue.Date).AsQueryable();
            }
            //Generic Task Changes
            if (jobstatus != null && jobstatus.Count > 0)
            {
                tasklists = tasklists.AsEnumerable().Where(x => jobstatus.Contains(x.IdJobStatus.Value) || x.IsGeneric == true || ((x.IdRfp != null && advancedSearchParameters.IsRfp) ||
                    (x.IdCompany != null && advancedSearchParameters.IsCompany) ||
                    (x.IdContact != null && advancedSearchParameters.IsContact))).AsQueryable();
            }

            //var result = tasklists
            //              .AsEnumerable()
            //              .Select(j => j)
            //              .AsQueryable()
            //              .DataTableParameters(advancedSearchParameters, out recordsFiltered)
            //              .ToList().Skip(0).Take(10);
            ////.OrderBy(x => x.ClosedDate).ThenBy(x => x.CompleteBy);

            //result = OrderTaskDetails(result);
            var result = tasklists
                        .AsEnumerable()
                        .Select(j => j)
                        .AsQueryable()
                        .DataTableParameters(advancedSearchParameters, out recordsFiltered)
                        .ToList();
            //orignal
            //var result = tasklists
            //             .AsEnumerable()
            //             .Select(j => j)
            //             .AsQueryable()
            //             .DataTableParameters(advancedSearchParameters, out recordsFiltered)
            //             .ToList();
            ////.OrderBy(x => x.ClosedDate).ThenBy(x => x.CompleteBy);

            result = OrderTaskDetails1(result);


            List<TasklistDTO> objfinal = new List<TasklistDTO>();

            var res = result.Where(d => d.BadgeClass == "red" && d.BadgeClass != "orange" && d.BadgeClass != "blue" && d.BadgeClass != "yellow" && d.BadgeClass != "green" && d.BadgeClass != "grey" && d.IdTaskStatus != 3).OrderBy(d => d.CompleteBy).ToList();
            objfinal.AddRange(res);

            var res1 = result.Where(d => d.BadgeClass != "red" && d.BadgeClass == "orange" && d.BadgeClass != "blue" && d.BadgeClass != "yellow" && d.BadgeClass != "green" && d.BadgeClass != "grey" && d.IdTaskStatus != 3).OrderBy(d => d.CompleteBy).ToList();
            objfinal.AddRange(res1);

            var res2 = result.Where(d => d.BadgeClass != "red" && d.BadgeClass != "orange" && d.BadgeClass == "blue" && d.BadgeClass != "yellow" && d.BadgeClass != "green" && d.BadgeClass != "grey" && d.IdTaskStatus != 3).OrderBy(d => d.CompleteBy).ToList();
            objfinal.AddRange(res2);

            var res3 = result.Where(d => d.BadgeClass != "red" && d.BadgeClass != "orange" && d.BadgeClass != "blue" && d.BadgeClass == "yellow" && d.BadgeClass != "green" && d.BadgeClass != "grey" && d.IdTaskStatus != 3).OrderBy(d => d.CompleteBy).ToList();
            objfinal.AddRange(res3);

            var res4 = result.Where(d => d.BadgeClass != "red" && d.BadgeClass != "orange" && d.BadgeClass != "blue" && d.BadgeClass != "yellow" && d.BadgeClass == "green" && d.BadgeClass != "grey" && d.IdTaskStatus != 3).OrderBy(d => d.CompleteBy).ToList();
            objfinal.AddRange(res4);
            var res5 = result.Where(d => d.BadgeClass != "red" && d.BadgeClass != "orange" && d.BadgeClass != "blue" && d.BadgeClass != "yellow" && d.BadgeClass != "green" && d.BadgeClass == "grey" && (d.IdTaskStatus == 3 || d.IdTaskStatus == 4)).OrderBy(d => d.CompleteBy).ToList();
            objfinal.AddRange(res5);

            List<TasklistDTO> sortJobs = new List<TasklistDTO>();
            if (advancedSearchParameters.IdJob == null || advancedSearchParameters.IdJob != 0)
            {
                var minimumjobsno = objfinal.Where(d => d.IdTaskStatus == 1).OrderBy(d => d.CompleteBy).Select(d => d.IdJob).Distinct().ToList();


                foreach (var itemjob in minimumjobsno)
                {
                    var resjobs = objfinal.Where(d => d.IdJob == itemjob && d.IdTaskStatus == 1).OrderBy(d => d.IdJob).ThenBy(d => d.CompleteBy).ToList();
                    sortJobs.AddRange(resjobs);
                }

                var minimumcompjobsno = objfinal.Where(d => (d.IdTaskStatus == 3 || d.IdTaskStatus == 4)).OrderBy(d => d.CompleteBy).Select(d => d.IdJob).Distinct().ToList();

                foreach (var itemjob in minimumcompjobsno)
                {
                    var resjobs = objfinal.Where(d => d.IdJob == itemjob && d.BadgeClass != "red" && d.BadgeClass != "orange" && d.BadgeClass != "blue" && d.BadgeClass != "yellow" && d.BadgeClass != "green" && d.BadgeClass == "grey" && (d.IdTaskStatus == 3 || d.IdTaskStatus == 4)).OrderBy(d => d.IdJob).ThenBy(d => d.CompleteBy).ToList();
                    sortJobs.AddRange(resjobs);
                }
            }
            else
            {
                sortJobs.AddRange(objfinal);
            }
            return Ok(new DataTableResponse
            {
                Draw = advancedSearchParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                // Data = sortJobs.Distinct()
                Data = sortJobs.Distinct()
            });
        }
        /// <summary>
        /// Gets the task.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Gets the task.</returns>
        [ResponseType(typeof(TaskDetailsDTO))]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult GetTask(int id)
        {
            Task task = rpoContext.Tasks
                        .Include("AssignedTo")
                        .Include("AssignedBy")
                        .Include("Examiner")
                        .Include("TaskType")
                        .Include("TaskStatus")
                        .Include("Notes")
                        .Include("Company")
                        .Include("Rfp")
                        .Include("Job")
                        .Include("Contact")
                        .Include("JobApplication")
                        .Include("JobApplication.JobApplicationType")
                        .Include("LastModified")
                        .Include("JobFeeSchedule.RfpWorkType.Parent.Parent.Parent.Parent")
                        .Include("RfpJobType.Parent.Parent.Parent.Parent")
                        .Include("TaskJobDocuments.JobDocument.DocumentMaster")
                        .Include("CreatedByEmployee")
                        .FirstOrDefault(x => x.Id == id);
            if (task == null)
            {
                return this.NotFound();
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return Ok(FormatTasks(task));
        }

        /// <summary>
        /// Gets the task note.
        /// </summary>
        /// <param name="idTask">The identifier task.</param>
        /// <returns>Gets the task note List.</returns>
        [Authorize]
        [RpoAuthorize]
        [Route("api/tasks/{idTask}/TaskNotes")]
        [ResponseType(typeof(TaskNote))]
        public IHttpActionResult GetTaskNote(int idTask)
        {
            Task task = rpoContext.Tasks.Find(idTask);
            if (task == null)
            {
                return this.NotFound();
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            var result = rpoContext.TaskNotes
                .Include("LastModified")
                .Where(x => x.IdTask == idTask)
                .AsEnumerable()
                .Select(jc => new
                {
                    Id = jc.Id,
                    TaskNumber = task.TaskNumber,
                    IdTask = jc.IdTask,
                    LastModifiedDate = jc.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(jc.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jc.LastModifiedDate,
                    LastModifiedBy = jc.LastModifiedBy != null ? jc.LastModifiedBy : jc.CreatedBy,
                    LastModified = jc.LastModified != null ? jc.LastModified.FirstName + " " + jc.LastModified.LastName : (jc.CreatedByEmployee != null ? jc.CreatedByEmployee.FirstName + " " + jc.CreatedByEmployee.LastName : string.Empty),
                    Notes = jc.Notes
                }).OrderByDescending(x => x.LastModifiedDate);

            return Ok(result);
        }

        /// <summary>
        /// Puts the task.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="jobTask">The job task.</param>
        /// <returns>update the task.</returns>
        [Authorize]
        [RpoAuthorize]
        [HttpPut]
        [ResponseType(typeof(Task))]
        public IHttpActionResult PutTask(int id, TaskRequestDTO jobTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jobTask.Id)
            {
                return BadRequest();
            }
            var task = rpoContext.Tasks.Find(id);

            if (task.IdMilestone == jobTask.IdJobFeeSchedule && task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() && jobTask.IsMilestone == true)
            {
                var jobMilestones = rpoContext.JobMilestones.FirstOrDefault(x => x.Id == task.IdMilestone);

                if (jobMilestones.Status == "Completed")
                {
                    throw new RpoBusinessException(StaticMessages.JobMilestoneAllreadyMessage);
                }
            }

            if (task.IdJobFeeSchedule == jobTask.IdJobFeeSchedule && task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() && jobTask.IsMilestone == false)
            {
                throw new RpoBusinessException(StaticMessages.FeeScheduleAllreadyMessage);
            }

            if (task.JobFeeSchedule != null && task.JobFeeSchedule.IsRemoved && task.JobFeeSchedule.Id == jobTask.IdJobFeeSchedule && jobTask.IsMilestone == false)
            {
                throw new RpoBusinessException(StaticMessages.FeeScheduleRemovedTaskNotEditMessage);
            }

            int? previousStatus = task.IdTaskStatus;
            JobBillingType? previousBillingType = task.JobBillingType;
            int? previousIdJobFeeSchedule = task.IdJobFeeSchedule;
            int? previousIdRfpJobType = task.IdRfpJobType;
            double? previousServiceQuantity = task.ServiceQuantity;
            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            int? oldTaskStatus = task.IdTaskStatus;

            if (jobTask.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            {
                if (jobTask.IdJob != null && jobTask.IdJob > 0)
                {
                    if (jobTask.JobBillingType == JobBillingType.ScopeBilling && jobTask.IsMilestone == false)
                    {
                        var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == jobTask.IdJobFeeSchedule);

                        //if (jobFeeSchedules.Status == "Completed")
                        //{
                        //    throw new RpoBusinessException(StaticMessages.FeeScheduleAllreadyMessage);
                        //}

                        if (jobFeeSchedules.QuantityPending < jobTask.ServiceQuantity)
                        {
                            throw new RpoBusinessException(StaticMessages.FeeScheduleQuantityExceedsLimitMessage);
                        }
                    }
                }
            }

            if (jobTask.IdWorkPermitType != null && jobTask.IdWorkPermitType.Count() > 0)
            {
                task.IdWorkPermitType = string.Join(",", jobTask.IdWorkPermitType.Select(x => x.ToString()));
            }
            else
            {
                task.IdWorkPermitType = string.Empty;
            }

            if (jobTask.DocumentsToDelete != null)
            {
                List<int> deletedDocs = jobTask.DocumentsToDelete.ToList();
                rpoContext.TaskDocuments.RemoveRange(rpoContext.TaskDocuments.Where(ac => ac.IdTask == task.Id && deletedDocs.Any(eacIds => eacIds == ac.Id)));
            }

            if (jobTask.DocumentsToDelete != null)
            {
                foreach (var item in jobTask.DocumentsToDelete)
                {
                    int taskDocumentId = Convert.ToInt32(item);
                    TaskDocument taskDocument = rpoContext.TaskDocuments.Where(x => x.Id == taskDocumentId).FirstOrDefault();
                    if (taskDocument != null)
                    {
                        rpoContext.TaskDocuments.Remove(taskDocument);
                        var path = HttpRuntime.AppDomainAppPath;
                        string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.TaskDocumentPath));
                        string directoryDelete = Convert.ToString(taskDocument.Id) + "_" + taskDocument.DocumentPath;
                        string deletefilename = System.IO.Path.Combine(directoryName, directoryDelete);

                        if (File.Exists(deletefilename))
                        {
                            File.Delete(deletefilename);
                        }
                    }
                }
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            // task.AssignedDate = jobTask.AssignedDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobTask.AssignedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTask.AssignedDate;
            //  task.CompleteBy = jobTask.CompleteBy != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobTask.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTask.CompleteBy;
            task.AssignedDate = jobTask.AssignedDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobTask.AssignedDate), DateTimeKind.Utc) : jobTask.AssignedDate;
            task.CompleteBy = jobTask.CompleteBy != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobTask.CompleteBy), DateTimeKind.Utc) : jobTask.CompleteBy;
            task.GeneralNotes = jobTask.GeneralNotes;
            task.IdAssignedBy = jobTask.IdAssignedBy;
            task.IdAssignedTo = jobTask.IdAssignedTo;
            task.IdJob = jobTask.IdJob;
            task.IdJobApplication = jobTask.IdJobApplication;
            task.IdJobViolation = jobTask.IdJobViolation;

            task.IdTaskStatus = jobTask.IdTaskStatus;
            task.IdTaskType = jobTask.IdTaskType;
            task.IdRfp = jobTask.IdRfp;
            task.IdContact = jobTask.IdContact;
            task.IdCompany = jobTask.IdCompany;
            task.IdExaminer = jobTask.IdExaminer;
            task.JobBillingType = jobTask.JobBillingType;
            task.IdMilestone = null;
            task.IdJobFeeSchedule = null;
            if (jobTask.IsMilestone == true)
            {
                task.IdMilestone = jobTask.IdJobFeeSchedule;
            }
            else
            {
                task.IdJobFeeSchedule = jobTask.IdJobFeeSchedule;
            }
            task.IdRfpJobType = jobTask.IdRfpJobType;
            task.ServiceQuantity = jobTask.ServiceQuantity;
            task.TaskDuration = jobTask.TaskDuration;
            task.IdJobType = jobTask.IdJobType;
            task.IsGeneric = jobTask.IsGeneric;

            task.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                task.LastModifiedBy = employee.Id;
            }

            rpoContext.SaveChanges();
            /**********************************************/

            if (previousStatus == EnumTaskStatus.Completed.GetHashCode() && jobTask.IsMilestone == false)
            {
                if (task.JobBillingType != previousBillingType)
                {
                    #region Not Equal Billing Point
                    if (task.JobBillingType == JobBillingType.ScopeBilling)
                    {
                        var jobFeeSchedule = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == previousIdJobFeeSchedule && x.IsFromScope == true);
                        if (jobFeeSchedule != null)
                        {
                            task.IdJobFeeSchedule = null;
                            task.IdRfpJobType = null;
                            rpoContext.SaveChanges();

                            List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedule.Id).ToList();
                            if (taskHistoryList != null && taskHistoryList.Count > 0)
                            {
                                rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                                rpoContext.SaveChanges();
                            }
                            rpoContext.JobFeeSchedules.Remove(jobFeeSchedule);
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (task.JobBillingType == JobBillingType.AdditionalBilling)
                    {
                        var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == previousIdJobFeeSchedule && x.IsFromScope != true);
                        jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending + previousServiceQuantity;
                        jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved - previousServiceQuantity;
                        jobFeeSchedules.Status = "";
                        rpoContext.SaveChanges();

                        List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedules.Id).ToList();
                        if (taskHistoryList != null && taskHistoryList.Count > 0)
                        {
                            rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                            rpoContext.SaveChanges();
                        }
                    }
                    else
                    {
                        switch (previousBillingType)
                        {
                            case JobBillingType.ScopeBilling:
                                {
                                    var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == previousIdJobFeeSchedule && x.IsFromScope == true);
                                    jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending + previousServiceQuantity;
                                    jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved - previousServiceQuantity;
                                    jobFeeSchedules.Status = "";
                                    rpoContext.SaveChanges();

                                    List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedules.Id).ToList();
                                    if (taskHistoryList != null && taskHistoryList.Count > 0)
                                    {
                                        rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                                        rpoContext.SaveChanges();
                                    }
                                    break;
                                }
                            case JobBillingType.AdditionalBilling:
                                {
                                    var jobFeeSchedule = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == previousIdJobFeeSchedule && x.IsFromScope != true);
                                    if (jobFeeSchedule != null)
                                    {
                                        task.IdJobFeeSchedule = null;
                                        task.IdRfpJobType = null;
                                        rpoContext.SaveChanges();

                                        List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedule.Id).ToList();
                                        if (taskHistoryList != null && taskHistoryList.Count > 0)
                                        {
                                            rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                                            rpoContext.SaveChanges();
                                        }
                                        rpoContext.JobFeeSchedules.Remove(jobFeeSchedule);
                                        rpoContext.SaveChanges();
                                    }

                                    break;
                                }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Billingpoint is equal
                    if (task.JobBillingType == JobBillingType.ScopeBilling)
                    {
                        var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == previousIdJobFeeSchedule && x.IsFromScope == true);
                        jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending + previousServiceQuantity;
                        jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved - previousServiceQuantity;
                        jobFeeSchedules.Status = "";
                        rpoContext.SaveChanges();

                        List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedules.Id).ToList();
                        if (taskHistoryList != null && taskHistoryList.Count > 0)
                        {
                            rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                            rpoContext.SaveChanges();
                        }
                    }
                    else if (task.JobBillingType == JobBillingType.AdditionalBilling)
                    {
                        var jobFeeSchedule = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == previousIdJobFeeSchedule && x.IsFromScope != true);
                        if (jobFeeSchedule != null)
                        {
                            task.IdJobFeeSchedule = null;
                            task.IdRfpJobType = null;
                            rpoContext.SaveChanges();

                            List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedule.Id).ToList();
                            if (taskHistoryList != null && taskHistoryList.Count > 0)
                            {
                                rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                                rpoContext.SaveChanges();
                            }
                            rpoContext.JobFeeSchedules.Remove(jobFeeSchedule);
                            rpoContext.SaveChanges();
                        }
                    }
                    else
                    {
                        switch (previousBillingType)
                        {
                            case JobBillingType.ScopeBilling:
                                {
                                    var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == previousIdJobFeeSchedule && x.IsFromScope == true);
                                    jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending + previousServiceQuantity;
                                    jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved - previousServiceQuantity;
                                    jobFeeSchedules.Status = "";
                                    rpoContext.SaveChanges();

                                    List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedules.Id).ToList();
                                    if (taskHistoryList != null && taskHistoryList.Count > 0)
                                    {
                                        rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                                        rpoContext.SaveChanges();
                                    }
                                    break;
                                }
                            case JobBillingType.AdditionalBilling:
                                {
                                    var jobFeeSchedule = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == previousIdJobFeeSchedule && x.IsFromScope != true);
                                    if (jobFeeSchedule != null)
                                    {
                                        task.IdJobFeeSchedule = null;
                                        task.IdRfpJobType = null;
                                        rpoContext.SaveChanges();

                                        List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedule.Id).ToList();
                                        if (taskHistoryList != null && taskHistoryList.Count > 0)
                                        {
                                            rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                                            rpoContext.SaveChanges();
                                        }
                                        rpoContext.JobFeeSchedules.Remove(jobFeeSchedule);
                                        rpoContext.SaveChanges();
                                    }

                                    break;
                                }
                        }
                    }

                    #endregion
                }
            }

            /***********************************************/
            var taskJobDocuments = task.TaskJobDocuments.ToList();
            if (task.TaskJobDocuments != null)
            {
                if (jobTask.IdJobDocuments != null)
                {
                    foreach (TaskJobDocument item in taskJobDocuments)
                    {
                        var projectDetail = jobTask.IdJobDocuments.FirstOrDefault(x => x == item.IdJobDocument);
                        if (projectDetail <= 0)
                        {
                            this.rpoContext.TaskJobDocuments.Remove(item);
                        }
                    }
                }
                else
                {
                    this.rpoContext.TaskJobDocuments.RemoveRange(task.TaskJobDocuments);
                }
            }
            this.rpoContext.SaveChanges();

            if (jobTask.IdJobDocuments != null)
            {
                foreach (int item in jobTask.IdJobDocuments)
                {
                    TaskJobDocument taskJobDocument = rpoContext.TaskJobDocuments.FirstOrDefault(x => x.IdTask == task.Id && x.IdJobDocument == item);
                    if (taskJobDocument != null)
                    {
                        taskJobDocument.IdTask = task.Id;
                        taskJobDocument.IdJobDocument = item;
                        taskJobDocument.LastModifiedDate = DateTime.UtcNow;
                        taskJobDocument.LastModifiedBy = employee.Id;
                    }
                    else
                    {
                        taskJobDocument = new TaskJobDocument();
                        taskJobDocument.IdTask = task.Id;
                        taskJobDocument.IdJobDocument = item;
                        taskJobDocument.LastModifiedDate = DateTime.UtcNow;
                        taskJobDocument.LastModifiedBy = employee.Id;
                        taskJobDocument.CreatedDate = DateTime.UtcNow;
                        taskJobDocument.CreatedBy = employee.Id;
                        rpoContext.TaskJobDocuments.Add(taskJobDocument);
                    }
                }
            }

            rpoContext.SaveChanges();

            if (previousStatus == EnumTaskStatus.Completed.GetHashCode() && jobTask.IsMilestone == false)
            {
                if (task.IdTaskStatus != previousStatus)
                {
                    if (task.IdJob != null && task.IdJob > 0)
                    {
                        var jobFeeSchedule = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == task.IdJobFeeSchedule);

                        if (jobFeeSchedule != null)
                        {
                            if (task.JobBillingType == JobBillingType.ScopeBilling)
                            {
                                var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == task.IdJobFeeSchedule);
                                jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending + task.ServiceQuantity;
                                jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved - task.ServiceQuantity;
                                jobFeeSchedules.Status = "";
                                jobFeeSchedules.IsShow = false;
                                rpoContext.SaveChanges();
                            }

                            List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedule.Id).ToList();
                            if (taskHistoryList != null && taskHistoryList.Count > 0)
                            {
                                rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                                rpoContext.SaveChanges();
                            }
                            if (task.JobBillingType == JobBillingType.AdditionalBilling)
                            {
                                rpoContext.JobFeeSchedules.Remove(jobFeeSchedule);
                                rpoContext.SaveChanges();
                            }
                        }



                        if (task.IdRfpJobType != null && task.IdRfpJobType > 0)
                        {
                            JobFeeSchedule jobFeeSchedule1 = task.JobFeeSchedule;

                            if (jobFeeSchedule1 != null)
                            {
                                List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedule1.Id).ToList();
                                if (taskHistoryList != null && taskHistoryList.Count > 0)
                                {
                                    rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                                    rpoContext.SaveChanges();
                                }
                                rpoContext.JobFeeSchedules.Remove(jobFeeSchedule1);
                                rpoContext.SaveChanges();
                            }
                        }
                    }
                }
            }

            if (jobTask.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            {
                if (jobTask.IdJob != null && jobTask.IdJob > 0)
                {
                    if (task.JobBillingType == JobBillingType.ScopeBilling)
                    {
                        if (jobTask.IsMilestone == true)
                        {
                            if (jobTask.IdJobFeeSchedule != null && jobTask.IdJobFeeSchedule > 0 && jobTask.IdRfpJobType == null && jobTask.IsMilestone == true)
                            {
                                DirectMilestoneCompleted(task.Id, jobTask.IdJobFeeSchedule, jobTask.IdJob, "Completed", employee.Id);
                            }
                        }
                        else
                        {
                            var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == jobTask.IdJobFeeSchedule);

                            jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending - task.ServiceQuantity;
                            jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved + task.ServiceQuantity;

                            rpoContext.SaveChanges();

                            if (jobFeeSchedules.QuantityAchieved == jobFeeSchedules.Quantity)
                            {
                                jobFeeSchedules.Status = "Completed";
                                jobFeeSchedules.CompletedDate = DateTime.UtcNow;
                                jobFeeSchedules.LastModified = DateTime.UtcNow;
                                if (employee != null)
                                {
                                    jobFeeSchedules.ModifiedBy = employee.Id;
                                }
                                rpoContext.SaveChanges();
                            }

                            rpoContext.SaveChanges();
                        }

                        // Common.UpdateMilestoneStatus(jobFeeSchedules.Id, Hub);
                    }
                    else if (task.JobBillingType == JobBillingType.AdditionalBilling)
                    {
                        RfpJobType rfpJobType = rpoContext.RfpJobTypes.FirstOrDefault(x => x.Id == jobTask.IdRfpJobType);
                        JobFeeSchedule jobFeeSchedule = new JobFeeSchedule();
                        jobFeeSchedule.IdJob = Convert.ToInt32(jobTask.IdJob);
                        jobFeeSchedule.IdRfpWorkType = jobTask.IdRfpJobType;
                        jobFeeSchedule.IsInvoiced = false;
                        jobFeeSchedule.Quantity = jobTask.ServiceQuantity;
                        jobFeeSchedule.QuantityAchieved = jobTask.ServiceQuantity;
                        jobFeeSchedule.QuantityPending = 0;
                        jobFeeSchedule.Cost = rfpJobType.Cost;
                        jobFeeSchedule.TotalCost = (rfpJobType.Cost * jobFeeSchedule.Quantity);
                        jobFeeSchedule.Status = "Completed";
                        jobFeeSchedule.CompletedDate = DateTime.UtcNow;
                        jobFeeSchedule.LastModified = DateTime.UtcNow;
                        if (task.JobBillingType == JobBillingType.AdditionalBilling)
                        {
                            jobFeeSchedule.IsAdditionalService = true;
                            jobFeeSchedule.IsShow = true;
                            jobFeeSchedule.FromTask = true;
                        }

                        if (employee != null)
                        {
                            jobFeeSchedule.ModifiedBy = employee.Id;
                        }
                        rpoContext.JobFeeSchedules.Add(jobFeeSchedule);
                        rpoContext.SaveChanges();

                        task.IdJobFeeSchedule = jobFeeSchedule.Id;
                        rpoContext.SaveChanges();

                        //Common.UpdateMilestoneStatus(jobFeeSchedule.Id, Hub);
                    }
                }
            }

            if (task.ClosedDate == null && task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() && jobTask.IsMilestone == false)
            {
                task.ClosedDate = DateTime.UtcNow;
                // SendTaskCompletedNotification(task.Id, jobTask.IdJob);

                JobFeeSchedule jobFeeScheduletmp = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == task.IdJobFeeSchedule);
                string jobFeeScheduleDetailName = string.Empty;
                try
                {
                    jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeScheduletmp);
                }
                catch (Exception)
                {

                }

                //string taskHistoryMessage = TaskHistoryMessages.TaskCompletedHistoryMessage;
                string taskHistoryMessage = TaskHistoryMessages.TaskCompletedHistoryMessage
                                            .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                                            .Replace("##NewServiceStatus##", "Completed")
                                            .Replace("##OldServiceStatus##", "Pending")
                                            .Replace("##TaskId##", task.Id.ToString())
                                            .Replace("##TaskNumber##", task.TaskNumber)
                                            .Replace("##TaskType##", (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() != null ? (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() : "Not Set");

                string changeStatusScopeHistory = JobHistoryMessages.ChangeStatusScopeMessage
                       .Replace("##ServiceItemName##", !string.IsNullOrEmpty(jobFeeScheduleDetailName) ? jobFeeScheduleDetailName : JobHistoryMessages.NoSetstring);


                Common.SaveTaskHistory(employee.Id, changeStatusScopeHistory, task.Id, Convert.ToInt32(task.IdJobFeeSchedule));

            }
            else if (oldTaskStatus != task.IdTaskStatus && task.IdTaskStatus == EnumTaskStatus.Unattainable.GetHashCode())
            {
                SendTaskUnattainableNotification(task.Id, jobTask.IdJob);
            }
            else
            {
                task.ClosedDate = null;

            }
            try
            {
                rpoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            //if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            //{
            /// update the milestone status
            updateMilestoneStatus(task.Id, task.IdJobFeeSchedule, task.IdJob, employee.Id);
            /// update the additional status
            additionalCompleted(task.IdJobFeeSchedule, task.Id, task.IdJob, employee.Id);

            //  }
            return Ok(FormatDetails(task));

        }

        /// <summary>
        /// Posts the task.
        /// </summary>
        /// <param name="jobTask">The job task.</param>
        /// <returns>create new task.</returns>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskDetailsDTO))]
        public IHttpActionResult PostTask(TaskRequestDTO jobTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (jobTask.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            {
                if (jobTask.IdJob != null && jobTask.IdJob > 0)
                {
                    if (jobTask.IdJobFeeSchedule != null && jobTask.IdJobFeeSchedule > 0 && jobTask.IsMilestone == false)
                    {
                        var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == jobTask.IdJobFeeSchedule);
                        if (jobFeeSchedules.QuantityPending < jobTask.ServiceQuantity)
                        {
                            throw new RpoBusinessException(StaticMessages.FeeScheduleQuantityExceedsLimitMessage);
                        }
                    }
                }
            }


            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            var task = new Task();
            //task.AssignedDate = jobTask.AssignedDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobTask.AssignedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTask.AssignedDate;
            //task.CompleteBy = jobTask.CompleteBy != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(jobTask.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : jobTask.CompleteBy;
            task.AssignedDate = jobTask.AssignedDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobTask.AssignedDate), DateTimeKind.Utc) : jobTask.AssignedDate;
            task.CompleteBy = jobTask.CompleteBy != null ? DateTime.SpecifyKind(Convert.ToDateTime(jobTask.CompleteBy), DateTimeKind.Utc) : jobTask.CompleteBy;
            task.GeneralNotes = jobTask.GeneralNotes;
            task.IdAssignedBy = jobTask.IdAssignedBy;
            task.IdAssignedTo = jobTask.IdAssignedTo;
            task.IdJob = jobTask.IdJob;
            task.IdJobApplication = jobTask.IdJobApplication;
            task.IdJobViolation = jobTask.IdJobViolation;
            task.IsGeneric = jobTask.IsGeneric;

            if (jobTask.IdWorkPermitType != null && jobTask.IdWorkPermitType.Count() > 0)
            {
                task.IdWorkPermitType = string.Join(",", jobTask.IdWorkPermitType.Select(x => x.ToString()));
            }
            else
            {
                task.IdWorkPermitType = string.Empty;
            }
            task.IdTaskStatus = jobTask.IdTaskStatus;
            task.IdTaskType = jobTask.IdTaskType;
            task.IdRfp = jobTask.IdRfp;
            task.IdContact = jobTask.IdContact;
            task.IdCompany = jobTask.IdCompany;
            task.IdExaminer = jobTask.IdExaminer;
            task.JobBillingType = jobTask.JobBillingType;
            if (jobTask.IsMilestone == true)
            {
                task.IdMilestone = jobTask.IdJobFeeSchedule;
            }
            else
            {
                task.IdJobFeeSchedule = jobTask.IdJobFeeSchedule;
            }
            task.IdRfpJobType = jobTask.IdRfpJobType;
            task.ServiceQuantity = jobTask.ServiceQuantity;
            task.TaskDuration = jobTask.TaskDuration;
            task.IdJobType = jobTask.IdJobType;

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            task.LastModifiedDate = DateTime.UtcNow;
            task.CreatedDate = DateTime.UtcNow;
            if (employee != null)
            {
                task.CreatedBy = employee.Id;
            }

            if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            {
                task.ClosedDate = DateTime.UtcNow;
            }

            rpoContext.Tasks.Add(task);
            rpoContext.SaveChanges();
            if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            {
                //    SendTaskCompletedNotification(task.Id, jobTask.IdJob);
            }
            else if (task.IdTaskStatus == EnumTaskStatus.Unattainable.GetHashCode())
            {
                SendTaskUnattainableNotification(task.Id, jobTask.IdJob);
            }

            task.TaskNumber = task.Id.ToString(); //.ToString("000000");
            rpoContext.SaveChanges();

            if (jobTask.IdJobDocuments != null)
            {
                foreach (int item in jobTask.IdJobDocuments)
                {
                    TaskJobDocument taskJobDocument = rpoContext.TaskJobDocuments.FirstOrDefault(x => x.IdTask == task.Id && x.IdJobDocument == item);
                    if (taskJobDocument != null)
                    {
                        taskJobDocument.IdTask = task.Id;
                        taskJobDocument.IdJobDocument = item;
                        taskJobDocument.LastModifiedDate = DateTime.UtcNow;
                        taskJobDocument.LastModifiedBy = employee.Id;
                    }
                    else
                    {
                        taskJobDocument = new TaskJobDocument();
                        taskJobDocument.IdTask = task.Id;
                        taskJobDocument.IdJobDocument = item;
                        taskJobDocument.LastModifiedDate = DateTime.UtcNow;
                        taskJobDocument.LastModifiedBy = employee.Id;
                        taskJobDocument.CreatedDate = DateTime.UtcNow;
                        taskJobDocument.CreatedBy = employee.Id;
                        rpoContext.TaskJobDocuments.Add(taskJobDocument);
                    }
                }
            }

            rpoContext.SaveChanges();

            if (jobTask.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            {
                if (jobTask.IdJob != null && jobTask.IdJob > 0)
                {
                    if (jobTask.IdJobFeeSchedule != null && jobTask.IdJobFeeSchedule > 0 && jobTask.IdRfpJobType == null && jobTask.IsMilestone == true)
                    {
                        DirectMilestoneCompleted(task.Id, jobTask.IdJobFeeSchedule, jobTask.IdJob, "Completed", employee.Id);
                    }

                    if (jobTask.IdJobFeeSchedule != null && jobTask.IdJobFeeSchedule > 0 && jobTask.IdRfpJobType == null && jobTask.IsMilestone == false)
                    {
                        var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == jobTask.IdJobFeeSchedule);

                        jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending - task.ServiceQuantity;
                        jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved + task.ServiceQuantity;
                        if (jobTask.JobBillingType == JobBillingType.AdditionalBilling)
                        {
                            jobFeeSchedules.IsAdditionalService = true;
                            jobFeeSchedules.IsShow = true;
                            jobFeeSchedules.FromTask = true;
                        }
                        rpoContext.SaveChanges();
                        string ScopeStatus = string.Empty;
                        JobFeeSchedule jobFeeSchedule = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobTask.IdJobFeeSchedule);

                        if (jobFeeSchedule.QuantityAchieved == jobFeeSchedule.Quantity)
                        {
                            jobFeeSchedules.Status = "Completed";
                            jobFeeSchedules.CompletedDate = DateTime.UtcNow;
                            jobFeeSchedules.LastModified = DateTime.UtcNow;
                            if (employee != null)
                            {
                                jobFeeSchedules.ModifiedBy = employee.Id;
                            }
                            ScopeStatus = "Completed";
                            if (jobTask.JobBillingType == JobBillingType.AdditionalBilling)
                            {
                                jobFeeSchedules.IsAdditionalService = true;
                                jobFeeSchedules.IsShow = true;
                            }
                            rpoContext.SaveChanges();
                        }

                        string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeSchedule);

                        string taskHistoryMessage = TaskHistoryMessages.TaskCompletedHistoryMessage
                                                     .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                                                     .Replace("##NewServiceStatus##", ScopeStatus)
                                                     .Replace("##OldServiceStatus##", "Pending")
                                                     .Replace("##TaskId##", task.Id.ToString())
                                                     .Replace("##TaskNumber##", task.TaskNumber)
                                                     .Replace("##TaskType##", (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() != null ? (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() : "Not Set");

                        string changeStatusScopeHistory = JobHistoryMessages.ChangeStatusScopeMessage
                       .Replace("##ServiceItemName##", !string.IsNullOrEmpty(jobFeeScheduleDetailName) ? jobFeeScheduleDetailName : JobHistoryMessages.NoSetstring);

                        Common.SaveTaskHistory(employee.Id, changeStatusScopeHistory, task.Id, Convert.ToInt32(task.IdJobFeeSchedule));
                    }

                    if (jobTask.IdRfpJobType != null && jobTask.IdRfpJobType > 0 && jobTask.IsMilestone == false)
                    {
                        RfpJobType rfpJobType = rpoContext.RfpJobTypes.FirstOrDefault(x => x.Id == jobTask.IdRfpJobType);
                        JobFeeSchedule jobFeeSchedule = new JobFeeSchedule();
                        jobFeeSchedule.IdJob = Convert.ToInt32(jobTask.IdJob);
                        jobFeeSchedule.IdRfpWorkType = jobTask.IdRfpJobType;
                        jobFeeSchedule.IsInvoiced = false;
                        jobFeeSchedule.Quantity = jobTask.ServiceQuantity;
                        jobFeeSchedule.QuantityAchieved = jobTask.ServiceQuantity;
                        jobFeeSchedule.QuantityPending = 0;
                        jobFeeSchedule.Cost = rfpJobType.Cost;
                        jobFeeSchedule.TotalCost = (rfpJobType.Cost * jobFeeSchedule.Quantity);
                        //if (jobTask.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
                        //{
                        jobFeeSchedule.Status = "Completed";
                        jobFeeSchedule.CompletedDate = DateTime.UtcNow;
                        jobFeeSchedule.LastModified = DateTime.UtcNow;
                        if (employee != null)
                        {
                            jobFeeSchedule.ModifiedBy = employee.Id;
                        }
                        if (jobTask.JobBillingType == JobBillingType.AdditionalBilling)
                        {
                            jobFeeSchedule.IsAdditionalService = true;
                            jobFeeSchedule.IsShow = true;
                            jobFeeSchedule.FromTask = true;
                        }
                        rpoContext.JobFeeSchedules.Add(jobFeeSchedule);
                        rpoContext.SaveChanges();

                        task.IdJobFeeSchedule = jobFeeSchedule.Id;
                        rpoContext.SaveChanges();

                        JobFeeSchedule jobFeeScheduletmp = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobFeeSchedule.Id);

                        string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeScheduletmp);

                        string taskHistoryMessage = TaskHistoryMessages.TaskCompletedHistoryMessage
                                                     .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                                                     .Replace("##NewServiceStatus##", "Completed")
                                                     .Replace("##OldServiceStatus##", "Pending")
                                                     .Replace("##TaskId##", task.Id.ToString())
                                                     .Replace("##TaskNumber##", task.TaskNumber)
                                                     .Replace("##TaskType##", (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() != null ? (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() : "Not Set");

                        string changeStatusScopeHistory = JobHistoryMessages.ChangeStatusScopeMessage
                      .Replace("##ServiceItemName##", !string.IsNullOrEmpty(jobFeeScheduleDetailName) ? jobFeeScheduleDetailName : JobHistoryMessages.NoSetstring);

                        Common.SaveTaskHistory(employee.Id, changeStatusScopeHistory, task.Id, Convert.ToInt32(jobFeeSchedule.Id));
                    }

                    int additiontype1 = JobBillingType.AdditionalBilling.GetHashCode();
                    if (jobTask.JobBillingType == JobBillingType.AdditionalBilling)
                    {
                        //OtherBillable(additiontype1, task.Id, task.IdJob, "Completed");
                    }
                }
            }
            else
            {
                if (jobTask.IdJob != null && jobTask.IdJob > 0)
                {
                    //if (jobTask.IdJobFeeSchedule != null && jobTask.IdJobFeeSchedule > 0 && jobTask.IdRfpJobType == null)
                    //{
                    //    var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == jobTask.IdJobFeeSchedule);

                    //    if (jobFeeSchedules.QuantityPending < jobTask.ServiceQuantity)
                    //    {
                    //        // throw new RpoBusinessException(StaticMessages.FeeScheduleQuantityExceedsLimitMessage);
                    //    }

                    //    rpoContext.SaveChanges();

                    //    string ScopeStatus = string.Empty;
                    //    JobFeeSchedule jobFeeSchedule = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobTask.IdJobFeeSchedule);
                    //    if (jobTask.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() && jobFeeSchedule.QuantityAchieved == jobFeeSchedule.Quantity && jobTask.JobBillingType == JobBillingType.ScopeBilling)
                    //    {
                    //        jobFeeSchedule.QuantityPending = jobFeeSchedule.QuantityPending - task.ServiceQuantity;
                    //        jobFeeSchedule.QuantityAchieved = jobFeeSchedule.QuantityAchieved + task.ServiceQuantity;

                    //        jobFeeSchedule.Status = "Completed";
                    //        jobFeeSchedule.CompletedDate = DateTime.UtcNow;
                    //        jobFeeSchedule.LastModified = DateTime.UtcNow;
                    //        if (employee != null)
                    //        {
                    //            jobFeeSchedules.ModifiedBy = employee.Id;
                    //        }
                    //        ScopeStatus = "Completed";
                    //        rpoContext.SaveChanges();


                    //        string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeSchedule);

                    //        string TsType = string.Empty;
                    //        if ((from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() != null)
                    //        {
                    //            TsType = (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault();
                    //        }
                    //        else
                    //        {
                    //            TsType = "Not Set";
                    //        }
                    //        string taskHistoryMessagecomplet = TaskHistoryMessages.TaskCompletedHistoryMessage
                    //                                     .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                    //                                     .Replace("##NewServiceStatus##", ScopeStatus)
                    //                                     .Replace("##OldServiceStatus##", "Pending")
                    //                                     .Replace("##TaskId##", task.Id.ToString())
                    //                                     .Replace("##TaskNumber##", task.TaskNumber)
                    //                                     .Replace("##TaskType##", TsType);

                    //        Common.SaveTaskHistory(employee.Id, taskHistoryMessagecomplet, task.Id, Convert.ToInt32(task.IdJobFeeSchedule));

                    //        int additiontypetmp = JobBillingType.AdditionalBilling.GetHashCode();

                    //        if (jobTask.JobBillingType == JobBillingType.AdditionalBilling)
                    //        {
                    //            OtherBillable(additiontypetmp, task.Id, task.IdJob, "Completed");
                    //        }
                    //    }
                    //    //if (jobTask.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() && jobFeeSchedule.QuantityAchieved == jobFeeSchedule.Quantity && jobTask.JobBillingType == JobBillingType.AdditionalBilling)
                    //    //{
                    //    //    //jobFeeSchedule.QuantityPending = jobFeeSchedule.QuantityPending - task.ServiceQuantity;
                    //    //    //jobFeeSchedule.QuantityAchieved = jobFeeSchedule.QuantityAchieved + task.ServiceQuantity;

                    //    //    //jobFeeSchedules.IsShow = true;
                    //    //    //jobFeeSchedule.Status = "Completed";
                    //    //    //jobFeeSchedule.CompletedDate = DateTime.UtcNow;
                    //    //    //jobFeeSchedule.LastModified = DateTime.UtcNow;
                    //    //    //if (employee != null)
                    //    //    //{
                    //    //    //    jobFeeSchedules.ModifiedBy = employee.Id;
                    //    //    //}
                    //    //    //ScopeStatus = "Completed";
                    //    //    //rpoContext.SaveChanges();

                    //    //    int additiontypetmp = JobBillingType.AdditionalBilling.GetHashCode();

                    //    //    if (jobTask.JobBillingType == JobBillingType.AdditionalBilling)
                    //    //    {
                    //    //        OtherBillable(additiontypetmp, task.Id, task.IdJob, "Completed");
                    //    //    }

                    //    //    string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeSchedule);

                    //    //    string TsType = string.Empty;
                    //    //    if ((from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() != null)
                    //    //    {
                    //    //        TsType = (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault();
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        TsType = "Not Set";
                    //    //    }
                    //    //    string taskHistoryMessagecomplet = TaskHistoryMessages.TaskCompletedHistoryMessage
                    //    //                                 .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                    //    //                                 .Replace("##NewServiceStatus##", ScopeStatus)
                    //    //                                 .Replace("##OldServiceStatus##", "Pending")
                    //    //                                 .Replace("##TaskId##", task.Id.ToString())
                    //    //                                 .Replace("##TaskNumber##", task.TaskNumber)
                    //    //                                 .Replace("##TaskType##", TsType);

                    //    //    Common.SaveTaskHistory(employee.Id, taskHistoryMessagecomplet, task.Id, Convert.ToInt32(task.IdJobFeeSchedule));

                    //    //}

                    //    }
                }
            }
            if (jobTask.IdAssignedBy != jobTask.IdAssignedTo)
            {
                this.SendTaskAssignMail(task.Id, (jobTask.IdJob != null && jobTask.IdJob > 0) ? jobTask.IdJob : 0, jobTask.ModuleName);
            }
            //if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            //{
            updateMilestoneStatus(task.Id, task.IdJobFeeSchedule, task.IdJob, employee.Id);
            // }

            additionalCompleted(task.IdJobFeeSchedule, task.Id, task.IdJob, employee.Id);

            return Ok(FormatDetails(task));

        }
        /// <summary>
        /// Deletes the task.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns> Deletes the task.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = rpoContext.Tasks.Include("Notes").Include("TaskReminders").Include("TaskHistories").FirstOrDefault(x => x.Id == id);
            if (task == null)
            {
                return this.NotFound();
            }

            var employee = rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (employee != null && employee.Id != task.CreatedBy && employee.Id != task.IdAssignedBy)
            {
                throw new RpoBusinessException(StaticMessages.OnlyCreatorCanDeleteTaskMessage);
            }

            if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            {
                throw new RpoBusinessException(StaticMessages.CompletedTaskCannotDeleteMessage);
            }

            try
            {
                if (task.TaskHistories.Any())
                {
                    rpoContext.TaskHistories.RemoveRange(task.TaskHistories);
                    rpoContext.SaveChanges();
                }

                if (task.Notes.Any())
                {
                    rpoContext.TaskNotes.RemoveRange(task.Notes);
                    rpoContext.SaveChanges();
                }

                if (task.TaskReminders.Any())
                {
                    rpoContext.TaskReminders.RemoveRange(task.TaskReminders);
                    rpoContext.SaveChanges();
                }

                rpoContext.Tasks.Remove(task);
                rpoContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new RpoBusinessException(StaticMessages.DeleteReferenceExistExceptionMessage);
            }

            return Ok(task);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rpoContext.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Puts the taskstatus.
        /// </summary>
        /// <param name="taskStatusUpdate">The task status update.</param>
        /// <returns>update the task status.</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskDetailsDTO))]
        [Route("api/Tasks/status")]
        public IHttpActionResult PutTaskstatus(TaskStatusUpdate taskStatusUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Task task = rpoContext.Tasks.Include("JobFeeSchedule").FirstOrDefault(x => x.Id == taskStatusUpdate.Id);

            if (task == null)
            {
                return this.NotFound();
            }

            int? previousStatus = task.IdTaskStatus;

            task.IdTaskStatus = taskStatusUpdate.IdTaskStatus;

            if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() && task.IdMilestone == null)
            {
                task.ClosedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                if (task.IdJobFeeSchedule != null && task.IdJobFeeSchedule > 0)
                {
                    var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == task.IdJobFeeSchedule && x.IsRemoved == true);
                    if (jobFeeSchedules != null)
                    {
                        throw new RpoBusinessException(StaticMessages.ServicedeletedtaskMessage);
                    }
                }
                else
                {
                    if (task.JobBillingType == JobBillingType.AdditionalBilling)
                    {
                        var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == task.IdJobFeeSchedule && x.IsFromScope != true);
                        if (jobFeeSchedules == null)
                        {
                            OtherBillableStatus(JobBillingType.AdditionalBilling.GetHashCode(), task.Id, task.IdJob, "Completed");
                        }
                    }
                }

                if (task.IdJob != null && task.IdJob > 0)
                {
                    if (task.JobBillingType == JobBillingType.ScopeBilling)
                    {
                        if (task.IdMilestone != null)
                        {
                            var jobMilestones = rpoContext.JobMilestones.FirstOrDefault(x => x.Id == task.IdMilestone);

                            if (jobMilestones.Status == "Completed")
                            {
                                throw new RpoBusinessException(StaticMessages.JobMilestoneAllreadyMessage);
                            }
                        }
                        else
                        {
                            var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == task.IdJobFeeSchedule);

                            if (jobFeeSchedules.Status == "Completed")
                            {
                                throw new RpoBusinessException(StaticMessages.FeeScheduleAllreadyMessage);
                            }

                            if (jobFeeSchedules.QuantityPending < task.ServiceQuantity)
                            {
                                throw new RpoBusinessException(StaticMessages.FeeScheduleQuantityExceedsLimitMessage);
                            }
                        }
                    }
                }
            }
            else
            {
                task.ClosedDate = null;
            }

            if (previousStatus == EnumTaskStatus.Completed.GetHashCode())
            {
                #region PreviousStatusCompleted
                if (task.IdMilestone != null)
                {
                    if (task.IdJob != null && task.IdJob > 0)
                    {
                        var joMilestone = rpoContext.JobMilestones.FirstOrDefault(x => x.Id == task.IdMilestone);

                        if (joMilestone != null && task.IdTaskStatus == EnumTaskStatus.Pending.GetHashCode())
                        {
                            joMilestone.Status = "Pending";
                            joMilestone.LastModified = DateTime.UtcNow;
                            rpoContext.SaveChanges();
                        }
                    }
                }
                else if (task.IdTaskStatus != previousStatus)
                {
                    if (task.IdJob != null && task.IdJob > 0)
                    {
                        var jobFeeSchedule = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == task.IdJobFeeSchedule && x.IsFromScope == true);

                        if (jobFeeSchedule != null)
                        {
                            if (task.JobBillingType == JobBillingType.ScopeBilling)
                            {
                                var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == task.IdJobFeeSchedule && x.IsFromScope == true);
                                jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending + task.ServiceQuantity;
                                jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved - task.ServiceQuantity;
                                jobFeeSchedules.Status = "";
                                jobFeeSchedules.IsShow = true;
                                rpoContext.SaveChanges();
                            }

                            List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedule.Id).ToList();
                            if (taskHistoryList != null && taskHistoryList.Count > 0)
                            {
                                rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                                rpoContext.SaveChanges();
                            }
                            if (task.JobBillingType == JobBillingType.AdditionalBilling || task.JobBillingType == JobBillingType.NonBillableItems)
                            {
                                var jobFeeScheduleonlytask = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == task.IdJobFeeSchedule && x.IsFromScope != true);
                                rpoContext.JobFeeSchedules.Remove(jobFeeScheduleonlytask);
                                rpoContext.SaveChanges();
                            }
                        }

                        ////  if (task.IdJobFeeSchedule != null && task.IdJobFeeSchedule > 0 && task.IdRfpJobType == null)
                        //if (task.IdJobFeeSchedule != null && task.IdJobFeeSchedule > 0)
                        //{
                        //    var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == task.IdJobFeeSchedule);
                        //    if (task.JobBillingType == JobBillingType.AdditionalBilling)
                        //    {
                        //        jobFeeSchedules.IsShow = false;                             
                        //        jobFeeSchedules.Status = "";
                        //        jobFeeSchedules.QuantityPending = task.ServiceQuantity;
                        //        jobFeeSchedules.QuantityAchieved = 0;
                        //        rpoContext.SaveChanges();
                        //    }
                        //    else
                        //    {
                        //        // var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == task.IdJobFeeSchedule);
                        //        jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending + task.ServiceQuantity;
                        //        jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved - task.ServiceQuantity;
                        //        jobFeeSchedules.Status = "";
                        //        jobFeeSchedules.IsShow = false;
                        //        rpoContext.SaveChanges();
                        //    }
                        //    List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedules.Id).ToList();
                        //    if (taskHistoryList != null && taskHistoryList.Count > 0)
                        //    {
                        //        rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                        //        rpoContext.SaveChanges();
                        //    }
                        // }

                        if (task.IdRfpJobType != null && task.IdRfpJobType > 0)
                        {
                            JobFeeSchedule jobFeeSchedule1 = task.JobFeeSchedule;

                            if (jobFeeSchedule1 != null)
                            {
                                List<TaskHistory> taskHistoryList = rpoContext.TaskHistories.Where(x => x.IdJobFeeSchedule == jobFeeSchedule1.Id).ToList();
                                if (taskHistoryList != null && taskHistoryList.Count > 0)
                                {
                                    rpoContext.TaskHistories.RemoveRange(taskHistoryList);
                                    rpoContext.SaveChanges();
                                }
                                rpoContext.JobFeeSchedules.Remove(jobFeeSchedule1);
                                rpoContext.SaveChanges();
                            }
                        }
                    }
                }
                #endregion
            }

            if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            {
                if (task.IdJob != null && task.IdJob > 0)
                {
                    if (task.IdMilestone != null && task.IdJobFeeSchedule == null)
                    {
                        DirectMilestoneCompleted(task.Id, task.IdMilestone, task.IdJob, "Completed", employee.Id);
                    }
                    if (task.IdJobFeeSchedule != null && task.IdJobFeeSchedule > 0)
                    {
                        var jobFeeSchedules = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == task.IdJobFeeSchedule);

                        jobFeeSchedules.QuantityPending = jobFeeSchedules.QuantityPending - task.ServiceQuantity;
                        jobFeeSchedules.QuantityAchieved = jobFeeSchedules.QuantityAchieved + task.ServiceQuantity;

                        if (jobFeeSchedules.QuantityAchieved == jobFeeSchedules.Quantity)
                        {
                            jobFeeSchedules.Status = "Completed";
                            jobFeeSchedules.CompletedDate = DateTime.UtcNow;
                            jobFeeSchedules.LastModified = DateTime.UtcNow;
                            if (task.JobBillingType == JobBillingType.AdditionalBilling)
                            {
                                jobFeeSchedules.IsAdditionalService = true;
                                jobFeeSchedules.IsShow = true;
                            }
                            if (employee != null)
                            {
                                jobFeeSchedules.ModifiedBy = employee.Id;
                            }
                        }
                        rpoContext.SaveChanges();

                        // Common.SaveTaskHistory(employee.Id, TaskHistoryMessages.CreateTaskHistoryMessage.Replace("##TaskNumber##", task.TaskNumber), task.Id, jobFeeSchedules.Id);

                        string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeSchedules);
                        //string taskHistoryMessage = TaskHistoryMessages.TaskCompletedHistoryMessage;
                        string taskHistoryMessage = TaskHistoryMessages.TaskCompletedHistoryMessage
                                                    .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                                                    .Replace("##NewServiceStatus##", "Completed")
                                                    .Replace("##OldServiceStatus##", "Pending")
                                                    .Replace("##TaskId##", task.Id.ToString())
                                                    .Replace("##TaskNumber##", task.TaskNumber)
                                                    .Replace("##TaskType##", (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() != null ? (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() : "Not Set");


                        string changeStatusScopeHistory = JobHistoryMessages.ChangeStatusScopeMessage
                     .Replace("##ServiceItemName##", !string.IsNullOrEmpty(jobFeeScheduleDetailName) ? jobFeeScheduleDetailName : JobHistoryMessages.NoSetstring);

                        Common.SaveTaskHistory(employee.Id, changeStatusScopeHistory, task.Id, Convert.ToInt32(jobFeeSchedules.Id));

                    }

                    if (task.IdRfpJobType != null && task.IdRfpJobType > 0 && task.IdJobFeeSchedule == null)
                    {
                        JobFeeSchedule jobFeeSchedule = new JobFeeSchedule();
                        jobFeeSchedule.IdJob = Convert.ToInt32(task.IdJob);
                        jobFeeSchedule.IdRfpWorkType = task.IdRfpJobType;
                        jobFeeSchedule.IsInvoiced = false;
                        jobFeeSchedule.Quantity = task.ServiceQuantity;
                        jobFeeSchedule.QuantityAchieved = task.ServiceQuantity;
                        jobFeeSchedule.QuantityPending = 0;
                        jobFeeSchedule.Status = "Completed";
                        jobFeeSchedule.CompletedDate = DateTime.UtcNow;
                        jobFeeSchedule.LastModified = DateTime.UtcNow;
                        if (employee != null)
                        {
                            jobFeeSchedule.ModifiedBy = employee.Id;
                        }
                        jobFeeSchedule.Status = "Completed";
                        jobFeeSchedule.CompletedDate = DateTime.UtcNow;
                        jobFeeSchedule.LastModified = DateTime.UtcNow;
                        if (task.JobBillingType == JobBillingType.AdditionalBilling)
                        {
                            jobFeeSchedule.IsAdditionalService = true;
                            jobFeeSchedule.IsShow = true;
                            jobFeeSchedule.FromTask = true;
                        }
                        rpoContext.JobFeeSchedules.Add(jobFeeSchedule);
                        rpoContext.SaveChanges();

                        task.IdJobFeeSchedule = jobFeeSchedule.Id;
                        rpoContext.SaveChanges();

                        JobFeeSchedule jobFeeScheduletmp = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobFeeSchedule.Id);

                        string jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeScheduletmp);
                        //string taskHistoryMessage = TaskHistoryMessages.TaskCompletedHistoryMessage;
                        string taskHistoryMessage = TaskHistoryMessages.TaskCompletedHistoryMessage
                                                    .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                                                    .Replace("##NewServiceStatus##", "Completed")
                                                    .Replace("##OldServiceStatus##", "Pending")
                                                    .Replace("##TaskId##", task.Id.ToString())
                                                    .Replace("##TaskNumber##", task.TaskNumber)
                                                    .Replace("##TaskType##", (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() != null ? (from d in rpoContext.TaskTypes where d.Id == task.IdTaskType select d.Name).FirstOrDefault() : "Not Set");


                        string changeStatusScopeHistory = JobHistoryMessages.ChangeStatusScopeMessage
                     .Replace("##ServiceItemName##", !string.IsNullOrEmpty(jobFeeScheduleDetailName) ? jobFeeScheduleDetailName : JobHistoryMessages.NoSetstring);

                        Common.SaveTaskHistory(employee.Id, changeStatusScopeHistory, task.Id, Convert.ToInt32(jobFeeSchedule.Id));


                        //Common.UpdateMilestoneStatus(jobFeeSchedule.Id, Hub);
                    }
                }

                // SendTaskCompletedNotification(task.Id, taskStatusUpdate.IdJob);
            }

            if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
            {
                if (task.IdJobFeeSchedule != null && task.IdJobFeeSchedule > 0)
                {
                    var jobFeeSchedules = rpoContext.JobFeeSchedules.FirstOrDefault(x => x.Id == task.IdJobFeeSchedule);
                    if (jobFeeSchedules != null && jobFeeSchedules.QuantityPending == 0)
                    {
                        jobFeeSchedules.IsShow = true;
                        jobFeeSchedules.Status = "Completed";
                    }
                    else
                    {
                        jobFeeSchedules.Status = "";
                    }
                    rpoContext.SaveChanges();
                }
            }
            updateMilestoneStatus(task.Id, task.IdJobFeeSchedule, task.IdJob, employee.Id);
            additionalCompleted(task.IdJobFeeSchedule, task.Id, task.IdJob, employee.Id);

            if (task.IdTaskStatus == EnumTaskStatus.Unattainable.GetHashCode())
            {
                SendTaskUnattainableNotification(task.Id, taskStatusUpdate.IdJob);
            }

            task.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                task.LastModifiedBy = employee.Id;
            }

            this.rpoContext.SaveChanges();


            return this.Ok(this.FormatDetails(task));
        }
        /// <summary>
        /// Job milestoen completed method to competed status and send email
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="IdMilestone"></param>
        /// <param name="idjob"></param>
        /// <param name="Status"></param>
        /// <param name="employeeid"></param>
        private void DirectMilestoneCompleted(int? taskid, int? IdMilestone, int? idjob, string Status, int employeeid)
        {
            JobMilestone objmilestone = (from d in rpoContext.JobMilestones where d.IdJob == idjob && d.Id == IdMilestone select d).FirstOrDefault();

            if (objmilestone != null)
            {
                string changeStatusMilestone = JobHistoryMessages.ChangeStatusMilestone
              .Replace("##BillingPointName##", !string.IsNullOrEmpty(objmilestone.Name) ? objmilestone.Name : JobHistoryMessages.NoSetstring)
              .Replace("##OldStatus##", "Pending")
              .Replace("##NewStatus##", !string.IsNullOrEmpty(Status) ? Status : JobHistoryMessages.NoSetstring);

                Common.SaveJobHistory(employeeid, objmilestone.IdJob, changeStatusMilestone, JobHistoryType.Milestone);

                Common.SaveTaskMilestoneHistory(employeeid, changeStatusMilestone, taskid.Value, Convert.ToInt32(IdMilestone));

                objmilestone.Status = "Completed";
                objmilestone.LastModified = DateTime.UtcNow;
                rpoContext.SaveChanges();

                JobMilestoneCompleted(objmilestone.Id, taskid, objmilestone.IdJob);
            }
        }
        /// <summary>
        /// Update the milestone status and send notification
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="jobFeeScheduleid"></param>
        /// <param name="idjob"></param>
        /// <param name="employeeid"></param>
        private void updateMilestoneStatus(int? taskid, int? jobFeeScheduleid, int? idjob, int employeeid)
        {
            List<int> objmilestonemain = (from d in rpoContext.JobMilestones where d.IdJob == idjob select d.Id).ToList();

            JobMilestoneService objservie = (from d in rpoContext.JobMilestoneServices where objmilestonemain.Contains(d.IdMilestone) && d.IdJobFeeSchedule == jobFeeScheduleid select d).FirstOrDefault();

            if (objservie != null && objservie.IdMilestone != 0)
            {
                JobMilestone objmilestone = (from d in rpoContext.JobMilestones where d.IdJob == idjob && d.Id == objservie.IdMilestone select d).FirstOrDefault();

                List<int> objservieresList = (from d in rpoContext.JobMilestoneServices where d.IdMilestone == objmilestone.Id select d.IdJobFeeSchedule).ToList();

                string tmpstatus = "Completed";

                List<JobFeeSchedule> objschdule = (from d in rpoContext.JobFeeSchedules where d.IdJob == idjob && objservieresList.Contains(d.Id) && d.QuantityPending == 0 && d.Status.Trim().Contains(tmpstatus) select d).ToList();

                if (objservieresList.Count == objschdule.Count && objmilestone.Status != "Completed")
                {
                    string changeStatusMilestone = JobHistoryMessages.ChangeStatusMilestone
               .Replace("##BillingPointName##", !string.IsNullOrEmpty(objmilestone.Name) ? objmilestone.Name : JobHistoryMessages.NoSetstring)
               .Replace("##OldStatus##", "Pending")
               .Replace("##NewStatus##", !string.IsNullOrEmpty(objmilestone.Status) ? objmilestone.Status : JobHistoryMessages.NoSetstring);

                    Common.SaveJobHistory(employeeid, objmilestone.IdJob, changeStatusMilestone, JobHistoryType.Milestone);

                    objmilestone.Status = "Completed";
                    objmilestone.LastModified = DateTime.UtcNow;
                    rpoContext.SaveChanges();

                    JobMilestoneCompleted(objmilestone.Id, taskid, objmilestone.IdJob);
                }

            }
        }
        /// <summary>
        /// Additional serviceitem completed and send notification
        /// </summary>
        /// <param name="jobFeeScheduleid"></param>
        /// <param name="taskid"></param>
        /// <param name="idJob"></param>
        /// <param name="employeeid"></param>
        private void additionalCompleted(int? jobFeeScheduleid, int? taskid, int? idJob, int employeeid)
        {
            if (taskid > 0 && jobFeeScheduleid != null)
            {
                string javascript = "click=\"redirectFromNotification(j)\"";
                Task tasks = rpoContext.Tasks.Include("TaskType").Include("AssignedBy").Include("AssignedTo").Where(x => x.Id == taskid && x.IdJob == idJob).FirstOrDefault();

                List<int> objmilestonemain = (from d in rpoContext.JobMilestones where d.IdJob == idJob select d.Id).ToList();

                List<int> objservie = (from d in rpoContext.JobMilestoneServices where d.IdJobFeeSchedule == jobFeeScheduleid && objmilestonemain.Contains(d.IdMilestone) select d.IdJobFeeSchedule).ToList();

                JobFeeSchedule additionalservice = rpoContext.JobFeeSchedules.Where(x => x.Id == jobFeeScheduleid && x.IsAdditionalService == true && x.Status == "Completed" && !objservie.Contains(x.Id)).FirstOrDefault();
                Job job = rpoContext.Jobs.Include("RfpAddress").Include("RfpAddress.Borough").FirstOrDefault(x => x.Id == idJob);

                string JobAddress = job != null && job.RfpAddress != null ? (!string.IsNullOrEmpty(job.RfpAddress.HouseNumber) ? job.RfpAddress.HouseNumber : string.Empty) + " " + (!string.IsNullOrEmpty(job.RfpAddress.Street) ? job.RfpAddress.Street : string.Empty) + " " + (job.RfpAddress.Borough != null ? job.RfpAddress.Borough.Description : JobHistoryMessages.NoSetstring) + " " + (!string.IsNullOrEmpty(job.RfpAddress.ZipCode) ? job.RfpAddress.ZipCode : string.Empty) + " " + (!string.IsNullOrEmpty(job.SpecialPlace) ? "-" + job.SpecialPlace : string.Empty) : string.Empty;

                JobFeeSchedule jobFeeScheduletmp = rpoContext.JobFeeSchedules.Include("Rfp").Include("RfpWorkType.Parent.Parent.Parent.Parent").FirstOrDefault(x => x.Id == jobFeeScheduleid && x.IdJob == idJob);
                string jobFeeScheduleDetailName = string.Empty;
                string rfpServiceItemForEmail = string.Empty;
                try
                {
                    rfpServiceItemForEmail = jobFeeScheduletmp.RfpWorkType != null ? jobFeeScheduletmp.RfpWorkType.Name : string.Empty;

                    jobFeeScheduleDetailName = Common.GetServiceItemName(jobFeeScheduletmp);
                }
                catch (Exception)
                {

                }

                string taskHistoryMessage = string.Empty;

                string tasksetRedirecturl = string.Empty;
                if (tasks.IdJob != null)
                {
                    tasksetRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + tasks.IdJob + "" + "/jobtask";
                }
                else
                {
                    tasksetRedirecturl = Properties.Settings.Default.FrontEndUrl + "tasks";
                }

                taskHistoryMessage = TaskHistoryMessages.AdditonalTaskCompletedHistoryMessage
                                                           .Replace("##ServiceItemName##", jobFeeScheduleDetailName)
                                                           .Replace("##NewServiceStatus##", "Completed")
                                                           .Replace("##OldServiceStatus##", "Pending")
                                                           .Replace("##TaskId##", tasks.Id.ToString())
                                                           .Replace("##RedirectionTask##", tasksetRedirecturl)
                                                           .Replace("##TaskNumber##", tasks.TaskNumber)
                                                           .Replace("##JobNumber##", job.JobNumber)
                                                           .Replace("##RedirectionLinkJob##", Properties.Settings.Default.FrontEndUrl + "/job/" + job.Id + "/application")
                                                           .Replace("##TaskType##", (from d in rpoContext.TaskTypes where d.Id == tasks.IdTaskType select d.Name).FirstOrDefault() != null ? (from d in rpoContext.TaskTypes where d.Id == tasks.IdTaskType select d.Name).FirstOrDefault() : "Not Set");

                string changeStatusScopeHistory = JobHistoryMessages.ChangeStatusScopeMessage
                     .Replace("##ServiceItemName##", !string.IsNullOrEmpty(jobFeeScheduleDetailName) ? jobFeeScheduleDetailName : JobHistoryMessages.NoSetstring);

                if (tasks.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
                {
                    Common.SaveJobHistory(employeeid, idJob != null ? idJob.Value : 0, changeStatusScopeHistory, JobHistoryType.Scope);
                }
                if (additionalservice != null && tasks != null)
                {
                    SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.AdditionalBilling);
                    if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                    {
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/TaskCompleted.htm")))
                        {
                            body = reader.ReadToEnd();
                        }

                        foreach (var item in systemSettingDetail.Value)
                        {
                            string setRedirecturl = string.Empty;
                            if (tasks.IdJob != null)
                            {
                                setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + tasks.IdJob + "" + "/jobtask";
                            }
                            else
                            {
                                setRedirecturl = Properties.Settings.Default.FrontEndUrl + "tasks";
                            }

                            string emailBody = body;
                            emailBody = emailBody.Replace("##EmployeeName##", item.FirstName != null ? item.FirstName : string.Empty);

                            string emailMessage = "'##ServiceItemName##' has been updated to Completed from Pending with reference to Task# <a id='linktask' data-type='task' data-id='##TaskId##' class='taskHistory_Click' href=\"##RedirectionTask##\" rel='noreferrer'>##TaskNumber##</a> in job# <a href=\"##RedirectionLinkJob##\">##JobNumber##</a> - ##JobAddress##.<br> <a href=\"##RedirectionLinkscope##\">Click here</a> to go job scope.";

                            emailMessage = emailMessage.Replace("##ServiceItemName##", (!string.IsNullOrEmpty(rfpServiceItemForEmail) ? rfpServiceItemForEmail : JobHistoryMessages.NoSetstring))
                                                     .Replace("##TaskId##", tasks.Id.ToString())
                                                           .Replace("##RedirectionTask##", tasksetRedirecturl)
                                                           .Replace("##JobAddress##", JobAddress)
                                                           .Replace("##TaskNumber##", tasks.TaskNumber)
                                                           .Replace("##JobNumber##", job.JobNumber)
                                                           .Replace("##RedirectionLinkJob##", Properties.Settings.Default.FrontEndUrl + "job/" + job.Id + "/application")
                                                           .Replace("##RedirectionLinkscope##", Properties.Settings.Default.FrontEndUrl + "job/" + job.Id + "/scope");

                            emailBody = emailBody.Replace("##Message##", emailMessage);

                            // Common.SendInAppNotifications(item.Id, taskHistoryMessage, Hub, idJob > 0 || idJob != null ? "job/" + idJob + "/jobtask" : "tasks");

                            List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                            to.Add(new KeyValuePair<string, string>(item.Email, item.FirstName + " " + item.LastName));

                            List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();
                            Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, EmailNotificationSubject.AdditionalCompleted.Replace("##ServiceItemName##", (!string.IsNullOrEmpty(rfpServiceItemForEmail) ? rfpServiceItemForEmail : JobHistoryMessages.NoSetstring)).Replace("##Job##", idJob != null && idJob != 0 ? idJob.ToString() + ": " : string.Empty).Replace("##JobAddress##", JobAddress), emailBody, true);
                        }
                    }
                    if (taskHistoryMessage != "")
                    {
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
                            // Common.SendInAppNotifications(itemcomp.Id, taskHistoryMessage, Hub, idJob > 0 || idJob != null ? "job/" + idJob + "/jobtask" : "tasks");
                        }
                    }
                    additionalservice.CompletedDate = DateTime.UtcNow;
                    var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

                    if (employee != null)
                    {
                        additionalservice.ModifiedBy = employee.Id;
                    }
                    rpoContext.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Job Milestone completed
        /// </summary>
        /// <param name="milestoneid"></param>
        /// <param name="taskid"></param>
        /// <param name="idJob"></param>
        private void JobMilestoneCompleted(int? milestoneid, int? taskid, int? idJob)
        {
            int? tid = null;

            if (taskid > 0)
            {
                string javascript = "click=\"redirectFromNotification(j)\"";
                Task tasks = rpoContext.Tasks.Include("TaskType").Include("AssignedBy").Include("AssignedTo").Where(x => x.Id == taskid && x.IdJob == idJob).FirstOrDefault();

                if (tasks.IdMilestone != null)
                {
                    tid = tasks.Id;
                }


                if (milestoneid != null && tasks != null)
                {
                    JobMilestone jobMilestone = rpoContext.JobMilestones.Include("Job.RfpAddress.Borough").FirstOrDefault(x => x.Id == milestoneid && x.IdJob == idJob);

                    string houseStreetNameBorrough = jobMilestone != null && jobMilestone.Job.RfpAddress != null ? jobMilestone.Job.RfpAddress.HouseNumber + " " + jobMilestone.Job.RfpAddress.Street + (jobMilestone.Job.RfpAddress.Borough != null ? " " + jobMilestone.Job.RfpAddress.Borough.Description : string.Empty) : string.Empty;
                    string specialPlaceName = jobMilestone != null && jobMilestone.Job.SpecialPlace != null ? " - " + jobMilestone.Job.SpecialPlace : string.Empty;
                    NotificationMails.SendMilestoneCompletedMail(jobMilestone.Name, jobMilestone.Job.JobNumber, houseStreetNameBorrough, specialPlaceName, jobMilestone.IdJob, tid, Hub);

                }
            }
        }
        /// <summary>
        /// Other billable service tiem update status and send notification.
        /// </summary>
        /// <param name="billableType"></param>
        /// <param name="taskid"></param>
        /// <param name="idJob"></param>
        /// <param name="status"></param>
        private void OtherBillable(int billableType, int? taskid, int? idJob, string status)
        {
            if (taskid > 0)
            {
                Task tasks = rpoContext.Tasks.Where(x => x.Id == taskid && x.IdJob == idJob).FirstOrDefault();


                JobFeeSchedule rfpFeeScheduleNew = new JobFeeSchedule();
                rfpFeeScheduleNew.IdJob = tasks.IdJob.Value;
                rfpFeeScheduleNew.IdRfpWorkType = tasks.IdRfpJobType;
                rfpFeeScheduleNew.Status = status;
                RfpCostType cstType = (from d in rpoContext.RfpJobTypes where d.Id == tasks.IdRfpJobType select d.CostType).FirstOrDefault();

                if (cstType != null && cstType.ToString() == "HourlyCost")
                {
                    rfpFeeScheduleNew.Quantity = tasks.ServiceQuantity * 60;
                }
                else
                {
                    rfpFeeScheduleNew.Quantity = tasks.ServiceQuantity;
                }


                RfpJobType rfpWorkType = rpoContext.RfpJobTypes.FirstOrDefault(x => x.Id == tasks.IdRfpJobType);
                if (rfpWorkType != null)
                {
                    switch (rfpWorkType.CostType)
                    {
                        case RfpCostType.AdditionalCostPerUnit:
                            rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                            rfpFeeScheduleNew.TotalCost = rfpFeeScheduleNew.Cost + ((rfpFeeScheduleNew.Quantity - 1) * rfpWorkType.AdditionalUnitPrice);
                            break;
                        case RfpCostType.CostForUnitRange:

                            var perunitcost = rfpWorkType.RfpJobTypeCostRanges != null ?
                                               rfpWorkType.RfpJobTypeCostRanges
                                              .Where(x => x.MaximumQuantity >= rfpFeeScheduleNew.Quantity && x.MinimumQuantity <= rfpFeeScheduleNew.Quantity)
                                              .Select(x => x.Cost).FirstOrDefault()
                                              : 0;
                            rfpFeeScheduleNew.Cost = perunitcost;
                            rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);

                            break;
                        case RfpCostType.CummulativeCost:
                            if (rfpFeeScheduleNew.RfpWorkType != null && rfpFeeScheduleNew.RfpWorkType.RfpJobTypeCumulativeCosts != null)
                            {
                                var cummulativeCostList = rfpWorkType.RfpJobTypeCumulativeCosts.Where(x => x.Quantity <= rfpFeeScheduleNew.Quantity).ToList();

                                if (cummulativeCostList != null)
                                {
                                    int maxQuantity = cummulativeCostList.Max(x => x.Quantity);
                                    if (maxQuantity == rfpFeeScheduleNew.Quantity)
                                    {
                                        rfpFeeScheduleNew.Cost = 0;
                                        rfpFeeScheduleNew.TotalCost = cummulativeCostList.Sum(x => x.Cost);
                                    }
                                    else
                                    {
                                        int samePriceQuantity = Convert.ToInt32(rfpFeeScheduleNew.Quantity) - maxQuantity;
                                        double maxQuantityCost = cummulativeCostList.Where(x => x.Quantity == maxQuantity).Select(x => (double)x.Cost).FirstOrDefault();
                                        double cummulativeCost = cummulativeCostList.Sum(x => (double)x.Cost);
                                        rfpFeeScheduleNew.Cost = 0;
                                        rfpFeeScheduleNew.TotalCost = cummulativeCost + (samePriceQuantity * maxQuantityCost);
                                    }
                                }
                                else
                                {
                                    rfpFeeScheduleNew.Cost = 0;
                                    rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);
                                }
                            }
                            else
                            {
                                rfpFeeScheduleNew.Cost = 0;
                                rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);
                            }
                            break;
                        case RfpCostType.FixedCost:
                            rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                            rfpFeeScheduleNew.TotalCost = rfpFeeScheduleNew.Cost;
                            break;
                        case RfpCostType.MinimumCost:
                            rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                            rfpFeeScheduleNew.TotalCost = rfpFeeScheduleNew.Cost;
                            break;
                        case RfpCostType.PerUnitPrice:
                            rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                            rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);
                            break;

                        case RfpCostType.HourlyCost:
                            rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                            rfpFeeScheduleNew.TotalCost = ((rfpFeeScheduleNew.Quantity / 60) * rfpFeeScheduleNew.Cost);
                            break;
                    }
                }

                rfpFeeScheduleNew.Description = tasks.GeneralNotes;
                rfpFeeScheduleNew.QuantityAchieved = 0;
                if (cstType != null && cstType.ToString() == "HourlyCost")
                {
                    rfpFeeScheduleNew.QuantityPending = tasks.ServiceQuantity * 60;
                }
                else
                {
                    rfpFeeScheduleNew.QuantityPending = tasks.ServiceQuantity;
                }

                if (status == "Completed")
                {
                    rfpFeeScheduleNew.QuantityPending = 0;
                    rfpFeeScheduleNew.CompletedDate = DateTime.UtcNow;
                    var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

                    if (employee != null)
                    {
                        rfpFeeScheduleNew.ModifiedBy = employee.Id;
                    }
                    if (cstType != null && cstType.ToString() == "HourlyCost")
                    {
                        rfpFeeScheduleNew.QuantityAchieved = tasks.ServiceQuantity * 60;
                    }
                    else
                    {
                        rfpFeeScheduleNew.QuantityAchieved = tasks.ServiceQuantity;
                    }
                    rfpFeeScheduleNew.Status = "Completed";
                    rfpFeeScheduleNew.IsAdditionalService = true;
                    rfpFeeScheduleNew.IsShow = true;
                    rfpFeeScheduleNew.LastModified = DateTime.UtcNow;
                    rfpFeeScheduleNew.CompletedDate = DateTime.UtcNow;
                }
                else
                {
                    rfpFeeScheduleNew.Quantity = tasks.ServiceQuantity;
                    rfpFeeScheduleNew.QuantityPending = tasks.ServiceQuantity;
                    // rfpFeeScheduleNew.IsAdditionalService = false;
                    rfpFeeScheduleNew.Status = "";
                    rfpFeeScheduleNew.IsShow = false;
                }


                this.rpoContext.JobFeeSchedules.Add(rfpFeeScheduleNew);
                this.rpoContext.SaveChanges();

                tasks.IdJobFeeSchedule = rfpFeeScheduleNew.Id;
                rpoContext.SaveChanges();
            }

        }
        /// <summary>
        /// Other billable service tiem update status and send notification.
        /// </summary>
        /// <param name="billableType"></param>
        /// <param name="taskid"></param>
        /// <param name="idJob"></param>
        /// <param name="status"></param>
        private void OtherBillableStatus(int billableType, int? taskid, int? idJob, string status)
        {
            if (taskid > 0)
            {
                Task tasks = rpoContext.Tasks.Where(x => x.Id == taskid && x.IdJob == idJob).FirstOrDefault();


                JobFeeSchedule rfpFeeScheduleNew = new JobFeeSchedule();
                rfpFeeScheduleNew.IdJob = tasks.IdJob.Value;
                rfpFeeScheduleNew.IdRfpWorkType = tasks.IdRfpJobType;
                rfpFeeScheduleNew.Status = status;
                RfpCostType cstType = (from d in rpoContext.RfpJobTypes where d.Id == tasks.IdRfpJobType select d.CostType).FirstOrDefault();

                if (cstType != null && cstType.ToString() == "HourlyCost")
                {
                    rfpFeeScheduleNew.Quantity = tasks.ServiceQuantity * 60;
                }
                else
                {
                    rfpFeeScheduleNew.Quantity = tasks.ServiceQuantity;
                }


                RfpJobType rfpWorkType = rpoContext.RfpJobTypes.FirstOrDefault(x => x.Id == tasks.IdRfpJobType);
                if (rfpWorkType != null)
                {
                    switch (rfpWorkType.CostType)
                    {
                        case RfpCostType.AdditionalCostPerUnit:
                            rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                            rfpFeeScheduleNew.TotalCost = rfpFeeScheduleNew.Cost + ((rfpFeeScheduleNew.Quantity - 1) * rfpWorkType.AdditionalUnitPrice);
                            break;
                        case RfpCostType.CostForUnitRange:

                            var perunitcost = rfpWorkType.RfpJobTypeCostRanges != null ?
                                               rfpWorkType.RfpJobTypeCostRanges
                                              .Where(x => x.MaximumQuantity >= rfpFeeScheduleNew.Quantity && x.MinimumQuantity <= rfpFeeScheduleNew.Quantity)
                                              .Select(x => x.Cost).FirstOrDefault()
                                              : 0;
                            rfpFeeScheduleNew.Cost = perunitcost;
                            rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);

                            break;
                        case RfpCostType.CummulativeCost:
                            if (rfpFeeScheduleNew.RfpWorkType != null && rfpFeeScheduleNew.RfpWorkType.RfpJobTypeCumulativeCosts != null)
                            {
                                var cummulativeCostList = rfpWorkType.RfpJobTypeCumulativeCosts.Where(x => x.Quantity <= rfpFeeScheduleNew.Quantity).ToList();

                                if (cummulativeCostList != null)
                                {
                                    int maxQuantity = cummulativeCostList.Max(x => x.Quantity);
                                    if (maxQuantity == rfpFeeScheduleNew.Quantity)
                                    {
                                        rfpFeeScheduleNew.Cost = 0;
                                        rfpFeeScheduleNew.TotalCost = cummulativeCostList.Sum(x => x.Cost);
                                    }
                                    else
                                    {
                                        int samePriceQuantity = Convert.ToInt32(rfpFeeScheduleNew.Quantity) - maxQuantity;
                                        double maxQuantityCost = cummulativeCostList.Where(x => x.Quantity == maxQuantity).Select(x => (double)x.Cost).FirstOrDefault();
                                        double cummulativeCost = cummulativeCostList.Sum(x => (double)x.Cost);
                                        rfpFeeScheduleNew.Cost = 0;
                                        rfpFeeScheduleNew.TotalCost = cummulativeCost + (samePriceQuantity * maxQuantityCost);
                                    }
                                }
                                else
                                {
                                    rfpFeeScheduleNew.Cost = 0;
                                    rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);
                                }
                            }
                            else
                            {
                                rfpFeeScheduleNew.Cost = 0;
                                rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);
                            }
                            break;
                        case RfpCostType.FixedCost:
                            rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                            rfpFeeScheduleNew.TotalCost = rfpFeeScheduleNew.Cost;
                            break;
                        case RfpCostType.MinimumCost:
                            rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                            rfpFeeScheduleNew.TotalCost = rfpFeeScheduleNew.Cost;
                            break;
                        case RfpCostType.PerUnitPrice:
                            rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                            rfpFeeScheduleNew.TotalCost = (rfpFeeScheduleNew.Quantity * rfpFeeScheduleNew.Cost);
                            break;

                        case RfpCostType.HourlyCost:
                            rfpFeeScheduleNew.Cost = rfpWorkType.Cost;
                            rfpFeeScheduleNew.TotalCost = ((rfpFeeScheduleNew.Quantity / 60) * rfpFeeScheduleNew.Cost);
                            break;
                    }
                }

                rfpFeeScheduleNew.Description = tasks.GeneralNotes;
                rfpFeeScheduleNew.QuantityAchieved = 0;
                if (cstType != null && cstType.ToString() == "HourlyCost")
                {
                    rfpFeeScheduleNew.QuantityPending = tasks.ServiceQuantity * 60;
                }
                else
                {
                    rfpFeeScheduleNew.QuantityPending = tasks.ServiceQuantity;
                }

                //if (status == "Completed")
                //{
                //    rfpFeeScheduleNew.QuantityPending = 0;
                //    rfpFeeScheduleNew.CompletedDate = DateTime.UtcNow;
                //    var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

                //    if (employee != null)
                //    {
                //        rfpFeeScheduleNew.ModifiedBy = employee.Id;
                //    }
                //    if (cstType != null && cstType.ToString() == "HourlyCost")
                //    {
                //        rfpFeeScheduleNew.QuantityAchieved = tasks.ServiceQuantity * 60;
                //    }
                //    else
                //    {
                //        rfpFeeScheduleNew.QuantityAchieved = tasks.ServiceQuantity;
                //    }
                //    rfpFeeScheduleNew.Status = "Completed";
                //    rfpFeeScheduleNew.IsAdditionalService = true;
                //}
                //else
                //{
                rfpFeeScheduleNew.Quantity = tasks.ServiceQuantity;
                rfpFeeScheduleNew.QuantityAchieved = 0;
                rfpFeeScheduleNew.QuantityPending = tasks.ServiceQuantity;
                //  rfpFeeScheduleNew.IsAdditionalService = false;
                rfpFeeScheduleNew.Status = "";
                //}

                this.rpoContext.JobFeeSchedules.Add(rfpFeeScheduleNew);
                this.rpoContext.SaveChanges();

                tasks.IdJobFeeSchedule = rfpFeeScheduleNew.Id;
                rpoContext.SaveChanges();
            }

        }
        /// <summary>
        /// Puts the tasks assigned to.
        /// </summary>
        /// <param name="taskAssignedToUpdate">The task assigned to update.</param>
        /// <returns>update the task and send email to assigned user.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskDetailsDTO))]
        [Route("api/Tasks/assignedto")]
        public IHttpActionResult PutTasksAssignedTo(TaskAssignedToUpdate taskAssignedToUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Task task = rpoContext.Tasks.FirstOrDefault(x => x.Id == taskAssignedToUpdate.Id);

            if (task == null)
            {
                return this.NotFound();
            }

            task.IdAssignedTo = taskAssignedToUpdate.IdAssignedTo;
            task.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                task.LastModifiedBy = employee.Id;
            }

            this.rpoContext.SaveChanges();
            return this.Ok(this.FormatDetails(task));
        }

        /// <summary>
        /// Puts the job violation Due date.
        /// </summary>
        /// <param name="taskDueDateUpdate">The job violation due date update.</param>
        /// <returns>update the due date of task.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(TaskDetailsDTO))]
        [Route("api/Tasks/DueDate")]
        public IHttpActionResult PutTaskDueDate(TaskDueDateUpdate taskDueDateUpdate)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            Task task = rpoContext.Tasks.FirstOrDefault(x => x.Id == taskDueDateUpdate.Id);

            if (task == null)
            {
                return this.NotFound();
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            task.CompleteBy = taskDueDateUpdate.DueDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(taskDueDateUpdate.DueDate), DateTimeKind.Utc) : taskDueDateUpdate.DueDate;
            //  task.CompleteBy = taskDueDateUpdate.DueDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(taskDueDateUpdate.DueDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : taskDueDateUpdate.DueDate;
            task.LastModifiedDate = DateTime.UtcNow;
            if (employee != null)
            {
                task.LastModifiedBy = employee.Id;
            }

            this.rpoContext.SaveChanges();

            return this.Ok(this.FormatDetails(task));
        }

        /// <summary>
        /// Tasks the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool TaskExists(int id)
        {
            return rpoContext.Tasks.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Sends the task assign mail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void SendTaskAssignMail(int id, int? jobId, string moduleName)
        {
            if (id > 0)
            {
                string javascript = "click=\"redirectFromNotification(j)\"";

                //string javascript = "[routerLink]=\"['/job',"+ jobId + ",'jobtask']\"";
                Task tasks = rpoContext.Tasks.Include("Job").Include("TaskType").Include("AssignedBy").Include("AssignedTo").Where(x => x.Id == id).FirstOrDefault();

                if (tasks != null && tasks.AssignedTo != null && tasks.IdAssignedTo != tasks.IdAssignedBy)
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/EmailTemplate/NewTaskAssignedEmailTemplate.htm")))
                    {
                        body = reader.ReadToEnd();
                    }

                    if (tasks != null && tasks.AssignedTo != null && !string.IsNullOrWhiteSpace(tasks.AssignedTo.Email))
                    {
                        string setRedirecturl = string.Empty;
                        if (tasks.IdJob != null)
                        {
                            setRedirecturl = Properties.Settings.Default.FrontEndUrl + "job/" + tasks.IdJob + "" + "/jobtask";
                        }
                        else
                        {
                            setRedirecturl = Properties.Settings.Default.FrontEndUrl + "tasks";
                        }

                        string emailBody = body;
                        emailBody = emailBody.Replace("##EmployeeName##", tasks.AssignedTo != null ? tasks.AssignedTo.FirstName : string.Empty);
                        emailBody = emailBody.Replace("##AssignBy##", tasks.AssignedBy != null ? tasks.AssignedBy.FirstName + " " + tasks.AssignedBy.LastName : string.Empty);
                        emailBody = emailBody.Replace("##Job##", tasks.IdJob != null ? tasks.IdJob.ToString() : "Not Set");
                        emailBody = emailBody.Replace("##House##", tasks.Job != null && !string.IsNullOrEmpty(tasks.Job.HouseNumber) ? tasks.Job.HouseNumber : string.Empty);
                        emailBody = emailBody.Replace("##StreetName##", tasks.Job != null && !string.IsNullOrEmpty(tasks.Job.StreetNumber) ? tasks.Job.StreetNumber : string.Empty);
                        emailBody = emailBody.Replace("##TaskType##", tasks != null && tasks.TaskType != null ? tasks.TaskType.Name : "Not Set");
                        emailBody = emailBody.Replace("##TaskNumber##", tasks != null ? tasks.TaskNumber : string.Empty);
                        if (!tasks.TaskType.IsDisplayTime)
                        {
                            // emailBody = emailBody.Replace("##DueDate##", tasks != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(tasks.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString() + " EST" : string.Empty);
                            emailBody = emailBody.Replace("##DueDate##", tasks != null ? DateTime.SpecifyKind(Convert.ToDateTime(tasks.CompleteBy), DateTimeKind.Utc).ToShortDateString() : string.Empty);
                        }
                        else
                        {
                            //emailBody = emailBody.Replace("##DueDate##", tasks != null ? Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(tasks.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))) + " EST" : string.Empty);
                            emailBody = emailBody.Replace("##DueDate##", tasks != null ? DateTime.SpecifyKind(Convert.ToDateTime(tasks.CompleteBy), DateTimeKind.Utc).ToString() : string.Empty);
                        }
                        emailBody = emailBody.Replace("##TaskDetails##", tasks.GeneralNotes);
                        //emailBody = emailBody.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/tasks");
                        emailBody = emailBody.Replace("##RedirectionLink##", setRedirecturl);

                        List<KeyValuePair<string, string>> to = new List<KeyValuePair<string, string>>();
                        to.Add(new KeyValuePair<string, string>(tasks.AssignedTo.Email, tasks.AssignedTo.FirstName + " " + tasks.AssignedTo.LastName));
                        List<KeyValuePair<string, string>> cc = new List<KeyValuePair<string, string>>();

                        SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.NewTaskIsCreated);
                        if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                        {
                            foreach (var item in systemSettingDetail.Value)
                            {
                                if (item.Email != tasks.AssignedTo.Email)
                                {
                                    cc.Add(new KeyValuePair<string, string>(item.Email, item.EmployeeName));
                                    string newTaskAssignedSetting = InAppNotificationMessage._NewTaskAssigned
                                           .Replace("##AssignBy##", tasks.AssignedBy != null ? tasks.AssignedBy.FirstName + " " + tasks.AssignedBy.LastName : string.Empty)
                                           .Replace("##TaskType##", tasks != null && tasks.TaskType != null ? tasks.TaskType.Name : "Not Set")
                                           .Replace("##TaskNumber##", tasks != null ? tasks.TaskNumber : string.Empty)
                                    //.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/tasks");
                                    .Replace("##RedirectionLink##", javascript);
                                    if (!tasks.TaskType.IsDisplayTime)
                                    {
                                        //newTaskAssignedSetting = newTaskAssignedSetting.Replace("##DueDate##", tasks != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(tasks.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString() + " EST" : string.Empty);
                                        newTaskAssignedSetting = newTaskAssignedSetting.Replace("##DueDate##", tasks != null ? DateTime.SpecifyKind(Convert.ToDateTime(tasks.CompleteBy), DateTimeKind.Utc).ToShortDateString() : string.Empty);
                                    }
                                    else
                                    {
                                        //newTaskAssignedSetting = newTaskAssignedSetting.Replace("##DueDate##", tasks != null ? Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(tasks.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))) + " EST" : string.Empty);
                                        newTaskAssignedSetting = newTaskAssignedSetting.Replace("##DueDate##", tasks != null ? DateTime.SpecifyKind(Convert.ToDateTime(tasks.CompleteBy), DateTimeKind.Utc).ToString() : string.Empty);
                                    }
                                    newTaskAssignedSetting = newTaskAssignedSetting.Replace("##TaskDetails##", tasks.GeneralNotes);
                                    Common.SendInAppNotifications(item.Id, newTaskAssignedSetting, Hub, !string.IsNullOrEmpty(moduleName) && moduleName == "JobModule" ? "job/" + jobId + "/jobtask" : "/tasks");
                                }
                            }
                        }
                        Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to, cc, EmailNotificationSubject.NewTaskAssigned, emailBody, true);

                        List<KeyValuePair<string, string>> to1 = new List<KeyValuePair<string, string>>();
                        to1.Add(new KeyValuePair<string, string>(tasks.AssignedBy.Email, tasks.AssignedBy.FirstName + " " + tasks.AssignedBy.LastName));

                        // Mail.Send(new KeyValuePair<string, string>(Properties.Settings.Default.SmtpUserName, "RPO APP"), to1, cc, "New Task Assigned By", emailBody, true);

                        string newTaskAssigned = InAppNotificationMessage._NewTaskAssigned
                            .Replace("##AssignBy##", tasks.AssignedBy != null ? tasks.AssignedBy.FirstName + " " + tasks.AssignedBy.LastName : string.Empty)
                            .Replace("##TaskType##", tasks != null && tasks.TaskType != null ? tasks.TaskType.Name : "Not Set")
                            .Replace("##TaskNumber##", tasks != null ? tasks.TaskNumber : string.Empty)
                            .Replace("##RedirectionLink##", javascript);
                        if (!tasks.TaskType.IsDisplayTime)
                        {
                            // newTaskAssigned = newTaskAssigned.Replace("##DueDate##", tasks != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(tasks.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToShortDateString() + " EST" : string.Empty);
                            newTaskAssigned = newTaskAssigned.Replace("##DueDate##", tasks != null ? DateTime.SpecifyKind(Convert.ToDateTime(tasks.CompleteBy), DateTimeKind.Utc).ToShortDateString() : string.Empty);
                        }
                        else
                        {
                            //newTaskAssigned = newTaskAssigned.Replace("##DueDate##", tasks != null ? Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(tasks.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))) + " EST" : string.Empty);
                            newTaskAssigned = newTaskAssigned.Replace("##DueDate##", tasks != null ? DateTime.SpecifyKind(Convert.ToDateTime(tasks.CompleteBy), DateTimeKind.Utc).ToString() : string.Empty);
                        }
                        newTaskAssigned = newTaskAssigned.Replace("##TaskDetails##", tasks.GeneralNotes);
                        //Common.SendInAppNotifications(tasks.AssignedTo.Id, newTaskAssigned, Hub, !string.IsNullOrEmpty(moduleName) && moduleName == "JobModule" ? "job/" + jobId + "/jobtask" : "/tasks");
                    }
                }
            }

        }

        /// <summary>
        /// Sends the task assign mail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void SendTaskCompletedNotification(int id, int? idJob)
        {
            if (id > 0)
            {
                string javascript = "click=\"redirectFromNotification(j)\"";
                Task tasks = rpoContext.Tasks.Include("TaskType").Include("AssignedBy").Include("AssignedTo").Where(x => x.Id == id).FirstOrDefault();
                if (tasks.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
                {
                    if (tasks != null && tasks.AssignedBy != null && tasks.AssignedTo != null)
                    {
                        if (tasks.JobBillingType == JobBillingType.ScopeBilling || tasks.JobBillingType == JobBillingType.AdditionalBilling)
                        {
                            SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenTaskIsMarkedComplete);
                            if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                            {
                                foreach (var item in systemSettingDetail.Value)
                                {
                                    if (item.Email != tasks.AssignedBy.Email)
                                    {
                                        string taskCompleteSetting = InAppNotificationMessage._TaskCompleted
                                            .Replace("##TaskDetails##", tasks.GeneralNotes)
                                            .Replace("##TaskType##", tasks != null && tasks.TaskType != null ? tasks.TaskType.Name : "Not Set")
                                            .Replace("##TaskNumber##", tasks != null ? tasks.TaskNumber : string.Empty)
                                            //.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/tasks");
                                            .Replace("##RedirectionLink##", javascript);
                                        Common.SendInAppNotifications(item.Id, taskCompleteSetting, Hub, idJob > 0 || idJob != null ? "job/" + idJob + "/jobtask" : "tasks");
                                    }
                                }
                            }
                        }
                        string taskComplete = InAppNotificationMessage._TaskCompleted
                                        .Replace("##TaskDetails##", tasks.GeneralNotes)
                                        .Replace("##TaskType##", tasks != null && tasks.TaskType != null ? tasks.TaskType.Name : "Not Set")
                                        .Replace("##TaskNumber##", tasks != null ? tasks.TaskNumber : string.Empty)
                        //.Replace("##RedirectionLink##", Properties.Settings.Default.FrontEndUrl + "/tasks");
                        .Replace("##RedirectionLink##", javascript);
                        Common.SendInAppNotifications(tasks.AssignedBy.Id, taskComplete, Hub, idJob > 0 || idJob != null ? "job/" + idJob + "/jobtask" : "tasks");
                        Common.SendInAppNotifications(tasks.AssignedTo.Id, taskComplete, Hub, idJob > 0 || idJob != null ? "job/" + idJob + "/jobtask" : "tasks");
                    }
                }
            }

        }

        /// <summary>
        /// Sends the task unattainable notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private void SendTaskUnattainableNotification(int id, int? idJob)
        {
            if (id > 0)
            {
                string javascript = "click=\"redirectFromNotification(j)\"";
                Task tasks = rpoContext.Tasks.Include("TaskType").Include("AssignedBy").Include("AssignedTo").Where(x => x.Id == id).FirstOrDefault();
                if (tasks.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode())
                {
                    if (tasks != null && tasks.AssignedBy != null && tasks.AssignedTo != null)
                    {
                        SystemSettingDetail systemSettingDetail = Common.ReadSystemSetting(Enums.SystemSetting.WhenTaskIsMarkedUnattainable);
                        if (systemSettingDetail != null && systemSettingDetail.Value != null && systemSettingDetail.Value.Count() > 0)
                        {
                            foreach (var item in systemSettingDetail.Value)
                            {
                                if (item.Email != tasks.AssignedBy.Email)
                                {
                                    string taskUnattainableSetting = InAppNotificationMessage._TaskUnattainable
                                        .Replace("##TaskDetails##", tasks.GeneralNotes)
                                        .Replace("##TaskType##", tasks != null && tasks.TaskType != null ? tasks.TaskType.Name : "Not Set")
                                        .Replace("##TaskNumber##", tasks != null ? tasks.TaskNumber : string.Empty)
                                        .Replace("##RedirectionLink##", javascript);
                                    Common.SendInAppNotifications(item.Id, taskUnattainableSetting, Hub, idJob > 0 || idJob != null ? "job/" + idJob + "/jobtask" : "tasks");
                                }
                            }
                        }

                        string taskUnattainable = InAppNotificationMessage._TaskUnattainable
                                        .Replace("##TaskDetails##", tasks.GeneralNotes)
                                        .Replace("##TaskType##", tasks != null && tasks.TaskType != null ? tasks.TaskType.Name : "Not Set")
                                        .Replace("##TaskNumber##", tasks != null ? tasks.TaskNumber : string.Empty)
                                        .Replace("##RedirectionLink##", javascript);
                        Common.SendInAppNotifications(tasks.AssignedBy.Id, taskUnattainable, Hub, idJob > 0 || idJob != null ? "job/" + idJob + "/jobtask" : "tasks");
                        Common.SendInAppNotifications(tasks.AssignedTo.Id, taskUnattainable, Hub, idJob > 0 || idJob != null ? "job/" + idJob + "/jobtask" : "tasks");
                    }
                }
            }

        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>TaskDetailsDTO.</returns>
        private TaskDetailsDTO FormatDetails(Task task)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            List<int> workPermitType = task.IdWorkPermitType != null && !string.IsNullOrEmpty(task.IdWorkPermitType) ? (task.IdWorkPermitType.Split(',') != null && task.IdWorkPermitType.Split(',').Any() ? task.IdWorkPermitType.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            string badgeClass = string.Empty;
            DateTime? completeBy = task.CompleteBy != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.CompleteBy), DateTimeKind.Utc) : task.CompleteBy;
            DateTime utcDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() || task.IdTaskStatus == EnumTaskStatus.Unattainable.GetHashCode())
            {
                badgeClass = "grey";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date == utcDate.Date)
            {
                badgeClass = "orange";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date == (utcDate.AddDays(1)).Date)
            {
                badgeClass = "blue";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date >= (utcDate.AddDays(1)).Date && Convert.ToDateTime(completeBy).Date <= (utcDate.AddDays(5)).Date)
            {
                badgeClass = "yellow";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date > (utcDate.AddDays(5)).Date)
            {
                badgeClass = "green";
            }
            else
            {
                badgeClass = "red";
            }
            return new TaskDetailsDTO
            {
                Id = task.Id,
                //  AssignedDate = task.AssignedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.AssignedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.AssignedDate,
                AssignedDate = task.AssignedDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.AssignedDate), DateTimeKind.Utc) : task.AssignedDate,
                IdAssignedTo = task.IdAssignedTo,
                AssignedTo = task.AssignedTo != null ? task.AssignedTo.FirstName + " " + task.AssignedTo.LastName : string.Empty,
                IdAssignedBy = task.IdAssignedBy,
                AssignedBy = task.AssignedBy != null ? task.AssignedBy.FirstName + " " + task.AssignedBy.LastName : string.Empty,
                IdTaskType = task.IdTaskType,
                TaskType = task.TaskType != null ? task.TaskType.Name : string.Empty,
                //CompleteBy = task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.CompleteBy,
                //ClosedDate = task.ClosedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.ClosedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : DateTime.MinValue,
                CompleteBy = task.CompleteBy != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.CompleteBy), DateTimeKind.Utc) : task.CompleteBy,
                ClosedDate = task.ClosedDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.ClosedDate), DateTimeKind.Utc) : DateTime.MinValue,
                IdTaskStatus = task.IdTaskStatus,
                TaskStatus = task.TaskStatus != null ? task.TaskStatus.Name : string.Empty,
                GeneralNotes = task.GeneralNotes,
                IdJobApplication = task.IdJobApplication,
                JobApplication = (task.JobApplication != null && task.JobApplication.JobApplicationType != null) ? task.JobApplication.JobApplicationType.Description : string.Empty,
                WorkPermitType = task.IdWorkPermitType,
                IdWorkPermitType = rpoContext.JobApplicationWorkPermitTypes.Where(x => workPermitType.Contains(x.Id)).Select(x => new WorkPermitTypeDTO
                {
                    Id = x.Id,
                    ItemName = x.JobWorkType != null ? x.JobWorkType.Description : string.Empty
                }),
                IdJobViolation = task.IdJobViolation,
                SummonsNumber = task.JobViolation != null ? task.JobViolation.SummonsNumber : string.Empty,
                IdJob = task.IdJob,
                IdRfp = task.IdRfp,
                TaskDuration = task.TaskDuration,
                IdContact = task.IdContact,
                IdCompany = task.IdCompany,
                IdExaminer = task.IdExaminer,
                TaskNumber = task.TaskNumber,
                JobBillingType = task.JobBillingType,
                IdJobFeeSchedule = task.IdJobFeeSchedule,
                IdRfpJobType = task.IdRfpJobType,
                ServiceQuantity = task.ServiceQuantity,
                IdJobType = task.IdJobType,
                Examiner = task.Examiner != null ? task.Examiner.FirstName + " " + task.Examiner.LastName : string.Empty,
                LastModifiedDate = task.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.LastModifiedDate,
                LastModifiedBy = task.LastModifiedBy != null ? task.LastModifiedBy : task.CreatedBy,
                LastModified = task.LastModified != null ? task.LastModified.FirstName + " " + task.LastModified.LastName : (task.CreatedByEmployee != null ? task.CreatedByEmployee.FirstName + " " + task.CreatedByEmployee.LastName : string.Empty),
                IsScopeRemoved = task.JobFeeSchedule != null ? task.JobFeeSchedule.IsRemoved : false,
                BadgeClass = badgeClass,
                IsGeneric = task.IsGeneric
            };
        }

        /// <summary>
        /// Formats the tasks.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>TaskDetailsDTO.</returns>
        private TaskDetailsDTO FormatTasks(Task task)
        {
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            int dobApplicationType = ApplicationType.DOB.GetHashCode();
            int dotApplicationType = ApplicationType.DOT.GetHashCode();
            int depApplicationType = ApplicationType.DEP.GetHashCode();
            int violationApplicationType = ApplicationType.Violation.GetHashCode();

            List<int> workPermitType = task.IdWorkPermitType != null && !string.IsNullOrEmpty(task.IdWorkPermitType) ? (task.IdWorkPermitType.Split(',') != null && task.IdWorkPermitType.Split(',').Any() ? task.IdWorkPermitType.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();


            return new TaskDetailsDTO
            {
                Id = task.Id,
                // AssignedDate = task.AssignedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.AssignedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.AssignedDate,
                AssignedDate = task.AssignedDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.AssignedDate), DateTimeKind.Utc) : task.AssignedDate,
                IdAssignedTo = task.IdAssignedTo,
                AssignedTo = task.AssignedTo != null ? task.AssignedTo.FirstName + " " + task.AssignedTo.LastName : string.Empty,
                IdAssignedBy = task.IdAssignedBy,
                AssignedBy = task.AssignedBy != null ? task.AssignedBy.FirstName + " " + task.AssignedBy.LastName : string.Empty,
                IdTaskType = task.IdTaskType,
                TaskType = task.TaskType != null ? task.TaskType.Name : string.Empty,
                // CompleteBy = task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.CompleteBy,
                CompleteBy = task.CompleteBy != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.CompleteBy), DateTimeKind.Utc) : task.CompleteBy,
                //   ClosedDate = task.ClosedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.ClosedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : DateTime.MinValue,
                ClosedDate = task.ClosedDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.ClosedDate), DateTimeKind.Utc) : DateTime.MinValue,
                IdTaskStatus = task.IdTaskStatus,
                TaskStatus = task.TaskStatus != null ? task.TaskStatus.Name : string.Empty,
                GeneralNotes = task.GeneralNotes,
                IdJobApplication = task.IdJobApplication,
                JobApplication = task.IdJob != null && task.IdJob > 0 ?
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType && task.JobApplication.ApplicationNumber != null && task.JobApplication.ApplicationNumber != string.Empty)
                ? "A# : " + task.JobApplication.ApplicationNumber :
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
                ? "LOCATION : " + task.JobApplication.JobApplicationType.Description + "|" + task.JobApplication.StreetWorkingOn + "|" + task.JobApplication.StreetFrom + "|" + task.JobApplication.StreetTo :
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == depApplicationType)
                ? "LOCATION : " + task.JobApplication.JobApplicationType.Description + "|" + task.JobApplication.StreetWorkingOn + "|" + task.JobApplication.StreetFrom + "|" + task.JobApplication.StreetTo :
                ((task.JobViolation != null && task.JobViolation.SummonsNumber != null && task.JobViolation.SummonsNumber != string.Empty) ? ("V# : " + task.JobViolation.SummonsNumber) : string.Empty)))) : string.Empty,
                //JobApplication = (task.JobApplication != null && task.JobApplication.JobApplicationType != null) ? task.JobApplication.JobApplicationType.Description : string.Empty,
                //WorkPermitType = task.IdWorkPermitType,
                WorkPermitType =
                task.IdJob != null && task.IdJob > 0 ?
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType)
                ? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.JobWorkType.Description + ((x.JobWorkType.Code ?? "") != "" ? " (" + x.JobWorkType.Code + ")" : "")))
                : ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
                ? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.PermitType + " (" + x.PermitNumber + ")")) :
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == depApplicationType)
                ? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.JobWorkType.Description + " (" + x.PermitNumber + ")")) : string.Empty))) : string.Empty,
                JobContact = task != null && task.Job != null && task.Job.Contact != null ? task.Job.Contact.FirstName + " " + task.Job.Contact.LastName : string.Empty,
                JobCompany = task != null && task.Job != null && task.Job.Company != null ? task.Job.Company.Name : string.Empty,
                RFPContact = task != null && task.Rfp != null && task.Rfp.Contact != null ? task.Rfp.Contact.FirstName + " " + task.Rfp.Contact.LastName : string.Empty,
                RFPCompany = task != null && task.Rfp != null && task.Rfp.Company != null ? task.Rfp.Company.Name : string.Empty,
                IdWorkPermitType = rpoContext.JobApplicationWorkPermitTypes.Where(x => workPermitType.Contains(x.Id)).Select(x => new WorkPermitTypeDTO
                {
                    Id = x.Id,
                    ItemName = x.PermitType != null && x.PermitType != "" ? x.PermitType : (x.JobWorkType != null ? x.JobWorkType.Description : string.Empty)
                }),
                IdJob = task.IdJob,
                IdJobViolation = task.IdJobViolation,
                SummonsNumber = task.JobViolation != null ? task.JobViolation.SummonsNumber : string.Empty,
                JobNumber = task.Job != null ? task.Job.JobNumber : string.Empty,
                IdRfp = task.IdRfp,
                RfpNumber = task.Rfp != null ? task.Rfp.RfpNumber : string.Empty,
                IdContact = task.IdContact,
                ContactName = task.Contact != null ? task.Contact.FirstName + " " + task.Contact.LastName : string.Empty,
                IdCompany = task.IdCompany,
                CompanyName = task.Company != null ? task.Company.Name : string.Empty,
                IdExaminer = task.IdExaminer,
                TaskNumber = task.TaskNumber,
                TaskDuration = task.TaskDuration,
                IdJobType = task.IdJobType,
                CostType = task.JobFeeSchedule != null && task.JobFeeSchedule.RfpWorkType != null ? task.JobFeeSchedule.RfpWorkType.CostType : (task.RfpJobType != null ? task.RfpJobType.CostType : RfpCostType.FixedCost),
                CostTypeName = task.JobFeeSchedule != null && task.JobFeeSchedule.RfpWorkType != null ? task.JobFeeSchedule.RfpWorkType.CostType.ToString() : (task.RfpJobType != null ? task.RfpJobType.CostType.ToString() : string.Empty),
                Notes = task.Notes.Select(x => new TaskNoteDetails
                {
                    Id = x.Id,
                    IdTask = x.IdTask,
                    Notes = x.Notes,
                    CreatedBy = x.CreatedBy,
                    CreatedByEmployee = (from d in rpoContext.Employees where d.Id == x.CreatedBy select d.FirstName + " " + d.LastName).FirstOrDefault() != null ? (from d in rpoContext.Employees where d.Id == x.CreatedBy select d.FirstName + " " + d.LastName).FirstOrDefault() : string.Empty,
                    LastModified = (from d in rpoContext.Employees where d.Id == x.LastModifiedBy select d.FirstName + " " + d.LastName).FirstOrDefault() != null ? (from d in rpoContext.Employees where d.Id == x.LastModifiedBy select d.FirstName + " " + d.LastName).FirstOrDefault() : string.Empty,
                    LastModifiedBy = x.LastModifiedBy,
                    LastModifiedDate = x.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.LastModifiedDate,
                    CreatedDate = x.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.CreatedDate,
                }).OrderByDescending(x => x.LastModifiedDate),
                Examiner = task.Examiner != null ? task.Examiner.FirstName + " " + task.Examiner.LastName : string.Empty,
                CreatedDate = task.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.CreatedDate,
                LastModifiedDate = task.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.LastModifiedDate,
                LastModifiedBy = task.LastModifiedBy != null ? task.LastModifiedBy : task.CreatedBy,
                LastModified = task.LastModified != null ? task.LastModified.FirstName + " " + task.LastModified.LastName : (task.CreatedByEmployee != null ? task.CreatedByEmployee.FirstName + " " + task.CreatedByEmployee.LastName : string.Empty),
                CreatedByEmployee = (from d in rpoContext.Employees where d.Id == task.CreatedBy select d.FirstName + " " + d.LastName).FirstOrDefault() != null ? (from d in rpoContext.Employees where d.Id == task.CreatedBy select d.FirstName + " " + d.LastName).FirstOrDefault() : string.Empty,
                JobBillingType = task.JobBillingType,
                IdJobFeeSchedule = task.IdMilestone != null && task.IdMilestone != 0 ? task.IdMilestone : task.IdJobFeeSchedule,
                JobFeeSchedule = task.IdMilestone != null && task.IdMilestone != 0 ? task.JobMilestone.Name : (task.JobFeeSchedule != null ? Common.GetServiceItemName(task.JobFeeSchedule) : string.Empty),
                IdMilestone = task.IdMilestone,
                IdRfpJobType = task.IdRfpJobType,
                RfpJobType = task.RfpJobType != null ? Common.GetServiceItemName(task.RfpJobType) : string.Empty,
                ServiceQuantity = task.ServiceQuantity,
                IsScopeRemoved = task.JobFeeSchedule != null ? task.JobFeeSchedule.IsRemoved : false,
                TaskDocuments = task.TaskDocuments.Select(x => new TaskDocumentDetail()
                {
                    Id = x.Id,
                    DocumentPath = APIUrl + "/" + Properties.Settings.Default.TaskDocumentPath + "/" + x.Id + "_" + x.DocumentPath,
                    IdTask = x.IdTask,
                    Name = x.Name
                }).ToList(),
                TaskJobDocuments = task.TaskJobDocuments.Select(x => new TaskJobDocumentDetail()
                {
                    Id = x.Id,
                    DocumentPath = Properties.Settings.Default.DropboxExternalUrl + "/" + x.JobDocument.IdJob + "/" + x.JobDocument.Id + "_" + x.JobDocument.DocumentPath,
                    IdTask = x.IdTask,
                    IdJobDocument = x.IdJobDocument,
                    Name = x.JobDocument.DocumentName,
                    ItemName = "[" + x.JobDocument.DocumentMaster.Code + "] - " + x.JobDocument.DocumentName + " " + x.JobDocument.DocumentDescription,
                    Copies = 1,
                    IdDocument = x.JobDocument != null ? x.JobDocument.IdDocument : 0,
                    DocumentName = x.JobDocument != null ? x.JobDocument.DocumentName : string.Empty,
                    DocumentDescription = x.JobDocument != null ? x.JobDocument.DocumentDescription : string.Empty,
                    DocumentCode = x.JobDocument != null && x.JobDocument.DocumentMaster != null ? x.JobDocument.DocumentMaster.Code : string.Empty,
                    ApplicationNumber = x.JobDocument != null && x.JobDocument.JobApplication != null ? x.JobDocument.JobApplication.ApplicationNumber : string.Empty,
                    //AppplicationType = (from d in rpoContext.JobApplicationTypes where d.Id== x.JobDocument.JobApplication.IdJobApplicationType select d.Description).FirstOrDefault() != null ? (from d in rpoContext.JobApplicationTypes where d.Id == x.JobDocument.JobApplication.IdJobApplicationType select d.Description).FirstOrDefault() : string.Empty,
                    AppplicationType = x.JobDocument != null && x.JobDocument.JobApplication != null ? x.JobDocument.JobApplication.JobApplicationType != null ? x.JobDocument.JobApplication.JobApplicationType.Description : string.Empty : string.Empty,
                    CreatedByEmployeeName = x.JobDocument != null && x.JobDocument.CreatedByEmployee != null ? x.JobDocument.CreatedByEmployee.FirstName + " " + x.JobDocument.CreatedByEmployee.LastName : string.Empty,
                    CreatedDate = x.JobDocument != null && x.JobDocument.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.JobDocument.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.JobDocument.CreatedDate,
                    LastModifiedByEmployeeName = x.JobDocument != null && x.JobDocument.LastModifiedByEmployee != null ? x.JobDocument.LastModifiedByEmployee.FirstName + " " + x.JobDocument.LastModifiedByEmployee.LastName : (x.JobDocument != null && x.JobDocument.CreatedByEmployee != null ? x.JobDocument.CreatedByEmployee.FirstName + " " + x.JobDocument.CreatedByEmployee.LastName : string.Empty),
                    LastModifiedDate = x.JobDocument != null && x.JobDocument.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.JobDocument.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (x.JobDocument != null && x.JobDocument.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.JobDocument.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.JobDocument.CreatedDate),

                }).ToList(),
                TaskJobTransmittals = task.JobTransmittals.Select(x => new TaskJobTransmittalDetail()
                {
                    Id = x.Id,
                    IdJobTransmittal = x.Id,
                    IdTask = x.IdTask,
                    TransmittalNumber = x.TransmittalNumber,
                    IdFromEmployee = x.IdFromEmployee,
                    FromEmployee = x.FromEmployee != null ? x.FromEmployee.FirstName + " " + x.FromEmployee.LastName : string.Empty,
                    IdToCompany = x.IdToCompany,
                    ToCompany = x.ToCompany != null ? x.ToCompany.Name : string.Empty,
                    IdContactAttention = x.IdContactAttention,
                    ContactAttention = x.ContactAttention != null ? x.ContactAttention.FirstName + " " + x.ContactAttention.LastName : string.Empty,
                    IdTransmissionType = x.IdTransmissionType,
                    TransmissionType = x.TransmissionType != null ? x.TransmissionType.Name : string.Empty,
                    IdEmailType = x.IdEmailType,
                    EmailType = x.EmailType != null ? x.EmailType.Name : string.Empty,
                    SentDate = x.SentDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(x.SentDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : x.SentDate,
                    IdSentBy = x.IdSentBy,
                    SentBy = x.SentBy != null ? x.SentBy.FirstName + " " + x.SentBy.LastName : string.Empty,
                    EmailMessage = x.EmailMessage,
                    EmailSubject = x.EmailSubject,
                    IsAdditionalAtttachment = x.IsAdditionalAtttachment,
                    IsEmailSent = x.IsEmailSent,

                }).ToList(),
                IsGeneric = task.IsGeneric,
            };
        }

        /// <summary>
        /// Formats the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>TaskDetailsDTO.</returns>
        private TaskDetailsDTO Format(Task task)
        {

            string taskFor = string.Empty;

            string JobApplicationType = string.Empty;

            if (task.IdJob != null && task.Job != null)
            {
                taskFor = "Job# " + task.Job.JobNumber;

                JobApplicationType = string.Join(",", task.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }
            else if (task.IdRfp != null && task.Rfp != null)
            {
                taskFor = "RFP# " + task.Rfp.RfpNumber;
            }
            else if (task.IdContact != null && task.Contact != null)
            {
                taskFor = "Contact: " + task.Contact.FirstName + " " + task.Contact.LastName;
            }
            else if (task.IdCompany != null && task.Company != null)
            {
                taskFor = "Company: " + task.Company.Name;
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            string badgeClass = string.Empty;

            DateTime? completeBy = task.CompleteBy != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.CompleteBy), DateTimeKind.Utc) : task.CompleteBy;
            //  DateTime? completeBy = task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.CompleteBy;
            // DateTime utcDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
            DateTime utcDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() || task.IdTaskStatus == EnumTaskStatus.Unattainable.GetHashCode())
            {
                badgeClass = "grey";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date == utcDate.Date)
            {
                badgeClass = "orange";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date == (utcDate.AddDays(1)).Date)
            {
                badgeClass = "blue";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date >= (utcDate.AddDays(1)).Date && Convert.ToDateTime(completeBy).Date <= (utcDate.AddDays(5)).Date)
            {
                badgeClass = "yellow";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date > (utcDate.AddDays(5)).Date)
            {
                badgeClass = "green";
            }
            else
            {
                badgeClass = "red";
            }

            int dobApplicationType = ApplicationType.DOB.GetHashCode();
            int dotApplicationType = ApplicationType.DOT.GetHashCode();
            int depApplicationType = ApplicationType.DEP.GetHashCode();
            int violationApplicationType = ApplicationType.Violation.GetHashCode();

            List<int> workPermitType = task.IdWorkPermitType != null && !string.IsNullOrEmpty(task.IdWorkPermitType) ? (task.IdWorkPermitType.Split(',') != null && task.IdWorkPermitType.Split(',').Any() ? task.IdWorkPermitType.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            TaskNote objTaskNote = task.Notes.OrderByDescending(d => d.LastModifiedDate).FirstOrDefault();

            return new TaskDetailsDTO
            {
                Id = task.Id,
                //AssignedDate = task.AssignedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.AssignedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.AssignedDate,
                //IdAssignedTo = task.IdAssignedTo,
                //AssignedTo = task.AssignedTo != null ? task.AssignedTo.FirstName + " " + task.AssignedTo.LastName : string.Empty,
                //AssignedToLastName = task.AssignedTo != null ? task.AssignedTo.LastName : string.Empty,
                //IdAssignedBy = task.IdAssignedBy,
                //AssignedBy = task.AssignedBy != null ? task.AssignedBy.FirstName + " " + task.AssignedBy.LastName : string.Empty,
                //AssignedByLastName = task.AssignedBy != null ? task.AssignedBy.LastName : string.Empty,
                //IdTaskType = task.IdTaskType,
                //TaskType = task.TaskType != null ?
                //task.TaskType.Name +
                //(task.TaskType.IsDisplayContact != null && task.TaskType.IsDisplayContact == true && task.Examiner != null ? " with " + task.Examiner.FirstName + " " + task.Examiner.LastName : string.Empty) +
                //(task.TaskType.IsDisplayTime != null && task.TaskType.IsDisplayTime == true ? " at " + (task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToShortTimeString() : string.Empty) : string.Empty)
                // : string.Empty,
                //CompleteBy = task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.CompleteBy,
                //ClosedDate = task.ClosedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.ClosedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : DateTime.MinValue,
                // AssignedDate = task.AssignedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.AssignedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.AssignedDate,
                AssignedDate = task.AssignedDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.AssignedDate), DateTimeKind.Utc) : task.AssignedDate,
                IdAssignedTo = task.IdAssignedTo,
                AssignedTo = task.AssignedTo != null ? task.AssignedTo.FirstName + " " + task.AssignedTo.LastName : string.Empty,
                AssignedToLastName = task.AssignedTo != null ? task.AssignedTo.LastName : string.Empty,
                IdAssignedBy = task.IdAssignedBy,
                AssignedBy = task.AssignedBy != null ? task.AssignedBy.FirstName + " " + task.AssignedBy.LastName : string.Empty,
                AssignedByLastName = task.AssignedBy != null ? task.AssignedBy.LastName : string.Empty,
                IdTaskType = task.IdTaskType,
                TaskType = task.TaskType != null ?
                task.TaskType.Name +
                (task.TaskType.IsDisplayContact != null && task.TaskType.IsDisplayContact == true && task.Examiner != null ? " with " + task.Examiner.FirstName + " " + task.Examiner.LastName : string.Empty) +
                (task.TaskType.IsDisplayTime != null && task.TaskType.IsDisplayTime == true ? " at " + (task.CompleteBy != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.CompleteBy), DateTimeKind.Utc).ToShortTimeString() : string.Empty) : string.Empty)
                 : string.Empty,
                // CompleteBy = task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.CompleteBy,
                CompleteBy = task.CompleteBy != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.CompleteBy), DateTimeKind.Utc) : task.CompleteBy,
                //   ClosedDate = task.ClosedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.ClosedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : DateTime.MinValue,
                ClosedDate = task.ClosedDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.ClosedDate), DateTimeKind.Utc) : DateTime.MinValue,

                IdTaskStatus = task.IdTaskStatus,
                TaskStatus = task.TaskStatus != null ? task.TaskStatus.Name : string.Empty,
                GeneralNotes = task.GeneralNotes,
                NotesDateStamp = objTaskNote != null && objTaskNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(objTaskNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToString("MM/dd/yyyy") : string.Empty,
                NotesTimeStamp = objTaskNote != null && objTaskNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(objTaskNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToString("hh:mm tt") : string.Empty,
                IdJobApplication = task.IdJobApplication,
                SpecialPlace = task.Job != null ? task.Job.SpecialPlace : string.Empty,
                HouseNumber = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.HouseNumber : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.HouseNumber : string.Empty),
                Street = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.Street : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.Street : string.Empty),
                Borough = task.Job != null && task.Job.RfpAddress != null && task.Job.RfpAddress.Borough != null ? task.Job.RfpAddress.Borough.Description : (task.Rfp != null && task.Rfp.RfpAddress != null && task.Rfp.RfpAddress.Borough != null ? task.Rfp.RfpAddress.Borough.Description : string.Empty),
                ZipCode = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.ZipCode : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.ZipCode : string.Empty),
                JobApplication = task.IdJob != null && task.IdJob > 0 ?
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType && task.JobApplication.ApplicationNumber != null && task.JobApplication.ApplicationNumber != string.Empty)
                ? "A# : " + task.JobApplication.ApplicationNumber :
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
                ? "LOCATION : " + task.JobApplication.JobApplicationType.Description + "|" + task.JobApplication.StreetWorkingOn + "|" + task.JobApplication.StreetFrom + "|" + task.JobApplication.StreetTo :
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == depApplicationType)
                ? "LOCATION : " + task.JobApplication.JobApplicationType.Description + "|" + task.JobApplication.StreetWorkingOn + "|" + task.JobApplication.StreetFrom + "|" + task.JobApplication.StreetTo :
                ((task.JobViolation != null && task.JobViolation.SummonsNumber != null && task.JobViolation.SummonsNumber != string.Empty) ? ("V# : " + task.JobViolation.SummonsNumber) : string.Empty)))) : string.Empty,
                WorkPermitType =
                task.IdJob != null && task.IdJob > 0 ?
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType)
                ? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.JobWorkType.Description + ((x.JobWorkType.Code ?? "") != "" ? " (" + x.JobWorkType.Code + ")" : "")))
                : ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
                ? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.PermitType + " (" + x.PermitNumber + ")")) :
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == depApplicationType)
                ? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.JobWorkType.Description + " (" + x.PermitNumber + ")")) : string.Empty))) : string.Empty,

                //JobApplication = task.IdJob != null && task.IdJob > 0 ?
                //((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType && task.JobApplication.ApplicationNumber != null && task.JobApplication.ApplicationNumber != string.Empty)
                //? "A# : " + task.JobApplication.ApplicationNumber :
                //((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
                //? "LOCATION : " + task.JobApplication.JobApplicationType.Description + "|" + task.JobApplication.StreetWorkingOn + "|" + task.JobApplication.StreetFrom + "|" + task.JobApplication.StreetTo :
                //((task.JobViolation != null && task.JobViolation.SummonsNumber != null && task.JobViolation.SummonsNumber != string.Empty) ? ("V# : " + task.JobViolation.SummonsNumber) :
                //((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == depApplicationType)
                //? task.JobApplication.JobApplicationType.Description + "|" + (string.IsNullOrEmpty(task.JobApplication.StreetWorkingOn) ? "-|" : task.JobApplication.StreetWorkingOn + "|") + (string.IsNullOrEmpty(task.JobApplication.StreetFrom) ? "-|" : task.JobApplication.StreetFrom + "|") + (string.IsNullOrEmpty(task.JobApplication.StreetTo) ? "-|" : task.JobApplication.StreetTo + "|") : string.Empty)
                //))) : string.Empty,
                //WorkPermitType =
                //task.IdJob != null && task.IdJob > 0 ?
                //((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType)
                //? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.JobWorkType.Description))
                //: ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
                //? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Where(x => workPermitType.Contains(x.Id)).Select(x => x.PermitNumber)) : string.Empty)) : string.Empty,//need to change in this line so do not remove this
                //((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == depApplicationType) // do not remove this
                //? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Where(x => workPermitType.Contains(x.Id)).Select(x => x.PermitNumber)) : string.Empty)// do not remove this
                //)) : "Outer WorkPermit",
                IdJobViolation = task.IdJobViolation,
                SummonsNumber = task.JobViolation != null ? task.JobViolation.SummonsNumber : string.Empty,
                IdJob = task.IdJob,
                JobApplicationType = JobApplicationType,
                IdRfp = task.IdRfp,
                IdContact = task.IdContact,
                IdCompany = task.IdCompany,
                IdExaminer = task.IdExaminer,
                TaskNumber = task.TaskNumber,
                TaskDuration = task.TaskDuration,
                IdJobType = task.IdJobType,
                Examiner = task.Examiner != null ? task.Examiner.FirstName + " " + task.Examiner.LastName : string.Empty,
                LastModifiedDate = task.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.LastModifiedDate,
                LastModifiedBy = task.LastModifiedBy != null ? task.LastModifiedBy : task.CreatedBy,
                LastModified = task.LastModified != null ? task.LastModified.FirstName + " " + task.LastModified.LastName : (task.CreatedByEmployee != null ? task.CreatedByEmployee.FirstName + " " + task.CreatedByEmployee.LastName : string.Empty),
                BadgeClass = badgeClass,
                TaskFor = taskFor,
                ProgressNote = task.Notes != null ? task.Notes.OrderByDescending(x => x.LastModifiedDate).Select(x => x.Notes).FirstOrDefault() : string.Empty,
                IdJobStatus = task.Job != null ? (int)task.Job.Status : 0,
                IsScopeRemoved = task.JobFeeSchedule != null ? task.JobFeeSchedule.IsRemoved : false,
                IsGeneric = task.IsGeneric,
            };
        }

        private TasklistDTO Format1(Task task)
        {

            string taskFor = string.Empty;

            string JobApplicationType = string.Empty;

            if (task.IdJob != null && task.Job != null)
            {
                taskFor = "Job# " + task.Job.JobNumber;

                JobApplicationType = string.Join(",", task.Job.JobApplicationTypes.Select(x => x.Id.ToString()));
            }
            else if (task.IdRfp != null && task.Rfp != null)
            {
                taskFor = "RFP# " + task.Rfp.RfpNumber;
            }
            else if (task.IdContact != null && task.Contact != null)
            {
                taskFor = "Contact: " + task.Contact.FirstName + " " + task.Contact.LastName;
            }
            else if (task.IdCompany != null && task.Company != null)
            {
                taskFor = "Company: " + task.Company.Name;
            }

            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            string badgeClass = string.Empty;

            DateTime? completeBy = task.CompleteBy != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.CompleteBy), DateTimeKind.Utc) : task.CompleteBy;
            //  DateTime? completeBy = task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.CompleteBy;
            // DateTime utcDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone));
            DateTime utcDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            if (task.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() || task.IdTaskStatus == EnumTaskStatus.Unattainable.GetHashCode())
            {
                badgeClass = "grey";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date == utcDate.Date)
            {
                badgeClass = "orange";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date == (utcDate.AddDays(1)).Date)
            {
                badgeClass = "blue";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date >= (utcDate.AddDays(1)).Date && Convert.ToDateTime(completeBy).Date <= (utcDate.AddDays(5)).Date)
            {
                badgeClass = "yellow";
            }
            else if (completeBy != null && Convert.ToDateTime(completeBy).Date > (utcDate.AddDays(5)).Date)
            {
                badgeClass = "green";
            }
            else
            {
                badgeClass = "red";
            }

            int dobApplicationType = ApplicationType.DOB.GetHashCode();
            int dotApplicationType = ApplicationType.DOT.GetHashCode();
            int depApplicationType = ApplicationType.DEP.GetHashCode();
            int violationApplicationType = ApplicationType.Violation.GetHashCode();

            List<int> workPermitType = task.IdWorkPermitType != null && !string.IsNullOrEmpty(task.IdWorkPermitType) ? (task.IdWorkPermitType.Split(',') != null && task.IdWorkPermitType.Split(',').Any() ? task.IdWorkPermitType.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            TaskNote objTaskNote = task.Notes.OrderByDescending(d => d.LastModifiedDate).FirstOrDefault();

            return new TasklistDTO
            {
                Id = task.Id,
                AssignedDate = task.AssignedDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.AssignedDate), DateTimeKind.Utc) : task.AssignedDate,
                IdAssignedTo = task.IdAssignedTo,
                // AssignedToLastName = task.AssignedTo != null ? task.AssignedTo.LastName : string.Empty,
                // IdAssignedBy = task.IdAssignedBy,
                IdTaskType = task.IdTaskType,
                //IdJobApplication = task.IdJobApplication,
                //IdJobViolation = task.IdJobViolation,
                //SummonsNumber = task.JobViolation != null ? task.JobViolation.SummonsNumber : string.Empty,
                //IdExaminer = task.IdExaminer,
                //TaskDuration = task.TaskDuration,
                //IdJobType = task.IdJobType,
                //Examiner = task.Examiner != null ? task.Examiner.FirstName + " " + task.Examiner.LastName : string.Empty,
                //LastModifiedDate = task.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.LastModifiedDate,
                //LastModifiedBy = task.LastModifiedBy != null ? task.LastModifiedBy : task.CreatedBy,
                //LastModified = task.LastModified != null ? task.LastModified.FirstName + " " + task.LastModified.LastName : (task.CreatedByEmployee != null ? task.CreatedByEmployee.FirstName + " " + task.CreatedByEmployee.LastName : string.Empty),
                AssignedTo = task.AssignedTo != null ? task.AssignedTo.FirstName + " " + task.AssignedTo.LastName : string.Empty,
                //IsScopeRemoved = task.JobFeeSchedule != null ? task.JobFeeSchedule.IsRemoved : false,

                AssignedBy = task.AssignedBy != null ? task.AssignedBy.FirstName + " " + task.AssignedBy.LastName : string.Empty,
                AssignedByLastName = task.AssignedBy != null ? task.AssignedBy.LastName : string.Empty,

                TaskType = task.TaskType != null ?
                task.TaskType.Name +
                (task.TaskType.IsDisplayContact != null && task.TaskType.IsDisplayContact == true && task.Examiner != null ? " with " + task.Examiner.FirstName + " " + task.Examiner.LastName : string.Empty) +
                (task.TaskType.IsDisplayTime != null && task.TaskType.IsDisplayTime == true ? " at " + (task.CompleteBy != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.CompleteBy), DateTimeKind.Utc).ToShortTimeString() : string.Empty) : string.Empty)
                 : string.Empty,
                // CompleteBy = task.CompleteBy != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.CompleteBy), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : task.CompleteBy,
                CompleteBy = task.CompleteBy != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.CompleteBy), DateTimeKind.Utc) : task.CompleteBy,
                //   ClosedDate = task.ClosedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(task.ClosedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : DateTime.MinValue,
                ClosedDate = task.ClosedDate != null ? DateTime.SpecifyKind(Convert.ToDateTime(task.ClosedDate), DateTimeKind.Utc) : DateTime.MinValue,

                IdTaskStatus = task.IdTaskStatus,
                TaskStatus = task.TaskStatus != null ? task.TaskStatus.Name : string.Empty,
                GeneralNotes = task.GeneralNotes,
                NotesDateStamp = objTaskNote != null && objTaskNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(objTaskNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToString("MM/dd/yyyy") : string.Empty,
                NotesTimeStamp = objTaskNote != null && objTaskNote.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(objTaskNote.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)).ToString("hh:mm tt") : string.Empty,

                SpecialPlace = task.Job != null ? task.Job.SpecialPlace : string.Empty,
                HouseNumber = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.HouseNumber : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.HouseNumber : string.Empty),
                Street = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.Street : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.Street : string.Empty),
                Borough = task.Job != null && task.Job.RfpAddress != null && task.Job.RfpAddress.Borough != null ? task.Job.RfpAddress.Borough.Description : (task.Rfp != null && task.Rfp.RfpAddress != null && task.Rfp.RfpAddress.Borough != null ? task.Rfp.RfpAddress.Borough.Description : string.Empty),
                ZipCode = task.Job != null && task.Job.RfpAddress != null ? task.Job.RfpAddress.ZipCode : (task.Rfp != null && task.Rfp.RfpAddress != null ? task.Rfp.RfpAddress.ZipCode : string.Empty),
                JobApplication = task.IdJob != null && task.IdJob > 0 ?
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType && task.JobApplication.ApplicationNumber != null && task.JobApplication.ApplicationNumber != string.Empty)
                ? "A# : " + task.JobApplication.ApplicationNumber :
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
                ? "LOCATION : " + task.JobApplication.JobApplicationType.Description + "|" + task.JobApplication.StreetWorkingOn + "|" + task.JobApplication.StreetFrom + "|" + task.JobApplication.StreetTo :
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == depApplicationType)
                ? "LOCATION : " + task.JobApplication.JobApplicationType.Description + "|" + task.JobApplication.StreetWorkingOn + "|" + task.JobApplication.StreetFrom + "|" + task.JobApplication.StreetTo :
                ((task.JobViolation != null && task.JobViolation.SummonsNumber != null && task.JobViolation.SummonsNumber != string.Empty) ? ("V# : " + task.JobViolation.SummonsNumber) : string.Empty)))) : string.Empty,
                WorkPermitType =
                task.IdJob != null && task.IdJob > 0 ?
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType)
                ? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.JobWorkType.Description + ((x.JobWorkType.Code ?? "") != "" ? " (" + x.JobWorkType.Code + ")" : "")))
                : ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
                ? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.PermitType + " (" + x.PermitNumber + ")")) :
                ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == depApplicationType)
                ? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.JobWorkType.Description + " (" + x.PermitNumber + ")")) : string.Empty))) : string.Empty,

                ////JobApplication = task.IdJob != null && task.IdJob > 0 ?
                ////((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType && task.JobApplication.ApplicationNumber != null && task.JobApplication.ApplicationNumber != string.Empty)
                ////? "A# : " + task.JobApplication.ApplicationNumber :
                ////((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
                ////? "LOCATION : " + task.JobApplication.JobApplicationType.Description + "|" + task.JobApplication.StreetWorkingOn + "|" + task.JobApplication.StreetFrom + "|" + task.JobApplication.StreetTo :
                ////((task.JobViolation != null && task.JobViolation.SummonsNumber != null && task.JobViolation.SummonsNumber != string.Empty) ? ("V# : " + task.JobViolation.SummonsNumber) :
                ////((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == depApplicationType)
                ////? task.JobApplication.JobApplicationType.Description + "|" + (string.IsNullOrEmpty(task.JobApplication.StreetWorkingOn) ? "-|" : task.JobApplication.StreetWorkingOn + "|") + (string.IsNullOrEmpty(task.JobApplication.StreetFrom) ? "-|" : task.JobApplication.StreetFrom + "|") + (string.IsNullOrEmpty(task.JobApplication.StreetTo) ? "-|" : task.JobApplication.StreetTo + "|") : string.Empty)
                ////))) : string.Empty,
                ////WorkPermitType =
                ////task.IdJob != null && task.IdJob > 0 ?
                ////((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dobApplicationType)
                ////? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Include("JobWorkType").Where(x => workPermitType.Contains(x.Id)).Select(x => x.JobWorkType.Description))
                ////: ((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == dotApplicationType)
                ////? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Where(x => workPermitType.Contains(x.Id)).Select(x => x.PermitNumber)) : string.Empty)) : string.Empty,//need to change in this line so do not remove this
                ////((task.JobApplication != null && task.JobApplication.JobApplicationType != null && task.JobApplication.JobApplicationType.IdParent == depApplicationType) // do not remove this
                ////? string.Join(", ", rpoContext.JobApplicationWorkPermitTypes.Where(x => workPermitType.Contains(x.Id)).Select(x => x.PermitNumber)) : string.Empty)// do not remove this
                ////)) : "Outer WorkPermit",

                IdJob = task.IdJob,
                JobApplicationType = JobApplicationType,
                IdRfp = task.IdRfp,
                IdContact = task.IdContact,
                IdCompany = task.IdCompany,
                TaskNumber = task.TaskNumber,
                BadgeClass = badgeClass,
                TaskFor = taskFor,
                ProgressNote = task.Notes != null ? task.Notes.OrderByDescending(x => x.LastModifiedDate).Select(x => x.Notes).FirstOrDefault() : string.Empty,
                IdJobStatus = task.Job != null ? (int)task.Job.Status : 0,

                IsGeneric = task.IsGeneric,
            };
        }
        /// <summary>
        /// Orders the task details.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>List&lt;TaskDetailsDTO&gt;.</returns>
        private List<TaskDetailsDTO> OrderTaskDetails(List<TaskDetailsDTO> list)
        {
            //Generic Task Changes
            //List<int> jobTask = list.Where(x => x.IdJob > 0).OrderBy(x => x.CompleteBy).Select(x => x.IdJob ?? 0).Distinct().ToList();
            List<int> jobTask = list.Where(x => x.IdJob > 0 || x.IsGeneric == true).OrderBy(x => x.CompleteBy).Select(x => x.IdJob ?? 0).Distinct().ToList();
            List<int> rfpTask = list.Where(x => x.IdRfp > 0).OrderBy(x => x.CompleteBy).Select(x => x.IdRfp ?? 0).Distinct().ToList();
            List<int> companyTask = list.Where(x => x.IdCompany > 0).OrderBy(x => x.CompleteBy).Select(x => x.IdCompany ?? 0).Distinct().ToList();
            List<int> contactTask = list.Where(x => x.IdContact > 0).OrderBy(x => x.CompleteBy).Select(x => x.IdContact ?? 0).Distinct().ToList();

            List<TaskDetailsDTO> taskListDetails = new List<TaskDetailsDTO>();
            foreach (int item in jobTask)
            {
                //Generic Task Changes
                //List<TaskDetailsDTO> jobTaskList = list.Where(x => x.IdJob == item).OrderBy(x => x.CompleteBy).ToList();
                List<TaskDetailsDTO> jobTaskList = list.Where(x => x.IdJob == item || x.IsGeneric == true).OrderBy(x => x.CompleteBy).ToList();
                taskListDetails.AddRange(jobTaskList);
            }

            foreach (int item in rfpTask)
            {
                List<TaskDetailsDTO> rfpTaskList = list.Where(x => x.IdRfp == item).OrderBy(x => x.CompleteBy).ToList();
                taskListDetails.AddRange(rfpTaskList);
            }

            foreach (int item in companyTask)
            {
                List<TaskDetailsDTO> companyTaskList = list.Where(x => x.IdCompany == item).OrderBy(x => x.CompleteBy).ToList();
                taskListDetails.AddRange(companyTaskList);
            }

            foreach (int item in contactTask)
            {
                List<TaskDetailsDTO> contactTaskList = list.Where(x => x.IdContact == item).OrderBy(x => x.CompleteBy).ToList();
                taskListDetails.AddRange(contactTaskList);
            }

            //List<int> Taskids = new List<int>();

            //List<TaskDetailsDTO> taskListDetailsFinal = new List<TaskDetailsDTO>();

            //foreach (int item in jobTask)
            //{
            //    Taskids = list.Where(x => x.IdJob == item && x.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() || x.IdTaskStatus == EnumTaskStatus.Unattainable.GetHashCode()).OrderBy(x => x.CompleteBy).Select(d=>d.Id).ToList();

            //    Taskids = list.Where(x => x.IdJob == item && x.CompleteBy != null && Convert.ToDateTime(x.CompleteBy).Date == DateTime.UtcNow.Date).OrderBy(x => x.CompleteBy).Select(d => d.Id).ToList();

            //    Taskids = list.Where(x => x.IdJob == item && x.CompleteBy != null && Convert.ToDateTime(x.CompleteBy).Date == DateTime.UtcNow.AddDays(1).Date).OrderBy(x => x.CompleteBy).Select(d => d.Id).ToList();

            //    Taskids = list.Where(x => x.IdJob == item && x.CompleteBy != null && Convert.ToDateTime(x.CompleteBy).Date >= DateTime.UtcNow.AddDays(1).Date && Convert.ToDateTime(x.CompleteBy).Date <= DateTime.UtcNow.AddDays(5).Date).OrderBy(x => x.CompleteBy).Select(d => d.Id).ToList();

            //    Taskids = list.Where(x => x.IdJob == item && x.CompleteBy != null && Convert.ToDateTime(x.CompleteBy).Date > DateTime.UtcNow.AddDays(5).Date).OrderBy(x => x.CompleteBy).Select(d => d.Id).ToList();

            //    Taskids = list.Where(x => x.IdJob == item && x.BadgeClass == "red").OrderBy(x => x.CompleteBy).Select(d => d.Id).ToList();

            //    Taskids = Taskids.Distinct().ToList();

            //    taskListDetailsFinal = list.Where(x => Taskids.Contains(x.Id)).ToList();

            //    //taskListDetailsFinal.AddRange(jobTaskList5);
            //    //taskListDetailsFinal.AddRange(jobTaskList1);
            //    //taskListDetailsFinal.AddRange(jobTaskList2);
            //    //taskListDetailsFinal.AddRange(jobTaskList3);
            //    //taskListDetailsFinal.AddRange(jobTaskList4);
            //    //taskListDetailsFinal.AddRange(jobTaskList);
            //    taskListDetailsFinal.AddRange(taskListDetails);
            //}

            return taskListDetails;
        }
        private List<TasklistDTO> OrderTaskDetails1(IEnumerable<TasklistDTO> list)
        {
            //Generic Task Changes
            //List<int> jobTask = list.Where(x => x.IdJob > 0).OrderBy(x => x.CompleteBy).Select(x => x.IdJob ?? 0).Distinct().ToList();
            List<int> jobTask = list.Where(x => x.IdJob > 0 || x.IsGeneric == true).OrderBy(x => x.CompleteBy).Select(x => x.IdJob ?? 0).Distinct().ToList();
            List<int> rfpTask = list.Where(x => x.IdRfp > 0).OrderBy(x => x.CompleteBy).Select(x => x.IdRfp ?? 0).Distinct().ToList();
            List<int> companyTask = list.Where(x => x.IdCompany > 0).OrderBy(x => x.CompleteBy).Select(x => x.IdCompany ?? 0).Distinct().ToList();
            List<int> contactTask = list.Where(x => x.IdContact > 0).OrderBy(x => x.CompleteBy).Select(x => x.IdContact ?? 0).Distinct().ToList();

            List<TasklistDTO> taskListDetails = new List<TasklistDTO>();
            foreach (int item in jobTask)
            {
                //Generic Task Changes
                //List<TaskDetailsDTO> jobTaskList = list.Where(x => x.IdJob == item).OrderBy(x => x.CompleteBy).ToList();
                List<TasklistDTO> jobTaskList = list.Where(x => x.IdJob == item || x.IsGeneric == true).OrderBy(x => x.CompleteBy).ToList();
                taskListDetails.AddRange(jobTaskList);
            }

            foreach (int item in rfpTask)
            {
                List<TasklistDTO> rfpTaskList = list.Where(x => x.IdRfp == item).OrderBy(x => x.CompleteBy).ToList();
                taskListDetails.AddRange(rfpTaskList);
            }

            foreach (int item in companyTask)
            {
                List<TasklistDTO> companyTaskList = list.Where(x => x.IdCompany == item).OrderBy(x => x.CompleteBy).ToList();
                taskListDetails.AddRange(companyTaskList);
            }

            foreach (int item in contactTask)
            {
                List<TasklistDTO> contactTaskList = list.Where(x => x.IdContact == item).OrderBy(x => x.CompleteBy).ToList();
                taskListDetails.AddRange(contactTaskList);
            }

            //List<int> Taskids = new List<int>();

            //List<TaskDetailsDTO> taskListDetailsFinal = new List<TaskDetailsDTO>();

            //foreach (int item in jobTask)
            //{
            //    Taskids = list.Where(x => x.IdJob == item && x.IdTaskStatus == EnumTaskStatus.Completed.GetHashCode() || x.IdTaskStatus == EnumTaskStatus.Unattainable.GetHashCode()).OrderBy(x => x.CompleteBy).Select(d=>d.Id).ToList();

            //    Taskids = list.Where(x => x.IdJob == item && x.CompleteBy != null && Convert.ToDateTime(x.CompleteBy).Date == DateTime.UtcNow.Date).OrderBy(x => x.CompleteBy).Select(d => d.Id).ToList();

            //    Taskids = list.Where(x => x.IdJob == item && x.CompleteBy != null && Convert.ToDateTime(x.CompleteBy).Date == DateTime.UtcNow.AddDays(1).Date).OrderBy(x => x.CompleteBy).Select(d => d.Id).ToList();

            //    Taskids = list.Where(x => x.IdJob == item && x.CompleteBy != null && Convert.ToDateTime(x.CompleteBy).Date >= DateTime.UtcNow.AddDays(1).Date && Convert.ToDateTime(x.CompleteBy).Date <= DateTime.UtcNow.AddDays(5).Date).OrderBy(x => x.CompleteBy).Select(d => d.Id).ToList();

            //    Taskids = list.Where(x => x.IdJob == item && x.CompleteBy != null && Convert.ToDateTime(x.CompleteBy).Date > DateTime.UtcNow.AddDays(5).Date).OrderBy(x => x.CompleteBy).Select(d => d.Id).ToList();

            //    Taskids = list.Where(x => x.IdJob == item && x.BadgeClass == "red").OrderBy(x => x.CompleteBy).Select(d => d.Id).ToList();

            //    Taskids = Taskids.Distinct().ToList();

            //    taskListDetailsFinal = list.Where(x => Taskids.Contains(x.Id)).ToList();

            //    //taskListDetailsFinal.AddRange(jobTaskList5);
            //    //taskListDetailsFinal.AddRange(jobTaskList1);
            //    //taskListDetailsFinal.AddRange(jobTaskList2);
            //    //taskListDetailsFinal.AddRange(jobTaskList3);
            //    //taskListDetailsFinal.AddRange(jobTaskList4);
            //    //taskListDetailsFinal.AddRange(jobTaskList);
            //    taskListDetailsFinal.AddRange(taskListDetails);
            //}

            return taskListDetails;
        }

    }
}