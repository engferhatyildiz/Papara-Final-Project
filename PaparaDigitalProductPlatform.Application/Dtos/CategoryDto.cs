using System.Text.Json.Serialization;

namespace PaparaDigitalProductPlatform.Application.Dtos;

public class CategoryDto
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string Tags { get; set; }
}