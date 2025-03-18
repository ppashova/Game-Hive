using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.IServices
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string userId, string email, decimal totalPrice, List<OrderDetail> orderDetails);
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<decimal> GetTotalPriceAsync(Guid orderId);
    }

}
