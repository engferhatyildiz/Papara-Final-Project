using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;

namespace PaparaDigitalProductPlatform.Validation
{
    public class UserValidator : AbstractValidator<UserRegistrationDto>
    {
        public UserValidator()
        {
            Include(new BaseUserValidator<UserRegistrationDto>()); // Generik Base Validator'ı dahil ediyoruz

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz."); // Sadece kayıt işlemi için gerekli olan kural
        }
    }
}