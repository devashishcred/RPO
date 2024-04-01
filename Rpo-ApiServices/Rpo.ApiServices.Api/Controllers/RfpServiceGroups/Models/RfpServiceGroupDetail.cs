// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-08-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-29-2018
// ***********************************************************************
// <copyright file="RfpServiceGroupDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Service Group Detail.</summary>
// ***********************************************************************

/// <summary>
/// The Rfp Service Groups namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.RfpServiceGroups
{
    using System;

    /// <summary>
    /// Class Rfp Service Group Detail.
    /// </summary>
    public class RfpServiceGroupDetail
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
        /// Gets or sets the type of the identifier RFP job.
        /// </summary>
        /// <value>The type of the identifier RFP job.</value>
        public int? IdRfpJobType { get; set; }

        /// <summary>
        /// Gets or sets the type of the RFP job.
        /// </summary>
        /// <value>The type of the RFP job.</value>
        public string RfpJobType { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP sub job.
        /// </summary>
        /// <value>The type of the identifier RFP sub job.</value>
        public int? IdRfpSubJobType { get; set; }

        /// <summary>
        /// Gets or sets the type of the RFP sub job.
        /// </summary>
        /// <value>The type of the RFP sub job.</value>
        public string RfpSubJobType { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP sub job type category.
        /// </summary>
        /// <value>The identifier RFP sub job type category.</value>
        public int? IdRfpSubJobTypeCategory { get; set; }

        /// <summary>
        /// Gets or sets the RFP sub job type category.
        /// </summary>
        /// <value>The RFP sub job type category.</value>
        public string RfpSubJobTypeCategory { get; set; }

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

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int? DisplayOrder { get; set; }
    }
}