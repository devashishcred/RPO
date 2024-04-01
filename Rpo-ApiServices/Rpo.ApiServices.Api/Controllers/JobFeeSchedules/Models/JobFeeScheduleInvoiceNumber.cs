// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobFeeScheduleInvoiceNumber.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Fee Schedule Invoice Number.</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobFeeSchedules
{
    /// <summary>
    /// Class Job Fee Schedule Invoice Number.
    /// </summary>
    public class JobFeeScheduleInvoiceNumber
    {
        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int IdJobFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        public string InvoiceNumber { get; set; }
    }
}