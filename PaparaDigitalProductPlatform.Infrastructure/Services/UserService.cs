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
            if (await UserExists(userDto.Email))
            {
                return CreateErrorResponse<User>("A user with this email already exists.");
            }

            var user = MapUserDtoToUser(userDto, "User");

            await _userRepository.AddAsync(user);

            return CreateSuccessResponse(user, "User registered successfully");
        }

        public async Task<ApiResponse<User>> RegisterAdmin(AdminRegistrationDto adminRegistrationDto)
        {
            if (await UserExists(adminRegistrationDto.Email))
            {
                return CreateErrorResponse<User>("A user with this email already exists.");
            }

            var user = MapAdminDtoToUser(adminRegistrationDto);

            await _userRepository.AddAsync(user);

            return CreateSuccessResponse(user, "Admin registered successfully");
        }

        public async Task<ApiResponse<User>> Login(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.GetByEmailAndPasswordAsync(userLoginDto.Email, userLoginDto.Password);
            if (user == null)
            {
                return CreateErrorResponse<User>("Invalid email or password");
            }

            return CreateSuccessResponse(user, "Login successful");
        }

        public async Task<ApiResponse<string>> UpdateUserByEmail(string email, UserDto userDto)
        {
            var user = await GetUserByEmail(email);
            if (user == null)
            {
                return CreateErrorResponse<string>("User not found");
            }

            if (!string.Equals(email, userDto.Email, StringComparison.OrdinalIgnoreCase) && await UserExists(userDto.Email))
            {
                return CreateErrorResponse<string>("A user with this email already exists.");
            }

            UpdateUserDetails(user, userDto);

            await _userRepository.UpdateAsync(user);

            return CreateSuccessResponse("User updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteUserByEmail(string email)
        {
            var user = await GetUserByEmail(email);
            if (user == null)
            {
                return CreateErrorResponse<string>("User not found");
            }

            await _userRepository.DeleteAsync(user.Id);

            return CreateSuccessResponse("User deleted successfully");
        }

        public async Task<ApiResponse<decimal>> GetUserPointsByEmail(string email)
        {
            var user = await GetUserByEmail(email);
            if (user == null)
            {
                return CreateErrorResponse<decimal>("User not found");
            }

            return CreateSuccessResponse(user.Points, "User points retrieved successfully");
        }

        public async Task<ApiResponse<List<User>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            if (users == null || !users.Any())
            {
                return CreateErrorResponse<List<User>>("No users found");
            }

            return CreateSuccessResponse(users.ToList(), "Users retrieved successfully");
        }

        private async Task<bool> UserExists(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }

        private async Task<User?> GetUserByEmail(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        private static User MapUserDtoToUser(UserDto userDto, string role)
        {
            return new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password,
                Role = role,
                Points = 0
            };
        }

        private static User MapAdminDtoToUser(AdminRegistrationDto adminRegistrationDto)
        {
            return new User
            {
                FirstName = adminRegistrationDto.FirstName,
                LastName = adminRegistrationDto.LastName,
                Email = adminRegistrationDto.Email,
                Password = adminRegistrationDto.Password,
                Role = "Admin",
                Points = 0
            };
        }

        private static void UpdateUserDetails(User user, UserDto userDto)
        {
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.Password = userDto.Password; 
        }

        private static ApiResponse<T> CreateErrorResponse<T>(string message)
        {
            return new ApiResponse<T> { Success = false, Message = message, Data = default };
        }

        private static ApiResponse<T> CreateSuccessResponse<T>(T data, string message)
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };
        }

        private static ApiResponse<string> CreateSuccessResponse(string message)
        {
            return new ApiResponse<string> { Success = true, Message = message, Data = null };
        }
    }
}
