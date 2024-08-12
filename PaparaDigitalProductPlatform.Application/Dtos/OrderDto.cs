﻿using System.Text.Json.Serialization;

namespace PaparaDigitalProductPlatform.Application.Dtos;

public class OrderDto
{
    public int UserId { get; set; }
    public List<OrderDetailDto> OrderDetails { get; set; }
    
    [JsonIgnore]
    public decimal CouponAmount { get; set; }
    public string CouponCode { get; set; }
    public decimal? PointAmount { get; set; }
}