// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Richa Patel
// Created          : 04-06-2018

// ***********************************************************************
// <copyright file="JobViolationExplanationOfCharges.cs" company="CREDENCYS">
//     Copyright ©  2018
// </copyright>
// <summary>Class Job Violation Explanation Of Charges.</summary>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rpo.ApiServices.Model.Models;

namespace Rpo.ApiServices.Model
{
    public class JobViolationExplanationOfCharges
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        public int? IdViolation { get; set; }

        [ForeignKey("IdViolation")]
        public virtual JobViolation JobViolation { get; set; }

        
        public string Code { get; set; }

        public string CodeSection { get; set; }

        public string Description { get; set; }

        public double? PaneltyAmount { get; set; }


        public bool IsFromAuth { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
