using System;
using System.ComponentModel.DataAnnotations;

namespace MyRestaurantProject.Models
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Nationality { get; set; }
        public string Password { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }

        public int RoleId { get; set; } = 1;
    }
}