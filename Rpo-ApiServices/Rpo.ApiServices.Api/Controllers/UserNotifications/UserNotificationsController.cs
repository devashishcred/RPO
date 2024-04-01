// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-14-2018
// ***********************************************************************
// <copyright file="UserNotificationsController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class User Notifications Controller.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.UserNotifications
{
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Rpo.ApiServices.Api.Tools;
    using System;
    using Rpo.ApiServices.Api.DataTable;
    using Hubs;
    using Filters;

    /// <summary>
    /// Class User Notifications Controller.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.Controllers.HubApiController{Rpo.ApiServices.Api.Hubs.GroupHub}" />
    /// <seealso cref="System.Web.Http.ApiController" />
    public class UserNotificationsController : HubApiController<GroupHub>
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Gets the document types.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>Gets the document types List.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetUserNotifications([FromUri] UserNotificationAdvancedSearchParameters dataTableParameters)
        {
            var userNotifications = rpoContext.UserNotifications.Include("UserNotified").Where(x => x.IdUserNotified == dataTableParameters.IdEmployee).AsQueryable();

            var recordsTotal = userNotifications.Count();
            var recordsFiltered = recordsTotal;

            var result = userNotifications.Distinct()
                .AsEnumerable()
                .Select(c => Format(c))
                .AsQueryable()
                .DataTableParameters(dataTableParameters, out recordsFiltered)
                .ToArray();

            foreach (UserNotification item in userNotifications)
            {
                item.IsView = true;
            }
            rpoContext.SaveChanges();
            Common.SendInAppNotifications(dataTableParameters.IdEmployee, Hub);

            return Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result.OrderByDescending(x=>x.NotificationDate)
            });
        }

        /// <summary>
        /// Gets the user notification badge list.
        /// </summary>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <returns>Gets the document types.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/employees/{idEmployee}/usernotifications/badgelist")]
        public IHttpActionResult GetUserNotificationBadgeList(int idEmployee)
        {
            var result = rpoContext.UserNotifications
                           .Where(x => x.IdUserNotified == idEmployee)
                           .OrderByDescending(x => x.NotificationDate)
                           .Take(11)
                           .AsEnumerable()
                           .Select(c => new
                           {
                               Id = c.Id,
                               NotificationMessage = c.NotificationMessage,
                               RedirectionUrl = c.RedirectionUrl,
                               NotificatoinDate = TimeZoneInfo.ConvertTimeFromUtc(c.NotificationDate, TimeZoneInfo.FindSystemTimeZoneById(Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey))),
                           }).ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Gets the user notification count.
        /// </summary>
        /// <param name="idEmployee">The identifier employee.</param>
        /// <returns> Gets the user notification count.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/employees/{idEmployee}/usernotifications/count")]
        [ResponseType(typeof(int))]
        public int GetUserNotificationCount(int idEmployee)
        {
            return rpoContext.UserNotifications.Where(x => x.IdUserNotified == idEmployee && x.IsView == false).Count();
        }

        /// <summary>
        /// Gets the user notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the user notification.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(UserNotificationDetail))]
        public IHttpActionResult GetUserNotification(int id)
        {
            UserNotification userNotification = rpoContext.UserNotifications.Include("UserNotified").FirstOrDefault(x => x.Id == id);

            if (userNotification == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(userNotification));
        }

        /// <summary>
        /// Deletes the type of the document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Deletes the type of the document.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(UserNotification))]
        public IHttpActionResult DeleteUserNotification(int id)
        {
            UserNotification userNotification = rpoContext.UserNotifications.Find(id);
            if (userNotification == null)
            {
                return this.NotFound();
            }

            rpoContext.UserNotifications.Remove(userNotification);
            rpoContext.Entry(userNotification).State = EntityState.Deleted;
            rpoContext.SaveChanges();

            Common.SendInAppNotifications(userNotification.IdUserNotified, Hub);
            return Ok(userNotification);
        }

        /// <summary>
        /// Reads the user notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>update Reads the user notification..</returns>
        [Route("api/usernotifications/read/{id}")]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(UserNotification))]
        public IHttpActionResult ReadUserNotification(int id)
        {
            UserNotification userNotification = rpoContext.UserNotifications.Find(id);
            if (userNotification == null)
            {
                return this.NotFound();
            }

            userNotification.IsRead = true;
            userNotification.IsView = true;

            rpoContext.SaveChanges();

            Common.SendInAppNotifications(userNotification.IdUserNotified, Hub);
            return Ok(userNotification);
        }

        /// <summary>
        /// Views the user notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>View the user notification..</returns>
        [Route("api/usernotifications/view/{id}")]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(UserNotification))]
        public IHttpActionResult ViewUserNotification(int id)
        {
            UserNotification userNotification = rpoContext.UserNotifications.Find(id);
            if (userNotification == null)
            {
                return this.NotFound();
            }

            userNotification.IsView = true;

            rpoContext.SaveChanges();

            Common.SendInAppNotifications(userNotification.IdUserNotified, Hub);
            return Ok(userNotification);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rpoContext.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Documents the type exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool UserNotificationExists(int id)
        {
            return rpoContext.UserNotifications.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified document type.
        /// </summary>
        /// <param name="userNotification">Type of the document.</param>
        /// <returns>UserNotificationDTO.</returns>
        private UserNotificationDTO Format(UserNotification userNotification)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new UserNotificationDTO
            {
                Id = userNotification.Id,
                NotificationMessage = userNotification.NotificationMessage,
                IdUserNotified = userNotification.IdUserNotified,
                IsRead = userNotification.IsRead,
                IsView = userNotification.IsView,
                RedirectionUrl = userNotification.RedirectionUrl,
                UserNotified = userNotification.UserNotified != null ? userNotification.UserNotified.FirstName + " " + userNotification.UserNotified.LastName : string.Empty,
                NotificationDate = userNotification.NotificationDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(userNotification.NotificationDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : userNotification.NotificationDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="userNotification">Type of the document.</param>
        /// <returns>UserNotificationDetail.</returns>
        private UserNotificationDetail FormatDetails(UserNotification userNotification)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new UserNotificationDetail
            {
                Id = userNotification.Id,
                NotificationMessage = userNotification.NotificationMessage,
                IdUserNotified = userNotification.IdUserNotified,
                IsRead = userNotification.IsRead,
                IsView = userNotification.IsView,
                RedirectionUrl = userNotification.RedirectionUrl,
                UserNotified = userNotification.UserNotified != null ? userNotification.UserNotified.FirstName + " " + userNotification.UserNotified.LastName : string.Empty,
                NotificationDate = userNotification.NotificationDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(userNotification.NotificationDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : userNotification.NotificationDate,
            };
        }
    }
}