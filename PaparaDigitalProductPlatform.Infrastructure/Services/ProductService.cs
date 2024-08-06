using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> AddProduct(ProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            IsActive = productDto.IsActive,
            PointRate = productDto.PointRate,
            MaxPoint = productDto.MaxPoint
        };

        await _productRepository.AddAsync(product);
        return product;
    }

    public async Task UpdateProduct(ProductDto productDto)
    {
        var product = await _productRepository.GetByIdAsync(productDto.Id);
        if (product != null)
        {
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.IsActive = productDto.IsActive;
            product.PointRate = productDto.PointRate;
            product.MaxPoint = productDto.MaxPoint;

            await _productRepository.UpdateAsync(product);
        }
    }

    public async Task DeleteProduct(int productId)
    {
        await _productRepository.DeleteAsync(productId);
    }

    public async Task<List<Product>> GetProductsByCategory(int categoryId)
    {
        return await _productRepository.GetByCategoryAsync(categoryId);
    }
    
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }
}