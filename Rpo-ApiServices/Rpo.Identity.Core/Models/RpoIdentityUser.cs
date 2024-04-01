using Microsoft.AspNet.Identity.EntityFramework;
using Rpo.Identity.Core.Managers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rpo.Identity.Core.Models
{
    public class RpoIdentityUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(RpoUserManager manager, string authenticationType)
        {
            return await manager.CreateIdentityAsync(this, authenticationType);
        }
    }
}