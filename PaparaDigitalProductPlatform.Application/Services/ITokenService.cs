using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}