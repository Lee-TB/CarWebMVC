using System.ComponentModel;

namespace CarWebMVC.Models;
public class VehicleLine
{
  public int Id { get; set; }
  [DisplayName("Dòng xe")]
  public string Name { get; set; }
  public int ManufacturerId { get; set; }
  public int VehicleTypeId { get; set; }
  public Manufacturer? Manufacturer { get; set; }
  public VehicleType? VehicleType { get; set; }
}