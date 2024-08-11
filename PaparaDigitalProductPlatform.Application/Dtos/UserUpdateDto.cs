using System.Text.Json.Serialization;

namespace PaparaDigitalProductPlatform.Application.Dtos;

public class UserUpdateDto : IUserBase
{
    [JsonIgnore]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}