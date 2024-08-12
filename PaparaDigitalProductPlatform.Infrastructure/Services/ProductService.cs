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
            // Öncelikle CategoryName'in geçerli olup olmadığını kontrol edelim
            var category = await _categoryRepository.GetByNameAsync(productDto.CategoryName);
            if (category == null)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Invalid category name",
                    Data = null
                };
            }

            // Ürün ismi ile aynı ürün olup olmadığını kontrol et
            var existingProduct = await _productRepository.GetByNameAsync(productDto.Name);
            if (existingProduct != null)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "A product with this name already exists.",
                    Data = null
                };
            }

            // Kategori geçerliyse ve ürün ismi benzersizse, ürünü eklemeye devam edin
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                IsActive = productDto.IsActive,
                PointRate = productDto.PointRate,
                MaxPoint = productDto.MaxPoint,
                Stock = productDto.Stock,
                CategoryId = category.Id // CategoryName yerine CategoryId'yi ayarlıyoruz
            };

            await _productRepository.AddAsync(product);

            return new ApiResponse<Product>
            {
                Success = true,
                Message = "Product added successfully",
                Data = product
            };
        }

        public async Task<ApiResponse<string>> UpdateProductByName(ProductDto productDto)
        {
            var product = await _productRepository.GetByNameAsync(productDto.Name);
            if (product == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Product not found",
                    Data = null
                };
            }

            // Yeni kategori adının geçerli olup olmadığını kontrol edin
            var category = await _categoryRepository.GetByNameAsync(productDto.CategoryName);
            if (category == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid category name",
                    Data = null
                };
            }

            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.IsActive = productDto.IsActive;
            product.PointRate = productDto.PointRate;
            product.MaxPoint = productDto.MaxPoint;
            product.Stock = productDto.Stock;
            product.CategoryId = category.Id; // Yeni kategori ID'sini ayarlayın

            await _productRepository.UpdateAsync(product);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Product updated successfully",
                Data = null
            };
        }

        public async Task<ApiResponse<string>> DeleteProductByName(string name)
        {
            var product = await _productRepository.GetByNameAsync(name);
            if (product == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Product not found",
                    Data = null
                };
            }

            await _productRepository.DeleteAsync(product.Id);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Product deleted successfully",
                Data = null
            };
        }

        public async Task<ApiResponse<List<Product>>> GetProductsByCategoryName(string categoryName)
        {
            var category = await _categoryRepository.GetByNameAsync(categoryName);
            if (category == null)
            {
                return new ApiResponse<List<Product>>
                {
                    Success = false,
                    Message = "Category not found",
                    Data = null
                };
            }

            var products = await _productRepository.GetByCategoryAsync(category.Id);
            if (products == null || products.Count == 0)
            {
                return new ApiResponse<List<Product>>
                {
                    Success = false,
                    Message = "No products found for the given category",
                    Data = null
                };
            }

            return new ApiResponse<List<Product>>
            {
                Success = true,
                Message = "Products retrieved successfully",
                Data = products
            };
        }

        public async Task<ApiResponse<List<Product>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            if (products == null || !products.Any())
            {
                return new ApiResponse<List<Product>>
                {
                    Success = false,
                    Message = "No products found",
                    Data = null
                };
            }

            return new ApiResponse<List<Product>>
            {
                Success = true,
                Message = "All products retrieved successfully",
                Data = products.ToList()
            };
        }
    }
}