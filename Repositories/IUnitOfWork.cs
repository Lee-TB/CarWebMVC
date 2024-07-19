using CarWebMVC.Models;

namespace CarWebMVC.Repositories;

public interface IUnitOfWork
{
    public IGenericRepository<VehicleModel> VehicleModelRepository { get; }
    public IGenericRepository<VehicleImage> VehicleImageRepository { get; }
    public IGenericRepository<Transmission> TransmissionRepository { get; }
    public IGenericRepository<EngineType> EngineTypeRepository { get; }
    public IGenericRepository<VehicleLine> VehicleLineRepository { get; }
    public Task SaveChangesAsync();
}