using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.JobDocument.Models
{
    public class JobDocumentVARPMTPermit
    {
        /// <summary>
        /// Gets or sets the type of the number document.
        /// </summary>
        /// <value>The type of the number document.</value>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the detail URL.
        /// </summary>
        /// <value>The detail URL.</value>
        public string DetailUrl { get; set; }
        /// <summary>
        /// Gets or sets the Entry Date.
        /// </summary>
        /// <value>The Entry Date.</value>
        public string EntryDate { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        /// <value>The history.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the seq no.
        /// </summary>
        /// <value>The seq no.</value>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the first issue date.
        /// </summary>
        /// <value>The first issue date.</value>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the last issue date.
        /// </summary>
        /// <value>The last issue date.</value>
        public string PermissibleDaysforeRenewal { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the applicant.
        /// </summary>
        /// <value>The applicant.</value>
        public string Applicant { get; set; }

        public string PDFLink { get; set; }

        public bool isError { get; set; }

    }
}