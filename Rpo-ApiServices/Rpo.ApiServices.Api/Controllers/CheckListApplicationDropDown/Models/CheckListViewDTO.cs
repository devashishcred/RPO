using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown
{
    public class CheckListViewDTO
    {
        /// <summary>
        /// Gets or sets the checklist Group name.
        /// </summary>
        /// <value>The name.</value>
        public string ChecklistGroupName { get; set; }

        /// <summary>
        /// Gets or sets the checklist Group Type.
        /// </summary>
        /// <value>The name.</value>
        public string ChecklistGroupType { get; set; }


        /// <summary>
        /// Gets or sets the checklist Item name.
        /// </summary>
        /// <value>The name.</value>
        public string checklistItems { get; set; }

        /// <summary>
        /// Gets or sets the commnets of the cheklist item.
        /// </summary>
        /// <value>The name.</value>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the Reference Document ID
        /// </summary>
        /// <value>The ID.</value>
        public string IdReferenceDocument { get; set; }

        /// <summary>
        /// Gets or sets the checklist document name.
        /// </summary>
        /// <value>The name.</value>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? TargetDate { get; set; }   
           public int? PartyResponsible1  { get; set; }
        public string ManualPartyResponsible { get; set; }
        public int? IdContact { get; set; }

      
        /// <summary>
        /// Gets or sets the Due date.
        /// </summary>
        /// <value>The Due Date date.</value>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the checklist Item status
        /// </summary>
        /// <value>The status.</value>
        public int? Status { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>The last modified date.</value>
        public DateTime? checklistItemLastDate { get; set; }

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>
        public int? IdCreateFormDocument { get; set; }

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>
        public int? IdUploadFormDocument { get; set; }

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>
        public int? JobChecklistHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>
        public int? JobChecklistGroupId { get; set; }

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>
        public int? JocbChecklistItemDetailsId { get; set; }
        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>

        public int? jobchecklistItemdetail_checklistItem { get; set; }

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>
        ///
        //public int? PLchecklistItemID { get; set; }

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>
        public string ApplicationNumber { get; set; }
        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>
        public string IdJobPlumbingInspection { get; set; }

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The permit.</value>
        public string InspectionPermit { get; set; }

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The floorname.</value>
        public string FloorName { get; set; }

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The floorname.</value>
        public string WorkOrderNumber { get; set; }  

        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        /// <value>The ID.</value>
        public DateTime? NextInspection { get; set; }  

        public string Code { get; set; }
        public string PLInspectpermit { get; set; }
        public string Stage { get; set; }
        public int? IdDesignApplicant { get; set; }
        public int? IdInspector { get; set; }
        public string Result { get; set; }
        public int JobChecklistGroupOrder { get; set; }
        public int ChecklistItemDetailOrder { get; set; }
        public int CheckListGroupsOrder { get; set; }

    }
}