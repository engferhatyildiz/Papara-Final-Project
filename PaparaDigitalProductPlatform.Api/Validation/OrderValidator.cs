using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;

namespace PaparaDigitalProductPlatform.Validation;

public class OrderValidator : AbstractValidator<OrderDto>
{
    public OrderValidator()
    {
        // UserId için doğrulama
        RuleFor(order => order.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than zero.");
        

        // PointAmount için doğrulama
        RuleFor(order => order.PointAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Point amount cannot be negative.");

        // OrderDetails için doğrulama
        RuleFor(order => order.OrderDetails)
            .NotNull()
            .WithMessage("Order details cannot be null.")
            .Must(orderDetails => orderDetails.Any())
            .WithMessage("Order must contain at least one product.");

        // OrderDetails içindeki her bir ürün için doğrulama
        RuleForEach(order => order.OrderDetails).SetValidator(new OrderDetailValidator());
    }
}