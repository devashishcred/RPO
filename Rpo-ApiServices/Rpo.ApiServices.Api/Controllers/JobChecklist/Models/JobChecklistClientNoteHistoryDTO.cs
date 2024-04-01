﻿



namespace Rpo.ApiServices.Api.Controllers.ChecklistItems
{
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Api.Controllers.TaskNotes;
    using Model.Models.Enums;
    using System;
    using System.Collections.Generic;
    using Rpo.ApiServices.Model.Models;
    public class JobChecklistClientNoteHistoryDTO
    {
        public int Id { get; set; }
        public int IdCustomer { get; set; }
        public int IdJobChecklistItemDetail { get; set; }
        public JobChecklistItemDetail JobChecklistItemDetail { get; set; }
        public string Description { get; set; }
        public bool Isinternal { get; set; }     

         // public Customer CustomerCreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }     

          public Customer CustomerLastModifiedBy{ get; set; }


        public DateTime? LastModifiedDate { get; set; }
        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>The last modified.</value>
      //  public string LastModified { get; set; }

    }
}