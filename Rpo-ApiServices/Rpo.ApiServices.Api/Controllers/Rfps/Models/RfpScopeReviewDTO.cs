// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-14-2018
// ***********************************************************************
// <copyright file="RfpScopeReviewDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Scope Review DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    using System.Collections.Generic;

    /// <summary>
    /// Class Rfp Scope Review DTO.
    /// </summary>
    public class RfpScopeReviewDTO
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the general notes.
        /// </summary>
        /// <value>The general notes.</value>
        public string GeneralNotes { get; set; }

        /// <summary>
        /// Gets or sets the contacts cc.
        /// </summary>
        /// <value>The contacts cc.</value>
        public string ContactsCc { get; set; }

        /// <summary>
        /// Gets or sets the contacts cc list.
        /// </summary>
        /// <value>The contacts cc list.</value>
        public IEnumerable<RfpScopeContactTeam> ContactsCcList { get; set; }
    }
}