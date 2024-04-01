// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 01-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="JobWorkType.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Work Type.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Job Work Type.
    /// </summary>
    public class JobWorkType
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [StringLength(100)]
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }
        
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        [StringLength(100)]
        public string Code { get; set; }

        public double? Cost { get; set; }

        public int IdJobApplicationType { get; set; }

        [ForeignKey("IdJobApplicationType")]
        public JobApplicationType JobApplicationType { get; set; }

        ///// <summary>
        ///// Gets or sets the job application type identifier.
        ///// </summary>
        ///// <value>The job application type identifier.</value>
        //public int IdJobApplicationType { get; set; }

        ///// <summary>
        ///// Gets or sets the job application type identifier.
        ///// </summary>
        ///// <value>The job application type identifier.</value>
        //[ForeignKey("IdJobApplicationType")]
        //public JobApplicationType JobApplicationType { get; set; }

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

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified by employee.
        /// </summary>
        /// <value>The last modified by employee.</value>
        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }
    }
}
