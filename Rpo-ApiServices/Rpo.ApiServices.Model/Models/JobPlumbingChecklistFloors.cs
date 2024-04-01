using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
    public class JobPlumbingChecklistFloors
    {
        [Key]
        public int Id { get; set; }
        public int IdJobCheckListHeader { get; set; }

        [ForeignKey("IdJobCheckListHeader")]
        public virtual JobChecklistHeader JobChecklistHeaders { get; set; }

        public string FloonNumber { get; set; }
       
        public int FloorDisplayOrder { get; set; }
        public List<InspectionType> InspectionType { get; set; }


    }
    public class InspectionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
