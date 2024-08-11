using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;

namespace PaparaDigitalProductPlatform.Validation
{
    public class BaseUserValidator<T> : AbstractValidator<T> where T : IUserBase
    {
        public BaseUserValidator()
        {
            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .Length(3, 50).WithMessage("Kullanıcı adı 3 ile 50 karakter arasında olmalıdır.");

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("Kullanıcı soyadı boş olamaz.")
                .Length(3, 50).WithMessage("Kullanıcı soyadı 3 ile 50 karakter arasında olmalıdır.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
        }
    }
}