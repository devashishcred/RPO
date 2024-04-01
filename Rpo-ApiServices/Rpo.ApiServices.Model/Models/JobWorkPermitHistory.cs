// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Krunal Pandya
// Created          : 06-05-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 06-05-2018
// ***********************************************************************
// <copyright file="JobWorkPermitHistory.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class JobWorkPermitHistory.
    /// </summary>
    public class JobWorkPermitHistory
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier work permit.
        /// </summary>
        /// <value>The identifier work permit.</value>
        public int? IdWorkPermit { get; set; }
        
        /// <summary>
        /// Gets or sets the job application work permit types.
        /// </summary>
        /// <value>The job application work permit types.</value>
        [ForeignKey("IdWorkPermit")]
        public JobApplicationWorkPermitType JobApplicationWorkPermitTypes { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier job application.
        /// </summary>
        /// <value>The identifier job application.</value>
        public int? IdJobApplication { get; set; }
        
        /// <summary>
        /// Gets or sets the job applicationss.
        /// </summary>
        /// <value>The job applicationss.</value>
        [ForeignKey("IdJobApplication")]
        public JobApplication JobApplications { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string NewNumber { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string OldNumber { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }


    }
}
