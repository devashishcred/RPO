// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-05-2018
// ***********************************************************************
// <copyright file="EmployeeGrantsDto.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Employee Grants DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Employees
{
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Employee Grants DTO.
    /// </summary>
    public class EmployeePermissionCreateOrUpdate
    {
        public int IdEmployee { get; set; }

        public int[] Permissions { get; set; }
    }
}