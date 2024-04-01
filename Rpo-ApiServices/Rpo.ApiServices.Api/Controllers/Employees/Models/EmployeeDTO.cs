// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="EmployeeDto.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Employee DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using Api.Controllers.Permissions;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Employee DTO.
    /// </summary>
    public class EmployeeDTO
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
        /// Gets or sets the address1.
        /// </summary>
        /// <value>The address1.</value>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the address2.
        /// </summary>
        /// <value>The address2.</value>
        public string Address2 { get; set; }

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
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the SSN.
        /// </summary>
        /// <value>The SSN.</value>
        public string Ssn { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>The dob.</value>
        public DateTime? Dob { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the final date.
        /// </summary>
        /// <value>The final date.</value>
        public DateTime? FinalDate { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the login password.
        /// </summary>
        /// <value>The login password.</value>
        public string LoginPassword { get; set; }

        /// <summary>
        /// Gets or sets the telephone password.
        /// </summary>
        /// <value>The telephone password.</value>
        public string TelephonePassword { get; set; }

        /// <summary>
        /// Gets or sets the computer password.
        /// </summary>
        /// <value>The computer password.</value>
        public string ComputerPassword { get; set; }

        /// <summary>
        /// Gets or sets the efilling password.
        /// </summary>
        /// <value>The efilling password.</value>
        public string EfillingPassword { get; set; }

        /// <summary>
        /// Gets or sets the name of the efilling user.
        /// </summary>
        /// <value>The name of the efilling user.</value>
        public string EfillingUserName { get; set; }

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
        /// Gets or sets the agent certificates.
        /// </summary>
        /// <value>The agent certificates.</value>
        public virtual IEnumerable<AgentCertificate> AgentCertificates { get; set; }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>The documents.</value>
        public virtual IEnumerable<EmployeeDocument> Documents { get; set; }

        /// <summary>
        /// Gets or sets the documents to delete.
        /// </summary>
        /// <value>The documents to delete.</value>
        public virtual IEnumerable<int> DocumentsToDelete { get; set; }

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
        /// Gets or sets the name of the emergency contact.
        /// </summary>
        /// <value>The name of the emergency contact.</value>
        public string EmergencyContactName { get; set; }

        /// <summary>
        /// Gets or sets the emergency contact number.
        /// </summary>
        /// <value>The emergency contact number.</value>
        public string EmergencyContactNumber { get; set; }

        /// <summary>
        /// Gets or sets the lock screen password.
        /// </summary>
        /// <value>The lock screen password.</value>
        public string LockScreenPassword { get; set; }

        /// <summary>
        /// Gets or sets the apple identifier.
        /// </summary>
        /// <value>The apple identifier.</value>
        public string AppleId { get; set; }

        /// <summary>
        /// Gets or sets the apple password.
        /// </summary>
        /// <value>The apple password.</value>
        public string ApplePassword { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public int[] Permissions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archive.
        /// </summary>
        /// <value><c>true</c> if this instance is archive; otherwise, <c>false</c>.</value>
        public bool IsArchive { get; set; }

        /// <summary>
        /// Gets or sets all permissions.
        /// </summary>
        /// <value>All permissions.</value>
        public List<PermissionModuleDTO> AllPermissions { get; set; }

        public int? CreatedBy { get; set; }

        public int? LastModifiedBy { get; set; }

        public string CreatedByEmployeeName { get; set; }

        public string LastModifiedByEmployeeName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string QBEmployeeName { get; set; }

        public AllergyType? AllergyType { get; set; }

        public string AllergyDescription { get; set; }
    }
}
