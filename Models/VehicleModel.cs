using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWebMVC.Models;

public class VehicleModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    [DisplayName("Giá")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    public string? Color { get; set; }
    public string? InteriorColor { get; set; }
    public string? CountryOfOrigin { get; set; }
    public int Year { get; set; }
    public int NumberOfDoors { get; set; }
    public int NumberOfSeats { get; set; }
    public int TransmissionId { get; set; }
    public Transmission? Transmission { get; set; }
    public int EngineTypeId { get; set; }
    public EngineType? EngineType { get; set; }
    public int VehicleLineId { get; set; }
    public VehicleLine? VehicleLine { get; set; }
}