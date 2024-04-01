using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpo.ApiServices.Api.Controllers.DocumentFields;
using Rpo.ApiServices.Model.Models;

namespace Rpo.ApiServices.Api.Controllers.JobDocument.Models
{
    public class JobDocumentGetDetailsDTO
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
        /// Get or Set DocumentCode
        /// </summary>
        public string DocumentCode { get; set; }

        /// <summary>
        /// Get or Set Application No
        /// </summary>
        public string ApplicationNo { get; set; }

        /// <summary>
        /// Get or Set Application Type
        /// </summary>
        public string AppplicationType { get; set; }

        /// <summary>
        /// Job Document Field
        /// </summary>
        public ICollection<DocumentFieldDTO> JobDocumentField { get; set; }

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

        public bool? IsAddPage { get; set; }
    }
}