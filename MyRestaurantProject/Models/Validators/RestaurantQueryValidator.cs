using System.Linq;
using FluentValidation;
using MyRestaurantProject.Entities;

namespace MyRestaurantProject.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private readonly int[] allowedPageSizes = {
            5,10,15,20,25,50
        };
        private readonly string[] allowedSortByColumnName = {
            nameof(Restaurant.Description), nameof(Restaurant.Name), nameof(Restaurant.Address)
        };
        
        public RestaurantQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.PageSize)
                .Custom((value, context) =>
                {
                    if (!allowedPageSizes.Contains(value))
                        context.AddFailure("PageSize",
                            $"Value must be 1 of : {string.Join(",", allowedPageSizes)}");
                });

            RuleFor(x => x.SortBy)
                .Must(x => string.IsNullOrEmpty(x) || allowedSortByColumnName.Contains(x))
                .WithMessage($"Value must be 1 of: {string.Join(",", allowedSortByColumnName)}");
        }
    }
}