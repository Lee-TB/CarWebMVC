using CarWebMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWebMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<EngineType> EngineTypes { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<VehicleLine> VehicleLines { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<Transmission> Transmission { get; set; } = default!;
    }
}
