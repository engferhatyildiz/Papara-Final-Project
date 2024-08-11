using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task AddOrderDetailAsync(OrderDetail orderDetail);
    Task<List<Order>> GetActiveOrdersByUserIdAsync(int userId);
    Task<List<Order>> GetOrderHistoryByUserIdAsync(int userId);
    Task<IEnumerable<Order>> GetAllAsync();
}