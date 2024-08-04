﻿using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services;

public interface IUserService
{
    Task<User> Register(UserRegistrationDto userRegistrationDto);
    Task<User> Login(UserLoginDto userLoginDto);
    Task UpdateUser(UserUpdateDto userUpdateDto);
    Task DeleteUser(int userId);
}