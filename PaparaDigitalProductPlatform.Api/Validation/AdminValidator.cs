using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;

namespace PaparaDigitalProductPlatform.Validation;

public class AdminValidator : AbstractValidator<AdminRegistrationDto>
{
    public AdminValidator()
    {
        RuleFor(user => user.FirstName)
            .NotEmpty().WithMessage("Admin kullanıcı adı boş olamaz.")
            .Length(3, 50).WithMessage("Kullanıcı adı 3 ile 50 karakter arasında olmalıdır.");

        RuleFor(user => user.LastName)
            .NotEmpty().WithMessage("Admin kullanıcı soyadı boş olamaz.")
            .Length(3, 50).WithMessage("Kullanıcı adı 3 ile 50 karakter arasında olmalıdır.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

        
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz.");
    }
}