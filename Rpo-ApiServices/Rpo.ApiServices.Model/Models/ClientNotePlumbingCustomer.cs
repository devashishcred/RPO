using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
    public class ClientNotePlumbingCustomer
    {
        [Key]
        public int Id { get; set; }

        public int IdJobPlumbingInspection { get; set; }

        [ForeignKey("IdJobPlumbingInspection")]
        public virtual JobPlumbingInspection JobPlumbingInspection { get; set; }
        public int Idcustomer { get; set; }

        [ForeignKey("Idcustomer")]
        public virtual Customer Customer { get; set; }
        public string Description { get; set; }
        public bool Isinternal { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}





