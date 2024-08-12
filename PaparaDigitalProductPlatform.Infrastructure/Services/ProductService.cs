using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ApiResponse<Product>> AddProduct(ProductDto productDto)
        {
            var category = await ValidateCategory(productDto.CategoryName);
            if (category == null)
            {
                return CreateErrorResponse<Product>("Invalid category name");
            }

            var existingProduct = await _productRepository.GetByNameAsync(productDto.Name);
            if (existingProduct != null)
            {
                return CreateErrorResponse<Product>("A product with this name already exists.");
            }

            var product = MapProductDtoToProduct(productDto, category.Id);

            await _productRepository.AddAsync(product);

            return CreateSuccessResponse(product, "Product added successfully");
        }

        public async Task<ApiResponse<string>> UpdateProductByName(ProductDto productDto)
        {
            var product = await _productRepository.GetByNameAsync(productDto.Name);
            if (product == null)
            {
                return CreateErrorResponse<string>("Product not found");
            }

            var category = await ValidateCategory(productDto.CategoryName);
            if (category == null)
            {
                return CreateErrorResponse<string>("Invalid category name");
            }

            UpdateProductDetails(product, productDto, category.Id);

            await _productRepository.UpdateAsync(product);

            return CreateSuccessResponse("Product updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteProductByName(string name)
        {
            var product = await _productRepository.GetByNameAsync(name);
            if (product == null)
            {
                return CreateErrorResponse<string>("Product not found");
            }

            await _productRepository.DeleteAsync(product.Id);

            return CreateSuccessResponse("Product deleted successfully");
        }

        public async Task<ApiResponse<List<Product>>> GetProductsByCategoryName(string categoryName)
        {
            var category = await ValidateCategory(categoryName);
            if (category == null)
            {
                return CreateErrorResponse<List<Product>>("Category not found");
            }

            var products = await _productRepository.GetByCategoryAsync(category.Id);
            if (products == null || !products.Any())
            {
                return CreateErrorResponse<List<Product>>("No products found for the given category");
            }

            return CreateSuccessResponse(products.ToList(), "Products retrieved successfully");
        }

        public async Task<ApiResponse<List<Product>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            if (products == null || !products.Any())
            {
                return CreateErrorResponse<List<Product>>("No products found");
            }

            return CreateSuccessResponse(products.ToList(), "All products retrieved successfully");
        }

        private async Task<Category?> ValidateCategory(string categoryName)
        {
            return await _categoryRepository.GetByNameAsync(categoryName);
        }

        private static Product MapProductDtoToProduct(ProductDto productDto, int categoryId)
        {
            return new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                IsActive = productDto.IsActive,
                PointRate = productDto.PointRate,
                MaxPoint = productDto.MaxPoint,
                Stock = productDto.Stock,
                CategoryId = categoryId
            };
        }

        private static void UpdateProductDetails(Product product, ProductDto productDto, int categoryId)
        {
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.IsActive = productDto.IsActive;
            product.PointRate = productDto.PointRate;
            product.MaxPoint = productDto.MaxPoint;
            product.Stock = productDto.Stock;
            product.CategoryId = categoryId;
        }

        private static ApiResponse<T> CreateErrorResponse<T>(string message)
        {
            return new ApiResponse<T> { Success = false, Message = message, Data = default };
        }

        private static ApiResponse<T> CreateSuccessResponse<T>(T data, string message)
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };
        }

        private static ApiResponse<string> CreateSuccessResponse(string message)
        {
            return new ApiResponse<string> { Success = true, Message = message, Data = null };
        }
    }
}