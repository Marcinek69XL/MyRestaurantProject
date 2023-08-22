using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyRestaurantProject.Entities;
using MyRestaurantProject.Exceptions;
using MyRestaurantProject.Models;
using MyRestaurantProject.Settings;

namespace MyRestaurantProject.Services
{
    public interface IAccountService
    {
        public int RegisterUser(RegisterUserDto dto);
        string GenerateJWT(LoginUserDto dto);
    }

    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _hasher;
        private readonly AuthenticationSettings _authSettings;
        private readonly RestaurantDbContext _db;

        public AccountService(IMapper mapper,
            IPasswordHasher<User> hasher,
            AuthenticationSettings authSettings,
            RestaurantDbContext db)
        {
            _mapper = mapper;
            _hasher = hasher;
            _authSettings = authSettings;
            _db = db;
        }
        
        public int RegisterUser(RegisterUserDto dto)
        {
            var newUser = _mapper.Map<RegisterUserDto, User>(dto);
            newUser.PasswordHash = _hasher.HashPassword(newUser, dto.Password);

            _db.Users.Add(newUser);
            _db.SaveChanges();

            return newUser.Id;
        }

        public string GenerateJWT(LoginUserDto dto)
        {
            var userInDb = _db.Users
                .Include(user => user.Role)
                .FirstOrDefault(x => x.Email == dto.Email);

            if (userInDb is null)
                throw new BadRequestException("Login failed, email with password not found");

            var isPasswordSame = _hasher.VerifyHashedPassword(userInDb, userInDb.PasswordHash, dto.Password);
            if (isPasswordSame == PasswordVerificationResult.Failed)
                throw new BadRequestException("Login failed, email with password not found");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userInDb.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{userInDb.FirstName} {userInDb.LastName}"),
                new Claim(ClaimTypes.Role, userInDb.Role.Name),
                new Claim("DateOfBirth", userInDb.BirthDate.Value.ToString("d")),
                new Claim("Nationality", userInDb.Nationality)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                issuer: _authSettings.JwtIssuer,
                audience: _authSettings.JwtIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}