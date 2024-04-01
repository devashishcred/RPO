using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown
{
    public class ManageOrderGroups
    {
        /// <summary>
        /// Gets or sets the JobChecklistGroupId.
        /// </summary>
        /// <value>The identifier.</value>
        public int JobChecklistGroupId { get; set; }
        /// <summary>
        /// Gets or sets the checklistGroupName.
        /// </summary>
        /// <value>The name.</value>
        public string checklistGroupName { get; set; }
        /// <summary>
        /// Gets or sets the DisplayOrder
        /// </summary>
        /// <value>The identifier.</value>
        public int DisplayOrder1 { get; set; }
    }
}