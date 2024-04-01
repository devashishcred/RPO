// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Mital Bhatt
// Created          : 02-10-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 02-10-2023
// ***********************************************************************
// <copyright file="News.cs" company="CREDENCYS">
//     Copyright ©  2023
// </copyright>
// <summary>Class News</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class MultipleDwelling Classification.
    /// </summary>
    public class News
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
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
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }

    }
}
