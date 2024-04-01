// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-24-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-16-2018
// ***********************************************************************
// <copyright file="EnumTaskStatus.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Enum Task Status.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Tools
{
    /// <summary>
    /// Enum Task Status.
    /// </summary>
    public enum EnumTaskStatus
    {
        /// <summary>
        /// The pending
        /// </summary>
        Pending = 1,

        ///// <summary>
        ///// The in progress
        ///// </summary>
        //InProgress = 2,

        /// <summary>
        /// The completed
        /// </summary>
        Completed = 3,

        /// <summary>
        /// The unattainable
        /// </summary>
        Unattainable = 4
    }
}