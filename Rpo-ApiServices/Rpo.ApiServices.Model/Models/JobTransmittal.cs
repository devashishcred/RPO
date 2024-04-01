// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="JobTransmittal.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Transmittal.</summary>
// ***********************************************************************


namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    /// <summary>
    /// Class Job Transmittal.
    /// </summary>
    public class JobTransmittal
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }
        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        /// <value>The job.</value>
        [ForeignKey("IdJob")]
        public virtual Job Job { get; set; }

        public int? IdTask { get; set; }

        [ForeignKey("IdTask")]
        public Task Task { get; set; }

        /* As part of Update Database Columns for checklist*/
        public int? IdChecklistItem { get; set; }
        [ForeignKey("IdChecklistItem")]
        public ChecklistItem ChecklistItem { get; set; }

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
        [ForeignKey("IdFromEmployee")]
        public Employee FromEmployee { get; set; }

        /// <summary>
        /// Gets or sets the identifier to company.
        /// </summary>
        /// <value>The identifier to company.</value>
        public int? IdToCompany { get; set; }

        /// <summary>
        /// Gets or sets to company.
        /// </summary>
        /// <value>To company.</value>
        [ForeignKey("IdToCompany")]
        public Company ToCompany { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact attention.
        /// </summary>
        /// <value>The identifier contact attention.</value>
        public int? IdContactAttention { get; set; }

        /// <summary>
        /// Gets or sets the contact attention.
        /// </summary>
        /// <value>The contact attention.</value>
        [ForeignKey("IdContactAttention")]
        public Contact ContactAttention { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier transmission.
        /// </summary>
        /// <value>The type of the identifier transmission.</value>
        public int? IdTransmissionType { get; set; }

        /// <summary>
        /// Gets or sets the type of the transmission.
        /// </summary>
        /// <value>The type of the transmission.</value>
        [ForeignKey("IdTransmissionType")]
        public TransmissionType TransmissionType { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier email.
        /// </summary>
        /// <value>The type of the identifier email.</value>
        public int? IdEmailType { get; set; }

        /// <summary>
        /// Gets or sets the type of the email.
        /// </summary>
        /// <value>The type of the email.</value>
        [ForeignKey("IdEmailType")]
        public EmailType EmailType { get; set; }

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
        [ForeignKey("IdSentBy")]
        public Employee SentBy { get; set; }

        /// <summary>
        /// Gets or sets the email message.
        /// </summary>
        /// <value>The email message.</value>
        [StringLength(10000)]
        public string EmailMessage { get; set; }

        /// <summary>
        /// Gets or sets the email subject.
        /// </summary>
        /// <value>The email subject.</value>
        [StringLength(5000)]
        public string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is additional atttachment.
        /// </summary>
        /// <value><c>true</c> if this instance is additional atttachment; otherwise, <c>false</c>.</value>
        public bool IsAdditionalAtttachment { get; set; }

        /// <summary>
        /// Gets or sets the job transmittal c cs.
        /// </summary>
        /// <value>The job transmittal c cs.</value>
        [ForeignKey("IdJobTransmittal")]
        public virtual ICollection<JobTransmittalCC> JobTransmittalCCs { get; set; }

        /// <summary>
        /// Gets or sets the job transmittal attachments.
        /// </summary>
        /// <value>The job transmittal attachments.</value>
        [ForeignKey("IdJobTransmittal")]
        public virtual ICollection<JobTransmittalAttachment> JobTransmittalAttachments { get; set; }


        [ForeignKey("IdJobTransmittal")]
        public virtual ICollection<JobTransmittalJobDocument> JobTransmittalJobDocuments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is email sent.
        /// </summary>
        /// <value><c>true</c> if this instance is email sent; otherwise, <c>false</c>.</value>
        public bool IsEmailSent { get; set; }

        public bool? IsDraft { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

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
        /// Gets or sets the last modified by employee.
        /// </summary>
        /// <value>The last modified by employee.</value>
        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }
    }
}