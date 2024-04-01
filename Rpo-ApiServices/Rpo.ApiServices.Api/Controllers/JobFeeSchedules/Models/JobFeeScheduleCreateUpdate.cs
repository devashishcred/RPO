// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-14-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-14-2018
// ***********************************************************************
// <copyright file="JobFeeScheduleCreateUpdate.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Fee Schedule Create Update.</summary>
// ***********************************************************************

/// <summary>
/// The JobFeeSchedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobFeeSchedules
{
    /// <summary>
    /// Class Job Fee Schedule Create Update.
    /// </summary>
    public class JobFeeScheduleCreateUpdate
    {
        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP work.
        /// </summary>
        /// <value>The type of the identifier RFP work.</value>
        public int? IdRfpWorkType { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public double? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        public bool IsAdditionalService { get; set; } = false;

        public int? Partof { get; set; }

        public bool IsFromScope { get; set; }
        
    }
}