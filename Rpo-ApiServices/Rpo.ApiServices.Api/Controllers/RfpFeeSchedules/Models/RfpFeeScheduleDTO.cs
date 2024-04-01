

namespace Rpo.ApiServices.Api.Controllers.RfpFeeSchedules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class RfpFeeScheduleDTO
    {
        public string Cost { get;  set; }

        public int Id { get;  set; }

        public int IdProjectDetail { get;  set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP job.
        /// </summary>
        /// <value>The type of the identifier RFP job.</value>
        public int? IdRfpJobType { get; set; }

        /// <summary>
        /// Gets or sets the type of the RFP job.
        /// </summary>
        /// <value>The type of the RFP job.</value>
        public string RfpJobType { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier RFP sub job.
        /// </summary>
        /// <value>The type of the identifier RFP sub job.</value>
        public int? IdRfpSubJobType { get; set; }

        /// <summary>
        /// Gets or sets the type of the RFP sub job.
        /// </summary>
        /// <value>The type of the RFP sub job.</value>
        public string RfpSubJobType { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP sub job type category.
        /// </summary>
        /// <value>The identifier RFP sub job type category.</value>
        public int? IdRfpSubJobTypeCategory { get; set; }

        /// <summary>
        /// Gets or sets the RFP sub job type category.
        /// </summary>
        /// <value>The RFP sub job type category.</value>
        public string RfpSubJobTypeCategory { get; set; }

        /// <summary>
        /// Gets or sets the identifier RFP work type category.
        /// </summary>
        /// <value>The identifier RFP work type category.</value>
        public int? IdRfpServiceGroup { get; set; }

        /// <summary>
        /// Gets or sets the RFP work type category.
        /// </summary>
        /// <value>The RFP work type category.</value>
        public string RfpServiceGroup { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier RFP work type category.
        /// </summary>
        /// <value>The identifier RFP work type category.</value>
        public int? IdRfpServiceItem { get; set; }

        /// <summary>
        /// Gets or sets the RFP work type category.
        /// </summary>
        /// <value>The RFP work type category.</value>
        public string RfpServiceItem { get; set; }

        public double? Quantity { get; set; }

        public string FormatedTotalCost { get; set; }

        public double? TotalCost { get; set; }
    }
}