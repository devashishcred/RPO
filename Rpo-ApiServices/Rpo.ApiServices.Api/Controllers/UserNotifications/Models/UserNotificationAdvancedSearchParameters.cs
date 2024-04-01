// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-24-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-24-2018
// ***********************************************************************
// <copyright file="UserNotificationAdvancedSearchParameters.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class User Notification Advanced Search Parameters.</summary>
// ***********************************************************************

/// <summary>
/// The User Notifications namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.UserNotifications
{
    using Rpo.ApiServices.Api.DataTable;

    /// <summary>
    /// Class User Notification Advanced Search Parameters.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
    public class UserNotificationAdvancedSearchParameters : DataTableParameters
    {
        /// <summary>
        /// Gets or sets the identifier employee.
        /// </summary>
        /// <value>The identifier employee.</value>
        public int IdEmployee { get; set; }
    }
}