using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarWebMVC.Models.ViewModels;

public class ProductCardViewModel
{
    public int Id { get; set; }
    [DisplayName("Mẫu xe")]
    public string Name { get; set; }

    [DisplayName("Giá bán")]
    [DataType(DataType.Currency)]
    public decimal? Price { get; set; }

    [DisplayName("Màu sắc")]
    public string? Color { get; set; }

    [DisplayName("Màu nội thất")]
    public string? InteriorColor { get; set; }

    [DisplayName("Xuất xứ")]
    public string? CountryOfOrigin { get; set; }

    [DisplayName("Năm sản xuất")]
    public int? Year { get; set; }

    [DisplayName("Số cửa")]
    public int? NumberOfDoors { get; set; }

    [DisplayName("Số chỗ ngồi")]
    public int? NumberOfSeats { get; set; }

    [DisplayName("Hình ảnh")]
    [DataType(DataType.ImageUrl)]
    public string? Thumbnail { get; set; }

    [DisplayName("Hộp số")]
    public string? TransmissionName { get; set; }

    [DisplayName("Loại động cơ")]
    public string? EngineTypeName { get; set; }

    [DisplayName("Dòng xe")]
    public string? VehicleLineName { get; set; }
}
