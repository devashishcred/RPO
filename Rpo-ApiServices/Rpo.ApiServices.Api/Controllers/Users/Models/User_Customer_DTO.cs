

// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 05-09-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 05-09-2023
// ***********************************************************************
// <copyright file="UserDTO.cs" company="CREDENCYS">
//     Copyright ©  2023
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
    public class User_Customer_DTO
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
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the notification count.
        /// </summary>
        /// <value>The notification count.</value>
        public int NotificationCount { get; set; }
    }
}