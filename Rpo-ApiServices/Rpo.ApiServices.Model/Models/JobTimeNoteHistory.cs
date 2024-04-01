
namespace Rpo.ApiServices.Model.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class JobTimeNoteHistory
    {
        [Key]
        public int Id { get; set; }
        
      
        public string Description { get; set; }

        public int? IdEmployee { get; set; }
        
        [ForeignKey("IdEmployee")]
        public Employee Employee { get; set; }
        
        public DateTime HistoryDate { get; set; }

        public int IdJobTimeNote { get; set; }
        
        [ForeignKey("IdJobTimeNote")]
        public JobTimeNote JobTimeNote { get; set; }

        public int? IdJobFeeSchedule { get; set; }

        [ForeignKey("IdJobFeeSchedule")]
        public JobFeeSchedule JobFeeSchedule { get; set; }
        /// <summary>
        /// Gets or sets the time hours.
        /// </summary>
        /// <value>The time hours.</value>
        public Int16? TimeHours { get; set; }
        /// <summary>
        /// Gets or sets the time minutes.
        /// </summary>
        /// <value>The time minutes.</value>
        public Int16? TimeMinutes { get; set; }
    }
}
