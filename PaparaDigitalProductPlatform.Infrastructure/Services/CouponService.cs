using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Infrastructure.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<ApiResponse<Coupon?>> CreateCoupon(CouponDto couponDto)
        {
            var existingCoupon = await _couponRepository.GetByCodeAsync(couponDto.Code);
            if (existingCoupon != null)
            {
                return CreateErrorResponse<Coupon?>("A coupon with this code already exists.");
            }

            var coupon = CreateNewCoupon(couponDto);
            await _couponRepository.AddAsync(coupon);

            return CreateSuccessResponse(coupon, "Coupon created successfully");
        }

        public async Task<ApiResponse<List<Coupon?>>> GetAllAsync()
        {
            var coupons = await _couponRepository.GetAllAsync();
            return CreateSuccessResponse(coupons, "Coupons retrieved successfully");
        }

        public async Task<ApiResponse<string>> DeleteCoupon(string couponCode)
        {
            var coupon = await _couponRepository.GetByCodeAsync(couponCode);
            if (coupon == null)
            {
                return CreateErrorResponse<string>("Coupon not found");
            }

            await _couponRepository.DeleteAsync(coupon.Id);
            return CreateSuccessResponse("Coupon has been successfully deleted.");
        }

        public async Task<ApiResponse<Coupon>> GetByCodeAsync(string code)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            if (coupon == null)
            {
                return CreateErrorResponse<Coupon>("Coupon not found");
            }

            return CreateSuccessResponse(coupon, "Coupon retrieved successfully");
        }

        public Task<ApiResponse<string>> IncreaseUsageCount(int couponId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool?>> GetCouponStatusAsync(string code)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            if (coupon == null)
            {
                return CreateErrorResponse<bool?>("Coupon not found");
            }

            return CreateSuccessResponse<bool?>(coupon.IsActive, "Coupon status retrieved successfully");
        }

        public async Task<ApiResponse<Coupon?>> UpdateCouponAsync(string code, CouponDto couponDto)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            if (coupon == null)
            {
                return CreateErrorResponse<Coupon?>("Coupon not found");
            }

            UpdateCouponFromDto(coupon, couponDto);
            await _couponRepository.UpdateAsync(coupon);

            return CreateSuccessResponse(coupon, "Coupon updated successfully");
        }

        // Private helper methods to keep methods under 25 lines
        private static ApiResponse<T> CreateErrorResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }

        private static ApiResponse<T?> CreateSuccessResponse<T>(T? data, string message)
        {
            return new ApiResponse<T?>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        private static ApiResponse<string> CreateSuccessResponse(string message)
        {
            return new ApiResponse<string>
            {
                Success = true,
                Message = message,
                Data = null
            };
        }

        private static Coupon CreateNewCoupon(CouponDto couponDto)
        {
            return new Coupon
            {
                Code = couponDto.Code,
                Amount = couponDto.Amount,
                ExpiryDate = couponDto.ExpiryDate,
                IsActive = true,
                UsageCount = 0
            };
        }

        private static void UpdateCouponFromDto(Coupon coupon, CouponDto couponDto)
        {
            coupon.Amount = couponDto.Amount;
            coupon.ExpiryDate = couponDto.ExpiryDate;
            coupon.IsActive = couponDto.IsActive;
        }
    }
}
