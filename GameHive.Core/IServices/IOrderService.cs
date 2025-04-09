using GameHive.Models;
using GameHive.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Core.IServices
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string userId, string firstName, string lastName, string email, decimal totalPrice, List<int> GameIds, List<int> quantities);
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<decimal> GetTotalPriceAsync(Guid orderId);
        Task<bool> UpdateOrderDetailApprovalStatusAsync(Guid orderId, int gameId, PublisherApprovalStatusEnums status, string publisherId);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByPublisherAsync(string publisherId, PublisherApprovalStatusEnums? status = null);
        Task<int> GetOrderDetailsCountByStatusAsync(string publisherId, PublisherApprovalStatusEnums status);
    }

}
