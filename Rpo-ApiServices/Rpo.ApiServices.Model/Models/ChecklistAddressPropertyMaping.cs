// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Mital Bhatt
// Created          : 19-09-2022
//
// Last Modified By :Mital Bhatt
// Last Modified On : 19-09-2022
// ***********************************************************************
// <copyright file="ChecklistAddressMaping.cs" company="CREDENCYS">
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
    public class ChecklistAddressPropertyMaping
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }
        public int? IdChecklistAddressProperty { get; set; }
        [ForeignKey("IdChecklistAddressProperty")]
        public virtual ChecklistAddressProperty ChecklistAddressProperty { get; set; }              
        public int? IdChecklistItem { get; set; }
        /// <summary>
        /// Gets or sets the ChecklistItem.
        /// </summary>
        /// <value>The ChecklistItem.</value>
        [ForeignKey("IdChecklistItem")]
        public virtual ChecklistItem ChecklistItem { get; set; }        
        public string Value { get; set; }
       public bool IsActive { get; set; }


    }
}
