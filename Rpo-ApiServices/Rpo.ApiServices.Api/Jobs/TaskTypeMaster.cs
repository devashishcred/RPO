// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-07-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-07-2018
// ***********************************************************************
// <copyright file="TaskTypeMaster.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Rpo.ApiServices.Api.Jobs
/// <summary>
/// Enum TaskTypeMaster
/// </summary>
{
    public enum TaskTypeMaster
    {
        /// <summary>
        /// The after hour permits
        /// </summary>
        After_Hour_Permits = 1,

        /// <summary>
        /// The appointments
        /// </summary>
        Appointments = 2,

        /// <summary>
        /// The co copy
        /// </summary>
        CO_Copy = 3,

        /// <summary>
        /// The check plumbing sign off
        /// </summary>
        Check_Plumbing_Sign_Off = 4,

        /// <summary>
        /// The check withdrawal
        /// </summary>
        Check_Withdrawal = 5,

        /// <summary>
        /// The file job
        /// </summary>
        File_Job = 6,

        /// <summary>
        /// The get bis information
        /// </summary>
        Get_Bis_Information = 7,

        /// <summary>
        /// The landmark pickup
        /// </summary>
        Landmark_Pickup = 8,

        /// <summary>
        /// The other
        /// </summary>
        Other = 9,

        /// <summary>
        /// The permit renewal
        /// </summary>
        Permit_Renewal = 10,

        /// <summary>
        /// The post approval amendment
        /// </summary>
        Post_Approval_Amendment = 11,

        /// <summary>
        /// The prepare forms
        /// </summary>
        Prepare_Forms = 12,

        /// <summary>
        /// The pull permits
        /// </summary>
        Pull_Permits = 13,

        /// <summary>
        /// The review jobs
        /// </summary>
        Review_Jobs = 14,

        /// <summary>
        /// The sign off
        /// </summary>
        Sign_Off = 15,

        /// <summary>
        /// The update insurance
        /// </summary>
        Update_Insurance = 16,

        Proposal_Review = 17,
    }
}