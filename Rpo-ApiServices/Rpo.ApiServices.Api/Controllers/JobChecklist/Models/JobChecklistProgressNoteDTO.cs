namespace Rpo.ApiServices.Api.Controllers.ChecklistItems
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Api.Controllers.TaskNotes;
    using Model.Models.Enums;
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;
    public class JobChecklistProgressNoteDTO
    {

        public int Id { get; set; }

        public int IdJobChecklistItemDetail { get; set; }       
        public string Description { get; set; }

        public int? CreatedBy { get; set; }

        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}