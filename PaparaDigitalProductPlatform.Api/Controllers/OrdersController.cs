using Microsoft.AspNetCore.Mvc;
using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Services;

namespace PaparaDigitalProductPlatform.Controllers;

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
        var response = await _orderService.CreateOrder(orderDto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet("active/{userId}")]
    public async Task<IActionResult> GetActiveOrders(int userId)
    {
        var response = await _orderService.GetActiveOrders(userId);
        if (response.Success)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpGet("history/{userId}")]
    public async Task<IActionResult> GetOrderHistory(int userId)
    {
        var response = await _orderService.GetOrderHistory(userId);
        if (response.Success)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _orderService.GetAllAsync();
        if (response.Success)
        {
            return Ok(response);
        }
        return NotFound(response);
    }
}