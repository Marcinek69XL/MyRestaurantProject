using System.Collections.Generic;

namespace MyRestaurantProject.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }

        public int AddressId { get; set; }
        
        /* Virtual daje to że umożliwia Lazy Loading */
        public virtual Address Address { get; set; }
        public virtual List<Dish> Dishes {get; set; }
    }
}
