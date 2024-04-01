// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Krunal Pandya
// Created          : 04-18-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-18-2018
// ***********************************************************************
// <copyright file="DocumentDetailsDTO.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Rpo.ApiServices.Api.Controllers.DocumentFields
{
    using System.Collections.Generic;
    public class DocumentDetailsDTO
    {
        /// <summary>
        /// Gets or sets the identifier document.
        /// </summary>
        /// <value>The identifier document.</value>
        public int IdDocument { get; set; }

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
        /// Gets or sets the document field dt os.
        /// </summary>
        /// <value>The document field dt os.</value>
        public IEnumerable<DocumentFieldDTO> DocumentFieldDTOs { get; set; }

    }
}