using System.Collections.Generic;
using System.Linq;
using MyRestaurantProject.Entities;

namespace MyRestaurantProject
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (!_dbContext.Database.CanConnect())
                return;
            if (_dbContext.Restaurants.Any())
                return;

            var restaurants = GetRestaurants();
            _dbContext.Restaurants.AddRange(restaurants);
            _dbContext.SaveChanges();
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>
            {
                new Restaurant
                {
                    Name = "Restaurant A",
                    Description = "Delicious food in a cozy ambiance.",
                    Category = "Italian",
                    HasDelivery = true,
                    ContactEmail = "restaurantA@example.com",
                    ContactNumber = "123-456-7890",
                    Address = new Address
                    {
                        City = "City A",
                        Street = "Street A",
                        PostalCode = "12345"
                    },
                    Dishes = new List<Dish>
                    {
                        new Dish { Name = "Pasta Carbonara", Description = "Creamy pasta with bacon and cheese.", Price = 12.99M },
                        new Dish { Name = "Margherita Pizza", Description = "Classic tomato and mozzarella pizza.", Price = 10.49M }
                    }
                },
                new Restaurant
                {
                    Name = "Restaurant B",
                    Description = "Authentic Asian cuisine.",
                    Category = "Asian",
                    HasDelivery = true,
                    ContactEmail = "restaurantB@example.com",
                    ContactNumber = "987-654-3210",
                    Address = new Address
                    {
                        City = "City B",
                        Street = "Street B",
                        PostalCode = "54321"
                    },
                    Dishes = new List<Dish>
                    {
                        new Dish { Name = "Pad Thai", Description = "Stir-fried noodles with shrimp and peanuts.", Price = 11.49M },
                        new Dish { Name = "Sushi Platter", Description = "Assortment of fresh sushi rolls.", Price = 15.99M }
                    }
                }
            };

            return restaurants;
        }
    }
}
