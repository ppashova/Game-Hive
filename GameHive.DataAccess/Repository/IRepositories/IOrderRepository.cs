using GameHive.Models;

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
}
