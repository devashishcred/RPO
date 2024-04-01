
namespace Rpo.ApiServices.Api.Controllers.JobApplicationTypes
{
    
    public class JobApplicationTypeCreateUpdate
    {
        public int Id { get; set; }
        
        public string Description { get; set; }

        public string Content { get; set; }
        public int? IdParent { get; set; }
    }
}