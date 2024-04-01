using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rpo.ApiServices.Model.Models;
namespace Rpo.ApiServices.Api.Controllers.CompositeChecklist.Models
{
    public class ChecklistViolationCommentDTO
    {

      
        public int Id { get; set; }
        public int IdJobViolation { get; set; }    
       
        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}