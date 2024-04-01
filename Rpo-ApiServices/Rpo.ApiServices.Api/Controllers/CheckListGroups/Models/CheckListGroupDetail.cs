
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 30-08-2022
//
// Last Modified By : Mital Bhatt
// Last Modified On : 20-08-2022
// ***********************************************************************
// <copyright file="CheckListGroupDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


/// <summary>
/// The States namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.CheckListGroups

{
    using System;

    /// <summary>
    /// Class StateDetail.
    /// </summary>
    public class CheckListGroupDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        /// <value>The created by.</value>

        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the Displayorder.
        /// </summary>
        /// <value>The Display Order.</value>
        public int? Displayorder1 { get; set; }
        /// <summary>
        /// Gets or sets the IsActive.
        /// </summary>
        /// <value>The IsActive.</value>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>

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

    }
}