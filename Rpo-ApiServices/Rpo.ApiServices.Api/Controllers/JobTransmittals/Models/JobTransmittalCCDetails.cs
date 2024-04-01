// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 03-05-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="JobTransmittalCCDetails.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Transmittal CC Details.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.JobTransmittals
{
    /// <summary>
    /// Class Job Transmittal CC Details.
    /// </summary>
    public class JobTransmittalCCDetails
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job transmittal.
        /// </summary>
        /// <value>The identifier job transmittal.</value>
        public int? IdJobTransmittal { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public string IdContact { get; set; }
        
        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>The contact.</value>
        public string Contact { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public string ItemName { get; set; }

        public bool IsContact { get; set; }
        public string IdWithType { get; set; }
    }
}