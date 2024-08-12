using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;

namespace PaparaDigitalProductPlatform.Validation
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must be less than 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .MaximumLength(500).WithMessage("Product description must be less than 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Product active status is required.");

            RuleFor(x => x.PointRate)
                .GreaterThan(0).WithMessage("Point rate must be greater than or equal to zero.");

            RuleFor(x => x.MaxPoint)
                .GreaterThan(0).WithMessage("Max point must be greater than or equal to zero.");

            
            RuleFor(product => product.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");
        }
    }
}