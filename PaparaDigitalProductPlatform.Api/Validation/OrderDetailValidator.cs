using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;

namespace PaparaDigitalProductPlatform.Validation;

public class OrderDetailValidator : AbstractValidator<OrderDetailDto>
{
    public OrderDetailValidator()
    {
        // ProductId için doğrulama
        RuleFor(detail => detail.ProductId)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than zero.");


        // Quantity için doğrulama
        RuleFor(detail => detail.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}