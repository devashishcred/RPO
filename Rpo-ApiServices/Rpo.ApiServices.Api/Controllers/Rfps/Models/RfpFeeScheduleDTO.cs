// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-14-2018
// ***********************************************************************
// <copyright file="RfpFeeScheduleDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp FeeSchedule DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    /// <summary>
    /// Class Rfp FeeSchedule DTO.
    /// </summary>
    public class RfpFeeScheduleDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier project detail.
        /// </summary>
        /// <value>The identifier project detail.</value>
        public int IdProjectDetail { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP work type category.
        /// </summary>
        /// <value>The identifier RFP work type category.</value>
        public int? IdRfpWorkTypeCategory { get; set; }

        /// <summary>
        /// Gets or sets the RFP work type category.
        /// </summary>
        /// <value>The RFP work type category.</value>
        public string RfpWorkTypeCategory { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP work.
        /// </summary>
        /// <value>The type of the identifier RFP work.</value>
        public int? IdRfpWorkType { get; set; }

        /// <summary>
        /// Gets or sets the type of the RFP work.
        /// </summary>
        /// <value>The type of the RFP work.</value>
        public string RfpWorkType { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public double? Cost { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public double? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the total cost.
        /// </summary>
        /// <value>The total cost.</value>
        public double? TotalCost { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RfpFeeScheduleDTO"/> is checked.
        /// </summary>
        /// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
        public bool Checked { get; set; }
    }
}