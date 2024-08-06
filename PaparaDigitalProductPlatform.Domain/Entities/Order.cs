namespace PaparaDigitalProductPlatform.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; } 
    public bool IsActive { get; set; } 
    public decimal TotalAmount { get; set; }
    public decimal CouponAmount { get; set; }
    public string CouponCode { get; set; }
    public decimal PointAmount { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }
}