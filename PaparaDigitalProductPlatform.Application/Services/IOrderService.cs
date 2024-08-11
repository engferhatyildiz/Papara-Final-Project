using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Services
{
    public interface IOrderService
    {
        Task<ApiResponse<Order>> CreateOrder(OrderDto orderDto);
        Task<ApiResponse<List<Order>>> GetActiveOrders(int userId);
        Task<ApiResponse<List<Order>>> GetOrderHistory(int userId);
        Task<ApiResponse<List<Order>>> GetAllAsync();
    }
}