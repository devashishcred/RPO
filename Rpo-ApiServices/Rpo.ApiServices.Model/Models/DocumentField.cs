// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Krunal Pandya
// Created          : 04-17-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-17-2018
// ***********************************************************************
// <copyright file="DocumentFields.cs" company="">
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class DocumentField
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Document Id.
        /// </summary>
        /// <value>The Document Id.</value>
        public int? IdDocument { get; set; }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>The document.</value>
        [ForeignKey("IdDocument")]
        public DocumentMaster Document { get; set; }

        /// <summary>
        /// Gets or sets the identifier field.
        /// </summary>
        /// <value>The identifier field.</value>
        public int? IdField { get; set; }

        /// <summary>
        /// Gets or sets the field master.
        /// </summary>
        /// <value>The field master.</value>
        [ForeignKey("IdField")]
        public Field Field { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value><c>true</c> if this instance is required; otherwise, <c>false</c>.</value>
        public bool? IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public int? Length { get; set; }

        /// <summary>
        /// Gets or sets the API URL.
        /// </summary>
        /// <value>The API URL.</value>
        public string APIUrl { get; set; }

        /// <summary>
        /// Gets or sets the Document Field List.
        /// </summary>
        /// <value>The Document Field List..</value>
        public string PdfFields { get; set; }

        /// <summary>
        /// Gets or sets the static description.
        /// </summary>
        /// <value>The static description.</value>
        public string StaticDescription { get; set; }

        /// <summary>
        /// Gets or sets the is place holder.
        /// </summary>
        /// <value>The is place holder.</value>
        public bool IsPlaceHolder { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public string DefaultValue { get; set; }

        public int? IdParentField { get; set; }

        public int? DisplayOrder { get; set; }
    }
}
