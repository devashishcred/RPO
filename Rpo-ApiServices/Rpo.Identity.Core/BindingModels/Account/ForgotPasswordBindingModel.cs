using System.ComponentModel.DataAnnotations;

namespace Rpo.Identity.Core.BindingModels.Account
{
    public class ForgotPasswordBindingModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string ExternalRoute { get; set; }
    }
}