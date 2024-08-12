using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services;

public interface ICategoryService
{
    Task<ApiResponse<Category>> AddCategory(CategoryDto categoryDto);

    Task<ApiResponse<string>> UpdateCategory(string name, CategoryDto categoryDto);

    Task<ApiResponse<string>> DeleteCategory(string name);

    Task<ApiResponse<List<Category>>> GetAllAsync();

    Task<ApiResponse<Category>> GetByNameAsync(string name);
}