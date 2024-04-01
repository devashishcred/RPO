namespace Rpo.ApiServices.Api.Controllers.JobViolations
{
    public class JobViolationFullyResolvedUpdate
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        public bool IsFullyResolved { get; set; }
    }
}