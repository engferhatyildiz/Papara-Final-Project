namespace PaparaDigitalProductPlatform.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    
    public int Stock { get; set; }
    public decimal PointRate { get; set; }
    public decimal MaxPoint { get; set; }
    public int CategoryId { get; set; }
    
    public ICollection<OrderDetail> OrderDetails { get; set; }
}