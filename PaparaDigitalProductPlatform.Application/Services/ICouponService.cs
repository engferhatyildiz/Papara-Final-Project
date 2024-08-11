using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaparaDigitalProductPlatform.Application.Services
{
    public interface ICouponService
    {
        Task<ApiResponse<Coupon?>> CreateCoupon(CouponDto couponDto);
        Task<ApiResponse<List<Coupon?>>> GetAllAsync();
        Task<ApiResponse<string?>> DeleteCoupon(int couponId);
        Task<ApiResponse<Coupon>> GetByCodeAsync(string code);
        Task<ApiResponse<string>> IncreaseUsageCount(int couponId);
    }
}