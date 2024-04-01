// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-19-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-19-2018
// ***********************************************************************
// <copyright file="JobTransmittalDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Transmittal Detail</summary>
// ***********************************************************************

/// <summary>
/// The Job Transmittals namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobTransmittals
{
    using System;
    using System.Collections.Generic;
    using JobDocument;
    /// <summary>
    /// Class Job Transmittal Detail.
    /// </summary>
    public class JobTransmittalDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }

        /// <summary>
        /// Gets or sets the job number.
        /// </summary>
        /// <value>The job number.</value>
        public string JobNumber { get; set; }

        /// <summary>
        /// Gets or sets the transmittal number.
        /// </summary>
        /// <value>The transmittal number.</value>
        public string TransmittalNumber { get; set; }

        /// <summary>
        /// Gets or sets the identifier from employee.
        /// </summary>
        /// <value>The identifier from employee.</value>
        public int? IdFromEmployee { get; set; }

        /// <summary>
        /// Gets or sets from employee.
        /// </summary>
        /// <value>From employee.</value>
        public string FromEmployee { get; set; }

        /// <summary>
        /// Gets or sets the identifier to company.
        /// </summary>
        /// <value>The identifier to company.</value>
        public int? IdToCompany { get; set; }

        /// <summary>
        /// Gets or sets to company.
        /// </summary>
        /// <value>To company.</value>
        public string ToCompany { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact attention.
        /// </summary>
        /// <value>The identifier contact attention.</value>
        public int? IdContactAttention { get; set; }

        /// <summary>
        /// Gets or sets the contact attention.
        /// </summary>
        /// <value>The contact attention.</value>
        public string ContactAttention { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier transmission.
        /// </summary>
        /// <value>The type of the identifier transmission.</value>
        public int? IdTransmissionType { get; set; }

        /// <summary>
        /// Gets or sets the type of the transmission.
        /// </summary>
        /// <value>The type of the transmission.</value>
        public string TransmissionType { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier email.
        /// </summary>
        /// <value>The type of the identifier email.</value>
        public int? IdEmailType { get; set; }

        /// <summary>
        /// Gets or sets the type of the email.
        /// </summary>
        /// <value>The type of the email.</value>
        public string EmailType { get; set; }

        /// <summary>
        /// Gets or sets the sent date.
        /// </summary>
        /// <value>The sent date.</value>
        public DateTime SentDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier sent by.
        /// </summary>
        /// <value>The identifier sent by.</value>
        public int? IdSentBy { get; set; }

        /// <summary>
        /// Gets or sets the sent by.
        /// </summary>
        /// <value>The sent by.</value>
        public string SentBy { get; set; }

        /// <summary>
        /// Gets or sets the email message.
        /// </summary>
        /// <value>The email message.</value>
        public string EmailMessage { get; set; }

        /// <summary>
        /// Gets or sets the email subject.
        /// </summary>
        /// <value>The email subject.</value>
        public string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is additional atttachment.
        /// </summary>
        /// <value><c>true</c> if this instance is additional atttachment; otherwise, <c>false</c>.</value>
        public bool IsAdditionalAtttachment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is email sent.
        /// </summary>
        /// <value><c>true</c> if this instance is email sent; otherwise, <c>false</c>.</value>
        public bool IsEmailSent { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the created by employee.
        /// </summary>
        /// <value>The name of the created by employee.</value>
        public string CreatedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last name of the modified by employee.
        /// </summary>
        /// <value>The last name of the modified by employee.</value>
        public string LastModifiedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }

        ///// <summary>
        ///// Gets or sets the job transmittal c cs.
        ///// </summary>
        ///// <value>The job transmittal c cs.</value>
        //public virtual IEnumerable<JobTransmittalCCDetails> JobTransmittalCCs { get; set; }

        public List<string> JobTransmittalCCs { get; set; }

        /// <summary>
        /// Gets or sets the job transmittal attachments.
        /// </summary>
        /// <value>The job transmittal attachments.</value>
        public virtual IEnumerable<JobTransmittalAttachmentDetails> JobTransmittalAttachments { get; set; }

        /// <summary>
        /// Gets or sets the job transmittal job documents.
        /// </summary>
        /// <value>The job transmittal job documents.</value>
        public virtual IEnumerable<JobTransmittalJobDocumentDetails> JobTransmittalJobDocuments { get; set; }

        /// <summary>
        /// Gets or sets the identifier task.
        /// </summary>
        /// <value>The identifier task.</value>
        public int? IdTask { get; set; }

        /// <summary>
        /// Gets or sets the task number.
        /// </summary>
        /// <value>The task number.</value>
        public string TaskNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is draft.
        /// </summary>
        /// <value><c>null</c> if [is draft] contains no value, <c>true</c> if [is draft]; otherwise, <c>false</c>.</value>
        public bool? IsDraft { get; set; }
        public string SentTimeStamp { get; set; }
        /* As part of Update Database Columns for checklist*/
        public int? IdChecklistItem { get; set; }
        }
}