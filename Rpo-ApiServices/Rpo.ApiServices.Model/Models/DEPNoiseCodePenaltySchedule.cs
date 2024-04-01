
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DEPNoiseCodePenaltySchedule
    {
        [Key]
        public int Id { get; set; }

        public string SectionOfLaw { get; set; }

        public string ViolationDescription { get; set; }

        public string Compliance { get; set; }

        public string Offense_1 { get; set; }

        public double? Penalty_1 { get; set; }

        public double? DefaultPenalty_1 { get; set; }

        public bool Stipulation_1 { get; set; }

        public string Offense_2 { get; set; }

        public double? Penalty_2 { get; set; }

        public double? DefaultPenalty_2 { get; set; }

        public bool Stipulation_2 { get; set; }

        public string Offense_3 { get; set; }

        public double? Penalty_3 { get; set; }

        public double? DefaultPenalty_3 { get; set; }

        public bool Stipulation_3 { get; set; }

        public string Offense_4 { get; set; }

        public double? Penalty_4 { get; set; }

        public double? DefaultPenalty_4 { get; set; }

        public bool Stipulation_4 { get; set; }

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
