using Microsoft.EntityFrameworkCore;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Persistance.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task AddOrderDetailAsync(OrderDetail orderDetail)
    {
        await _context.OrderDetails.AddAsync(orderDetail);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Order>> GetActiveByUserIdAsync(int userId)
    {
        return await _context.Orders.Where(o => o.UserId == userId && o.IsActive).ToListAsync();
    }

    public async Task<List<Order>> GetHistoryByUserIdAsync(int userId)
    {
        return await _context.Orders.Where(o => o.UserId == userId && !o.IsActive).ToListAsync();
    }
}