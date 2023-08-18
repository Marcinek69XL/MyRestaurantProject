using AutoMapper;
using MyRestaurantProject.Entities;
using MyRestaurantProject.Models;

namespace MyRestaurantProject
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(d => d.City,
                    x => x.MapFrom(x => x.Address.City))
                .ForMember(d => d.Street,
                    x => x.MapFrom(x => x.Address.Street))
                .ForMember(d => d.PostalCode,
                    x => x.MapFrom(x => x.Address.PostalCode));

            CreateMap<Dish, DishDto>();
        }
    }
}
