using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaparaDigitalProductPlatform.Application.Services
{
    public interface IProductService
    {
        Task<ApiResponse<Product>> AddProduct(ProductDto productDto);
        Task<ApiResponse<string>> UpdateProduct(ProductDto productDto);
        Task<ApiResponse<string>> DeleteProduct(int productId);
        Task<ApiResponse<List<Product>>> GetProductsByCategory(int categoryId);
        Task<ApiResponse<List<Product>>> GetAllAsync();
    }
}