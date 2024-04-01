// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="JobAssign.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Assign.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Jobs
{
    /// <summary>
    /// Class Job Assign.
    /// </summary>
    public class JobAssign
    {
        /// <summary>
        /// Gets or sets the identifier employee.
        /// </summary>
        /// <value>The identifier employee.</value>
        public int? IdEmployee { get; set; }

        public int? IdJob { get; set; }

        /// <summary>
        /// Gets or sets the name of the employee.
        /// </summary>
        /// <value>The name of the employee.</value>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the employee email.
        /// </summary>
        /// <value>The employee email.</value>
        public string EmployeeEmail { get; set; }

        /// <summary>
        /// Gets or sets the type of the job aplication.
        /// </summary>
        /// <value>The type of the job aplication.</value>
        public string JobAplicationType { get; set; }

        public string JobNumber { get; set; }

        public string RedirectionLink { get; set; }
    }
}