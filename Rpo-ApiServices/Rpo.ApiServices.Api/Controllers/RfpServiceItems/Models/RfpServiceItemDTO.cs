// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-08-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-08-2018
// ***********************************************************************
// <copyright file="RfpSubJobTypeDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp SubJob Type DTO.</summary>
// ***********************************************************************

/// <summary>
/// The RFP Sub Job Types namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.RfpServiceItems
{
    using System;
    using System.Collections.Generic;
    using Model.Models;
    using Model.Models.Enums;
    using RfpJobTypes;
    /// <summary>
    /// Class Rfp Sub Job Type DTO.
    /// </summary>
    public class RfpServiceItemDTO
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
        /// Gets or sets the identifier RFP work type category.
        /// </summary>
        /// <value>The identifier RFP work type category.</value>
        public int? IdRfpServiceGroup { get; set; }

        /// <summary>
        /// Gets or sets the RFP work type category.
        /// </summary>
        /// <value>The RFP work type category.</value>
        public string RfpServiceGroup { get; set; }

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

        public int Level { get; set; }

        public string ServiceDescription { get; set; }

        public bool AppendWorkDescription { get; set; }

        public bool CustomServiceDescription { get; set; }

        public double? AdditionalUnitPrice { get; set; }

        public double? Cost { get; set; }

        public string FormattedCost { get; set; }

        //public int? IdCostType { get; set; }

        public RfpCostType CostType { get; set; }

        public virtual ICollection<RfpJobTypeCostRangeDTO> RfpJobTypeCostRanges { get; set; }

        public virtual ICollection<RfpJobTypeCumulativeCostDTO> RfpJobTypeCumulativeCosts { get; set; }

        public bool IsShowScope { get; set; }

        public int? PartOf { get; set; }

        public bool IsActive { get; set; }
    }
}