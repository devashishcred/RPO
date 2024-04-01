using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.JobChecklist
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;
    public class JobPlumbingChecklistFloorsDTO
    {
     
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the identifier address.
        /// </summary>
        /// <value>The type of the identifier address.</value>
        public int IdJobChecklistHeader { get; set; }

        public  JobChecklistHeader JobChecklistHeader { get; set; }
        public int FloorNumber { get; set; }

        public string FloorName { get; set; }
    }
}