using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyRestaurantProject.Entities;
using MyRestaurantProject.Exceptions;
using MyRestaurantProject.Models;

namespace MyRestaurantProject.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto createDishDto);
        IEnumerable<DishDto> GetDishes(int restaurantId);
        DishDto GetDish(int restaurantId, int dishId);
        void DeleteDish(int restaurantId, int dishId);
        void DeleteDishes(int restaurantId);
    }

    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            _mapper = mapper;
        }
        public int Create(int restaurantId, CreateDishDto createDishDto)
        {
            GetRestaurantById(restaurantId);

            var dish = _mapper.Map<CreateDishDto, Dish>(createDishDto);
            dish.RestaurantId = restaurantId;

            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();

            return dish.Id;
        }

        public IEnumerable<DishDto> GetDishes(int restaurantId)
        {
            var dishes = _dbContext
                .Dishes
                .Where(d => d.RestaurantId == restaurantId);

            if (dishes is null || !dishes.Any())
                throw new NotFoundException("Dishes not found");
            
            var dto = _mapper.Map<IEnumerable<Dish>, IEnumerable<DishDto>>(dishes);
            return dto;
        }

        public DishDto GetDish(int restaurantId, int dishId)
        {
            var dish = GetDishById(dishId);

            if (dish.RestaurantId != restaurantId)
                throw new NotFoundException("Dish not found");
            
            var dto = _mapper.Map<Dish, DishDto>(dish);
            return dto;
        }

        public void DeleteDish(int restaurantId, int dishId)
        {
            var dish = GetDishById(dishId);

            if (dish.RestaurantId != restaurantId)
                throw new NotFoundException("Dish not found");

            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();
        }

        public void DeleteDishes(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            
            if (!restaurant.Dishes.Any())
                throw new NotFoundException("Dishes not found");
            
            _dbContext.Dishes.RemoveRange(restaurant.Dishes);
            _dbContext.SaveChanges();
        }

        private Restaurant GetRestaurantById(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Where(r => r.Id == id)
                .Include(r => r.Dishes)
                .FirstOrDefault();
            
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }

        private Dish GetDishById(int id)
        {
            var dish = _dbContext
                .Dishes
                .FirstOrDefault(d => d.Id == id);
            
            if (dish is null)
                throw new NotFoundException("Dish not found");

            return dish;
        }
    }
}