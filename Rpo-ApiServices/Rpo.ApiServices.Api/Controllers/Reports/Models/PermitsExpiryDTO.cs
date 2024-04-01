﻿using System;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class PermitsExpiryDTO
    {
        public string Apartment { get; set; }

        public string Borough { get; set; }

        public string Client { get; set; }

        public string Code { get; set; }

        public string Company { get; set; }

        public string CompanyResponsible { get; set; }

        public string DocumentPath { get; set; }

        public string EquipmentType { get; set; }

        public double? EstimatedCost { get; set; }

        public DateTime? Expires { get; set; }

        public DateTime? Filed { get; set; }

        public string FloorNumber { get; set; }

        public string ForPurposeOf { get; set; }

        public int Id { get; set; }

        public int? IdJob { get; set; }

        public int? IdJobApplication { get; set; }

        public int? IdJobApplicationType { get; set; }

        public int? IdJobWorkType { get; set; }

        public int? IdResponsibility { get; set; }

        public string Responsibility { get; set; }

        public DateTime? Issued { get; set; }

        public string JobAddress { get; set; }

        public string JobApplicationFloor { get; set; }

        public string JobApplicationFor { get; set; }

        public string JobApplicationNumber { get; set; }

        public string JobApplicationStatus { get; set; }

        public int? JobApplicationStatusId { get; set; }

        public string JobApplicationStreetFrom { get; set; }

        public string JobApplicationStreetTo { get; set; }

        public string JobApplicationStreetWorkingOn { get; set; }

        public string JobApplicationTypeName { get; set; }

        public string JobNumber { get; internal set; }

        public string JobWorkTypeCode { get; set; }

        public string JobWorkTypeContent { get; set; }

        public string JobWorkTypeDescription { get; set; }

        public string JobWorkTypeNumber { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string PermitCode { get; set; }

        public string PermitNumber { get; set; }

        public string PermitType { get; set; }

        public string PersonalResponsible { get; set; }

        public string PreviousPermitNumber { get; set; }

        public double? RenewalFee { get; set; }

        public DateTime? SignedOff { get; set; }

        public string SpecialPlace { get; set; }

        public DateTime? Withdrawn { get; set; }

        public string WorkDescription { get; set; }

        public DateTime? PlumbingSignedOff { get; set; }

        public DateTime? FinalElevator { get; set; }

        public DateTime? TempElevator { get; set; }

        public DateTime? ConstructionSignedOff { get; set; }

        public string Permittee { get; set; }

        public bool? IsPGL { get; set; }

        public bool L2 { get; set; }

        public int? IdJobType { get; set; }

        public string JobApplicationType { get; set; }
        public string ProjectName { get; set; }
        
    }
}