// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-05-2018
// ***********************************************************************
// <copyright file="JobApplicationDataTableParameters.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Application DataTable Parameters.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.JobApplications
{
    /// <summary>
    /// Class Job Application DataTable Parameters.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
    public class JobApplicationDataTableParameters : DataTable.DataTableParameters
    {
        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdJob { get; set; }

        public int? IdJobApplicationType { get; set; }
       //mb
        //   public int? IdJobWorkType { get; set; }
        public string IdJobWorkType { get; set; }

    }
}