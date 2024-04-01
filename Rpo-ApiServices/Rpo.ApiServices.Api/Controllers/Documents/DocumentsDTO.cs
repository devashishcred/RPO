// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Krunal Pandya
// Created          : 04-17-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-17-2018
// ***********************************************************************
// <copyright file="DocumentsDTO.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The Documents namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.Documents
{
    /// <summary>
    /// Class Documents DTO.
    /// </summary>
    public class DocumentsDTO
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
        public string DocumentName { get; set; }


        /// <summary>
        /// Gets or sets the Item Name
        /// </summary>
        /// <value>The Item Name.</value>
        public string ItemName { get; set; }

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

        public int DisplayOrder { get; set; }
    }
}