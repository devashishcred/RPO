// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-20-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="TransmissionTypeDefaultCCDetail.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Transmission Type Default CC Detail.</summary>
// ***********************************************************************

/// <summary>
/// The Transmission Types namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.TransmissionTypes
{
    /// <summary>
    /// Class Transmission Type Default CC Detail.
    /// </summary>
    public class TransmissionTypeDefaultCCDetail
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier transmission.
        /// </summary>
        /// <value>The type of the identifier transmission.</value>
        public int? IdTransmissionType { get; set; }

        /// <summary>
        /// Gets or sets the identifier employee.
        /// </summary>
        /// <value>The identifier employee.</value>
        public int? IdEmployee { get; set; }

        /// <summary>
        /// Gets or sets the employee.
        /// </summary>
        /// <value>The employee.</value>
        public string Employee { get; set; }
    }
}