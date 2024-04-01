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

namespace Rpo.ApiServices.Api.Controllers.CustomerNotifications
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
    using Models;

    /// <summary>
    /// Class User Notifications Controller.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.Controllers.HubApiController{Rpo.ApiServices.Api.Hubs.GroupHub}" />
    /// <seealso cref="System.Web.Http.ApiController" />
    public class CustomerNotificationsController : HubApiController<GroupHub>
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
        public IHttpActionResult GetCustomerNotifications([FromUri] CustomerNotificationAdvancedSearchParameters dataTableParameters)
        {
            var customerNotifications = rpoContext.CustomerNotifications.Include("CustomerNotified").Where(x => x.IdCustomerNotified == dataTableParameters.IdCustomer).AsQueryable();

            var recordsTotal = customerNotifications.Count();
            var recordsFiltered = recordsTotal;

            var result = customerNotifications.Distinct()
                .AsEnumerable()
                .Select(c => Format(c))
                .AsQueryable()
                .DataTableParameters(dataTableParameters, out recordsFiltered)
                .ToArray();

            foreach (CustomerNotification item in customerNotifications)
            {
                item.IsView = true;
            }
            rpoContext.SaveChanges();
            Common.SendCustomerInAppNotifications(dataTableParameters.IdCustomer, Hub);

            return Ok(new DataTableResponse
            {
                Draw = dataTableParameters.Draw,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
                Data = result.OrderByDescending(x => x.NotificationDate)
            });
        }

        /// <summary>
        /// Gets the customer notification badge list.
        /// </summary>
        /// <param name="idCustomer">The identifier customer.</param>
        /// <returns>Gets the document types.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/customers/{idCustomer}/customernotifications/badgelist")]
        public IHttpActionResult GetCustomerNotificationBadgeList(int idCustomer)
        {
            var result = rpoContext.CustomerNotifications
                           .Where(x => x.IdCustomerNotified == idCustomer)
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
        /// Gets the customer notification count.
        /// </summary>
        /// <param name="idCustomer">The identifier customer.</param>
        /// <returns> Gets the customer notification count.</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/customers/{idCustomer}/customernotifications/count")]
        [ResponseType(typeof(int))]
        public int GetCustomerNotificationCount(int idCustomer)
        {
            return rpoContext.CustomerNotifications.Where(x => x.IdCustomerNotified == idCustomer && x.IsView == false).Count();
        }

        /// <summary>
        /// Gets the customer notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Gets the customer notification.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CustomerNotificationDetail))]
        public IHttpActionResult GetCustomerNotification(int id)
        {
            CustomerNotification customerNotification = rpoContext.CustomerNotifications.Include("CustomerNotified").FirstOrDefault(x => x.Id == id);

            if (customerNotification == null)
            {
                return this.NotFound();
            }

            return Ok(FormatDetails(customerNotification));
        }

        /// <summary>
        /// Deletes the type of the document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Deletes the type of the document.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CustomerNotification))]
        public IHttpActionResult DeleteCustomerNotification(int id)
        {
            CustomerNotification customerNotification = rpoContext.CustomerNotifications.Find(id);
            if (customerNotification == null)
            {
                return this.NotFound();
            }

            rpoContext.CustomerNotifications.Remove(customerNotification);
            rpoContext.Entry(customerNotification).State = EntityState.Deleted;
            rpoContext.SaveChanges();

            Common.SendCustomerInAppNotifications(customerNotification.IdCustomerNotified, Hub);
            return Ok(customerNotification);
        }

        /// <summary>
        /// Reads the user notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>update Reads the user notification..</returns>
        [Route("api/customernotifications/read/{id}")]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CustomerNotification))]
        public IHttpActionResult ReadCustomerNotification(int id)
        {
            CustomerNotification customerNotification = rpoContext.CustomerNotifications.Find(id);
            if (customerNotification == null)
            {
                return this.NotFound();
            }

            customerNotification.IsRead = true;
            customerNotification.IsView = true;

            rpoContext.SaveChanges();

            Common.SendCustomerInAppNotifications(customerNotification.IdCustomerNotified, Hub);
            return Ok(customerNotification);
        }

        /// <summary>
        /// Views the customer notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>View the customer notification..</returns>
        [Route("api/customernotifications/view/{id}")]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CustomerNotification))]
        public IHttpActionResult ViewCustomerNotification(int id)
        {
            CustomerNotification customerNotification = rpoContext.CustomerNotifications.Find(id);
            if (customerNotification == null)
            {
                return this.NotFound();
            }

            customerNotification.IsView = true;

            rpoContext.SaveChanges();

            Common.SendCustomerInAppNotifications(customerNotification.IdCustomerNotified, Hub);
            return Ok(customerNotification);
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
        private bool CustomerNotificationExists(int id)
        {
            return rpoContext.CustomerNotifications.Count(e => e.Id == id) > 0;
        }

        /// <summary>
        /// Formats the specified document type.
        /// </summary>
        /// <param name="CustomerNotification">Type of the document.</param>
        /// <returns>CustomerNotificationDTO.</returns>
        private CustomerNotificationDTO Format(CustomerNotification customerNotification)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new CustomerNotificationDTO
            {
                Id = customerNotification.Id,
                NotificationMessage = customerNotification.NotificationMessage,
                IdCustomerNotified = customerNotification.IdCustomerNotified,
                IsRead = customerNotification.IsRead,
                IsView = customerNotification.IsView,
                RedirectionUrl = customerNotification.RedirectionUrl,
                UserNotified = customerNotification.CustomerNotified != null ? customerNotification.CustomerNotified.FirstName + " " + customerNotification.CustomerNotified.LastName : string.Empty,
                //remove production build commit
                // NotificationDate = customerNotification.NotificationDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(customerNotification.NotificationDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : customerNotification.NotificationDate,
                NotificationDate = customerNotification.NotificationDate != null ? Convert.ToDateTime(customerNotification.NotificationDate) : customerNotification.NotificationDate,
            };
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="customerNotification">Type of the document.</param>
        /// <returns>CustomerNotification.</returns>
        private CustomerNotificationDetail FormatDetails(CustomerNotification customerNotification)
        {
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new CustomerNotificationDetail
            {
                Id = customerNotification.Id,
                NotificationMessage = customerNotification.NotificationMessage,
                IdCustomerNotified = customerNotification.IdCustomerNotified,
                IsRead = customerNotification.IsRead,
                IsView = customerNotification.IsView,
                RedirectionUrl = customerNotification.RedirectionUrl,
                UserNotified = customerNotification.CustomerNotified != null ? customerNotification.CustomerNotified.FirstName + " " + customerNotification.CustomerNotified.LastName : string.Empty,
                //remove production build commit
                // NotificationDate = customerNotification.NotificationDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(customerNotification.NotificationDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : customerNotification.NotificationDate,
                NotificationDate = customerNotification.NotificationDate != null ? Convert.ToDateTime(customerNotification.NotificationDate) : customerNotification.NotificationDate,

            };
        }
    }
}