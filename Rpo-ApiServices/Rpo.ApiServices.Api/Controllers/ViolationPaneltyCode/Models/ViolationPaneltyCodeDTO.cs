// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Krunal Pandya
// Created          : 05-11-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 05-11-2018
// ***********************************************************************
// <copyright file="ViolationPaneltyCodeDTO.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary>Class Violation penalty Code DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.ViolationPaneltyCode.Models
{
    using System;

    /// <summary>
    /// Class Violation penalty Code DTO.
    /// </summary>
    public class ViolationPaneltyCodeDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the penalty code.
        /// </summary>
        /// <value>The penalty code.</value>
        public string PaneltyCode { get; set; }

        /// <summary>
        /// Gets or sets the code section.
        /// </summary>
        /// <value>The code section.</value>
        public string CodeSection { get; set; }
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the created by employee.
        /// </summary>
        /// <value>The name of the created by employee.</value>
        public string CreatedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified by.
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last name of the modified by employee.
        /// </summary>
        /// <value>The last name of the modified by employee.</value>
        public string LastModifiedByEmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? LastModifiedDate { get; set; }
        
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; internal set; }
    }
}