// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-24-2018
// ***********************************************************************
// <copyright file="CompanyDTODetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Company DTO Detail.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Companies
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Company DTO Detail.
    /// </summary>
    public class CompanyDTODetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the company types.
        /// </summary>
        /// <value>The company types.</value>
        public IEnumerable<CompanyType> CompanyTypes { get; set; }

        /// <summary>
        /// Gets or sets the tracking number.
        /// </summary>
        /// <value>The tracking number.</value>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets the tracking expiry.
        /// </summary>
        /// <value>The tracking expiry.</value>
        public DateTime? TrackingExpiry { get; set; }

        /// <summary>
        /// Gets or sets the special inspection agency number.
        /// </summary>
        /// <value>The special inspection agency number.</value>
        public string SpecialInspectionAgencyNumber { get; set; }

        /// <summary>
        /// Gets or sets the special inspection agency expiry.
        /// </summary>
        /// <value>The special inspection agency expiry.</value>
        public DateTime? SpecialInspectionAgencyExpiry { get; set; }

        /// <summary>
        /// Gets or sets the ibm number.
        /// </summary>
        /// <value>The ibm number.</value>
        public string IBMNumber { get; set; }

        /// <summary>
        /// Gets or sets the hic number.
        /// </summary>
        /// <value>The hic number.</value>
        public string HICNumber { get; set; }

        /// <summary>
        /// Gets or sets the hic expiry.
        /// </summary>
        /// <value>The hic expiry.</value>
        public DateTime? HICExpiry { get; set; }

        /// <summary>
        /// Gets or sets the tax identifier number.
        /// </summary>
        /// <value>The tax identifier number.</value>
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
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>The addresses.</value>
        public virtual IEnumerable<Addresses.AddressDTO> Addresses { get; set; }

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

        ///// <summary>
        ///// Gets or sets the fax number.
        ///// </summary>
        ///// <value>The fax number.</value>
        //public string FaxNumber { get; set; }
        /// <summary>
        /// Gets or sets the DOT insurance disability.
        /// </summary>
        /// <value>The DOT insurance disability.</value>
        public DateTime? DOTInsuranceWorkCompensation { get; set; }

        /// <summary>
        /// Gets or sets the DOT insurance general liability.
        /// </summary>
        /// <value>The DOT insurance general liability.</value>
        public DateTime? DOTInsuranceGeneralLiability { get; set; }

        public List<int> IdCompanyTypes { get; set; }
        /// <summary>
        /// Gets or sets the documents.
        /// </summary>
        /// <value>The documents.</value>
        public virtual IEnumerable<CompanyDocument> Documents { get; set; }
        //internal IEnumerable<CompanyDocument> Documents;

        /// <summary>
        /// Gets or sets the documents to delete.
        /// </summary>
        /// <value>The documents to delete.</value>
        public virtual IEnumerable<int> DocumentsToDelete { get; set; }
        //mb
        /// <summary>
        /// Gets or sets the contact licenses.
        /// </summary>
        /// <value>The contact licenses.</value>
        public IEnumerable<CompanyLicenseDTODetail> CompanyLicenses { get; set; }
        /// <summary>
        /// Gets or sets the Email Address .
        /// </summary>
        /// <value>The Email Address.</value>
       //mb Email
        public string EmailAddress { get; set; }
        /// <summary>
        /// Gets or sets the Email Password .
        /// </summary>
        /// <value>The Email Password.</value>
        public string EmailPassword { get; set; }
        /// <summary>
        /// Gets or sets the Responsibility .
        /// </summary>
        /// <value>The Responsibility.</value>
        public int? IdResponsibility { get; set; }
        public string ResponsibilityName { get; set; }
        //public IEnumerable<Responsibility> Responsibilities { get; set; }

    }
}
