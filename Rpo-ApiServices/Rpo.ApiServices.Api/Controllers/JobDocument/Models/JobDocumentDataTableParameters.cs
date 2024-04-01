// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobDocumentDataTableParameters.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Document Data Table Parameters.</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobDocument.Models
{
    /// <summary>
    /// Class Job Document Data Table Parameters.
    /// </summary>
    public class JobDocumentDataTableParameters : DataTable.DataTableParameters
    {
        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdJob { get; set; }

        public bool isTransmittal { get; set; }
        public int? IdJobchecklistItemDetails { get; set; }
        public int? IdJobPlumbinginspections { get; set; }
    }
}