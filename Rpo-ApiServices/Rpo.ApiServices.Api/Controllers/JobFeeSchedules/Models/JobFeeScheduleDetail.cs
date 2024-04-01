// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-14-2018
// ***********************************************************************
// <copyright file="JobFeeScheduleDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Fee Schedule Detail.</summary>
// ***********************************************************************

using Rpo.ApiServices.Model.Models.Enums;
/// <summary>
/// The Job Fee Schedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobFeeSchedules
{
    /// <summary>
    /// Class Job Fee Schedule Detail.
    /// </summary>
    public class JobFeeScheduleDetail
    {
        public RfpCostType CostType { get; internal set; }

        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int IdJobFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public string ItemName { get; set; }

        public string AllIds { get; set; }

        public int? Partof { get; set; }

        public double? TotalGroupCost { get; set; }

        public int IdMilestone { get; set; }
        public bool IsMilestone { get; set; }
        public string MilestoneServiceId { get; set; }
    }
}