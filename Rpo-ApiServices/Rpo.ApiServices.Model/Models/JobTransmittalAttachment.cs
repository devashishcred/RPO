// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 02-19-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="JobTransmittalAttachment.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Transmittal Attachment.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Class Job Transmittal Attachment.
    /// </summary>
    public class JobTransmittalAttachment
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
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
        [StringLength(500)]
        public string DocumentPath { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [StringLength(500)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the copies.
        /// </summary>
        /// <value>The copies.</value>
        public int Copies { get; set; } = 1;

    }
}
