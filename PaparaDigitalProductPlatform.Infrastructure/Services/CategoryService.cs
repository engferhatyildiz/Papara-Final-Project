using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ApiResponse<Category>> AddCategory(CategoryDto categoryDto)
        {
            var existingCategory = await _categoryRepository.GetByNameAsync(categoryDto.Name);
            if (existingCategory != null)
            {
                return CreateErrorResponse<Category>("A category with this name already exists.");
            }

            var category = CreateCategoryFromDto(categoryDto);
            await _categoryRepository.AddAsync(category);

            return CreateSuccessResponse(category, "Category added successfully");
        }

        public async Task<ApiResponse<string>> UpdateCategory(string name, CategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null)
            {
                return CreateErrorResponse<string>("Category not found");
            }

            UpdateCategoryFromDto(category, categoryDto);
            await _categoryRepository.UpdateAsync(category);

            return CreateSuccessResponse("Category updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteCategory(string name)
        {
            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null)
            {
                return CreateErrorResponse<string>("Category not found");
            }

            var canDelete = await CanDeleteCategory(category);
            if (!canDelete)
            {
                return CreateErrorResponse<string>("Cannot delete category. Category contains products.");
            }

            await _categoryRepository.DeleteAsync(category);
            return CreateSuccessResponse("Category deleted successfully");
        }

        public async Task<ApiResponse<List<Category>>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            if (!categories.Any())
            {
                return CreateErrorResponse<List<Category>>("No categories found");
            }

            return CreateSuccessResponse(categories.ToList(), "Categories retrieved successfully");
        }

        public async Task<ApiResponse<Category>> GetByNameAsync(string name)
        {
            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null)
            {
                return CreateErrorResponse<Category>("Category not found");
            }

            return CreateSuccessResponse(category, "Category retrieved successfully");
        }

        private static ApiResponse<T> CreateErrorResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }

        private static ApiResponse<T> CreateSuccessResponse<T>(T data, string message)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        private static ApiResponse<string> CreateSuccessResponse(string message)
        {
            return new ApiResponse<string>
            {
                Success = true,
                Message = message,
                Data = null
            };
        }

        private static Category CreateCategoryFromDto(CategoryDto categoryDto)
        {
            return new Category
            {
                Name = categoryDto.Name,
                Url = categoryDto.Url,
                Tags = categoryDto.Tags
            };
        }

        private static void UpdateCategoryFromDto(Category category, CategoryDto categoryDto)
        {
            category.Name = categoryDto.Name;
            category.Url = categoryDto.Url;
            category.Tags = categoryDto.Tags;
        }

        private async Task<bool> CanDeleteCategory(Category category)
        {
            return !await _categoryRepository.HasProductsAsync(category.Id);
        }
    }
}
