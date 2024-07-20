using CarWebMVC.Data;
using CarWebMVC.Models.Domain;

namespace CarWebMVC.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext context;
    public IGenericRepository<VehicleModel> VehicleModelRepository { get; }
    public IGenericRepository<VehicleImage> VehicleImageRepository { get; }

    public IGenericRepository<Transmission> TransmissionRepository { get; }

    public IGenericRepository<EngineType> EngineTypeRepository { get; }

    public IGenericRepository<VehicleLine> VehicleLineRepository { get; }

    public IGenericRepository<Manufacturer> ManufacturerRepository { get; }

    public IGenericRepository<VehicleType> VehicleTypeRepository { get; }

    public IGenericRepository<Customer> CustomerRepository { get; }

    public UnitOfWork(AppDbContext dbContext)
    {
        context = dbContext;
        VehicleModelRepository = new GenericRepository<VehicleModel>(dbContext);
        VehicleImageRepository = new GenericRepository<VehicleImage>(dbContext);
        TransmissionRepository = new GenericRepository<Transmission>(dbContext);
        EngineTypeRepository = new GenericRepository<EngineType>(dbContext);
        VehicleLineRepository = new GenericRepository<VehicleLine>(dbContext);
        ManufacturerRepository = new GenericRepository<Manufacturer>(dbContext);
        VehicleTypeRepository = new GenericRepository<VehicleType>(dbContext);
        CustomerRepository = new GenericRepository<Customer>(dbContext);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}