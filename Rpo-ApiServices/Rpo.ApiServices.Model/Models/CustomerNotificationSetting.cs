// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Mital Bhatt
// Created          : 20-09-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 20-09-2023
// ***********************************************************************
// <copyright file="CustomerNotificationSetting.cs" company="CREDENCYS">
//     Copyright ©  2023
// </copyright>
// <summary>Class CustomerNotificationSetting.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Address.
    /// </summary>
    public class CustomerNotificationSetting
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
        [ForeignKey("IdCustomer ")]
        public virtual Customer customer { get; set; }
        /// <summary>
        /// Gets or sets the type of the identifier ProjectAccessEmail.
        /// </summary>
        /// <value>The type of the identifier ProjectAccessEmail.</value>
        public bool ProjectAccessEmail { get; set; }
        /// <summary>
        /// Gets or sets the type of the identifier ProjectAccessInApp.
        /// </summary>
        /// <value>The type of the identifier ProjectAccessInApp.</value>
        public bool ProjectAccessInApp { get; set; }
        /// <summary>
        /// Gets or sets the type of the identifier ViolationEmail.
        /// </summary>
        /// <value>The type of the identifier ViolationEmail.</value>
        public bool ViolationEmail { get; set; }
        /// <summary>
        /// Gets or sets the type of the identifier ViolationInapp.
        /// </summary>
        /// <value>The type of the identifier ViolationInapp.</value>
        public bool ViolationInapp { get; set; }



    }
}
