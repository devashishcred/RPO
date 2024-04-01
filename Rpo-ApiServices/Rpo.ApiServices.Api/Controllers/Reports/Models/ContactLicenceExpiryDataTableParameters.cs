using Rpo.ApiServices.Api.DataTable;
using System;

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    public class ContactLicenceExpiryDataTableParameters : DataTableParameters
    {
        public DateTime? ExpiresFromDate { get; set; }

        public DateTime? ExpiresToDate { get; set; }

        public string IdJob { get; set; }
    }
}