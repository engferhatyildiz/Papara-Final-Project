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

        public async Task<ApiResponse<User>> Register(UserDto userDto)
        {
            // E-posta adresiyle kayıtlı bir kullanıcı olup olmadığını kontrol et
            var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "A user with this email already exists.",
                    Data = null
                };
            }

            // Kullanıcıyı oluşturma işlemi
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password, // Şifreyi hashlemeyi unutmayın
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
            var existingUser = await _userRepository.GetByEmailAsync(adminRegistrationDto.Email);
            if (existingUser != null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "A user with this email already exists.",
                    Data = null
                };
            }
            
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


        public async Task<ApiResponse<string>> UpdateUserByEmail(string email, UserDto userDto)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "User not found",
                    Data = null
                };
            }

            // Yeni email ile başka bir kullanıcı var mı kontrol et
            if (!string.Equals(email, userDto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
                if (existingUser != null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "A user with this email already exists.",
                        Data = null
                    };
                }
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email; // Email güncelleniyor
            user.Password = userDto.Password; // Password değişikliği şifreleme işlemiyle beraber yapılmalıdır

            await _userRepository.UpdateAsync(user);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "User updated successfully",
                Data = null
            };
        }

        public async Task<ApiResponse<string>> DeleteUserByEmail(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "User not found",
                    Data = null
                };
            }

            await _userRepository.DeleteAsync(user.Id);

            return new ApiResponse<string>
            {
                Success = true,
                Message = "User deleted successfully",
                Data = null
            };
        }
        
        public async Task<ApiResponse<decimal>> GetUserPointsByEmail(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse<decimal>
                {
                    Success = false,
                    Message = "User not found",
                    Data = 0
                };
            }

            return new ApiResponse<decimal>
            {
                Success = true,
                Message = "User points retrieved successfully",
                Data = user.Points
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