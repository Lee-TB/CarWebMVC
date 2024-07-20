using AutoMapper;
using CarWebMVC.Models.Domain;
using CarWebMVC.Models.ViewModels;

namespace CarWebMVC.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<VehicleModel, VehicleModelViewModel>();
    }
}