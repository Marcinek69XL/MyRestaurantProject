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
    }
}