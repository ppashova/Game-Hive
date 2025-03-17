using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameHive.Core.IServices;
using GameHive.Models;
using GameHive.Models.enums;

namespace GameHive.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrderAsync(string userId, string email, decimal totalPrice, List<OrderDetail> orderDetails)
        {
            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                TotalPrice = totalPrice,
                Status = OrderStatusEnums.Pending,
                OrderDetails = orderDetails
            };

            await _orderRepository.AddOrderAsync(newOrder);
            return newOrder;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orderRepository.GetUserOrdersAsync(userId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }
    }

}
