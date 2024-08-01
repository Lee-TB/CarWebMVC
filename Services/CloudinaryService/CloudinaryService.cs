using CarWebMVC.Models;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;

namespace CarWebMVC.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly CloudinarySettings _cloudinarySettings;
    public CloudinaryService(IOptions<CloudinarySettings> cloudinaryOptions)
    {
        _cloudinarySettings = cloudinaryOptions.Value;
    }

    public SignatureResponse GenerateSignature()
    {
        Account account = new Account(
            _cloudinarySettings.CloudName,
            _cloudinarySettings.ApiKey,
            _cloudinarySettings.ApiSecret
        );

        Cloudinary cloudinary = new Cloudinary(account);

        var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

        var parameters = new SortedDictionary<string, object>
        {
            {"timestamp", timestamp},
            {"source", "uw"},
            {"upload_preset", _cloudinarySettings.UploadPreset}
        };
        var signature = cloudinary.Api.SignParameters(parameters);

        return new SignatureResponse()
        {
            Signature = signature,
            Timestamp = timestamp,
            ApiKey = _cloudinarySettings.ApiKey,
            CloudName = _cloudinarySettings.CloudName,
            UploadPreset = _cloudinarySettings.UploadPreset
        };
    }
}