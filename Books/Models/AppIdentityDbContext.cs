using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Books.Models
{
    public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<AppUser> AppUsers { get; set; }

        public AppIdentityDbContext(
            DbContextOptions<AppIdentityDbContext> options)
            : base(options) { }

    }
}
