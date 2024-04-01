
namespace Rpo.ApiServices.Api.Controllers.JobWorkTypes
{
    
    public class JobWorkTypeCreateUpdate
    {
        public int Id { get; set; }
        
        public string Description { get; set; }

        public string Content { get; set; }

        public string Code { get; set; }

        public double? Cost { get; set; }

        public int IdJobApplicationType { get; set; }
        
        public int? IdJobType { get; set; }

        
    }
}