using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
    public class CompanyDocument
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Company.
        /// </summary>
        /// <value>The Company.</value>
        [ForeignKey("IdCompany")]
        public virtual Company Company { get; set; }

        /// <summary>
        /// Gets or sets the identifier Company.
        /// </summary>
        /// <value>The identifier Company.</value>
        public virtual int IdCompany { get; set; }

        /// <summary>
        /// Gets or sets the document path.
        /// </summary>
        /// <value>The document path.</value>
        [MaxLength(200)]
        public string DocumentPath { get; set; }

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
