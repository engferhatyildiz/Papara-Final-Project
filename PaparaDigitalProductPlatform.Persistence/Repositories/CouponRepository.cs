using Microsoft.EntityFrameworkCore;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Persistance.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly ApplicationDbContext _context;

    public CouponRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Coupon? coupon)
    {
        await _context.Coupons.AddAsync(coupon);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Coupon?>> GetAllAsync()
    {
        return await _context.Coupons.ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var coupon = await _context.Coupons.FindAsync(id);
        if (coupon != null)
        {
            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Coupon?> GetByIdAsync(int id)
    {
        return await _context.Coupons.FindAsync(id);
    }

    public async Task<Coupon?> GetByCodeAsync(string code)
    {
        return await _context.Coupons.FirstOrDefaultAsync(c => c != null && c.Code == code);
    }

    public async Task UpdateAsync(Coupon? coupon)
    {
        _context.Coupons.Update(coupon);
        await _context.SaveChangesAsync();
    }
}