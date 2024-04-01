using System;
using System.Collections.Generic;
using System.Linq;
// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Mital Bhatt
// Created          : 13-09-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 13-09-2023
// ***********************************************************************
// <copyright file="CustomerJobAccess.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Address.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   public class CustomerJobAccess
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier customer.
        /// </summary>
        /// <value>The type of the identifier customer.</value>
        public int IdCustomer { get; set; }

        /// <summary>
        /// Gets or sets the type of the customer.
        /// </summary>
        /// <value>The type of the customer.</value>
        [ForeignKey("IdCustomer")]
        public virtual Customer customer { get; set; }
        public int IdJob { get; set; }

        /// <summary>
        /// Gets or sets the type of the Job.
        /// </summary>
        /// <value>The type of the Job.</value>
        [ForeignKey("IdJob")]
        public virtual Job job { get; set; }

        /// <summary>
        /// Gets or sets the type of the CUI_Status.
        /// </summary>
        /// <value>The type of the CUI_Status.</value>
        public int CUI_Status { get; set; }

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


