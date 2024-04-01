

using Rpo.ApiServices.Api.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class CompositeChecklistReportDatatableParameter : DataTableParameters
    {
        
        public int IdCompositeChecklist { get; set; }
        public bool OnlyTCOItems { get; set; }
        public bool IncludeViolations { get; set; }
        public IEnumerable<ExportChecklist> lstexportChecklist { get; set; }

        //public int ChecklistIds { get; set; }        
        public int Displayorder { get; set; }

        //public string IdJob { get; set; }
        public string OrderFlag { get; set; }
        public string SearchText { get; set; }

    }

    public class ExportCompositeChecklist
    {
        public int jobChecklistHeaderId { get; set; }
        public int Displayorder { get; set; }
        public IEnumerable<ExportChecklistGroup> lstExportChecklistGroup { get; set; }
    }
    public class ExportCompositeChecklistGroup
    {
        public int jobChecklistGroupId { get; set; }
        public int displayOrder1 { get; set; }
    }
}