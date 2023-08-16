using Microsoft.EntityFrameworkCore;

namespace MyRestaurantProject.Entities
{
    public class RestaurantDbContext : DbContext
    {
        private string _connectionString = "Data Source=localhost\\master,1433;User Id=your_username;Password=your_password;";

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        /* Nadajemy dodatkowa konfiguracje */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Dish>()
                .Property(x => x.Name)
                .IsRequired();
        }

        /* Jaki typ bazy danych, oraz jak ma wygladac polaczenie do bazy */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
