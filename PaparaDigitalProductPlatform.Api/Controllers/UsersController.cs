using Microsoft.AspNetCore.Mvc;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaparaDigitalProductPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UsersController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var response = await _userService.Register(userDto);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var response = await _userService.Login(userLoginDto);
            if (!response.Success)
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid credentials",
                    Data = null
                });
            }

            var token = _tokenService.GenerateToken(response.Data);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Login successful",
                Data = token
            });
        }

       

        [HttpGet("points")]
        public async Task<IActionResult> GetUserPointsByEmail([FromQuery] string email)
        {
            var response = await _userService.GetUserPointsByEmail(email);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }
        
        
    }
}
