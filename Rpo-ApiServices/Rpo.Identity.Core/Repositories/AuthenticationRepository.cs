using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Rpo.Identity.Core.Managers;
using Rpo.Identity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rpo.Identity.Core.Repositories
{
    // TODO: Obsolete, create specifics repositories, like UserRepository<Context>, IdentityRepository<Context>.
    public class AuthenticationRepository : IDisposable
    {
        private RpoIdentityDbContext _context;
        private RpoUserManager _userManager;
        
        public AuthenticationRepository()
        {
            _context = new RpoIdentityDbContext();
            _userManager = new RpoUserManager(new UserStore<RpoIdentityUser>(_context));
        }

        public List<RpoIdentityUser> GetUsers() {
            return _context.Users.ToList();
        }

        //public async Task<IdentityResult> RegisterUser(UserViewModel userViewModel)
        //{
        //    RpoIdentityUser user = new RpoIdentityUser
        //    {
        //        UserName = userViewModel.UserName
        //    };

        //    return await _userManager.CreateAsync(user, userViewModel.Password);
        //} 

        public async Task<RpoIdentityUser> FindUser(string userName, string password)
        {
            return await _userManager.FindAsync(userName, password);
        }

        public async Task<RpoIdentityUser> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public RpoIdentityClient FindClient(string clientId)
        {
            return _context.RpoIdentityClients.Find(clientId);
        }

        public async Task<bool> AddRefreshToken(RpoIdentityRefreshToken token)
        {
            var existingToken = _context.RpoIdentityRefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).FirstOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _context.RpoIdentityRefreshTokens.Add(token);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RpoIdentityRefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _context.RpoIdentityRefreshTokens.Remove(refreshToken);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }
        
        public async Task<bool> RemoveRefreshToken(RpoIdentityRefreshToken refreshToken)
        {
            _context.RpoIdentityRefreshTokens.Remove(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RpoIdentityRefreshToken> FindRefreshToken(string refreshTokenId)
        {
            return await _context.RpoIdentityRefreshTokens.FindAsync(refreshTokenId);
        }

        public List<RpoIdentityRefreshToken> GetAllRefreshTokens()
        {
            return _context.RpoIdentityRefreshTokens.ToList();
        }

        public async Task<RpoIdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            return await _userManager.FindAsync(loginInfo);
        }

        public async Task<IdentityResult> CreateAsync(RpoIdentityUser user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            return await _userManager.AddLoginAsync(userId, login);
        }

        public void Dispose()
        {
            _context.Dispose();
            _userManager.Dispose();
        }
    }
}