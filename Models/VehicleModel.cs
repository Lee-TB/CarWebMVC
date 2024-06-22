using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWebMVC.Models;

public class VehicleModel
{
    public int Id { get; set; }
    [DisplayName("Mẫu xe")]
    public string Name { get; set; }
    [DisplayName("Giá")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    [DisplayName("Màu sắc")]
    public string? Color { get; set; }
    [DisplayName("Màu nội thất")]
    public string? InteriorColor { get; set; }
    [DisplayName("Xuất xứ")]
    public string? CountryOfOrigin { get; set; }
    [DisplayName("Năm sản xuất")]
    public int Year { get; set; }
    [DisplayName("Số cửa")]
    public int NumberOfDoors { get; set; }
    [DisplayName("Số chỗ ngồi")]
    public int NumberOfSeats { get; set; }
    public int TransmissionId { get; set; }
    public Transmission? Transmission { get; set; }
    public int EngineTypeId { get; set; }
    public EngineType? EngineType { get; set; }
    public int VehicleLineId { get; set; }
    public VehicleLine? VehicleLine { get; set; }
}