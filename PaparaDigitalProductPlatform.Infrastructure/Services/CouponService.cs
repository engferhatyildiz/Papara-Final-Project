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
            // Öncelikle aynı koda sahip bir kuponun olup olmadığını kontrol edelim
            var existingCoupon = await _couponRepository.GetByCodeAsync(couponDto.Code);
            if (existingCoupon != null)
            {
                return new ApiResponse<Coupon?>
                {
                    Success = false,
                    Message = "A coupon with this code already exists.",
                    Data = null
                };
            }

            // Yeni kupon oluşturma
            var coupon = new Coupon
            {
                Code = couponDto.Code,
                Amount = couponDto.Amount,
                ExpiryDate = couponDto.ExpiryDate,
                IsActive = true,
                UsageCount = 0
            };

            await _couponRepository.AddAsync(coupon);

            return new ApiResponse<Coupon?>
            {
                Success = true,
                Message = "Coupon created successfully",
                Data = coupon
            };
        }

        public async Task<ApiResponse<List<Coupon?>>> GetAllAsync()
        {
            var coupons = await _couponRepository.GetAllAsync();
            return new ApiResponse<List<Coupon?>>
            {
                Success = true,
                Message = "Coupons retrieved successfully",
                Data = coupons
            };
        }

        // Coupon'u kod üzerinden silme işlemi
        public async Task<ApiResponse<string>> DeleteCoupon(string couponCode)
        {
            var coupon = await _couponRepository.GetByCodeAsync(couponCode);
            if (coupon == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Coupon not found",
                    Data = null
                };
            }

            await _couponRepository.DeleteAsync(coupon.Id);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Coupon has been successfully deleted.",
                Data = null
            };
        }


        public async Task<ApiResponse<Coupon>> GetByCodeAsync(string code)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            if (coupon == null)
            {
                return new ApiResponse<Coupon>
                {
                    Success = false,
                    Message = "Coupon not found",
                    Data = null
                };
            }

            return new ApiResponse<Coupon>
            {
                Success = true,
                Message = "Coupon retrieved successfully",
                Data = coupon
            };
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
                return new ApiResponse<bool?>
                {
                    Success = false,
                    Message = "Coupon not found",
                    Data = null
                };
            }

            return new ApiResponse<bool?>
            {
                Success = true,
                Message = "Coupon status retrieved successfully",
                Data = coupon.IsActive
            };
        }
        
        public async Task<ApiResponse<Coupon?>> UpdateCouponAsync(string code, CouponDto couponDto)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            if (coupon == null)
            {
                return new ApiResponse<Coupon?>
                {
                    Success = false,
                    Message = "Coupon not found",
                    Data = null
                };
            }

            coupon.Amount = couponDto.Amount;
            coupon.ExpiryDate = couponDto.ExpiryDate;
            coupon.IsActive = couponDto.IsActive;

            await _couponRepository.UpdateAsync(coupon);

            return new ApiResponse<Coupon?>
            {
                Success = true,
                Message = "Coupon updated successfully",
                Data = coupon
            };
        }


    }
}