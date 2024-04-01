// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-06-2018
// ***********************************************************************
// <copyright file="BisDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class BIS DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Companies
{
    using System;

    /// <summary>
    /// Class BIS DTO.
    /// </summary>
    public class BisDTO
    {
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
        /// Gets or sets the concrete testing lab number.
        /// </summary>
        /// <value>The concrete testing lab number.</value>
        public string ConcreteTestingLabNumber { get; set; }

        /// <summary>
        /// Gets or sets the concrete testing lab expiry.
        /// </summary>
        /// <value>The concrete testing lab expiry.</value>
        public DateTime? ConcreteTestingLabExpiry { get; set; }

        /// <summary>
        /// Gets or sets the general contractor expiry.
        /// </summary>
        /// <value>The general contractor expiry.</value>
        public DateTime? GeneralContractorExpiry { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [endorsements demolition].
        /// </summary>
        /// <value><c>true</c> if [endorsements demolition]; otherwise, <c>false</c>.</value>
        public bool EndorsementsDemolition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [endorsements construction].
        /// </summary>
        /// <value><c>true</c> if [endorsements construction]; otherwise, <c>false</c>.</value>
        public bool EndorsementsConstruction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [endorsements concrete].
        /// </summary>
        /// <value><c>true</c> if [endorsements concrete]; otherwise, <c>false</c>.</value>
        public bool EndorsementsConcrete { get; set; }
    }
}