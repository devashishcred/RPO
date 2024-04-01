// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Mital Bhatt
// Created          : 19-09-2022
//
// Last Modified By :Mital Bhatt
// Last Modified On : 19-09-2022
// ***********************************************************************
// <copyright file="ChecklistAddressProperty.cs" company="CREDENCYS">
//     Copyright ©  2022
// </copyright>
// <summary>Class City.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class ChecklistAddressProperty .
    /// </summary>
    public class ChecklistAddressProperty
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
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
