// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-24-2018
// ***********************************************************************
// <copyright file="UserDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class User DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Users namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Users
{
    using System.Collections.Generic;
    using Permissions;

    /// <summary>
    /// Class User DTO.
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the menu.
        /// </summary>
        /// <value>The menu.</value>
        public IEnumerable<Menu> Menu { get; set; }

        /// <summary>
        /// Gets or sets the permission details.
        /// </summary>
        /// <value>The permission details.</value>
        public IEnumerable<PermissionsDTO> PermissionDetails { get; set; }

        /// <summary>
        /// Gets the employee identifier.
        /// </summary>
        /// <value>The employee identifier.</value>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the notification count.
        /// </summary>
        /// <value>The notification count.</value>
        public int NotificationCount { get; set; }
    }
}