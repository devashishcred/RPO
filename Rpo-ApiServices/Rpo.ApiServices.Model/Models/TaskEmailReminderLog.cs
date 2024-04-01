using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpo.ApiServices.Model.Models
{
    public class TaskEmailReminderLog
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(500)]
        public string FromEmail { get; set; }

        [MaxLength(100)]
        public string FromName { get; set; }

        [MaxLength(1000)]
        public string ToEmail { get; set; }

        [MaxLength(1000)]
        public string CcEmail { get; set; }

        [MaxLength(1000)]
        public string BccEmail { get; set; }

        public string EmailBody { get; set; }

        public int IdTask { get; set; }

        public int IdEmployee { get; set; }
        
        [MaxLength(200)]
        public string EmailSubject { get; set; }

        public bool IsMailSent { get; set; }

        public string ToName { get; set; }
    }
}
