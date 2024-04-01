// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Krunal Pandya
// Created          : 04-18-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="JobDocumentField.cs" company="">
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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class JobDocumentField.
    /// </summary>
    public class JobDocumentField
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the identifier job document.
        /// </summary>
        /// <value>The identifier job document.</value>
        public int IdJobDocument { get; set; }

        /// <summary>
        /// Gets or sets the job document.
        /// </summary>
        /// <value>The job document.</value>
        [ForeignKey("IdJobDocument")]
        public JobDocument JobDocument { get; set; }

        /// <summary>
        /// Gets or sets the identifier document field.
        /// </summary>
        /// <value>The identifier document field.</value>
        public int IdDocumentField { get; set; }

        /// <summary>
        /// Gets or sets the document field.
        /// </summary>
        /// <value>The document field.</value>
        [ForeignKey("IdDocumentField")]
        public DocumentField DocumentField { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the ActualValue.
        /// </summary>
        /// <value>The ActualValue.</value>
        public string ActualValue { get; set; }
    }
}
