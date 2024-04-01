// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="DataTableParameters.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class DataTable Parameters</summary>
// ***********************************************************************

/// <summary>
/// The DataTable namespace.
/// </summary>
namespace Rpo.ApiServices.Api.DataTable
{
    using System.Collections.Generic;

    /// <summary>
    /// Class Data Table Parameters.
    /// </summary>
    public class DataTableParameters
    {
        /// <summary>
        /// Gets or sets the draw.
        /// </summary>
        /// <value>The draw.</value>
        public int Draw { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>The start.</value>
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the search.
        /// </summary>
        /// <value>The search.</value>
        public string Search { get; set; }

        /// <summary>
        /// Gets or sets the ordered column.
        /// </summary>
        /// <value>The ordered column.</value>
        public OrderedColumn OrderedColumn { get; set; }

        /// <summary>
        /// Gets or sets the searchable columns.
        /// </summary>
        /// <value>The searchable columns.</value>
        public List<string> SearchableColumns { get; set; }
    }
}