using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class TransmissionTypeDefaultCC
    {
        [Key]
        public int Id { get; set; }

        public int? IdTransmissionType { get; set; }

        public int? IdEamilType { get; set; }

        public int? IdEmployee { get; set; }

        [ForeignKey("IdEmployee")]
        public Employee Employee { get; set; }
    }
}
