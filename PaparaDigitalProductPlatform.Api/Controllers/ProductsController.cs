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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            var response = await _productService.UpdateProduct(productDto);
            if (response.Success)
            {
                return NoContent();
            }

            return NotFound(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.DeleteProduct(id);
            if (response.Success)
            {
                return NoContent();
            }

            return NotFound(response);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var response = await _productService.GetProductsByCategory(categoryId);
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