namespace Rpo.ApiServices.Api.Controllers.JobDocument
{
    /// <summary>
    /// Class JobDocumentPermitDTO.
    /// </summary>
    public class JobDocumentPermitDTO
    {
        /// <summary>
        /// Gets or sets the type of the number document.
        /// </summary>
        /// <value>The type of the number document.</value>
        public string NumberDocType { get; set; }

        /// <summary>
        /// Gets or sets the detail URL.
        /// </summary>
        /// <value>The detail URL.</value>
        public string DetailUrl { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        /// <value>The history.</value>
        public string History { get; set; }

        /// <summary>
        /// Gets or sets the seq no.
        /// </summary>
        /// <value>The seq no.</value>
        public string SeqNo { get; set; }

        /// <summary>
        /// Gets or sets the first issue date.
        /// </summary>
        /// <value>The first issue date.</value>
        public string FirstIssueDate { get; set; }

        /// <summary>
        /// Gets or sets the last issue date.
        /// </summary>
        /// <value>The last issue date.</value>
        public string LastIssueDate { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the applicant.
        /// </summary>
        /// <value>The applicant.</value>
        public string Applicant { get; set; }

        public string DownloadLink { get; set; }
        public System.DateTime? IssuedDate { get; set; }
        public System.DateTime? ExpiredDate { get; set; }
        public string Permitee { get; set; }
        public int? JobApplicationWorkPermitTypeId { get; set; }

        public string JobApplicationNumber { get; set; }

        public string binNumber { get; set; }
        public string DocumentDescription { get; set; }

        public string ApplicationType { get; set; }

        public bool isError { get; set; }

        public int IdJob { get; set; }
        public string permit_type { get; set; }
        public string permit_subtype { get; set; }

        public string expiration_date { get; set; }
        public string issuance_date { get; set; }
        public string permittee_s_business_name { get; set; }
        public string permit_si_no { get; set; }
        
    }
}