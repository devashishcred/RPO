// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 04-09-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 04-13-2018
// ***********************************************************************
// <copyright file="JobFeeScheduleDTO.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Job Fee Schedule DTO.</summary>
// ***********************************************************************

/// <summary>
/// The Job Fee Schedules namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobFeeSchedules
{
    using System;
    using Model.Models.Enums;
    /// <summary>
    /// Class Job Fee Schedule DTO.
    /// </summary>
    public class JobFeeScheduleDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP job.
        /// </summary>
        /// <value>The type of the identifier RFP job.</value>
        public int? IdRfpJobType { get; set; }

        /// <summary>
        /// Gets or sets the type of the RFP job.
        /// </summary>
        /// <value>The type of the RFP job.</value>
        public string RfpJobType { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP sub job.
        /// </summary>
        /// <value>The type of the identifier RFP sub job.</value>
        public int? IdRfpSubJobType { get; set; }

        /// <summary>
        /// Gets or sets the type of the RFP sub job.
        /// </summary>
        /// <value>The type of the RFP sub job.</value>
        public string RfpSubJobType { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP sub job type category.
        /// </summary>
        /// <value>The identifier RFP sub job type category.</value>
        public int? IdRfpSubJobTypeCategory { get; set; }

        /// <summary>
        /// Gets or sets the RFP sub job type category.
        /// </summary>
        /// <value>The RFP sub job type category.</value>
        public string RfpSubJobTypeCategory { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP work type category.
        /// </summary>
        /// <value>The identifier RFP work type category.</value>
        public int? IdRfpServiceGroup { get; set; }

        /// <summary>
        /// Gets or sets the RFP work type category.
        /// </summary>
        /// <value>The RFP work type category.</value>
        public string RfpServiceGroup { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP work type category.
        /// </summary>
        /// <value>The identifier RFP work type category.</value>
        public int? IdRfpServiceItem { get; set; }

        /// <summary>
        /// Gets or sets the RFP work type category.
        /// </summary>
        /// <value>The RFP work type category.</value>
        public string RfpServiceItem { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public double? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the invoiced date.
        /// </summary>
        /// <value>The invoiced date.</value>
        public DateTime? InvoicedDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP.
        /// </summary>
        /// <value>The identifier RFP.</value>
        public int? IdRfp { get; set; }

        /// <summary>
        /// Gets or sets the RFP number.
        /// </summary>
        /// <value>The RFP number.</value>
        public string RfpNumber { get; set; }

        /// <summary>
        /// Gets or sets the quantity achieved.
        /// </summary>
        /// <value>The quantity achieved.</value>
        public double? QuantityAchieved { get; set; }

        /// <summary>
        /// Gets or sets the quantity achieved.
        /// </summary>
        /// <value>The quantity achieved.</value>
        public string QuantityHours { get; set; }

        /// <summary>
        /// Gets or sets the quantity achieved.
        /// </summary>
        /// <value>The quantity achieved.</value>
        public string QuantityMinutes { get; set; }
        /// <summary>
        /// Gets or sets the quantity pending.
        /// </summary>
        /// <value>The quantity pending.</value>
        public double? QuantityPending { get; set; }

        /// <summary>
        /// Gets or sets the po number.
        /// </summary>
        /// <value>The po number.</value>
        public string PONumber { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is invoiced.
        /// </summary>
        /// <value><c>true</c> if this instance is invoiced; otherwise, <c>false</c>.</value>
        public bool IsInvoiced { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the job milestone identifier.
        /// </summary>
        /// <value>The job milestone identifier.</value>
        public int JobMilestoneId { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }

        /// <summary>
        /// Gets or sets the name of the job milestone.
        /// </summary>
        /// <value>The name of the job milestone.</value>
        public string JobMilestoneName { get; set; }

        /// <summary>
        /// Gets or sets the job milestone value.
        /// </summary>
        /// <value>The job milestone value.</value>
        public string JobMilestoneValue { get; set; }

        /// <summary>
        /// Gets or sets the job milestone status.
        /// </summary>
        /// <value>The job milestone status.</value>
        public string JobMilestoneStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [job milestone is invoiced].
        /// </summary>
        /// <value><c>true</c> if [job milestone is invoiced]; otherwise, <c>false</c>.</value>
        public bool JobMilestoneIsInvoiced { get; set; }

        /// <summary>
        /// Gets or sets the job milestone invoiced date.
        /// </summary>
        /// <value>The job milestone invoiced date.</value>
        public DateTime? JobMilestoneInvoicedDate { get; set; }

        /// <summary>
        /// Gets or sets the job milestone invoice number.
        /// </summary>
        /// <value>The job milestone invoice number.</value>
        public string JobMilestoneInvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the job milestone po number.
        /// </summary>
        /// <value>The job milestone po number.</value>
        public string JobMilestonePONumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is removed.
        /// </summary>
        /// <value><c>true</c> if this instance is removed; otherwise, <c>false</c>.</value>
        public bool IsRemoved { get; set; }

        /// <summary>
        /// Gets or sets the type of the cost.
        /// </summary>
        /// <value>The type of the cost.</value>
        public RfpCostType CostType { get; set; }

        public string Cost { get; set; }

        public string TotalCost { get; set; }

        public bool IsAdditionalService { get; set; }

        public string AllIds { get; set; }

        public int? Partof { get; set; }

        public double? TotalGroupCost { get; set; }

        public int? Orderid { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModified { get; set; }
      
        public string LastModifiedBy { get; set; }
        public string CreatedBy { get; set; }

        public bool IsShow { get; set; }

        public bool IsFromScope { get; set; }

        public int[] ServiceItemIdTask { get; set; }

        public int[] ServiceItemIdTransamittal { get; set; }

        public int[] MilestoneIdTask { get; set; }

        public int[] MilestoneIdTransamittal { get; set; }
    }
}