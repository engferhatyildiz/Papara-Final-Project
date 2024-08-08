using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

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
            var user = await _userService.Register(userRegistrationDto);
            return Ok(user);
        }
        
        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin(AdminRegistrationDto adminRegistrationDto)
        {
            var user = await _userService.RegisterAdmin(adminRegistrationDto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var user = await _userService.Login(userLoginDto);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto userUpdateDto)
        {
            await _userService.UpdateUser(userUpdateDto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }
    }
}