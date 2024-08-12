using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Domain.Entities;

public interface IProductService
{
    Task<ApiResponse<Product>> AddProduct(ProductDto productDto);
    Task<ApiResponse<string>> UpdateProductByName(ProductDto productDto); // Yeni metod
    Task<ApiResponse<string>> DeleteProductByName(string name); // Yeni metod
    Task<ApiResponse<List<Product>>> GetProductsByCategoryName(string categoryName); // Yeni metod
    Task<ApiResponse<List<Product>>> GetAllAsync();
}