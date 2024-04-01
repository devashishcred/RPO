using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist.Models
{
    public class ManageOrderHeaders
    {        
        public string CompositeChecklistName { get; set; }
        public int IdCompositeChecklist { get; set; }
        public int JobChecklistHeaderId { get; set; }
        public int IdJob { get; set; }
        public int CompositeOrder { get; set; }
    }
}