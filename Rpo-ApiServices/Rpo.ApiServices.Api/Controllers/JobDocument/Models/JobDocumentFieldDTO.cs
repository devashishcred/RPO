// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Krunal Pandya
// Created          : 04-18-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="JobDocumentFieldDTO.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Rpo.ApiServices.Api.Controllers.JobDocument
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    public class JobDocumentFieldDTO
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
        /// Gets or sets the identifier document field.
        /// </summary>
        /// <value>The identifier document field.</value>
        public int IdDocumentField { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}