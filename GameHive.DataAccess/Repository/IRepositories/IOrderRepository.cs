using GameHive.Models;
using GameHive.Models.enums;

public interface IOrderRepository
{
    Task<Order> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(Guid orderId);
    Task<decimal> GetTotalPriceAsync(Guid orderId);
    Task AddOrderDetailsAsync(List<OrderDetail> orderDetails);
    Task<bool> UpdateOrderDetailApprovalStatusAsync(Guid orderId, int gameId, PublisherApprovalStatusEnums status);
    Task<IEnumerable<OrderDetail>> GetOrderDetailsByPublisherAsync(string publisherId, PublisherApprovalStatusEnums? status = null);
    Task<int> GetOrderDetailsCountByStatusAsync(string publisherId, PublisherApprovalStatusEnums status);
}
