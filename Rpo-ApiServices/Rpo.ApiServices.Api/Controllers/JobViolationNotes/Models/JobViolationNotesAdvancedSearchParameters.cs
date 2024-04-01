using Rpo.ApiServices.Api.DataTable;

namespace Rpo.ApiServices.Api.Controllers.JobViolationNotes
{
    public class JobViolationNotesAdvancedSearchParameters : DataTableParameters
    {
        /// <summary>
        /// Gets or sets the identifier Job Violation.
        /// </summary>
        /// <value>The identifier Job Violation.</value>
        public int? IdJobViolation { get; set; }
    }
}