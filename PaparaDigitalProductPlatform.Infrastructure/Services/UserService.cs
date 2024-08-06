using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Register(UserRegistrationDto userRegistrationDto)
    {
        var user = new User
        {
            FirstName = userRegistrationDto.FirstName,
            LastName = userRegistrationDto.LastName,
            Email = userRegistrationDto.Email,
            Password = userRegistrationDto.Password, // Hash password in real implementation
            Role = "User",
            WalletBalance = 0,
            PointBalance = 0
        };

        await _userRepository.AddAsync(user);
        return user;
    }

    public async Task<User> RegisterAdmin(AdminRegistrationDto adminRegistrationDto)
    {
        var user = new User
        {
            FirstName = adminRegistrationDto.FirstName,
            LastName = adminRegistrationDto.LastName,
            Email = adminRegistrationDto.Email,
            Password = adminRegistrationDto.Password, // Hash password in real implementation
            Role = "Admin",
            WalletBalance = 0,
            PointBalance = 0
        };

        await _userRepository.AddAsync(user);
        return user;
    }

    public async Task<User> Login(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetByEmailAndPasswordAsync(userLoginDto.Email, userLoginDto.Password);
        return user;
    }

    public async Task UpdateUser(UserUpdateDto userUpdateDto)
    {
        var user = await _userRepository.GetByIdAsync(userUpdateDto.Id);
        user.FirstName = userUpdateDto.FirstName;
        user.LastName = userUpdateDto.LastName;
        user.Email = userUpdateDto.Email;
        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteUser(int userId)
    {
        await _userRepository.DeleteAsync(userId);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }
}