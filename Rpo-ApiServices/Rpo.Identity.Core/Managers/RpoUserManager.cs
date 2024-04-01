using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Rpo.Identity.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rpo.Identity.Core.Managers
{
    internal class OwnPasswordValidator : IIdentityValidator<string>
    {
        public async Task<IdentityResult> ValidateAsync(string item)
        {
            PasswordValidator passwordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            IdentityResult result = await passwordValidator.ValidateAsync(item);

            if (result != IdentityResult.Success)
                return await Task.FromResult(IdentityResult.Failed(result.Errors.ToArray()));
            else if (!new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9]*$").IsMatch(item))
                return await Task.FromResult(IdentityResult.Failed("It is allowed only alphanumeric characters for password."));

            return await Task.FromResult(IdentityResult.Success);
        }
    }

    // Configure the application user manager which is used in this application.
    public class RpoUserManager : UserManager<RpoIdentityUser>
    {
        public RpoUserManager(IUserStore<RpoIdentityUser> store)
            : base(store)
        {
        }

        public static RpoUserManager Create(IdentityFactoryOptions<RpoUserManager> options, IOwinContext context)
        {
            RpoUserManager manager = new RpoUserManager(new UserStore<RpoIdentityUser>(context.Get<RpoIdentityDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<RpoIdentityUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new OwnPasswordValidator();

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(15);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // TODO: Not implemented
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            //manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<RpoIdentityUser>
            //{
            //    MessageFormat = "Your security code is {0}"
            //});

            //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<RpoIdentityUser>
            //{
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //});

            //manager.EmailService = new EmailService();

            // TODO: Not implemented
            // manager.SmsService = new SmsService();

            //IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;

            //if (dataProtectionProvider != null)
            //    manager.UserTokenProvider = new DataProtectorTokenProvider<RpoIdentityUser>(dataProtectionProvider.Create("UserToken"))
            //    {
            //        // Code for e-mail confirmation and reset password life time
            //        TokenLifespan = TimeSpan.FromDays(10)
            //    };

            return manager;
        }
    }
}
