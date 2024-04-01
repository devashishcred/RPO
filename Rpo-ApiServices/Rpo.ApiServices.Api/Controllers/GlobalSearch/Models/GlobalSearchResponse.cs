// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-30-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="GlobalSearchResponse.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Response class for the global search</summary>
// ***********************************************************************

/// <summary>
/// The GlobalSearch namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.GlobalSearch
{
    using System.Collections.Generic;

    /// <summary>
    /// Class GlobalSearchResponse.
    /// </summary>
    public class GlobalSearchResponse
    {
        /// <summary>
        /// Gets or sets the search result.
        /// </summary>
        /// <value>The search result.</value>
        public List<int> SearchResult { get; set; }
    }
}