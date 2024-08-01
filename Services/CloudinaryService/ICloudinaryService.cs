using CarWebMVC.Models;

namespace CarWebMVC.Services;

public interface ICloudinaryService
{
    public SignatureResponse GenerateSignature();
}