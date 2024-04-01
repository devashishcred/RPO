using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist.Models
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;
    public class ListofGeneralChecklist
    {
        public int IdJobCheckListHeader { get; set; }
        public string ChecklistName { get; set; }
        public int IdJob { get; set; }
    }
    public class GeneralChecklistResult
    {   
        public int IdJob { get; set; }
        public IEnumerable<ListofGeneralChecklist> listofGeneralChecklist { get; set; }
     
    }
}