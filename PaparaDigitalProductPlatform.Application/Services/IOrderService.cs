using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services;

public interface IOrderService
{
    Task<Order> CreateOrder(OrderDto orderDto);
    Task<List<Order>> GetActiveOrders(int userId);
    Task<List<Order>> GetOrderHistory(int userId);
}