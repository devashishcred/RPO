// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="ContactLicense.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Contact License.</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Contact License.
    /// </summary>
    public class ContactLicense
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier contact license.
        /// </summary>
        /// <value>The type of the identifier contact license.</value>
        public virtual int IdContactLicenseType { get; set; }

        /// <summary>
        /// Gets or sets the type of the contact license.
        /// </summary>
        /// <value>The type of the contact license.</value>
        [ForeignKey("IdContactLicenseType")]
        public virtual ContactLicenseType ContactLicenseType { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        [MaxLength(15)]
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the expiration license date.
        /// </summary>
        /// <value>The expiration license date.</value>
        public DateTime? ExpirationLicenseDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int IdContact { get; set; }

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
    }
}
