// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="Contact.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary> Class Contact.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Contact.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the personal.
        /// </summary>
        /// <value>The type of the personal.</value>
        public PersonalType PersonalType { get; set; }

        /// <summary>
        /// Gets or sets the identifier prefix.
        /// </summary>
        /// <value>The identifier prefix.</value>
        public virtual int? IdPrefix { get; set; }
        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        /// <value>The prefix.</value>
        [ForeignKey("IdPrefix")]
        public virtual Prefix Prefix { get; set; }

        /// <summary>
        /// Gets or sets the identifier suffix.
        /// </summary>
        /// <value>The identifier suffix.</value>
        public virtual int? IdSuffix { get; set; }
        /// <summary>
        /// Gets or sets the suffix.
        /// </summary>
        /// <value>The suffix.</value>
        [ForeignKey("IdSuffix")]
        public virtual Suffix Suffix { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>The name of the middle.</value>
        [MaxLength(2)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [MaxLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the identifier company.
        /// </summary>
        /// <value>The identifier company.</value>
        public int? IdCompany { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        [ForeignKey("IdCompany")]
        public virtual Company Company { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact title.
        /// </summary>
        /// <value>The identifier contact title.</value>
        public virtual int? IdContactTitle { get; set; }
        /// <summary>
        /// Gets or sets the contact title.
        /// </summary>
        /// <value>The contact title.</value>
        [ForeignKey("IdContactTitle")]
        public virtual ContactTitle ContactTitle { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>The addresses.</value>
        [ForeignKey("IdContact")]
        public virtual ICollection<Address> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>The birth date.</value>
        public DateTime? BirthDate { get; set; }

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
        /// Gets or sets the other phone.
        /// </summary>
        /// <value>The other phone.</value>
        [MaxLength(15)]
        public string OtherPhone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [MaxLength(255)]
        // [Index("IX_ContactEmail", IsUnique = true)]
        [Index("IX_ContactEmail")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the contact image path.
        /// </summary>
        /// <value>The contact image path.</value>
        [MaxLength(200)]
        public string ContactImagePath { get; set; }

        /// <summary>
        /// Gets or sets the contact image thumb path.
        /// </summary>
        /// <value>The contact image thumb path.</value>
        [MaxLength(200)]
        public string ContactImageThumbPath { get; set; }

        /// <summary>
        /// Gets or sets the contact licenses.
        /// </summary>
        /// <value>The contact licenses.</value>
        [ForeignKey("IdContact")]
        public virtual ICollection<ContactLicense> ContactLicenses { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>The documents.</value>
        [ForeignKey("IdContact")]
        public virtual ICollection<ContactDocument> Documents { get; set; }

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
        public Employee LastModifiedByEmployee { get; set; }
        ///// <summary>
        ///// Gets or sets the last modified by.
        ///// </summary>
        ///// <value>The last modified by.</value>
        //public int? LastModifiedByCus { get; set; }

        ///// <summary>
        ///// Gets or sets the last modified by employee.
        ///// </summary>
        ///// <value>The last modified by employee.</value>
        //[ForeignKey("LastModifiedByCus")]
        //public  Customer LastModifiedByCustomer { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the fax number.
        /// </summary>
        /// <value>The fax number.</value>
        public string FaxNumber { get; set; }

        /// <summary>
        /// Gets or sets the PrimaryCompanyAddressId number.
        /// </summary>
        /// <value>The PrimaryCompanyAddressId number.</value>
        /// 
        public int? IdPrimaryCompanyAddress { get; set; }
        /// <summary>
        /// Gets or sets the IsPrimaryCompanyAddress number.
        /// </summary>
        /// <value>The PrimaryCompanyAddressId number.</value>
        /// 
        public bool? IsPrimaryCompanyAddress { get; set; }

        public bool IsActive { get; set; }
        public bool IsHidden { get; set; }
    }
}
