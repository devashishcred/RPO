using Rpo.ApiServices.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.JobChecklist.Models
{
    public class JobPlumbingInspectionProgressNoteDTO
    {
        public int Id { get; set; }

        public int IdJobPlumbingInspection { get; set; }
        public string Description { get; set; }

        public int? CreatedBy { get; set; }

        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}