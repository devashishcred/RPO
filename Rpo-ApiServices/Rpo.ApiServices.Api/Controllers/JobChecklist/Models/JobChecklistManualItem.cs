using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.JobChecklist
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;
    public class JobChecklistManualItem
    {
        public int IdJobChecklistGroup { get; set; }
        public ChecklistItem checklistItems { get; set; }
        public string ItemName { get; set; }
        public int IdJobPlumbingCheckListFloors { get; set; }
    }
}