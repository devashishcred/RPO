// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Mital Bhatt
// Created          : 03-14-2017
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="CompanyLicense.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Company License.</summary>
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
    /// Class Company License.
    /// </summary>
    public class CompanyLicense
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier Company license.
        /// </summary>
        /// <value>The type of the identifier Company license.</value>
        public virtual int IdCompanyLicenseType { get; set; }

        /// <summary>
        /// Gets or sets the type of the Company license.
        /// </summary>
        /// <value>The type of the Company license.</value>
        [ForeignKey("IdCompanyLicenseType")]
        public virtual CompanyLicenseType CompanyLicenseType { get; set; }

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
        /// Gets or sets the identifier Company.
        /// </summary>
        /// <value>The identifier Company.</value>
        public int IdCompany { get; set; }

        /// <summary>
        /// Gets or sets the Company.
        /// </summary>
        /// <value>The Company.</value>
        [ForeignKey("IdCompany")]
        public virtual Company Company { get; set; }

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
