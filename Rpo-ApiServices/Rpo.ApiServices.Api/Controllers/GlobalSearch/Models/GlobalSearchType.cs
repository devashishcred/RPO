// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-30-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="GlobalSearchType.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Enum Global Search Type</summary>
// ***********************************************************************


/// <summary>
/// The GlobalSearch namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.GlobalSearch
{
    /// <summary>
    /// Enum Global Search Type
    /// </summary>
    public enum GlobalSearchType
    {
        /// <summary>
        /// The job number
        /// </summary>
        JobNumber = 1,

        /// <summary>
        /// The application number
        /// </summary>
        ApplicationNumber = 2,

        /// <summary>
        /// The RFP number
        /// </summary>
        RFPNumber = 3,

        /// <summary>
        /// The address
        /// </summary>
        Address = 4,

        /// <summary>
        /// The contact name
        /// </summary>
        ContactName = 5,

        /// <summary>
        /// The company name
        /// </summary>
        CompanyName = 6,

        /// <summary>
        /// The special place name
        /// </summary>
        SpecialPlaceName = 7,

        /// <summary>
        /// The transmittal number
        /// </summary>
        TransmittalNumber = 8,

        ZoneDistrict = 9,

        Overlay = 10,

        TrackingNumber = 11,

        ViolationNumber = 12,
        Task = 13,
    }
}