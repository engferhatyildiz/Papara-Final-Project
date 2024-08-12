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
            // Aynı isme sahip kategori var mı kontrol ediliyor
            var existingCategory = await _categoryRepository.GetByNameAsync(categoryDto.Name);
            if (existingCategory != null)
            {
                return new ApiResponse<Category>
                {
                    Success = false,
                    Message = "A category with this name already exists.",
                    Data = null
                };
            }

            // Yeni kategori oluşturuluyor
            var category = new Category
            {
                Name = categoryDto.Name,
                Url = categoryDto.Url,
                Tags = categoryDto.Tags
            };

            await _categoryRepository.AddAsync(category);

            return new ApiResponse<Category>
            {
                Success = true,
                Message = "Category added successfully",
                Data = category
            };
        }

        public async Task<ApiResponse<string>> UpdateCategory(string name, CategoryDto categoryDto)
        {
            // Kategori ismi ile veritabanında var mı kontrol ediliyor
            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Category not found",
                    Data = null
                };
            }

            // Kategori güncelleniyor
            category.Name = categoryDto.Name;
            category.Url = categoryDto.Url;
            category.Tags = categoryDto.Tags;

            await _categoryRepository.UpdateAsync(category);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Category updated successfully",
                Data = null
            };
        }

        public async Task<ApiResponse<string>> DeleteCategory(string name)
        {
            // Kategori ismi ile veritabanında var mı kontrol ediliyor
            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Category not found",
                    Data = null
                };
            }

            // Kategoride ürün olup olmadığı kontrol ediliyor
            var hasProducts = await _categoryRepository.HasProductsAsync(category.Id);
            if (hasProducts)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Cannot delete category. Category contains products.",
                    Data = null
                };
            }

            // Kategori siliniyor
            await _categoryRepository.DeleteAsync(category);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Category deleted successfully",
                Data = null
            };
        }

        public async Task<ApiResponse<List<Category>>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null || !categories.Any())
            {
                return new ApiResponse<List<Category>>
                {
                    Success = false,
                    Message = "No categories found",
                    Data = null
                };
            }

            return new ApiResponse<List<Category>>
            {
                Success = true,
                Message = "Categories retrieved successfully",
                Data = categories.ToList()
            };
        }

        public async Task<ApiResponse<Category>> GetByNameAsync(string name)
        {
            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null)
            {
                return new ApiResponse<Category>
                {
                    Success = false,
                    Message = "Category not found",
                    Data = null
                };
            }

            return new ApiResponse<Category>
            {
                Success = true,
                Message = "Category retrieved successfully",
                Data = category
            };
        }
    }
}
