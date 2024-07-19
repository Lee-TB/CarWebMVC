using CarWebMVC.Data;
using CarWebMVC.Models;

namespace CarWebMVC.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext context;
    public IGenericRepository<VehicleModel> VehicleModelRepository { get; }
    public IGenericRepository<VehicleImage> VehicleImageRepository { get; }

    public IGenericRepository<Transmission> TransmissionRepository { get; }

    public IGenericRepository<EngineType> EngineTypeRepository { get; }

    public IGenericRepository<VehicleLine> VehicleLineRepository { get; }

    public UnitOfWork(AppDbContext dbContext)
    {
        context = dbContext;
        VehicleModelRepository = new GenericRepository<VehicleModel>(dbContext);
        VehicleImageRepository = new GenericRepository<VehicleImage>(dbContext);
        TransmissionRepository = new GenericRepository<Transmission>(dbContext);
        EngineTypeRepository = new GenericRepository<EngineType>(dbContext);
        VehicleLineRepository = new GenericRepository<VehicleLine>(dbContext);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}