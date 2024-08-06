using Microsoft.AspNetCore.Mvc;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderDto orderDto)
    {
        var order = await _orderService.CreateOrder(orderDto);
        return Ok(order);
    }

    [HttpGet("active/{userId}")]
    public async Task<IActionResult> GetActiveOrders(int userId)
    {
        var orders = await _orderService.GetActiveOrders(userId);
        return Ok(orders);
    }

    [HttpGet("history/{userId}")]
    public async Task<IActionResult> GetOrderHistory(int userId)
    {
        var orders = await _orderService.GetOrderHistory(userId);
        return Ok(orders);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }
}