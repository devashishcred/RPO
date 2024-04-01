// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-24-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-24-2018
// ***********************************************************************
// <copyright file="JobFeeScheduleSearchParameters.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Fee Schedule Search Parameters.</summary>
// ***********************************************************************

/// <summary>
/// The Job Fee Schedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobFeeSchedules
{
    using Rpo.ApiServices.Api.DataTable;
    /// <summary>
    /// Class Job Fee Schedule Search Parameters.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
    public class JobFeeScheduleSearchParameters : DataTableParameters
    {
        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }
    }
}