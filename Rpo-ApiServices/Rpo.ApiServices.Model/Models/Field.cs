// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Krunal Pandya
// Created          : 04-17-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-17-2018
// ***********************************************************************
// <copyright file="FieldMaster.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpo.ApiServices.Model.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
    public class Field
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
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the Control Type
        /// </summary>
        /// <value>The Control Type.</value>
        public ControlType ControlType { get; set; }

        /// <summary>
        /// Gets or sets the Data Type
        /// </summary>
        /// <value>The Data Type.</value>
        public FieldDataType DataType { get; set; }

        /// <summary>
        /// Gets or sets the is display in frontend.
        /// </summary>
        /// <value>The is display in frontend.</value>
        public bool IsDisplayInFrontend { get; set; }

        /// <summary>
        /// Gets or sets the display name of the field.
        /// </summary>
        /// <value>The display name of the field.</value>
        public string DisplayFieldName { get; set; }

        public int? IdDocument { get; set; }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>The document.</value>
        [ForeignKey("IdDocument")]
        public DocumentMaster Document { get; set; }

    }
}
