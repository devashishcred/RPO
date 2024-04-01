using Rpo.ApiServices.Api.DataTable;
using Rpo.ApiServices.Model.Models.Enums;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class ClosedJobsWithOpenBillingDataTableParameters : DataTableParameters
    {
        public string IdJob { get; set; }

        public JobStatus Status { get; set; }
        
        public string IdCompany { get; set; }

        public string IdContact { get; set; }
    }
}