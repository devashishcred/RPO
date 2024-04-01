using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.JobFeeSchedules
{
    public class JobFeeSchedulePONumber
    {
        /// <summary>
        /// Gets or sets the identifier job fee schedule.
        /// </summary>
        /// <value>The identifier job fee schedule.</value>
        public int IdJobFeeSchedule { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        public string PONumber { get; set; }
    }
}