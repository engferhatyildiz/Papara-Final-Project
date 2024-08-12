using System.Text.Json.Serialization;

namespace PaparaDigitalProductPlatform.Application.Dtos;

public class CategoryDto
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string Tags { get; set; }
}