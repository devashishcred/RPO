// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Krunal Pandya
// Created          : 04-18-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="DocumentField.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Rpo.ApiServices.Api.Controllers.DocumentFields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Model.Models;
    using Model.Models.Enums;
    public class DocumentFieldDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>

        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        /// <value>The Name.</value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the Control Type
        /// </summary>
        /// <value>The Control Type.</value>
        public ControlType? ControlType { get; set; }

        /// <summary>
        /// Gets or sets the Data Type
        /// </summary>
        /// <value>The Data Type.</value>
        public FieldDataType? DataType { get; set; }


        /// <summary>
        /// Gets or sets the Document Id.
        /// </summary>
        /// <value>The Document Id.</value>
        public int? IdDocument { get; set; }

        /// <summary>
        /// Gets or sets the identifier field.
        /// </summary>
        /// <value>The identifier field.</value>
        public int? IdField { get; set; }

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
        /// Value for Post
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>The field.</value>
        public Field Field { get; set; }

        /// <summary>
        /// Gets or sets the attachment path.
        /// </summary>
        /// <value>The attachment path.</value>
        public string AttachmentPath { get; set; }

        public string DisplayFieldName { get; set; }

        public int? IdParentField { get; set; }
        public int? DisplayOrder { get;  set; }
        public string StaticDescription { get;  set; }
    }
}