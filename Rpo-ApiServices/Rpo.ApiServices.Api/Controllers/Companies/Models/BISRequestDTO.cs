// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="BISRequestDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class BIS Request DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Companies
{
    /// <summary>
    /// Class BIS Request DTO.
    /// </summary>
    public class BISRequestDTO
    {
        /// <summary>
        /// Gets or sets the name of the business.
        /// </summary>
        /// <value>The name of the business.</value>
        public string BusinessName { get; set; }

        /// <summary>
        /// Gets or sets the type of the license.
        /// </summary>
        /// <value>The type of the license.</value>
        public string LicenseType { get; set; }

        /// <summary>
        /// Gets or sets the license number.
        /// </summary>
        /// <value>The license number.</value>
        public string LicenseNumber { get; set; }
    }
}