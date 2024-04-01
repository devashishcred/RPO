
namespace Rpo.ApiServices.Api.Controllers.NotificationSettings
{
    using Rpo.ApiServices.Api.Controllers.NotificationSettings.Models;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Tools;

    public class NotificationSettingsController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private RpoContext rpoContext = new RpoContext();
        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>IHttpActionResult.</returns>
        [ResponseType(typeof(DataTableResponse))]
        public IEnumerable<CustomerNotificationSetting> GetNotificationSettings()
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewEmployeeUserGroup)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployeeUserGroup)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteEmployeeUserGroup))
            {
              
                return this.rpoContext.CustomerNotificationSettings;
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CustomerNotificationDetails))]
        [Route("api/NotificationSettings/CustomerNotifcationSettings")]
        public IHttpActionResult CustNotifcationSettings(CustomerNotificationDetails CustNotifiSetting)
        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployeeUserGroup))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                CustomerNotificationSetting cust = new CustomerNotificationSetting();
                cust.IdCustomer = CustNotifiSetting.IdCustomer;
                cust.ProjectAccessEmail = CustNotifiSetting.ProjectAccessEmail;
                cust.ProjectAccessInApp = CustNotifiSetting.ProjectAccessInApp;
                cust.ViolationEmail = CustNotifiSetting.ViolationEmail;
                cust.ViolationInapp = CustNotifiSetting.ViolationInApp;
                rpoContext.CustomerNotificationSettings.Add(cust);
                rpoContext.SaveChanges();

                return Ok(cust);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
        /// <summary>
        /// Puts the Customer Notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name=" CustomerNotification">The Customer Notification.</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(void))]
        [Route("api/NotificationSettings/CustomerNotificationSetting/{idCustomer}")]
        public IHttpActionResult PutCustomerNotificationSetting(int idCustomer, CustomerNotificationDetails CustNotifiSetting)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            if (idCustomer != CustNotifiSetting.IdCustomer)
            {
                return this.BadRequest();
            }
            CustomerNotificationSetting cust = new CustomerNotificationSetting();
            cust.Id = CustNotifiSetting.Id;
            cust.IdCustomer = CustNotifiSetting.IdCustomer;
            cust.ProjectAccessEmail = CustNotifiSetting.ProjectAccessEmail;
            cust.ProjectAccessInApp = CustNotifiSetting.ProjectAccessInApp;
            cust.ViolationEmail = CustNotifiSetting.ViolationEmail;
            cust.ViolationInapp = CustNotifiSetting.ViolationInApp;
            rpoContext.Entry(cust).State = EntityState.Modified;
                        
            try
            {
                rpoContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.CustomerNotificationSettingExists(idCustomer))
                {
                    return this.NotFound();
                }
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        /// Gets the Customer Notification.
        /// </summary>
        /// <param name="id">The Customer Notification..</param>
        /// <returns>IHttp Action Result.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(CustomerNotificationSetting))]
        [Route("api/NotificationSettings/GetCustomerNotificationSetting/{idCustomer}")]
        public IHttpActionResult GetCustomerNotificationSetting(int idCustomer)
        {
            CustomerNotificationSetting  customerNotificationSettings = this.rpoContext.CustomerNotificationSettings.Where(x=>x.IdCustomer==idCustomer).FirstOrDefault();
            if (customerNotificationSettings == null)
            {
                return Ok(false);
            }

            return this.Ok(customerNotificationSettings);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.rpoContext.Dispose();
            }

            base.Dispose(disposing);
        }
        /// <summary>
        /// CustomerNotificationSetting the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CustomerNotificationSettingExists(int idCustomer)
        {
            return this.rpoContext.CustomerNotificationSettings.Count(e => e.IdCustomer == idCustomer) > 0;
        }
    }
}
