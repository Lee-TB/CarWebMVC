using System.ComponentModel;
using CarWebMVC.Models.Domain;

namespace CarWebMVC.Models.ViewModels;

public class VehicleModelViewModel : VehicleModel
{
    public List<string> ImageUrls => Images.Select(i => i.ImageUrl).ToList();
    [DisplayName("Hình ảnh")]
    public string? Thumbnail => Images?.Count > 0 ? Images.FirstOrDefault()?.ImageUrl : "";
    [DisplayName("Hộp số")]
    public string? TransmissionName { get; set; }
    [DisplayName("Loại động cơ")]
    public string? EngineTypeName { get; set; }
    [DisplayName("Dòng xe")]
    public string? VehicleLineName { get; set; }
}