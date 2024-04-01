// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 02-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="ContactEmailHistory.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Contact Email History.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Contact Email History.
    /// </summary>
    public class ContactEmailHistory
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int? IdContact { get; set; }

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>The contact.</value>
        [ForeignKey("IdContact")]
        public Contact Contact { get; set; }

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
        [StringLength(5000)]
        public string EmailMessage { get; set; }

        /// <summary>
        /// Gets or sets the email subject.
        /// </summary>
        /// <value>The email subject.</value>
        [StringLength(100)]
        public string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is additional atttachment.
        /// </summary>
        /// <value><c>true</c> if this instance is additional atttachment; otherwise, <c>false</c>.</value>
        public bool IsAdditionalAtttachment { get; set; }

        /// <summary>
        /// Gets or sets the contact email cc histories.
        /// </summary>
        /// <value>The contact email cc histories.</value>
        [ForeignKey("IdContactEmailHistory")]
        public virtual ICollection<ContactEmailCCHistory> ContactEmailCCHistories { get; set; }

        /// <summary>
        /// Gets or sets the contact email attachment histories.
        /// </summary>
        /// <value>The contact email attachment histories.</value>
        [ForeignKey("IdContactEmailHistory")]
        public virtual ICollection<ContactEmailAttachmentHistory> ContactEmailAttachmentHistories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is email sent.
        /// </summary>
        /// <value><c>true</c> if this instance is email sent; otherwise, <c>false</c>.</value>
        public bool IsEmailSent { get; set; }
    }
}
