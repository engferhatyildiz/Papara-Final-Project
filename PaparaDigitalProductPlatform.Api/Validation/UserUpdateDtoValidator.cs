using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;

namespace PaparaDigitalProductPlatform.Validation
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateValidator()
        {
            Include(new BaseUserValidator<UserUpdateDto>()); // Generik Base Validator'ı dahil ediyoruz
        }
    }
}