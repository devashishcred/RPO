// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="JobTransmittalAttachmentDetails.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Transmittal Attachment Details.</summary>
// ***********************************************************************

/// <summary>
/// The Job Transmittals namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobTransmittals
{
    /// <summary>
    /// Class Job Transmittal Attachment Details.
    /// </summary>
    public class JobTransmittalAttachmentDetails
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
        /// Gets or sets the document path.
        /// </summary>
        /// <value>The document path.</value>
        public string DocumentPath { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
    }
}