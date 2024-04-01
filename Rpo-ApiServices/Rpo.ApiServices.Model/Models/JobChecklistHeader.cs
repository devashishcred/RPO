using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Rpo.ApiServices.Model.Models
{
    public class JobChecklistHeader
    {

        [Key]
        public int IdJobCheckListHeader { get; set; }

        public string ChecklistName { get; set; }
        public int IdJob { get; set; }
        [ForeignKey("IdJob")]
        public virtual Job Jobs { get; set; }

        public int IdJobApplication { get; set; }

        [ForeignKey("IdJobApplication")]
        public virtual JobApplication JobApplications { get; set; }

        //public int IdJobApplicationWorkPermitType { get; set; }

        //[ForeignKey("IdJobApplicationWorkPermitType")]
        //public virtual JobApplicationWorkPermitType JobApplicationWorkPermitTypes { get; set; }

        public virtual ICollection<JobApplicationWorkPermitType> JobApplicationWorkPermitTypes { get; set; }

        public int NoOfFloors { get; set; }

        public int? CreatedBy { get; set; }


        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string Others { get; set; }
    }
}
