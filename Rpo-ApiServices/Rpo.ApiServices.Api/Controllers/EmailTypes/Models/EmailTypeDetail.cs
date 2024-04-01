﻿// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="EmailTypeDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Email Type Detail.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.EmailTypes
{
    using System;
    using System.Collections.Generic;
    using TransmissionTypes;

    /// <summary>
    /// Class Email Type Detail.
    /// </summary>
    public class EmailTypeDetail
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
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the email body.
        /// </summary>
        /// <value>The email body.</value>
        public string EmailBody { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is RFP.
        /// </summary>
        /// <value><c>true</c> if this instance is RFP; otherwise, <c>false</c>.</value>
        public bool IsRfp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is company.
        /// </summary>
        /// <value><c>true</c> if this instance is company; otherwise, <c>false</c>.</value>
        public bool IsCompany { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is job.
        /// </summary>
        /// <value><c>true</c> if this instance is job; otherwise, <c>false</c>.</value>
        public bool IsJob { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is contact.
        /// </summary>
        /// <value><c>true</c> if this instance is contact; otherwise, <c>false</c>.</value>
        public bool IsContact { get; set; }

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
        /// Gets or sets the default cc.
        /// </summary>
        /// <value>The default cc.</value>
        public IEnumerable<EmailTypeDefaultCCDetail> DefaultCC { get; set; }
    }
}