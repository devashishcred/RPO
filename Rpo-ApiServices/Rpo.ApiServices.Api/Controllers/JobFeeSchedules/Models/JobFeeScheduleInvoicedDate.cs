// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobFeeScheduleInvoicedDate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Fee Schedule Invoiced Date.</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobFeeSchedules
{
    /// <summary>
    /// Class Job Fee Schedule Invoiced Date.
    /// </summary>
    public class JobFeeScheduleInvoicedDate
    {
        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int IdJobFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the invoiced date.
        /// </summary>
        /// <value>The invoiced date.</value>
        public string InvoicedDate { get; set; }
    }
}