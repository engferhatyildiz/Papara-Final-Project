using System.Text.Json.Serialization;

namespace PaparaDigitalProductPlatform.Domain.Entities;

public class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    
    [JsonIgnore]
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public decimal Price { get; set; }
}