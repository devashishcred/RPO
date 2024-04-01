// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 05-22-2018
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-22-2018
// ***********************************************************************
// <copyright file="WorkPermitDocumentReadResponse.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Work Permit Document Read Response.</summary>
// ***********************************************************************


/// <summary>
/// The Models namespace.
/// </summary>
namespace Rpo.ApiServices.Api.Controllers.JobApplicationWorkPermits
{
    /// <summary>
    /// Class Work Permit Document Read Response.
    /// </summary>
    public class WorkPermitDocumentReadResponse
    {
        public int? IdJob { get; set; }

        public string JobNumber { get; set; }

        public string ContractNumber { get;  set; }

        public string DOTDescription { get;  set; }

        public string ExpiredDate { get;  set; }

        public string FromDate { get;  set; }

        public string IssuedDate { get;  set; }

        public string PermitNumber { get;  set; }

        public string PreviousPermitNumber { get;  set; }

        public string RenewalFees { get;  set; }

        public string SequenceNumber { get;  set; }

        public string StreetWorkingFrom { get;  set; }

        public string StreetWorkingOn { get;  set; }

        public string StreetWorkingTo { get;  set; }

        public string TrackingNumber { get; set; }

        public string WorkTypeCode { get; set; }

        public string EquipmentType { get; set; }

        public string ForPurposeOf { get; set; }

        public string PermitType { get;  set; }
    }
}