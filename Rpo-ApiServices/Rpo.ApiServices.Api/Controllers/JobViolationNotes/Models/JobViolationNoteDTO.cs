using System;

namespace Rpo.ApiServices.Api.Controllers.JobViolationNotes
{
    public class JobViolationNoteDTO
    {
        public int Id { get; set; }

        public int IdJobViolation { get; set; }

        public string LastModified { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string Notes { get; set; }

        public int? CreatedBy { get; set; }

        public string CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}