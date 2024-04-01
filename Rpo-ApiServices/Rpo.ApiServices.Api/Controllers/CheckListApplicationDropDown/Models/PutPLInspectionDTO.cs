using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown
{
    public class PutPLInspectionDTO
    {

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>
        public int? IdJobPlumbingInspectionCommentHistory { get; set; }
        
        /// <summary>
        /// Gets or sets the commnets of the cheklist item.
        /// </summary>
        /// <value>The name.</value>
        public string Comments { get; set; }

      
    }
}