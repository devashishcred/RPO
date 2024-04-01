// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 25-07-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 25-07-2023
// ***********************************************************************
// <copyright file="JobViolationDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Violation DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Job Violations namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobViolations
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class Job DOB Violation DTO.
    /// </summary>
    public class JobDOBViolationDTO
    {
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdJob { get; set; }
        /// <summary>
        /// Gets or sets the date issued.
        /// </summary>
        /// <value>The date issued.</value>
        ///       /// <summary>
        public DateTime? DateIssued { get; set; }
        public string Description { get; set; }
        public bool IsCOC { get; set; }
        public bool IsFullyResolved { get; set; }
       
        public DateTime? DispositionDate { get; set; }
        public string DispositionComments { get; set; }
        public string DeviceNumber { get; set; }
        public string ViolationDescription { get; set; }
        public string ECBNumber { get; set; }
        public string ViolationNumber { get; set; }
        public string ViolationCategory { get; set; }
        public string BinNumber { get; set; }
    }
}