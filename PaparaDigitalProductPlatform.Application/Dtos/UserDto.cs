namespace PaparaDigitalProductPlatform.Application.Dtos;

public class UserDto : IUserBase
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}