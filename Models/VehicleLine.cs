namespace CarWebMVC.Models;
public class VehicleLine
{
  public int Id { get; set; }
  public string Name { get; set; }

  public Manufacturer Manufacturer { get; set; }
  public VehicleType VehicleType { get; set; }
}