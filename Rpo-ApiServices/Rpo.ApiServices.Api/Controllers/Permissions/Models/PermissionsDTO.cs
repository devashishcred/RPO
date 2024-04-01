// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-17-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-19-2018
// ***********************************************************************
// <copyright file="PermissionsDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Permissions DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Permissions namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Permissions
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Class Permissions DTO.
    /// </summary>
    public class PermissionModuleDTO
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets the groups.
        /// </summary>
        /// <value>The groups.</value>
        public List<PermissionGroupDTO> Groups { get; set; }
    }

    /// <summary>
    /// Class Permission Group DTO.
    /// </summary>
    public class PermissionGroupDTO
    {
        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public List<PermissionsDTO> Permissions { get; set; }
    }

    /// <summary>
    /// Class Permissions DTO.
    /// </summary>
    public class PermissionsDTO
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
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
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
        /// Gets or sets the last modified by employee.
        /// </summary>
        /// <value>The last modified by employee.</value>
        public string LastModifiedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets or sets the permission class.
        /// </summary>
        /// <value>The permission class.</value>
        public string PermissionClass { get; set; }
        public string ModuleName { get; set; }
        public int? DisplayOrder { get; set; }
    }
}