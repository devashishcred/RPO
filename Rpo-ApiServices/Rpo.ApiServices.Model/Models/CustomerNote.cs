

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
    public class CustomerNote
    {
        [Key]
        public int Id { get; set; }

        public int IdJobChecklistItemDetail { get; set; }

        [ForeignKey("IdJobChecklistItemDetail")]
        public virtual JobChecklistItemDetail JobChecklistItemDetail { get; set; }
        public int Idcustomer { get; set; }

        [ForeignKey("Idcustomer")]
        public virtual Customer Customer { get; set; }
        public string Description { get; set; }
        public bool Isinternal { get; set; }
      //  public int? CreatedByCus { get; set; }


        //[ForeignKey("CreatedByCus")]
        //public virtual Customer CustomerCreatedBy{ get; set; }

        //public DateTime? CreatedDate { get; set; }

        //public int? LastModifiedByCus{ get; set; }

        //[ForeignKey("LastModifiedByCus")]
        //public virtual Customer LastModifiedByCustomer { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}



