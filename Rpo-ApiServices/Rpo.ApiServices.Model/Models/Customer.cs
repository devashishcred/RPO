// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Mital Bhatt
// Created          : 5-9-2023
//
// Last Modified By :  Mital Bhatt
// Last Modified On : 5-9-2023
// ***********************************************************************
// <copyright file="Employee.cs" company="CREDENCYS">
//     Copyright ©  2023
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
    public class Customer
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        ///// <summary>
        ///// Gets or sets the first name.
        ///// </summary>
        ///// <value>The first name.</value>
        //[MaxLength(50)]
        //[Required]
        //[Index("IX_EmployeeName", Order = 1, IsUnique = false)]
        public string FirstName { get; set; }

        ///// <summary>
        ///// Gets or sets the last name.
        ///// </summary>
        ///// <value>The last name.</value>
        //[MaxLength(50)]
        //[Required]
        //[Index("IX_EmployeeName", Order = 2, IsUnique = false)]
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public virtual int IdContcat { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        /// 
        [ForeignKey("IdContcat")]
        public virtual Contact Contcat { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [MaxLength(255)]
        [Required]
       // [Index("IX_EmployeeEmail", IsUnique = true)]
        public string EmailAddress { get; set; }
        /// <summary>
        /// Gets or sets the login password.
        /// </summary>
        /// <value>The login password.</value>
        [MaxLength(25)]
        public string LoginPassword { get; set; }
    /// <summary>
    /// Gets or sets the contact image path.
    /// </summary>
    /// <value>The contact image path.</value>
    [MaxLength(200)]
    public string ProfileImage { get; set; }


    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
    public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the state of the identifier.
        /// </summary>
        /// <value>The state of the identifier.</value>
        /// 

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
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public string Permissions { get; set; }

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
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedByCus { get; set; }

        /// <summary>
        /// Gets or sets the last modified by employee.
        /// </summary>
        /// <value>The last modified by employee.</value>
        [ForeignKey("LastModifiedByCus")]
        public virtual Customer LastModifiedByCustomer { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }


     
        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
      
        public DateTime RenewalDate { get; set; }

        /// <summary>
        /// Gets or sets the work phone.
        /// </summary>
        /// <value>The work phone.</value> 
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the work phone ext.
        /// </summary>
        /// <value>The work phone ext.</value>       
        public bool CustomerConsent { get; set; }    


        ///// <summary>
        ///// Gets or sets the SSN.
        ///// </summary>
        ///// <value>The SSN.</value>
        //[MaxLength(10)]
        //public string Ssn { get; set; } 
    }
}

