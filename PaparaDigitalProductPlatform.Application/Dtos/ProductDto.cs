using System.Text.Json.Serialization;

namespace PaparaDigitalProductPlatform.Application.Dtos;

public class ProductDto
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public decimal PointRate { get; set; }
    public decimal MaxPoint { get; set; }
    
    public int Stock { get; set; }
    
    public string CategoryName { get; set; }
    
    [JsonIgnore]
    public int CategoryId { get; set; }
}