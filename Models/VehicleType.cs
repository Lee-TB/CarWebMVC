using System.ComponentModel;

namespace CarWebMVC.Models;

public class VehicleType
{
    public int Id { get; set; }
    [DisplayName("Kiểu dáng")]
    public string Name { get; set; }
}