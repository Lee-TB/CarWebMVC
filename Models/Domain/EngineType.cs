using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarWebMVC.Models.Domain;

public class EngineType
{
    public int Id { get; set; }
    [DisplayName("Loại động cơ")]
    [StringLength(30, ErrorMessage = "Loại động cơ không được quá 30 ký tự")]
    [Required(ErrorMessage = "Loại động cơ không được để trống")]
    public string Name { get; set; }
}