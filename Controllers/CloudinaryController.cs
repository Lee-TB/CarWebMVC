using CarWebMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarWebMVC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CloudinaryController : ControllerBase
{
    private readonly ICloudinaryService _cloudinaryService;

    public CloudinaryController(ICloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
    }

    [HttpGet("generate-signature")]
    public IActionResult GenerateSignature()
    {
        return Ok(_cloudinaryService.GenerateSignature());
    }
}