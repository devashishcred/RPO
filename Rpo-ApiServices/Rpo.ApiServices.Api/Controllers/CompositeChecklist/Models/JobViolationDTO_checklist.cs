
using Rpo.ApiServices.Api.Controllers.JobViolations;
using Rpo.ApiServices.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist.Models
{
    public class JobViolationDTO_checklist
    {     
        public virtual JobViolationDTO jobViolations { get; set; }
        public bool IsPartofchecklist { get; set; }

    }
}