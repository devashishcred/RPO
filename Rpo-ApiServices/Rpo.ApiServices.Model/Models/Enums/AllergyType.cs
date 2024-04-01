// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 05-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-21-2018
// ***********************************************************************
// <copyright file="AllergyType.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Enum Allergy Type</summary>
// ***********************************************************************

/// <summary>
/// The Enums namespace.
/// </summary>
namespace Rpo.ApiServices.Model.Models.Enums
{
    /// <summary>
    /// Enum Allergy Type
    /// </summary>
    public enum AllergyType
    {
        /// <summary>
        /// The yes allergy type
        /// </summary>
        Yes = 1,

        /// <summary>
        /// The none allergy type
        /// </summary>
        None = 2,

        /// <summary>
        /// The none provided allergy type
        /// </summary>
        NoneProvided = 3,
    }
}