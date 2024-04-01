// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-19-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-19-2018
// ***********************************************************************
// <copyright file="JobTransmittalEmailDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary> Class Job Transmittal Email DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Job Transmittals namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobTransmittals
{
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Job Transmittal Email DTO.
    /// </summary>
    public class JobTransmittalEmailDTO
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is draft.
        /// </summary>
        /// <value><c>null</c> if [is draft] contains no value, <c>true</c> if [is draft]; otherwise, <c>false</c>.</value>
        public bool? IsDraft { get; set; }

        /// <summary>
        /// Gets or sets the transmittal c cs.
        /// </summary>
        /// <value>The transmittal c cs.</value>
        public List<string> TransmittalCCs { get; set; }

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

        public int? IdTask { get; set; }

        /* As part of Update Database Columns for checklist*/
        public int? IdChecklistItem { get; set; }

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
        /// Gets or sets a value indicating whether this instance is resend.
        /// </summary>
        /// <value><c>true</c> if this instance is resend; otherwise, <c>false</c>.</value>
        public bool IsResend { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is revise.
        /// </summary>
        /// <value><c>true</c> if this instance is revise; otherwise, <c>false</c>.</value>
        public bool IsRevise { get; set; }

        /// <summary>
        /// Gets or sets the identifier old trasmittal.
        /// </summary>
        /// <value>The identifier old trasmittal.</value>
        public int? IdOldTrasmittal { get; set; }

        /// <summary>
        /// Gets or sets the job transmittal attachments.
        /// </summary>
        /// <value>The job transmittal attachments.</value>
        public ICollection<JobTransmittalAttachment> JobTransmittalAttachments { get; set; }

        /// <summary>
        /// Gets or sets the job transmittal job documents.
        /// </summary>
        /// <value>The job transmittal job documents.</value>
        public ICollection<JobTransmittalJobDocument> JobTransmittalJobDocuments { get; set; }

        /// <summary>
        /// Gets or sets the documents to delete.
        /// </summary>
        /// <value>The documents to delete.</value>
        public virtual IEnumerable<int> DocumentsToDelete { get; set; }

        public virtual IEnumerable<int> AttachmentsToDelete { get; set; }

        public string ReportDocumentName { get; set; }

        public string ReportDocumentPath { get; set; }

    }

    public class TransmittalCC
    {
        public int Id { get; set; }

        public bool IsContact { get; set; }

        public int IdGroup { get; set; }
    }
}