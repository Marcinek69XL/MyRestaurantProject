using System.Linq;
using FluentValidation;

namespace MyRestaurantProject.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private readonly int[] allowedPageSizes = {
            5,10,15,20,25,50
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
        }
    }
}