using System;
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
            
            if (!_dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                _dbContext.Restaurants.AddRange(restaurants);    
            }
            if (!_dbContext.Roles.Any())
            {
                var roles = GetRoles();
                _dbContext.Roles.AddRange(roles);    
            }
            
            _dbContext.SaveChanges();
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>();
            var random = new Random();

            for (int i = 1; i <= 5000; i++)
            {
                var restaurant = new Restaurant
                {
                    Name = "Restaurant " + i,
                    Description = "Description for Restaurant " + i,
                    Category = random.Next(2) == 0 ? "Italian" : "Asian", // Losowo wybieramy kategorię
                    HasDelivery = random.Next(2) == 0,
                    ContactEmail = "restaurant" + i + "@example.com",
                    ContactNumber = random.Next(100, 1000) + "-" + random.Next(100, 1000) + "-" + random.Next(1000, 10000),
                    Address = new Address
                    {
                        City = "City " + (random.Next(2) == 0 ? "A" : "B"), // Losowo wybieramy miasto
                        Street = "Street " + (random.Next(2) == 0 ? "X" : "Y"), // Losowo wybieramy ulicę
                        PostalCode = random.Next(10000, 99999).ToString()
                    },
                    Dishes = new List<Dish>
                    {
                        new Dish { Name = "Dish 1", Description = "Description for Dish 1", Price = (decimal) (random.Next(5, 20) + random.NextDouble()) },
                        new Dish { Name = "Dish 2", Description = "Description for Dish 2", Price = (decimal) (random.Next(5, 20) + random.NextDouble()) }
                    }
                };

                restaurants.Add(restaurant);
            }

            return restaurants;
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };

            return roles;
        }
    }
}
