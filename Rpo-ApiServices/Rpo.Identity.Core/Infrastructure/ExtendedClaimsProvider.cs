using Rpo.Identity.Core.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Rpo.Identity.Core.Infrastructure
{
    public static class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(RpoIdentityUser user)
        {
            List<Claim> claims = new List<Claim>();
            
            claims.Add(CreateClaim("FTE", "1"));
            claims.Add(CreateClaim(ClaimValueTypes.Email, user.Email));

            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }
    }
}
