
namespace Rpo.ApiServices.Api.Controllers.Customer
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
    using Model;
    using System.Configuration;
    using Employees;
    using System.Data.SqlClient;
    using Microsoft.ApplicationBlocks.Data;
    using Rpo.ApiServices.Api.Controllers.Contacts;

    public class CustomersAdminController : ApiController
    {
        /// <summary>
        /// The rpo context
        /// </summary>
        private RpoContext rpoContext = new RpoContext();
        /// <summary>
        /// Class Employee Advanced Search Parameters.
        /// </summary>
        /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
        public class CustomerAdvancedSearchParameters : DataTableParameters
        {      /// <summary>
               /// Gets or sets the type of the identifier contact license.
               /// </summary>
               /// <value>The type of the identifier contact license.</value>
            public int? IdContactLicenseType { get; set; }

            /// <summary>
            /// Gets or sets the identifier company.
            /// </summary>
            /// <value>The identifier company.</value>
            public int? IdCompany { get; set; }

            /// <summary>
            /// Gets or sets the type of the global search.
            /// </summary>
            /// <value>The type of the global search.</value>
            public int? GlobalSearchType { get; set; }

            /// <summary>
            /// Gets or sets the global search text.
            /// </summary>
            /// <value>The global search text.</value>
            public string GlobalSearchText { get; set; }

            public int? Individual { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether this instance is active.
            /// </summary>
            /// <value><c>null</c> if [is active] contains no value, <c>true</c> if [is active]; otherwise, <c>false</c>.</value>
            public bool? IsActive { get; set; }
        }
       
        /// <summary>
        /// Get the contact List
        /// </summary>
        /// <remarks>To get the list of all contacts</remarks>
        /// <param name="dataTableParameters"></param>
        /// <returns>Return Contact with pagingation List</returns>
        [HttpPost]
        [Authorize]
        [RpoAuthorize]
        [ResponseType(typeof(DataTableResponse))]
        //[Route("api/ContactListPost")]
        public IHttpActionResult GetCustomers(CustomerAdvancedSearchParameters dataTableParameters)

        {
            var employee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(employee.Permissions, Enums.Permission.ViewContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.AddContact)
                  || Common.CheckUserPermission(employee.Permissions, Enums.Permission.DeleteContact))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["RpoConnection"].ToString();
                string Firstname = string.Empty;
                string Lastname = string.Empty;
                //if (dataTableParameters != null)// && dataTableParameters.GlobalSearchType != null && dataTableParameters.GlobalSearchText != null && dataTableParameters.GlobalSearchType > 0)
                //{
                //    string[] strArray = dataTableParameters.GlobalSearchText.Split(' ');
                //    if (strArray.Count() > 0)
                //    {
                //        Firstname = strArray[0].Trim();
                //    }
                //    if (strArray.Count() > 1)
                //    {
                //        Lastname = strArray[1].Trim();
                //    }
                //}

                DataSet ds = new DataSet();

                SqlParameter[] spParameter = new SqlParameter[12];

                spParameter[0] = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50);
                spParameter[0].Direction = ParameterDirection.Input;
                spParameter[0].Value = Firstname;

                spParameter[1] = new SqlParameter("@LastName", SqlDbType.NVarChar, 50);
                spParameter[1].Direction = ParameterDirection.Input;
                spParameter[1].Value = Lastname;

                spParameter[2] = new SqlParameter("@IdContactLicenseType", SqlDbType.Int);
                spParameter[2].Direction = ParameterDirection.Input;
                spParameter[2].Value =  null;
                //spParameter[2].Value = dataTableParameters.IdContactLicenseType != null ? dataTableParameters.IdContactLicenseType : null;

                spParameter[3] = new SqlParameter("@PageIndex", SqlDbType.Int);
                spParameter[3].Direction = ParameterDirection.Input;
                spParameter[3].Value = dataTableParameters.Start;

                spParameter[4] = new SqlParameter("@PageSize", SqlDbType.Int);
                spParameter[4].Direction = ParameterDirection.Input;
                spParameter[4].Value = dataTableParameters.Length;

                spParameter[5] = new SqlParameter("@Column", SqlDbType.NVarChar, 50);
                spParameter[5].Direction = ParameterDirection.Input;
                spParameter[5].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Column : null;

                spParameter[6] = new SqlParameter("@Dir", SqlDbType.VarChar, 50);
                spParameter[6].Direction = ParameterDirection.Input;
                spParameter[6].Value = dataTableParameters.OrderedColumn != null ? dataTableParameters.OrderedColumn.Dir : null;

                spParameter[7] = new SqlParameter("@Search", SqlDbType.NVarChar, 50);
                spParameter[7].Direction = ParameterDirection.Input;
                spParameter[7].Value = !string.IsNullOrEmpty(dataTableParameters.Search) ? dataTableParameters.Search : string.Empty;

                spParameter[8] = new SqlParameter("@IdCompany", SqlDbType.Int);
                spParameter[8].Direction = ParameterDirection.Input;
               // spParameter[8].Value = dataTableParameters.IdCompany != null ? dataTableParameters.IdCompany : null;
                spParameter[8].Value = null;

                spParameter[9] = new SqlParameter("@Individual", SqlDbType.Int);
                spParameter[9].Direction = ParameterDirection.Input;
               // spParameter[9].Value = dataTableParameters.Individual != null ? dataTableParameters.Individual : null;
                spParameter[9].Value =  null;

                spParameter[10] = new SqlParameter("@IsActive", SqlDbType.Bit);
                spParameter[10].Direction = ParameterDirection.Input;
                 spParameter[10].Value = dataTableParameters.IsActive != null ? dataTableParameters.IsActive : true;
                

                spParameter[11] = new SqlParameter("@RecordCount", SqlDbType.Int);
                spParameter[11].Direction = ParameterDirection.Output;

                ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "Customer_portal_contact_list", spParameter);
                int totalrecord = 0;
                int Recordcount = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    totalrecord = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    Recordcount = int.Parse(spParameter[11].SqlValue.ToString());
                }
                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].TableName = "Data";
                }
                var result=ds.Tables[1].AsEnumerable().ToList();
                ds.Tables[1].Columns.Add("IdJobs");
                for (int i=0;i< ds.Tables[1].Rows.Count;i++)
                {
                    string emailId = ds.Tables[1].Rows[i]["email"].ToString();
                    var customer=  rpoContext.Customers.Where(x => x.EmailAddress == emailId).FirstOrDefault();                 
                    var jobs = rpoContext.CustomerJobAccess.Where(x => x.IdCustomer == customer.Id).Select(y => y.IdJob).ToList();
                    string idjobs=null;
                    foreach(var j in jobs)
                    {
                        idjobs += j + ", ";
                    }
                    if(idjobs!=null)
                    idjobs= idjobs.Remove(idjobs.Length - 2, 2);
                    ds.Tables[1].Rows[i]["IdJobs"] = idjobs;
                }
               
                return Ok(new DataTableResponse
                {
                    Draw = dataTableParameters.Start,
                    RecordsFiltered = Recordcount,
                    RecordsTotal = totalrecord,
                    Data = ds.Tables[1]
                });

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
        [ResponseType(typeof(CustomerDTO))]
        [HttpGet]
        [Authorize]
        [RpoAuthorize]
        [Route("api/customersadmin/{id:int}")]
        public IHttpActionResult GetCustomer(int id)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.ViewEmployee)
                  || Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.AddEmployee)
                  || Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.DeleteEmployee))
            {
                Customer customer = rpoContext.Customers.Find(id);
                if (customer == null)
                {
                    return this.NotFound();
                }
                List<int> customerPermissions = customer.Permissions != null && !string.IsNullOrEmpty(customer.Permissions) ? (customer.Permissions.Split(',') != null && customer.Permissions.Split(',').Any() ? customer.Permissions.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>()) : new List<int>();

                return Ok(FormatDetails(customer, customerPermissions));
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
        /// Put Sets the active.
        /// </summary>
        /// <remarks>Customer update the status active</remarks>
        /// <param name="id">The identifier.</param>
        /// <returns>update the customer status active.</returns>
        [ResponseType(typeof(void))]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/customersadmin/{id:int}/active")]
        public IHttpActionResult SetActive(int id)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.EditStatus))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Customer customer = rpoContext.Customers.Find(id);
                if (customer == null)
                {
                    return this.NotFound();
                }

                using (RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(rpoContext)))
                {
                    RpoIdentityUser systemUser = userManager.FindByEmail(customer.EmailAddress);
                    if (systemUser != null)
                    {
                        systemUser.UserName = customer.EmailAddress;
                        systemUser.LockoutEnabled = true;
                        userManager.Update(systemUser);
                    }
                }

                customer.IsActive = true;
                customer.LastModifiedBy = loginEmployee.Id;
                customer.LastModifiedDate = DateTime.UtcNow;
                rpoContext.SaveChanges();

                return Ok(customer);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }

        /// <summary>
        /// put Sets the inactive.
        /// </summary>
        /// <remarks>Customer update the status inactive</remarks>
        /// <param name="id">The identifier.</param>
        /// <returns>update the customer status inactive.</returns>
        [ResponseType(typeof(void))]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/customersadmin/{id:int}/inactive")]
        public IHttpActionResult SetInactive(int id)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.EditStatus))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Customer customer = rpoContext.Customers.Find(id);
                if (customer == null)
                {
                    return this.NotFound();
                }
                using (RpoUserManager userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(rpoContext)))
                {
                    RpoIdentityUser systemUser = userManager.FindByEmail(customer.EmailAddress);

                    if (systemUser != null)
                    {
                        systemUser.UserName = customer.EmailAddress;
                        systemUser.LockoutEnabled = false;
                        userManager.Update(systemUser);
                    }
                }

                customer.IsActive = false;

                customer.LastModifiedBy = loginEmployee.Id;
                customer.LastModifiedDate = DateTime.UtcNow;

                rpoContext.SaveChanges();

                return Ok(customer);
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
        private CustomerDTO FormatDetails(Customer customer, List<int> customerPermissions)
        {
            string APIUrl = Convert.ToString(Properties.Settings.Default.APIUrl);
            string currentTimeZone = Common.FetchHeaderValues(this.Request, Common.CurrentTimezoneHeaderKey);

            return new CustomerDTO
            {

                Addresses = rpoContext.Addresses.Where(d => d.IdContact == customer.IdContcat).Select(d => new Addresses.AddressDTO
                {
                    //Id = d.Id,
                    Address1 = d.Address1,
                    Address2 = d.Address2,
                    AddressType = d.AddressType,
                    IdAddressType = d.IdAddressType,
                    City = d.City,
                    IdState = d.IdState,
                    State = d.State.Acronym,
                    ZipCode = d.ZipCode,
                    Phone = d.Phone,
                    IsMainAddress = d.IsMainAddress,
                }).OrderBy(x => x.AddressType.DisplayOrder),
                Id = customer.Id,
                IdContact = customer.IdContcat,
                EmailAddress = customer.EmailAddress,
                FirstName = customer.FirstName,
                Group = customer.Group.Name,
                IdGroup = customer.IdGroup,
                Permissions = customerPermissions.ToArray(),
                LastName = customer.LastName,
                IsActive = customer.IsActive,
                AllPermissions = GetPermission(customerPermissions),
                CreatedBy = customer.CreatedBy,
                LastModifiedBy = customer.LastModifiedByCus != null ? customer.LastModifiedByCus : customer.CreatedBy,
                CreatedByEmployeeName = customer.CreatedByEmployee != null ? customer.CreatedByEmployee.FirstName + " " + customer.CreatedByEmployee.LastName : string.Empty,
                LastModifiedByCustomerName = customer.LastModifiedByCustomer != null ? customer.LastModifiedByCustomer.FirstName + " " + customer.LastModifiedByCustomer.LastName : (customer.CreatedByEmployee != null ? customer.CreatedByEmployee.FirstName + " " + customer.CreatedByEmployee.LastName : string.Empty),
                CreatedDate = customer.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(customer.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : customer.CreatedDate,
                LastModifiedDate = customer.LastModifiedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(customer.LastModifiedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : (customer.CreatedDate != null ? TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(customer.CreatedDate), TimeZoneInfo.FindSystemTimeZoneById(currentTimeZone)) : customer.CreatedDate),
            };
        }
        /// <summary>
        /// Gets the permission.
        /// </summary>
        /// <remarks> Employee permission List </remarks> 
        /// <param name="customerPermissions">The employee permissions.</param>
        /// <returns>List&lt;PermissionModuleDTO&gt;.Get the permission list</returns>
        private List<PermissionModuleDTO> GetPermission(List<int> customerPermissions)
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
                        if (customerPermissions.Contains(item.Id))
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
        /// Sets the grants.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="grants">The grants.</param>
        /// <returns>IHttpActionResult.</returns>
        [ResponseType(typeof(void))]
        [HttpPut]
        [Authorize]
        [RpoAuthorize]
        [Route("api/customersadmin/{id:int}/permissions")]
        public IHttpActionResult Permissions(int id, CustomerPermissionCreateOrUpdate customerePermission)
        {
            var loginEmployee = this.rpoContext.Employees.FirstOrDefault(e => e.Email == RequestContext.Principal.Identity.Name);
            if (Common.CheckUserPermission(loginEmployee.Permissions, Enums.Permission.AddEmployee))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Customer customer = rpoContext.Customers.Find(id);
                if (customer == null)
                {
                    return this.NotFound();
                }

                if (customerePermission.Permissions != null && customerePermission.Permissions.Count() > 0)
                {
                    customer.Permissions = string.Join(",", customerePermission.Permissions.Select(x => x.ToString()));
                }
                else
                {
                    customer.Permissions = string.Empty;
                }

                customer.LastModifiedBy = loginEmployee.Id;
                customer.LastModifiedDate = DateTime.UtcNow;

                rpoContext.Entry(customer).State = EntityState.Modified;

                rpoContext.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                throw new RpoBusinessException(StaticMessages.NoPermissionMessage);
            }
        }
    }
}
