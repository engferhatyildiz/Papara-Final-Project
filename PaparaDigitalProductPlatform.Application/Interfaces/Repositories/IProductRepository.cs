using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<Product> GetByIdAsync(int id);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    Task<List<Product>> GetByCategoryAsync(int categoryId);

    Task<IEnumerable<Product>> GetAllAsync();
}