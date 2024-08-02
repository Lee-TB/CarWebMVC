using CarWebMVC.Models;

namespace CarWebMVC.Services;

public interface ICloudinaryService
{
    public SignatureResponse GenerateSignature();
    public Task DeleteImagesAsync(IEnumerable<string> publicIds);
}