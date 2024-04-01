
// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Mital Bhatt
// Created          : 26-09-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 26-09-2023
//
// ***********************************************************************
// <copyright file="CustomerJobName.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Customer JobName.</summary>
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
    public class CustomerJobName
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the type of the  ProjectName.
        /// </summary>
        /// <value>The type of the ProjectName.</value>
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets the type of the identifier customer.
        /// </summary>
        /// <value>The type of the identifier customer.</value>
         public int IdCustomerJobAccess { get; set; }

        /// <summary>
        /// Gets or sets the type of the customer.
        /// </summary>
        /// <value>The type of the customer.</value>
        [ForeignKey("IdCustomerJobAccess")]
        public virtual CustomerJobAccess customerJobAccess { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier customer.
        /// </summary>
        /// <value>The type of the identifier customer.</value>
        // public int IdCustomer { get; set; }

        ///// <summary>
        ///// Gets or sets the type of the customer.
        ///// </summary>
        ///// <value>The type of the customer.</value>
        //[ForeignKey("IdCustomer")]
        //public virtual Customer customer { get; set; }
        ///// <summary>
        ///// Gets or sets the type of the identifier customer.
        ///// </summary>
        ///// <value>The type of the identifier customer.</value>
        //public int IdJob { get; set; }

        ///// <summary>
        ///// Gets or sets the type of the Job.
        ///// </summary>
        ///// <value>The type of the Job.</value>
        //[ForeignKey("IdJob")]
        //public virtual Job job { get; set; }
        /// <summary>
        /// Gets or sets the type IsActive.
        /// </summary>
        /// <value>The type of the IsActive.</value>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }
    }
}
