// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-24-2018
// ***********************************************************************
// <copyright file="AddressDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Address DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Addresses
{
    using Rpo.ApiServices.Model.Models;

    /// <summary>
    /// Class Address DTO.
    /// </summary>
    public class AddressDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier address.
        /// </summary>
        /// <value>The type of the identifier address.</value>
        public int IdAddressType { get; set; }

        /// <summary>
        /// Gets or sets the type of the address.
        /// </summary>
        /// <value>The type of the address.</value>
        public AddressType AddressType { get; set; }

        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>The address1.</value>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the address2.
        /// </summary>
        /// <value>The address2.</value>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city name.</value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state name.</value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the state of the identifier.
        /// </summary>
        /// <value>The state of the identifier.</value>
        public virtual int IdState { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>The zip code.</value>
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        public string Phone { get; set; }

        public bool? IsMainAddress { get; set; }

        /// <summary>
        /// Gets or sets the fax number.
        /// </summary>
        /// <value>The fax number.</value>
        public string FaxNumber { get; set; }

        /// <summary>
        /// Gets or sets the work number.
        /// </summary>
        /// <value>The work number.</value>
        public string WorkPhone { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        /// <value>The Email.</value>
        public string Email { get; set; }
    }
}
