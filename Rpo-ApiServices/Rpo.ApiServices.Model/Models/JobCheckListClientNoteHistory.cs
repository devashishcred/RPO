﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
    public class JobCheckListClientNoteHistory
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

