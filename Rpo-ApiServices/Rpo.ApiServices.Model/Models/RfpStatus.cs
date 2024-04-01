// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 02-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-05-2018
// ***********************************************************************
// <copyright file="RfpStatus.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Class RfpStatus.
    /// </summary>
    public class RfpStatus
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
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [MaxLength(50)]
        public string DisplayName { get; set; }

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
