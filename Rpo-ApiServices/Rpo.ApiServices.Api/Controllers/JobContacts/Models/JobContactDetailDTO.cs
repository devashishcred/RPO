// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-16-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="JobContactDetailDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Contact Detail DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.JobContacts
{
    using Rpo.ApiServices.Api.Controllers.Contacts;
    using Model.Models;
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Class Job Contact Detail DTO.
    /// </summary>
    public class JobContactDetailDTO
    {
        public int Id { get;  set; }
        public int IdJob { get;  set; }
        public int? IdContact { get;  set; }
        public int? IdJobContactType { get;  set; }
        public int? IdAddress { get;  set; }
        public bool IsBilling { get;  set; }
        public bool IsMainCompany { get;  set; }
        public int? CreatedBy { get;  set; }
        public string CreatedByEmployee { get;  set; }
        public DateTime? CreatedDate { get;  set; }
        public DateTime? LastModifiedDate { get;  set; }
        public int? LastModifiedBy { get;  set; }
        public string LastModifiedByEmployee { get;  set; }
        public int? IdCompany { get;  set; }

        public List<JobContactJobContactGroupDTO> JobContactJobContactGroups { get; set; }
        public bool HasJobAccess { get; set; }
        public bool IsRegisteredCustomer { get; set; }
        
    }

    public class JobContactJobContactGroupDTO
    {
        public int Id { get; set; }

        public int IdJobContact { get; set; }

        public string JobContact { get; set; }

        public int IdJobContactGroup { get; set; }

        public string JobContactGroup { get; set; }
    }
}