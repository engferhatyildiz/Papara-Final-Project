namespace PaparaDigitalProductPlatform.Domain.Entities;

public class Coupon
{
    public int Id { get; set; }
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; }
    
    public int UsageCount { get; set; } = 0;
}