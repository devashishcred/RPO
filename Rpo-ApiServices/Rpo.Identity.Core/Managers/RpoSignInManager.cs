using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Rpo.Identity.Core.Models;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rpo.Identity.Core.Managers
{
    // Configure the application sign-in manager which is used in this application.  
    public class RpoSignInManager : SignInManager<RpoIdentityUser, string>
    {
        public RpoSignInManager(RpoUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(RpoIdentityUser user)
        {
            return user.GenerateUserIdentityAsync((RpoUserManager)UserManager, ConfigurationManager.AppSettings["server:AuthenticationType"]);
        }

        public static RpoSignInManager Create(IdentityFactoryOptions<RpoSignInManager> options, IOwinContext context)
        {
            return new RpoSignInManager(context.GetUserManager<RpoUserManager>(), context.Authentication);
        }
    }
}
