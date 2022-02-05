using Microsoft.EntityFrameworkCore;
using VehicleApp.Entities;

namespace VehicleApp.Data
{
    public class VehicleAppDbContext : DbContext
    {
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<Truck> Trucks => Set<Truck>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=.;Database=VehliclesDatabase;Trusted_Connection=True;");
        }
    }
}
