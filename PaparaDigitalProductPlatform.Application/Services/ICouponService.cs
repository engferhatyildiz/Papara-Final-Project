using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services
{
    public interface ICouponService
    {
        Task<ApiResponse<Coupon?>> CreateCoupon(CouponDto couponDto);
        Task<ApiResponse<List<Coupon?>>> GetAllAsync();
        Task<ApiResponse<string?>> DeleteCoupon(string couponCode);
        Task<ApiResponse<Coupon>> GetByCodeAsync(string code);
        Task<ApiResponse<string>> IncreaseUsageCount(int couponId);
        Task<ApiResponse<bool?>> GetCouponStatusAsync(string code);  
        Task<ApiResponse<Coupon?>> UpdateCouponAsync(string code, CouponDto couponDto); 
    }

}