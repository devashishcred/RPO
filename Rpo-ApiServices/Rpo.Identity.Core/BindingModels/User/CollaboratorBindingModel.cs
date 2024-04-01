using System.ComponentModel.DataAnnotations;

namespace Rpo.Identity.Core.BindingModels.User
{
    public class CollaboratorBindingModel : UserBindingModel
    {
        [Required]
        public string Department { get; set; }
    }
}
