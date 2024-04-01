// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Mital Bhatt
// Created          : 08-09-2023
//
// Last Modified By : Mital Bhatt
// Last Modified On : 08-09-2023
// ***********************************************************************
// <copyright file="CustomerInvitationStatus.cs" company="CREDENCYS">
//     Copyright ©  2023
// </copyright>
// <summary>Class Address.</summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Class Address.
    /// </summary>
  public  class CustomerInvitationStatus
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier contact.
        /// </summary>
        /// <value>The type of the identifier contact.</value>
        public int IdContact { get; set; }

        /// <summary>
        /// Gets or sets the type of the contact.
        /// </summary>
        /// <value>The type of the contact.</value>
        [ForeignKey("IdContact")]
        public virtual Contact contact { get; set; }
        /// <summary>
        /// Gets or sets the type of the identifier Job.
        /// </summary>
        /// <value>The type of the identifier contact.</value>
        public int? IdJob { get; set; }

        /// <summary>
        /// Gets or sets the type of the contact.
        /// </summary>
        /// <value>The type of the contact.</value>
        [ForeignKey("IdJob")]
        public virtual Job job { get; set; }

        /// <summary>
        /// Gets or sets the CUI_Invitatuionstatus
        /// </summary>
        /// <value>The CUI_Invitatuionstatus.</value>
        public int CUI_Invitatuionstatus { get; set; }

        /// <summary>
        /// Gets or sets the CUI_Invitatuionstatus
        /// </summary>
        /// <value>The CUI_Invitatuionstatus.</value>
        public int InvitationSentCount { get; set; }
        
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ReminderCount { get; set; }
        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        /// <value>The updated date.</value>
        public DateTime? UpdatedDate { get; set; }
        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        /// <value>The updated date.</value>
        public string EmailAddress { get; set; }
    }
}
