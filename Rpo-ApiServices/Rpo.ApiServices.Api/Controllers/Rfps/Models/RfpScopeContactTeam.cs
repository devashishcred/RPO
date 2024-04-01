// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-14-2018
// ***********************************************************************
// <copyright file="RfpScopeContactTeam.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Scope Contact Team.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    /// <summary>
    /// Class Rfp Scope Contact Team.
    /// </summary>
    public class RfpScopeContactTeam
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public string ItemName { get; set; }
    }
}