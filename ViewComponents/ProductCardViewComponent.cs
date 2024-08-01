using AutoMapper;
using CarWebMVC.Models.Domain;
using CarWebMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarWebMVC.ViewComponents;

public class ProductCardViewComponent : ViewComponent
{
    private readonly IMapper _mapper;

    public ProductCardViewComponent(IMapper mapper)
    {
        _mapper = mapper;
    }

    public IViewComponentResult Invoke(VehicleModel vehicleModel)
    {
        var viewModel = _mapper.Map<ProductCardViewModel>(vehicleModel);
        viewModel.TransmissionName = viewModel.TransmissionName?.ToLower();
        viewModel.EngineTypeName = viewModel.EngineTypeName?.ToLower();
        return View(viewModel);
    }
}
