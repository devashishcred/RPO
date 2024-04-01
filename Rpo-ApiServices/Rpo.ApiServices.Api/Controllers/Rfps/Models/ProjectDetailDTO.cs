// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-14-2018
// ***********************************************************************
// <copyright file="ProjectDetailDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Project Detail DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class Project Detail DTO.
    /// </summary>
    public class ProjectDetailDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the work description.
        /// </summary>
        /// <value>The work description.</value>
        public string WorkDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [are plans not prepared].
        /// </summary>
        /// <value><c>true</c> if [are plans not prepared]; otherwise, <c>false</c>.</value>
        public bool ArePlansNotPrepared { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [are plans completed].
        /// </summary>
        /// <value><c>true</c> if [are plans completed]; otherwise, <c>false</c>.</value>
        public bool ArePlansCompleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is approved.
        /// </summary>
        /// <value><c>true</c> if this instance is approved; otherwise, <c>false</c>.</value>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is disaproved.
        /// </summary>
        /// <value><c>true</c> if this instance is disaproved; otherwise, <c>false</c>.</value>
        public bool IsDisaproved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is permitted.
        /// </summary>
        /// <value><c>true</c> if this instance is permitted; otherwise, <c>false</c>.</value>
        public bool IsPermitted { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP.
        /// </summary>
        /// <value>The identifier RFP.</value>
        public int IdRfp { get; set; }

        /// <summary>
        /// Gets or sets the RFP fee schedules.
        /// </summary>
        /// <value>The RFP fee schedules.</value>
        public virtual IEnumerable<RfpFeeScheduleDTO> RfpFeeSchedules { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP job.
        /// </summary>
        /// <value>The type of the identifier RFP job.</value>
        public int? IdRfpJobType { get; set; }

        public int? duplicateId { get; set; }
        /// <summary>
        /// Gets or sets the type of the RFP job.
        /// </summary>
        /// <value>The type of the RFP job.</value>
        public string RfpJobType { get; set; }

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
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the approved job number.
        /// </summary>
        /// <value>The approved job number.</value>
        [StringLength(9)]
        public string ApprovedJobNumber { get; set; }

        /// <summary>
        /// Gets or sets the dis approved job number.
        /// </summary>
        /// <value>The dis approved job number.</value>
        [StringLength(9)]
        public string DisApprovedJobNumber { get; set; }

        /// <summary>
        /// Gets or sets the permitted job number.
        /// </summary>
        /// <value>The permitted job number.</value>
        [StringLength(9)]
        public string PermittedJobNumber { get; set; }
    }
}