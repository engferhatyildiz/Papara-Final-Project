namespace PaparaDigitalProductPlatform.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool IsActive { get; set; } 
    public decimal TotalAmount { get; set; }
    public decimal CouponAmount { get; set; }
    public string? CouponCode { get; set; }  // Kupon kodu nullable olabilir
    public decimal PointAmount { get; set; }
    public decimal EarnedPoints { get; set; } // Siparişten kazanılan puan
    public DateTime OrderDate { get; set; }  // Sipariş tarihi
    public ICollection<OrderDetail> OrderDetails { get; set; }
}