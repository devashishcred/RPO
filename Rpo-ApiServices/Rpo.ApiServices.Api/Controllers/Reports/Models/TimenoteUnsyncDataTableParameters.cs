using Rpo.ApiServices.Api.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.Reports.Models
{
    public class TimenoteUnsyncDataTableParameters: DataTableParameters
    {
        public bool IsQuickbookSynced { get; set; }
    }
}