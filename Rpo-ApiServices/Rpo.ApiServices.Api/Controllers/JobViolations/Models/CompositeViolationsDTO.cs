// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Mital Bhatt
// Created          : 24-07-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 24-07-2023
// ***********************************************************************
// <copyright file="JobViolationDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Violation DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Job Violations namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobViolations
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ClassComposite Violations DTO
    /// </summary>

    public class CompositeViolationsDTO
    {
        public int Id { get; set; }
        public int IdCompositeChecklist { get; set; }
        public int IdJobViolations { get; set; }
       // public  List<JobViolations> jobViolations { get; set; }
    }
}