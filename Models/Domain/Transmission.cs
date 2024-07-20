using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarWebMVC.Models.Domain;

public class Transmission
{
    public int Id { get; set; }
    [DisplayName("Loại hộp số")]
    [StringLength(30, ErrorMessage = "Loại hộp số không được quá 30 ký tự")]
    [Required(ErrorMessage = "Loại hộp số không được để trống")]
    public string Name { get; set; }
    [DisplayName("Mô tả")]
    [StringLength(1000, ErrorMessage = "Mô tả không được quá 1000 ký tự")]
    public string? Description { get; set; }
}