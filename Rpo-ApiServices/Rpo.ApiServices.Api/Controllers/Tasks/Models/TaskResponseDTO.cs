// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 01-24-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="TaskResponseDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Task Response DTO.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Api.Controllers.Tasks
{
    using System;

    /// <summary>
    /// Class Task Response DTO.
    /// </summary>
    public class TaskResponseDTO
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
        /// Gets or sets the identifier assigned to.
        /// </summary>
        /// <value>The identifier assigned to.</value>
        public int? IdAssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the assigned to.
        /// </summary>
        /// <value>The assigned to.</value>
        public string AssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the identifier assigned by.
        /// </summary>
        /// <value>The identifier assigned by.</value>
        public int? IdAssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the assigned by.
        /// </summary>
        /// <value>The assigned by.</value>
        public string AssignedBy { get; set; }

        /// <summary>
        /// Gets or sets the complete by.
        /// </summary>
        /// <value>The complete by.</value>
        public DateTime? CompleteBy { get; set; }

        /// <summary>
        /// Gets or sets the identifier task status.
        /// </summary>
        /// <value>The identifier task status.</value>
        public int? IdTaskStatus { get; set; }

        /// <summary>
        /// Gets or sets the status description.
        /// </summary>
        /// <value>The status description.</value>
        public string StatusDescription { get; set; }

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

        /// <summary>
        /// Gets or sets the job application.
        /// </summary>
        /// <value>The job application.</value>
        public string JobApplication { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier work permit.
        /// </summary>
        /// <value>The type of the identifier work permit.</value>
        public int? IdWorkPermitType { get; set; }

        /// <summary>
        /// Gets or sets the type of the work permit.
        /// </summary>
        /// <value>The type of the work permit.</value>
        public string WorkPermitType { get; set; }

        /// <summary>
        /// Gets or sets the progress completion note.
        /// </summary>
        /// <value>The progress completion note.</value>
        public string ProgressCompletionNote { get; set; }
        
        /// <summary>
        /// Gets or sets the type of the identifier task.
        /// </summary>
        /// <value>The type of the identifier task.</value>
        public int IdTaskType { get; set; }

        /// <summary>
        /// Gets or sets the type of the task.
        /// </summary>
        /// <value>The type of the task.</value>
        public string TaskType { get;  set; }
        
        /// <summary>
        /// Gets or sets the task status.
        /// </summary>
        /// <value>The task status.</value>
        public string TaskStatus { get; set; }
        
        /// <summary>
        /// Gets or sets the badge class.
        /// </summary>
        /// <value>The badge class.</value>
        public string BadgeClass { get; set; }
        
        /// <summary>
        /// Gets or sets the closed date.
        /// </summary>
        /// <value>The closed date.</value>
        public DateTime? ClosedDate { get;  set; }
        
        /// <summary>
        /// Gets or sets the assigned date.
        /// </summary>
        /// <value>The assigned date.</value>
        public DateTime? AssignedDate { get; set; }
    }
}