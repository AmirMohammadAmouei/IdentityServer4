using IdentityServer4.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer4.Data
{
    public class IdentityServerDbContext : IdentityDbContext
    {

        public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options) : base(options)
        {
        }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionStirng =
                    "Server=.\\AMIR ; Initial Catalog=IdentityServerDb ; User Id=AmirFar ; Password=27101377";
                optionsBuilder.UseSqlServer(connectionStirng);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
