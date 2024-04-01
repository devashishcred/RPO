using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class RFPEmailHistory
    {
        [Key]
        public int Id { get; set; }

        public int? IdRfp { get; set; }

        [ForeignKey("IdRfp")]
        public Rfp Rfp { get; set; }

        public int? IdFromEmployee { get; set; }

        [ForeignKey("IdFromEmployee")]
        public Employee FromEmployee { get; set; }

        public int? IdToCompany { get; set; }

        [ForeignKey("IdToCompany")]
        public Company ToCompany { get; set; }

        public int? IdContactAttention { get; set; }

        [ForeignKey("IdContactAttention")]
        public Contact ContactAttention { get; set; }

        public int? IdTransmissionType { get; set; }

        [ForeignKey("IdTransmissionType")]
        public TransmissionType TransmissionType { get; set; }

        public int? IdEmailType { get; set; }

        [ForeignKey("IdEmailType")]
        public EmailType EmailType { get; set; }

        public DateTime SentDate { get; set; }

        [StringLength(5000)]
        public string EmailSubject { get; set; }

        public int? IdSentBy { get; set; }

        [ForeignKey("IdSentBy")]
        public Employee SentBy { get; set; }

        [StringLength(5000)]
        public string EmailMessage { get; set; }

        public bool IsAdditionalAtttachment { get; set; }

        [ForeignKey("IdRFPEmailHistory")]
        public virtual ICollection<RFPEmailCCHistory> RFPEmailCCHistories { get; set; }

        [ForeignKey("IdRFPEmailHistory")]
        public virtual ICollection<RFPEmailAttachmentHistory> RFPEmailAttachmentHistories { get; set; }

        public bool IsEmailSent { get; set; }
    }
}
