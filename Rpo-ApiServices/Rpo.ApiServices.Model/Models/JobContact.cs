// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="JobContact.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Contact.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Job Contact.
    /// </summary>
    public class JobContact
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
        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        /// <value>The job.</value>
        [ForeignKey("IdJob")]
        public virtual Job Job { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier company.
        /// </summary>
        /// <value>The identifier company.</value>
        public int? IdCompany { get; set; }
        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        [ForeignKey("IdCompany")]
        public virtual Company Company { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int? IdContact { get; set; }
        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>The contact.</value>
        [ForeignKey("IdContact")]
        public virtual Contact Contact { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier job contact.
        /// </summary>
        /// <value>The type of the identifier job contact.</value>
        public int? IdJobContactType { get; set; }

        /// <summary>
        /// Gets or sets the type of the job contact.
        /// </summary>
        /// <value>The type of the job contact.</value>
        [ForeignKey("IdJobContactType")]
        public JobContactType JobContactType { get; set; }

        /// <summary>
        /// Gets or sets the identifier address.
        /// </summary>
        /// <value>The identifier address.</value>
        public int? IdAddress { get; set; }
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        [ForeignKey("IdAddress")]
        public virtual Address Address { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is billing.
        /// </summary>
        /// <value><c>true</c> if this instance is billing; otherwise, <c>false</c>.</value>
        public bool IsBilling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is main company.
        /// </summary>
        /// <value><c>true</c> if this instance is main company; otherwise, <c>false</c>.</value>
        public bool IsMainCompany { get; set; }

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

        [ForeignKey("IdJobContact")]
        public virtual ICollection<JobContactJobContactGroup> JobContactJobContactGroups { get; set; }
        public bool IshiddenFromCustomer { get; set; }
    }
}