// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="HubApiController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Hub Api Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using System;
    using System.Web.Http;
    using Quartz;
    /// <summary>
    /// Class Hub Api Controller.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Web.Http.ApiController" />
    public abstract class HubApiController<T> : ApiController where T : IHub
    {
        /// <summary>
        /// The hub context
        /// </summary>
        Lazy<IHubContext> hubContext = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<T>());

        /// <summary>
        /// Gets the hub.
        /// </summary>
        /// <value>The hub.</value>
        protected IHubContext Hub
        {
            get { return hubContext.Value; }
        }
    }
}