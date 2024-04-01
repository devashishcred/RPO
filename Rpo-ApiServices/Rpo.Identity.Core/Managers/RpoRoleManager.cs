using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Rpo.Identity.Core.Managers
{
    public class RpoRoleManager : RoleManager<IdentityRole>
    {
        public RpoRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        { }

        public static RpoRoleManager Create(IdentityFactoryOptions<RpoRoleManager> options, IOwinContext context)
        {
            return new RpoRoleManager(new RoleStore<IdentityRole>(context.Get<RpoIdentityDbContext>()));
        }
    }
}
