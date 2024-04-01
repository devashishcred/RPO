using Rpo.ApiServices.Model.Models;
using System;
using System.Collections.Generic;

namespace Rpo.ApiServices.Api.Controllers.Contacts
{
    public class ContactDTODetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the personal.
        /// </summary>
        /// <value>The type of the personal.</value>
        public string PersonalType { get; set; }

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
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>The name of the middle.</value>
        public string MiddleName { get; set; }

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
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>The birth date.</value>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>The addresses.</value>
        public IEnumerable<Addresses.AddressDTO> CompanyAddresses { get; set; }
        public IEnumerable<Addresses.AddressDTO> Addresses { get; set; }

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
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the contact licenses.
        /// </summary>
        /// <value>The contact licenses.</value>
        public IEnumerable<ContactLicenseDTODetail> ContactLicenses { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }

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
        /// Gets or sets the documents.
        /// </summary>
        /// <value>The documents.</value>
        public virtual IEnumerable<ContactDocument> Documents { get; set; }

        /// <summary>
        /// Gets or sets the documents to delete.
        /// </summary>
        /// <value>The documents to delete.</value>
        public virtual IEnumerable<int> DocumentsToDelete { get; set; }

        /// <summary>
        /// Gets or sets the other phone.
        /// </summary>
        /// <value>The other phone.</value>
        public string OtherPhone { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the created by employee.
        /// </summary>
        /// <value>The name of the created by employee.</value>
        public string CreatedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the modified by employee.
        /// </summary>
        /// <value>The last name of the modified by employee.</value>
        public string LastModifiedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }
        public string Ext { get; set; }

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
        public int CUIInvitationStatus { get; set; }
        public bool IsHiddenFromCustomer { get; set; }
    }
}