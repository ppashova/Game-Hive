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

        public async Task<Order> CreateOrderAsync(string userId,string firstName,string lastName, string email, decimal totalPrice, List<int> GameIds)
        {
            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                TotalPrice = totalPrice,
                Status = OrderStatusEnums.Pending,
                OrderDate = DateTime.Now
            };

            await _orderRepository.AddOrderAsync(newOrder);
            var newOrderDetails = GameIds.Select(gameId => new OrderDetail
            {
                OrderId = newOrder.Id,
                GameId = gameId
            }).ToList();
            await _orderRepository.AddOrderDetailsAsync(newOrderDetails);


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

        public async Task<decimal> GetTotalPriceAsync(Guid orderId)
        {
            return await _orderRepository.GetTotalPriceAsync(orderId);
        }
    }

}
