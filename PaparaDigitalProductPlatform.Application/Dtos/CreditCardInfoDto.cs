namespace PaparaDigitalProductPlatform.Application.Dtos;

public class CreditCardInfoDto
{
    public string CardNumber { get; set; }
    public string ExpiryMonth { get; set; }
    public string ExpiryYear { get; set; }
    public string CVV { get; set; }
}