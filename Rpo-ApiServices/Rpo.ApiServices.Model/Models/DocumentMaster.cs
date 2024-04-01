// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Krunal Pandya
// Created          : 04-17-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-17-2018
// ***********************************************************************
// <copyright file="Document.cs" company="">
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
    /// Class Document.
    /// </summary>
    public class DocumentMaster
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        /// <value>The Name.</value>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the Code
        /// </summary>
        /// <value>The Code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the Path
        /// </summary>
        /// <value>The Path.</value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        public bool? IsDevelopementCompleted { get; set; }

        public bool? IsAddPage { get; set; }

        public bool IsNewDocument { get; set; }
    }
}
