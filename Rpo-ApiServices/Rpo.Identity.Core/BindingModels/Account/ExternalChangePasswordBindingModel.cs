namespace Rpo.Identity.Core.BindingModels.Account
{
    public class ExternalChangePasswordBindingModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}