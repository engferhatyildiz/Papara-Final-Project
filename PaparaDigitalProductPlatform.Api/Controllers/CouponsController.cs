using Microsoft.AspNetCore.Mvc;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Services;

namespace PaparaDigitalProductPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CouponsController : ControllerBase
{
    private readonly ICouponService _couponService;

    public CouponsController(ICouponService couponService)
    {
        _couponService = couponService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CouponDto couponDto)
    {
        var coupon = await _couponService.CreateCoupon(couponDto);
        return Ok(coupon);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var coupons = await _couponService.GetAllAsync();
        return Ok(coupons);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _couponService.DeleteCoupon(id);
        return NoContent();
    }
}