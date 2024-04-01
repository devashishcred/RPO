// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Krunal Pandya
// Created          : 04-27-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-27-2018
// ***********************************************************************
// <copyright file="JobDocumentType.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Class JobDocumentType.
    /// </summary>
    public class JobDocumentType
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
       public int Id { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
       public string Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier document.
        /// </summary>
        /// <value>The identifier document.</value>
        public int? IdDocument { get; set; }

        /// <summary>
        /// Gets or sets the document master.
        /// </summary>
        /// <value>The document master.</value>
        [ForeignKey("IdDocument")]
        public DocumentMaster DocumentMaster { get; set; }
        
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
