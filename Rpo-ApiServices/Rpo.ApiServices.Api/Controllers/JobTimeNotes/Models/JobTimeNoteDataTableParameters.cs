// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-09-2018
// ***********************************************************************
// <copyright file="JobTimeNoteDataTableParameters.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Time Note Data Table Parameters.</summary>
// ***********************************************************************

/// <summary>
/// The Job Time Notes namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobTimeNotes
{
    using Rpo.ApiServices.Api.DataTable;

    /// <summary>
    /// Class Job Time Note Data Table Parameters.
    /// </summary>
    /// <seealso cref="Rpo.ApiServices.Api.DataTable.DataTableParameters" />
    public class JobTimeNoteDataTableParameters : DataTableParameters
    {
        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdJob { get; set; }
    }
}