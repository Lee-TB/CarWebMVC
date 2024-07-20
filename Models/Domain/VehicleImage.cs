using System.ComponentModel.DataAnnotations;

namespace CarWebMVC.Models.Domain;

public class VehicleImage
{
    public int Id { get; set; }
    [Required]
    [StringLength(255)]
    public string ImageUrl { get; set; }
    public int VehicleModelId { get; set; }
}
