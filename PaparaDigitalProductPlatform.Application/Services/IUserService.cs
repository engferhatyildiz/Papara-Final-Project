using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services;

public interface IUserService
{
    Task<User> Register(UserRegistrationDto userRegistrationDto);
    Task<User> RegisterAdmin(AdminRegistrationDto adminRegistrationDto);
    Task<User> Login(UserLoginDto userLoginDto);
    Task UpdateUser(UserUpdateDto userUpdateDto);
    Task DeleteUser(int userId);
    Task<IEnumerable<User>> GetAllAsync();
}