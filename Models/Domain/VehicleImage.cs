using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWebMVC.Models.Domain;

public class VehicleImage
{
    public int Id { get; set; }
    [Required]
    [StringLength(255)]
    public string ImageUrl { get; set; }
    [NotMapped]
    public string PublicId => GetPublicIdFromImageUrl(ImageUrl);
    public int VehicleModelId { get; set; }
    private string GetPublicIdFromImageUrl(string imageUrl)
    {
        var segments = imageUrl.Split('/');
        if (segments.Length < 2) return "";
        string folder = segments[segments.Length - 2];
        string id = (segments[segments.Length - 1].Split('.'))[0];        
        string publicId = $"{folder}/{id}";
        return publicId;
    }
}
