using Microsoft.AspNetCore.Mvc;

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
    }
}