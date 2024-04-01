// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobTransmittalJobDocumentDetails.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Transmittal Job Document Details.</summary>
// ***********************************************************************

/// <summary>
/// The Job Transmittals namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobTransmittals
{
    using System;

    /// <summary>
    /// Class Job Transmittal Job Document Details.
    /// </summary>
    public class JobTransmittalJobDocumentDetails
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job transmittal.
        /// </summary>
        /// <value>The identifier job transmittal.</value>
        public int? IdJobTransmittal { get; set; }

        /// <summary>
        /// Gets or sets the identifier job document.
        /// </summary>
        /// <value>The identifier job document.</value>
        public int? IdJobDocument { get; set; }

        /// <summary>
        /// Gets or sets the document path.
        /// </summary>
        /// <value>The document path.</value>
        public string DocumentPath { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the copies.
        /// </summary>
        /// <value>The copies.</value>
        public int Copies { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the last name of the modified by employee.
        /// </summary>
        /// <value>The last name of the modified by employee.</value>
        public string LastModifiedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the created by employee.
        /// </summary>
        /// <value>The name of the created by employee.</value>
        public string CreatedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the application number.
        /// </summary>
        /// <value>The application number.</value>
        public string ApplicationNumber { get; set; }

        /// <summary>
        /// Gets or sets the type of the appplication.
        /// </summary>
        /// <value>The type of the appplication.</value>
        public string AppplicationType { get; set; }

        /// <summary>
        /// Gets or sets the identifier document.
        /// </summary>
        /// <value>The identifier document.</value>
        public int IdDocument { get; set; }

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        /// <value>The name of the document.</value>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the document code.
        /// </summary>
        /// <value>The document code.</value>
        public string DocumentCode { get; set; }
        public string DocumentDescription { get; internal set; }
    }
}