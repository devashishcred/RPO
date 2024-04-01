// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-21-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="RfpProgressNoteDetails.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class RfpProgress Note Details.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.RfpReviewerTeams
{
    using System;
    
    public class RfpReviewerTeamDetails
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        public int? IdReviewer { get; set; }

        public int IdRfp { get; set; }

        public string Reviewer { get; set; }
    }
}