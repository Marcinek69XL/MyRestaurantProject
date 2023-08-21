using System.Linq;
using FluentValidation;
using MyRestaurantProject.Entities;

namespace MyRestaurantProject.Models.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x => x.Password)
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Email)
                .Custom((email, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == email);
                    if (emailInUse)
                        context.AddFailure("Email", "The email is used");
                });
        }
    }
}