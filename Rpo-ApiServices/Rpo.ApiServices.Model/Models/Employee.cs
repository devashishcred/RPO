// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-27-2018
// ***********************************************************************
// <copyright file="Employee.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using Rpo.ApiServices.Model.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Employee.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [MaxLength(50)]
        [Required]
        [Index("IX_EmployeeName", Order = 1, IsUnique = false)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [MaxLength(50)]
        [Required]
        [Index("IX_EmployeeName", Order = 2, IsUnique = false)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>The address1.</value>
        [MaxLength(50)]
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the address2.
        /// </summary>
        /// <value>The address2.</value>
        [MaxLength(50)]
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state of the identifier.
        /// </summary>
        /// <value>The state of the identifier.</value>
        public int? IdState { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        [ForeignKey("IdState")]
        public virtual State State { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        [MaxLength(10)]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the work phone.
        /// </summary>
        /// <value>The work phone.</value>
        [MaxLength(15)]
        public string WorkPhone { get; set; }

        /// <summary>
        /// Gets or sets the work phone ext.
        /// </summary>
        /// <value>The work phone ext.</value>
        [MaxLength(5)]
        public string WorkPhoneExt { get; set; }

        /// <summary>
        /// Gets or sets the mobile phone.
        /// </summary>
        /// <value>The mobile phone.</value>
        [MaxLength(15)]
        public string MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets the home phone.
        /// </summary>
        /// <value>The home phone.</value>
        [MaxLength(15)]
        public string HomePhone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [MaxLength(255)]
        [Required]
        [Index("IX_EmployeeEmail", IsUnique = true)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the SSN.
        /// </summary>
        /// <value>The SSN.</value>
        [MaxLength(10)]
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
        /// Gets or sets the telephone password.
        /// </summary>
        /// <value>The telephone password.</value>
        [MaxLength(25)]
        public string TelephonePassword { get; set; }

        /// <summary>
        /// Gets or sets the computer password.
        /// </summary>
        /// <value>The computer password.</value>
        [MaxLength(25)]
        public string ComputerPassword { get; set; }

        /// <summary>
        /// Gets or sets the efilling password.
        /// </summary>
        /// <value>The efilling password.</value>
        [MaxLength(25)]
        public string EfillingPassword { get; set; }

        /// <summary>
        /// Gets or sets the name of the efilling user.
        /// </summary>
        /// <value>The name of the efilling user.</value>
        [MaxLength(25)]
        public string EfillingUserName { get; set; }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        [ForeignKey("IdGroup")]
        public virtual Group Group { get; set; }

        /// <summary>
        /// Gets or sets the identifier group.
        /// </summary>
        /// <value>The identifier group.</value>
        public virtual int IdGroup { get; set; }

        /// <summary>
        /// Gets or sets the agent certificates.
        /// </summary>
        /// <value>The agent certificates.</value>
        [ForeignKey("IdEmployee")]
        public virtual ICollection<AgentCertificate> AgentCertificates { get; set; }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>The documents.</value>
        [ForeignKey("IdEmployee")]
        public virtual ICollection<EmployeeDocument> Documents { get; set; }


        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public string Permissions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the login password.
        /// </summary>
        /// <value>The login password.</value>
        [MaxLength(25)]
        public string LoginPassword { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the emergency contact.
        /// </summary>
        /// <value>The name of the emergency contact.</value>
        [MaxLength(50)]
        public string EmergencyContactName { get; set; }

        /// <summary>
        /// Gets or sets the emergency contact number.
        /// </summary>
        /// <value>The emergency contact number.</value>
        [MaxLength(25)]
        public string EmergencyContactNumber { get; set; }

        /// <summary>
        /// Gets or sets the lock screen password.
        /// </summary>
        /// <value>The lock screen password.</value>
        [MaxLength(25)]
        public string LockScreenPassword { get; set; }

        /// <summary>
        /// Gets or sets the apple identifier.
        /// </summary>
        /// <value>The apple identifier.</value>
        [MaxLength(50)]
        public string AppleId { get; set; }

        /// <summary>
        /// Gets or sets the apple password.
        /// </summary>
        /// <value>The apple password.</value>
        [MaxLength(25)]
        public string ApplePassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archive.
        /// </summary>
        /// <value><c>true</c> if this instance is archive; otherwise, <c>false</c>.</value>
        public bool IsArchive { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified by employee.
        /// </summary>
        /// <value>The last modified by employee.</value>
        [ForeignKey("LastModifiedBy")]
        public virtual Employee LastModifiedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the qb employee.
        /// </summary>
        /// <value>The name of the qb employee.</value>
        public string QBEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the type of the allergy.
        /// </summary>
        /// <value>The type of the allergy.</value>
        public AllergyType? AllergyType { get; set; }

        /// <summary>
        /// Gets or sets the allergy description.
        /// </summary>
        /// <value>The allergy description.</value>
        public string AllergyDescription { get; set; }
    }
}
