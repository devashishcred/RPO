// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="JobMilestone.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Milestone.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Job Milestone.
    /// </summary>
    public class JobMilestone
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }

        public int? IdRfp { get; set; }

        [ForeignKey("IdJob")]
        public Job Job { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double? Value { get; set; }

        public string Status { get; set; }

        public bool IsInvoiced { get; set; }

        public DateTime? InvoicedDate { get; set; }

        public string InvoiceNumber { get; set; }

        public string PONumber { get; set; }

        /// <summary>
        /// Gets or sets the milestone services.
        /// </summary>
        /// <value>The milestone services.</value>
        [ForeignKey("IdMilestone")]
        public virtual ICollection<JobMilestoneService> JobMilestoneServices { get; set; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets the modified by employee.
        /// </summary>
        /// <value>The modified by employee.</value>
        [ForeignKey("ModifiedBy")]
        public Employee ModifiedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>The modified by.</value>
        public int? ModifiedBy { get; set; }

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
