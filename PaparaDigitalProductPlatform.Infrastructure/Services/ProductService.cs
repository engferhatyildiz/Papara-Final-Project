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
            // Öncelikle categoryId'nin geçerli olup olmadığını kontrol edelim
            // Kategori adı üzerinden kontrol yapın
            var categoryExists = await _categoryRepository.ExistsAsync(productDto.Name); // CategoryId yerine CategoryName'i kontrol edin

            if (!categoryExists)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Invalid category ID",
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

            // Kategori ID geçerliyse ve ürün ismi benzersizse, ürünü eklemeye devam edin
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                IsActive = productDto.IsActive,
                PointRate = productDto.PointRate,
                MaxPoint = productDto.MaxPoint,
                CategoryId = productDto.CategoryId
            };

            await _productRepository.AddAsync(product);

            return new ApiResponse<Product>
            {
                Success = true,
                Message = "Product added successfully",
                Data = product
            };
        }

        public async Task<ApiResponse<string>> UpdateProduct(ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(productDto.Id);
            if (product == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Product not found",
                    Data = null
                };
            }

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.IsActive = productDto.IsActive;
            product.PointRate = productDto.PointRate;
            product.MaxPoint = productDto.MaxPoint;
            product.CategoryId = productDto.CategoryId;

            await _productRepository.UpdateAsync(product);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Product updated successfully",
                Data = null
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

            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.IsActive = productDto.IsActive;
            product.PointRate = productDto.PointRate;
            product.MaxPoint = productDto.MaxPoint;
            product.CategoryId = productDto.CategoryId;

            await _productRepository.UpdateAsync(product);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Product updated successfully",
                Data = null
            };
        }

        public async Task<ApiResponse<string>> DeleteProduct(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Product not found",
                    Data = null
                };
            }

            await _productRepository.DeleteAsync(productId);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Product deleted successfully",
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

        public async Task<ApiResponse<List<Product>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
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
