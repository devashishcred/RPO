// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-21-2018
// ***********************************************************************
// <copyright file="RfpAdvancedSearchParameters.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Advanced Search Parameters.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    using Rpo.ApiServices.Api.DataTable;

    /// <summary>
    /// Class Rfp Advanced Search Parameters.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
    public class RfpAdvancedSearchParameters : DataTableParameters
    {
        /// <summary>
        /// Gets or sets the identifier company.
        /// </summary>
        /// <value>The identifier company.</value>
        public int? IdCompany { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int? IdContact { get; set; }

        /// <summary>
        /// Gets or sets the type of the global search.
        /// </summary>
        /// <value>The type of the global search.</value>
        public int? GlobalSearchType { get; set; }

        /// <summary>
        /// Gets or sets the global search text.
        /// </summary>
        /// <value>The global search text.</value>
        public string GlobalSearchText { get; set; }
    }
}