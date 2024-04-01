// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="AgentCertificate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Agent Certificate.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Agent Certificate.
    /// </summary>
    public class AgentCertificate
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        /// <value>The type of the document.</value>
        [ForeignKey("IdDocumentType")]
        public virtual DocumentType DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier document.
        /// </summary>
        /// <value>The type of the identifier document.</value>
        public virtual int IdDocumentType { get; set; }

        /// <summary>
        /// Gets or sets the number identifier.
        /// </summary>
        /// <value>The number identifier.</value>
        public string NumberId { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>The expiration date.</value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the pin.
        /// </summary>
        /// <value>The pin.</value>
        [MaxLength(10)]
        public string Pin { get; set; }

        /// <summary>
        /// Gets or sets the employee.
        /// </summary>
        /// <value>The employee.</value>
        [ForeignKey("IdEmployee")]
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets the identifier employee.
        /// </summary>
        /// <value>The identifier employee.</value>
        public virtual int IdEmployee { get; set; }

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
