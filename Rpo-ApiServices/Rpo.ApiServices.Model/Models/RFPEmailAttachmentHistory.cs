using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class RFPEmailAttachmentHistory
    {
        [Key]
        public int Id { get; set; }

        public int? IdRFPEmailHistory { get; set; }

        [StringLength(500)]
        public string DocumentPath { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

    }
}
