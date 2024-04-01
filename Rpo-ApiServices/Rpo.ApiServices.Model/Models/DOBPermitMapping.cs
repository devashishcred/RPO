namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DOBPermitMapping
    {
        [Key]
        public int Id { get; set; }
        public int IdJob { get; set; }
        public int IdJobApplication { get; set; }
        public int? IdWorkPermit { get; set; }
        public string Seq { get; set; }
        public string Permit { get; set; }
        public string NumberDocType { get; set; }
        public string PermitType { get; set; }
        public string PermitSubType { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
