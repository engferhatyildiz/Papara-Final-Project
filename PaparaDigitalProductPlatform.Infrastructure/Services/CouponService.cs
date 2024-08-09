using FluentValidation;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Infrastructure.Services;

public class CouponService : ICouponService
{
    private readonly ICouponRepository _couponRepository;
    private readonly IValidator<CouponDto> _validator;

    public CouponService(ICouponRepository couponRepository,IValidator<CouponDto> validator)
    {
        _couponRepository = couponRepository;
        _validator = validator;
    }

    public async Task<Coupon> CreateCoupon(CouponDto couponDto)
    {
        var validationResult = _validator.Validate(couponDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var coupon = new Coupon
        {
            Code = couponDto.Code,
            Amount = couponDto.Amount,
            ExpiryDate = couponDto.ExpiryDate,
            IsActive = true
        };

        await _couponRepository.AddAsync(coupon);
        return coupon;
    }

    public async Task<List<Coupon>> GetAllAsync()
    {
        return await _couponRepository.GetAllAsync();
    }

    public async Task DeleteCoupon(int couponId)
    {
        await _couponRepository.DeleteAsync(couponId);
    }
}
