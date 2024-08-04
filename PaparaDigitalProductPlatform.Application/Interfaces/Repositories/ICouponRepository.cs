using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Interfaces.Repositories;

public interface ICouponRepository
{
    Task AddAsync(Coupon coupon);
    Task<List<Coupon>> GetAllAsync();
    Task DeleteAsync(int id);
}