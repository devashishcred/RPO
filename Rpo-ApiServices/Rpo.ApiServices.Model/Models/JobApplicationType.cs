// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 01-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="JobApplicationType.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Application Type.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Job Application Type.
    /// </summary>
    public class JobApplicationType
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

        ///// <summary>
        ///// Gets or sets the number.
        ///// </summary>
        ///// <value>The number.</value>
        //[StringLength(5)]
        //public string Number { get; set; }

        /// <summary>
        /// Gets or sets the identifier parent.
        /// </summary>
        /// <value>The identifier parent.</value>
        public int? IdParent { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        [ForeignKey("IdParent")]
        public JobApplicationType Parent { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        [ForeignKey("IdParent")]
        public virtual ICollection<JobApplicationType> Children { get; set; }

        /// <summary>
        /// Gets or sets the job work types.
        /// </summary>
        /// <value>The job work types.</value>
        [ForeignKey("IdJobApplicationType")]
        public virtual ICollection<JobWorkType> JobWorkTypes { get; set; }

        ///// <summary>
        ///// Gets or sets the job work types.
        ///// </summary>
        ///// <value>The job work types.</value>
        //[ForeignKey("JobApplicationType_Id")]
        //public virtual ICollection<JobWorkType> JobWorkTypes { get; set; }


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
