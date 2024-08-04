namespace PaparaDigitalProductPlatform.Application.Dtos;

public class CouponDto
{
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpiryDate { get; set; }
}