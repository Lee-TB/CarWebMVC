using CarWebMVC.Models.Domain;

namespace CarWebMVC.Models.LuceneDTO;

public class VehicleModelLuceneDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
    public string? InteriorColor { get; set; }
    public string? CountryOfOrigin { get; set; }
    public int Year { get; set; }
    public string? TransmissionName { get; set; }
    public string? EngineTypeName { get; set; }
    public string? VehicleLineName { get; set; }
}