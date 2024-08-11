namespace PaparaDigitalProductPlatform.Application.Dtos;

public interface IUserBase
{
    string FirstName { get; set; }
    string LastName { get; set; }
    string Email { get; set; }
}