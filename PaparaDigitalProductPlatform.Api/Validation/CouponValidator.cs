using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;

namespace PaparaDigitalProductPlatform.Validation;

public class CouponValidator : AbstractValidator<CouponDto>
{
    public CouponValidator()
    {
        RuleFor(coupon => coupon.Code)
            .NotEmpty().WithMessage("Kupon kodu boş olamaz.")
            .Length(1, 10).WithMessage("Kupon kodu maksimum 10 karakter uzumluğunda olmalıdır.");

        RuleFor(coupon => coupon.Amount)
            .GreaterThan(0).WithMessage("İndirim tutarı sıfırdan büyük olmalıdır.");

        RuleFor(coupon => coupon.ExpiryDate)
            .GreaterThan(DateTime.Now).WithMessage("Kuponun son kullanma tarihi geçmiş olamaz.");
        
    }
}