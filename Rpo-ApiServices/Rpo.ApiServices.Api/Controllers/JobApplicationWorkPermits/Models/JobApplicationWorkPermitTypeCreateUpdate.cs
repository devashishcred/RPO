using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.JobApplicationWorkPermits
{
    public class JobApplicationWorkPermitTypeCreateUpdate
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the estimated cost.
        /// </summary>
        /// <value>The estimated cost.</value>
        public double? EstimatedCost { get; set; }

        /// <summary>
        /// Gets or sets the withdrawn.
        /// </summary>
        /// <value>The withdrawn.</value>
        public DateTime? Withdrawn { get; set; }

        /// <summary>
        /// Gets or sets the filed.
        /// </summary>
        /// <value>The filed.</value>
        public DateTime? Filed { get; set; }

        /// <summary>
        /// Gets or sets the issued.
        /// </summary>
        /// <value>The issued.</value>
        public DateTime? Issued { get; set; }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Gets or sets the signed off.
        /// </summary>
        /// <value>The signed off.</value>
        public DateTime? SignedOff { get; set; }

        /// <summary>
        /// Gets or sets the work description.
        /// </summary>
        /// <value>The work description.</value>
        public string WorkDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is person responsible.
        /// </summary>
        /// <value><c>true</c> if this instance is person responsible; otherwise, <c>false</c>.</value>
        public bool IsPersonResponsible { get; set; }

        /// <summary>
        /// Gets or sets the identifier contact responsible.
        /// </summary>
        /// <value>The identifier contact responsible.</value>
        public int? IdContactResponsible { get; set; }

        /// <summary>
        /// Gets or sets the identifier responsibility.
        /// </summary>
        /// <value>The identifier responsibility.</value>
        public int? IdResponsibility { get; set; }

        /// <summary>
        /// Gets or sets the document path.
        /// </summary>
        /// <value>The document path.</value>
        public string DocumentPath { get; set; }
        /// <summary>
        /// Gets or sets the permit number.
        /// </summary>
        /// <value>The permit number.</value>
        public string PermitNumber { get; set; }
        /// <summary>
        /// Gets or sets the previous permit number.
        /// </summary>
        /// <value>The previous permit number.</value>
        public string PreviousPermitNumber { get; set; }

        /// <summary>
        /// Gets or sets the renewal fee.
        /// </summary>
        /// <value>The renewal fee.</value>
        public double? RenewalFee { get; set; }

        /// <summary>
        /// Gets or sets the contract number.
        /// </summary>
        /// <value>The contract number.</value>
        public string ContractNumber { get; set; }

        /// <summary>
        /// Gets or sets the street working from.
        /// </summary>
        /// <value>The street working from.</value>
        public string StreetWorkingFrom { get; set; }
        /// <summary>
        /// Gets or sets the street working on.
        /// </summary>
        /// <value>The street working on.</value>
        public string StreetWorkingOn { get; set; }

        /// <summary>
        /// Gets or sets the street working to.
        /// </summary>
        /// <value>The street working to.</value>
        public string StreetWorkingTo { get; set; }

        /// <summary>
        /// Gets or sets the tracking number.
        /// </summary>
        /// <value>The tracking number.</value>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets the identifier job.
        /// </summary>
        /// <value>The identifier job.</value>
        public int IdJob { get; set; }

        /// <summary>
        /// Gets or sets the work type code.
        /// </summary>
        /// <value>The work type code.</value>
        public string WorkTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the permit.
        /// </summary>
        /// <value>The type of the permit.</value>
        public string PermitType { get; set; }
        /// <summary>
        /// Gets or sets for purpose of.
        /// </summary>
        /// <value>For purpose of.</value>
        public string ForPurposeOf { get; set; }
        /// <summary>
        /// Gets or sets the type of the equipment.
        /// </summary>
        /// <value>The type of the equipment.</value>
        public string EquipmentType { get; set; }
        public bool HasSuperintendentofconstruction { get; set; }
        public bool HasSiteSafetyCoordinator { get; set; }
        public bool HasSiteSafetyManager { get; set; }
    }
}