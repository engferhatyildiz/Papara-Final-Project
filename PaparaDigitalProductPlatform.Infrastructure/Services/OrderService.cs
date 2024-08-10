using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaparaDigitalProductPlatform.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            ICouponRepository couponRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _couponRepository = couponRepository;
            _productRepository = productRepository;
        }

        public async Task<Order> CreateOrder(OrderDto orderDto)
        {
            var user = await _userRepository.GetByIdAsync(orderDto.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("Kullanıcı bulunamadı.");
            }

            var totalAmount = orderDto.OrderDetails.Sum(od => od.Price);

            // Kuponu doğrula ve indirimi uygula
            var coupon = !string.IsNullOrEmpty(orderDto.CouponCode)
                ? await _couponRepository.GetByCodeAsync(orderDto.CouponCode)
                : null;

            if (coupon != null && coupon.IsActive && coupon.ExpiryDate > DateTime.UtcNow)
            {
                totalAmount -= orderDto.CouponAmount;
                
                coupon.UsageCount += 1;
                await _couponRepository.UpdateAsync(coupon);
            }

            // Kullanıcı puanlarını uygula
            if (orderDto.PointAmount > user.Points)
            {
                throw new InvalidOperationException("Yetersiz puan.");
            }

            totalAmount -= orderDto.PointAmount;

            if (totalAmount < 0)
            {
                totalAmount = 0;
            }

            user.Points -= orderDto.PointAmount;

            // Net ödenen tutara göre kazanılacak puan miktarını hesapla
            decimal earnedPoints = 0;
            foreach (var detail in orderDto.OrderDetails)
            {
                var product = await _productRepository.GetByIdAsync(detail.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException("Ürün bulunamadı.");
                }

                // Stok kontrolü
                if (product.Stock <= 0)
                {
                    throw new InvalidOperationException($"Ürün stoğu yetersiz: {product.Name}");
                }

                // Stok miktarını azalt
                product.Stock -= 1;
                await _productRepository.UpdateAsync(product);

                var productTotal = detail.Price - orderDto.CouponAmount - orderDto.PointAmount;
                if (productTotal > 0)
                {
                    var pointsFromProduct = productTotal * (product.PointRate / 100);
                    earnedPoints += Math.Min(pointsFromProduct, product.MaxPoint);
                }
            }

            user.Points += earnedPoints;

            // Siparişi oluştur
            var order = new Order
            {
                UserId = orderDto.UserId,
                TotalAmount = totalAmount,
                CouponAmount = orderDto.CouponAmount,
                CouponCode = orderDto.CouponCode,
                PointAmount = orderDto.PointAmount,
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

            await _userRepository.UpdateAsync(user);

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
}
