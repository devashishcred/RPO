// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-27-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 02-27-2018
// ***********************************************************************
// <copyright file="JobProjectTeam.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Project Team.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Jobs
{
    /// <summary>
    /// Class Job Project Team.
    /// </summary>
    public class JobProjectTeam
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public string ItemName { get; set; }
        /// <summary>
        /// Gets or sets the Email Address.
        /// </summary>
        /// <value>The Email Address.</value>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Gets or sets the Email Address.
        /// </summary>
        /// <value>The Email Address.</value>
        public string ProjectNumberLink { get; set; }
        public string ContactLink { get; set; }
        
    }
}