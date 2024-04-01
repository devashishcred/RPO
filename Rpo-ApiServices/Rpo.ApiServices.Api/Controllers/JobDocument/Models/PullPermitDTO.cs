namespace Rpo.ApiServices.Api.Controllers.JobDocument
{
    public class PullPermitDTO
    {
        public int IdJobDocument { get; set; }

        public string DetailUrl { get; set; }

        public string NumberDocType { get; set; }
    }

    public class PullPermitResultDTO
    {
        public string JobDocumentUrl { get; set; }

        public bool IsPdfExist { get; set; }
    }
}