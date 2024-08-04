using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services;

public interface IProductService
{
    Task<Product> AddProduct(ProductDto productDto);
    Task UpdateProduct(ProductDto productDto);
    Task DeleteProduct(int productId);
    Task<List<Product>> GetProductsByCategory(int categoryId);
}