// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="EmailType.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Email Type.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Email Type.
    /// </summary>
    public class EmailType
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [MaxLength(50, ErrorMessage = "Name allow max 50 characters!")]
        [Index("IX_EmailTypeName", IsUnique = true)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        [MaxLength(200, ErrorMessage = "Subject allow max 200 characters!")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the email body.
        /// </summary>
        /// <value>The email body.</value>
        //[MaxLength(10000, ErrorMessage = "Email body allow max 10000 characters!")]
        public string EmailBody { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [MaxLength(1000, ErrorMessage = "Description allow max 1000 characters!")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is RFP.
        /// </summary>
        /// <value><c>true</c> if this instance is RFP; otherwise, <c>false</c>.</value>
        public bool IsRfp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is company.
        /// </summary>
        /// <value><c>true</c> if this instance is company; otherwise, <c>false</c>.</value>
        public bool IsCompany { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is job.
        /// </summary>
        /// <value><c>true</c> if this instance is job; otherwise, <c>false</c>.</value>
        public bool IsJob { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is contact.
        /// </summary>
        /// <value><c>true</c> if this instance is contact; otherwise, <c>false</c>.</value>
        public bool IsContact { get; set; }

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

        [ForeignKey("IdEamilType")]
        public ICollection<TransmissionTypeDefaultCC> DefaultCC { get; set; }
    }
}
