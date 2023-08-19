using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRestaurantProject.Entities;
using MyRestaurantProject.Models;

namespace MyRestaurantProject.Services
{
    public interface IRestaurantService
    {
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto Get(int id);
        int CreateRestaurant(CreateRestaurantDto createDto);
        bool Delete(int id);
        bool UpdateRestaurant(UpdateRestaurantDto updateDto, int id);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(IMapper mapper, RestaurantDbContext dbContext, ILogger<RestaurantService> logger)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
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

        public bool Delete(int id)
        {
            _logger.LogError($"Restaurant with id: {id} DELETE action invoked");
            
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
                return false;

            _dbContext.Remove(restaurant);
            _dbContext.SaveChanges();
            return true;
        }

        public bool UpdateRestaurant(UpdateRestaurantDto updateDto, int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == id);
            
            if (restaurant is null)
                return false;

            restaurant.Name = updateDto.Name;
            restaurant.Description = updateDto.Description;
            restaurant.HasDelivery = updateDto.HasDelivery;

            _dbContext.SaveChanges();
            return true;
        }
    }
}