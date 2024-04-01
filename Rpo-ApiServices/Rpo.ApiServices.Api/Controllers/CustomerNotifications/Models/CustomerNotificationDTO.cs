// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-14-2018
// ***********************************************************************
// <copyright file="CustomerNotificationDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class .Customer Notification DTO.</summary>
// ***********************************************************************

/// <summary>
/// The UserNotifications namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.CustomerNotifications.Models
{
    using System;

    /// <summary>
    /// Class User Notification DTO.
    /// </summary>
    public class CustomerNotificationDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the notification message.
        /// </summary>
        /// <value>The notification message.</value>
        public string NotificationMessage { get; set; }

        /// <summary>
        /// Gets or sets the notification date.
        /// </summary>
        /// <value>The notification date.</value>
        public DateTime NotificationDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier user notified.
        /// </summary>
        /// <value>The identifier user notified.</value>
        public int IdCustomerNotified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read.
        /// </summary>
        /// <value><c>true</c> if this instance is read; otherwise, <c>false</c>.</value>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// Gets or sets the user notified.
        /// </summary>
        /// <value>The user notified.</value>
        public string UserNotified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is view.
        /// </summary>
        /// <value><c>true</c> if this instance is view; otherwise, <c>false</c>.</value>
        public bool IsView { get; set; }

        /// <summary>
        /// Gets or sets the redirection URL.
        /// </summary>
        /// <value>The redirection URL.</value>
        public string RedirectionUrl { get; set; }
    }
}