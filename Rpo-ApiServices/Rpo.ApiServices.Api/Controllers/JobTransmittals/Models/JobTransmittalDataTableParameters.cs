using Rpo.ApiServices.Api.DataTable;

namespace Rpo.ApiServices.Api.Controllers.JobTransmittals
{
    /// <summary>
    /// Class Job Transmittal DataTable Parameters.
    /// </summary>
    public class JobTransmittalDataTableParameters : DataTableParameters
    {
        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdJob { get; set; }
    }
}