﻿
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DOBPenaltySchedule
    {
        [Key]
        public int Id { get; set; }

        public string SectionOfLaw { get; set; }

        public string Classification { get; set; }

        public string InfractionCode { get; set; }

        public string ViolationDescription { get; set; }

        public bool Cure { get; set; }

        public bool Stipulation { get; set; }

        public double? StandardPenalty { get; set; }

        public bool MitigatedPenalty { get; set; }

        public double? DefaultPenalty { get; set; }

        public double? AggravatedPenalty_I { get; set; }

        public double? AggravatedDefaultPenalty_I { get; set; }

        public double? AggravatedPenalty_II { get; set; }

        public double? AggravatedDefaultMaxPenalty_II { get; set; }

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
