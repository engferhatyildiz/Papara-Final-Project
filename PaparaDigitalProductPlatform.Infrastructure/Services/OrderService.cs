using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> CreateOrder(OrderDto orderDto)
    {
        var order = new Order
        {
            TotalAmount = orderDto.OrderDetails.Sum(od => od.Price) - orderDto.CouponAmount,
            CouponAmount = orderDto.CouponAmount,
            CouponCode = orderDto.CouponCode,
            PointAmount = orderDto.PointAmount
        };

        await _orderRepository.AddAsync(order);

        foreach (var detail in orderDto.OrderDetails)
        {
            var orderDetail = new OrderDetail
            {
                OrderId = order.Id,
                ProductId = detail.ProductId,
                Price = detail.Price
            };

            await _orderRepository.AddOrderDetailAsync(orderDetail);
        }

        return order;
    }

    public async Task<List<Order>> GetActiveOrders(int userId)
    {
        return await _orderRepository.GetActiveByUserIdAsync(userId);
    }

    public async Task<List<Order>> GetOrderHistory(int userId)
    {
        return await _orderRepository.GetHistoryByUserIdAsync(userId);
    }
    
    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _orderRepository.GetAllAsync();
    }
}