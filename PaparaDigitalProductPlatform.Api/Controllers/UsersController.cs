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
        public async Task<IActionResult> Register(UserRegistrationDto userRegistrationDto)
        {
            var response = await _userService.Register(userRegistrationDto);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
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

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto userUpdateDto)
        {
            userUpdateDto.Id = id;

            var response = await _userService.UpdateUser(userUpdateDto);
    
            if (!response.Success)
            {
                if (response.Message == "User not found")
                {
                    return NotFound(response); 
                }

                return BadRequest(response); 
            }

            return Ok(response);
        }


        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userService.DeleteUser(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<User>>>> GetAll()
        {
            var response = await _userService.GetAllAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
