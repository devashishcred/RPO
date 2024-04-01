using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist.Models
{
    public class CompositeChecklistGroupHeader
    {
        public string CompositeChecklistName { get; set; }
        public int? IdCompositeChecklist { get; set; }
        public int? jobChecklistHeaderId { get; set; }
        public int? IdJob { get; set; }
        public int? CompositeOrder { get; set; }
       public bool IsCOProject { get; set; }
        public bool IsParentCheckList { get; set; }
        
        public List<CompositeChecklistGroup> groups { get; set; }
        public string IdWorkPermits { get; set; }
        public string Others { get; set; }
    }
    public class CompositeChecklistGroup
    {
        public string checkListGroupName { get; set; }
        public string checkListGroupType { get; set; }
        public int? jobChecklistGroupId { get; set; }
        public int? DisplayOrder1 { get; set; }

        public List<Item> item { get; set; }
    }
    public class Item
    {
        public string checklistItemName { get; set; }
        public int? IdChecklistItem { get; set; }
        public int? jocbChecklistItemDetailsId { get; set; }
        public string comments { get; set; }
        public int? idDesignApplicant { get; set; }
        public int? idInspector { get; set; }
        public string stage { get; set; }
        public int? partyResponsible1 { get; set; }
        public string manualPartyResponsible { get; set; }

        public int? idContact { get; set; }
        public string referenceDocumentId { get; set; }
        public DateTime? dueDate { get; set; }
        public int? status { get; set; }

        public DateTime? checkListLastModifiedDate { get; set; }
        public int? idCreateFormDocument { get; set; }
        public int? idUploadFormDocument { get; set; }
        public int? Displayorder { get; set; }
        public int? ChecklistDetailDisplayOrder { get; set; }
        public string CompositeName { get; set; }
        public int? IdCompositeChecklist { get; set; }
        public bool IsRequiredForTCO { get; set; }
        public string PartyResponsible { get; set; }
     
        public bool IsParentCheckList { get; set; }
        public string FloorNumber { get; set; }
        public int? idJobPlumbingCheckListFloors { get; set; }
        public int? FloorDisplayOrder { get; set; }
        public List<Details> Details { get; set; }
        public bool HasDocument { get; set; }
     
    }
    public class Details
    {
        public string checklistGroupType { get; set; }
        public int? idJobPlumbingInspection { get; set; }
        public int? idJobPlumbingCheckListFloors { get; set; }
        public string inspectionPermit { get; set; }
        public string floorName { get; set; }
        public string workOrderNumber { get; set; }
        public string plComments { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? nextInspection { get; set; }
        public string result { get; set; }
        public bool IsRequiredForTCO { get; set; }
        public bool HasDocument { get; set; }       
    }
}