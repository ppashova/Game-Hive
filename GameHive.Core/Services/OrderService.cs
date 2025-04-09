using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameHive.Core.IServices;
using GameHive.Models;
using GameHive.Models.enums;

namespace GameHive.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGameService _gameService;

        public OrderService(IOrderRepository orderRepository, IGameService gameService)
        {
            _orderRepository = orderRepository;
            _gameService = gameService;
        }

        public async Task<Order> CreateOrderAsync(string userId, string firstName, string lastName, string email, decimal totalPrice, List<int> gameIds, List<int> quantities)
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

            // Ensure gameIds and quantities are paired correctly
            var newOrderDetails = gameIds.Select((gameId, index) => new OrderDetail
            {
                OrderId = newOrder.Id,
                GameId = gameId,
                Quantity = quantities[index], // Assign the corresponding quantity
                ApprovalStatus = PublisherApprovalStatusEnums.Pending // Set initial approval status
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

        // New methods for publisher approval system
        public async Task<bool> UpdateOrderDetailApprovalStatusAsync(Guid orderId, int gameId, PublisherApprovalStatusEnums status, string publisherId)
        {
            // Verify the game belongs to the publisher
            var game = await _gameService.GetGameByIdAsync(gameId);
            if (game == null || game.PublisherId != publisherId)
            {
                return false;
            }

            // Update the order detail approval status
            return await _orderRepository.UpdateOrderDetailApprovalStatusAsync(orderId, gameId, status);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByPublisherAsync(string publisherId, PublisherApprovalStatusEnums? status = null)
        {
            return await _orderRepository.GetOrderDetailsByPublisherAsync(publisherId, status);
        }

        public async Task<int> GetOrderDetailsCountByStatusAsync(string publisherId, PublisherApprovalStatusEnums status)
        {
            return await _orderRepository.GetOrderDetailsCountByStatusAsync(publisherId, status);
        }
    }
}