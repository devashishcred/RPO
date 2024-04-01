// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Krunal Pandya
// Last Modified On : 18-04-2018
// ***********************************************************************
// <copyright file="JobDocument.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Document.</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Job Document.
    /// </summary>
    public class JobDocument
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
        public Job Job { get; set; }

        /// <summary>
        /// Gets or sets the identifier document.
        /// </summary>
        /// <value>The identifier document.</value>
        public int IdDocument { get; set; }

        [ForeignKey("IdDocument")]
        public DocumentMaster DocumentMaster { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the document description.
        /// </summary>
        /// <value>The document description.</value>
        public string DocumentDescription { get; set; }

        /// <summary>
        /// Gets or sets the document path.
        /// </summary>
        /// <value>The document path.</value>
        public string DocumentPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archived.
        /// </summary>
        /// <value><c>true</c> if this instance is archived; otherwise, <c>false</c>.</value>
        public bool IsArchived { get; set; }

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

        /// <summary>
        /// Gets or sets the job document fields.
        /// </summary>
        /// <value>The job document fields.</value>
        [ForeignKey("IdJobDocument")]
        public ICollection<JobDocumentField> JobDocumentFields { get; set; }

        /// <summary>
        /// Gets or sets the identifier document.
        /// </summary>
        /// <value>The identifier document.</value>
        public int? IdJobApplication { get; set; }

        /// <summary>
        /// Gets or sets the job application.
        /// </summary>
        /// <value>The job application.</value>
        [ForeignKey("IdJobApplication")]
        public virtual JobApplication JobApplication { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier job application work permit.
        /// </summary>
        /// <value>The type of the identifier job application work permit.</value>
        public int? IdJobApplicationWorkPermitType { get; set; }

        /// <summary>
        /// Gets or sets the type of the job application work permit.
        /// </summary>
        /// <value>The type of the job application work permit.</value>
        [ForeignKey("IdJobApplicationWorkPermitType")]
        public virtual JobApplicationWorkPermitType JobApplicationWorkPermitType { get; set; }

        /// <summary>
        /// Gets or sets the identifier job violation.
        /// </summary>
        /// <value>The identifier job violation.</value>
        public int? IdJobViolation { get; set; }

        /// <summary>
        /// Gets or sets the job violation.
        /// </summary>
        /// <value>The job violation.</value>
        [ForeignKey("IdJobViolation")]
        public virtual JobViolation JobViolation { get; set; }

        /// <summary>
        /// Gets or sets the job document for.
        /// </summary>
        /// <value>The job document for.</value>
        public string JobDocumentFor { get; set; }

        /// <summary>
        /// Gets or sets the job document attachments.
        /// </summary>
        /// <value>The job document attachments.</value>
        [ForeignKey("IdJobDocument")]
        public virtual ICollection<JobDocumentAttachment> JobDocumentAttachments { get; set; }

        /// <summary>
        /// Gets or sets the identifier parent.
        /// </summary>
        /// <value>The identifier parent.</value>
        public int? IdParent { get; set; }

        /// <summary>
        /// Gets or sets the parent job job document.
        /// </summary>
        /// <value>The parent job job document.</value>
        [ForeignKey("IdParent")]
        public virtual JobDocument ParentJobJobDocument { get; set; }

        public string TrackingNumber { get; set; }

        public string PermitNumber { get; set; }
        public int? IdJobchecklistItemDetails { get; set; }
        //[ForeignKey("IdJobchecklistItemDetails")]
        //public virtual JobChecklistItemDetail JobchecklistItemDetails { get; set; }
        public int? IdJobPlumbingInspections { get; set; }
        //[ForeignKey("IdJobPlumbingInspections")]
        //public virtual JobPlumbingInspection JobPlumbingInspections { get; set; }
    }
}