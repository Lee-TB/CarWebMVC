using CarWebMVC.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace CarWebMVC.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly CloudinarySettings _cloudinarySettings;
    private readonly Cloudinary _cloudinary;
    private readonly Account _account;
    public CloudinaryService(IOptions<CloudinarySettings> cloudinaryOptions)
    {
        _cloudinarySettings = cloudinaryOptions.Value;
        _account = new Account(
            _cloudinarySettings.CloudName,
            _cloudinarySettings.ApiKey,
            _cloudinarySettings.ApiSecret
        );
        _cloudinary = new Cloudinary(_account);
    }

    public SignatureResponse GenerateSignature()
    {       
        var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

        var parameters = new SortedDictionary<string, object>
        {
            {"timestamp", timestamp},
            {"source", "uw"},
            {"upload_preset", _cloudinarySettings.UploadPreset}
        };
        var signature = _cloudinary.Api.SignParameters(parameters);

        return new SignatureResponse()
        {
            Signature = signature,
            Timestamp = timestamp,
            ApiKey = _cloudinarySettings.ApiKey,
            CloudName = _cloudinarySettings.CloudName,
            UploadPreset = _cloudinarySettings.UploadPreset
        };
    }    

    public async Task DeleteImagesAsync(IEnumerable<string> publicIds)
    {
        await _cloudinary.DeleteResourcesAsync(ResourceType.Image, publicIds.ToArray());
    }
}