// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="EmployeesController.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>API for the employee </summary>
// ***********************************************************************

/// <summary>
/// The Employees namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Employees
{

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Permissions;
    using Rpo.ApiServices.Api.DataTable;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Api.Tools;
    using Rpo.ApiServices.Model;
    using Rpo.ApiServices.Model.Models;
    using Rpo.ApiServices.Model.Models.Enums;
    using Rpo.Identity.Core.Managers;
    using Rpo.Identity.Core.Models;

    /// <summary>
    /// Class EmployeesController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />

    public class EmployeesController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();

        /// <summary>
        /// Class Employee Advanced Search Parameters.
        /// </summary>
        /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
        public class EmployeeAdvancedSearchParameters : DataTableParameters
        {
            /// <summary>
            /// Gets or sets a value indicating whether this instance is active.
            /// </summary>
            /// <value><c>null</c> if [is active] contains no value, <c>true</c> if [is active]; otherwise, <c>false</c>.</value>
            public bool? IsActive { get; set; }
        }

        /// <summary>
        /// Gets the employees list.
        /// </summary>
        /// <remarks>List of all employees</remarks>
        /// <param name="dataTableParameters">The data table parameters.</param>
        /// <returns>The employees.</returns>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        public IHttpActionResult GetEmployees([FromUri] EmployeeAdvancedSearchParameters dataTableParameters)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.ViewEmployee)
                  || Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.AddEmployee)
                  || Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.DeleteEmployee))
            {
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                using (RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(rpoContext)))
                {
                    IQueryable<Employee> employees = rpoContext.Employees.Where(x => x.IsArchive == false);

                    var recordsTotal = employees.Count();
                    var recordsFiltered = recordsTotal;

                    if (dataTableParameters.IsActive != null)
                    {
                        bool isActive = Convert.ToBoolean(dataTableParameters.IsActive);
                        employees = employees.Where(r => r.IsActive == isActive);
                    }

                    EmployeeDTO[] result = employees
                        .AsEnumerable()
                        .Select(employee => new EmployeeDTO
                        {
                            Address1 = employee.Address1,
                            Address2 = employee.Address2,
                            Id = employee.Id,
                            City = employee.City,
                            State = employee.IdState.HasValue ? employee.State.Acronym : "",
                            IdState = employee.IdState.HasValue ? (int?)employee.State.Id : null,
                            ZipCode = employee.ZipCode,
                            ComputerPassword = employee.ComputerPassword,
                            Dob = employee.Dob,
                            EfillingPassword = employee.EfillingPassword,
                            EfillingUserName = employee.EfillingUserName,
                            Email = employee.Email,
                            FinalDate = employee.FinalDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(employee.FinalDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.FinalDate,
                            FirstName = employee.FirstName,
                            Group = employee.Group.Name,
                            IdGroup = employee.IdGroup,
                            LastName = employee.LastName,
                            MobilePhone = employee.MobilePhone,
                            HomePhone = employee.HomePhone,
                            Notes = employee.Notes,
                            Ssn = employee.Ssn,
                            StartDate = employee.StartDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(employee.StartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.StartDate,
                            TelephonePassword = employee.TelephonePassword,
                            WorkPhone = employee.WorkPhone,
                            WorkPhoneExt = employee.WorkPhoneExt,
                            IsActive = employee.IsActive,
                            IsArchive = employee.IsArchive,
                            QBEmployeeName = employee.QBEmployeeName,
                            CreatedBy = employee.CreatedBy,
                            LastModifiedBy = employee.LastModifiedBy != null ? employee.LastModifiedBy : employee.CreatedBy,
                            CreatedByEmployeeName = employee.CreatedByEmployee != null ? employee.CreatedByEmployee.FirstName + " " + employee.CreatedByEmployee.LastName : string.Empty,
                            LastModifiedByEmployeeName = employee.LastModifiedByEmployee != null ? employee.LastModifiedByEmployee.FirstName + " " + employee.LastModifiedByEmployee.LastName : (employee.CreatedByEmployee != null ? employee.CreatedByEmployee.FirstName + " " + employee.CreatedByEmployee.LastName : string.Empty),
                            CreatedDate = employee.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(employee.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.CreatedDate,
                            LastModifiedDate = employee.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(employee.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (employee.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(employee.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.CreatedDate),
                        })
                        .AsQueryable()
                        .DataTableParameters(dataTableParameters, out recordsFiltered)
                        .ToArray();

                    return Ok(new DataTableResponse
                    {
                        Draw = dataTableParameters.Draw,
                        RecordsFiltered = recordsFiltered,
                        RecordsTotal = recordsTotal,
                        Data = result
                    });
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the employee by Id.
        /// </summary>
        /// <remarks> Employee Details for Requested Id </remarks> 
        /// <param name="id">The identifier.</param>
        /// <returns> Gets the employee detail by Id.</returns>
        [ResponseType(typeof(EmployeeDTO))]
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/employees/{id:int}")]
        public IHttpActionResult GetEmployee(int id)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.ViewEmployee)
                  || Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.AddEmployee)
                  || Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.DeleteEmployee))
            {
                Employee employee = rpoContext.Employees.Find(id);
                if (employee == null)
                {
                    return this.NotFound();
                }
                List<int> employeePermissions = employee.Permissions != null && !string.IsNullOrEmpty(employee.Permissions) ? (employee.Permissions.Split(',') != null && employee.Permissions.Split(',').Any() ? employee.Permissions.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                return Ok(FormatDetails(employee, employeePermissions));
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the permission.
        /// </summary>
        /// <remarks> Employee permission List </remarks> 
        /// <param name="employeePermissions">The employee permissions.</param>
        /// <returns>List&lt;PermissionModuleDTO&gt;.Get the permission list</returns>
        private List<PermissionModuleDTO> GetPermission(List<int> employeePermissions)
        {
            var permissions = rpoContext.Permissions.Include("CreatedByEmployee").Include("LastModifiedByEmployee").AsQueryable();

            List<PermissionModuleDTO> permissionModuleList = new List<PermissionModuleDTO>();
            var moduleList = permissions.Select(x => x.ModuleName).Distinct();
            PermissionsController permissionsController = new PermissionsController();

            foreach (var module in moduleList)
            {
                PermissionModuleDTO permissionModule = new PermissionModuleDTO();
                List<PermissionGroupDTO> permissionGroupList = new List<PermissionGroupDTO>();
                permissionModule.ModuleName = module;

                var groupList = permissions.Where(x => x.ModuleName == module).Select(x => new { x.GroupName, x.DisplayOrder }).Distinct().OrderBy(x => x.DisplayOrder);
                foreach (var group in groupList)
                {
                    PermissionGroupDTO permissionGroup = new PermissionGroupDTO();
                    List<PermissionsDTO> permissionDTOList = new List<PermissionsDTO>();
                    permissionGroup.GroupName = group.GroupName;
                    var permissionList = permissions.Where(x => x.GroupName == group.GroupName).ToList();
                    foreach (var item in permissionList)
                    {
                        PermissionsDTO permissionsDTO = permissionsController.Format(item);
                        if (employeePermissions.Contains(item.Id))
                        {
                            permissionsDTO.IsChecked = true;
                        }
                        else
                        {
                            permissionsDTO.IsChecked = false;
                        }
                        permissionDTOList.Add(permissionsDTO);
                    }

                    permissionGroup.Permissions = permissionDTOList;
                    permissionGroupList.Add(permissionGroup);
                }

                permissionModule.Groups = permissionGroupList;
                permissionModuleList.Add(permissionModule);
            }

            return permissionModuleList;
        }

        /// <summary>
        /// Gets the employee dropdown.
        /// </summary>
        /// <remarks> Active Employee List Bind a Dropdown </remarks> 
        /// <returns>Get the list of active employees</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(IEnumerable<EmployeeDetail>))]
        [Route("api/employees/dropdown")]
        public IHttpActionResult GetEmployeeDropdown()
        {
            IEnumerable<EmployeeDetail> result = rpoContext.Employees.Where(x => x.IsArchive == false && x.IsActive)
                .AsEnumerable()
                .Select(c => new EmployeeDetail()
                {
                    Id = c.Id,
                    EmployeeName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty),
                    ItemName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                    Email = c.Email,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    IdEmployee = c.Id,
                })
                .ToArray()
                .Select(c =>
                {
                    return c;
                });
            return Ok(result);
        }
        /// <summary>
        /// Gets the All employee dropdown.
        /// </summary>
        /// <remarks>All Employee List </remarks> 
        /// <returns>Get the list of all employees</returns>
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(IEnumerable<EmployeeDetail>))]
        [Route("api/employees/AllEmployeedropdown")]
        public IHttpActionResult GetAllEmployeeDropdown()
        {
            IEnumerable<EmployeeDetail> result = rpoContext.Employees.Where(x => x.IsArchive == false)
                .AsEnumerable()
                .Select(c => new EmployeeDetail()
                {
                    Id = c.Id,
                    EmployeeName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty),
                    ItemName = c.FirstName + (!string.IsNullOrWhiteSpace(c.LastName) ? " " + c.LastName : string.Empty) + (!string.IsNullOrWhiteSpace(c.Email) ? " (" + c.Email + ")" : string.Empty),
                    Email = c.Email,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    IdEmployee = c.Id,
                })
                .ToArray()
                .Select(c =>
                {
                    return c;
                });
            return Ok(result);
        }

        /// <summary>
        /// Puts the employee.
        /// </summary>
        /// <remark> Update employee using put method</remark>
        /// <param name="id">The identifier.</param>
        /// <param name="employeeDTO">The employee dto.</param>
        /// <returns>Update employee details by employee Id </returns>
        /// <exception cref="RpoBusinessException">
        /// </exception>
        [ResponseType(typeof(void))]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/employees/{id:int}")]
        public IHttpActionResult PutEmployee(int id, [FromBody]EmployeeDTO employeeDTO)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.AddEmployee))
            {
                string employeePermissions = rpoContext.Employees.Where(x => x.Id == employeeDTO.Id).Select(x => x.Permissions).FirstOrDefault();
                var employee = rpoContext.Employees.Find(id); //
               // var employee = employeeDTO.CloneAs<Employee>();

                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);


                using (DbContextTransaction transaction = rpoContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }

                        if (id != employee.Id)
                        {
                            return BadRequest();
                        }

                        if (!employeeDTO.IsActive)
                        {
                            string employeeId = Convert.ToString(id);

                            if (rpoContext.Jobs.Where(x => (x.IdProjectManager == id ||
                               (x.DOBProjectTeam == employeeId || x.DOBProjectTeam.StartsWith(employeeId + ",") || x.DOBProjectTeam.Contains("," + employeeId + ",") || x.DOBProjectTeam.EndsWith("," + employeeId))
                               || (x.DOTProjectTeam == employeeId || x.DOTProjectTeam.StartsWith(employeeId + ",") || x.DOTProjectTeam.Contains("," + employeeId + ",") || x.DOTProjectTeam.EndsWith("," + employeeId))
                               || (x.DEPProjectTeam == employeeId || x.DEPProjectTeam.StartsWith(employeeId + ",") || x.DEPProjectTeam.Contains("," + employeeId + ",") || x.DEPProjectTeam.EndsWith("," + employeeId))
                               || (x.ViolationProjectTeam == employeeId || x.ViolationProjectTeam.StartsWith(employeeId + ",") || x.ViolationProjectTeam.Contains("," + employeeId + ",") || x.ViolationProjectTeam.EndsWith("," + employeeId))
                                    ) && (x.Status != JobStatus.Close)).Any())
                            {
                                throw new RpoBusinessException(string.Format(StaticMessages.EmployeeInactiveValidationMessage, employee.FirstName + " " + employee.LastName));
                            }

                            if (rpoContext.Tasks.Where(x => x.IdAssignedTo == id && (x.IdTaskStatus != (int)EnumTaskStatus.Completed || x.IdTaskStatus != (int)EnumTaskStatus.Unattainable)).Any())
                            {
                                throw new RpoBusinessException(string.Format(StaticMessages.EmployeeInactiveValidationMessage, employee.FirstName + " " + employee.LastName));
                            }
                        }

                        employee.StartDate = employee.StartDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(employee.StartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.StartDate;
                        employee.FinalDate = employee.FinalDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(employee.FinalDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.FinalDate;
                        employee.Dob = employee.Dob != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(employee.Dob), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.Dob;
                        employee.FirstName = employeeDTO.FirstName != null ? employeeDTO.FirstName : string.Empty;
                        employee.LastName = employeeDTO.LastName != null ? employeeDTO.LastName : string.Empty;
                        employee.MobilePhone = employeeDTO.MobilePhone != null ? employeeDTO.MobilePhone : string.Empty;
                        employee.Address1 = employeeDTO.Address1 != null ? employeeDTO.Address1 : string.Empty;
                        employee.Address2 = employeeDTO.Address2 != null ? employeeDTO.Address2 : string.Empty;
                        employee.AllergyDescription = employeeDTO.AllergyDescription != null ? employeeDTO.AllergyDescription : string.Empty;
                        employee.AppleId = employeeDTO.AppleId != null ? employeeDTO.AppleId : string.Empty;
                        employee.ApplePassword = employeeDTO.ApplePassword != null ? employeeDTO.ApplePassword : string.Empty;
                        employee.City = employeeDTO.City != null ? employeeDTO.City : string.Empty;
                        employee.ComputerPassword = employeeDTO.ComputerPassword != null ? employeeDTO.ComputerPassword : string.Empty;
                        employee.Dob = employeeDTO.Dob != null ? employeeDTO.Dob : null;
                        employee.EfillingPassword = employeeDTO.EfillingPassword != null ? employeeDTO.EfillingPassword : string.Empty;
                        employee.EfillingUserName = employeeDTO.EfillingUserName != null ? employeeDTO.EfillingUserName : string.Empty;
                        employee.Email = employeeDTO.Email != null ? employeeDTO.Email : string.Empty;
                        employee.EmergencyContactName = employeeDTO.EmergencyContactName != null ? employeeDTO.EmergencyContactName : string.Empty;
                        employee.EmergencyContactNumber = employeeDTO.EmergencyContactNumber != null ? employeeDTO.EmergencyContactNumber : string.Empty;
                        employee.HomePhone = employeeDTO.HomePhone != null ? employeeDTO.HomePhone : string.Empty;
                        employee.IdGroup = employeeDTO.IdGroup != 0 ? employeeDTO.IdGroup : 0;
                        employee.IdState = employeeDTO.IdState != null ? employeeDTO.IdState : null;
                        employee.IsActive = employeeDTO.IsActive != false ? employeeDTO.IsActive : false;
                        employee.IsArchive = employeeDTO.IsArchive != false ? employeeDTO.IsArchive : false;
                        employee.LockScreenPassword = employeeDTO.LockScreenPassword != null ? employeeDTO.LockScreenPassword : string.Empty;
                        employee.LoginPassword = employeeDTO.LoginPassword != null ? employeeDTO.LoginPassword : employee.LoginPassword;
                        employee.MobilePhone = employeeDTO.MobilePhone != null ? employeeDTO.MobilePhone : string.Empty;
                        employee.Notes = employeeDTO.Notes != null ? employeeDTO.Notes : string.Empty;
                        employee.QBEmployeeName = employeeDTO.QBEmployeeName != null ? employeeDTO.QBEmployeeName : string.Empty;
                        employee.Ssn = employeeDTO.Ssn != null ? employeeDTO.Ssn : string.Empty;
                        employee.StartDate = employeeDTO.StartDate != null ? employeeDTO.StartDate : null;
                        employee.TelephonePassword = employeeDTO.TelephonePassword != null ? employeeDTO.TelephonePassword : string.Empty;
                        employee.WorkPhone = employeeDTO.WorkPhone != null ? employeeDTO.WorkPhone : string.Empty;
                        employee.WorkPhoneExt = employeeDTO.WorkPhoneExt != null ? employeeDTO.WorkPhoneExt : string.Empty;
                        employee.ZipCode = employeeDTO.ZipCode != null ? employeeDTO.ZipCode : string.Empty;
                        employee.FinalDate = employeeDTO.FinalDate != null ? employeeDTO.FinalDate : null;

                        employee.LastModifiedBy = loginEmployee.Id;
                        employee.LastModifiedDate = DateTime.UtcNow;
                        employee.Permissions = employeePermissions;

                        if (employeeDTO.AgentCertificates != null)
                        {
                            foreach (var certificate in employeeDTO.AgentCertificates)
                            {
                                certificate.DocumentType = null;
                                certificate.IdEmployee = employee.Id;
                            }
                        }

                        if (employeeDTO.AgentCertificates != null)
                        {
                            var ids = employeeDTO.AgentCertificates.Select(ac => ac.Id);

                            rpoContext.AgentCertificates.RemoveRange(rpoContext.AgentCertificates.Where(ac => ac.IdEmployee == employee.Id && !ids.Any(eacIds => eacIds == ac.Id)));
                            //rpoContext.Entry(employee).State = EntityState.Modified;

                            foreach (var certificate in employeeDTO.AgentCertificates)
                            {
                                certificate.ExpirationDate = certificate.ExpirationDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(certificate.ExpirationDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : certificate.ExpirationDate;
                                rpoContext.Entry(certificate).State = certificate.Id == 0 ? EntityState.Added : EntityState.Modified;
                            }
                        }

                        if (employeeDTO.DocumentsToDelete != null)
                        {
                            List<int> deletedDocs = employeeDTO.DocumentsToDelete.ToList();
                            rpoContext.EmployeeDocuments.RemoveRange(rpoContext.EmployeeDocuments.Where(ac => ac.IdEmployee == employee.Id && deletedDocs.Any(eacIds => eacIds == ac.Id)));
                        }


                        if (employeeDTO.DocumentsToDelete != null)
                        {
                            foreach (var item in employeeDTO.DocumentsToDelete)
                            {
                                int employeeDocumentId = Convert.ToInt32(item);
                                EmployeeDocument employeeDocument = rpoContext.EmployeeDocuments.Where(x => x.Id == employeeDocumentId).FirstOrDefault();
                                if (employeeDocument != null)
                                {
                                    rpoContext.EmployeeDocuments.Remove(employeeDocument);
                                    var path = HttpRuntime.AppDomainAppPath;
                                    string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.EmployeeDocumentPath));
                                    string directoryDelete = Convert.ToString(employeeDocument.Id) + "_" + employeeDocument.DocumentPath;
                                    string deletefilename = System.IO.Path.Combine(directoryName, directoryDelete);

                                    if (File.Exists(deletefilename))
                                    {
                                        File.Delete(deletefilename);
                                    }
                                }
                            }
                        }

                        if( rpoContext.Entry(employee).State != EntityState.Detached) { 
                                rpoContext.Entry(employee).State = EntityState.Modified;
                        }

                        using (RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(rpoContext)))
                        {
                            RpoIdentityUser systemUser = userManager.FindByEmail(employee.Email);
                            if (employeeDTO.IsActive)
                            {
                                if (systemUser != null)
                                {
                                    systemUser.UserName = employee.Email;
                                    systemUser.LockoutEnabled = employeeDTO.IsActive;
                                    userManager.Update(systemUser);

                                    if (!string.IsNullOrWhiteSpace(employeeDTO.LoginPassword))
                                    {
                                        userManager.RemovePassword(systemUser.Id);
                                        var result = userManager.AddPassword(systemUser.Id, employeeDTO.LoginPassword);
                                        if (!result.Succeeded)
                                        {
                                            throw new RpoBusinessException(string.Join("!" + Environment.NewLine, result.Errors));
                                        }
                                    }
                                }
                                else
                                {
                                    systemUser = new RpoIdentityUser
                                    {
                                        UserName = employee.Email,
                                        Email = employee.Email,
                                        EmailConfirmed = true,
                                        LockoutEnabled = employeeDTO.IsActive
                                    };
                                    var result = userManager.Create(systemUser, employeeDTO.LoginPassword);
                                    if (!result.Succeeded)
                                    {
                                        throw new RpoBusinessException(string.Join("!" + Environment.NewLine, result.Errors));
                                    }
                                }
                            }
                            else
                            {
                                if (systemUser != null)
                                {
                                    systemUser.UserName = employee.Email;
                                    systemUser.LockoutEnabled = employeeDTO.IsActive;
                                    userManager.Update(systemUser);
                                }
                            }
                        }
                        try
                        {
                            rpoContext.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!EmployeeExists(id))
                            {
                                return this.NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }

                        transaction.Commit();
                        return StatusCode(HttpStatusCode.NoContent);
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Posts the employee. -- POST: api/Employees
        /// </summary>
        /// <remark> Create new employee using post method</remark>
        /// <param name="employee">The employee.</param>
        /// <returns>create a new employee</returns>
        /// <exception cref="RpoBusinessException">
        /// The current user does not have a password. Please enter a password!
        /// or
        /// </exception>
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(Employee))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.AddEmployee))
            {
                string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

                using (DbContextTransaction transaction = rpoContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }

                        if (rpoContext.Employees.Any(e => e.Email == employee.Email))
                        {
                            throw new RpoBusinessException($"Email {employee.Email} is already taken.");
                        }

                        employee.IsArchive = false;
                        employee.StartDate = employee.StartDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(employee.StartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.StartDate;
                        employee.FinalDate = employee.FinalDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(employee.FinalDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.FinalDate;
                        employee.Dob = employee.Dob != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(employee.Dob), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.Dob;

                        employee.CreatedBy = loginEmployee.Id;
                        employee.LastModifiedBy = loginEmployee.Id;

                        employee.CreatedDate = DateTime.UtcNow;
                        employee.LastModifiedDate = DateTime.UtcNow;

                        employee = rpoContext.Employees.Add(employee);

                        foreach (var certificate in employee.AgentCertificates)
                        {
                            certificate.ExpirationDate = certificate.ExpirationDate != null ? TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(certificate.ExpirationDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : certificate.ExpirationDate;
                        }

                        if (string.IsNullOrWhiteSpace(employee.LoginPassword))
                        {
                            throw new RpoBusinessException("The current user does not have a password. Please enter a password!");
                        }

                        using (RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(rpoContext)))
                        {
                            RpoIdentityUser systemUser = new RpoIdentityUser
                            {
                                UserName = employee.Email,
                                Email = employee.Email,
                                EmailConfirmed = true,
                                LockoutEnabled = employee.IsActive
                            };
                            var result = userManager.Create(systemUser, employee.LoginPassword);

                            if (!result.Succeeded)
                            {
                                throw new RpoBusinessException(string.Join("!" + Environment.NewLine, result.Errors));
                            }
                        }

                        var group = rpoContext.Groups.FirstOrDefault(g => g.Id == employee.IdGroup);
                        if (group != null)
                        {
                            employee.Permissions = group.Permissions;
                        }

                        rpoContext.SaveChanges();

                        transaction.Commit();
                        return this.CreatedAtRoute("DefaultApi", new { controller = "employees", id = employee.Id }, employee);
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Deletes the employee.
        /// </summary>
        /// <remarks>Delete employee by employee id</remarks>
        /// <param name="id">The identifier.</param>
        /// <returns>delete the employee in database.</returns>
        [ResponseType(typeof(Employee))]
        [HttpDelete]
        [Authorize]
        [RpoAuthorize]
        [Route("api/employees/{id:int}")]
        public IHttpActionResult DeleteEmployee(int id)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.DeleteEmployee))
            {
                Employee employee = rpoContext.Employees.Find(id);
                if (employee == null)
                {
                    return this.NotFound();
                }

                string employeeId = Convert.ToString(id);
                if (rpoContext.Jobs.Where(x => (x.IdProjectManager == id ||
                (x.DOBProjectTeam == employeeId || x.DOBProjectTeam.StartsWith(employeeId + ",") || x.DOBProjectTeam.Contains("," + employeeId + ",") || x.DOBProjectTeam.EndsWith("," + employeeId))
                               || (x.DOTProjectTeam == employeeId || x.DOTProjectTeam.StartsWith(employeeId + ",") || x.DOTProjectTeam.Contains("," + employeeId + ",") || x.DOTProjectTeam.EndsWith("," + employeeId))
                               || (x.DEPProjectTeam == employeeId || x.DEPProjectTeam.StartsWith(employeeId + ",") || x.DEPProjectTeam.Contains("," + employeeId + ",") || x.DEPProjectTeam.EndsWith("," + employeeId))
                               || (x.ViolationProjectTeam == employeeId || x.ViolationProjectTeam.StartsWith(employeeId + ",") || x.ViolationProjectTeam.Contains("," + employeeId + ",") || x.ViolationProjectTeam.EndsWith("," + employeeId))
                               ) && (x.Status != JobStatus.Close)).Any())
                {
                    throw new RpoBusinessException(string.Format(StaticMessages.EmployeeDeleteValidationMessage, employee.FirstName + " " + employee.LastName));
                }

                if (rpoContext.Tasks.Where(x => x.IdAssignedTo == id && (x.IdTaskStatus != (int)EnumTaskStatus.Completed || x.IdTaskStatus != (int)EnumTaskStatus.Unattainable)).Any())
                {
                    throw new RpoBusinessException(string.Format(StaticMessages.EmployeeDeleteValidationMessage, employee.FirstName + " " + employee.LastName));
                }

                employee.IsArchive = true;
                rpoContext.SaveChanges();

                return Ok(employee);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
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
        /// Employees the exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool EmployeeExists(int id)
        {
            return rpoContext.Employees.Any(e => e.Id == id);
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <remarks>Change password of  employee by employee id</remarks>
        /// <param name="id">The identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>Change password updated of employee</returns>
        /// <exception cref="RpoBusinessException"></exception>
        [ResponseType(typeof(void))]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/employees/{id:int}/password/{password}")]
        public IHttpActionResult ChangePassword(int id, string password)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.AddEmployee))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Employee employee = rpoContext.Employees.Find(id);
                if (employee == null)
                {
                    return this.NotFound();
                }

                using (RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(rpoContext)))
                {
                    RpoIdentityUser systemUser = userManager.FindByEmail(employee.Email);

                    if (systemUser != null)
                    {
                        systemUser.UserName = employee.Email;
                        userManager.Update(systemUser);

                        if (!string.IsNullOrWhiteSpace(password))
                        {
                            userManager.RemovePassword(systemUser.Id);
                            var result = userManager.AddPassword(systemUser.Id, password);
                            if (!result.Succeeded)
                            {
                                throw new RpoBusinessException(string.Join("!" + Environment.NewLine, result.Errors));
                            }
                        }
                    }
                }
                employee.LoginPassword = password;

                employee.LastModifiedBy = loginEmployee.Id;
                employee.LastModifiedDate = DateTime.UtcNow;

                rpoContext.SaveChanges();

                return Ok(employee);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Put Sets the active.
        /// </summary>
        /// <remarks>Employee update the status active</remarks>
        /// <param name="id">The identifier.</param>
        /// <returns>update the employee status active.</returns>
        [ResponseType(typeof(void))]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/employees/{id:int}/active")]
        public IHttpActionResult SetActive(int id)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.EditStatus))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Employee employee = rpoContext.Employees.Find(id);
                if (employee == null)
                {
                    return this.NotFound();
                }

                using (RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(rpoContext)))
                {
                    RpoIdentityUser systemUser = userManager.FindByEmail(employee.Email);
                    if (systemUser != null)
                    {
                        systemUser.UserName = employee.Email;
                        systemUser.LockoutEnabled = true;
                        userManager.Update(systemUser);
                    }
                }

                employee.IsActive = true;
                employee.LastModifiedBy = loginEmployee.Id;
                employee.LastModifiedDate = DateTime.UtcNow;
                rpoContext.SaveChanges();

                return Ok(employee);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// put Sets the inactive.
        /// </summary>
        /// <remarks>Employee update the status inactive</remarks>
        /// <param name="id">The identifier.</param>
        /// <returns>update the employee status inactive.</returns>
        [ResponseType(typeof(void))]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/employees/{id:int}/inactive")]
        public IHttpActionResult SetInactive(int id)
         {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.EditStatus))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Employee employee = rpoContext.Employees.Find(id);
                if (employee == null)
                {
                    return this.NotFound();
                }

                string employeeId = Convert.ToString(id);

                if (rpoContext.Jobs.Where(x => (x.IdProjectManager == id ||
                (x.DOBProjectTeam == employeeId || x.DOBProjectTeam.StartsWith(employeeId + ",") || x.DOBProjectTeam.Contains("," + employeeId + ",") || x.DOBProjectTeam.EndsWith("," + employeeId))
                               || (x.DOTProjectTeam == employeeId || x.DOTProjectTeam.StartsWith(employeeId + ",") || x.DOTProjectTeam.Contains("," + employeeId + ",") || x.DOTProjectTeam.EndsWith("," + employeeId))
                               || (x.DEPProjectTeam == employeeId || x.DEPProjectTeam.StartsWith(employeeId + ",") || x.DEPProjectTeam.Contains("," + employeeId + ",") || x.DEPProjectTeam.EndsWith("," + employeeId))
                               || (x.ViolationProjectTeam == employeeId || x.ViolationProjectTeam.StartsWith(employeeId + ",") || x.ViolationProjectTeam.Contains("," + employeeId + ",") || x.ViolationProjectTeam.EndsWith("," + employeeId))
                               ) && (x.Status != JobStatus.Close)).Any())
                {
                    throw new RpoBusinessException(string.Format(StaticMessages.EmployeeInactiveValidationMessage, employee.FirstName + " " + employee.LastName));
                }

                if (rpoContext.Tasks.Where(x => x.IdAssignedTo == id && (x.IdTaskStatus == (int)EnumTaskStatus.Pending)).Any())
                {
                    throw new RpoBusinessException(string.Format(StaticMessages.EmployeeInactiveValidationMessage, employee.FirstName + " " + employee.LastName));
                }

                using (RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(rpoContext)))
                {
                    RpoIdentityUser systemUser = userManager.FindByEmail(employee.Email);

                    if (systemUser != null)
                    {
                        systemUser.UserName = employee.Email;
                        systemUser.LockoutEnabled = false;
                        userManager.Update(systemUser);
                    }
                }

                employee.IsActive = false;
                employee.LastModifiedBy = loginEmployee.Id;
                employee.LastModifiedDate = DateTime.UtcNow;

                rpoContext.SaveChanges();

                return Ok(employee);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Gets the grants.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>update the employee grant.</returns>
        [ResponseType(typeof(PermissionsDTO))]
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/employees/{id:int}/grants")]
        public IHttpActionResult GetGrants(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Employee employee = rpoContext.Employees.Find(id);
            if (employee == null)
            {
                return this.NotFound();
            }

            List<int> employeePermissions = employee.Permissions != null && !string.IsNullOrEmpty(employee.Permissions) ? (employee.Permissions.Split(',') != null && employee.Permissions.Split(',').Any() ? employee.Permissions.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

            return Ok(rpoContext.Permissions.Where(x => employeePermissions.Contains(x.Id)).Select(x => new PermissionsDTO
            {
                Id = x.Id,
                Name = x.Name,
                DisplayName = x.DisplayName,
                GroupName = x.GroupName
            }));
        }

        /// <summary>
        /// Sets the grants.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="grants">The grants.</param>
        /// <returns>IHttpActionResult.</returns>
        [ResponseType(typeof(void))]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/employees/{id:int}/grants")]
        public IHttpActionResult SetGrants(int id, EmployeePermissionCreateOrUpdate employeePermission)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.AddEmployee))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Employee employee = rpoContext.Employees.Find(id);
                if (employee == null)
                {
                    return this.NotFound();
                }

                if (employeePermission.Permissions != null && employeePermission.Permissions.Count() > 0)
                {
                    employee.Permissions = string.Join(",", employeePermission.Permissions.Select(x => x.ToString()));
                }
                else
                {
                    employee.Permissions = string.Empty;
                }

                employee.LastModifiedBy = loginEmployee.Id;
                employee.LastModifiedDate = DateTime.UtcNow;

                rpoContext.Entry(employee).State = EntityState.Modified;

                rpoContext.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Parameter 1 : DeletedDocumentIds (String)
        /// Parameter 2 : idEmployee (String)
        /// Parameter 3 : Document files want to Upload (File)
        /// </summary>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        [Authorize]
        [RpoAuthorize]
        [Route("api/employees/Document")]
        [ResponseType(typeof(EmployeeDocument))]
        public async Task<HttpResponseMessage> PutEmployee()
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.EditDocuments))
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                System.Collections.Specialized.NameValueCollection formData = provider.FormData;
                IList<HttpContent> files = provider.Files;
                string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
                int idEmployee = Convert.ToInt32(formData["idEmployee"]);

                string deletedDocumentIds = Convert.ToString(formData["deletedDocumentIds"]);

                if (!string.IsNullOrEmpty(deletedDocumentIds))
                {
                    foreach (var item in deletedDocumentIds.Split(','))
                    {
                        int employeeDocumentId = Convert.ToInt32(item);
                        EmployeeDocument employeeDocument = rpoContext.EmployeeDocuments.Where(x => x.Id == employeeDocumentId).FirstOrDefault();
                        if (employeeDocument != null)
                        {
                            rpoContext.EmployeeDocuments.Remove(employeeDocument);
                            var path = HttpRuntime.AppDomainAppPath;
                            string directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.EmployeeDocumentPath));
                            string directoryDelete = Convert.ToString(employeeDocument.Id) + "_" + employeeDocument.DocumentPath;
                            string deletefilename = System.IO.Path.Combine(directoryName, directoryDelete);

                            if (File.Exists(deletefilename))
                            {
                                File.Delete(deletefilename);
                            }
                        }
                    }
                    rpoContext.SaveChanges();
                }

                foreach (HttpContent item in files)
                {
                    HttpContent file1 = item;
                    var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

                    string filename = string.Empty;
                    Stream input = await file1.ReadAsStreamAsync();
                    string directoryName = string.Empty;
                    string URL = string.Empty;

                    EmployeeDocument employeeDocument = new EmployeeDocument();
                    employeeDocument.DocumentPath = thisFileName;
                    employeeDocument.IdEmployee = idEmployee;
                    employeeDocument.Name = thisFileName;
                    rpoContext.EmployeeDocuments.Add(employeeDocument);
                    rpoContext.SaveChanges();

                    var path = HttpRuntime.AppDomainAppPath;
                    directoryName = System.IO.Path.Combine(path, Convert.ToString(Properties.Settings.Default.EmployeeDocumentPath));

                    string directoryFileName = Convert.ToString(employeeDocument.Id) + "_" + thisFileName;
                    filename = System.IO.Path.Combine(directoryName, directoryFileName);

                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }

                    using (Stream file = File.OpenWrite(filename))
                    {
                        input.CopyTo(file);
                        file.Close();
                    }
                }

                var employeeDocumentList = rpoContext.EmployeeDocuments
                    .Where(x => x.IdEmployee == idEmployee).ToList();

                var response = Request.CreateResponse<List<EmployeeDocument>>(HttpStatusCode.OK, employeeDocumentList);
                return response;
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// Formats the details.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <param name="employeePermissions">The employee permissions.</param>
        /// <returns>Rpo.ApiServices.Model.Models.EmployeeDTO.</returns>
        private EmployeeDTO FormatDetails(Employee employee, List<int> employeePermissions)
        {
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new EmployeeDTO
            {
                Address1 = employee.Address1,
                Address2 = employee.Address2,
                Id = employee.Id,
                AgentCertificates = employee.AgentCertificates.Select(ac => new AgentCertificate
                {
                    Id = ac.Id,
                    DocumentType = ac.DocumentType,
                    IdDocumentType = ac.IdDocumentType,
                    NumberId = ac.NumberId,
                    ExpirationDate = ac.ExpirationDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(ac.ExpirationDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : ac.ExpirationDate,
                    Pin = ac.Pin,
                    IdEmployee = ac.IdEmployee
                }),
                City = employee.City,
                State = employee.IdState.HasValue ? employee.State.Acronym : "",
                IdState = employee.IdState,
                ZipCode = employee.ZipCode,
                ComputerPassword = employee.ComputerPassword,
                Dob = employee.Dob != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(employee.Dob), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.Dob,
                Documents = employee.Documents.Select(d => new EmployeeDocument
                {
                    Name = d.Name,
                    Id = d.Id,
                    DocumentPath = APIUrl + "/" + Properties.Settings.Default.EmployeeDocumentPath + "/" + d.Id + "_" + d.DocumentPath,
                }),
                EfillingPassword = employee.EfillingPassword,
                EfillingUserName = employee.EfillingUserName,
                Email = employee.Email,
                FinalDate = employee.FinalDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(employee.FinalDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.FinalDate,
                FirstName = employee.FirstName,
                Group = employee.Group.Name,
                IdGroup = employee.IdGroup,
                LastName = employee.LastName,
                MobilePhone = employee.MobilePhone,
                HomePhone = employee.HomePhone,
                Notes = employee.Notes,
                Ssn = employee.Ssn,
                StartDate = employee.StartDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(employee.StartDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.StartDate,
                TelephonePassword = employee.TelephonePassword,
                WorkPhone = employee.WorkPhone,
                WorkPhoneExt = employee.WorkPhoneExt,
                IsActive = employee.IsActive,
                DocumentsToDelete = new int[0],
                EmergencyContactName = employee.EmergencyContactName,
                EmergencyContactNumber = employee.EmergencyContactNumber,
                LockScreenPassword = employee.LockScreenPassword,
                AppleId = employee.AppleId,
                ApplePassword = employee.ApplePassword,
                Permissions = employeePermissions.ToArray(),
                IsArchive = employee.IsArchive,
                AllergyType = employee.AllergyType,
                AllergyDescription = employee.AllergyDescription,
                QBEmployeeName = employee.QBEmployeeName,
                AllPermissions = GetPermission(employeePermissions),
                CreatedBy = employee.CreatedBy,
                LastModifiedBy = employee.LastModifiedBy != null ? employee.LastModifiedBy : employee.CreatedBy,
                CreatedByEmployeeName = employee.CreatedByEmployee != null ? employee.CreatedByEmployee.FirstName + " " + employee.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByEmployeeName = employee.LastModifiedByEmployee != null ? employee.LastModifiedByEmployee.FirstName + " " + employee.LastModifiedByEmployee.LastName : (employee.CreatedByEmployee != null ? employee.CreatedByEmployee.FirstName + " " + employee.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = employee.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(employee.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.CreatedDate,
                LastModifiedDate = employee.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(employee.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (employee.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(employee.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : employee.CreatedDate),
            };
        }
    }
}