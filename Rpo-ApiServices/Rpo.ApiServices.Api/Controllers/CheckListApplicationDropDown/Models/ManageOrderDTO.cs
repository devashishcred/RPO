using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown
{
    public class ManageOrderDTO
    {
        /// <summary>
        /// Gets or sets the JobChecklistHeaderId.
        /// </summary>
        /// <value>The identifier.</value>
        public int JobChecklistHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the Group of checklist values.
        /// </summary>
        /// <value>The identifier.</value>
        public IEnumerable<ManageOrderGroups> Groups { get; set; }      

    }
}