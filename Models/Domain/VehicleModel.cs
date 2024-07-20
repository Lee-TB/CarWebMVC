using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWebMVC.Models.Domain;

public class VehicleModel
{
    public int Id { get; set; }
    [DisplayName("Mẫu xe")]
    [StringLength(50, ErrorMessage = "Mẫu xe không được quá 50 ký tự")]
    [Required(ErrorMessage = "Mẫu xe không được để trống")]
    public string Name { get; set; }
    [DisplayName("Giá bán")]
    [DataType(DataType.Currency, ErrorMessage = "Giá bán không hợp lệ")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }
    [DisplayName("Màu sắc")]
    [StringLength(20, ErrorMessage = "Màu sắc không được quá 20 ký tự")]
    public string? Color { get; set; }
    [DisplayName("Màu nội thất")]
    [StringLength(20, ErrorMessage = "Màu nội thất không được quá 20 ký tự")]
    public string? InteriorColor { get; set; }
    [DisplayName("Xuất xứ")]
    [StringLength(20, ErrorMessage = "Xuất xứ không được quá 20 ký tự")]
    public string? CountryOfOrigin { get; set; }
    [DisplayName("Năm sản xuất")]
    [Range(1900, 2200, ErrorMessage = "Năm sản xuất phải nằm trong khoảng từ 1900 đến 2200")]
    public int? Year { get; set; }
    [DisplayName("Số cửa")]
    [Range(0, 100, ErrorMessage = "Số cửa nằm trong khoảng từ 0 đến 100")]
    public int? NumberOfDoors { get; set; }
    [DisplayName("Số chỗ ngồi")]
    [Range(1, 100, ErrorMessage = "Số chỗ ngồi nằm trong khoảng từ 1 đến 100")]
    public int? NumberOfSeats { get; set; }
    public ICollection<VehicleImage>? Images { get; set; }
    [Required(ErrorMessage = "Loại hộp số không được để trống")]
    public int TransmissionId { get; set; }
    public Transmission? Transmission { get; set; }
    [Required(ErrorMessage = "Loại động cơ không được để trống")]
    public int EngineTypeId { get; set; }
    public EngineType? EngineType { get; set; }
    [Required(ErrorMessage = "Dòng xe không được để trống")]
    public int VehicleLineId { get; set; }
    public VehicleLine? VehicleLine { get; set; }
}