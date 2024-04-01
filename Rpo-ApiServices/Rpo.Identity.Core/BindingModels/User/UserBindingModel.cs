using System.ComponentModel.DataAnnotations;

namespace Rpo.Identity.Core.BindingModels.User
{
    public abstract class UserBindingModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
