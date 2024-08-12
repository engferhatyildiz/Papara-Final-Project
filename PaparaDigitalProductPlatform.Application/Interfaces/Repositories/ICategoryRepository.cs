using PaparaDigitalProductPlatform.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaparaDigitalProductPlatform.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Yeni bir kategori ekler.
        /// </summary>
        /// <param name="category">Eklenecek kategori nesnesi</param>
        /// <returns>Eklenen kategori nesnesi</returns>
        Task<Category> AddAsync(Category category);

        /// <summary>
        /// Mevcut bir kategoriyi günceller.
        /// </summary>
        /// <param name="category">Güncellenecek kategori nesnesi</param>
        Task UpdateAsync(Category category);

        /// <summary>
        /// Mevcut bir kategoriyi siler.
        /// </summary>
        /// <param name="category">Silinecek kategori nesnesi</param>
        Task DeleteAsync(Category category);

        /// <summary>
        /// Kategori adını kullanarak bir kategori getirir.
        /// </summary>
        /// <param name="name">Kategori adı</param>
        /// <returns>Bulunan kategori nesnesi</returns>
        Task<Category?> GetByNameAsync(string name);

        /// <summary>
        /// Tüm kategorileri getirir.
        /// </summary>
        /// <returns>Kategorilerin listesi</returns>
        Task<List<Category>> GetAllAsync();

        /// <summary>
        /// Belirtilen ada sahip bir kategori olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="name">Kategori adı</param>
        /// <returns>Belirtilen ada sahip bir kategori varsa true döner, aksi takdirde false</returns>
        Task<bool> ExistsAsync(string name);

        /// <summary>
        /// Belirtilen ID'ye sahip bir kategorinin içinde ürün olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="categoryId">Kategori ID'si</param>
        /// <returns>Kategori ürün içeriyorsa true döner, aksi takdirde false</returns>
        Task<bool> HasProductsAsync(int categoryId);
    }
}
