
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rpo.ApiServices.Model.Models
{
    public class EmployeePermission
    {
        [Key]
        public int Id { get; set; }

        public int IdEmployee { get; set; }

        [ForeignKey("IdEmployee")]
        public Employee Employee { get; set; }

        public string Permissions { get; set; }
    }
}
