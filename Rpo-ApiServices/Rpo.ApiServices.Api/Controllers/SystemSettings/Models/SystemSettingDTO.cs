// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-15-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="DocumentTypeDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class System Setting DTO.</summary>
// ***********************************************************************

/// <summary>
/// The System Settings namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.SystemSettings
{
    using System;
    using System.Collections.Generic;
    using Employees;

    /// <summary>
    /// Class System Setting DTO.
    /// </summary>
    public class SystemSettingDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the created by employee.
        /// </summary>
        /// <value>The name of the created by employee.</value>
        public string CreatedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public string LastModified { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}