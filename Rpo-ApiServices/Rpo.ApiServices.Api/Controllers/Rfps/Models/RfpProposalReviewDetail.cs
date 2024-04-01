// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-14-2018
// ***********************************************************************
// <copyright file="RfpProposalReviewDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Proposal Review Detail.</summary>
// ***********************************************************************

using Rpo.ApiServices.Model.Models.Enums;
/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    /// <summary>
    /// Class Rfp Proposal Review Detail.
    /// </summary>
    public class RfpProposalReviewDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP.
        /// </summary>
        /// <value>The identifier RFP.</value>
        public int IdRfp { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the identifier verbiage.
        /// </summary>
        /// <value>The identifier verbiage.</value>
        public int IdVerbiage { get; set; }

        /// <summary>
        /// Gets or sets the name of the verbiage.
        /// </summary>
        /// <value>The name of the verbiage.</value>
        public string VerbiageName { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int? DisplayOrder { get; set; }

        public VerbiageType? VerbiageType { get; set; }
    }
}