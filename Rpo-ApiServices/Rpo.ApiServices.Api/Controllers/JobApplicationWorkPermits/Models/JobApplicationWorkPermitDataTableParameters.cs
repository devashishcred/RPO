// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-09-2018
// ***********************************************************************
// <copyright file="JobApplicationWorkPermitDataTableParameters.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Application Work Permit Data Table Parameters.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.JobApplicationWorkPermits
{
    /// <summary>
    /// Class Job Application Work Permit Data Table Parameters.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
    public class JobApplicationWorkPermitDataTableParameters : DataTable.DataTableParameters
    {
        /// <summary>
        /// Gets or sets the identifier job application.
        /// </summary>
        /// <value>The identifier job application.</value>
        public int? IdJobApplication { get; set; }

        public int? IdJob { get; set; }
    }
}