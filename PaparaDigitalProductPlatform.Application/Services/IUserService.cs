using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services
{
    public interface IUserService
    {
        Task<ApiResponse<User>> Register(UserDto userDto);
        Task<ApiResponse<User>> RegisterAdmin(AdminRegistrationDto adminRegistrationDto);
        Task<ApiResponse<User>> Login(UserLoginDto userLoginDto);
        Task<ApiResponse<string>> UpdateUserByEmail(string email, UserDto userDto);
        Task<ApiResponse<string>> DeleteUserByEmail(string email);
        Task<ApiResponse<decimal>> GetUserPointsByEmail(string email);
        Task<ApiResponse<List<User>>> GetAllAsync();
    }
}