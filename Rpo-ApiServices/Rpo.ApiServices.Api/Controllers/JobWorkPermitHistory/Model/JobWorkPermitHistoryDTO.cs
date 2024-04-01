using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpo.ApiServices.Model.Models;

namespace Rpo.ApiServices.Api.Controllers.JobWorkPermitHistory.Model
{
    public class JobWorkPermitHistoryDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the identifier work permit.
        /// </summary>
        /// <value>The identifier work permit.</value>
        public int IdWorkPermit { get; set; }
        /// <summary>
        /// Gets or sets the job application work permit types.
        /// </summary>
        /// <value>The job application work permit types.</value>
        public JobApplicationWorkPermitType JobApplicationWorkPermitTypes { get; set; }
        /// <summary>
        /// Gets or sets the identifier job application.
        /// </summary>
        /// <value>The identifier job application.</value>
        public int IdJobApplication { get; set; }
        /// <summary>
        /// Gets or sets the job applicationss.
        /// </summary>
        /// <value>The job applicationss.</value>
        public JobApplication JobApplications { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the employee.
        /// </summary>
        /// <value>The name of the employee.</value>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }
    }
}