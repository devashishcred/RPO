// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 12-14-2017
// ***********************************************************************
// <copyright file="DataTableResponse.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class for the datatable response</summary>
// ***********************************************************************

/// <summary>
/// The DataTable namespace.
/// </summary>
namespace Rpo.ApiServices.Api.DataTable
{
    /// <summary>
    /// Class DataTableResponse.
    /// </summary>
    public class DataTableResponse
    {
        /// <summary>
        /// Gets or sets the draw.
        /// </summary>
        /// <value>The draw.</value>
        public int Draw { get; set; }

        /// <summary>
        /// Gets or sets the records filtered.
        /// </summary>
        /// <value>The records filtered.</value>
        public int RecordsFiltered { get; set; }
        /// <summary>
        /// Gets or sets the records total.
        /// </summary>
        /// <value>The records total.</value>
        public int RecordsTotal { get; set; }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public object Data { get; set; }
    }
}