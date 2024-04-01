using System.ComponentModel.DataAnnotations;

namespace Rpo.Identity.Core.BindingModels.User
{
    public class ClientBindingModel : UserBindingModel
    {
        [Required]
        public string Unit { get; set; }
    }
}
