using Rpo.ApiServices.Model.Models.Enums;

namespace Rpo.ApiServices.Api.Controllers.Jobs
{
    public class JobStatusDTO
    {
        public JobStatus JobStatus { get; set; }

        public string StatusReason { get; set; }
    }
}