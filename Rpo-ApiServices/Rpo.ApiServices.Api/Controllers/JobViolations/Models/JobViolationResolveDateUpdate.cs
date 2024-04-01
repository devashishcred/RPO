using System;

namespace Rpo.ApiServices.Api.Controllers.JobViolations
{
    public class JobViolationResolveDateUpdate
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the hearing date.
        /// </summary>
        /// <value>The hearing date.</value>
        public DateTime? ResolveDate { get; set; }
    }
}