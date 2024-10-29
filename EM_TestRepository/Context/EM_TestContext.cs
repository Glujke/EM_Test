using EM_TestRepository.Entity;
using Microsoft.EntityFrameworkCore;

namespace EM_TestRepository.Context
{
    public class EM_TestContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Request> Requests { get; set; }

        public EM_TestContext(DbContextOptions<EM_TestContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*#region Debug
            CreateDebugDBValue(modelBuilder);
            #endregion*/

            modelBuilder.Entity<Order>()
                 .HasOne(p => p.Location)
                 .WithMany()
                 .HasForeignKey(p => p.LocationId);

            modelBuilder.Entity<Location>()
                 .HasIndex(l => l.Name)
                 .IsUnique();
        }

        /*#region Debug
        private void CreateDebugDBValue(ModelBuilder modelBuilder)
        {
            Random rnd = new Random();

            for (int i = 1; i <= 50; ++i)
            {
                modelBuilder.Entity<Location>().HasData(new Location { Id = i, Name = $"Улица номер #{i}"});
            }

            for (int i = 1; i <= 7000; ++i)
            {
                modelBuilder.Entity<Order>().HasData(new Order { 
                    Id = i, 
                    Number = i, 
                    LocationId = rnd.Next(1, 50),
                    Date = DateTime.Now.AddMinutes(rnd.Next(0, 60)),
                    Weight = rnd.Next(0, 5000)});
            }
        }
        #endregion*/
    }
}

