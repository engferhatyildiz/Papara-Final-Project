using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services;

public interface ICategoryService
{
    Task<Category> AddCategory(CategoryDto categoryDto);
    Task UpdateCategory(CategoryDto categoryDto);
    Task DeleteCategory(int categoryId);
    Task<IEnumerable<Category>> GetAllAsync();
}