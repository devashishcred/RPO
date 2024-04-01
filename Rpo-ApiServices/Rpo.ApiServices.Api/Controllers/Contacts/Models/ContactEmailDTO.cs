
namespace Rpo.ApiServices.Api.Controllers.Contacts
{
    public class ContactEmailDTO
    {
        public int[] IdContactsCc { get; set; }

        public int? IdContactsTo { get; set; }

        public int IdContactAttention { get; set; }

        public int IdFromEmployee { get; set; }

        public int IdEmailType { get; set; }

        public int IdTransmissionType { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }

        public bool IsAdditionalAtttachment { get; set; }
    }
}