using Rpo.ApiServices.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown
{
    public class PlumbingInspectionDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        /// <value>The name.</value>
        public int IdJobPlumbingInspection { get; set; }
        public JobPlumbingInspection JobchecklispPlumbingInspection { get; set; }

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>
        public JobPlumbingInspectionComment IdJobPlumbingInspectionComment { get; set; }

        /// <summary>
        /// Gets or sets the commnets of the cheklist item.
        /// </summary>
        /// <value>The name.</value>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>The name.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the DueDate.
        /// </summary>
        /// <value>The name.</value>
        public DateTime NextInspection { get; set; }

        /// <summary>
        /// Gets or sets the WorkOrder.
        /// </summary>
        /// <value>The name.</value>
        public string WorkOrder { get; set; }



    }
}