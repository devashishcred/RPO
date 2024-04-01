using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.JobChecklist
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;

    public class JobChecklistHeaderDTO
    {
        public int IdJobCheckListHeader { get; set; }
       public string ChecklistName { get; set; }
        public int IdJob { get; set; }
        public IEnumerable<Job> Jobs { get; set; }

        public int IdJobApplication { get; set; }

        public IEnumerable<JobApplication> JobApplications { get; set; }
        public virtual IEnumerable<JobApplicationWorkPermitType> JobApplicationWorkPermitTypes { get; set; }
        public string ApplicationTypeNumber { get; set; }
        public string strIdJobApplicationWorkPermitTypes { get; set; }
        public string strJobApplicationWorkPermitTypes { get; set; }
        public int NoOfFloors { get; set; }
        public int? CreatedBy { get; set; }

        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }


        public IEnumerable<CheckListGroup> CheckListGroups { get; set; }

     
        public IEnumerable<JobPlumbingInspection> JobChecklistPlumbingInspection { get; set; }

        public IEnumerable<JobPlumbingChecklistFloors> JobPlumbingChecklistFloors { get; set; }
        public int IdCompositeChecklist { get; set; }
        public bool IsParentCheckList { get; set; }

    }
}