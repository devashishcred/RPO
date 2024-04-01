using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.ChecklistItems
{
    public class ManageOrderItem
    {       
        /// <summary>
        /// Gets or sets the jocbChecklistItemDetailsId.
        /// </summary>
        /// <value>The identifier.</value>
        public int jobChecklistItemsDetailsId { get; set; }
    
        /// <summary>
        /// Gets or sets the DisplayOrder
        /// </summary>
        /// <value>The identifier.</value>
        public int DisplayOrder { get; set; }
    }
}