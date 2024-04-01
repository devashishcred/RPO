using System;
using Rpo.ApiServices.Api.DataTable;
using Rpo.ApiServices.Model.Models.Enums;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class PermitsExpiryDataTableParameters : DataTableParameters
    {
        public string IdJob { get; set; }

        public string IdProjectManager { get; set; }

        public string IdBorough { get; set; }

        public int? IdJobType { get; set; }

        public JobStatus Status { get; set; }

        public string PermitCode { get; set; }

        public string IdCompany { get; set; }

        public string IdContact { get; set; }

         //public int? IdResponsibility { get; set; }
         public string IdResponsibility { get; set; }

        public DateTime? ExpiresFromDate { get; set; }

        public DateTime? ExpiresToDate { get; set; }
    }
}