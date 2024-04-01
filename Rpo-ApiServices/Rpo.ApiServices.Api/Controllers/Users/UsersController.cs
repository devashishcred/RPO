// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="UsersController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Users Controller.</summary>
// ***********************************************************************

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

    /// <summary>
    /// Class Users Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class UsersController : ApiController
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
        /// Gets Users the information.
        /// </summary>
        /// <returns>IHttpActionResult. its return the employees detail and employee Permissions list and Menus detail for the particular user and also notification count</returns>
        [ResponseType(typeof(UserDTO))]
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        public IHttpActionResult UserInfo()
        {
            var employee = rpoContext.Employees.Where(e => e.Email.ToLower() == this.User.Identity.Name.ToLower()).FirstOrDefault();

            List<int> employeePermissions = employee.Permissions != null && !string.IsNullOrEmpty(employee.Permissions) ? (employee.Permissions.Split(',') != null && employee.Permissions.Split(',').Any() ? employee.Permissions.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();
            return Ok(new UserDTO
            {
                Name = employee.FirstName + " " + employee.LastName,
                EmployeeId = employee.Id,
                NotificationCount = rpoContext.UserNotifications.Where(x => x.IdUserNotified == employee.Id && x.IsView == false).Count(),
                Menu = this.Menu(employee),
                PermissionDetails = rpoContext.Permissions.Where(x => employeePermissions.Contains(x.Id)).Select(x => new PermissionsDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    GroupName = x.GroupName
                })
            });
        }

        /// <summary>
        /// Menus the specified employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns>IEnumerable&lt;Menu&gt;.</returns>
        public IEnumerable<Menu> Menu(Employee employee)
        {
            var result = new List<Menu>();
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewCompany)
               || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddCompany)
               || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteCompany)
               || Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
               || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
               || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact)
               || Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewAddress)
               || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddAddress)
               || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteAddress)
               )
            {
                var crm = new Menu("CRM", null, "<span class=\"material-symbols-rounded\">laptop_windows</span>");
                if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewCompany)
                   || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddCompany)
                   || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteCompany))
                {
                    crm.Items.Add(new Menu("Companies", "/company"));
                }

                if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
                    || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                    || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
                {
                    crm.Items.Add(new Menu("Contacts", "/contacts"));
                }

                if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewAddress)
                    || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddAddress)
                    || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteAddress))
                {
                    crm.Items.Add(new Menu("Addresses", "/address"));
                }

                result.Add(crm);
            }

            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewRFP)
                || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddRFP)
                || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteRFP))
            {
                result.Add(new Menu("PROPOSALS", "/rfp", "<span class=\"material-symbols-outlined\">adjust</span>"));
            }

            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewJob)
                || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddJob)
                || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteJob))
            {
                result.Add(new Menu("PROJECTS", "/jobs", "<span class=\"material-symbols-outlined filled\">assignment</span>"));
            }

            //if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewReport))
            //{
            var reportsMenu = new Menu("REPORTS", null, "<span class=\"material-symbols-outlined filled\">insert_chart</span>");
            reportsMenu.Items.Add(new Menu("Violation Reports", "/allviolationreport"));
            reportsMenu.Items.Add(new Menu("Permit Reports", "/permitsexpiryreport"));
            reportsMenu.Items.Add(new Menu("AHV Permit Reports", "/ahvpermitsexpiryreport"));
            reportsMenu.Items.Add(new Menu("Application Status Reports", "/applicationstatusreport"));
            reportsMenu.Items.Add(new Menu("Contractor Insurance Reports", "/contractorinsurancesexpiryreport"));
            reportsMenu.Items.Add(new Menu("Contact License Reports", "/contactlicenseexpiryreport"));
            reportsMenu.Items.Add(new Menu("Consolidated Status Reports", "/consolidatedstatusreport"));
            reportsMenu.Items.Add(new Menu("Proposal Reports(Open or Not Sent)", " /rfpreport"));
            reportsMenu.Items.Add(new Menu("Unsynced Timenote Reports", "/unsynctimenotereport"));
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewComplateScopeReport))
            {
                reportsMenu.Items.Add(new Menu("Completed Scope / Billing Points but not Invoiced", "/completedscopebillingpointreport"));
            }
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewClosedScopeReport))
            {
                reportsMenu.Items.Add(new Menu("Closed Projects with Open Billing Points", "/closedjobswithopenbillingreport"));
            }
            result.Add(reportsMenu);
            //}

            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewReferenceLinks)
                || Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewReferenceDocument))
            {
                var referencesMenu = new Menu("RESOURCES", null, "<span class=\"material-symbols-outlined filled\">chat_bubble</span>")
                {

                };
                if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewReferenceLinks))
                {
                    referencesMenu.Items.AddRange(rpoContext.ReferenceLinks.AsEnumerable().Select(rl => new Menu(rl.Name, Regex.Replace(rl.Name, @"\s+", ""))
                    {
                        Url = rl.Url
                    }));
                }

                if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewReferenceDocument))
                {
                    referencesMenu.Items.Add(new Menu("Documents", "/documents"));
                }
                result.Add(referencesMenu);
            }

            result.Add(new Menu("TASKS", "/tasks", "<span class=\"material-symbols-outlined filled\">event_available</span>"));

            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewEmployee)
              || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployee)
              || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteEmployee)
              || Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewEmployeeUserGroup)
              || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployeeUserGroup)
              || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteEmployeeUserGroup)
              || Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
              || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
              || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData)
                || Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                )
            {
                var administration = new Menu("ADMINISTRATION", null, "<span class=\"material-symbols-outlined filled\">settings</span>");

                if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewEmployee)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployee)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteEmployee))
                {
                    administration.Items.Add(new Menu("Employees", "/employee"));
                }
                if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewCustomer)
                 || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddCustomer)
                 || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteCustomer))
                {
                   // administration.Items.Add(new Menu("Customers", "/customer-permissions"));
                    administration.Items.Add(new Menu("Manage Portal Contacts", "/customer-permissions"));
                }
                if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewEmployeeUserGroup)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddEmployeeUserGroup)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteEmployeeUserGroup))
                {
                    administration.Items.Add(new Menu("User Groups", "/userGroup"));
                }

                if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewFeeScheduleMaster)
                  )
                {
                    var rfpMasters = new Menu("Proposal Masters", "/rfpmasters");

                    rfpMasters.Items = new List<Menu>();
                    rfpMasters.Items.Add(new Menu("Project Types", "/jobtype"));
                    rfpMasters.Items.Add(new Menu("Project Type Descriptions", "/subjobtypecategory"));
                    rfpMasters.Items.Add(new Menu("Project Sub-Types", "/subjobtype"));
                    rfpMasters.Items.Add(new Menu("Service Groups", "/worktypecategory"));
                    rfpMasters.Items.Add(new Menu("Service Items", "/worktype"));
                    rfpMasters.Items.Add(new Menu("Verbiages", "/verbiage"));

                    administration.Items.Add(rfpMasters);

                    var PaneltyCodeMastersMenu = new Menu("Penalty Schedule Masters", "/penaltymasters");
                    PaneltyCodeMastersMenu.Items = new List<Menu>();
                    PaneltyCodeMastersMenu.Items.Add(new Menu("DOB Penalty Schedule", "/dobpenaltyschedule"));
                    PaneltyCodeMastersMenu.Items.Add(new Menu("FDNY Penalty Schedule", "/fdnypenaltyschedule"));
                    PaneltyCodeMastersMenu.Items.Add(new Menu("DOT Penalty Schedule", "/dotpenaltyschedule"));
                    PaneltyCodeMastersMenu.Items.Add(new Menu("DOHMH Cooling Tower Penalty Schedule", "/dohmhcoolingtowerpenaltyschedule"));
                    PaneltyCodeMastersMenu.Items.Add(new Menu("DEP Noise Code Penalty Schedule", "/depnoisecodepenaltyschedule"));
                    administration.Items.Add(PaneltyCodeMastersMenu);

                    if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewMasterData)
                             || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddMasterData)
                                || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteMasterData)
                        )
                    {
                        var AddressMastersMenu = new Menu("Address Masters", "/masters");
                        AddressMastersMenu.Items = new List<Menu>();
                        AddressMastersMenu.Items.Add(new Menu("Owner Types", "/ownertype"));
                        AddressMastersMenu.Items.Add(new Menu("Multiple Dwelling Classifications", "/multipledwellingclassification"));
                        AddressMastersMenu.Items.Add(new Menu("Occupancy Classifications", "/occupancyclassification"));
                        AddressMastersMenu.Items.Add(new Menu("Seismic Design Categories", "/seismicdesigncategory"));
                        AddressMastersMenu.Items.Add(new Menu("Structure Occupancy Categories", "/structureoccupancycategory"));
                        AddressMastersMenu.Items.Add(new Menu("Construction Classifications", "/constructionclassification"));
                        AddressMastersMenu.Items.Add(new Menu("Primary Structural Systems", "/primarystructuralsystem"));
                        administration.Items.Add(AddressMastersMenu);

                        var ChecklistMastersMenu = new Menu("Checklist Masters", "/checklistmasters");
                        ChecklistMastersMenu.Items = new List<Menu>();
                        ChecklistMastersMenu.Items.Add(new Menu("Checklist Groups", "/checklistGroupMaster"));
                        ChecklistMastersMenu.Items.Add(new Menu("Checklist Items", "/checklistItemMaster"));
                        administration.Items.Add(ChecklistMastersMenu);

                        var otherMastersMenu = new Menu("Other Masters", "/othermasters");
                        otherMastersMenu.Items = new List<Menu>();
                        otherMastersMenu.Items.Add(new Menu("Notification Settings", "/systemsetting"));
                        otherMastersMenu.Items.Add(new Menu("Address Types", "/addresstype"));
                        otherMastersMenu.Items.Add(new Menu("Contact Titles", "/contacttitle"));
                        otherMastersMenu.Items.Add(new Menu("Transmittal Types", "/emailtype"));
                        otherMastersMenu.Items.Add(new Menu("Sent Via", "/sentvia"));
                        otherMastersMenu.Items.Add(new Menu("Company Types", "/companytype"));
                        otherMastersMenu.Items.Add(new Menu("Contact License Types", "/licensetype"));
                        otherMastersMenu.Items.Add(new Menu("Company License Types", "/companylicensetype"));
                        otherMastersMenu.Items.Add(new Menu("Certificate Types", "/documenttype"));
                        otherMastersMenu.Items.Add(new Menu("Project Contact Types", "/jobcontacttype"));
                        //otherMastersMenu.Items.Add(new Menu("Job Time Note Category", "/jobtimenotecategory"));
                        otherMastersMenu.Items.Add(new Menu("Task Types", "/tasktype"));
                        //otherMastersMenu.Items.Add(new Menu("Penalty code", "/paneltycode"));
                        otherMastersMenu.Items.Add(new Menu("Suffix", "/suffix"));
                        otherMastersMenu.Items.Add(new Menu("Prefix", "/prefix"));
                        otherMastersMenu.Items.Add(new Menu("Holiday Calender", "/holidaycalender"));
                        otherMastersMenu.Items.Add(new Menu("DEP Cost Settings", "/depcostsetting"));
                        otherMastersMenu.Items.Add(new Menu("Project Application Types", "/jobapplicationtype"));
                        otherMastersMenu.Items.Add(new Menu("Work Permit Types", "/workpermittype"));
                        otherMastersMenu.Items.Add(new Menu("Document Masters", "/documentmaster"));
                        administration.Items.Add(otherMastersMenu);
                    }

                }
                result.Add(administration);
            }
            return result;
        }
    }
}
