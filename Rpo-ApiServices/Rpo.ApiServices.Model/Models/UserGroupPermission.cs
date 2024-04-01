
namespace Rpo.ApiServices.Model.Models
{
    using System.ComponentModel.DataAnnotations;
    
    public class UserGroupPermission
    {
        [Key]
        public int Id { get; set; }
        
        public string Permissions { get; set; }
    }
}
