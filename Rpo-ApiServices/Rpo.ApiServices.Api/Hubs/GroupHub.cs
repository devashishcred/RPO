// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-24-2018
// ***********************************************************************
// <copyright file="GroupHub.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Group Hub.</summary>
// ***********************************************************************

/// <summary>
/// The Hubs namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Hubs
{
    using Microsoft.AspNet.SignalR;

    /// <summary>
    /// Class Group Hub.
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.SignalR.Hub" />
    public class GroupHub : Hub
    {
        /// <summary>
        /// Subscribes the specified group name.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        public void Subscribe(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }

        /// <summary>
        /// Unsubscribes the specified group name.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        public void Unsubscribe(string groupName)
        {
            Groups.Remove(Context.ConnectionId, groupName);
        }
    }
}