using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarWebMVC.Models.Domain;

public class Manufacturer
{
    public int Id { get; set; }
    [DisplayName("Hãng xe")]
    [StringLength(50, ErrorMessage = "Tên hãng xe không được vượt quá 50 ký tự")]
    [Required(ErrorMessage = "Tên hãng xe không được để trống")]
    public string Name { get; set; }
    [DisplayName("Quốc gia")]
    [StringLength(50, ErrorMessage = "Quốc gia không được vượt quá 50 ký tự")]
    public string? Country { get; set; }
    [DisplayName("Năm thành lập")]
    [Range(1800, 2200, ErrorMessage = "Năm thành lập phải nằm trong khoảng từ 1800 đến 2200")]
    public int? FoundedYear { get; set; }
}