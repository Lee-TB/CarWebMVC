using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarWebMVC.Models.Domain;
public class VehicleLine
{
  public int Id { get; set; }
  [DisplayName("Dòng xe")]
  [StringLength(50, ErrorMessage = "Dòng xe không được quá 50 ký tự")]
  [Required(ErrorMessage = "Dòng xe không được để trống")]
  public string Name { get; set; }
  [Required(ErrorMessage = "Hãng xe không được để trống")]
  public int ManufacturerId { get; set; }
  [Required(ErrorMessage = "Kiểu dáng không được để trống")]
  public int VehicleTypeId { get; set; }
  public Manufacturer? Manufacturer { get; set; }
  public VehicleType? VehicleType { get; set; }
}