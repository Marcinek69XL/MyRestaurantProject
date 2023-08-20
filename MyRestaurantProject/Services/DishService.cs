using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == restaurantId);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

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
            var dish = _dbContext.Dishes
                .FirstOrDefault(x => x.Id == dishId);

            if (dish is null || dish.RestaurantId != restaurantId)
                throw new NotFoundException("Dish not found");
            
            var dto = _mapper.Map<Dish, DishDto>(dish);
            return dto;
        }
    }
}