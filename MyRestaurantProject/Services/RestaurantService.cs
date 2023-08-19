using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyRestaurantProject.Entities;
using MyRestaurantProject.Models;

namespace MyRestaurantProject.Services
{
    public interface IRestaurantService
    {
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto Get(int id);
        int CreateRestaurant(CreateRestaurantDto createDto);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;

        public RestaurantService(IMapper mapper, RestaurantDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants =_dbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .ToList();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);
            return restaurantsDto;
        }
        
        public RestaurantDto Get(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Where(x => x.Id == id)
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .FirstOrDefault();

            
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
            return restaurantDto;
        }
     
        public int CreateRestaurant(CreateRestaurantDto createDto)
        {
            var restaurant = _mapper.Map<Restaurant>(createDto);
            _dbContext.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }
    }
}