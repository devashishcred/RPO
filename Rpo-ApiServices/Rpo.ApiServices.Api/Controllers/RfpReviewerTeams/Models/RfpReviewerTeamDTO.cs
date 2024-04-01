// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 02-27-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="RfpProgressNoteDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class RfpProgress Note DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.RfpReviewerTeams
{
    /// <summary>
    /// Class RfpProgress Note DTO.
    /// </summary>
    public class RfpReviewerTeamDTO
    {
        public int Id { get; set; }

        public int? IdReviewer { get; set; }

        public int IdRfp { get; set; }

        public string ItemName { get; set; }

        public string Reviewer { get; set; }
    }
}