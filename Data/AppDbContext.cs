using CarWebMVC.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarWebMVC.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
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
        public DbSet<VehicleImage> VehicleImages { get; set; }
    }
}
