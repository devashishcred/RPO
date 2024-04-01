// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-18-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-01-2018
// ***********************************************************************
// <copyright file="JobScopeDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Scope DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Jobs
{
    using System;

    /// <summary>
    /// Class Job Scope DTO.
    /// </summary>
    public class JobScopeDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public string LastModifiedBy { get; set; }
    }
}
