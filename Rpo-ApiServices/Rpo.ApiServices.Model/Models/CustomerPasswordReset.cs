// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Mital Bhatt
// Created          : 15-09-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 15-09-2023
// ***********************************************************************
// <copyright file="CustomerPasswordReset.cs" company="CREDENCYS">
//     Copyright ©  2023
// </copyright>
// <summary>Class CustomerPasswordReset.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
  public  class CustomerPasswordReset
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier IdCustomer.
        /// </summary>
        /// <value>The type of the identifier IdCustomer.</value>
        public int IdCustomer { get; set; }

        /// <summary>
        /// Gets or sets the type of the customer.
        /// </summary>
        /// <value>The type of the customer.</value>
        [ForeignKey("IdCustomer")]
        public virtual Customer customer { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier EmailAddress.
        /// </summary>
        /// <value>The type of the identifier EmailAddress.</value>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Gets or sets the type of the identifier RequestDate.
        /// </summary>
        /// <value>The type of the identifier RequestDate.</value>
        public DateTime RequestDate { get; set; }
        /// <summary>
        /// Gets or sets the type of the identifier IsPasswordchanged.
        /// </summary>
        /// <value>The type of the identifier IsPasswordchanged.</value>
        public bool IsPasswordchanged { get; set; }
        /// <summary>
        /// Gets or sets the type of the identifier PasswordChangedDate.
        /// </summary>
        /// <value>The type of the identifier PasswordChangedDate.</value>
        public DateTime PasswordChangedDate { get; set; }

    }
}
