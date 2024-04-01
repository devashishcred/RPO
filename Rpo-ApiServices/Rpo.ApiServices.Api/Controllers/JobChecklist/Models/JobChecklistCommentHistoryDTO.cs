
namespace Rpo.ApiServices.Api.Controllers.ChecklistItems
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Api.Controllers.TaskNotes;
    using Model.Models.Enums;
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;
    public class JobChecklistCommentHistoryDTO
    {
        public int Id { get; set; }

        public int IdJobChecklistItemDetail { get; set; }       
        public  JobChecklistItemDetail JobChecklistItemDetail { get; set; }
        public string Description { get; set; }
        public bool Isinternal { get; set; }
        public int? CreatedBy { get; set; }
      
      //  public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }
     
        public Employee LastModifiedByEmployee { get; set; }

     
        public DateTime? LastModifiedDate { get; set; }

       
       

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
        public string LastModified { get; set; }


        /// <summary>
        /// Gets or sets the created by employee.
        /// </summary>
        /// <value>The created by employee.</value>
        public string CreatedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
      

    }
}