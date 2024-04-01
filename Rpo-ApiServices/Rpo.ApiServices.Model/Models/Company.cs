

﻿// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="Company.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Company.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Company.
    /// </summary>
    public class Company
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Index("IX_CompanyName", IsUnique = true)]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tracking number.
        /// </summary>
        /// <value>The tracking number.</value>
        [MaxLength(10)]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets the tracking expiry.
        /// </summary>
        /// <value>The tracking expiry.</value>
        public DateTime? TrackingExpiry { get; set; }

        /// <summary>
        /// Gets or sets the ibm number.
        /// </summary>
        /// <value>The ibm number.</value>
        [MaxLength(10)]
        public string IBMNumber { get; set; }

        /// <summary>
        /// Gets or sets the special inspection agency number.
        /// </summary>
        /// <value>The special inspection agency number.</value>
        [MaxLength(10)]
        public string SpecialInspectionAgencyNumber { get; set; }

        /// <summary>
        /// Gets or sets the special inspection agency expiry.
        /// </summary>
        /// <value>The special inspection agency expiry.</value>
        public DateTime? SpecialInspectionAgencyExpiry { get; set; }

        /// <summary>
        /// Gets or sets the hic number.
        /// </summary>
        /// <value>The hic number.</value>
        [MaxLength(10)]
        public string HICNumber { get; set; }

        /// <summary>
        /// Gets or sets the hic expiry.
        /// </summary>
        /// <value>The hic expiry.</value>
        public DateTime? HICExpiry { get; set; }

        /// <summary>
        /// Gets or sets the ct license number.
        /// </summary>
        /// <value>The ct license number.</value>
        public string CTLicenseNumber { get; set; }

        /// <summary>
        /// Gets or sets the ct expiration date.
        /// </summary>
        /// <value>The ct expiration date.</value>
        public DateTime? CTExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the tax identifier number.
        /// </summary>
        /// <value>The tax identifier number.</value>
        [MaxLength(9)]
        public string TaxIdNumber { get; set; }

        /// <summary>
        /// Gets or sets the insurance work compensation.
        /// </summary>
        /// <value>The insurance work compensation.</value>
        public DateTime? InsuranceWorkCompensation { get; set; }

        /// <summary>
        /// Gets or sets the insurance disability.
        /// </summary>
        /// <value>The insurance disability.</value>
        public DateTime? InsuranceDisability { get; set; }

        /// <summary>
        /// Gets or sets the insurance general liability.
        /// </summary>
        /// <value>The insurance general liability.</value>
        public DateTime? InsuranceGeneralLiability { get; set; }

        /// <summary>
        /// Gets or sets the insurance obstruction bond.
        /// </summary>
        /// <value>The insurance obstruction bond.</value>
        public DateTime? InsuranceObstructionBond { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the company types.
        /// </summary>
        /// <value>The company types.</value>
        public virtual ICollection<CompanyType> CompanyTypes { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>The addresses.</value>
        [ForeignKey("IdCompany")]
        public virtual ICollection<Address> Addresses { get; set; }

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

        ///// <summary>
        ///// Gets or sets the last modified date.
        ///// </summary>
        ///// <value>The last modified date.</value>
        //public string FaxNumber { get; set; }


        /// <summary>
        /// Gets or sets the DOT insurance WorkCompensation.
        /// </summary>
        /// <value>The DOT insurance WorkCompensation.</value>
        public DateTime? DOTInsuranceWorkCompensation { get; set; }

        /// <summary>
        /// Gets or sets the DOT insurance general liability.
        /// </summary>
        /// <value>The DOT insurance general liability.</value>
        public DateTime? DOTInsuranceGeneralLiability { get; set; }

        [ForeignKey("IdCompany")]
        public virtual ICollection<CompanyDocument> Documents { get; set; }

        [ForeignKey("IdCompany")]
        public virtual ICollection<CompanyLicense> CompanyLicenses { get; set; }


        //mb
        /// <summary>
        /// Gets or sets the EmailAddress.
        /// </summary>
        /// <value>The EmailAddress.</value>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Gets or sets the EmailPassword.
        /// </summary>
        /// <value>The EmailPassword.</value>
        public string EmailPassword { get; set; }
        public int? IDResponsibility { get; set; }
        [ForeignKey("IDResponsibility")]
        public virtual Responsibility Responsibilities { get; set; }
    }
}

