using Microsoft.AspNetCore.Mvc;
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
    }
}