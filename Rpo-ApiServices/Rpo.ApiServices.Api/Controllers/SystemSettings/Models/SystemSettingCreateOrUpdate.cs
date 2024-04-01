// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-20-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="SystemSettingCreateOrUpdate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class System Setting Create Or Update.</summary>
// ***********************************************************************

/// <summary>
/// The System Settings namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.SystemSettings
{
    /// <summary>
    /// Class System Setting Create Or Update.
    /// </summary>
    public class SystemSettingCreateOrUpdate
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
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int[] Value { get; set; }
    }
}