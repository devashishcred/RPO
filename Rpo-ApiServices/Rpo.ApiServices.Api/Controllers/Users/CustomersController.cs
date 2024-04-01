

namespace Rpo.ApiServices.Api.Controllers.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Filters;
    using Permissions;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;

    public class CustomersController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

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
        /// Gets Customers the information.
        /// </summary>
        /// <returns>IHttpActionResult. its return the Customers detail and Customer Permissions list and Menus detail for the particular user and also notification count</returns>
        [ResponseType(typeof(UserDTO))]
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult CustomerInfo()
        {
            var customer = rpoContext.Customers.Where(e => e.EmailAddress.ToLower() == this.User.Identity.Name.ToLower()).FirstOrDefault();

            if (customer == null)
            {
                return this.NotFound();
            }
            List<int> customerPermissions = customer.Permissions != null && !string.IsNullOrEmpty(customer.Permissions) ? (customer.Permissions.Split(',') != null && customer.Permissions.Split(',').Any() ? customer.Permissions.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            return Ok(new UserDTO
            {
                Name = customer.FirstName + " " + customer.LastName,
                EmployeeId = customer.Id,
                NotificationCount = rpoContext.CustomerNotifications.Where(x => x.IdCustomerNotified == customer.Id && x.IsView == false).Count(),
                Menu = this.Menu(customer),
                PermissionDetails = rpoContext.Permissions.Where(x => customerPermissions.Contains(x.Id)).Select(x => new PermissionsDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    GroupName = x.GroupName
                })
            });
        }
        /// <summary>
        /// Menus the specified customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>IEnumerable&lt;Menu&gt;.</returns>
        public IEnumerable<Menu> Menu(Customer customer)
        {
            var result = new List<Menu>();
            //if (Common.CheckUserPermission(customer.Permissions, Enums.Permission.ViewDashboard))
            //{
            //    result.Add(new Menu("DASHBOARD", "/customer-dashboard", "<span class=\"material-symbols-outlined filled\">swap_driving_apps_wheel</span>"));

            //}
            if (Common.CheckUserPermission(customer.Permissions, Enums.Permission.ViewJob)
                || Common.CheckUserPermission(customer.Permissions, Enums.Permission.AddJob))
            {
                result.Add(new Menu("PROJECTS", "/jobs", "<span class=\"material-symbols-outlined filled\">assignment</span>"));
            }

            var reportsMenu = new Menu("REPORTS", null, "<span class=\"material-symbols-outlined filled\">insert_chart</span>");
            bool ismenu = false;
            if (Common.CheckUserPermission(customer.Permissions, Enums.Permission.ViewAllViolationsReport))
            {
                ismenu = true;
                reportsMenu.Items.Add(new Menu("Violation Reports", "/allviolationreport"));
            }
            if (Common.CheckUserPermission(customer.Permissions, Enums.Permission.ViewPermitExpiryReport))
            {
                ismenu = true;
                // reportsMenu.Items.Add(new Menu("Permits Expiry Reports", "/permitsexpiryreport"));
                reportsMenu.Items.Add(new Menu("Permit Expiration Reports", "/permitsexpiryreport"));
            }
            if (ismenu)
                result.Add(reportsMenu);
            if (Common.CheckUserPermission(customer.Permissions, Enums.Permission.ViewReferenceLinks))
            {
                var referencesMenu = new Menu("Resources", null, "<span class=\"material-symbols-outlined filled\">chat_bubble</span>")
                {

                };
                if (Common.CheckUserPermission(customer.Permissions, Enums.Permission.ViewReferenceLinks))
                {
                    referencesMenu.Items.AddRange(rpoContext.ReferenceLinks.AsEnumerable().Select(rl => new Menu(rl.Name, Regex.Replace(rl.Name, @"\s+", ""))
                    {
                        Url = rl.Url
                    }));
                }
                result.Add(referencesMenu);
            }
            //if (Common.CheckUserPermission(customer.Permissions, Enums.Permission.ViewCustomer) || Common.CheckUserPermission(customer.Permissions, Enums.Permission.AddCustomer))
            //{  result.Add(new Menu("MY ACCOUNT", "/customer-my-account", "<span class=\"material-symbols-outlined filled\">assignment</span>"));

            //}

            // }
            return result;
        }
    }
}
