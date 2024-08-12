using Microsoft.AspNetCore.Mvc;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Services;

namespace PaparaDigitalProductPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryDto categoryDto)
        {
            var response = await _categoryService.AddCategory(categoryDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Update(string name, CategoryDto categoryDto)
        {
            var response = await _categoryService.UpdateCategory(name, categoryDto);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var response = await _categoryService.DeleteCategory(name);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var response = await _categoryService.GetByNameAsync(name);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
    }
}
