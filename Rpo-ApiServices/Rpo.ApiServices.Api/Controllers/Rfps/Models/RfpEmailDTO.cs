// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="RfpEmailDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Rfp Email DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Rfps namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Rfps
{
    /// <summary>
    /// Class Rfp Email DTO.
    /// </summary>
    public class RfpEmailDTO
    {
        /// <summary>
        /// Gets or sets the identifier contacts cc.
        /// </summary>
        /// <value>The identifier contacts cc.</value>
        public int[] IdContactsCc { get; set; }

        /// <summary>
        /// Gets or sets the identifier contacts to.
        /// </summary>
        /// <value>The identifier contacts to.</value>
        public int? IdContactsTo { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact attention.
        /// </summary>
        /// <value>The identifier contact attention.</value>
        public int IdContactAttention { get; set; }

        /// <summary>
        /// Gets or sets the identifier from employee.
        /// </summary>
        /// <value>The identifier from employee.</value>
        public int IdFromEmployee { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier email.
        /// </summary>
        /// <value>The type of the identifier email.</value>
        public int IdEmailType { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier transmission.
        /// </summary>
        /// <value>The type of the identifier transmission.</value>
        public int IdTransmissionType { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is additional atttachment.
        /// </summary>
        /// <value><c>true</c> if this instance is additional atttachment; otherwise, <c>false</c>.</value>
        public bool IsAdditionalAtttachment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is proposal PDF attached.
        /// </summary>
        /// <value><c>true</c> if this instance is proposal PDF attached; otherwise, <c>false</c>.</value>
        public bool IsProposalPDFAttached { get; set; }
    }
}