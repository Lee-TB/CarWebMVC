using CarWebMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarWebMVC.Models.Domain;
using Microsoft.VisualStudio.TextTemplating;

namespace CarWebMVC.Controllers;

[AllowAnonymous]
[Route("[controller]")]
[Route("san-pham")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var productList = await _unitOfWork.VehicleModelRepository
            .GetAsync(includeProperties: [
                "Transmission",
                "EngineType",
                "Images"
            ]);
        return View(productList);
    }
}