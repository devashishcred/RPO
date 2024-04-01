// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-09-2018
// ***********************************************************************
// <copyright file="BoroughDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Borough Detail.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Boroughes
{
    using System;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Borough Detail.
    /// </summary>
    public class BoroughDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the BIS code.
        /// </summary>
        /// <value>The BIS code.</value>
        public int BisCode { get; set; }

        /// <summary>
        /// Gets or sets the identifier created by.
        /// </summary>
        /// <value>The identifier created by.</value>
        public int? IdCreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        public Employee CreatedByEmployee { get; set; }

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
        /// Gets or sets the identifier last modified by.
        /// </summary>
        /// <value>The identifier last modified by.</value>
        public int? IdLastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modified by employee.
        /// </summary>
        /// <value>The last modified by employee.</value>
        public Employee LastModifiedByEmployee { get; set; }

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