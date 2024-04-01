// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-14-2018
// ***********************************************************************
// <copyright file="RfpProposalReviewCreateUpdate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Proposal Review Create Update.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Rfp Proposal Review Create Update.
    /// </summary>
    public class RfpProposalReviewCreateUpdate
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

        public bool? IsSignatureNewPage { get; set; }

        /// <summary>
        /// Gets or sets the RFP proposal review list.
        /// </summary>
        /// <value>The RFP proposal review list.</value>
        public List<RfpProposalReview> RfpProposalReviewList { get; set; }

        /// <summary>
        /// Gets or sets the RFP milestone list.
        /// </summary>
        /// <value>The RFP milestone list.</value>
        public List<Milestone> RfpMilestoneList { get; set; }

        /// <summary>
        /// Gets or sets the milestone services.
        /// </summary>
        /// <value>The milestone services.</value>
        public List<MilestoneService> MilestoneServices { get; set; }

        public string IdSignature { get; set; }

    }
}