using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.ChecklistItems
{
    public class ManageOrderchecklistITemDTO
    {
         
        /// <summary>
        /// Gets or sets the Group of checklist values.
        /// </summary>
        /// <value>The identifier.</value>
        public IEnumerable<ManageOrderItem> Items { get; set; }
    }
}