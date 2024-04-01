using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class RFPEmailCCHistory
    {
        [Key]
        public int Id { get; set; }

        public int? IdRFPEmailHistory { get; set; }

        public int? IdContact { get; set; }

        [ForeignKey("IdContact")]
        public Contact Contact { get; set; }

    }
}
