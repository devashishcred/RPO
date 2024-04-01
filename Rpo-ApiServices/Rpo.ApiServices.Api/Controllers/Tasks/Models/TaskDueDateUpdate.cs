using System;

namespace Rpo.ApiServices.Api.Controllers.Tasks
{
    public class TaskDueDateUpdate
    {
        
        public int Id { get; set; }

        
        public DateTime? DueDate { get; set; }
    }
}