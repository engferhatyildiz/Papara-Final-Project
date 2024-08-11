using PaparaDigitalProductPlatform.Application.Dtos;
using PaparaDigitalProductPlatform.Application.Interfaces.Repositories;
using PaparaDigitalProductPlatform.Application.Responses;
using PaparaDigitalProductPlatform.Application.Services;
using PaparaDigitalProductPlatform.Domain.Entities;

namespace PaparaDigitalProductPlatform.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICouponRepository _couponRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, ICouponRepository couponRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _couponRepository = couponRepository;
        }

        public async Task<ApiResponse<Order>> CreateOrder(OrderDto orderDto)
        {
            decimal totalAmount = 0;
            var coupon = await _couponRepository.GetByCodeAsync(orderDto.CouponCode);
            orderDto.CouponAmount = coupon.Amount;

            var orderDetails = new List<OrderDetail>();
            foreach (var detailDto in orderDto.OrderDetails)
            {
                var product = await _productRepository.GetByIdAsync(detailDto.ProductId);
                if (product == null)
                {
                    return new ApiResponse<Order>
                    {
                        Success = false,
                        Message = $"Product with ID {detailDto.ProductId} not found",
                        Data = null
                    };
                }

                var orderDetail = new OrderDetail
                {
                    ProductId = detailDto.ProductId,
                    Price = detailDto.Price,
                    Product = product
                };

                orderDetails.Add(orderDetail);
                totalAmount += detailDto.Price;
            }

            totalAmount -= orderDto.CouponAmount;
            totalAmount -= orderDto.PointAmount;

            var order = new Order
            {
                UserId = orderDto.UserId,
                IsActive = true,
                TotalAmount = totalAmount,
                CouponAmount = orderDto.CouponAmount,
                CouponCode = orderDto.CouponCode,
                PointAmount = orderDto.PointAmount,
                OrderDetails = orderDetails
            };

            await _orderRepository.AddAsync(order);

            return new ApiResponse<Order>
            {
                Success = true,
                Message = "Order created successfully",
                Data = order
            };
        }

        public async Task<ApiResponse<List<Order>>> GetActiveOrders(int userId)
        {
            var orders = await _orderRepository.GetActiveOrdersByUserIdAsync(userId);
            if (orders == null || orders.Count == 0)
            {
                return new ApiResponse<List<Order>>
                {
                    Success = false,
                    Message = "No active orders found",
                    Data = null
                };
            }

            return new ApiResponse<List<Order>>
            {
                Success = true,
                Message = "Active orders retrieved successfully",
                Data = orders
            };
        }

        public async Task<ApiResponse<List<Order>>> GetOrderHistory(int userId)
        {
            var orders = await _orderRepository.GetOrderHistoryByUserIdAsync(userId);
            if (orders == null || orders.Count == 0)
            {
                return new ApiResponse<List<Order>>
                {
                    Success = false,
                    Message = "No order history found",
                    Data = null
                };
            }

            return new ApiResponse<List<Order>>
            {
                Success = true,
                Message = "Order history retrieved successfully",
                Data = orders
            };
        }

        public async Task<ApiResponse<List<Order>>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            if (orders == null || !orders.Any())
            {
                return new ApiResponse<List<Order>>
                {
                    Success = false,
                    Message = "No orders found",
                    Data = null
                };
            }

            return new ApiResponse<List<Order>>
            {
                Success = true,
                Message = "All orders retrieved successfully",
                Data = orders.ToList() // IEnumerable'den List'e dönüştürüyoruz
            };
        }
    }
}