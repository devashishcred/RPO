using Microsoft.AspNet.Identity.EntityFramework;
using Rpo.Identity.Core.Models;
using System.Data.Entity;

namespace Rpo.Identity.Core
{
    public class RpoIdentityDbContext : IdentityDbContext<RpoIdentityUser>
    {
        public RpoIdentityDbContext()
#if DEBUG
            : base("RpoConnection")
#else
            : base("RpoProdConnection")
#endif
            {
            Configuration.LazyLoadingEnabled = true;
        }

        public static RpoIdentityDbContext Create()
        {
            return new RpoIdentityDbContext();
        }

        public DbSet<RpoIdentityClient> RpoIdentityClients { get; set; }
        
        public DbSet<RpoIdentityRefreshToken> RpoIdentityRefreshTokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}