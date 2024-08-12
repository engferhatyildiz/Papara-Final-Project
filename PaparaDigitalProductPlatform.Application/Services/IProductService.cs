using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services;

public interface IProductService
{
    Task<ApiResponse<Product>> AddProduct(ProductDto productDto);
    Task<ApiResponse<string>> UpdateProductByName(ProductDto productDto); 
    Task<ApiResponse<string>> DeleteProductByName(string name); 
    Task<ApiResponse<List<Product>>> GetProductsByCategoryName(string categoryName); 
    Task<ApiResponse<List<Product>>> GetAllAsync();
}