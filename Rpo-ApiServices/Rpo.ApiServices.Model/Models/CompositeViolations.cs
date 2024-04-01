// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Mital Bhatt
// Created          : 20-07-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 02-20-2018
// ***********************************************************************
// <copyright file="CompositeViolations.cs" company="CREDENCYS">
//     Copyright ©  2023
// </copyright>
// <summary>Class Composite Violations.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class JobViolation.
    /// </summary>
    public class CompositeViolations
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        public int IdCompositeChecklist { get; set; }
        public int IdJobViolations { get; set; }
        [ForeignKey("IdJobViolations")]
        public JobViolation JobViolation { get; set; }
    }
}
