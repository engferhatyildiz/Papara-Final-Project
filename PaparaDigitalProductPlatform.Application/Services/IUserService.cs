using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaparaDigitalProductPlatform.Application.Services
{
    public interface IUserService
    {
        Task<ApiResponse<User>> Register(UserRegistrationDto userRegistrationDto);
        Task<ApiResponse<User>> RegisterAdmin(AdminRegistrationDto adminRegistrationDto);
        Task<ApiResponse<User>> Login(UserLoginDto userLoginDto);
        Task<ApiResponse<string>> UpdateUser(UserUpdateDto userUpdateDto);
        Task<ApiResponse<string>> DeleteUser(int id);
        Task<ApiResponse<List<User>>> GetAllAsync();
    }
}