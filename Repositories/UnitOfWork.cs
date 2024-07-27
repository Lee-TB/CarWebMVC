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

    public UnitOfWork(
        AppDbContext dbContext,
        IGenericRepository<VehicleModel> vehicleModelRepository,
        IGenericRepository<VehicleImage> vehicleImageRepository,
        IGenericRepository<Transmission> transmissionRepository,
        IGenericRepository<EngineType> engineTypeRepository,
        IGenericRepository<VehicleLine> vehicleLineRepository,
        IGenericRepository<Manufacturer> manufacturerRepository,
        IGenericRepository<VehicleType> vehicleTypeRepository,
        IGenericRepository<Customer> customerRepository)
    {
        context = dbContext;
        VehicleModelRepository = vehicleModelRepository;
        VehicleImageRepository = vehicleImageRepository;
        TransmissionRepository = transmissionRepository;
        EngineTypeRepository = engineTypeRepository;
        VehicleLineRepository = vehicleLineRepository;
        ManufacturerRepository = manufacturerRepository;
        VehicleTypeRepository = vehicleTypeRepository;
        CustomerRepository = customerRepository;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}