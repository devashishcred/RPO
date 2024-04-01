

namespace Rpo.ApiServices.Api.Controllers.Reports
{
    using System;
    using Rpo.ApiServices.Api.DataTable;
    

    public class ContractorInsurancesExpiryDataTableParameters : DataTableParameters
    {
        public DateTime DOBWCFromDate { get;  set; }

        public DateTime DOBWCToDate { get;  set; }
        public DateTime DOBGLFromDate { get; set; }

        public DateTime DOBGLToDate { get; set; }
        public DateTime DOBDIFromDate { get; set; }

        public DateTime DOBDIToDate { get; set; }


        public DateTime DOTWCFromDate { get; set; }

        public DateTime DOTWCToDate { get; set; }
        public DateTime DOTGLFromDate { get; set; }

        public DateTime DOTGLToDate { get; set; }
        public DateTime DOTSOBFromDate { get; set; }

        public DateTime DOTSOBToDate { get; set; }


        public DateTime TrackingFromDate { get; set; }

        public DateTime TrackingToDate { get; set; }
    }
}