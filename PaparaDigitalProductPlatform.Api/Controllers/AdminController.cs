using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Services;

namespace PaparaDigitalProductPlatform.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")] // Sadece Admin rolündeki kullanıcılar erişebilir
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ICouponService _couponService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public AdminController(
            ICategoryService categoryService,
            ICouponService couponService,
            IProductService productService,
            IUserService userService)
        {
            _categoryService = categoryService;
            _couponService = couponService;
            _productService = productService;
            _userService = userService;
        }

        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin(AdminRegistrationDto adminRegistrationDto)
        {
            var response = await _userService.RegisterAdmin(adminRegistrationDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        // Users

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllAsync();
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpPut("users/{email}")]
        public async Task<IActionResult> UpdateUser(string email, UserDto userDto)
        {
            var response = await _userService.UpdateUserByEmail(email, userDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpDelete("users/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var response = await _userService.DeleteUserByEmail(email);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }


        // Categories

        [HttpPost("categories")]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
        {
            var response = await _categoryService.AddCategory(categoryDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut("categories/{name}")]
        public async Task<IActionResult> UpdateCategory(string name, CategoryDto categoryDto)
        {
            var response = await _categoryService.UpdateCategory(name, categoryDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpDelete("categories/{name}")]
        public async Task<IActionResult> DeleteCategory(string name)
        {
            var response = await _categoryService.DeleteCategory(name);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        // Coupons

        [HttpPost("coupons")]
        public async Task<IActionResult> CreateCoupon(CouponDto couponDto)
        {
            var response = await _couponService.CreateCoupon(couponDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut("coupons/{code}")]
        public async Task<IActionResult> UpdateCoupon(string code, CouponDto couponDto)
        {
            var response = await _couponService.UpdateCouponAsync(code, couponDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpDelete("coupons/{code}")]
        public async Task<IActionResult> DeleteCoupon(string code)
        {
            var response = await _couponService.DeleteCoupon(code);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        // Products

        [HttpPost("products")]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            var response = await _productService.AddProduct(productDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut("products/{name}")]
        public async Task<IActionResult> UpdateProduct(string name, ProductDto productDto)
        {
            productDto.Name = name;
            var response = await _productService.UpdateProductByName(productDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpDelete("products/{name}")]
        public async Task<IActionResult> DeleteProduct(string name)
        {
            var response = await _productService.DeleteProductByName(name);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
    }
}