using Microsoft.Owin.Security.Infrastructure;
using Rpo.Identity.Core.Models;
using Rpo.Identity.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace Rpo.Identity.Core.Providers
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            string clientId = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientId))
                return;

            string refreshTokenId = Guid.NewGuid().ToString("n");

            using (AuthenticationRepository _repository = new AuthenticationRepository())
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var token = new RpoIdentityRefreshToken
                {
                    Id = Helper.GetHash(refreshTokenId),
                    ClientId = clientId,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                bool result = await _repository.AddRefreshToken(token);

                if (result)
                    context.SetToken(refreshTokenId);
            }
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            string allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Helper.GetHash(context.Token);

            using (AuthenticationRepository _repository = new AuthenticationRepository())
            {
                var refreshToken = await _repository.FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    // Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    var result = await _repository.RemoveRefreshToken(hashedTokenId);
                }
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}