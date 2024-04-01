// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-24-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="TaskRequestDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Task Request DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Tasks
{
    using System;
    using System.Collections.Generic;
    using Model.Models.Enums;
    /// <summary>
    /// Class Task Request DTO.
    /// </summary>
    public class TaskRequestDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int? IdJob { get; set; }

        /// <summary>
        /// Gets or sets the assigned date.
        /// </summary>
        /// <value>The assigned date.</value>
        public DateTime? AssignedDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier assigned to.
        /// </summary>
        /// <value>The identifier assigned to.</value>
        public int? IdAssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the identifier assigned by.
        /// </summary>
        /// <value>The identifier assigned by.</value>
        public int? IdAssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the complete by.
        /// </summary>
        /// <value>The complete by.</value>
        public DateTime? CompleteBy { get; set; }

        /// <summary>
        /// Gets or sets the general notes.
        /// </summary>
        /// <value>The general notes.</value>
        public string GeneralNotes { get; set; }

        /// <summary>
        /// Gets or sets the identifier job application.
        /// </summary>
        /// <value>The identifier job application.</value>
        public int? IdJobApplication { get; set; }

        public int? IdJobViolation { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier work permit.
        /// </summary>
        /// <value>The type of the identifier work permit.</value>
        public int[] IdWorkPermitType { get; set; }

        /// <summary>
        /// Gets or sets the identifier task status.
        /// </summary>
        /// <value>The identifier task status.</value>
        public int? IdTaskStatus { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier task.
        /// </summary>
        /// <value>The type of the identifier task.</value>
        public int IdTaskType { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP.
        /// </summary>
        /// <value>The identifier RFP.</value>
        public int? IdRfp { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact.
        /// </summary>
        /// <value>The identifier contact.</value>
        public int? IdContact { get; set; }

        /// <summary>
        /// Gets or sets the identifier company.
        /// </summary>
        /// <value>The identifier company.</value>
        public int? IdCompany { get; set; }

        /// <summary>
        /// Gets or sets the identifier examiner.
        /// </summary>
        /// <value>The identifier examiner.</value>
        public int? IdExaminer { get; set; }

        /// <summary>
        /// Gets or sets the type of the job billing.
        /// </summary>
        /// <value>The type of the job billing.</value>
        public JobBillingType? JobBillingType { get; set; }

        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int? IdJobFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP job.
        /// </summary>
        /// <value>The type of the identifier RFP job.</value>
        public int? IdRfpJobType { get; set; }

        /// <summary>
        /// Gets or sets the service quantity.
        /// </summary>
        /// <value>The service quantity.</value>
        public double? ServiceQuantity { get; set; }

        /// <summary>
        /// Gets or sets the duration of the task.
        /// </summary>
        /// <value>The duration of the task.</value>
        public string TaskDuration { get; set; }


        public int? IdJobType { get; set; }

        public virtual IEnumerable<int> DocumentsToDelete { get; set; }

        public int[] IdJobDocuments { get; set; }

        public string ModuleName { get; set; }

        public int? IdMilestone { get; set; }
        public bool IsMilestone { get; set; }
        public bool IsGeneric { get; set; }
    }
}