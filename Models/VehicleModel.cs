using System.ComponentModel;

namespace CarWebMVC.Models;

public class VehicleModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? Color { get; set; }
    public string? InteriorColor { get; set; }
    public string? CountryOfOrigin { get; set; }
    public int Year { get; set; }
    public int NumberOfDoors { get; set; }
    public int NumberOfSeats { get; set; }

    public EngineType EngineType { get; set; }
    public VehicleLine VehicleLine { get; set; }
}