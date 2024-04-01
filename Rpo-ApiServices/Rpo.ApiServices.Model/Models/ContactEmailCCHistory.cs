// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 02-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="ContactEmailCCHistory.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Contact Email CC History.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Contact Email CC History.
    /// </summary>
    public class ContactEmailCCHistory
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact email history.
        /// </summary>
        /// <value>The identifier contact email history.</value>
        public int? IdContactEmailHistory { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int? IdContact { get; set; }

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>The contact.</value>
        [ForeignKey("IdContact")]
        public Contact Contact { get; set; }
    }
}
