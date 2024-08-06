using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services;

public interface ICouponService
{
    Task<Coupon> CreateCoupon(CouponDto couponDto);
    Task<List<Coupon>> GetAllAsync();
    Task DeleteCoupon(int couponId);
    
    
}