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
        public async Task<IActionResult> Create(CouponDto couponDto)
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
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _couponService.DeleteCoupon(id);
            if (response.Success)
            {
                return Ok(response); // Mesajı içerir
            }

            return NotFound(response);
        }


        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var response = await _couponService.GetByCodeAsync(code);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
    }
}