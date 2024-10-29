using EM_TestRepository.Entity;
using Microsoft.EntityFrameworkCore;

namespace EM_TestRepository.Context
{
    public class EM_TestContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }

        public EM_TestContext(DbContextOptions<EM_TestContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                 .HasOne(p => p.Location)
                 .WithMany()
                 .HasForeignKey(p => p.LocationId);

            modelBuilder.Entity<Location>()
                 .HasIndex(l => l.Name)
                 .IsUnique();
        }
    }
}

