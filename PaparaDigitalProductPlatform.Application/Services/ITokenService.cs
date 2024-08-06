namespace PaparaDigitalProductPlatform.Domain.Entities;

public interface ITokenService
{
    string GenerateToken(User user);
}