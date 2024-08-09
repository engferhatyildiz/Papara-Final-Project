using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;

namespace PaparaDigitalProductPlatform.Validation;

public class CategoryValidator : AbstractValidator<CategoryDto>
{
    public CategoryValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty().WithMessage("Kategori adı boş olamaz.")
            .Length(3, 100).WithMessage("Kategori adı 3 ile 100 karakter arasında olmalıdır.");

        RuleFor(category => category.Url)
            .NotEmpty().WithMessage("Url) adı boş olamaz.")
            .Length(3, 100).WithMessage("Url) adı 3 ile 100 karakter arasında olmalıdır.");

        RuleFor(category => category.Tags)
            .NotEmpty().WithMessage("Tag adı boş olamaz.")
            .Length(3, 100).WithMessage("Tag adı 3 ile 100 karakter arasında olmalıdır.");
    }
}