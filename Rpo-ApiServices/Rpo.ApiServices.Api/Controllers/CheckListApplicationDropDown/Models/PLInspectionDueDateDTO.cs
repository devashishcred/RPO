using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpo.ApiServices.Model.Models;

namespace Rpo.ApiServices.Api.Controllers.CheckListApplicationDropDown
{
    public class PLInspectionDueDateDTO
    {
        public int IdJobPlumbingInspectionDueDate { get; set; }

        public int  IdJobPlumbingInspection { get; set; }
        public DateTime DueDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

    }
}