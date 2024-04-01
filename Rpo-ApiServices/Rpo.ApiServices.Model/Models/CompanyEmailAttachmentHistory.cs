// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 02-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="CompanyEmailAttachmentHistory.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Company Email Attachment History.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class Company Email Attachment History.
    /// </summary>
    public class CompanyEmailAttachmentHistory
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier company email history.
        /// </summary>
        /// <value>The identifier company email history.</value>
        public int? IdCompanyEmailHistory { get; set; }

        /// <summary>
        /// Gets or sets the document path.
        /// </summary>
        /// <value>The document path.</value>
        [StringLength(500)]
        public string DocumentPath { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [StringLength(500)]
        public string Name { get; set; }
    }
}
