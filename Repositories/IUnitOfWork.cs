using CarWebMVC.Models.Domain;

namespace CarWebMVC.Repositories;

public interface IUnitOfWork
{
    public IGenericRepository<VehicleModel> VehicleModelRepository { get; }
    public IGenericRepository<VehicleLine> VehicleLineRepository { get; }
    public IGenericRepository<VehicleImage> VehicleImageRepository { get; }
    public IGenericRepository<Transmission> TransmissionRepository { get; }
    public IGenericRepository<EngineType> EngineTypeRepository { get; }
    public IGenericRepository<Manufacturer> ManufacturerRepository { get; }
    public IGenericRepository<VehicleType> VehicleTypeRepository { get; }
    public IGenericRepository<Customer> CustomerRepository { get; }
    public Task SaveChangesAsync();
}