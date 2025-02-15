﻿using Microsoft.EntityFrameworkCore;

namespace MyRestaurantProject.Entities
{
    public class RestaurantDbContext : DbContext
    {
        private string _connectionString = "Data Source=localhost\\master,1433;User Id=sa;Password=1qaz!QAZ;";

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        

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

            modelBuilder.Entity<Address>()
                .Property(x => x.City)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(x => x.Street)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(x => x.RoleId)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(x => x.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<Role>()
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
