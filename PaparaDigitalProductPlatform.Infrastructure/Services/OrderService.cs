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

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository,
            ICouponRepository couponRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _couponRepository = couponRepository;
            _userRepository = userRepository;
        }
        
public async Task<ApiResponse<Order>> CreateOrder(OrderDto orderDto)
{
    // Toplam miktar ve kazanılan puanları tutacak değişkenler
    decimal totalAmount = 0;
    decimal earnedPoints = 0;
    Coupon? coupon = null;

    // Kupon kodu varsa kontrol et
    if (!string.IsNullOrEmpty(orderDto.CouponCode))
    {
        coupon = await _couponRepository.GetByCodeAsync(orderDto.CouponCode);
        if (coupon == null || !coupon.IsActive || coupon.UsageCount > 0)
        {
            return new ApiResponse<Order>
            {
                Success = false,
                Message = "Invalid or inactive coupon code",
                Data = null
            };
        }
    }

    // Kullanıcıyı getir ve kontrol et
    var user = await _userRepository.GetByIdAsync(orderDto.UserId);
    if (user == null)
    {
        return new ApiResponse<Order>
        {
            Success = false,
            Message = "User not found",
            Data = null
        };
    }

    // Kullanıcının yeterli puanı olup olmadığını kontrol et
    if (orderDto.PointAmount > user.Points)
    {
        return new ApiResponse<Order>
        {
            Success = false,
            Message = "Insufficient points",
            Data = null
        };
    }

    // Sipariş detaylarını oluştur
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

        // Stok kontrolü yap
        if (product.Stock < detailDto.Quantity)
        {
            return new ApiResponse<Order>
            {
                Success = false,
                Message = $"Insufficient stock for product: {product.Name}",
                Data = null
            };
        }

        // Stok miktarını güncelle
        product.Stock -= detailDto.Quantity;
        await _productRepository.UpdateAsync(product);

        // Sipariş detayını oluştur
        var orderDetail = new OrderDetail
        {
            ProductId = detailDto.ProductId,
            Price = product.Price,  // Ürün fiyatını direkt olarak ürünün kendisinden alıyoruz
            Quantity = detailDto.Quantity,
            Product = product
        };

        orderDetails.Add(orderDetail);
        totalAmount += product.Price * detailDto.Quantity;  // Toplam miktarı hesapla

        // Kazanılan puanları hesapla
        var productPoints = product.Price * product.PointRate * detailDto.Quantity;
        earnedPoints += productPoints > product.MaxPoint ? product.MaxPoint : productPoints;
    }

    // Kupon indirimi uygula
    if (coupon != null)
    {
        totalAmount -= coupon.Amount;
    }

    // Puan indirimi uygula
    totalAmount -= orderDto.PointAmount ?? 0;

    // Siparişi oluştur
    var order = new Order
    {
        UserId = orderDto.UserId,
        IsActive = true,
        TotalAmount = totalAmount,
        CouponAmount = coupon?.Amount ?? 0,
        CouponCode = coupon?.Code ?? string.Empty,
        PointAmount = orderDto.PointAmount ?? 0,
        EarnedPoints = earnedPoints,
        OrderDate = DateTime.UtcNow.Date,  // Sadece tarih bilgisi (saat olmadan)
        OrderDetails = orderDetails
    };

    // Siparişi veritabanına ekle
    await _orderRepository.AddAsync(order);

    // Kullanıcı puanlarını güncelle
    user.Points = user.Points - (orderDto.PointAmount ?? 0) + earnedPoints;
    await _userRepository.UpdateAsync(user);

    // Kuponun kullanım durumunu güncelle
    if (coupon != null)
    {
        coupon.UsageCount++;
        if (coupon.UsageCount >= 1)
        {
            coupon.IsActive = false;
        }
        await _couponRepository.UpdateAsync(coupon);
    }

    // Başarılı yanıt dön
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