using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task AddOrderDetailAsync(OrderDetail orderDetail);
    Task<List<Order>> GetActiveByUserIdAsync(int userId);
    Task<List<Order>> GetHistoryByUserIdAsync(int userId);
}