using Rpo.ApiServices.Api.Controllers.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.Customer.Model
{
    /// <summary>
    ///  Class Customer DTO.
    /// </summary>
    public class CustomerDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the CompanyName.
        /// </summary>
        /// <value>The CompanyName.</value>
        public string CompanyName { get; set; }      
       
        public IEnumerable<Addresses.AddressDTO> Addresses { get; set; }
        public virtual int? IdContact { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the identifier city.
        /// </summary>
        /// <value>The identifier city.</value>
        public virtual int? IdCity { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the state of the identifier.
        /// </summary>
        /// <value>The state of the identifier.</value>
        public virtual int? IdState { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the work phone.
        /// </summary>
        /// <value>The work phone.</value>
        public string WorkPhone { get; set; }

        /// <summary>
        /// Gets or sets the work phone ext.
        /// </summary>
        /// <value>The work phone ext.</value>
        public string WorkPhoneExt { get; set; }

        /// <summary>
        /// Gets or sets the mobile phone.
        /// </summary>
        /// <value>The mobile phone.</value>
        public string MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets the home phone.
        /// </summary>
        /// <value>The home phone.</value>
        public string HomePhone { get; set; }

        /// <summary>
        /// Gets or sets the EmailAddress.
        /// </summary>
        /// <value>The EmailAddress.</value>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the login password.
        /// </summary>
        /// <value>The login password.</value>
        public string LoginPassword { get; set; }
        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        public virtual string Group { get; set; }

        /// <summary>
        /// Gets or sets the identifier group.
        /// </summary>
        /// <value>The identifier group.</value>
        public virtual int IdGroup { get; set; }
       

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get { return this.IsActive ? "Active" : "Inactive"; } }       

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public int[] Permissions { get; set; }       

        /// <summary>
        /// Gets or sets all permissions.
        /// </summary>
        /// <value>All permissions.</value>
        public List<PermissionModuleDTO> AllPermissions { get; set; }

        public int? CreatedBy { get; set; }

        public int? LastModifiedBy { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public string LastModifiedByEmployeeName { get; set; }
        public string LastModifiedByCustomerName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public bool CustomerConsent { get; set; }
        public int customerInvitationStatus { get; set; }
        public List<int> ProjectList { get; set; }
       
       // public IEnumerable<ProjectsDTO> ProjectID { get; set; }
    }
    //public class ProjectsDTO
    //{
    //    public string Projects { get; set; }
    //}
}