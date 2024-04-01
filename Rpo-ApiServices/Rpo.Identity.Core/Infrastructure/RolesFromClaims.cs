using Rpo.Identity.Core.Models.Enumerations;
using System.Collections.Generic;
using System.Security.Claims;

namespace Rpo.Identity.Core.Infrastructure
{
    internal class RolesFromClaims
    {
        public static IEnumerable<Claim> CreateRolesBasedOnClaims(ClaimsIdentity identity)
        {
            List<Claim> claims = new List<Claim>();

            if (identity.HasClaim(c => c.Type == RpoClaimTypes.FTE.ToString() && c.Value == "1"))
            {
                if (identity.HasClaim(ClaimTypes.Role, RpoRoles.Administrator.ToString()))
                    claims.Add(new Claim(ClaimTypes.Role, RpoRoles.Administrator.ToString()));

                if (identity.HasClaim(ClaimTypes.Role, RpoRoles.Employee.ToString()))
                    claims.Add(new Claim(ClaimTypes.Role, RpoRoles.Employee.ToString()));

                if (identity.HasClaim(ClaimTypes.Role, RpoRoles.Customer.ToString()))
                    claims.Add(new Claim(ClaimTypes.Role, RpoRoles.Customer.ToString()));
            }
            return claims;
        }
    }
}
