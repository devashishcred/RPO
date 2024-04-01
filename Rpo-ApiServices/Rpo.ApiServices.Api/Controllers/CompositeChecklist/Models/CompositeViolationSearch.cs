using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist.Models
{
    public class CompositeViolationSearch
    {       
        public int IdCompositeChecklist { get; set; }  
        public bool IsCOProject { get; set; }

        /// <summary>
        /// Gets or sets the searchable columns.
        /// </summary>
        /// <value>The searchable columns.</value>
        public string SearchText { get; set; }
    }
}