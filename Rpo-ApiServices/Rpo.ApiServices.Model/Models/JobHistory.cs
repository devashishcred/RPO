// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Prajesh Baria
// Created          : 01-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 03-07-2018
// ***********************************************************************
// <copyright file="JobHistory.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Job History.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Rpo.ApiServices.Model.Models.Enums;

    /// <summary>
    /// Class Job History.
    /// </summary>
    public class JobHistory
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        //[StringLength(400)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the identifier employee.
        /// </summary>
        /// <value>The identifier employee.</value>
        public int? IdEmployee { get; set; }

        /// <summary>
        /// Gets or sets the employee.
        /// </summary>
        /// <value>The employee.</value>
        [ForeignKey("IdEmployee")]
        public Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets the history date.
        /// </summary>
        /// <value>The history date.</value>
        public DateTime HistoryDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the job history.
        /// </summary>
        /// <value>The type of the job history.</value>
        public JobHistoryType JobHistoryType { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }

        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        /// <value>The job.</value>
        [ForeignKey("IdJob")]
        public Job Job { get; set; }
    }
}
