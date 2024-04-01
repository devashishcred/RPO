using Microsoft.AspNet.Identity.Owin;
using Rpo.Identity.Core.Managers;
using Rpo.Identity.Core.Models;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rpo.Identity.Core.Infrastructure
{
    public class RpoIdentityApiController : ApiController
    {
        public RpoIdentityApiController()
            : base()
        {
        }

        private RpoIdentityDbContext _identityDbContext = null;
        private RpoRoleManager _roleManager = null;
        private RpoUserManager _userManager = null;
        private Task<RpoIdentityUser> _identityUser = null;

        protected virtual RpoIdentityDbContext Context
        {
            get
            {
                if (_identityDbContext == null)
                {
                    _identityDbContext = Request.GetOwinContext().Get<RpoIdentityDbContext>();
                }

                return _identityDbContext;
            }
        }

        protected RpoRoleManager RoleManager
        {
            get
            {
                if (_roleManager == null)
                {
                    _roleManager = Request.GetOwinContext().Get<RpoRoleManager>();
                }
                return _roleManager;
            }
        }

        protected RpoUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = Request.GetOwinContext().GetUserManager<RpoUserManager>();

                return _userManager;
            }
        }

        protected Task<RpoIdentityUser> IdentityUser
        {
            get
            {
                if (_identityUser == null && this.RequestContext.Principal.Identity.IsAuthenticated)
                {
                    ClaimsIdentity claimsIdentity = this.RequestContext.Principal.Identity as ClaimsIdentity;

                    if (claimsIdentity != null)
                    {
                        Claim emailAddressClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimValueTypes.Email);
                        if (emailAddressClaim != null)
                            return (_identityUser = UserManager.FindByEmailAsync(emailAddressClaim.Value));
                    }
                }

                return Task.FromResult<RpoIdentityUser>(null);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_identityDbContext != null)
                    _identityDbContext.Dispose();

                if (_userManager != null)
                    _userManager.Dispose();

                if (_roleManager != null)
                    _roleManager.Dispose();

                if (_identityUser != null)
                    _identityUser.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}