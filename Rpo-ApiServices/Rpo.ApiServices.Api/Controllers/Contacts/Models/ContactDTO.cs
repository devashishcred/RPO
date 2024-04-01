// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="ContactDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


/// <summary>
/// The Contacts namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Contacts
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class ContactDTO.
    /// </summary>
    public class ContactDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier prefix.
        /// </summary>
        /// <value>The identifier prefix.</value>
        public int? IdPrefix { get; set; }

        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        /// <value>The prefix.</value>
        public string Prefix { get; set; }

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
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the identifier company.
        /// </summary>
        /// <value>The identifier company.</value>
        public int? IdCompany { get; set; }

        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>The address1.</value>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

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
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>The zip.</value>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the work phone.
        /// </summary>
        /// <value>The work phone.</value>
        public string WorkPhone { get; set; }

        /// <summary>
        /// Gets or sets the ext.
        /// </summary>
        /// <value>The ext.</value>
        public string Ext { get; set; }

        /// <summary>
        /// Gets or sets the cell number.
        /// </summary>
        /// <value>The cell number.</value>
        public string CellNumber { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the license.
        /// </summary>
        /// <value>The license.</value>
        public string License { get; set; }

        /// <summary>
        /// Gets or sets the license number.
        /// </summary>
        /// <value>The license number.</value>
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Gets or sets the license expiration date.
        /// </summary>
        /// <value>The license expiration date.</value>
        public DateTime?  LicenseExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has image.
        /// </summary>
        /// <value><c>true</c> if this instance has image; otherwise, <c>false</c>.</value>
        public bool HasImage { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public byte[] Image { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the image thumb URL.
        /// </summary>
        /// <value>The image thumb URL.</value>
        public string ImageThumbUrl { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact title.
        /// </summary>
        /// <value>The identifier contact title.</value>
        public int? IdContactTitle { get; set; }

        /// <summary>
        /// Gets or sets the contact title.
        /// </summary>
        /// <value>The contact title.</value>
        public string ContactTitle { get; set; }
        /// <summary>
        /// Gets or sets the identifier suffix.
        /// </summary>
        /// <value>The identifier suffix.</value>
        public int? IdSuffix { get; set; }

        /// <summary>
        /// Gets or sets the suffix.
        /// </summary>
        /// <value>The suffix.</value>
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets the contact licenses.
        /// </summary>
        /// <value>The contact licenses.</value>
        public ICollection<ContactLicense> ContactLicenses { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>The addresses.</value>
        public ICollection<Address> Addresses { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the created by employee.
        /// </summary>
        /// <value>The name of the created by employee.</value>
        public string CreatedByEmployeeName { get; set; }

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
        /// Gets or sets the last name of the modified by employee.
        /// </summary>
        /// <value>The last name of the modified by employee.</value>
        public string LastModifiedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        public string OtherPhone { get; set; }

        public string WorkPhoneExt { get; set; }

        public int IdContactLicenseType { get; set; }

        public bool IsActive { get; set; }

    }
}