using System.ComponentModel;
using CarWebMVC.Models.Domain;

namespace CarWebMVC.Models.ViewModels;

public class VehicleModelViewModel : VehicleModel
{
    public List<string> ImageUrls => Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>();
    [DisplayName("Hình ảnh")]
    public string? Thumbnail => Images?.Count > 0 ? GetCloudinaryThumbnail(Images.ElementAt(0).ImageUrl) : "";
    [DisplayName("Hộp số")]
    public string? TransmissionName { get; set; }
    [DisplayName("Loại động cơ")]
    public string? EngineTypeName { get; set; }
    [DisplayName("Dòng xe")]
    public string? VehicleLineName { get; set; }

    private string GetCloudinaryThumbnail(string imageUrl, int width = 160, int height = 90)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            return "";
        }

        var uploadIndex = imageUrl.IndexOf("upload");
        if (uploadIndex == -1)
        {
            return imageUrl; // Return the original URL if "upload" is not found
        }

        var prefix = imageUrl.Substring(0, uploadIndex + "upload".Length);
        var suffix = imageUrl.Substring(uploadIndex + "upload".Length);

        var transformation = $"/c_thumb,w_{width},h_{height}";
        var thumbnailUrl = $"{prefix}{transformation}{suffix}";

        return thumbnailUrl;
    }
}