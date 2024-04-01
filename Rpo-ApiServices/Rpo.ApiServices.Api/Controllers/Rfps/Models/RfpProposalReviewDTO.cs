// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-14-2018
// ***********************************************************************
// <copyright file="RfpProposalReviewDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Proposal Review DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    using System.Collections.Generic;

    /// <summary>
    /// Class Rfp Proposal Review DTO.
    /// </summary>
    public class RfpProposalReviewDTO
    {
        /// <summary>
        /// Gets or sets the identifier RFP.
        /// </summary>
        /// <value>The identifier RFP.</value>
        public int IdRfp { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public double Cost { get; set; }

        /// <summary>
        /// Gets or sets the calculated cost.
        /// </summary>
        /// <value>The calculated cost.</value>
        public double CalculatedCost { get; set; }

        /// <summary>
        /// Gets or sets the RFP proposal review list.
        /// </summary>
        /// <value>The RFP proposal review list.</value>
        public List<RfpProposalReviewDetail> RfpProposalReviewList { get; set; }

        /// <summary>
        /// Gets or sets the RFP milestone list.
        /// </summary>
        /// <value>The RFP milestone list.</value>
        public List<MilestoneDetail> RfpMilestoneList { get; set; }

        public bool? IsSignatureNewPage { get; set; }
        public string IdSignature { get; set; }
    }
}