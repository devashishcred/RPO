using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class TransmissionType
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "Name allow max 50 characters!")]
        [Index("IX_TransmissionTypeName", IsUnique = true)]
        [Required]
        public string Name { get; set; }

        [ForeignKey("IdTransmissionType")]
        public ICollection<TransmissionTypeDefaultCC> DefaultCC { get; set; }

        public bool IsSendEmail { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public Employee CreatedByEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        [ForeignKey("LastModifiedBy")]
        public Employee LastModifiedByEmployee { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
