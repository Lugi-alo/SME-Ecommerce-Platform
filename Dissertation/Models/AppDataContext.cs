using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dissertation.Models
{
    public class AppDataContext : IdentityDbContext<AppUser>
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) { }
        public DbSet<ServiceListings> ServiceListings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Features> Features { get; set; }


    }
}
