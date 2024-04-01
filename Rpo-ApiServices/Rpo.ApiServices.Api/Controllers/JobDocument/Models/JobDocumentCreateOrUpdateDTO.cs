// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Krunal Pandya
// Created          : 04-18-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="JobDocumentCreateOrUpdateDTO.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


/// <summary>
/// The JobDocument namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobDocument
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class JobDocumentCreateOrUpdateDTO
    {
        /// <summary>
        /// Gets or sets the identifier job document .
        /// </summary>
        /// <value>The identifier job document.</value>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }

        /// <summary>
        /// Gets or sets the identifier document.
        /// </summary>
        /// <value>The identifier document.</value>
        public int IdDocument { get; set; }

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        /// <value>The name of the document.</value>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the job document field dt os.
        /// </summary>
        /// <value>The job document field dt os.</value>
        public ICollection<JobDocumentFieldDTO> JobDocumentFields { get; set; }

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
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Created Document Path
        /// </summary>
        public string DocumentPath { get; set; }
        /// <summary>
        /// Created By Employee Name
        /// </summary>
        public string CreatedByEmployeeName { get; set; }

        /// <summary>
        /// Created By Employee Name
        /// </summary>
        public string LastModifiedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? LastModifiedDate { get; set; }
        
        /// <summary>
        /// Get or Set DocumentCode
        /// </summary>
        public string DocumentCode { get; set; }
        
        /// <summary>
        /// Get or Set Application No
        /// </summary>
        public string ApplicationNumber { get; set; }
        
        /// <summary>
        /// Get or Set Application Type
        /// </summary>
        public string AppplicationType { get; set; }

        /// <summary>
        /// Gets or sets the copies.
        /// </summary>
        /// <value>The copies.</value>
        public int Copies { get; set; }

        /// <summary>
        /// Gets or sets the identifier job document.
        /// </summary>
        /// <value>The identifier job document.</value>
        public int IdJobDocument { get; set; }

        /// <summary>
        /// Gets or sets the identifier job application.
        /// </summary>
        /// <value>The identifier job application.</value>
        public int? IdJobApplication { get; set; }

        /// <summary>
        /// Gets or sets the type of the parent application.
        /// </summary>
        /// <value>The type of the parent application.</value>
        public string ParentApplicationType { get; set; }


        /// <summary>
        /// Gets or sets the type of the identifier parent application.
        /// </summary>
        /// <value>The type of the identifier parent application.</value>
        public int? IdParentApplicationType { get; set; }

        /// <summary>
        /// Gets or sets the job document for.
        /// </summary>
        /// <value>The job document for.</value>
        public string JobDocumentFor { get;  set; }

        /// <summary>
        /// Gets or sets the document description.
        /// </summary>
        /// <value>The document description.</value>
        public string DocumentDescription { get; set; }

        /// <summary>
        /// Gets or sets the identifier job violation.
        /// </summary>
        /// <value>The identifier job violation.</value>
        public int? IdJobViolation { get; set; }

        public int? IdJobApplicationWorkPermitType { get; set; }

        public bool? IsAddPage { get; set; }

        /// <summary>
        /// Gets or sets the AHV Reference No.
        /// </summary>
        /// <value>The job document for.</value>
        public string AHVReferenceNumber { get; set; }
        public string TrackingNumber { get; set; }

        public string PermitNumber { get; set; }
        public int? IdJobchecklistitemdetails { get; set; }
        public int? IdJobPlumbingInspections { get; set; }
        public int? IdJobChecklistGroup { get; set; }

    }
}