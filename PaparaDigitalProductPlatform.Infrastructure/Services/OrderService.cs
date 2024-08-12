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
        private readonly IUserRepository _userRepository;

        public OrderService(
            IOrderRepository orderRepository, 
            IProductRepository productRepository,
            ICouponRepository couponRepository, 
            IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _couponRepository = couponRepository;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<Order>> CreateOrder(OrderDto orderDto)
        {
            var coupon = await ValidateCoupon(orderDto.CouponCode);
            if (coupon == null && !string.IsNullOrEmpty(orderDto.CouponCode))
            {
                return CreateErrorResponse<Order>("Invalid or inactive coupon code");
            }

            var user = await _userRepository.GetByIdAsync(orderDto.UserId);
            if (user == null)
            {
                return CreateErrorResponse<Order>("User not found");
            }

            if (!HasSufficientPoints(user, orderDto.PointAmount))
            {
                return CreateErrorResponse<Order>("Insufficient points");
            }

            var orderDetails = await CreateOrderDetails(orderDto);
            if (orderDetails == null)
            {
                return CreateErrorResponse<Order>("Order creation failed due to product issues");
            }

            var totalAmount = CalculateTotalAmount(orderDetails, coupon, orderDto.PointAmount);
            var earnedPoints = CalculateEarnedPoints(orderDetails);

            var order = await CreateAndSaveOrder(orderDto, orderDetails, totalAmount, earnedPoints, coupon);
            await UpdateUserPoints(user, orderDto.PointAmount, earnedPoints);
            await UpdateCouponUsage(coupon);

            return CreateSuccessResponse(order, "Order created successfully");
        }

        private async Task<Coupon?> ValidateCoupon(string couponCode)
        {
            if (string.IsNullOrEmpty(couponCode)) return null;
            var coupon = await _couponRepository.GetByCodeAsync(couponCode);
            return coupon?.IsActive == true && coupon.UsageCount == 0 ? coupon : null;
        }

        private bool HasSufficientPoints(User user, decimal? pointAmount) => pointAmount <= user.Points;

        private async Task<List<OrderDetail>?> CreateOrderDetails(OrderDto orderDto)
        {
            var orderDetails = new List<OrderDetail>();
            foreach (var detailDto in orderDto.OrderDetails)
            {
                var product = await _productRepository.GetByIdAsync(detailDto.ProductId);
                if (product == null || !HasSufficientStock(product, detailDto.Quantity))
                {
                    return null;
                }

                await UpdateProductStock(product, detailDto.Quantity);
                orderDetails.Add(CreateOrderDetail(product, detailDto.Quantity));
            }
            return orderDetails;
        }

        private static bool HasSufficientStock(Product product, int quantity) => product.Stock >= quantity;

        private async Task UpdateProductStock(Product product, int quantity)
        {
            product.Stock -= quantity;
            await _productRepository.UpdateAsync(product);
        }

        private static OrderDetail CreateOrderDetail(Product product, int quantity) =>
            new OrderDetail
            {
                ProductId = product.Id,
                Price = product.Price,
                Quantity = quantity,
                Product = product
            };

        private static decimal CalculateTotalAmount(IEnumerable<OrderDetail> orderDetails, Coupon? coupon, decimal? pointAmount)
        {
            var totalAmount = orderDetails.Sum(detail => detail.Price * detail.Quantity);
            if (coupon != null) totalAmount -= coupon.Amount;
            return totalAmount - (pointAmount ?? 0);
        }

        private static decimal CalculateEarnedPoints(IEnumerable<OrderDetail> orderDetails) =>
            orderDetails.Sum(detail =>
            {
                var points = detail.Price * detail.Product.PointRate * detail.Quantity;
                return points > detail.Product.MaxPoint ? detail.Product.MaxPoint : points;
            });

        private async Task<Order> CreateAndSaveOrder(OrderDto orderDto, List<OrderDetail> orderDetails, decimal totalAmount, decimal earnedPoints, Coupon? coupon)
        {
            var order = new Order
            {
                UserId = orderDto.UserId,
                IsActive = true,
                TotalAmount = totalAmount,
                CouponAmount = coupon?.Amount ?? 0,
                CouponCode = coupon?.Code ?? string.Empty,
                PointAmount = orderDto.PointAmount ?? 0,
                EarnedPoints = earnedPoints,
                OrderDate = DateTime.UtcNow.Date,
                OrderDetails = orderDetails
            };

            await _orderRepository.AddAsync(order);
            return order;
        }

        private async Task UpdateUserPoints(User user, decimal? pointAmount, decimal earnedPoints)
        {
            user.Points = user.Points - (pointAmount ?? 0) + earnedPoints;
            await _userRepository.UpdateAsync(user);
        }

        private async Task UpdateCouponUsage(Coupon? coupon)
        {
            if (coupon == null) return;

            coupon.UsageCount++;
            if (coupon.UsageCount >= 1) coupon.IsActive = false;
            await _couponRepository.UpdateAsync(coupon);
        }

        public async Task<ApiResponse<List<Order>>> GetActiveOrders(int userId)
        {
            var orders = await _orderRepository.GetActiveOrdersByUserIdAsync(userId);
            return orders == null || !orders.Any()
                ? CreateErrorResponse<List<Order>>("No active orders found")
                : CreateSuccessResponse(orders, "Active orders retrieved successfully");
        }

        public async Task<ApiResponse<List<Order>>> GetOrderHistory(int userId)
        {
            var orders = await _orderRepository.GetOrderHistoryByUserIdAsync(userId);
            return orders == null || !orders.Any()
                ? CreateErrorResponse<List<Order>>("No order history found")
                : CreateSuccessResponse(orders, "Order history retrieved successfully");
        }

        public async Task<ApiResponse<List<Order>>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders == null || !orders.Any()
                ? CreateErrorResponse<List<Order>>("No orders found")
                : CreateSuccessResponse(orders.ToList(), "All orders retrieved successfully");
        }

        private static ApiResponse<T> CreateErrorResponse<T>(string message) =>
            new ApiResponse<T> { Success = false, Message = message, Data = default };

        private static ApiResponse<T> CreateSuccessResponse<T>(T data, string message) =>
            new ApiResponse<T> { Success = true, Message = message, Data = data };
    }
}
