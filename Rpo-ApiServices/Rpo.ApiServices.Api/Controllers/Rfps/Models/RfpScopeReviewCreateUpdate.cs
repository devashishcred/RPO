// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-14-2018
// ***********************************************************************
// <copyright file="RfpScopeReviewCreateUpdate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Scope Review Create Update.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    /// <summary>
    /// Class Rfp Scope Review Create Update.
    /// </summary>
    public class RfpScopeReviewCreateUpdate
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

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
        public int[] ContactsCc { get; set; }
    }
}