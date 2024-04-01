// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-20-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="TransmissionTypeCreateUpdateDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Transmission Type Create Update DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.TransmissionTypes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Transmission Type Create Update DTO.
    /// </summary>
    public class TransmissionTypeCreateUpdateDTO
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
        [MaxLength(50, ErrorMessage = "Name allow max 50 characters!")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the default cc.
        /// </summary>
        /// <value>The default cc.</value>
        public ICollection<TransmissionTypeDefaultCC> DefaultCC { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is send email.
        /// </summary>
        /// <value><c>true</c> if this instance is send email; otherwise, <c>false</c>.</value>
        public bool IsSendEmail { get; set; }
    }
}