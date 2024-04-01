// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-16-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="JobContactCreateDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Contact Create DTO.</summary>
// ***********************************************************************

using System.Collections.Generic;
using Rpo.ApiServices.Model.Models;

namespace Rpo.ApiServices.Api.Controllers.JobContacts
{
    /// <summary>
    /// Class Job Contact Create DTO.
    /// </summary>
    public class JobContactCreateDTO
    {
        public int Id { get; set; }

        public int IdJob { get; set; }

        public int? IdContact { get; set; }

        public int? IdJobContactType { get; set; }

        public int? IdAddress { get; set; }

        public bool IsBilling { get; set; }

        public bool IsMainCompany { get; set; }

        public int? IdCompany { get; set; }
      
        public bool hasJobAccess { get; set; }

        public ICollection<JobContactJobContactGroup> JobContactJobContactGroups { get; set; }
    }
}