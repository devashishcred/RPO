using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens;
using System.Security.Claims;

namespace Rpo.Identity.Core.Providers
{
    /// <summary>
    /// Create a Jwt format token to client
    /// </summary>
    /// <remarks>
    /// Add this config to your Web.config:
    /// <appSettings>
    ///     <add key = "as:AudienceId" value="414e1927a3884f68abc79f7283837fd1" />
    ///     <add key = "as:AudienceSecret" value="qMCdFDQuF23RV1Y-1Gq9L3cF3VmuFwVbam4fMTdAfpo" />
    /// </appSettings>
    /// You can modify the values.
    /// </remarks>
    public class RpoJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _issuer = string.Empty;

        public RpoJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["as:AudienceSecret"];

            byte[] keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            HmacSigningCredentials signingKey = new HmacSigningCredentials(keyByteArray);

            DateTimeOffset? issued = data.Properties.IssuedUtc;

            DateTimeOffset? expires = data.Properties.ExpiresUtc;

            JwtSecurityToken token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(token);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}
