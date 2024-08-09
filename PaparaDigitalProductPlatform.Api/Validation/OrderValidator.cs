using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;

namespace PaparaDigitalProductPlatform.Validation;

public class OrderValidator : AbstractValidator<OrderDto>
{
    public OrderValidator()
    {
        RuleFor(order => order.CouponCode)
            .NotEmpty().WithMessage("Kupon kodu boş olamaz.")
            .Length(1, 10).WithMessage("Kupon kodu maksimum 10 karakter uzumluğunda olmalıdır.");

        RuleFor(order => order.CouponAmount)
            .GreaterThan(0).WithMessage("İndirim tutarı sıfırdan büyük olmalıdır.");

        RuleFor(order => order.PointAmount)
            .GreaterThan(0).WithMessage("İndirim tutarı sıfırdan büyük olmalıdır.");
        
    }
}