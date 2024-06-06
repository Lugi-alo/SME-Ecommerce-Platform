using Dissertation.Models;
using Microsoft.AspNetCore.Identity;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceListingFeature>()
                .HasKey(sl => new { sl.ServiceListingsId, sl.FeaturesId });

            modelBuilder.Entity<ServiceListingFeature>()
                .HasOne(sl => sl.ServiceListings)
                .WithMany(s => s.ServiceListingFeatures)
                .HasForeignKey(sl => sl.ServiceListingsId);

            modelBuilder.Entity<ServiceListingFeature>()
                .HasOne(sl => sl.Features)
                .WithMany(f => f.ServiceListingFeatures)
                .HasForeignKey(sl => sl.FeaturesId);
        }


    }
}


