using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Controllers
{
    [NonController]
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICouponService _couponService;

        public AdminController(
            IUserService userService,
            IProductService productService,
            ICategoryService categoryService,
            ICouponService couponService)
        {
            _userService = userService;
            _productService = productService;
            _categoryService = categoryService;
            _couponService = couponService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAdmin(AdminRegistrationDto adminRegistrationDto)
        {
            var user = await _userService.RegisterAdmin(adminRegistrationDto);
            return Ok(user);
        }

        [HttpPost("product")]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            var product = await _productService.AddProduct(productDto);
            return Ok(product);
        }

        [HttpPut("product")]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            await _productService.UpdateProduct(productDto);
            return NoContent();
        }

        [HttpDelete("product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }

        [HttpPost("coupon")]
        public async Task<IActionResult> AddCoupon(CouponDto couponDto)
        {
            var coupon = await _couponService.CreateCoupon(couponDto);
            return Ok(coupon);
        }

        [HttpDelete("coupon/{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            await _couponService.DeleteCoupon(id);
            return NoContent();
        }

        [HttpGet("coupon")]
        public async Task<ActionResult<IEnumerable<Coupon>>> GetAllCoupons()
        {
            var coupons = await _couponService.GetAllAsync();
            return Ok(coupons);
        }

        [HttpPost("category")]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
        {
            var category = await _categoryService.AddCategory(categoryDto);
            return Ok(category);
        }

        [HttpPut("category")]
        public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto)
        {
            await _categoryService.UpdateCategory(categoryDto);
            return NoContent();
        }

        [HttpDelete("category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}
