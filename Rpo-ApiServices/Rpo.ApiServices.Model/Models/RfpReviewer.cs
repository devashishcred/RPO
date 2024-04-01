using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class RfpReviewer
    {
        [Key]
        public int Id { get; set; }

        public int IdRfp { get; set; }

        public int? IdReviewer { get; set; }

        [ForeignKey("IdReviewer")]
        public Employee Reviewer { get; set; }
    }
}
