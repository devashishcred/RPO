using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown.Models
{
    public class ChecklistSearch
    {
        public string ChecklistIds { get; set; }

        /// <summary>
        /// Gets or sets the OrderFlag column.
        /// </summary>
        /// <value>The OrderFlag column.</value>
        public string OrderFlag { get; set; }

        /// <summary>
        /// Gets or sets the searchable columns.
        /// </summary>
        /// <value>The searchable columns.</value>
        public string SearchText { get; set; }
    }
}