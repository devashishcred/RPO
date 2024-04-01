// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="OrderedColumn.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class OrderedColumn.</summary>
// ***********************************************************************


/// <summary>
/// The DataTable namespace.
/// </summary>
namespace Rpo.ApiServices.Api.DataTable
{
    /// <summary>
    /// Class OrderedColumn.
    /// </summary>
    public class OrderedColumn
    {
        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>The column.</value>
        public string Column { get; set; }
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public string Dir { get; set; }
    }
}