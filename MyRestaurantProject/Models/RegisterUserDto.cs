using System;
using System.ComponentModel.DataAnnotations;

namespace MyRestaurantProject.Models
{
    public class RegisterUserDto
    {
        public DateTime? BirthDate { get; set; }
        public string Nationality { get; set; }
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public int RoleId { get; set; } = 1;
    }
}