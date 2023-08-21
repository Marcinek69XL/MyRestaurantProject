using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyRestaurantProject.Entities;
using MyRestaurantProject.Exceptions;
using MyRestaurantProject.Models;

namespace MyRestaurantProject.Services
{
    public interface IAccountService
    {
        public int RegisterUser(RegisterUserDto dto);
    }

    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _hasher;
        private readonly RestaurantDbContext _db;

        public AccountService(IMapper mapper, IPasswordHasher<User> hasher, RestaurantDbContext db)
        {
            _mapper = mapper;
            _hasher = hasher;
            _db = db;
        }
        
        public int RegisterUser(RegisterUserDto dto)
        {
            var user = _db.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user != null)
                throw new NotFoundException($"Email '{user.Email}' is used");
            
            var newUser = _mapper.Map<RegisterUserDto, User>(dto);
            newUser.PasswordHash = _hasher.HashPassword(newUser, dto.Password);

            _db.Users.Add(newUser);
            _db.SaveChanges();

            return newUser.Id;
        }
    }
}