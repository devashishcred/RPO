namespace Rpo.ApiServices.Api.Controllers.Tasks
{
    public class TaskStatusUpdate
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        public int IdTaskStatus { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdJob { get; set; }
    }
}