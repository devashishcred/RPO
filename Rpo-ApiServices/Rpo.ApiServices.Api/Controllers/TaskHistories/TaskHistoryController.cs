// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-20-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="TaskHistoryController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Task History Controller.</summary>
// ***********************************************************************

/// <summary>
/// The Task Histories namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.TaskHistories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Http;
    using Filters;
    using Model.Models;
    using Model.Models.Enums;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;

    /// <summary>
    /// Class Task History Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class TaskHistoryController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the task history.
        /// </summary>
        /// <param name="idJobFeeSchedule">The identifier job fee schedule.</param>
        /// <returns>IQueryable&lt;TaskHistoryDTO&gt;.</returns>
        [Authorize]
        [RpoAuthorize]
        public IQueryable<TaskHistoryDTO> GetTaskHistory(int idJobFeeSchedule)
        {
            IQueryable<TaskHistoryDTO> taskHistories = rpoContext.TaskHistories.Include("Employee").Include("Task.JobTransmittals.EmailType")
                .Where(x => x.IdJobFeeSchedule == idJobFeeSchedule )
                .AsEnumerable()
                .Select(c => FormatDetail(c)).OrderByDescending(x => x.HistoryDate).AsQueryable();

            return taskHistories;
        }
        /// <summary>
        /// Bind the history
        /// </summary>
        /// <param name="taskHistory"></param>
        /// <returns></returns>
        private TaskHistoryDTO FormatDetail(TaskHistory taskHistory)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);
            string description = string.Empty;
            if ((taskHistory.Task != null && taskHistory.Task.JobFeeSchedule != null && taskHistory.Task.JobFeeSchedule.RfpWorkType != null && taskHistory.Task.JobFeeSchedule.RfpWorkType.CostType == RfpCostType.FixedCost)
                || (taskHistory.Task != null && taskHistory.Task.RfpJobType != null && taskHistory.Task.RfpJobType.CostType != null && taskHistory.Task.RfpJobType.CostType == RfpCostType.FixedCost))
            {
                description = string.Empty;
                description = TaskHistoryMessages.TaskCompletedHistoryFixedPriceMessage
                              .Replace("##TaskNumber##", taskHistory != null && taskHistory.Task != null ? taskHistory.Task.TaskNumber : string.Empty)
                              .Replace("##QuantityOfTheServiceItem##", taskHistory != null && taskHistory.Task != null ? Convert.ToString(taskHistory.Task.ServiceQuantity) : string.Empty)
                              .Replace("##TaskId##", taskHistory != null && taskHistory.Task != null ? taskHistory.Task.Id.ToString() : string.Empty);
            }
            else { 
                description = string.Empty;
                description = taskHistory.Description
                             .Replace("##TaskNumber##", taskHistory != null && taskHistory.Task != null ? taskHistory.Task.TaskNumber : string.Empty)
                             .Replace("##TaskId##", taskHistory != null && taskHistory.Task != null ? taskHistory.Task.Id.ToString() : string.Empty);
                
            }

            List<JobTransmittal> jobTransmittalList = taskHistory.Task != null && taskHistory.Task.JobTransmittals != null ? taskHistory.Task.JobTransmittals.ToList() : null;

            foreach (JobTransmittal item in jobTransmittalList)
            {
                string transmittalDetail = TaskHistoryMessages.TaskTramittalHistoryMessage
                    .Replace("##TaskNumber##", taskHistory != null && taskHistory.Task != null ? taskHistory.Task.TaskNumber : string.Empty)
                    .Replace("##TransmittalId##", item.Id.ToString())
                    .Replace("##TransmittalType##", item != null && item.EmailType != null ? item.EmailType.Name : string.Empty)
                    .Replace("##TransmittalNumber##", item != null ? item.TransmittalNumber : string.Empty);
                description = description + "<br />       - " + transmittalDetail;
            }

            return new TaskHistoryDTO
            {
                Id = taskHistory.Id,
                Description = description,
                IdEmployee = taskHistory.IdEmployee,
                FirstName = taskHistory.Employee != null ? taskHistory.Employee.FirstName : string.Empty,
                LastName = taskHistory.Employee != null ? taskHistory.Employee.LastName : string.Empty,
                HistoryDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(taskHistory.HistoryDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)),
                IdTask = taskHistory.IdTask,
                TaskNumber = taskHistory.Task != null ? taskHistory.Task.TaskNumber : string.Empty,
                IdJobFeeSchedule = taskHistory.IdJobFeeSchedule
            };
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
    }
}