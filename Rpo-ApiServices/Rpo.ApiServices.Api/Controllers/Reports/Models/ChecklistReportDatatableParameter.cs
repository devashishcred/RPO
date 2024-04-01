
using Rpo.ApiServices.Api.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class ChecklistReportDatatableParameter : DataTableParameters
    {
        public IEnumerable<ExportChecklist> lstexportChecklist { get; set; }
       
        //public int ChecklistIds { get; set; }        
        public int Displayorder { get; set; }

        //public string IdJob { get; set; }
        public string OrderFlag { get; set; }

    }

    public class ExportChecklist
    {
        public int jobChecklistHeaderId { get; set; }
        public int Displayorder { get; set; }
        public IEnumerable<ExportChecklistGroup> lstExportChecklistGroup { get; set; }
    }
    public class ExportChecklistGroup
    {
        public int jobChecklistGroupId { get; set; }
        public int displayOrder1 { get; set; }
    }
}