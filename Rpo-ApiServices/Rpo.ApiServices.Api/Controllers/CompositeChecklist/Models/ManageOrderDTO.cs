using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist.Models
{
    public class ManageOrderDTO
    {        
        /// <summary>
        /// Gets or sets the Group of checklist values.
        /// </summary>
        /// <value>The identifier.</value>
        public IEnumerable<ManageOrderHeaders> Headers { get; set; }
    }
}