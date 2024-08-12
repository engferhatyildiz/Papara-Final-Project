using Microsoft.AspNetCore.Mvc;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Services;

namespace PaparaDigitalProductPlatform.Controllers
{
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
        public async Task<IActionResult> CreateCoupon(CouponDto couponDto)
        {
            var response = await _couponService.CreateCoupon(couponDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _couponService.GetAllAsync();
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
        
        [HttpGet("{code}/status")]
        public async Task<IActionResult> GetCouponStatus(string code)
        {
            var response = await _couponService.GetCouponStatusAsync(code);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
        
        [HttpPut("{code}")]
        public async Task<IActionResult> UpdateCoupon(string code, CouponDto couponDto)
        {
            var response = await _couponService.UpdateCouponAsync(code, couponDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteCoupon(string code)
        {
            var response = await _couponService.DeleteCoupon(code);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

       

    }
}