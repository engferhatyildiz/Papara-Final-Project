using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<User>> Register(UserRegistrationDto userRegistrationDto)
        {
            var user = new User
            {
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
                Email = userRegistrationDto.Email,
                Password = userRegistrationDto.Password,
                Role = "User",
                Points = 0
            };

            await _userRepository.AddAsync(user);

            return new ApiResponse<User>
            {
                Success = true,
                Message = "User registered successfully",
                Data = user
            };
        }

        public async Task<ApiResponse<User>> RegisterAdmin(AdminRegistrationDto adminRegistrationDto)
        {
            var user = new User
            {
                FirstName = adminRegistrationDto.FirstName,
                LastName = adminRegistrationDto.LastName,
                Email = adminRegistrationDto.Email,
                Password = adminRegistrationDto.Password,
                Role = "Admin",
                Points = 0
            };

            await _userRepository.AddAsync(user);

            return new ApiResponse<User>
            {
                Success = true,
                Message = "Admin registered successfully",
                Data = user
            };
        }

        public async Task<ApiResponse<User>> Login(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.GetByEmailAndPasswordAsync(userLoginDto.Email, userLoginDto.Password);
            if (user == null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "Invalid email or password",
                    Data = null
                };
            }

            return new ApiResponse<User>
            {
                Success = true,
                Message = "Login successful",
                Data = user
            };
        }

        public async Task<ApiResponse<string>> UpdateUser(UserUpdateDto userUpdateDto)
        {
            // Öncelikle kullanıcının var olup olmadığını kontrol edin
            var user = await _userRepository.GetByIdAsync(userUpdateDto.Id);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "User not found", // Kullanıcı bulunamadı mesajı
                    Data = null
                };
            }

            // Kullanıcı bulunduysa güncelleme işlemlerini yap
            user.FirstName = userUpdateDto.FirstName;
            user.LastName = userUpdateDto.LastName;
            user.Email = userUpdateDto.Email;

            await _userRepository.UpdateAsync(user);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "User updated successfully",
                Data = null
            };
        }

        public async Task<ApiResponse<string>> DeleteUser(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "User not found",
                    Data = null
                };
            }

            await _userRepository.DeleteAsync(userId);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "User deleted successfully",
                Data = null
            };
        }

        public async Task<ApiResponse<List<User>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            if (users == null || !users.Any())
            {
                return new ApiResponse<List<User>>
                {
                    Success = false,
                    Message = "No users found",
                    Data = null
                };
            }

            return new ApiResponse<List<User>>
            {
                Success = true,
                Message = "Users retrieved successfully",
                Data = users.ToList()
            };
        }
    }
}