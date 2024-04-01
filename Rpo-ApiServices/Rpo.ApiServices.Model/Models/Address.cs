// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="Address.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Address.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Address.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier address.
        /// </summary>
        /// <value>The type of the identifier address.</value>
        public int IdAddressType { get; set; }

        /// <summary>
        /// Gets or sets the type of the address.
        /// </summary>
        /// <value>The type of the address.</value>
        [ForeignKey("IdAddressType")]
        public virtual AddressType AddressType { get; set; }

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
        public int IdState { get; set; }

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
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        [MaxLength(15)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        [MaxLength(15)]
        public string Fax { get; set; }

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
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int? IdContact { get; set; }

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>The contact.</value>
        [ForeignKey("IdContact")]
        public virtual Contact Contact { get; set; }

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

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        public bool? IsMainAddress { get; set; }

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
    }
}
