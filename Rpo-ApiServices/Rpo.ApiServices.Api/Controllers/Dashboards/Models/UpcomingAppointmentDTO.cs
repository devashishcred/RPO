using Rpo.ApiServices.Model.Models;
using System;

namespace Rpo.ApiServices.Api.Controllers.Dashboards
{
    public class UpcomingAppointmentDTO
    {
        public string AssignedBy { get; set; }
        public string AssignedByLastName { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToLastName { get; set; }
        public string Borough { get; set; }
        public DateTime ClosedDate { get; set; }
        public DateTime? CompleteBy { get; set; }
        public string Examiner { get; set; }
        public string GeneralNotes { get; set; }
        public string HouseNumber { get; set; }
        public int Id { get; set; }
        public int? IdAssignedBy { get; set; }
        public int? IdAssignedTo { get; set; }
        public int? IdCompany { get; set; }
        public int? IdContact { get; set; }
        public int? IdExaminer { get; set; }
        public int? IdJob { get; set; }
        public int? IdRfp { get; set; }
        public string JobAddress { get; set; }
        public string SpecialPlace { get; set; }
        public string Street { get; set; }
        public string TaskDuration { get; set; }
        public string TaskFor { get; set; }

        public virtual TaskStatus TaskStatus { get; set; }

        public string TaskNumber { get; set; }
        public string ZipCode { get; set; }
        public string JobApplicationType { get; set; }
    }
}