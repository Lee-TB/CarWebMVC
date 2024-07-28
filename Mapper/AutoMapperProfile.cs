using AutoMapper;
using CarWebMVC.Models.Domain;
using CarWebMVC.Models.LuceneDTO;
using CarWebMVC.Models.ViewModels;

namespace CarWebMVC.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<VehicleModel, VehicleModelViewModel>().ReverseMap();

        CreateMap<VehicleModel, VehicleModelLuceneDTO>().ReverseMap();

    }
}