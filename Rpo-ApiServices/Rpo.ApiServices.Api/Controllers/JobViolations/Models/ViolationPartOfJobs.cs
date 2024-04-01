using Rpo.ApiServices.Model.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.JobViolations.Models
    {
    public class ViolationPartOfJobs
        {
        public int JobId { get; set; }
        public string JobNumber { get; set; }
        public JobStatus Status { get; set; }
        public string StatusDescription { get; set; }
        public int IdRfpAddress { get; set; }
        public int? IdRfp { get; set; }
        public string RfpAddress { get; set; }
        public string ZipCode { get; set; }
        public int? RAIdBorough { get; set; }
        public string Borough { get; set; }
        public string HouseNumber { get; set; }
        public string StreetNumber { get; set; }
        public string FloorNumber { get; set; }
        public string Apartment { get; set; }
        public string SpecialPlace { get; set; }
        public string Block { get; set; }
        public string Lot { get; set; }
        public string BinNumber { get; set; }
        public bool HasLandMarkStatus { get; set; }
        public bool HasEnvironmentalRestriction { get; set; }
        public bool HasOpenWork { get; set; }
        public int? IdCompany { get; set; }
        public string CompanyName { get; set; }
        public int? IdContact { get; set; }
        public string ContactName { get; set; }
        public DateTime? LastModiefiedDate { get; set; }
        public int? IdJobContactType { get; set; }
        public string JobContactTypeDescription { get; set; }
        public int? IdProjectManager { get; set; }
        public string ProjectManagerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ParentStatusId { get; set; }
        public string OCMCNumber { get; set; }
        public string StreetWorkingFrom { get; set; }
        public string StreetWorkingOn { get; set; }
        public string StreetWorkingTo { get; set; }
        public string QBJobName { get; set; }
        public string JobStatusNotes { get; set; }
        }
    }