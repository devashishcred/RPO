// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Krunal Pandya
// Created          : 04-18-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="JobDocumentAttachment.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    /// <summary>
    /// Class JobDocumentAttachment.
    /// </summary>
    public class JobDocumentAttachment
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
        /// Gets or sets the name of the document.
        /// </summary>
        /// <value>The name of the document.</value>
        public string DocumentName { get; set; }
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }
    }
}
