using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Rpo.Identity.Core.Infrastructure;
using Rpo.Identity.Core.Managers;
using Rpo.Identity.Core.Models;
using Rpo.Identity.Core.Models.Enumerations;
using Rpo.Identity.Core.Repositories;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Rpo.Identity.Model.Services;
using System.Web;

namespace Rpo.Identity.Core.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId = string.Empty;
            string clientSecret = string.Empty;
            RpoIdentityClient client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
                context.TryGetFormCredentials(out clientId, out clientSecret);

            if (context.ClientId == null)
            {
                context.Validated();

                return Task.FromResult<object>(null);
            }

            using (AuthenticationRepository _repository = new AuthenticationRepository())
                client = _repository.FindClient(context.ClientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string absolutepath = HttpContext.Current.Request.Url.AbsolutePath;
            string allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            string Usertype = context.Scope[0];//added mital
            if (allowedOrigin == null)
            {
                allowedOrigin = "*";
            }

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            RpoIdentityUser user;
            ClaimsIdentity claimsIdentity;

            using (RpoUserManager userManager = context.OwinContext.GetUserManager<RpoUserManager>())
            {
                user = await userManager.FindByEmailAsync(context.UserName);

                if (user == null || !await userManager.CheckPasswordAsync(user, context.Password))
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect");
                    return;
                }

                if (!user.EmailConfirmed)
                {
                    context.SetError("invalid_grant", "User did not confirm email");
                    return;
                }

                //EmployeeAuthentication employeeAuthentication = new EmployeeAuthentication();
                //EmployeeAuthentication.EmployeeDetails employeeDetails = employeeAuthentication.GetEmployeeByEmail(context.UserName);
                EmployeeAuthentication employeeAuthentication = new EmployeeAuthentication();
                EmployeeAuthentication.EmployeeDetails employeeDetails = employeeAuthentication.GetEmployeeByEmail(context.UserName);
                if (Usertype.ToLower() == "customer")
                {
                    employeeDetails = employeeAuthentication.GetCustomerByEmail(context.UserName);
                }


                if (employeeDetails.IsArchive)
                {
                    context.SetError("invalid_grant", "User is deleted");
                    return;
                }

                if (!employeeDetails.IsActive)
                {
                    context.SetError("invalid_grant", "User is not active");
                    return;
                }

                claimsIdentity = await user.GenerateUserIdentityAsync(userManager, ConfigurationManager.AppSettings["server:AuthenticationType"]);
            }

            claimsIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
            claimsIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(claimsIdentity));

            Dictionary<string, string> properties = new Dictionary<string, string>
            {
                { "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId },
                { "userName", context.UserName }
            };

            AuthenticationProperties authenticationProperties = new AuthenticationProperties(properties) { IsPersistent = true };

            AuthenticationTicket ticket = new AuthenticationTicket(claimsIdentity, authenticationProperties);

            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}