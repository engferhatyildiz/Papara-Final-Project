using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> AddAsync(Category category);


        Task UpdateAsync(Category category);


        Task DeleteAsync(Category category);


        Task<Category?> GetByNameAsync(string name);


        Task<List<Category>> GetAllAsync();

        Task<bool> HasProductsAsync(int categoryId);
    }
}