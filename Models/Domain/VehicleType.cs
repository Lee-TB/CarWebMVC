using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarWebMVC.Models.Domain;

public class VehicleType
{
    public int Id { get; set; }
    [DisplayName("Kiểu dáng")]
    [StringLength(20, ErrorMessage = "Kiểu dáng không được quá 20 ký tự")]
    [Required(ErrorMessage = "Kiểu dáng không được để trống")]
    public string Name { get; set; }
}