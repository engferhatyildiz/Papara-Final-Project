using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category> GetByIdAsync(int id);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
}