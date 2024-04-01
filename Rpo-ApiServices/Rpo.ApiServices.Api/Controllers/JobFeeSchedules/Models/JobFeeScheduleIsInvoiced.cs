// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobFeeScheduleIsInvoiced.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Fee Schedule Is Invoiced.</summary>
// ***********************************************************************

/// <summary>
/// The Job Fee Schedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobFeeSchedules
{
    /// <summary>
    /// Class Job Fee Schedule Is Invoiced.
    /// </summary>
    public class JobFeeScheduleIsInvoiced
    {
        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int IdJobFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is invoiced.
        /// </summary>
        /// <value><c>true</c> if this instance is invoiced; otherwise, <c>false</c>.</value>
        public bool IsInvoiced { get; set; }
    }
}