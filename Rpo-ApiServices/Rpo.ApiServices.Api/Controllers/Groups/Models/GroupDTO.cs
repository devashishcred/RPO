// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-20-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-24-2018
// ***********************************************************************
// <copyright file="GroupDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Group DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Groups namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Groups
{
    using System.Collections.Generic;
    using Rpo.ApiServices.Api.Controllers.Permissions;

    /// <summary>
    /// Class Group DTO.
    /// </summary>
    public class GroupDTO
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the Permissions
        /// </summary>
        /// <value>The Permissions.</value>
        public int[] Permissions { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets all permissions.
        /// </summary>
        /// <value>All permissions.</value>
        public List<PermissionModuleDTO> AllPermissions { get; set; }
    }
}