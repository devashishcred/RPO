// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="CompanyAdvancedSearchParameters.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Company Advanced Search Parameters.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Companies
{
    using Rpo.ApiServices.Api.DataTable;

    /// <summary>
    /// Class Company Advanced Search Parameters.
    /// </summary>
    /// <seealso cref="DataTableParameters" />
    public class CompanyAdvancedSearchParameters : DataTableParameters
    {
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

        public string CompanyTypes { get; set; }

        public int? IdCompanyLicenseType { get; set; }
    }
}