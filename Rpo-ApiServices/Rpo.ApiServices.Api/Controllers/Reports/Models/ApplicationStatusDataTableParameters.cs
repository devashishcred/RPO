using System;
using Rpo.ApiServices.Api.DataTable;
using Rpo.ApiServices.Model.Models.Enums;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class ApplicationStatusDataTableParameters : DataTableParameters
    {
        public DateTime? FiledOnFromDate { get; set; }

        public DateTime? FiledOnToDate { get; set; }

        public string IdJob { get; set; }

        public string IdProjectManager { get; set; }

        public DateTime? IssuedFromDate { get; set; }

        public DateTime? IssuedToDate { get; set; }

        public DateTime? SignedOffFromDate { get; set; }

        public DateTime? SignedOffToDate { get; set; }

        public string IdApplicationType { get; set; }

        public string IdContact { get; set; }

        public string IdApplicationStatus { get; set; }

        public string IdCompany { get; set; }

        public bool? HasLandMarkStatus { get; set; }

        public string Description { get; set; }

        public JobStatus? JobStatus { get; set; }

    }
}