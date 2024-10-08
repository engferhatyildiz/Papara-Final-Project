﻿namespace PaparaDigitalProductPlatform.Application.Dtos;

public class OrderDto
{
    public int UserId { get; set; }
    public List<OrderDetailDto> OrderDetails { get; set; }
    public string? CouponCode { get; set; }
    public decimal? PointAmount { get; set; } 
    
    public CreditCardInfoDto CreditCardInfo { get; set; }
}