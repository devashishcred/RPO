// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="Group.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Group.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Group.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set;  }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        [Index("IX_GroupName", IsUnique = true)]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
        /// <summary>
        /// Get Or Sets the Permissions
        /// </summary>
        /// <value>The Permission</value>

        public string Permissions { get; set; }

        //public int? IdGroupPermission { get; set; }

        ///// <summary>
        ///// Gets or sets the created by employee.
        ///// </summary>
        ///// <value>The created by employee.</value>
        //[ForeignKey("IdGroupPermission")]
        //public UserGroupPermission UserGroupPermission { get; set; }


        ///// <summary>
        ///// Gets or sets the employee employee information.
        ///// </summary>
        ///// <value>The employee employee information.</value>
        //public GrantType EmployeeEmployeeInfo { get; set; }

        ///// <summary>
        ///// Gets or sets the employee contact information.
        ///// </summary>
        ///// <value>The employee contact information.</value>
        //public GrantType EmployeeContactInfo { get; set; }

        ///// <summary>
        ///// Gets or sets the employee personal information.
        ///// </summary>
        ///// <value>The employee personal information.</value>
        //public GrantType EmployeePersonalInfo { get; set; }

        ///// <summary>
        ///// Gets or sets the employee agent certificates.
        ///// </summary>
        ///// <value>The employee agent certificates.</value>
        //public GrantType EmployeeAgentCertificates { get; set; }

        ///// <summary>
        ///// Gets or sets the employee system access information.
        ///// </summary>
        ///// <value>The employee system access information.</value>
        //public GrantType EmployeeSystemAccessInformation { get; set; }

        ///// <summary>
        ///// Gets or sets the employee user group.
        ///// </summary>
        ///// <value>The employee user group.</value>
        //public GrantType EmployeeUserGroup { get; set; }

        ///// <summary>
        ///// Gets or sets the employee documents.
        ///// </summary>
        ///// <value>The employee documents.</value>
        //public GrantType EmployeeDocuments { get; set; }

        ///// <summary>
        ///// Gets or sets the employee status.
        ///// </summary>
        ///// <value>The employee status.</value>
        //public GrantType EmployeeStatus { get; set; }

        ///// <summary>
        ///// Gets or sets the jobs.
        ///// </summary>
        ///// <value>The jobs.</value>
        //public GrantType Jobs { get; set; }

        ///// <summary>
        ///// Gets or sets the contacts.
        ///// </summary>
        ///// <value>The contacts.</value>
        //public GrantType Contacts { get; set; }

        ///// <summary>
        ///// Gets or sets the contacts export.
        ///// </summary>
        ///// <value>The contacts export.</value>
        //public GrantType ContactsExport { get; set; }

        ///// <summary>
        ///// Gets or sets the company.
        ///// </summary>
        ///// <value>The company.</value>
        //public GrantType Company { get; set; }

        ///// <summary>
        ///// Gets or sets the company export.
        ///// </summary>
        ///// <value>The company export.</value>
        //public GrantType CompanyExport { get; set; }

        ///// <summary>
        ///// Gets or sets the RFP.
        ///// </summary>
        ///// <value>The RFP.</value>
        //public GrantType RFP { get; set; }

        ///// <summary>
        ///// Gets or sets the tasks.
        ///// </summary>
        ///// <value>The tasks.</value>
        //public GrantType Tasks { get; set; }

        ///// <summary>
        ///// Gets or sets the reports.
        ///// </summary>
        ///// <value>The reports.</value>
        //public GrantType Reports { get; set; }

        ///// <summary>
        ///// Gets or sets the reference links.
        ///// </summary>
        ///// <value>The reference links.</value>
        //public GrantType ReferenceLinks { get; set; }

        ///// <summary>
        ///// Gets or sets the reference documents.
        ///// </summary>
        ///// <value>The reference documents.</value>
        //public GrantType ReferenceDocuments { get; set; }

        ///// <summary>
        ///// Gets or sets the user group.
        ///// </summary>
        ///// <value>The user group.</value>
        //public GrantType UserGroup { get; set; }

        ///// <summary>
        ///// Gets or sets the masters.
        ///// </summary>
        ///// <value>The masters.</value>
        //public GrantType Masters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }
    }
}
