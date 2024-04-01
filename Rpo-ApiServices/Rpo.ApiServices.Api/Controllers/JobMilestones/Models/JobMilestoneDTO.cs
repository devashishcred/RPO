// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-18-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-01-2018
// ***********************************************************************
// <copyright file="JobMilestoneDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Milestone DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.JobMilestones
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Model.Models;
    
    /// <summary>
    /// Class Job Milestone DTO.
    /// </summary>
    public class JobMilestoneDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }

        public int? IdRfp { get; set; }
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

        public string FormattedValue { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is invoiced.
        /// </summary>
        /// <value><c>true</c> if this instance is invoiced; otherwise, <c>false</c>.</value>
        public bool IsInvoiced { get; set; }

        /// <summary>
        /// Gets or sets the invoiced date.
        /// </summary>
        /// <value>The invoiced date.</value>
        public DateTime? InvoicedDate { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the po number.
        /// </summary>
        /// <value>The po number.</value>
        public string PONumber { get; set; }

        /// <summary>
        /// Gets or sets the job milestone services.
        /// </summary>
        /// <value>The job milestone services.</value>
        public virtual ICollection<JobMilestoneServiceDetail> JobMilestoneServices { get; set; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public string LastModifiedBy { get; set; }
    }
}
