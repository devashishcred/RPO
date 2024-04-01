// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 03-10-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 03-10-2023
// ***********************************************************************
// <copyright file="NewsLetterDetail.cs" company="CREDENCYS">
//     Copyright ©  2023
// </copyright>
// <summary>Class NewsLetter Detail.</summary>
// ***********************************************************************

/// <summary>
/// The Job Time Notes namespace.
/// </summary>

namespace Rpo.ApiServices.Api.Controllers.NewsLetter.Models
{
    using System;
    using Model.Models.Enums;
    /// <summary>
    /// Class Job NewsLetter Detail.
    /// </summary>
    public class NewsLetterDetail
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>    
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        /// <value>The code.</value>

        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Image.
        /// </summary>
        /// <value>The code.</value>
        ///  public string Image { get; set; }
        public string NewsImagePath { get; set; }

        public string URL { get; set; }
        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        /// <value>The code.</value>

        public string Description { get; set; }
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
    }
}