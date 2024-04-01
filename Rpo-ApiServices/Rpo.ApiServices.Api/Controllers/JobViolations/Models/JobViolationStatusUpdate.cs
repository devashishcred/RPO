// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-16-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobViolationStatusUpdate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Violation Status Update.</summary>
// ***********************************************************************

/// <summary>
/// The Job Violations namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobViolations
{
    /// <summary>
    /// Class Job Violation Status Update.
    /// </summary>
    public class JobViolationStatusUpdate
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the status of summons notice.
        /// </summary>
        /// <value>The status of summons notice.</value>
        public string StatusOfSummonsNotice { get;  set; }
    }
}