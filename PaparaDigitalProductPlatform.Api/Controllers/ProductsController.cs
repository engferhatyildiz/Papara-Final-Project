using Microsoft.AspNetCore.Mvc;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Services;

namespace PaparaDigitalProductPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(ProductDto productDto)
        {
            var response = await _productService.AddProduct(productDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{name}")]
        public async Task<IActionResult> Update(string name, ProductDto productDto)
        {
            productDto.Name = name; // DTO'ya ad değeri atayın
            var response = await _productService.UpdateProductByName(productDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var response = await _productService.DeleteProductByName(name);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet("category/{categoryName}")]
        public async Task<IActionResult> GetByCategory(string categoryName)
        {
            var response = await _productService.GetProductsByCategoryName(categoryName);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productService.GetAllAsync();
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
    }
}
