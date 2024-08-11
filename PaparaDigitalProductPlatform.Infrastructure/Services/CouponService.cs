using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using PaparaDigitalProductPlatform.Application.Services;

namespace PaparaDigitalProductPlatform.Infrastructure.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IValidator<CouponDto> _validator;

        public CouponService(ICouponRepository couponRepository, IValidator<CouponDto> validator)
        {
            _couponRepository = couponRepository;
            _validator = validator;
        }

        public async Task<ApiResponse<Coupon?>> CreateCoupon(CouponDto couponDto)
        {
            var validationResult = _validator.Validate(couponDto);
            if (!validationResult.IsValid)
            {
                return new ApiResponse<Coupon?>
                {
                    Success = false,
                    Message = "Validation failed",
                    Data = null
                };
            }

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

        public async Task<ApiResponse<string?>> DeleteCoupon(int couponId)
        {
            var coupon = await _couponRepository.GetByIdAsync(couponId);
            if (coupon == null)
            {
                return new ApiResponse<string?>
                {
                    Success = false,
                    Message = "Coupon not found",
                    Data = null
                };
            }

            await _couponRepository.DeleteAsync(couponId);

            return new ApiResponse<string?>
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

        public async Task<ApiResponse<string>> IncreaseUsageCount(int couponId)
        {
            var coupon = await _couponRepository.GetByIdAsync(couponId);
            if (coupon == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Coupon not found",
                    Data = null
                };
            }

            coupon.UsageCount++;
            await _couponRepository.UpdateAsync(coupon);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "Usage count increased successfully",
                Data = null
            };
        }
    }
}
